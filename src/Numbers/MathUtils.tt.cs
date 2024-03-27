using System;




// ReSharper disable RedundantCast

namespace RI.Utilities.Numbers
{
    /// <summary>
    ///     Provides various mathematical utility methods.
    /// </summary>
    /// <threadsafety static="false" instance="false" />
    public static class MathUtils
    {
        #region Static Methods

        /// <summary>
        ///     Finds the greatest common divisor (GCD) of two values.
        /// </summary>
        /// <param name="x"> The first value. </param>
        /// <param name="y"> The second value. </param>
        /// <returns>
        ///     The clamped value.
        /// </returns>
        [CLSCompliant(false),]
        public static sbyte Gcd (sbyte x, sbyte y)
        {
            while (!(y == 0))
            {
                sbyte temp = y;
                y = (sbyte)(x % y);
                x = temp;
            }

            return x;
        }

        /// <summary>
        ///     Finds the greatest common divisor (GCD) of two values.
        /// </summary>
        /// <param name="x"> The first value. </param>
        /// <param name="y"> The second value. </param>
        /// <returns>
        ///     The clamped value.
        /// </returns>
        [CLSCompliant(true),]
        public static byte Gcd (byte x, byte y)
        {
            while (!(y == 0))
            {
                byte temp = y;
                y = (byte)(x % y);
                x = temp;
            }

            return x;
        }

        /// <summary>
        ///     Finds the greatest common divisor (GCD) of two values.
        /// </summary>
        /// <param name="x"> The first value. </param>
        /// <param name="y"> The second value. </param>
        /// <returns>
        ///     The clamped value.
        /// </returns>
        [CLSCompliant(true),]
        public static short Gcd (short x, short y)
        {
            while (!(y == 0))
            {
                short temp = y;
                y = (short)(x % y);
                x = temp;
            }

            return x;
        }

        /// <summary>
        ///     Finds the greatest common divisor (GCD) of two values.
        /// </summary>
        /// <param name="x"> The first value. </param>
        /// <param name="y"> The second value. </param>
        /// <returns>
        ///     The clamped value.
        /// </returns>
        [CLSCompliant(false),]
        public static ushort Gcd (ushort x, ushort y)
        {
            while (!(y == 0))
            {
                ushort temp = y;
                y = (ushort)(x % y);
                x = temp;
            }

            return x;
        }

        /// <summary>
        ///     Finds the greatest common divisor (GCD) of two values.
        /// </summary>
        /// <param name="x"> The first value. </param>
        /// <param name="y"> The second value. </param>
        /// <returns>
        ///     The clamped value.
        /// </returns>
        [CLSCompliant(true),]
        public static int Gcd (int x, int y)
        {
            while (!(y == 0))
            {
                int temp = y;
                y = (int)(x % y);
                x = temp;
            }

            return x;
        }

        /// <summary>
        ///     Finds the greatest common divisor (GCD) of two values.
        /// </summary>
        /// <param name="x"> The first value. </param>
        /// <param name="y"> The second value. </param>
        /// <returns>
        ///     The clamped value.
        /// </returns>
        [CLSCompliant(false),]
        public static uint Gcd (uint x, uint y)
        {
            while (!(y == 0))
            {
                uint temp = y;
                y = (uint)(x % y);
                x = temp;
            }

            return x;
        }

        /// <summary>
        ///     Finds the greatest common divisor (GCD) of two values.
        /// </summary>
        /// <param name="x"> The first value. </param>
        /// <param name="y"> The second value. </param>
        /// <returns>
        ///     The clamped value.
        /// </returns>
        [CLSCompliant(true),]
        public static long Gcd (long x, long y)
        {
            while (!(y == 0))
            {
                long temp = y;
                y = (long)(x % y);
                x = temp;
            }

            return x;
        }

        /// <summary>
        ///     Finds the greatest common divisor (GCD) of two values.
        /// </summary>
        /// <param name="x"> The first value. </param>
        /// <param name="y"> The second value. </param>
        /// <returns>
        ///     The clamped value.
        /// </returns>
        [CLSCompliant(false),]
        public static ulong Gcd (ulong x, ulong y)
        {
            while (!(y == 0ul))
            {
                ulong temp = y;
                y = (ulong)(x % y);
                x = temp;
            }

            return x;
        }

        /// <summary>
        ///     Finds the greatest common divisor (GCD) of two values.
        /// </summary>
        /// <param name="x"> The first value. </param>
        /// <param name="y"> The second value. </param>
        /// <returns>
        ///     The clamped value.
        /// </returns>
        [CLSCompliant(true),]
        public static float Gcd (float x, float y)
        {
            while (!y.AlmostZero())
            {
                float temp = y;
                y = (float)(x % y);
                x = temp;
            }

            return x;
        }

        /// <summary>
        ///     Finds the greatest common divisor (GCD) of two values.
        /// </summary>
        /// <param name="x"> The first value. </param>
        /// <param name="y"> The second value. </param>
        /// <returns>
        ///     The clamped value.
        /// </returns>
        [CLSCompliant(true),]
        public static double Gcd (double x, double y)
        {
            while (!y.AlmostZero())
            {
                double temp = y;
                y = (double)(x % y);
                x = temp;
            }

            return x;
        }

