using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace UWW.CS215.Project2
{
    internal class Program
    {
        private static void Main(string[] args)
        {
            Random rndm = new Random();

            Console.WriteLine("Calculating p and q...");
            long p = rndm.NextLong(int.MaxValue, long.MaxValue); //11;
            long q = rndm.NextLong(int.MaxValue, long.MaxValue); //13;

            Console.WriteLine("Calculating n...");
            long n = p * q;

            Console.WriteLine("Calculating phi...");
            long phi = CalculatePhi(p, q);

            Console.WriteLine("Calculating e (coprime)...");
            long e = SelectCoPrime(phi);

            Console.WriteLine("Calculating d...");
            long d = CalculateD(e, phi);

            string publicKey = "(" + e + "," + n + ")";
            string privateKey = "(" + d + "," + n + ")";

            long m = 4;
            long c = 4;

            Console.WriteLine("Encrypting...");
            long encryptedM = Encrypt(m, e, n);

            Console.WriteLine("Decrypting...");
            long decryptedM = Decrpyt(encryptedM, d, n);

            Output(p, q, n, phi, e, d, m, c, encryptedM, decryptedM);
        }

        private static void Output(long p, long q, long n, long phi, long e, long d, long m, long c, long encryptedM, long decryptedM)
        {
            Console.WriteLine("p = " + p);
            Console.WriteLine("q = " + q);
            Console.WriteLine("n = " + n);
            Console.WriteLine("phi = " + phi);
            Console.WriteLine("e = " + e);
            Console.WriteLine("d = " + d);
            Console.WriteLine("m = " + m);
            Console.WriteLine("c = " + c);
            Console.WriteLine("encrypted m = " + encryptedM);
            Console.WriteLine("decrypted m = " + decryptedM);
            Console.ReadLine();
        }

        /// <summary>
        /// Calculates the phi of n using the values of p and q.
        /// </summary>
        /// <param name="p">Value of p.</param>
        /// <param name="q">Value of q.</param>
        /// <returns>Phi of n.</returns>
        private static long CalculatePhi(long p, long q)
        {
            return ((p-1) * (q-1));
        }

        /// <summary>
        /// Selects a random value, e, such that 1 &#60; e &#60; phi and e and phi are coprime.
        /// </summary>
        /// <param name="phi">The value of the phi of n.</param>
        /// <returns>Random value.</returns>
        private static long SelectCoPrime(long phi)
        {
            Random rndm = new Random();
            long e = rndm.NextLong(1, phi);

            while (GetGcdByModulus(e, phi) != 1)
            {
                e = rndm.NextLong(1, phi);
            }

            return e;
        }

        /// <summary>
        /// Calculates the greatest common denominator of the provided two values. Returns 1 if coprime.
        /// </summary>
        /// <param name="value1">The first value.</param>
        /// <param name="value2">The second value.</param>
        /// <returns>The value of the two value's greatest common denominator.</returns>
        private static long GetGcdByModulus(long value1, long value2)
        {
            while (value1 != 0 && value2 != 0)
            {
                if (value1 > value2)
                {
                    value1 %= value2;
                }
                else
                {
                    value2 %= value1;
                }
            }
            return Math.Max(value1, value2);
        }

        private static long CalculateD(long e, long phi)
        {
            Random rndm = new Random();
            long d = rndm.NextLong(int.MaxValue, long.MaxValue);

            while (d*e%phi != 1)
            {
                d = rndm.NextLong(int.MaxValue, long.MaxValue);
            }

            return d;
        }

        private static long Encrypt(long m, long e, long n)
        {
            return m ^ e%n;
        }

        private static long Decrpyt(long c, long d, long n)
        {
            return c ^ d%n;
        }
    }
}
