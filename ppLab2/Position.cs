using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;

namespace ppLab2
{
    public struct Position
    {
        public int x;
        public int y;
        
        public Position(int x, int y)
        {
            this.x = x;
            this.y = y;
        }

        public static int distanceBetween(Position A, Position B)
        {
            double xl = A.x - B.x;
            double yl = A.y - B.y;

            double res = Math.Sqrt(xl * xl + yl * yl);
            return (int)res;
        }
    }
}
