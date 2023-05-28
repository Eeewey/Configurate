using System;
using System.Collections.Generic;
using System.Linq; 
using System.Text;
using System.Threading.Tasks;

namespace Configurate
{
    public class Testlerp
    {
        private const float PI = 3.145926535987f;

        public double LinInterpolate(double firstNum, double seecondNum, double by)
        {
            return firstNum * (1 - by) + seecondNum * by;
        }

        public double CosInterpolate(double firstNum, double seecondNum, double by)
        {
            var bybuff = (1 - Math.Cos(by * PI)) / 2;

            return firstNum * (1 - bybuff) + seecondNum * bybuff;
        }
    }
}
