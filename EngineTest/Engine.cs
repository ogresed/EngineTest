using System;
using System.Runtime.Serialization;

namespace EngineTest
{
    class Engine
    {
        //момент инерции
        public readonly int I;
        //M
        public int M;
        //V
        public double V;
        //функция зависимости M от V
        public readonly Pair<int, int>[] MVFunction;
        //температура перегрева
        public readonly int TP;
        //коэффициент зависимости скорости нагрева от крутящего момента
        public readonly double HM;
        //коэффициент зависимости скорости нагрева от скорости вращения коленвала
        public readonly double HV;
        //коэффициент зависимости скорости охлаждения от температуры двигателя и окружающей среды
        public readonly double C;

        //ускорение коленвала: a = M \ I
        public double a;
        //скорость нагрева двигателя рассчитывать как VH = M × HM + V2 × HV(С0/сек)
        public double VH;
        //скорость охлаждения двигателя рассчитывать как VC = C × (Tсреды - Тдвигателя) (С 0/сек)
        public double VC;
        //температура двигателя
        public double T;

        public Engine(int I,
                       Pair<int, int>[] MVFunction,
                       int TP,
                       double HM,
                       double HV,
                       double C)
        {
            if(configsIsIllegal(I, MVFunction, TP, HM, HV, C))
            {
                throw new IllegalConfigurationsForEngineException();
            }
            this.I = I;
            this.MVFunction = MVFunction;
            this.TP = TP;
            this.HM = HM;
            this.HV = HV;
            this.C = C;

            a = 1.0 * MVFunction[0].First / I;
        }

        private bool configsIsIllegal(
            int i,
            Pair<int, int>[] mVFunction,
            int tP,
            double hM,
            double hV,
            double c)
        {
            return
                Utils.ValueBetween(i, 0, 1000) &&
                Utils.ValueBetween(tP, 0, 3000) &&
                Utils.ValueBetween(hM, -1000.0, 1000.0) &&
                Utils.ValueBetween(hV, -1000.0, 1000.0) &&
                Utils.ValueBetween(c, -1000.0, 1000.0) &&
                Utils.FunInRange(mVFunction);

        }
        public void run()
        {
            V += a;
            M = GetMByV(V);
            VH = M * HM + V * V * HV;
            T += VH;
            //at the end
            a = 1.0 * M / I;
        }

        private int GetMByV(double v)
        {
            var counter = 0;
            while (v > MVFunction[counter].Second)
            {
                counter++;
            }
            var y1 = MVFunction[counter - 1].First;
            var y2 = MVFunction[counter].First;
            var x1 = MVFunction[counter - 1].Second;
            var x2 = MVFunction[counter].Second;
            var koeff = 1.0 * (v - x1) /(x2 - x1);
            return (int)((y2 - y1) * koeff + y1);
        }
    }

    [Serializable]
    internal class IllegalConfigurationsForEngineException : Exception
    {
        public IllegalConfigurationsForEngineException()
        {
        }

        public IllegalConfigurationsForEngineException(string message) : base(message)
        {
        }

        public IllegalConfigurationsForEngineException(string message, Exception innerException) : base(message, innerException)
        {
        }

        protected IllegalConfigurationsForEngineException(SerializationInfo info, StreamingContext context) : base(info, context)
        {
        }
    }
}   
