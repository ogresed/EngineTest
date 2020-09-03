using System;
using System.Collections.Generic;
using System.Text;

namespace EngineTest
{
    class Simulation
    {    
        public int OverHeatTest(Engine engine, int temperatureOfNature)
        {
            var timeCounter = 0;
            engine.T = temperatureOfNature;
            while (engine.T < engine.TP)
            {
                engine.run();
                var VC = engine.C * (temperatureOfNature - engine.T);
                engine.T -= VC;
                timeCounter++;
            }
            return timeCounter;
        }
    }
}
