using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace UWW.CS215.Project2
{
    class Program
    {
        static void Main(string[] args)
        {
            const long p = 11;
            const long q = 13;

            long n = p * q;

            long phi = CalculatePhi(p, q);
        }

        private static long CalculatePhi(long p, long q)
        {
            return ((p-1) * (q-1));
        }

        private static long SelectCoPrime(long n)
        {
            return 0;
        }
    }
}
