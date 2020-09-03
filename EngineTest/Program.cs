using System;
using System.IO;
using System.Collections.Generic;

namespace EngineTest
{
    class Program
    {
        static readonly string ITeg = "I";
        static readonly string MVTeg = "MV";
        static readonly string TPTeg = "TP";
        static readonly string HMTeg = "HM";
        static readonly string HVTeg = "HV";
        static readonly string CTeg = "C";

        static Dictionary<string, string> configs;
        static void Main(string[] args)
        {
            Engine engine;
            try
            {
                var configLines = File.ReadAllLines("engineConfig.txt");
                configs = new Dictionary<string, string>();
                foreach (var conf in configLines)
                {
                    var values = conf.Split(" ");
                    configs.Add(values[0], values[1]);
                }
                // create engine
                engine = CreateEngine(configs);
            }
            catch (Exception ex)
            {
                if (ex is IOException)
                {
                    Console.WriteLine("Cannot read file");
                }
                else if (ex is IndexOutOfRangeException || ex is FormatException)
                {
                    Console.WriteLine("Wrong configs format");
                }
                else if (ex is IllegalConfigurationsForEngineException)
                {
                    Console.WriteLine("Configuration for engine is illegal");
                }
                else
                {
                    Console.WriteLine("Expluatation error");
                }
                return;
            }
            try 
            {
                var simulation = new Simulation();
                //get temperature 
                var stringTemp = Console.ReadLine();
                var temp = Int32.Parse(stringTemp);
                if(!Utils.ValueBetween(temp, -271, 200))
                {
                    throw new FormatException();
                }
                //simulation
                var time = simulation.OverHeatTest(engine, temp);
                Console.WriteLine(time);
            }
            catch (Exception ex)
            {
                if (ex is IndexOutOfRangeException)
                {
                    Console.WriteLine("Incorrect configurations");
                }
                else if (ex is FormatException)
                {
                    Console.WriteLine("Wrong temperature of environment");
                }
                else
                {
                    Console.WriteLine("Expluatation error");
                }
            }
        }

        private static Engine CreateEngine(Dictionary<string, string> configs) 
        {
            var I = Int32.Parse(GetStringValue(ITeg));
            var MV = GetStringValue(MVTeg);
            var dots = CreatePairs(MV);
            var TP = Int32.Parse(GetStringValue(TPTeg));
            var HM = Double.Parse(GetStringValue(HMTeg));
            var HV = Double.Parse(GetStringValue(HVTeg));
            var C = Double.Parse(GetStringValue(CTeg));
            return new Engine(I, dots, TP, HM, HV, C);
        }

        private static Pair<int, int>[] CreatePairs(string mV)
        {
            var MVData = mV.Split("|");
            var numberOfDots = Int32.Parse(MVData[0]);
            var stringDots = MVData[1].Split(";");
            var dots = new Pair<int, int>[numberOfDots];
            for (int index = 0; index < numberOfDots; index++) 
            {
                var stringDote = stringDots[index];
                var values = stringDote.Split("-");
                var M = Int32.Parse(values[0]);
                var V = Int32.Parse(values[1]);
                dots[index] = new Pair<int, int>
                {
                    First = M,
                    Second = V
                };
            }
            return dots;
        }

        static string GetStringValue(string key)
        {
            var value = "";
            configs.TryGetValue(key, out value);
            if ("".Equals(value)) 
            {
                throw new FormatException();
            }
            return value;
        }
    }
}
