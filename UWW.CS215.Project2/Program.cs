using System;
using System.Numerics;
using System.Security.Cryptography;

namespace UWW.CS215.Project2
{
    internal class Program
    {
        private static readonly BigInteger MaxValue = 10000;

        private static void Main(string[] args)
        {
            Console.WriteLine("Max Value: " + MaxValue);
            Console.WriteLine("Calculating p and q...");
            BigInteger p = RandomIntegerBelow(MaxValue);
            while (!IsProbablyPrime(p, 20))
            {
                p = RandomIntegerBelow(MaxValue);
            }
            Console.WriteLine("p = " + p);
            BigInteger q = RandomIntegerBelow(MaxValue);
            while (!IsProbablyPrime(q, 20))
            {
                q = RandomIntegerBelow(MaxValue);
            }
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
            Console.WriteLine("Public Key: " + publicKey);
            string privateKey = "(" + d + "," + n + ")";
            Console.WriteLine("Private Key: " + privateKey);

            BigInteger m = 4;
            BigInteger c = 4;
            Console.WriteLine("m = " + m);
            Console.WriteLine("c = " + c);

            Console.WriteLine("Encrypting...");
            BigInteger encryptedM = Encrypt(m, e, n);
            Console.WriteLine("Encrypted m = " + encryptedM);

            Console.WriteLine("Decrypting...");
            BigInteger decryptedM = Decrpyt(encryptedM, d, n);
            Console.WriteLine("Decrypted m = " + decryptedM);

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
            Random random = new Random();
            BigInteger d = RandomIntegerBelow(random, MaxValue * 10);

            while ((d * e) % phi != 1)
            {
                d = RandomIntegerBelow(random, MaxValue * 10);
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

        public static bool IsProbablyPrime(BigInteger n, int k)
        {
            bool result = false;
            if (n < 2)
                return false;
            if (n == 2)
                return true;
            // return false if n is even -> divisible by 2
            if (n % 2 == 0)
                return false;
            //writing n-1 as 2^s.d
            BigInteger d = n - 1;
            BigInteger s = 0;
            while (d % 2 == 0)
            {
                d >>= 1;
                s = s + 1;
            }
            for (int i = 0; i < k; i++)
            {
                BigInteger a;
                do
                {
                    a = RandomIntegerBelow(n - 2);
                }
                while (a < 2 || a >= n - 2);

                if (BigInteger.ModPow(a, d, n) == 1) return true;
                for (int j = 0; j < s - 1; j++)
                {
                    if (BigInteger.ModPow(a, 2 * j * d, n) == n - 1)
                        return true;
                }
                result = false;
            }
            return result;
        }

        public static BigInteger RandomIntegerBelow(int n)
        {
            var rng = new RNGCryptoServiceProvider();
            byte[] bytes = new byte[n / 8];

            rng.GetBytes(bytes);

            var msb = bytes[n / 8 - 1];
            var mask = 0;
            while (mask < msb)
                mask = (mask << 1) + 1;

            bytes[n - 1] &= Convert.ToByte(mask);
            BigInteger p = new BigInteger(bytes);
            return p;
        }

        public static BigInteger RandomIntegerBelow(BigInteger bound)
        {
            var rng = new RNGCryptoServiceProvider();
            //Get a byte buffer capable of holding any value below the bound
            var buffer = (bound << 16).ToByteArray(); // << 16 adds two bytes, which decrease the chance of a retry later on

            //Compute where the last partial fragment starts, in order to retry if we end up in it
            var generatedValueBound = BigInteger.One << (buffer.Length * 8 - 1); //-1 accounts for the sign bit
            var validityBound = generatedValueBound - generatedValueBound % bound;

            while (true)
            {
                //generate a uniformly random value in [0, 2^(buffer.Length * 8 - 1))
                rng.GetBytes(buffer);
                buffer[buffer.Length - 1] &= 0x7F; //force sign bit to positive
                var r = new BigInteger(buffer);

                //return unless in the partial fragment
                if (r >= validityBound) continue;
                return r % bound;
            }
        }
    }
}