        /// <summary>
        ///     Finds the greatest common divisor (GCD) of two values.
        /// </summary>
        /// <param name="x"> The first value. </param>
        /// <param name="y"> The second value. </param>
        /// <returns>
        ///     The clamped value.
        /// </returns>
        [CLSCompliant(true),]
        public static decimal Gcd (decimal x, decimal y)
        {
            while (!(y == 0.0m))
            {
                decimal temp = y;
                y = (decimal)(x % y);
                x = temp;
            }

            return x;
        }

        /// <summary>
        ///     Finds the least common multiple (LCM) of two values.
        /// </summary>
        /// <param name="x"> The first value. </param>
        /// <param name="y"> The second value. </param>
        /// <returns>
        ///     The clamped value.
        /// </returns>
        [CLSCompliant(false),]
        public static sbyte Lcm (sbyte x, sbyte y)
        {
            return (sbyte)((x / MathUtils.Gcd(x, y)) * y);
        }

        /// <summary>
        ///     Finds the least common multiple (LCM) of two values.
        /// </summary>
        /// <param name="x"> The first value. </param>
        /// <param name="y"> The second value. </param>
        /// <returns>
        ///     The clamped value.
        /// </returns>
        [CLSCompliant(true),]
        public static byte Lcm (byte x, byte y)
        {
            return (byte)((x / MathUtils.Gcd(x, y)) * y);
        }

        /// <summary>
        ///     Finds the least common multiple (LCM) of two values.
        /// </summary>
        /// <param name="x"> The first value. </param>
        /// <param name="y"> The second value. </param>
        /// <returns>
        ///     The clamped value.
        /// </returns>
        [CLSCompliant(true),]
        public static short Lcm (short x, short y)
        {
            return (short)((x / MathUtils.Gcd(x, y)) * y);
        }

        /// <summary>
        ///     Finds the least common multiple (LCM) of two values.
        /// </summary>
        /// <param name="x"> The first value. </param>
        /// <param name="y"> The second value. </param>
        /// <returns>
        ///     The clamped value.
        /// </returns>
        [CLSCompliant(false),]
        public static ushort Lcm (ushort x, ushort y)
        {
            return (ushort)((x / MathUtils.Gcd(x, y)) * y);
        }

        /// <summary>
        ///     Finds the least common multiple (LCM) of two values.
        /// </summary>
        /// <param name="x"> The first value. </param>
        /// <param name="y"> The second value. </param>
        /// <returns>
        ///     The clamped value.
        /// </returns>
        [CLSCompliant(true),]
        public static int Lcm (int x, int y)
        {
            return (int)((x / MathUtils.Gcd(x, y)) * y);
        }

        /// <summary>
        ///     Finds the least common multiple (LCM) of two values.
        /// </summary>
        /// <param name="x"> The first value. </param>
        /// <param name="y"> The second value. </param>
        /// <returns>
        ///     The clamped value.
        /// </returns>
        [CLSCompliant(false),]
        public static uint Lcm (uint x, uint y)
        {
            return (uint)((x / MathUtils.Gcd(x, y)) * y);
        }

        /// <summary>
        ///     Finds the least common multiple (LCM) of two values.
        /// </summary>
        /// <param name="x"> The first value. </param>
        /// <param name="y"> The second value. </param>
        /// <returns>
        ///     The clamped value.
        /// </returns>
        [CLSCompliant(true),]
        public static long Lcm (long x, long y)
        {
            return (long)((x / MathUtils.Gcd(x, y)) * y);
        }

        /// <summary>
        ///     Finds the least common multiple (LCM) of two values.
        /// </summary>
        /// <param name="x"> The first value. </param>
        /// <param name="y"> The second value. </param>
        /// <returns>
        ///     The clamped value.
        /// </returns>
        [CLSCompliant(false),]
        public static ulong Lcm (ulong x, ulong y)
        {
            return (ulong)((x / MathUtils.Gcd(x, y)) * y);
        }

        /// <summary>
        ///     Finds the least common multiple (LCM) of two values.
        /// </summary>
        /// <param name="x"> The first value. </param>
        /// <param name="y"> The second value. </param>
        /// <returns>
        ///     The clamped value.
        /// </returns>
        [CLSCompliant(true),]
        public static float Lcm (float x, float y)
        {
            return (float)((x / MathUtils.Gcd(x, y)) * y);
        }

        /// <summary>
        ///     Finds the least common multiple (LCM) of two values.
        /// </summary>
        /// <param name="x"> The first value. </param>
        /// <param name="y"> The second value. </param>
        /// <returns>
        ///     The clamped value.
        /// </returns>
        [CLSCompliant(true),]
        public static double Lcm (double x, double y)
        {
            return (double)((x / MathUtils.Gcd(x, y)) * y);
        }

        /// <summary>
        ///     Finds the least common multiple (LCM) of two values.
        /// </summary>
        /// <param name="x"> The first value. </param>
        /// <param name="y"> The second value. </param>
        /// <returns>
        ///     The clamped value.
        /// </returns>
        [CLSCompliant(true),]
        public static decimal Lcm (decimal x, decimal y)
        {
            return (decimal)((x / MathUtils.Gcd(x, y)) * y);
        }

        #endregion
    }
}
