using System;

namespace dnWalker.Z3
{
    /// <summary>
    /// https://rosettacode.org/wiki/Convert_decimal_number_to_rational#C.23
    /// </summary>
    public readonly struct RationalNumber : IEquatable<RationalNumber>
    {
        public long Numerator { get; }
        public long Denominator { get; }

        public RationalNumber(long numerator, long denominator)
        {
            Numerator = numerator;
            Denominator = denominator;
        }

        public override bool Equals(object obj)
        {
            return obj is RationalNumber number && Equals(number);
        }

        public bool Equals(RationalNumber other)
        {
            return Numerator == other.Numerator &&
                   Denominator == other.Denominator;
        }

        public override int GetHashCode()
        {
            return HashCode.Combine(Numerator, Denominator);
        }

        public override string ToString()
        {
            return string.Format("{0} / {1}", Numerator, Denominator);
        }

        public static bool operator ==(RationalNumber left, RationalNumber right)
        {
            return left.Equals(right);
        }

        public static bool operator !=(RationalNumber left, RationalNumber right)
        {
            return !(left == right);
        }

        public static RationalNumber FromDouble(double real, long maxDenominator = 4096)
        {
            long denominator;
            long numerator;

            /* Translated from the C version. */
            /*  a: continued fraction coefficients. */
            long a;
            Span<long> h = stackalloc long[3] { 0, 1, 0 };
            Span<long> k = stackalloc long[3] { 1, 0, 0 };
            long x, d, n = 1;
            int i;
            bool neg = false;

            if (maxDenominator <= 1)
            {
                denominator = 1;
                numerator = (long)real;
                return new RationalNumber(numerator, denominator);
            }

            if (real < 0) { neg = true; real = -real; }

            while (real != Math.Floor(real)) { n <<= 1; real *= 2; }
            d = (long)real;

            /* continued fraction and check denominator each step */
            for (i = 0; i < 64; i++)
            {
                a = (n != 0) ? d / n : 0;
                if ((i != 0) && (a == 0)) break;

                x = d; d = n; n = x % n;

                x = a;
                if (k[1] * a + k[0] >= maxDenominator)
                {
                    x = (maxDenominator - k[0]) / k[1];
                    if (x * 2 >= a || k[1] >= maxDenominator)
                        i = 65;
                    else
                        break;
                }

                h[2] = x * h[1] + h[0]; h[0] = h[1]; h[1] = h[2];
                k[2] = x * k[1] + k[0]; k[0] = k[1]; k[1] = k[2];
            }
            denominator = k[1];
            numerator = neg ? -h[1] : h[1];

            return new RationalNumber(numerator, denominator);
        }
    }
}
