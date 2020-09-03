using System;
using System.Collections.Generic;
using System.Text;

namespace EngineTest
{
    class Utils
    {
        public static bool ValueBetween(double c, double v1, double v2)
        {
            return c > v1 && c < v2;
        }

        public static bool ValueBetween(int c, int v1, int v2)
        {
            return c > v1 && c < v2;
        }

        public static bool FunInRange(Pair<int, int>[] mVFunction)
        {
            foreach (var pair in mVFunction)
            {
                var m = pair.First;
                var v = pair.Second;
                if (!(ValueBetween(m, 0, 1000) && ValueBetween(v, 0, 1000)))
                {
                    return false;
                }
            }
            return true;
        }
    }
}
