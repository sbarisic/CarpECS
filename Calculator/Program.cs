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

			float AmbientPressure = 99000;
			float RPM = 7000;
			float Boost = 2.0f;
			float AmbientTemp = 28;
			Calculate(Displacement, AmbientTemp, Boost, RPM, AmbientPressure);

			Console.WriteLine("Done!");
			Console.ReadLine();
		}


		static void Calculate(float Displacement, float AmbientAirTemp, float Boost, float RPM, float UncompPressure) {
			float DesiredBoost = Boost;

			Console.WriteLine("Displacement: {0} L", M3ToLiter(Displacement));
			Console.WriteLine("Theoretical boost: {0:0.00} bar", Boost);
			Console.WriteLine("Ambient air temperature: {0} °C", AmbientAirTemp);

			float IntercoolerEfficiency = 0.9f;
			Console.WriteLine("Intercooler efficiency: {0} %", IntercoolerEfficiency * 100);

			float UncompTempCels = 0;
			float FinalPressure = 0;
			float PressureAfterIC = 0;
			float FinalTemp = 0;
			float FinalTempAfterIC = 0;

			float HPA = 0;
			float HPB = 0;
			float HPC = 0;

			float A = 0;
			float B = 0;
			float C = 0;

			float LastBoostDiff = Boost;

			for (int i = 0; i < 50; i++) {
				// Nitrogen + Oxygen only
				float Gamma = 7.0f / 5.0f;

				UncompTempCels = AmbientAirTemp;
				float UncompTemp = CelsiusToKelvin(UncompTempCels);

				float CompPressure = UncompPressure + BarToPascal(Boost);
				float PressureRatio = CompPressure / UncompPressure;
				float UncompDisplacement = Displacement * PressureRatio;

				// Adiabatic constant
				float P1V1Gamma = (float)(UncompPressure * Math.Pow(UncompDisplacement, Gamma));

				FinalPressure = (float)(UncompPressure * Math.Pow(PressureRatio, Gamma));
				//float FinalPressureBar = PascalToBar(FinalPressure);

				float PVT = (UncompPressure * UncompDisplacement) / UncompTemp;
				FinalTemp = (FinalPressure * Displacement) / PVT;
				float FinalTempCelsius = KelvinToCelsius(FinalTemp);



				FinalTempAfterIC = (IntercoolerEfficiency * (FinalTemp - UncompTemp) - FinalTemp) * -1.0f;
				// float FinalTempAfterIC_Celsius = KelvinToCelsius(FinalTempAfterIC);

				PressureAfterIC = (PVT * FinalTempAfterIC) / Displacement;
				//float FinalBoostPressure = PascalToBar(PressureAfterIC - UncompPressure);

				A = AirWeight(UncompPressure, UncompTemp, Displacement);
				B = AirWeight(FinalPressure, FinalTemp, Displacement);
				C = AirWeight(PressureAfterIC, FinalTempAfterIC, Displacement);

				HPA = A * (RPM / 60 / 2) / 0.8f;
				HPB = B * (RPM / 60 / 2) / 0.8f;
				HPC = C * (RPM / 60 / 2) / 0.8f;

				float BoostDiff = PascalToBar(PressureAfterIC - UncompPressure) - DesiredBoost;


				if (BoostDiff < 0.001f) {
					break;
				}

				if (BoostDiff < LastBoostDiff) {
					LastBoostDiff = BoostDiff;
					Boost = Boost - BoostDiff * 0.1f;
				} else {
					break;
				}
			}



			Console.WriteLine();
			Console.WriteLine("Pressure before turbo:   {0:0.00} bar @ {1:000} °C - {2} g; {3:000} hp @ {4} RPM", PascalToBar(UncompPressure - UncompPressure), UncompTempCels, A, HPA, RPM);
			Console.WriteLine("Pressure after turbo:    {0:0.00} bar @ {1:000} °C - {2} g; {3:000} hp @ {4} RPM", PascalToBar(FinalPressure - UncompPressure), KelvinToCelsius(FinalTemp), B, HPB, RPM);
			Console.WriteLine("Pressure after IC:       {0:0.00} bar @ {1:000} °C - {2} g; {3:000} hp @ {4} RPM", PascalToBar(PressureAfterIC - UncompPressure), KelvinToCelsius(FinalTempAfterIC), C, HPC, RPM);

			Console.WriteLine();
		}

		static float AirWeight(float PressurePascal, float TemperatureKelvin, float VolumeM3) {
			// float Moles = (PascalToBar(PressurePascal) * VolumeLiters) / (0.0821f * TemperatureKelvin);

			// Specific gas constant for dry air
			float RSpecific = 287.058f;

			// Kg/M3
			float AirDensity = PressurePascal / (RSpecific * TemperatureKelvin);
			float AirWeight = KgToG(AirDensity * VolumeM3);

			return AirWeight;
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
