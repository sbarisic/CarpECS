using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.IO;

namespace InjectorCalculator {
    struct CSVPoint {
        public int RPM;
        public float CylAirmass;
        public float MAT;
        public float AFR;
        public float EQ;
        public float InjMS;
        public float AccelPedal;

        public float LTFT;
        public float STFT;

        public CSVPoint(string RPM, string CylAirmass, string MAT, string AFR, string InjMS, string AccelPedal, string EQ, string LTFT, string STFT) {
            this.RPM = (int)float.Parse(RPM);
            this.CylAirmass = float.Parse(CylAirmass);
            this.MAT = float.Parse(MAT);
            this.AFR = float.Parse(AFR);
            this.InjMS = float.Parse(InjMS);
            this.AccelPedal = float.Parse(AccelPedal);
            this.EQ = float.Parse(EQ);
            this.LTFT = float.Parse(LTFT);
            this.STFT = float.Parse(STFT);
        }

        public override string ToString() {
            return string.Format("{0} RPM", RPM);
        }
    }

    class HPTCSV {
        string[] Lines;

        public HPTCSV() {
        }

        public void Load(string FileName) {
            Lines = File.ReadAllLines(FileName);
        }


        public static CSVPoint[] ParseEntriesHPT(string CSVFile) {
            string[] Lines = File.ReadAllText(CSVFile).Trim().Split(new[] { '\n' }).Select(L => L.Trim()).ToArray();

            int ChannelInfoIdx = FindIndex(Lines, "[Channel Information]");
            int ChannelDataIdx = FindIndex(Lines, "[Channel Data]");

            string[] ChannelInfo = Lines.Skip(ChannelInfoIdx + 1).Take(ChannelDataIdx - ChannelInfoIdx - 1).ToArray();
            string[] InfoNames = ChannelInfo[1].Split(new[] { ',' }).ToArray();

            int RPMIdx = -1;
            int Airmass = -1;
            int MAT = -1;
            int InjMS = -1;
            int AFR = -1;
            int EQ = -1;
            int AccelPedal = -1;
            int LTFT = -1;
            int STFT = -1;

            for (int i = 0; i < InfoNames.Length; i++) {
                if (InfoNames[i].Contains("Engine RPM"))
                    RPMIdx = i;

                if (InfoNames[i].Contains("Cylinder Airmass"))
                    Airmass = i;

                if (InfoNames[i].Contains("Manifold Air Temp") && MAT == -1)
                    MAT = i;

                if (InfoNames[i].Contains("Injector Pulse Width"))
                    InjMS = i;

                if (InfoNames[i].Contains("Air-Fuel Ratio Commanded"))
                    AFR = i;

                if (InfoNames[i].Contains("Accelerator Pedal Position"))
                    AccelPedal = i;

                if (InfoNames[i].Contains("Equivalence Ratio Commanded"))
                    EQ = i;

                if (InfoNames[i].Contains("Long Term Fuel Trim"))
                    LTFT = i;

                if (InfoNames[i].Contains("Short Term Fuel Trim"))
                    STFT = i;
            }

            List<CSVPoint> DynoPointsList = new List<CSVPoint>();

            string[][] DataLines = Lines.Skip(ChannelDataIdx + 1).Select(L => L.Split(',')).ToArray();
            for (int i = 0; i < DataLines.Length; i++) {
                CSVPoint Point = new CSVPoint(DataLines[i][RPMIdx], DataLines[i][Airmass], DataLines[i][MAT], DataLines[i][AFR], DataLines[i][InjMS], DataLines[i][AccelPedal], DataLines[i][EQ], DataLines[i][LTFT], DataLines[i][STFT]);
                DynoPointsList.Add(Point);
            }

            return DynoPointsList.OrderBy(P => P.RPM).ToArray();
        }

        static int FindIndex(string[] Lines, string Src) {
            for (int i = 0; i < Lines.Length; i++) {
                if (Lines[i] == Src)
                    return i;
            }

            return 0;
        }
    }
}
