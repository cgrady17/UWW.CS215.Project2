using System;
using System.Numerics;

namespace UWW.CS215.Project2
{
    internal class Program
    {
        private static readonly BigInteger MaxValue = 300;

        private static void Main(string[] args)
        {
            Random rndm = new Random();
            Console.WriteLine("Max Value: " + MaxValue);
            Console.WriteLine("Calculating p and q...");
            BigInteger p = 11; //RandomIntegerBelow(rndm, MaxValue);
            Console.WriteLine("p = " + p);
            BigInteger q = 13; //RandomIntegerBelow(rndm, MaxValue);
            Console.WriteLine("q = " + q);

            Console.WriteLine("Calculating n...");
            BigInteger n = p * q;
            Console.WriteLine("n = " + n);

            Console.WriteLine("Calculating phi...");
            BigInteger phi = CalculatePhi(p, q);
            Console.WriteLine("phi = " + phi);

            Console.WriteLine("Calculating e (coprime)...");
            BigInteger e = SelectCoPrime(phi);
            Console.WriteLine("e = " + e);

            Console.WriteLine("Calculating d...");
            BigInteger d = CalculateD(e, phi);
            Console.WriteLine("d = " + d);

            string publicKey = "(" + e + "," + n + ")";
            string privateKey = "(" + d + "," + n + ")";

            BigInteger m = 4;
            BigInteger c = 4;

            Console.WriteLine("Encrypting...");
            BigInteger encryptedM = Encrypt(m, e, n);

            Console.WriteLine("Decrypting...");
            BigInteger decryptedM = Decrpyt(encryptedM, d, n);

            Output(p, q, n, phi, e, d, m, c, encryptedM, decryptedM);
        }

        private static void Output(BigInteger p, BigInteger q, BigInteger n, BigInteger phi, BigInteger e, BigInteger d, BigInteger m, BigInteger c, BigInteger encryptedM, BigInteger decryptedM)
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
            Console.WriteLine("Does decrypted m (" + decryptedM + ") match original m (" + m + ")? " + (m == decryptedM ? "Yes" : "No") + "!");
            Console.ReadLine();
        }

        /// <summary>
        /// Calculates the phi of n using the values of p and q.
        /// </summary>
        /// <param name="p">Value of p.</param>
        /// <param name="q">Value of q.</param>
        /// <returns>Phi of n.</returns>
        private static BigInteger CalculatePhi(BigInteger p, BigInteger q)
        {
            return ((p - 1) * (q - 1));
        }

        /// <summary>
        /// Selects a random value, e, such that 1 &#60; e &#60; phi and e and phi are coprime.
        /// </summary>
        /// <param name="phi">The value of the phi of n.</param>
        /// <returns>Random value.</returns>
        private static BigInteger SelectCoPrime(BigInteger phi)
        {
            Random rndm = new Random();
            BigInteger e = RandomIntegerBelow(rndm, phi);

            while (GetGcdByModulus(e, phi) != 1)
            {
                e = RandomIntegerBelow(rndm, phi);
            }

            return e;
        }

        /// <summary>
        /// Calculates the greatest common denominator of the provided two values. Returns 1 if coprime.
        /// </summary>
        /// <param name="value1">The first value.</param>
        /// <param name="value2">The second value.</param>
        /// <returns>The value of the two value's greatest common denominator.</returns>
        private static BigInteger GetGcdByModulus(BigInteger value1, BigInteger value2)
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
            return BigInteger.Max(value1, value2);
        }

        /// <summary>
        /// Calculates a value d, such that (d * e) % phi(n) = 1.
        /// </summary>
        /// <param name="e"></param>
        /// <param name="phi"></param>
        /// <returns></returns>
        private static BigInteger CalculateD(BigInteger e, BigInteger phi)
        {
            Random rndm = new Random();
            BigInteger d = RandomIntegerBelow(rndm, MaxValue);

            while ((d * e) % phi != 1)
            {
                d = RandomIntegerBelow(rndm, MaxValue);
            }
            return d;
        }

        private static BigInteger Encrypt(BigInteger m, BigInteger e, BigInteger n)
        {
            return BigInteger.ModPow(m, e, n);
        }

        private static BigInteger Decrpyt(BigInteger c, BigInteger d, BigInteger n)
        {
            return BigInteger.ModPow(c, d, n);
        }

        private static BigInteger RandomIntegerBelow(Random random, BigInteger n)
        {
            byte[] bytes = n.ToByteArray();
            BigInteger r;

            do
            {
                random.NextBytes(bytes);
                bytes[bytes.Length - 1] &= 0x7F; //force sign bit to positive
                r = new BigInteger(bytes);
            } while (r >= n);

            return r;
        }
    }
}