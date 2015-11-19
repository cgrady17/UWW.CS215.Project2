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
            Random rndm = new Random();

            long p = rndm.NextLong(int.MaxValue, long.MaxValue); //11;
            long q = rndm.NextLong(int.MaxValue, long.MaxValue); //13;

            long n = p * q;

            long phi = CalculatePhi(p, q);

            Output(p, q, n, phi);
        }

        private static void Output(long p, long q, long n, long phi)
        {
            Console.WriteLine("p = " + p);
            Console.WriteLine("q = " + q);
            Console.WriteLine("n = " + n);
            Console.WriteLine("phi = " + phi);
            Console.ReadLine();
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
