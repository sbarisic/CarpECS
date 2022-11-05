using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Calculator {
	class Program {
		static void Main(string[] args) {
			float Displacement = LiterToM3(1.364f);
			//float Displacement = LiterToM3(2.0f);

			float AmbientPressure = 95000;
			float RPM = 6800;
			float DesiredManifoldPressure = 245000;
			float AmbientTemp = 22;
			Calculate(Displacement, AmbientTemp, DesiredManifoldPressure, RPM, AmbientPressure);

			/*float[] Chev = "7	7	7	7	7	7	7	6.375	6.25	6.25	6.25	6	5.75	4.375	3.75	3.625	3.5	3.25	3.25	3.25	3.25	3.375	3.5	3.5	3.5	3.625	3.625	3.75	3.625	3.5	3.5	3.375	3.25".Split(new[] { '\t' }).Select(T => float.Parse(T)).ToArray();
			float[] Opel = new float[] { 13, 13, 13, 13, 13.5f, 14, 14, 14, 14.5f, 14.5f, 15, 14.5f, 14, 14, 13.5f, 13 };

			float[] ChevToOp = Convert(Chev);*/

			Console.WriteLine("Done!");
			Console.ReadLine();
		}

		static float[] Convert(float[] In) {
			float[] Ret = new float[In.Length];

			for (int i = 0; i < Ret.Length; i++) {
				float IN = In[i];

				byte B = (byte)(IN * 8);
				float F = B + 128;

				float OUT = (F / 2.0f) - 64.0f;
				Ret[i] = OUT;

			}

			string Tabs = string.Join("\t", Ret);

			return Ret;
		}

		static byte ChevToByte(float F) {
			return 0;
		}

		static byte OpToByte(float F) {
			return 0;
		}


		static void Calculate(float Displacement, float AmbientTempCelsius, float DesiredManifoldPressure, float RPM, float AmbientPressure) {
			int NumCyl = 4;
			float DesiredBoost = DesiredManifoldPressure - AmbientPressure;

			Console.WriteLine("Displacement: {0} L", M3ToLiter(Displacement));
			Console.WriteLine("Ambient Pressure: {0} kPa", AmbientPressure / 1000);
			Console.WriteLine("Desired Manifold Pressure: {0} kPa", DesiredManifoldPressure / 1000);
			Console.WriteLine("Required Boost: {0:0.00} bar", PascalToBar(DesiredBoost));
			Console.WriteLine("Ambient air temperature: {0} °C", AmbientTempCelsius);

			float IntercoolerEfficiency = 0.76f;
			Console.WriteLine("Intercooler efficiency: {0} %", IntercoolerEfficiency * 100);

			float CompressorEfficiency = 0.6f;
			Console.WriteLine("Compressor efficiency: {0} %", CompressorEfficiency * 100);

			float IntercoolerPressureDropFactor = 0.10f;


			float FinalTemp = 0;
			float FinalPressure = 0;
			float PressureAfterIC = 0;
			float FinalTempAfterIC = 0;

			float HPA = 0;
			float HPB = 0;
			float HPC = 0;

			float A = 0;
			float B = 0;
			float C = 0;

			float AGCyl = 0;
			float BGCyl = 0;
			float CGCyl = 0;

			float PressureRatioCorrection = 0.0f;

			// Nitrogen + Oxygen only
			float Gamma = 7.0f / 5.0f;

			float AmbientTemp = CelsiusToKelvin(AmbientTempCelsius);

			//float CompPressure = AmbientPressure + BarToPascal(Boost);
			float CompPressure = DesiredManifoldPressure;

			// Ideal compressor pressure ratio
			float IdealPressureRatio = CompPressure / AmbientPressure;

			// Pressure drop across the intercooler in pascal
			float IntercoolerPressureDropPa = AmbientPressure * IdealPressureRatio * IntercoolerPressureDropFactor;

			// Correction factor for compressor pressure ratio based on intercooler pressure drop
			PressureRatioCorrection = ((CompPressure + IntercoolerPressureDropPa) / AmbientPressure) - IdealPressureRatio;

			// Real pressure ratio
			float PressureRatio = IdealPressureRatio + PressureRatioCorrection;
			float UncompDisplacement = Displacement * PressureRatio;

			// Ideal and final temperature with compressor efficiency
			float IdealFinalTemp = (float)(AmbientTemp * Math.Pow(PressureRatio, (Gamma - 1) / Gamma));
			float IdealFinalTempC = KelvinToCelsius(IdealFinalTemp);

			FinalTemp = AmbientTemp + (IdealFinalTemp - AmbientTemp) / CompressorEfficiency;
			float FinalTempC = KelvinToCelsius(FinalTemp);

			FinalPressure = AmbientPressure * PressureRatio;
			float FinalPressureBar = PascalToBar(FinalPressure);

			// Final temperature after intercooler based on intercooler efficiency
			FinalTempAfterIC = FinalTemp - IntercoolerEfficiency * (FinalTemp - AmbientTemp);
			float FinalTempAfterIC_Celsius = KelvinToCelsius(FinalTempAfterIC);

			// Final pressure after intercooler based on intercooler pressure drop
			PressureAfterIC = FinalPressure - IntercoolerPressureDropPa;

			A = AirWeight(AmbientPressure, AmbientTemp, Displacement);
			B = AirWeight(FinalPressure, FinalTemp, Displacement);
			C = AirWeight(PressureAfterIC, FinalTempAfterIC, Displacement);

			AGCyl = (float)Math.Round(A / NumCyl, 3);
			BGCyl = (float)Math.Round(B / NumCyl, 3);
			CGCyl = (float)Math.Round(C / NumCyl, 3);

			const float AirmassToHPConstant = 0.8f; // 0.7457f;
			HPA = A * (RPM / 60 / 2) / AirmassToHPConstant;
			HPB = B * (RPM / 60 / 2) / AirmassToHPConstant;
			HPC = C * (RPM / 60 / 2) / AirmassToHPConstant;


			Console.WriteLine();

			PrintStat("Pressure before turbo", AmbientPressure, AmbientPressure, AmbientTempCelsius, AGCyl, HPA, RPM);
			PrintStat("Pressure after turbo", FinalPressure, AmbientPressure, KelvinToCelsius(FinalTemp), BGCyl, HPB, RPM);
			PrintStat("Pressure after IC", PressureAfterIC, AmbientPressure, KelvinToCelsius(FinalTempAfterIC), CGCyl, HPC, RPM);

			Console.WriteLine();
		}

		static void PrintStat(string Name, float PressurePascal, float AmbientPressurePascal, float TempCelsius, float GPerCyl, float HP, float RPM) {
			const string Separator = " | ";
			Name = Name + ":";

			Console.Write("{0}{1}", Name, new string(' ', 25 - Name.Length));

			//Console.Write("{0:0.00} bar" + Separator, PressureBar);
			Console.Write("{0:000} kPa" + Separator, PressurePascal / 1000);
			Console.Write("{0:0.00} Ratio" + Separator, PressurePascal / AmbientPressurePascal);
			Console.Write("{0:000} °C" + Separator, TempCelsius);
			Console.Write("{0} g/cyl" + Separator, GPerCyl);
			Console.Write("{0:000} hp" + Separator, HP);
			Console.Write("{0:000} Nm" + Separator, HPToNm(HP, RPM));
			Console.Write("{0} RPM" + Separator, RPM);
			Console.Write("{0} g/s" + Separator, AirMass(GPerCyl, RPM));

			Console.WriteLine();
		}

		static float AirWeight(float PressurePascal, float TemperatureKelvin, float VolumeM3) {
			// float Moles = (PascalToBar(PressurePascal) * VolumeLiters) / (0.0821f * TemperatureKelvin);

			// Specific gas constant for dry air
			float RSpecific = 287.058f;

			// Kg/M3
			float AirDensity = PressurePascal / (RSpecific * TemperatureKelvin);
			float AirWeight = KgToG(AirDensity * VolumeM3);

			// Per cylinder
			return AirWeight;
		}

		static float HPToNm(float Hp, float RPM) {
			return 9548.8f * (Hp * 0.735499f) / RPM;
		}

		static float AirMass(float AirmassPerCyl, float RPM) {
			return (AirmassPerCyl * 2) * RPM / 60;
		}

		static float BarToPascal(float Bar) {
			return Bar * 100000;
		}

		static float KgM3ToKgL(float KgM3) {
			return KgM3 * 0.001f;
		}

		static float KgToG(float Kg) {
			return Kg * 1000;
		}

		static float PascalToBar(float Pascal) {
			return Pascal / 100000.0f;
		}

		static float LiterToM3(float L) {
			return L * 0.001f;
		}

		static float M3ToLiter(float L) {
			return L / 0.001f;
		}

		static float CelsiusToKelvin(float C) {
			return C + 273.15f;
		}

		static float KelvinToCelsius(float K) {
			return K - 273.15f;
		}
	}
}
