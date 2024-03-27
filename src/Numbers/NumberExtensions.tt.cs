
using System;
using System.Linq;
using System.Globalization;
using System.Collections.Generic;
using System.Diagnostics.CodeAnalysis;

// ReSharper disable RedundantCast

namespace RI.Utilities.Numbers
{
    /// <summary>
    ///     Provides utility/extension methods for numerical types.
    /// </summary>
    /// <remarks>
    /// <para>
    /// Numerical types are: <see cref="sbyte"/>, <see cref="byte"/>, <see cref="short"/>, <see cref="ushort"/>, <see cref="int"/>, <see cref="uint"/>, <see cref="long"/>, <see cref="ulong"/>, <see cref="float"/>, <see cref="double"/>, <see cref="decimal"/>.
    /// </para>
    /// </remarks>
    /// <threadsafety static="false" instance="false" />
    public static class NumberExtensions
    {

        /// <summary>
        /// Clamps a value between an inclusive minimum and maximum value.
        /// </summary>
        /// <param name="value">The value.</param>
        /// <param name="min">The lowest possible value.</param>
        /// <param name="max">The highest possible value.</param>
        /// <returns>
        /// The clamped value.
        /// </returns>
        [CLSCompliant(false)]
        public static sbyte Clamp (this sbyte value, sbyte min, sbyte max)
        {
            if(value < min)
            {
                return min;
            }
            if(value > max)
            {
                return max;
            }
            return value;
        }

        /// <summary>
        /// Clamps a value to an inclusive minimum value.
        /// </summary>
        /// <param name="value">The value.</param>
        /// <param name="min">The lowest possible value.</param>
        /// <returns>
        /// The clamped value.
        /// </returns>
        [CLSCompliant(false)]
        public static sbyte ClampMin (this sbyte value, sbyte min)
        {
            if(value < min)
            {
                return min;
            }
            return value;
        }

        /// <summary>
        /// Clamps a value to an inclusive maximum value.
        /// </summary>
        /// <param name="value">The value.</param>
        /// <param name="max">The highest possible value.</param>
        /// <returns>
        /// The clamped value.
        /// </returns>
        [CLSCompliant(false)]
        public static sbyte ClampMax (this sbyte value, sbyte max)
        {
            if(value > max)
            {
                return max;
            }
            return value;
        }

        /// <summary>
        /// Quantizes a value to the nearest value of a multiple.
        /// </summary>
        /// <param name="value">The value.</param>
        /// <param name="multiple">The multiple.</param>
        /// <returns>
        /// The quantized value.
        /// </returns>
        /// <remarks>
        /// <para>
        /// <see cref="MidpointRounding.ToEven"/> is used for <see cref="MidpointRounding"/>.
        /// </para>
        /// </remarks>
        [CLSCompliant(false)]
        public static sbyte Quantize (this sbyte value, sbyte multiple) => (sbyte)(Math.Round((double)value / (double)multiple) * (double)multiple);

        /// <summary>
        /// Quantizes a value to the nearest value of a multiple.
        /// </summary>
        /// <param name="value">The value.</param>
        /// <param name="multiple">The multiple.</param>
        /// <param name="rounding">The kind of rounding to use.</param>
        /// <returns>
        /// The quantized value.
        /// </returns>
        [CLSCompliant(false)]
        public static sbyte Quantize (this sbyte value, sbyte multiple, MidpointRounding rounding) => (sbyte)(Math.Round((double)value / (double)multiple, rounding) * (double)multiple);

        /// <summary>
        /// Finds the greatest common divisor (GCD) of two values.
        /// </summary>
        /// <param name="x">The first value.</param>
        /// <param name="y">The second value.</param>
        /// <returns>
        /// The clamped value.
        /// </returns>
        [CLSCompliant(false)]
        public static sbyte Gcd (this sbyte x, sbyte y) => MathUtils.Gcd(x, y);

        /// <summary>
        /// Finds the least common multiple (LCM) of two values.
        /// </summary>
        /// <param name="x">The first value.</param>
        /// <param name="y">The second value.</param>
        /// <returns>
        /// The clamped value.
        /// </returns>
        [CLSCompliant(false)]
        public static sbyte Lcm (this sbyte x, sbyte y) => MathUtils.Lcm(x, y);

        /// <summary>
        /// Gets the sum from a sequence of values.
        /// </summary>
        /// <param name="values">The sequence of values.</param>
        /// <returns>
        /// The sum of the sequence.
        /// </returns>
        /// <remarks>
        /// <para>
        /// <paramref name="values" /> is enumerated only once.
        /// </para>
        /// </remarks>
        /// <exception cref="ArgumentNullException"> <paramref name="values" /> is null. </exception>
        [CLSCompliant(false)]
        public static sbyte Sum (this IEnumerable<sbyte> values)
        {
            if(values == null)
            {
                throw new ArgumentNullException(nameof(values));
            }

            sbyte sum = 0;
            foreach(sbyte value in values)
            {
                sum += value;
            }
            return sum;
        }

        /// <summary>
        /// Gets the minimum value from a sequence of values.
        /// </summary>
        /// <param name="values">The sequence of values.</param>
        /// <returns>
        /// The minimum value of the sequence.
        /// </returns>
        /// <remarks>
        /// <para>
        /// <paramref name="values" /> is enumerated only once.
        /// </para>
        /// </remarks>
        /// <exception cref="ArgumentNullException"> <paramref name="values" /> is null. </exception>
        [CLSCompliant(false)]
        public static sbyte Min (this IEnumerable<sbyte> values)
        {
            if(values == null)
            {
                throw new ArgumentNullException(nameof(values));
            }

            sbyte min = sbyte.MaxValue;
            foreach(sbyte value in values)
            {
                if(value < min)
                {
                    min = value;
                }
            }
            return min;
        }

        /// <summary>
        /// Gets the maximum value from a sequence of values.
        /// </summary>
        /// <param name="values">The sequence of values.</param>
        /// <returns>
        /// The maximum value of the sequence.
        /// </returns>
        /// <remarks>
        /// <para>
        /// <paramref name="values" /> is enumerated only once.
        /// </para>
        /// </remarks>
        /// <exception cref="ArgumentNullException"> <paramref name="values" /> is null. </exception>
        [CLSCompliant(false)]
        public static sbyte Max (this IEnumerable<sbyte> values)
        {
            if(values == null)
            {
                throw new ArgumentNullException(nameof(values));
            }

            sbyte max = sbyte.MinValue;
            foreach(sbyte value in values)
            {
                if(value > max)
                {
                    max = value;
                }
            }
            return max;
        }

        /// <summary>
        /// Finds the greatest common divisor (GCD) of a sequence of values.
        /// </summary>
        /// <param name="values">The sequence of values.</param>
        /// <returns>
        /// The GCD of the sequence or zero if the sequence is empty.
        /// </returns>
        /// <remarks>
        /// <para>
        /// <paramref name="values" /> is enumerated only once.
        /// </para>
        /// </remarks>
        /// <exception cref="ArgumentNullException"> <paramref name="values" /> is null. </exception>
        [CLSCompliant(false)]
        public static sbyte Gcd (this IEnumerable<sbyte> values)
        {
            if(values == null)
            {
                throw new ArgumentNullException(nameof(values));
            }

            sbyte gcd = 0;
            int index = 0;
            foreach(sbyte value in values)
            {
                if(index == 0)
                {
                    gcd = value;
                }
                else
                {
                    gcd = MathUtils.Gcd(gcd, value);
                }
                index++;
            }
            return gcd;
        }

        /// <summary>
        /// Finds the least common multiple (LCM) of a sequence of values.
        /// </summary>
        /// <param name="values">The sequence of values.</param>
        /// <returns>
        /// The LCM of the sequence or zero if the sequence is empty.
        /// </returns>
        /// <remarks>
        /// <para>
        /// <paramref name="values" /> is enumerated only once.
        /// </para>
        /// </remarks>
        /// <exception cref="ArgumentNullException"> <paramref name="values" /> is null. </exception>
        [CLSCompliant(false)]
        public static sbyte Lcm (this IEnumerable<sbyte> values)
        {
            if(values == null)
            {
                throw new ArgumentNullException(nameof(values));
            }

            sbyte lcm = 0;
            int index = 0;
            foreach(sbyte value in values)
            {
                if(index == 0)
                {
                    lcm = value;
                }
                else
                {
                    lcm = MathUtils.Lcm(lcm, value);
                }
                index++;
            }
            return lcm;
        }

        /// <summary>
        /// Converts a sequence of values to floats.
        /// </summary>
        /// <param name="values">The sequence of values.</param>
        /// <returns>
        /// The list of values as floats.
        /// An empty list is returned if the sequence is empty.
        /// </returns>
        /// <remarks>
        /// <para>
        /// <paramref name="values" /> is enumerated only once.
        /// </para>
        /// </remarks>
        /// <exception cref="ArgumentNullException"> <paramref name="values" /> is null. </exception>
        [CLSCompliant(false)]
        public static IEnumerable<float> AsFloat (this IEnumerable<sbyte> values)
        {
            if(values == null)
            {
                throw new ArgumentNullException(nameof(values));
            }

            return values.Select(x => (float)x);
        }

        /// <summary>
        /// Converts a sequence of values to doubles.
        /// </summary>
        /// <param name="values">The sequence of values.</param>
        /// <returns>
        /// The list of values as doubles.
        /// An empty list is returned if the sequence is empty.
        /// </returns>
        /// <remarks>
        /// <para>
        /// <paramref name="values" /> is enumerated only once.
        /// </para>
        /// </remarks>
        /// <exception cref="ArgumentNullException"> <paramref name="values" /> is null. </exception>
        [CLSCompliant(false)]
        public static IEnumerable<double> AsDouble (this IEnumerable<sbyte> values)
        {
            if(values == null)
            {
                throw new ArgumentNullException(nameof(values));
            }

            return values.Select(x => (double)x);
        }

        /// <summary>
        /// Gets statistics for a sequence of values.
        /// </summary>
        /// <param name="values">The sequence of values.</param>
        /// <returns>
        /// The statistics for the sequence.
        /// </returns>
        /// <remarks>
        /// <para>
        /// <paramref name="values" /> is enumerated only once.
        /// </para>
        /// </remarks>
        /// <exception cref="ArgumentNullException"> <paramref name="values" /> is null. </exception>
        [CLSCompliant(false)]
        public static StatisticValues GetStatistics (this IEnumerable<sbyte> values) => new StatisticValues(values.AsDouble());

        /// <summary>
        /// Gets each digit of a value as separate integers.
        /// </summary>
        /// <param name="value">The value.</param>
        /// <returns>
        /// The list of digits.
        /// The most-significant digit comes first, the least-significant digit comes last.
        /// </returns>
        [CLSCompliant(false)]
        public static List<int> GetDigits (this sbyte value)
        {
            string stringValue = value.ToString("D", CultureInfo.InvariantCulture);
            List<int> digits = new List<int>(20);
            foreach(char chr in stringValue)
            {
                if((chr >= 48) && (chr <= 57))
                {
                    digits.Add(chr - 48);
                }
            }
            return digits;
        }

        /// <summary>
        /// Clamps a value between an inclusive minimum and maximum value.
        /// </summary>
        /// <param name="value">The value.</param>
        /// <param name="min">The lowest possible value.</param>
        /// <param name="max">The highest possible value.</param>
        /// <returns>
        /// The clamped value.
        /// </returns>
        [CLSCompliant(true)]
        public static byte Clamp (this byte value, byte min, byte max)
        {
            if(value < min)
            {
                return min;
            }
            if(value > max)
            {
                return max;
            }
            return value;
        }

        /// <summary>
        /// Clamps a value to an inclusive minimum value.
        /// </summary>
        /// <param name="value">The value.</param>
        /// <param name="min">The lowest possible value.</param>
        /// <returns>
        /// The clamped value.
        /// </returns>
        [CLSCompliant(true)]
        public static byte ClampMin (this byte value, byte min)
        {
            if(value < min)
            {
                return min;
            }
            return value;
        }

        /// <summary>
        /// Clamps a value to an inclusive maximum value.
        /// </summary>
        /// <param name="value">The value.</param>
        /// <param name="max">The highest possible value.</param>
        /// <returns>
        /// The clamped value.
        /// </returns>
        [CLSCompliant(true)]
        public static byte ClampMax (this byte value, byte max)
        {
            if(value > max)
            {
                return max;
            }
            return value;
        }

        /// <summary>
        /// Quantizes a value to the nearest value of a multiple.
        /// </summary>
        /// <param name="value">The value.</param>
        /// <param name="multiple">The multiple.</param>
        /// <returns>
        /// The quantized value.
        /// </returns>
        /// <remarks>
        /// <para>
        /// <see cref="MidpointRounding.ToEven"/> is used for <see cref="MidpointRounding"/>.
        /// </para>
        /// </remarks>
        [CLSCompliant(true)]
        public static byte Quantize (this byte value, byte multiple) => (byte)(Math.Round((double)value / (double)multiple) * (double)multiple);

        /// <summary>
        /// Quantizes a value to the nearest value of a multiple.
        /// </summary>
        /// <param name="value">The value.</param>
        /// <param name="multiple">The multiple.</param>
        /// <param name="rounding">The kind of rounding to use.</param>
        /// <returns>
        /// The quantized value.
        /// </returns>
        [CLSCompliant(true)]
        public static byte Quantize (this byte value, byte multiple, MidpointRounding rounding) => (byte)(Math.Round((double)value / (double)multiple, rounding) * (double)multiple);

        /// <summary>
        /// Finds the greatest common divisor (GCD) of two values.
        /// </summary>
        /// <param name="x">The first value.</param>
        /// <param name="y">The second value.</param>
        /// <returns>
        /// The clamped value.
        /// </returns>
        [CLSCompliant(true)]
        public static byte Gcd (this byte x, byte y) => MathUtils.Gcd(x, y);

        /// <summary>
        /// Finds the least common multiple (LCM) of two values.
        /// </summary>
        /// <param name="x">The first value.</param>
        /// <param name="y">The second value.</param>
        /// <returns>
        /// The clamped value.
        /// </returns>
        [CLSCompliant(true)]
        public static byte Lcm (this byte x, byte y) => MathUtils.Lcm(x, y);

        /// <summary>
        /// Gets the sum from a sequence of values.
        /// </summary>
        /// <param name="values">The sequence of values.</param>
        /// <returns>
        /// The sum of the sequence.
        /// </returns>
        /// <remarks>
        /// <para>
        /// <paramref name="values" /> is enumerated only once.
        /// </para>
        /// </remarks>
        /// <exception cref="ArgumentNullException"> <paramref name="values" /> is null. </exception>
        [CLSCompliant(true)]
        public static byte Sum (this IEnumerable<byte> values)
        {
            if(values == null)
            {
                throw new ArgumentNullException(nameof(values));
            }

            byte sum = 0;
            foreach(byte value in values)
            {
                sum += value;
            }
            return sum;
        }

        /// <summary>
        /// Gets the minimum value from a sequence of values.
        /// </summary>
        /// <param name="values">The sequence of values.</param>
        /// <returns>
        /// The minimum value of the sequence.
        /// </returns>
        /// <remarks>
        /// <para>
        /// <paramref name="values" /> is enumerated only once.
        /// </para>
        /// </remarks>
        /// <exception cref="ArgumentNullException"> <paramref name="values" /> is null. </exception>
        [CLSCompliant(true)]
        public static byte Min (this IEnumerable<byte> values)
        {
            if(values == null)
            {
                throw new ArgumentNullException(nameof(values));
            }

            byte min = byte.MaxValue;
            foreach(byte value in values)
            {
                if(value < min)
                {
                    min = value;
                }
            }
            return min;
        }

        /// <summary>
        /// Gets the maximum value from a sequence of values.
        /// </summary>
        /// <param name="values">The sequence of values.</param>
        /// <returns>
        /// The maximum value of the sequence.
        /// </returns>
        /// <remarks>
        /// <para>
        /// <paramref name="values" /> is enumerated only once.
        /// </para>
        /// </remarks>
        /// <exception cref="ArgumentNullException"> <paramref name="values" /> is null. </exception>
        [CLSCompliant(true)]
        public static byte Max (this IEnumerable<byte> values)
        {
            if(values == null)
            {
                throw new ArgumentNullException(nameof(values));
            }

            byte max = byte.MinValue;
            foreach(byte value in values)
            {
                if(value > max)
                {
                    max = value;
                }
            }
            return max;
        }

        /// <summary>
        /// Finds the greatest common divisor (GCD) of a sequence of values.
        /// </summary>
        /// <param name="values">The sequence of values.</param>
        /// <returns>
        /// The GCD of the sequence or zero if the sequence is empty.
        /// </returns>
        /// <remarks>
        /// <para>
        /// <paramref name="values" /> is enumerated only once.
        /// </para>
        /// </remarks>
        /// <exception cref="ArgumentNullException"> <paramref name="values" /> is null. </exception>
        [CLSCompliant(true)]
        public static byte Gcd (this IEnumerable<byte> values)
        {
            if(values == null)
            {
                throw new ArgumentNullException(nameof(values));
            }

            byte gcd = 0;
            int index = 0;
            foreach(byte value in values)
            {
                if(index == 0)
                {
                    gcd = value;
                }
                else
                {
                    gcd = MathUtils.Gcd(gcd, value);
                }
                index++;
            }
            return gcd;
        }

        /// <summary>
        /// Finds the least common multiple (LCM) of a sequence of values.
        /// </summary>
        /// <param name="values">The sequence of values.</param>
        /// <returns>
        /// The LCM of the sequence or zero if the sequence is empty.
        /// </returns>
        /// <remarks>
        /// <para>
        /// <paramref name="values" /> is enumerated only once.
        /// </para>
        /// </remarks>
        /// <exception cref="ArgumentNullException"> <paramref name="values" /> is null. </exception>
        [CLSCompliant(true)]
        public static byte Lcm (this IEnumerable<byte> values)
        {
            if(values == null)
            {
                throw new ArgumentNullException(nameof(values));
            }

            byte lcm = 0;
            int index = 0;
            foreach(byte value in values)
            {
                if(index == 0)
                {
                    lcm = value;
                }
                else
                {
                    lcm = MathUtils.Lcm(lcm, value);
                }
                index++;
            }
            return lcm;
        }

        /// <summary>
        /// Converts a sequence of values to floats.
        /// </summary>
        /// <param name="values">The sequence of values.</param>
        /// <returns>
        /// The list of values as floats.
        /// An empty list is returned if the sequence is empty.
        /// </returns>
        /// <remarks>
        /// <para>
        /// <paramref name="values" /> is enumerated only once.
        /// </para>
        /// </remarks>
        /// <exception cref="ArgumentNullException"> <paramref name="values" /> is null. </exception>
        [CLSCompliant(true)]
        public static IEnumerable<float> AsFloat (this IEnumerable<byte> values)
        {
            if(values == null)
            {
                throw new ArgumentNullException(nameof(values));
            }

            return values.Select(x => (float)x);
        }

        /// <summary>
        /// Converts a sequence of values to doubles.
        /// </summary>
        /// <param name="values">The sequence of values.</param>
        /// <returns>
        /// The list of values as doubles.
        /// An empty list is returned if the sequence is empty.
        /// </returns>
        /// <remarks>
        /// <para>
        /// <paramref name="values" /> is enumerated only once.
        /// </para>
        /// </remarks>
        /// <exception cref="ArgumentNullException"> <paramref name="values" /> is null. </exception>
        [CLSCompliant(true)]
        public static IEnumerable<double> AsDouble (this IEnumerable<byte> values)
        {
            if(values == null)
            {
                throw new ArgumentNullException(nameof(values));
            }

            return values.Select(x => (double)x);
        }

        /// <summary>
        /// Gets statistics for a sequence of values.
        /// </summary>
        /// <param name="values">The sequence of values.</param>
        /// <returns>
        /// The statistics for the sequence.
        /// </returns>
        /// <remarks>
        /// <para>
        /// <paramref name="values" /> is enumerated only once.
        /// </para>
        /// </remarks>
        /// <exception cref="ArgumentNullException"> <paramref name="values" /> is null. </exception>
        [CLSCompliant(true)]
        public static StatisticValues GetStatistics (this IEnumerable<byte> values) => new StatisticValues(values.AsDouble());

        /// <summary>
        /// Gets each digit of a value as separate integers.
        /// </summary>
        /// <param name="value">The value.</param>
        /// <returns>
        /// The list of digits.
        /// The most-significant digit comes first, the least-significant digit comes last.
        /// </returns>
        [CLSCompliant(true)]
        public static List<int> GetDigits (this byte value)
        {
            string stringValue = value.ToString("D", CultureInfo.InvariantCulture);
            List<int> digits = new List<int>(20);
            foreach(char chr in stringValue)
            {
                if((chr >= 48) && (chr <= 57))
                {
                    digits.Add(chr - 48);
                }
            }
            return digits;
        }

        /// <summary>
        /// Clamps a value between an inclusive minimum and maximum value.
        /// </summary>
        /// <param name="value">The value.</param>
        /// <param name="min">The lowest possible value.</param>
        /// <param name="max">The highest possible value.</param>
        /// <returns>
        /// The clamped value.
        /// </returns>
        [CLSCompliant(true)]
        public static short Clamp (this short value, short min, short max)
        {
            if(value < min)
            {
                return min;
            }
            if(value > max)
            {
                return max;
            }
            return value;
        }

        /// <summary>
        /// Clamps a value to an inclusive minimum value.
        /// </summary>
        /// <param name="value">The value.</param>
        /// <param name="min">The lowest possible value.</param>
        /// <returns>
        /// The clamped value.
        /// </returns>
        [CLSCompliant(true)]
        public static short ClampMin (this short value, short min)
        {
            if(value < min)
            {
                return min;
            }
            return value;
        }

        /// <summary>
        /// Clamps a value to an inclusive maximum value.
        /// </summary>
        /// <param name="value">The value.</param>
        /// <param name="max">The highest possible value.</param>
        /// <returns>
        /// The clamped value.
        /// </returns>
        [CLSCompliant(true)]
        public static short ClampMax (this short value, short max)
        {
            if(value > max)
            {
                return max;
            }
            return value;
        }

        /// <summary>
        /// Quantizes a value to the nearest value of a multiple.
        /// </summary>
        /// <param name="value">The value.</param>
        /// <param name="multiple">The multiple.</param>
        /// <returns>
        /// The quantized value.
        /// </returns>
        /// <remarks>
        /// <para>
        /// <see cref="MidpointRounding.ToEven"/> is used for <see cref="MidpointRounding"/>.
        /// </para>
        /// </remarks>
        [CLSCompliant(true)]
        public static short Quantize (this short value, short multiple) => (short)(Math.Round((double)value / (double)multiple) * (double)multiple);

        /// <summary>
        /// Quantizes a value to the nearest value of a multiple.
        /// </summary>
        /// <param name="value">The value.</param>
        /// <param name="multiple">The multiple.</param>
        /// <param name="rounding">The kind of rounding to use.</param>
        /// <returns>
        /// The quantized value.
        /// </returns>
        [CLSCompliant(true)]
        public static short Quantize (this short value, short multiple, MidpointRounding rounding) => (short)(Math.Round((double)value / (double)multiple, rounding) * (double)multiple);

        /// <summary>
        /// Finds the greatest common divisor (GCD) of two values.
        /// </summary>
        /// <param name="x">The first value.</param>
        /// <param name="y">The second value.</param>
        /// <returns>
        /// The clamped value.
        /// </returns>
        [CLSCompliant(true)]
        public static short Gcd (this short x, short y) => MathUtils.Gcd(x, y);

        /// <summary>
        /// Finds the least common multiple (LCM) of two values.
        /// </summary>
        /// <param name="x">The first value.</param>
        /// <param name="y">The second value.</param>
        /// <returns>
        /// The clamped value.
        /// </returns>
        [CLSCompliant(true)]
        public static short Lcm (this short x, short y) => MathUtils.Lcm(x, y);

        /// <summary>
        /// Gets the sum from a sequence of values.
        /// </summary>
        /// <param name="values">The sequence of values.</param>
        /// <returns>
        /// The sum of the sequence.
        /// </returns>
        /// <remarks>
        /// <para>
        /// <paramref name="values" /> is enumerated only once.
        /// </para>
        /// </remarks>
        /// <exception cref="ArgumentNullException"> <paramref name="values" /> is null. </exception>
        [CLSCompliant(true)]
        public static short Sum (this IEnumerable<short> values)
        {
            if(values == null)
            {
                throw new ArgumentNullException(nameof(values));
            }

            short sum = 0;
            foreach(short value in values)
            {
                sum += value;
            }
            return sum;
        }

        /// <summary>
        /// Gets the minimum value from a sequence of values.
        /// </summary>
        /// <param name="values">The sequence of values.</param>
        /// <returns>
        /// The minimum value of the sequence.
        /// </returns>
        /// <remarks>
        /// <para>
        /// <paramref name="values" /> is enumerated only once.
        /// </para>
        /// </remarks>
        /// <exception cref="ArgumentNullException"> <paramref name="values" /> is null. </exception>
        [CLSCompliant(true)]
        public static short Min (this IEnumerable<short> values)
        {
            if(values == null)
            {
                throw new ArgumentNullException(nameof(values));
            }

            short min = short.MaxValue;
            foreach(short value in values)
            {
                if(value < min)
                {
                    min = value;
                }
            }
            return min;
        }

        /// <summary>
        /// Gets the maximum value from a sequence of values.
        /// </summary>
        /// <param name="values">The sequence of values.</param>
        /// <returns>
        /// The maximum value of the sequence.
        /// </returns>
        /// <remarks>
        /// <para>
        /// <paramref name="values" /> is enumerated only once.
        /// </para>
        /// </remarks>
        /// <exception cref="ArgumentNullException"> <paramref name="values" /> is null. </exception>
        [CLSCompliant(true)]
        public static short Max (this IEnumerable<short> values)
        {
            if(values == null)
            {
                throw new ArgumentNullException(nameof(values));
            }

            short max = short.MinValue;
            foreach(short value in values)
            {
                if(value > max)
                {
                    max = value;
                }
            }
            return max;
        }

        /// <summary>
        /// Finds the greatest common divisor (GCD) of a sequence of values.
        /// </summary>
        /// <param name="values">The sequence of values.</param>
        /// <returns>
        /// The GCD of the sequence or zero if the sequence is empty.
        /// </returns>
        /// <remarks>
        /// <para>
        /// <paramref name="values" /> is enumerated only once.
        /// </para>
        /// </remarks>
        /// <exception cref="ArgumentNullException"> <paramref name="values" /> is null. </exception>
        [CLSCompliant(true)]
        public static short Gcd (this IEnumerable<short> values)
        {
            if(values == null)
            {
                throw new ArgumentNullException(nameof(values));
            }

            short gcd = 0;
            int index = 0;
            foreach(short value in values)
            {
                if(index == 0)
                {
                    gcd = value;
                }
                else
                {
                    gcd = MathUtils.Gcd(gcd, value);
                }
                index++;
            }
            return gcd;
        }

        /// <summary>
        /// Finds the least common multiple (LCM) of a sequence of values.
        /// </summary>
        /// <param name="values">The sequence of values.</param>
        /// <returns>
        /// The LCM of the sequence or zero if the sequence is empty.
        /// </returns>
        /// <remarks>
        /// <para>
        /// <paramref name="values" /> is enumerated only once.
        /// </para>
        /// </remarks>
        /// <exception cref="ArgumentNullException"> <paramref name="values" /> is null. </exception>
        [CLSCompliant(true)]
        public static short Lcm (this IEnumerable<short> values)
        {
            if(values == null)
            {
                throw new ArgumentNullException(nameof(values));
            }

            short lcm = 0;
            int index = 0;
            foreach(short value in values)
            {
                if(index == 0)
                {
                    lcm = value;
                }
                else
                {
                    lcm = MathUtils.Lcm(lcm, value);
                }
                index++;
            }
            return lcm;
        }

        /// <summary>
        /// Converts a sequence of values to floats.
        /// </summary>
        /// <param name="values">The sequence of values.</param>
        /// <returns>
        /// The list of values as floats.
        /// An empty list is returned if the sequence is empty.
        /// </returns>
        /// <remarks>
        /// <para>
        /// <paramref name="values" /> is enumerated only once.
        /// </para>
        /// </remarks>
        /// <exception cref="ArgumentNullException"> <paramref name="values" /> is null. </exception>
        [CLSCompliant(true)]
        public static IEnumerable<float> AsFloat (this IEnumerable<short> values)
        {
            if(values == null)
            {
                throw new ArgumentNullException(nameof(values));
            }

            return values.Select(x => (float)x);
        }

        /// <summary>
        /// Converts a sequence of values to doubles.
        /// </summary>
        /// <param name="values">The sequence of values.</param>
        /// <returns>
        /// The list of values as doubles.
        /// An empty list is returned if the sequence is empty.
        /// </returns>
        /// <remarks>
        /// <para>
        /// <paramref name="values" /> is enumerated only once.
        /// </para>
        /// </remarks>
        /// <exception cref="ArgumentNullException"> <paramref name="values" /> is null. </exception>
        [CLSCompliant(true)]
        public static IEnumerable<double> AsDouble (this IEnumerable<short> values)
        {
            if(values == null)
            {
                throw new ArgumentNullException(nameof(values));
            }

            return values.Select(x => (double)x);
        }

        /// <summary>
        /// Gets statistics for a sequence of values.
        /// </summary>
        /// <param name="values">The sequence of values.</param>
        /// <returns>
        /// The statistics for the sequence.
        /// </returns>
        /// <remarks>
        /// <para>
        /// <paramref name="values" /> is enumerated only once.
        /// </para>
        /// </remarks>
        /// <exception cref="ArgumentNullException"> <paramref name="values" /> is null. </exception>
        [CLSCompliant(true)]
        public static StatisticValues GetStatistics (this IEnumerable<short> values) => new StatisticValues(values.AsDouble());

        /// <summary>
        /// Gets each digit of a value as separate integers.
        /// </summary>
        /// <param name="value">The value.</param>
        /// <returns>
        /// The list of digits.
        /// The most-significant digit comes first, the least-significant digit comes last.
        /// </returns>
        [CLSCompliant(true)]
        public static List<int> GetDigits (this short value)
        {
            string stringValue = value.ToString("D", CultureInfo.InvariantCulture);
            List<int> digits = new List<int>(20);
            foreach(char chr in stringValue)
            {
                if((chr >= 48) && (chr <= 57))
                {
                    digits.Add(chr - 48);
                }
            }
            return digits;
        }

        /// <summary>
        /// Clamps a value between an inclusive minimum and maximum value.
        /// </summary>
        /// <param name="value">The value.</param>
        /// <param name="min">The lowest possible value.</param>
        /// <param name="max">The highest possible value.</param>
        /// <returns>
        /// The clamped value.
        /// </returns>
        [CLSCompliant(false)]
        public static ushort Clamp (this ushort value, ushort min, ushort max)
        {
            if(value < min)
            {
                return min;
            }
            if(value > max)
            {
                return max;
            }
            return value;
        }

        /// <summary>
        /// Clamps a value to an inclusive minimum value.
        /// </summary>
        /// <param name="value">The value.</param>
        /// <param name="min">The lowest possible value.</param>
        /// <returns>
        /// The clamped value.
        /// </returns>
        [CLSCompliant(false)]
        public static ushort ClampMin (this ushort value, ushort min)
        {
            if(value < min)
            {
                return min;
            }
            return value;
        }

        /// <summary>
        /// Clamps a value to an inclusive maximum value.
        /// </summary>
        /// <param name="value">The value.</param>
        /// <param name="max">The highest possible value.</param>
        /// <returns>
        /// The clamped value.
        /// </returns>
        [CLSCompliant(false)]
        public static ushort ClampMax (this ushort value, ushort max)
        {
            if(value > max)
            {
                return max;
            }
            return value;
        }

        /// <summary>
        /// Quantizes a value to the nearest value of a multiple.
        /// </summary>
        /// <param name="value">The value.</param>
        /// <param name="multiple">The multiple.</param>
        /// <returns>
        /// The quantized value.
        /// </returns>
        /// <remarks>
        /// <para>
        /// <see cref="MidpointRounding.ToEven"/> is used for <see cref="MidpointRounding"/>.
        /// </para>
        /// </remarks>
        [CLSCompliant(false)]
        public static ushort Quantize (this ushort value, ushort multiple) => (ushort)(Math.Round((double)value / (double)multiple) * (double)multiple);

        /// <summary>
        /// Quantizes a value to the nearest value of a multiple.
        /// </summary>
        /// <param name="value">The value.</param>
        /// <param name="multiple">The multiple.</param>
        /// <param name="rounding">The kind of rounding to use.</param>
        /// <returns>
        /// The quantized value.
        /// </returns>
        [CLSCompliant(false)]
        public static ushort Quantize (this ushort value, ushort multiple, MidpointRounding rounding) => (ushort)(Math.Round((double)value / (double)multiple, rounding) * (double)multiple);

        /// <summary>
        /// Finds the greatest common divisor (GCD) of two values.
        /// </summary>
        /// <param name="x">The first value.</param>
        /// <param name="y">The second value.</param>
        /// <returns>
        /// The clamped value.
        /// </returns>
        [CLSCompliant(false)]
        public static ushort Gcd (this ushort x, ushort y) => MathUtils.Gcd(x, y);

        /// <summary>
        /// Finds the least common multiple (LCM) of two values.
        /// </summary>
        /// <param name="x">The first value.</param>
        /// <param name="y">The second value.</param>
        /// <returns>
        /// The clamped value.
        /// </returns>
        [CLSCompliant(false)]
        public static ushort Lcm (this ushort x, ushort y) => MathUtils.Lcm(x, y);

        /// <summary>
        /// Gets the sum from a sequence of values.
        /// </summary>
        /// <param name="values">The sequence of values.</param>
        /// <returns>
        /// The sum of the sequence.
        /// </returns>
        /// <remarks>
        /// <para>
        /// <paramref name="values" /> is enumerated only once.
        /// </para>
        /// </remarks>
        /// <exception cref="ArgumentNullException"> <paramref name="values" /> is null. </exception>
        [CLSCompliant(false)]
        public static ushort Sum (this IEnumerable<ushort> values)
        {
            if(values == null)
            {
                throw new ArgumentNullException(nameof(values));
            }

            ushort sum = 0;
            foreach(ushort value in values)
            {
                sum += value;
            }
            return sum;
        }

        /// <summary>
        /// Gets the minimum value from a sequence of values.
        /// </summary>
        /// <param name="values">The sequence of values.</param>
        /// <returns>
        /// The minimum value of the sequence.
        /// </returns>
        /// <remarks>
        /// <para>
        /// <paramref name="values" /> is enumerated only once.
        /// </para>
        /// </remarks>
        /// <exception cref="ArgumentNullException"> <paramref name="values" /> is null. </exception>
        [CLSCompliant(false)]
        public static ushort Min (this IEnumerable<ushort> values)
        {
            if(values == null)
            {
                throw new ArgumentNullException(nameof(values));
            }

            ushort min = ushort.MaxValue;
            foreach(ushort value in values)
            {
                if(value < min)
                {
                    min = value;
                }
            }
            return min;
        }

        /// <summary>
        /// Gets the maximum value from a sequence of values.
        /// </summary>
        /// <param name="values">The sequence of values.</param>
        /// <returns>
        /// The maximum value of the sequence.
        /// </returns>
        /// <remarks>
        /// <para>
        /// <paramref name="values" /> is enumerated only once.
        /// </para>
        /// </remarks>
        /// <exception cref="ArgumentNullException"> <paramref name="values" /> is null. </exception>
        [CLSCompliant(false)]
        public static ushort Max (this IEnumerable<ushort> values)
        {
            if(values == null)
            {
                throw new ArgumentNullException(nameof(values));
            }

            ushort max = ushort.MinValue;
            foreach(ushort value in values)
            {
                if(value > max)
                {
                    max = value;
                }
            }
            return max;
        }

        /// <summary>
        /// Finds the greatest common divisor (GCD) of a sequence of values.
        /// </summary>
        /// <param name="values">The sequence of values.</param>
        /// <returns>
        /// The GCD of the sequence or zero if the sequence is empty.
        /// </returns>
        /// <remarks>
        /// <para>
        /// <paramref name="values" /> is enumerated only once.
        /// </para>
        /// </remarks>
        /// <exception cref="ArgumentNullException"> <paramref name="values" /> is null. </exception>
        [CLSCompliant(false)]
        public static ushort Gcd (this IEnumerable<ushort> values)
        {
            if(values == null)
            {
                throw new ArgumentNullException(nameof(values));
            }

            ushort gcd = 0;
            int index = 0;
            foreach(ushort value in values)
            {
                if(index == 0)
                {
                    gcd = value;
                }
                else
                {
                    gcd = MathUtils.Gcd(gcd, value);
                }
                index++;
            }
            return gcd;
        }

        /// <summary>
        /// Finds the least common multiple (LCM) of a sequence of values.
        /// </summary>
        /// <param name="values">The sequence of values.</param>
        /// <returns>
        /// The LCM of the sequence or zero if the sequence is empty.
        /// </returns>
        /// <remarks>
        /// <para>
        /// <paramref name="values" /> is enumerated only once.
        /// </para>
        /// </remarks>
        /// <exception cref="ArgumentNullException"> <paramref name="values" /> is null. </exception>
        [CLSCompliant(false)]
        public static ushort Lcm (this IEnumerable<ushort> values)
        {
            if(values == null)
            {
                throw new ArgumentNullException(nameof(values));
            }

            ushort lcm = 0;
            int index = 0;
            foreach(ushort value in values)
            {
                if(index == 0)
                {
                    lcm = value;
                }
                else
                {
                    lcm = MathUtils.Lcm(lcm, value);
                }
                index++;
            }
            return lcm;
        }

        /// <summary>
        /// Converts a sequence of values to floats.
        /// </summary>
        /// <param name="values">The sequence of values.</param>
        /// <returns>
        /// The list of values as floats.
        /// An empty list is returned if the sequence is empty.
        /// </returns>
        /// <remarks>
        /// <para>
        /// <paramref name="values" /> is enumerated only once.
        /// </para>
        /// </remarks>
        /// <exception cref="ArgumentNullException"> <paramref name="values" /> is null. </exception>
        [CLSCompliant(false)]
        public static IEnumerable<float> AsFloat (this IEnumerable<ushort> values)
        {
            if(values == null)
            {
                throw new ArgumentNullException(nameof(values));
            }

            return values.Select(x => (float)x);
        }

        /// <summary>
        /// Converts a sequence of values to doubles.
        /// </summary>
        /// <param name="values">The sequence of values.</param>
        /// <returns>
        /// The list of values as doubles.
        /// An empty list is returned if the sequence is empty.
        /// </returns>
        /// <remarks>
        /// <para>
        /// <paramref name="values" /> is enumerated only once.
        /// </para>
        /// </remarks>
        /// <exception cref="ArgumentNullException"> <paramref name="values" /> is null. </exception>
        [CLSCompliant(false)]
        public static IEnumerable<double> AsDouble (this IEnumerable<ushort> values)
        {
            if(values == null)
            {
                throw new ArgumentNullException(nameof(values));
            }

            return values.Select(x => (double)x);
        }

        /// <summary>
        /// Gets statistics for a sequence of values.
        /// </summary>
        /// <param name="values">The sequence of values.</param>
        /// <returns>
        /// The statistics for the sequence.
        /// </returns>
        /// <remarks>
        /// <para>
        /// <paramref name="values" /> is enumerated only once.
        /// </para>
        /// </remarks>
        /// <exception cref="ArgumentNullException"> <paramref name="values" /> is null. </exception>
        [CLSCompliant(false)]
        public static StatisticValues GetStatistics (this IEnumerable<ushort> values) => new StatisticValues(values.AsDouble());

        /// <summary>
        /// Gets each digit of a value as separate integers.
        /// </summary>
        /// <param name="value">The value.</param>
        /// <returns>
        /// The list of digits.
        /// The most-significant digit comes first, the least-significant digit comes last.
        /// </returns>
        [CLSCompliant(false)]
        public static List<int> GetDigits (this ushort value)
        {
            string stringValue = value.ToString("D", CultureInfo.InvariantCulture);
            List<int> digits = new List<int>(20);
            foreach(char chr in stringValue)
            {
                if((chr >= 48) && (chr <= 57))
                {
                    digits.Add(chr - 48);
                }
            }
            return digits;
        }

        /// <summary>
        /// Clamps a value between an inclusive minimum and maximum value.
        /// </summary>
        /// <param name="value">The value.</param>
        /// <param name="min">The lowest possible value.</param>
        /// <param name="max">The highest possible value.</param>
        /// <returns>
        /// The clamped value.
        /// </returns>
        [CLSCompliant(true)]
        public static int Clamp (this int value, int min, int max)
        {
            if(value < min)
            {
                return min;
            }
            if(value > max)
            {
                return max;
            }
            return value;
        }

        /// <summary>
        /// Clamps a value to an inclusive minimum value.
        /// </summary>
        /// <param name="value">The value.</param>
        /// <param name="min">The lowest possible value.</param>
        /// <returns>
        /// The clamped value.
        /// </returns>
        [CLSCompliant(true)]
        public static int ClampMin (this int value, int min)
        {
            if(value < min)
            {
                return min;
            }
            return value;
        }

        /// <summary>
        /// Clamps a value to an inclusive maximum value.
        /// </summary>
        /// <param name="value">The value.</param>
        /// <param name="max">The highest possible value.</param>
        /// <returns>
        /// The clamped value.
        /// </returns>
        [CLSCompliant(true)]
        public static int ClampMax (this int value, int max)
        {
            if(value > max)
            {
                return max;
            }
            return value;
        }

        /// <summary>
        /// Quantizes a value to the nearest value of a multiple.
        /// </summary>
        /// <param name="value">The value.</param>
        /// <param name="multiple">The multiple.</param>
        /// <returns>
        /// The quantized value.
        /// </returns>
        /// <remarks>
        /// <para>
        /// <see cref="MidpointRounding.ToEven"/> is used for <see cref="MidpointRounding"/>.
        /// </para>
        /// </remarks>
        [CLSCompliant(true)]
        public static int Quantize (this int value, int multiple) => (int)(Math.Round((double)value / (double)multiple) * (double)multiple);

        /// <summary>
        /// Quantizes a value to the nearest value of a multiple.
        /// </summary>
        /// <param name="value">The value.</param>
        /// <param name="multiple">The multiple.</param>
        /// <param name="rounding">The kind of rounding to use.</param>
        /// <returns>
        /// The quantized value.
        /// </returns>
        [CLSCompliant(true)]
        public static int Quantize (this int value, int multiple, MidpointRounding rounding) => (int)(Math.Round((double)value / (double)multiple, rounding) * (double)multiple);

        /// <summary>
        /// Finds the greatest common divisor (GCD) of two values.
        /// </summary>
        /// <param name="x">The first value.</param>
        /// <param name="y">The second value.</param>
        /// <returns>
        /// The clamped value.
        /// </returns>
        [CLSCompliant(true)]
        public static int Gcd (this int x, int y) => MathUtils.Gcd(x, y);

        /// <summary>
        /// Finds the least common multiple (LCM) of two values.
        /// </summary>
        /// <param name="x">The first value.</param>
        /// <param name="y">The second value.</param>
        /// <returns>
        /// The clamped value.
        /// </returns>
        [CLSCompliant(true)]
        public static int Lcm (this int x, int y) => MathUtils.Lcm(x, y);

        /// <summary>
        /// Gets the sum from a sequence of values.
        /// </summary>
        /// <param name="values">The sequence of values.</param>
        /// <returns>
        /// The sum of the sequence.
        /// </returns>
        /// <remarks>
        /// <para>
        /// <paramref name="values" /> is enumerated only once.
        /// </para>
        /// </remarks>
        /// <exception cref="ArgumentNullException"> <paramref name="values" /> is null. </exception>
        [CLSCompliant(true)]
        public static int Sum (this IEnumerable<int> values)
        {
            if(values == null)
            {
                throw new ArgumentNullException(nameof(values));
            }

            int sum = 0;
            foreach(int value in values)
            {
                sum += value;
            }
            return sum;
        }

        /// <summary>
        /// Gets the minimum value from a sequence of values.
        /// </summary>
        /// <param name="values">The sequence of values.</param>
        /// <returns>
        /// The minimum value of the sequence.
        /// </returns>
        /// <remarks>
        /// <para>
        /// <paramref name="values" /> is enumerated only once.
        /// </para>
        /// </remarks>
        /// <exception cref="ArgumentNullException"> <paramref name="values" /> is null. </exception>
        [CLSCompliant(true)]
        public static int Min (this IEnumerable<int> values)
        {
            if(values == null)
            {
                throw new ArgumentNullException(nameof(values));
            }

            int min = int.MaxValue;
            foreach(int value in values)
            {
                if(value < min)
                {
                    min = value;
                }
            }
            return min;
        }

        /// <summary>
        /// Gets the maximum value from a sequence of values.
        /// </summary>
        /// <param name="values">The sequence of values.</param>
        /// <returns>
        /// The maximum value of the sequence.
        /// </returns>
        /// <remarks>
        /// <para>
        /// <paramref name="values" /> is enumerated only once.
        /// </para>
        /// </remarks>
        /// <exception cref="ArgumentNullException"> <paramref name="values" /> is null. </exception>
        [CLSCompliant(true)]
        public static int Max (this IEnumerable<int> values)
        {
            if(values == null)
            {
                throw new ArgumentNullException(nameof(values));
            }

            int max = int.MinValue;
            foreach(int value in values)
            {
                if(value > max)
                {
                    max = value;
                }
            }
            return max;
        }

        /// <summary>
        /// Finds the greatest common divisor (GCD) of a sequence of values.
        /// </summary>
        /// <param name="values">The sequence of values.</param>
        /// <returns>
        /// The GCD of the sequence or zero if the sequence is empty.
        /// </returns>
        /// <remarks>
        /// <para>
        /// <paramref name="values" /> is enumerated only once.
        /// </para>
        /// </remarks>
        /// <exception cref="ArgumentNullException"> <paramref name="values" /> is null. </exception>
        [CLSCompliant(true)]
        public static int Gcd (this IEnumerable<int> values)
        {
            if(values == null)
            {
                throw new ArgumentNullException(nameof(values));
            }

            int gcd = 0;
            int index = 0;
            foreach(int value in values)
            {
                if(index == 0)
                {
                    gcd = value;
                }
                else
                {
                    gcd = MathUtils.Gcd(gcd, value);
                }
                index++;
            }
            return gcd;
        }

        /// <summary>
        /// Finds the least common multiple (LCM) of a sequence of values.
        /// </summary>
        /// <param name="values">The sequence of values.</param>
        /// <returns>
        /// The LCM of the sequence or zero if the sequence is empty.
        /// </returns>
        /// <remarks>
        /// <para>
        /// <paramref name="values" /> is enumerated only once.
        /// </para>
        /// </remarks>
        /// <exception cref="ArgumentNullException"> <paramref name="values" /> is null. </exception>
        [CLSCompliant(true)]
        public static int Lcm (this IEnumerable<int> values)
        {
            if(values == null)
            {
                throw new ArgumentNullException(nameof(values));
            }

            int lcm = 0;
            int index = 0;
            foreach(int value in values)
            {
                if(index == 0)
                {
                    lcm = value;
                }
                else
                {
                    lcm = MathUtils.Lcm(lcm, value);
                }
                index++;
            }
            return lcm;
        }

        /// <summary>
        /// Converts a sequence of values to floats.
        /// </summary>
        /// <param name="values">The sequence of values.</param>
        /// <returns>
        /// The list of values as floats.
        /// An empty list is returned if the sequence is empty.
        /// </returns>
        /// <remarks>
        /// <para>
        /// <paramref name="values" /> is enumerated only once.
        /// </para>
        /// </remarks>
        /// <exception cref="ArgumentNullException"> <paramref name="values" /> is null. </exception>
        [CLSCompliant(true)]
        public static IEnumerable<float> AsFloat (this IEnumerable<int> values)
        {
            if(values == null)
            {
                throw new ArgumentNullException(nameof(values));
            }

            return values.Select(x => (float)x);
        }

        /// <summary>
        /// Converts a sequence of values to doubles.
        /// </summary>
        /// <param name="values">The sequence of values.</param>
        /// <returns>
        /// The list of values as doubles.
        /// An empty list is returned if the sequence is empty.
        /// </returns>
        /// <remarks>
        /// <para>
        /// <paramref name="values" /> is enumerated only once.
        /// </para>
        /// </remarks>
        /// <exception cref="ArgumentNullException"> <paramref name="values" /> is null. </exception>
        [CLSCompliant(true)]
        public static IEnumerable<double> AsDouble (this IEnumerable<int> values)
        {
            if(values == null)
            {
                throw new ArgumentNullException(nameof(values));
            }

            return values.Select(x => (double)x);
        }

        /// <summary>
        /// Gets statistics for a sequence of values.
        /// </summary>
        /// <param name="values">The sequence of values.</param>
        /// <returns>
        /// The statistics for the sequence.
        /// </returns>
        /// <remarks>
        /// <para>
        /// <paramref name="values" /> is enumerated only once.
        /// </para>
        /// </remarks>
        /// <exception cref="ArgumentNullException"> <paramref name="values" /> is null. </exception>
        [CLSCompliant(true)]
        public static StatisticValues GetStatistics (this IEnumerable<int> values) => new StatisticValues(values.AsDouble());

        /// <summary>
        /// Gets each digit of a value as separate integers.
        /// </summary>
        /// <param name="value">The value.</param>
        /// <returns>
        /// The list of digits.
        /// The most-significant digit comes first, the least-significant digit comes last.
        /// </returns>
        [CLSCompliant(true)]
        public static List<int> GetDigits (this int value)
        {
            string stringValue = value.ToString("D", CultureInfo.InvariantCulture);
            List<int> digits = new List<int>(20);
            foreach(char chr in stringValue)
            {
                if((chr >= 48) && (chr <= 57))
                {
                    digits.Add(chr - 48);
                }
            }
            return digits;
        }

        /// <summary>
        /// Clamps a value between an inclusive minimum and maximum value.
        /// </summary>
        /// <param name="value">The value.</param>
        /// <param name="min">The lowest possible value.</param>
        /// <param name="max">The highest possible value.</param>
        /// <returns>
        /// The clamped value.
        /// </returns>
        [CLSCompliant(false)]
        public static uint Clamp (this uint value, uint min, uint max)
        {
            if(value < min)
            {
                return min;
            }
            if(value > max)
            {
                return max;
            }
            return value;
        }

        /// <summary>
        /// Clamps a value to an inclusive minimum value.
        /// </summary>
        /// <param name="value">The value.</param>
        /// <param name="min">The lowest possible value.</param>
        /// <returns>
        /// The clamped value.
        /// </returns>
        [CLSCompliant(false)]
        public static uint ClampMin (this uint value, uint min)
        {
            if(value < min)
            {
                return min;
            }
            return value;
        }

        /// <summary>
        /// Clamps a value to an inclusive maximum value.
        /// </summary>
        /// <param name="value">The value.</param>
        /// <param name="max">The highest possible value.</param>
        /// <returns>
        /// The clamped value.
        /// </returns>
        [CLSCompliant(false)]
        public static uint ClampMax (this uint value, uint max)
        {
            if(value > max)
            {
                return max;
            }
            return value;
        }

        /// <summary>
        /// Quantizes a value to the nearest value of a multiple.
        /// </summary>
        /// <param name="value">The value.</param>
        /// <param name="multiple">The multiple.</param>
        /// <returns>
        /// The quantized value.
        /// </returns>
        /// <remarks>
        /// <para>
        /// <see cref="MidpointRounding.ToEven"/> is used for <see cref="MidpointRounding"/>.
        /// </para>
        /// </remarks>
        [CLSCompliant(false)]
        public static uint Quantize (this uint value, uint multiple) => (uint)(Math.Round((double)value / (double)multiple) * (double)multiple);

        /// <summary>
        /// Quantizes a value to the nearest value of a multiple.
        /// </summary>
        /// <param name="value">The value.</param>
        /// <param name="multiple">The multiple.</param>
        /// <param name="rounding">The kind of rounding to use.</param>
        /// <returns>
        /// The quantized value.
        /// </returns>
        [CLSCompliant(false)]
        public static uint Quantize (this uint value, uint multiple, MidpointRounding rounding) => (uint)(Math.Round((double)value / (double)multiple, rounding) * (double)multiple);

        /// <summary>
        /// Finds the greatest common divisor (GCD) of two values.
        /// </summary>
        /// <param name="x">The first value.</param>
        /// <param name="y">The second value.</param>
        /// <returns>
        /// The clamped value.
        /// </returns>
        [CLSCompliant(false)]
        public static uint Gcd (this uint x, uint y) => MathUtils.Gcd(x, y);

        /// <summary>
        /// Finds the least common multiple (LCM) of two values.
        /// </summary>
        /// <param name="x">The first value.</param>
        /// <param name="y">The second value.</param>
        /// <returns>
        /// The clamped value.
        /// </returns>
        [CLSCompliant(false)]
        public static uint Lcm (this uint x, uint y) => MathUtils.Lcm(x, y);

        /// <summary>
        /// Gets the sum from a sequence of values.
        /// </summary>
        /// <param name="values">The sequence of values.</param>
        /// <returns>
        /// The sum of the sequence.
        /// </returns>
        /// <remarks>
        /// <para>
        /// <paramref name="values" /> is enumerated only once.
        /// </para>
        /// </remarks>
        /// <exception cref="ArgumentNullException"> <paramref name="values" /> is null. </exception>
        [CLSCompliant(false)]
        public static uint Sum (this IEnumerable<uint> values)
        {
            if(values == null)
            {
                throw new ArgumentNullException(nameof(values));
            }

            uint sum = 0;
            foreach(uint value in values)
            {
                sum += value;
            }
            return sum;
        }

        /// <summary>
        /// Gets the minimum value from a sequence of values.
        /// </summary>
        /// <param name="values">The sequence of values.</param>
        /// <returns>
        /// The minimum value of the sequence.
        /// </returns>
        /// <remarks>
        /// <para>
        /// <paramref name="values" /> is enumerated only once.
        /// </para>
        /// </remarks>
        /// <exception cref="ArgumentNullException"> <paramref name="values" /> is null. </exception>
        [CLSCompliant(false)]
        public static uint Min (this IEnumerable<uint> values)
        {
            if(values == null)
            {
                throw new ArgumentNullException(nameof(values));
            }

            uint min = uint.MaxValue;
            foreach(uint value in values)
            {
                if(value < min)
                {
                    min = value;
                }
            }
            return min;
        }

        /// <summary>
        /// Gets the maximum value from a sequence of values.
        /// </summary>
        /// <param name="values">The sequence of values.</param>
        /// <returns>
        /// The maximum value of the sequence.
        /// </returns>
        /// <remarks>
        /// <para>
        /// <paramref name="values" /> is enumerated only once.
        /// </para>
        /// </remarks>
        /// <exception cref="ArgumentNullException"> <paramref name="values" /> is null. </exception>
        [CLSCompliant(false)]
        public static uint Max (this IEnumerable<uint> values)
        {
            if(values == null)
            {
                throw new ArgumentNullException(nameof(values));
            }

            uint max = uint.MinValue;
            foreach(uint value in values)
            {
                if(value > max)
                {
                    max = value;
                }
            }
            return max;
        }

        /// <summary>
        /// Finds the greatest common divisor (GCD) of a sequence of values.
        /// </summary>
        /// <param name="values">The sequence of values.</param>
        /// <returns>
        /// The GCD of the sequence or zero if the sequence is empty.
        /// </returns>
        /// <remarks>
        /// <para>
        /// <paramref name="values" /> is enumerated only once.
        /// </para>
        /// </remarks>
        /// <exception cref="ArgumentNullException"> <paramref name="values" /> is null. </exception>
        [CLSCompliant(false)]
        public static uint Gcd (this IEnumerable<uint> values)
        {
            if(values == null)
            {
                throw new ArgumentNullException(nameof(values));
            }

            uint gcd = 0;
            int index = 0;
            foreach(uint value in values)
            {
                if(index == 0)
                {
                    gcd = value;
                }
                else
                {
                    gcd = MathUtils.Gcd(gcd, value);
                }
                index++;
            }
            return gcd;
        }

        /// <summary>
        /// Finds the least common multiple (LCM) of a sequence of values.
        /// </summary>
        /// <param name="values">The sequence of values.</param>
        /// <returns>
        /// The LCM of the sequence or zero if the sequence is empty.
        /// </returns>
        /// <remarks>
        /// <para>
        /// <paramref name="values" /> is enumerated only once.
        /// </para>
        /// </remarks>
        /// <exception cref="ArgumentNullException"> <paramref name="values" /> is null. </exception>
        [CLSCompliant(false)]
        public static uint Lcm (this IEnumerable<uint> values)
        {
            if(values == null)
            {
                throw new ArgumentNullException(nameof(values));
            }

            uint lcm = 0;
            int index = 0;
            foreach(uint value in values)
            {
                if(index == 0)
                {
                    lcm = value;
                }
                else
                {
                    lcm = MathUtils.Lcm(lcm, value);
                }
                index++;
            }
            return lcm;
        }

        /// <summary>
        /// Converts a sequence of values to floats.
        /// </summary>
        /// <param name="values">The sequence of values.</param>
        /// <returns>
        /// The list of values as floats.
        /// An empty list is returned if the sequence is empty.
        /// </returns>
        /// <remarks>
        /// <para>
        /// <paramref name="values" /> is enumerated only once.
        /// </para>
        /// </remarks>
        /// <exception cref="ArgumentNullException"> <paramref name="values" /> is null. </exception>
        [CLSCompliant(false)]
        public static IEnumerable<float> AsFloat (this IEnumerable<uint> values)
        {
            if(values == null)
            {
                throw new ArgumentNullException(nameof(values));
            }

            return values.Select(x => (float)x);
        }

        /// <summary>
        /// Converts a sequence of values to doubles.
        /// </summary>
        /// <param name="values">The sequence of values.</param>
        /// <returns>
        /// The list of values as doubles.
        /// An empty list is returned if the sequence is empty.
        /// </returns>
        /// <remarks>
        /// <para>
        /// <paramref name="values" /> is enumerated only once.
        /// </para>
        /// </remarks>
        /// <exception cref="ArgumentNullException"> <paramref name="values" /> is null. </exception>
        [CLSCompliant(false)]
        public static IEnumerable<double> AsDouble (this IEnumerable<uint> values)
        {
            if(values == null)
            {
                throw new ArgumentNullException(nameof(values));
            }

            return values.Select(x => (double)x);
        }

        /// <summary>
        /// Gets statistics for a sequence of values.
        /// </summary>
        /// <param name="values">The sequence of values.</param>
        /// <returns>
        /// The statistics for the sequence.
        /// </returns>
        /// <remarks>
        /// <para>
        /// <paramref name="values" /> is enumerated only once.
        /// </para>
        /// </remarks>
        /// <exception cref="ArgumentNullException"> <paramref name="values" /> is null. </exception>
        [CLSCompliant(false)]
        public static StatisticValues GetStatistics (this IEnumerable<uint> values) => new StatisticValues(values.AsDouble());

        /// <summary>
        /// Gets each digit of a value as separate integers.
        /// </summary>
        /// <param name="value">The value.</param>
        /// <returns>
        /// The list of digits.
        /// The most-significant digit comes first, the least-significant digit comes last.
        /// </returns>
        [CLSCompliant(false)]
        public static List<int> GetDigits (this uint value)
        {
            string stringValue = value.ToString("D", CultureInfo.InvariantCulture);
            List<int> digits = new List<int>(20);
            foreach(char chr in stringValue)
            {
                if((chr >= 48) && (chr <= 57))
                {
                    digits.Add(chr - 48);
                }
            }
            return digits;
        }

        /// <summary>
        /// Clamps a value between an inclusive minimum and maximum value.
        /// </summary>
        /// <param name="value">The value.</param>
        /// <param name="min">The lowest possible value.</param>
        /// <param name="max">The highest possible value.</param>
        /// <returns>
        /// The clamped value.
        /// </returns>
        [CLSCompliant(true)]
        public static long Clamp (this long value, long min, long max)
        {
            if(value < min)
            {
                return min;
            }
            if(value > max)
            {
                return max;
            }
            return value;
        }

        /// <summary>
        /// Clamps a value to an inclusive minimum value.
        /// </summary>
        /// <param name="value">The value.</param>
        /// <param name="min">The lowest possible value.</param>
        /// <returns>
        /// The clamped value.
        /// </returns>
        [CLSCompliant(true)]
        public static long ClampMin (this long value, long min)
        {
            if(value < min)
            {
                return min;
            }
            return value;
        }

        /// <summary>
        /// Clamps a value to an inclusive maximum value.
        /// </summary>
        /// <param name="value">The value.</param>
        /// <param name="max">The highest possible value.</param>
        /// <returns>
        /// The clamped value.
        /// </returns>
        [CLSCompliant(true)]
        public static long ClampMax (this long value, long max)
        {
            if(value > max)
            {
                return max;
            }
            return value;
        }

        /// <summary>
        /// Quantizes a value to the nearest value of a multiple.
        /// </summary>
        /// <param name="value">The value.</param>
        /// <param name="multiple">The multiple.</param>
        /// <returns>
        /// The quantized value.
        /// </returns>
        /// <remarks>
        /// <para>
        /// <see cref="MidpointRounding.ToEven"/> is used for <see cref="MidpointRounding"/>.
        /// </para>
        /// </remarks>
        [CLSCompliant(true)]
        public static long Quantize (this long value, long multiple) => (long)(Math.Round((double)value / (double)multiple) * (double)multiple);

        /// <summary>
        /// Quantizes a value to the nearest value of a multiple.
        /// </summary>
        /// <param name="value">The value.</param>
        /// <param name="multiple">The multiple.</param>
        /// <param name="rounding">The kind of rounding to use.</param>
        /// <returns>
        /// The quantized value.
        /// </returns>
        [CLSCompliant(true)]
        public static long Quantize (this long value, long multiple, MidpointRounding rounding) => (long)(Math.Round((double)value / (double)multiple, rounding) * (double)multiple);

        /// <summary>
        /// Finds the greatest common divisor (GCD) of two values.
        /// </summary>
        /// <param name="x">The first value.</param>
        /// <param name="y">The second value.</param>
        /// <returns>
        /// The clamped value.
        /// </returns>
        [CLSCompliant(true)]
        public static long Gcd (this long x, long y) => MathUtils.Gcd(x, y);

        /// <summary>
        /// Finds the least common multiple (LCM) of two values.
        /// </summary>
        /// <param name="x">The first value.</param>
        /// <param name="y">The second value.</param>
        /// <returns>
        /// The clamped value.
        /// </returns>
        [CLSCompliant(true)]
        public static long Lcm (this long x, long y) => MathUtils.Lcm(x, y);

        /// <summary>
        /// Gets the sum from a sequence of values.
        /// </summary>
        /// <param name="values">The sequence of values.</param>
        /// <returns>
        /// The sum of the sequence.
        /// </returns>
        /// <remarks>
        /// <para>
        /// <paramref name="values" /> is enumerated only once.
        /// </para>
        /// </remarks>
        /// <exception cref="ArgumentNullException"> <paramref name="values" /> is null. </exception>
        [CLSCompliant(true)]
        public static long Sum (this IEnumerable<long> values)
        {
            if(values == null)
            {
                throw new ArgumentNullException(nameof(values));
            }

            long sum = 0;
            foreach(long value in values)
            {
                sum += value;
            }
            return sum;
        }

        /// <summary>
        /// Gets the minimum value from a sequence of values.
        /// </summary>
        /// <param name="values">The sequence of values.</param>
        /// <returns>
        /// The minimum value of the sequence.
        /// </returns>
        /// <remarks>
        /// <para>
        /// <paramref name="values" /> is enumerated only once.
        /// </para>
        /// </remarks>
        /// <exception cref="ArgumentNullException"> <paramref name="values" /> is null. </exception>
        [CLSCompliant(true)]
        public static long Min (this IEnumerable<long> values)
        {
            if(values == null)
            {
                throw new ArgumentNullException(nameof(values));
            }

            long min = long.MaxValue;
            foreach(long value in values)
            {
                if(value < min)
                {
                    min = value;
                }
            }
            return min;
        }

        /// <summary>
        /// Gets the maximum value from a sequence of values.
        /// </summary>
        /// <param name="values">The sequence of values.</param>
        /// <returns>
        /// The maximum value of the sequence.
        /// </returns>
        /// <remarks>
        /// <para>
        /// <paramref name="values" /> is enumerated only once.
        /// </para>
        /// </remarks>
        /// <exception cref="ArgumentNullException"> <paramref name="values" /> is null. </exception>
        [CLSCompliant(true)]
        public static long Max (this IEnumerable<long> values)
        {
            if(values == null)
            {
                throw new ArgumentNullException(nameof(values));
            }

            long max = long.MinValue;
            foreach(long value in values)
            {
                if(value > max)
                {
                    max = value;
                }
            }
            return max;
        }

        /// <summary>
        /// Finds the greatest common divisor (GCD) of a sequence of values.
        /// </summary>
        /// <param name="values">The sequence of values.</param>
        /// <returns>
        /// The GCD of the sequence or zero if the sequence is empty.
        /// </returns>
        /// <remarks>
        /// <para>
        /// <paramref name="values" /> is enumerated only once.
        /// </para>
        /// </remarks>
        /// <exception cref="ArgumentNullException"> <paramref name="values" /> is null. </exception>
        [CLSCompliant(true)]
        public static long Gcd (this IEnumerable<long> values)
        {
            if(values == null)
            {
                throw new ArgumentNullException(nameof(values));
            }

            long gcd = 0;
            int index = 0;
            foreach(long value in values)
            {
                if(index == 0)
                {
                    gcd = value;
                }
                else
                {
                    gcd = MathUtils.Gcd(gcd, value);
                }
                index++;
            }
            return gcd;
        }

        /// <summary>
        /// Finds the least common multiple (LCM) of a sequence of values.
        /// </summary>
        /// <param name="values">The sequence of values.</param>
        /// <returns>
        /// The LCM of the sequence or zero if the sequence is empty.
        /// </returns>
        /// <remarks>
        /// <para>
        /// <paramref name="values" /> is enumerated only once.
        /// </para>
        /// </remarks>
        /// <exception cref="ArgumentNullException"> <paramref name="values" /> is null. </exception>
        [CLSCompliant(true)]
        public static long Lcm (this IEnumerable<long> values)
        {
            if(values == null)
            {
                throw new ArgumentNullException(nameof(values));
            }

            long lcm = 0;
            int index = 0;
            foreach(long value in values)
            {
                if(index == 0)
                {
                    lcm = value;
                }
                else
                {
                    lcm = MathUtils.Lcm(lcm, value);
                }
                index++;
            }
            return lcm;
        }

        /// <summary>
        /// Converts a sequence of values to floats.
        /// </summary>
        /// <param name="values">The sequence of values.</param>
        /// <returns>
        /// The list of values as floats.
        /// An empty list is returned if the sequence is empty.
        /// </returns>
        /// <remarks>
        /// <para>
        /// <paramref name="values" /> is enumerated only once.
        /// </para>
        /// </remarks>
        /// <exception cref="ArgumentNullException"> <paramref name="values" /> is null. </exception>
        [CLSCompliant(true)]
        public static IEnumerable<float> AsFloat (this IEnumerable<long> values)
        {
            if(values == null)
            {
                throw new ArgumentNullException(nameof(values));
            }

            return values.Select(x => (float)x);
        }

        /// <summary>
        /// Converts a sequence of values to doubles.
        /// </summary>
        /// <param name="values">The sequence of values.</param>
        /// <returns>
        /// The list of values as doubles.
        /// An empty list is returned if the sequence is empty.
        /// </returns>
        /// <remarks>
        /// <para>
        /// <paramref name="values" /> is enumerated only once.
        /// </para>
        /// </remarks>
        /// <exception cref="ArgumentNullException"> <paramref name="values" /> is null. </exception>
        [CLSCompliant(true)]
        public static IEnumerable<double> AsDouble (this IEnumerable<long> values)
        {
            if(values == null)
            {
                throw new ArgumentNullException(nameof(values));
            }

            return values.Select(x => (double)x);
        }

        /// <summary>
        /// Gets statistics for a sequence of values.
        /// </summary>
        /// <param name="values">The sequence of values.</param>
        /// <returns>
        /// The statistics for the sequence.
        /// </returns>
        /// <remarks>
        /// <para>
        /// <paramref name="values" /> is enumerated only once.
        /// </para>
        /// </remarks>
        /// <exception cref="ArgumentNullException"> <paramref name="values" /> is null. </exception>
        [CLSCompliant(true)]
        public static StatisticValues GetStatistics (this IEnumerable<long> values) => new StatisticValues(values.AsDouble());

        /// <summary>
        /// Gets each digit of a value as separate integers.
        /// </summary>
        /// <param name="value">The value.</param>
        /// <returns>
        /// The list of digits.
        /// The most-significant digit comes first, the least-significant digit comes last.
        /// </returns>
        [CLSCompliant(true)]
        public static List<int> GetDigits (this long value)
        {
            string stringValue = value.ToString("D", CultureInfo.InvariantCulture);
            List<int> digits = new List<int>(20);
            foreach(char chr in stringValue)
            {
                if((chr >= 48) && (chr <= 57))
                {
                    digits.Add(chr - 48);
                }
            }
            return digits;
        }

        /// <summary>
        /// Clamps a value between an inclusive minimum and maximum value.
        /// </summary>
        /// <param name="value">The value.</param>
        /// <param name="min">The lowest possible value.</param>
        /// <param name="max">The highest possible value.</param>
        /// <returns>
        /// The clamped value.
        /// </returns>
        [CLSCompliant(false)]
        public static ulong Clamp (this ulong value, ulong min, ulong max)
        {
            if(value < min)
            {
                return min;
            }
            if(value > max)
            {
                return max;
            }
            return value;
        }

        /// <summary>
        /// Clamps a value to an inclusive minimum value.
        /// </summary>
        /// <param name="value">The value.</param>
        /// <param name="min">The lowest possible value.</param>
        /// <returns>
        /// The clamped value.
        /// </returns>
        [CLSCompliant(false)]
        public static ulong ClampMin (this ulong value, ulong min)
        {
            if(value < min)
            {
                return min;
            }
            return value;
        }

        /// <summary>
        /// Clamps a value to an inclusive maximum value.
        /// </summary>
        /// <param name="value">The value.</param>
        /// <param name="max">The highest possible value.</param>
        /// <returns>
        /// The clamped value.
        /// </returns>
        [CLSCompliant(false)]
        public static ulong ClampMax (this ulong value, ulong max)
        {
            if(value > max)
            {
                return max;
            }
            return value;
        }

        /// <summary>
        /// Quantizes a value to the nearest value of a multiple.
        /// </summary>
        /// <param name="value">The value.</param>
        /// <param name="multiple">The multiple.</param>
        /// <returns>
        /// The quantized value.
        /// </returns>
        /// <remarks>
        /// <para>
        /// <see cref="MidpointRounding.ToEven"/> is used for <see cref="MidpointRounding"/>.
        /// </para>
        /// </remarks>
        [CLSCompliant(false)]
        public static ulong Quantize (this ulong value, ulong multiple) => (ulong)(Math.Round((double)value / (double)multiple) * (double)multiple);

        /// <summary>
        /// Quantizes a value to the nearest value of a multiple.
        /// </summary>
        /// <param name="value">The value.</param>
        /// <param name="multiple">The multiple.</param>
        /// <param name="rounding">The kind of rounding to use.</param>
        /// <returns>
        /// The quantized value.
        /// </returns>
        [CLSCompliant(false)]
        public static ulong Quantize (this ulong value, ulong multiple, MidpointRounding rounding) => (ulong)(Math.Round((double)value / (double)multiple, rounding) * (double)multiple);

        /// <summary>
        /// Finds the greatest common divisor (GCD) of two values.
        /// </summary>
        /// <param name="x">The first value.</param>
        /// <param name="y">The second value.</param>
        /// <returns>
        /// The clamped value.
        /// </returns>
        [CLSCompliant(false)]
        public static ulong Gcd (this ulong x, ulong y) => MathUtils.Gcd(x, y);

        /// <summary>
        /// Finds the least common multiple (LCM) of two values.
        /// </summary>
        /// <param name="x">The first value.</param>
        /// <param name="y">The second value.</param>
        /// <returns>
        /// The clamped value.
        /// </returns>
        [CLSCompliant(false)]
        public static ulong Lcm (this ulong x, ulong y) => MathUtils.Lcm(x, y);

        /// <summary>
        /// Gets the sum from a sequence of values.
        /// </summary>
        /// <param name="values">The sequence of values.</param>
        /// <returns>
        /// The sum of the sequence.
        /// </returns>
        /// <remarks>
        /// <para>
        /// <paramref name="values" /> is enumerated only once.
        /// </para>
        /// </remarks>
        /// <exception cref="ArgumentNullException"> <paramref name="values" /> is null. </exception>
        [CLSCompliant(false)]
        public static ulong Sum (this IEnumerable<ulong> values)
        {
            if(values == null)
            {
                throw new ArgumentNullException(nameof(values));
            }

            ulong sum = 0;
            foreach(ulong value in values)
            {
                sum += value;
            }
            return sum;
        }

        /// <summary>
        /// Gets the minimum value from a sequence of values.
        /// </summary>
        /// <param name="values">The sequence of values.</param>
        /// <returns>
        /// The minimum value of the sequence.
        /// </returns>
        /// <remarks>
        /// <para>
        /// <paramref name="values" /> is enumerated only once.
        /// </para>
        /// </remarks>
        /// <exception cref="ArgumentNullException"> <paramref name="values" /> is null. </exception>
        [CLSCompliant(false)]
        public static ulong Min (this IEnumerable<ulong> values)
        {
            if(values == null)
            {
                throw new ArgumentNullException(nameof(values));
            }

            ulong min = ulong.MaxValue;
            foreach(ulong value in values)
            {
                if(value < min)
                {
                    min = value;
                }
            }
            return min;
        }

        /// <summary>
        /// Gets the maximum value from a sequence of values.
        /// </summary>
        /// <param name="values">The sequence of values.</param>
        /// <returns>
        /// The maximum value of the sequence.
        /// </returns>
        /// <remarks>
        /// <para>
        /// <paramref name="values" /> is enumerated only once.
        /// </para>
        /// </remarks>
        /// <exception cref="ArgumentNullException"> <paramref name="values" /> is null. </exception>
        [CLSCompliant(false)]
        public static ulong Max (this IEnumerable<ulong> values)
        {
            if(values == null)
            {
                throw new ArgumentNullException(nameof(values));
            }

            ulong max = ulong.MinValue;
            foreach(ulong value in values)
            {
                if(value > max)
                {
                    max = value;
                }
            }
            return max;
        }

        /// <summary>
        /// Finds the greatest common divisor (GCD) of a sequence of values.
        /// </summary>
        /// <param name="values">The sequence of values.</param>
        /// <returns>
        /// The GCD of the sequence or zero if the sequence is empty.
        /// </returns>
        /// <remarks>
        /// <para>
        /// <paramref name="values" /> is enumerated only once.
        /// </para>
        /// </remarks>
        /// <exception cref="ArgumentNullException"> <paramref name="values" /> is null. </exception>
        [CLSCompliant(false)]
        public static ulong Gcd (this IEnumerable<ulong> values)
        {
            if(values == null)
            {
                throw new ArgumentNullException(nameof(values));
            }

            ulong gcd = 0;
            int index = 0;
            foreach(ulong value in values)
            {
                if(index == 0)
                {
                    gcd = value;
                }
                else
                {
                    gcd = MathUtils.Gcd(gcd, value);
                }
                index++;
            }
            return gcd;
        }

        /// <summary>
        /// Finds the least common multiple (LCM) of a sequence of values.
        /// </summary>
        /// <param name="values">The sequence of values.</param>
        /// <returns>
        /// The LCM of the sequence or zero if the sequence is empty.
        /// </returns>
        /// <remarks>
        /// <para>
        /// <paramref name="values" /> is enumerated only once.
        /// </para>
        /// </remarks>
        /// <exception cref="ArgumentNullException"> <paramref name="values" /> is null. </exception>
        [CLSCompliant(false)]
        public static ulong Lcm (this IEnumerable<ulong> values)
        {
            if(values == null)
            {
                throw new ArgumentNullException(nameof(values));
            }

            ulong lcm = 0;
            int index = 0;
            foreach(ulong value in values)
            {
                if(index == 0)
                {
                    lcm = value;
                }
                else
                {
                    lcm = MathUtils.Lcm(lcm, value);
                }
                index++;
            }
            return lcm;
        }

        /// <summary>
        /// Converts a sequence of values to floats.
        /// </summary>
        /// <param name="values">The sequence of values.</param>
        /// <returns>
        /// The list of values as floats.
        /// An empty list is returned if the sequence is empty.
        /// </returns>
        /// <remarks>
        /// <para>
        /// <paramref name="values" /> is enumerated only once.
        /// </para>
        /// </remarks>
        /// <exception cref="ArgumentNullException"> <paramref name="values" /> is null. </exception>
        [CLSCompliant(false)]
        public static IEnumerable<float> AsFloat (this IEnumerable<ulong> values)
        {
            if(values == null)
            {
                throw new ArgumentNullException(nameof(values));
            }

            return values.Select(x => (float)x);
        }

        /// <summary>
        /// Converts a sequence of values to doubles.
        /// </summary>
        /// <param name="values">The sequence of values.</param>
        /// <returns>
        /// The list of values as doubles.
        /// An empty list is returned if the sequence is empty.
        /// </returns>
        /// <remarks>
        /// <para>
        /// <paramref name="values" /> is enumerated only once.
        /// </para>
        /// </remarks>
        /// <exception cref="ArgumentNullException"> <paramref name="values" /> is null. </exception>
        [CLSCompliant(false)]
        public static IEnumerable<double> AsDouble (this IEnumerable<ulong> values)
        {
            if(values == null)
            {
                throw new ArgumentNullException(nameof(values));
            }

            return values.Select(x => (double)x);
        }

        /// <summary>
        /// Gets statistics for a sequence of values.
        /// </summary>
        /// <param name="values">The sequence of values.</param>
        /// <returns>
        /// The statistics for the sequence.
        /// </returns>
        /// <remarks>
        /// <para>
        /// <paramref name="values" /> is enumerated only once.
        /// </para>
        /// </remarks>
        /// <exception cref="ArgumentNullException"> <paramref name="values" /> is null. </exception>
        [CLSCompliant(false)]
        public static StatisticValues GetStatistics (this IEnumerable<ulong> values) => new StatisticValues(values.AsDouble());

        /// <summary>
        /// Gets each digit of a value as separate integers.
        /// </summary>
        /// <param name="value">The value.</param>
        /// <returns>
        /// The list of digits.
        /// The most-significant digit comes first, the least-significant digit comes last.
        /// </returns>
        [CLSCompliant(false)]
        public static List<int> GetDigits (this ulong value)
        {
            string stringValue = value.ToString("D", CultureInfo.InvariantCulture);
            List<int> digits = new List<int>(20);
            foreach(char chr in stringValue)
            {
                if((chr >= 48) && (chr <= 57))
                {
                    digits.Add(chr - 48);
                }
            }
            return digits;
        }

        /// <summary>
        /// Clamps a value between an inclusive minimum and maximum value.
        /// </summary>
        /// <param name="value">The value.</param>
        /// <param name="min">The lowest possible value.</param>
        /// <param name="max">The highest possible value.</param>
        /// <returns>
        /// The clamped value.
        /// </returns>
        [CLSCompliant(true)]
        public static float Clamp (this float value, float min, float max)
        {
            if(value < min)
            {
                return min;
            }
            if(value > max)
            {
                return max;
            }
            return value;
        }

        /// <summary>
        /// Clamps a value to an inclusive minimum value.
        /// </summary>
        /// <param name="value">The value.</param>
        /// <param name="min">The lowest possible value.</param>
        /// <returns>
        /// The clamped value.
        /// </returns>
        [CLSCompliant(true)]
        public static float ClampMin (this float value, float min)
        {
            if(value < min)
            {
                return min;
            }
            return value;
        }

        /// <summary>
        /// Clamps a value to an inclusive maximum value.
        /// </summary>
        /// <param name="value">The value.</param>
        /// <param name="max">The highest possible value.</param>
        /// <returns>
        /// The clamped value.
        /// </returns>
        [CLSCompliant(true)]
        public static float ClampMax (this float value, float max)
        {
            if(value > max)
            {
                return max;
            }
            return value;
        }

        /// <summary>
        /// Quantizes a value to the nearest value of a multiple.
        /// </summary>
        /// <param name="value">The value.</param>
        /// <param name="multiple">The multiple.</param>
        /// <returns>
        /// The quantized value.
        /// </returns>
        /// <remarks>
        /// <para>
        /// <see cref="MidpointRounding.ToEven"/> is used for <see cref="MidpointRounding"/>.
        /// </para>
        /// </remarks>
        [CLSCompliant(true)]
        public static float Quantize (this float value, float multiple) => (float)(Math.Round((double)value / (double)multiple) * (double)multiple);

        /// <summary>
        /// Quantizes a value to the nearest value of a multiple.
        /// </summary>
        /// <param name="value">The value.</param>
        /// <param name="multiple">The multiple.</param>
        /// <param name="rounding">The kind of rounding to use.</param>
        /// <returns>
        /// The quantized value.
        /// </returns>
        [CLSCompliant(true)]
        public static float Quantize (this float value, float multiple, MidpointRounding rounding) => (float)(Math.Round((double)value / (double)multiple, rounding) * (double)multiple);

        /// <summary>
        /// Finds the greatest common divisor (GCD) of two values.
        /// </summary>
        /// <param name="x">The first value.</param>
        /// <param name="y">The second value.</param>
        /// <returns>
        /// The clamped value.
        /// </returns>
        [CLSCompliant(true)]
        public static float Gcd (this float x, float y) => MathUtils.Gcd(x, y);

        /// <summary>
        /// Finds the least common multiple (LCM) of two values.
        /// </summary>
        /// <param name="x">The first value.</param>
        /// <param name="y">The second value.</param>
        /// <returns>
        /// The clamped value.
        /// </returns>
        [CLSCompliant(true)]
        public static float Lcm (this float x, float y) => MathUtils.Lcm(x, y);

        /// <summary>
        /// Gets the sum from a sequence of values.
        /// </summary>
        /// <param name="values">The sequence of values.</param>
        /// <returns>
        /// The sum of the sequence.
        /// </returns>
        /// <remarks>
        /// <para>
        /// <paramref name="values" /> is enumerated only once.
        /// </para>
        /// </remarks>
        /// <exception cref="ArgumentNullException"> <paramref name="values" /> is null. </exception>
        [CLSCompliant(true)]
        public static float Sum (this IEnumerable<float> values)
        {
            if(values == null)
            {
                throw new ArgumentNullException(nameof(values));
            }

            float sum = 0;
            foreach(float value in values)
            {
                sum += value;
            }
            return sum;
        }

        /// <summary>
        /// Gets the minimum value from a sequence of values.
        /// </summary>
        /// <param name="values">The sequence of values.</param>
        /// <returns>
        /// The minimum value of the sequence.
        /// </returns>
        /// <remarks>
        /// <para>
        /// <paramref name="values" /> is enumerated only once.
        /// </para>
        /// </remarks>
        /// <exception cref="ArgumentNullException"> <paramref name="values" /> is null. </exception>
        [CLSCompliant(true)]
        public static float Min (this IEnumerable<float> values)
        {
            if(values == null)
            {
                throw new ArgumentNullException(nameof(values));
            }

            float min = float.MaxValue;
            foreach(float value in values)
            {
                if(value < min)
                {
                    min = value;
                }
            }
            return min;
        }

        /// <summary>
        /// Gets the maximum value from a sequence of values.
        /// </summary>
        /// <param name="values">The sequence of values.</param>
        /// <returns>
        /// The maximum value of the sequence.
        /// </returns>
        /// <remarks>
        /// <para>
        /// <paramref name="values" /> is enumerated only once.
        /// </para>
        /// </remarks>
        /// <exception cref="ArgumentNullException"> <paramref name="values" /> is null. </exception>
        [CLSCompliant(true)]
        public static float Max (this IEnumerable<float> values)
        {
            if(values == null)
            {
                throw new ArgumentNullException(nameof(values));
            }

            float max = float.MinValue;
            foreach(float value in values)
            {
                if(value > max)
                {
                    max = value;
                }
            }
            return max;
        }

        /// <summary>
        /// Finds the greatest common divisor (GCD) of a sequence of values.
        /// </summary>
        /// <param name="values">The sequence of values.</param>
        /// <returns>
        /// The GCD of the sequence or zero if the sequence is empty.
        /// </returns>
        /// <remarks>
        /// <para>
        /// <paramref name="values" /> is enumerated only once.
        /// </para>
        /// </remarks>
        /// <exception cref="ArgumentNullException"> <paramref name="values" /> is null. </exception>
        [CLSCompliant(true)]
        public static float Gcd (this IEnumerable<float> values)
        {
            if(values == null)
            {
                throw new ArgumentNullException(nameof(values));
            }

            float gcd = 0;
            int index = 0;
            foreach(float value in values)
            {
                if(index == 0)
                {
                    gcd = value;
                }
                else
                {
                    gcd = MathUtils.Gcd(gcd, value);
                }
                index++;
            }
            return gcd;
        }

        /// <summary>
        /// Finds the least common multiple (LCM) of a sequence of values.
        /// </summary>
        /// <param name="values">The sequence of values.</param>
        /// <returns>
        /// The LCM of the sequence or zero if the sequence is empty.
        /// </returns>
        /// <remarks>
        /// <para>
        /// <paramref name="values" /> is enumerated only once.
        /// </para>
        /// </remarks>
        /// <exception cref="ArgumentNullException"> <paramref name="values" /> is null. </exception>
        [CLSCompliant(true)]
        public static float Lcm (this IEnumerable<float> values)
        {
            if(values == null)
            {
                throw new ArgumentNullException(nameof(values));
            }

            float lcm = 0;
            int index = 0;
            foreach(float value in values)
            {
                if(index == 0)
                {
                    lcm = value;
                }
                else
                {
                    lcm = MathUtils.Lcm(lcm, value);
                }
                index++;
            }
            return lcm;
        }

        /// <summary>
        /// Converts a sequence of values to doubles.
        /// </summary>
        /// <param name="values">The sequence of values.</param>
        /// <returns>
        /// The list of values as doubles.
        /// An empty list is returned if the sequence is empty.
        /// </returns>
        /// <remarks>
        /// <para>
        /// <paramref name="values" /> is enumerated only once.
        /// </para>
        /// </remarks>
        /// <exception cref="ArgumentNullException"> <paramref name="values" /> is null. </exception>
        [CLSCompliant(true)]
        public static IEnumerable<double> AsDouble (this IEnumerable<float> values)
        {
            if(values == null)
            {
                throw new ArgumentNullException(nameof(values));
            }

            return values.Select(x => (double)x);
        }

        /// <summary>
        /// Gets statistics for a sequence of values.
        /// </summary>
        /// <param name="values">The sequence of values.</param>
        /// <returns>
        /// The statistics for the sequence.
        /// </returns>
        /// <remarks>
        /// <para>
        /// <paramref name="values" /> is enumerated only once.
        /// </para>
        /// </remarks>
        /// <exception cref="ArgumentNullException"> <paramref name="values" /> is null. </exception>
        [CLSCompliant(true)]
        public static StatisticValues GetStatistics (this IEnumerable<float> values) => new StatisticValues(values.AsDouble());

        /// <summary>
        /// Clamps a value between an inclusive minimum and maximum value.
        /// </summary>
        /// <param name="value">The value.</param>
        /// <param name="min">The lowest possible value.</param>
        /// <param name="max">The highest possible value.</param>
        /// <returns>
        /// The clamped value.
        /// </returns>
        [CLSCompliant(true)]
        public static double Clamp (this double value, double min, double max)
        {
            if(value < min)
            {
                return min;
            }
            if(value > max)
            {
                return max;
            }
            return value;
        }

        /// <summary>
        /// Clamps a value to an inclusive minimum value.
        /// </summary>
        /// <param name="value">The value.</param>
        /// <param name="min">The lowest possible value.</param>
        /// <returns>
        /// The clamped value.
        /// </returns>
        [CLSCompliant(true)]
        public static double ClampMin (this double value, double min)
        {
            if(value < min)
            {
                return min;
            }
            return value;
        }

        /// <summary>
        /// Clamps a value to an inclusive maximum value.
        /// </summary>
        /// <param name="value">The value.</param>
        /// <param name="max">The highest possible value.</param>
        /// <returns>
        /// The clamped value.
        /// </returns>
        [CLSCompliant(true)]
        public static double ClampMax (this double value, double max)
        {
            if(value > max)
            {
                return max;
            }
            return value;
        }

        /// <summary>
        /// Quantizes a value to the nearest value of a multiple.
        /// </summary>
        /// <param name="value">The value.</param>
        /// <param name="multiple">The multiple.</param>
        /// <returns>
        /// The quantized value.
        /// </returns>
        /// <remarks>
        /// <para>
        /// <see cref="MidpointRounding.ToEven"/> is used for <see cref="MidpointRounding"/>.
        /// </para>
        /// </remarks>
        [CLSCompliant(true)]
        public static double Quantize (this double value, double multiple) => (double)(Math.Round((double)value / (double)multiple) * (double)multiple);

        /// <summary>
        /// Quantizes a value to the nearest value of a multiple.
        /// </summary>
        /// <param name="value">The value.</param>
        /// <param name="multiple">The multiple.</param>
        /// <param name="rounding">The kind of rounding to use.</param>
        /// <returns>
        /// The quantized value.
        /// </returns>
        [CLSCompliant(true)]
        public static double Quantize (this double value, double multiple, MidpointRounding rounding) => (double)(Math.Round((double)value / (double)multiple, rounding) * (double)multiple);

        /// <summary>
        /// Finds the greatest common divisor (GCD) of two values.
        /// </summary>
        /// <param name="x">The first value.</param>
        /// <param name="y">The second value.</param>
        /// <returns>
        /// The clamped value.
        /// </returns>
        [CLSCompliant(true)]
        public static double Gcd (this double x, double y) => MathUtils.Gcd(x, y);

        /// <summary>
        /// Finds the least common multiple (LCM) of two values.
        /// </summary>
        /// <param name="x">The first value.</param>
        /// <param name="y">The second value.</param>
        /// <returns>
        /// The clamped value.
        /// </returns>
        [CLSCompliant(true)]
        public static double Lcm (this double x, double y) => MathUtils.Lcm(x, y);

        /// <summary>
        /// Gets the sum from a sequence of values.
        /// </summary>
        /// <param name="values">The sequence of values.</param>
        /// <returns>
        /// The sum of the sequence.
        /// </returns>
        /// <remarks>
        /// <para>
        /// <paramref name="values" /> is enumerated only once.
        /// </para>
        /// </remarks>
        /// <exception cref="ArgumentNullException"> <paramref name="values" /> is null. </exception>
        [CLSCompliant(true)]
        public static double Sum (this IEnumerable<double> values)
        {
            if(values == null)
            {
                throw new ArgumentNullException(nameof(values));
            }

            double sum = 0;
            foreach(double value in values)
            {
                sum += value;
            }
            return sum;
        }

        /// <summary>
        /// Gets the minimum value from a sequence of values.
        /// </summary>
        /// <param name="values">The sequence of values.</param>
        /// <returns>
        /// The minimum value of the sequence.
        /// </returns>
        /// <remarks>
        /// <para>
        /// <paramref name="values" /> is enumerated only once.
        /// </para>
        /// </remarks>
        /// <exception cref="ArgumentNullException"> <paramref name="values" /> is null. </exception>
        [CLSCompliant(true)]
        public static double Min (this IEnumerable<double> values)
        {
            if(values == null)
            {
                throw new ArgumentNullException(nameof(values));
            }

            double min = double.MaxValue;
            foreach(double value in values)
            {
                if(value < min)
                {
                    min = value;
                }
            }
            return min;
        }

        /// <summary>
        /// Gets the maximum value from a sequence of values.
        /// </summary>
        /// <param name="values">The sequence of values.</param>
        /// <returns>
        /// The maximum value of the sequence.
        /// </returns>
        /// <remarks>
        /// <para>
        /// <paramref name="values" /> is enumerated only once.
        /// </para>
        /// </remarks>
        /// <exception cref="ArgumentNullException"> <paramref name="values" /> is null. </exception>
        [CLSCompliant(true)]
        public static double Max (this IEnumerable<double> values)
        {
            if(values == null)
            {
                throw new ArgumentNullException(nameof(values));
            }

            double max = double.MinValue;
            foreach(double value in values)
            {
                if(value > max)
                {
                    max = value;
                }
            }
            return max;
        }

        /// <summary>
        /// Finds the greatest common divisor (GCD) of a sequence of values.
        /// </summary>
        /// <param name="values">The sequence of values.</param>
        /// <returns>
        /// The GCD of the sequence or zero if the sequence is empty.
        /// </returns>
        /// <remarks>
        /// <para>
        /// <paramref name="values" /> is enumerated only once.
        /// </para>
        /// </remarks>
        /// <exception cref="ArgumentNullException"> <paramref name="values" /> is null. </exception>
        [CLSCompliant(true)]
        public static double Gcd (this IEnumerable<double> values)
        {
            if(values == null)
            {
                throw new ArgumentNullException(nameof(values));
            }

            double gcd = 0;
            int index = 0;
            foreach(double value in values)
            {
                if(index == 0)
                {
                    gcd = value;
                }
                else
                {
                    gcd = MathUtils.Gcd(gcd, value);
                }
                index++;
            }
            return gcd;
        }

        /// <summary>
        /// Finds the least common multiple (LCM) of a sequence of values.
        /// </summary>
        /// <param name="values">The sequence of values.</param>
        /// <returns>
        /// The LCM of the sequence or zero if the sequence is empty.
        /// </returns>
        /// <remarks>
        /// <para>
        /// <paramref name="values" /> is enumerated only once.
        /// </para>
        /// </remarks>
        /// <exception cref="ArgumentNullException"> <paramref name="values" /> is null. </exception>
        [CLSCompliant(true)]
        public static double Lcm (this IEnumerable<double> values)
        {
            if(values == null)
            {
                throw new ArgumentNullException(nameof(values));
            }

            double lcm = 0;
            int index = 0;
            foreach(double value in values)
            {
                if(index == 0)
                {
                    lcm = value;
                }
                else
                {
                    lcm = MathUtils.Lcm(lcm, value);
                }
                index++;
            }
            return lcm;
        }

        /// <summary>
        /// Converts a sequence of values to floats.
        /// </summary>
        /// <param name="values">The sequence of values.</param>
        /// <returns>
        /// The list of values as floats.
        /// An empty list is returned if the sequence is empty.
        /// </returns>
        /// <remarks>
        /// <para>
        /// <paramref name="values" /> is enumerated only once.
        /// </para>
        /// </remarks>
        /// <exception cref="ArgumentNullException"> <paramref name="values" /> is null. </exception>
        [CLSCompliant(true)]
        public static IEnumerable<float> AsFloat (this IEnumerable<double> values)
        {
            if(values == null)
            {
                throw new ArgumentNullException(nameof(values));
            }

            return values.Select(x => (float)x);
        }

        /// <summary>
        /// Gets statistics for a sequence of values.
        /// </summary>
        /// <param name="values">The sequence of values.</param>
        /// <returns>
        /// The statistics for the sequence.
        /// </returns>
        /// <remarks>
        /// <para>
        /// <paramref name="values" /> is enumerated only once.
        /// </para>
        /// </remarks>
        /// <exception cref="ArgumentNullException"> <paramref name="values" /> is null. </exception>
        [CLSCompliant(true)]
        public static StatisticValues GetStatistics (this IEnumerable<double> values) => new StatisticValues(values);

        /// <summary>
        /// Clamps a value between an inclusive minimum and maximum value.
        /// </summary>
        /// <param name="value">The value.</param>
        /// <param name="min">The lowest possible value.</param>
        /// <param name="max">The highest possible value.</param>
        /// <returns>
        /// The clamped value.
        /// </returns>
        [CLSCompliant(true)]
        public static decimal Clamp (this decimal value, decimal min, decimal max)
        {
            if(value < min)
            {
                return min;
            }
            if(value > max)
            {
                return max;
            }
            return value;
        }

        /// <summary>
        /// Clamps a value to an inclusive minimum value.
        /// </summary>
        /// <param name="value">The value.</param>
        /// <param name="min">The lowest possible value.</param>
        /// <returns>
        /// The clamped value.
        /// </returns>
        [CLSCompliant(true)]
        public static decimal ClampMin (this decimal value, decimal min)
        {
            if(value < min)
            {
                return min;
            }
            return value;
        }

        /// <summary>
        /// Clamps a value to an inclusive maximum value.
        /// </summary>
        /// <param name="value">The value.</param>
        /// <param name="max">The highest possible value.</param>
        /// <returns>
        /// The clamped value.
        /// </returns>
        [CLSCompliant(true)]
        public static decimal ClampMax (this decimal value, decimal max)
        {
            if(value > max)
            {
                return max;
            }
            return value;
        }

        /// <summary>
        /// Quantizes a value to the nearest value of a multiple.
        /// </summary>
        /// <param name="value">The value.</param>
        /// <param name="multiple">The multiple.</param>
        /// <returns>
        /// The quantized value.
        /// </returns>
        /// <remarks>
        /// <para>
        /// <see cref="MidpointRounding.ToEven"/> is used for <see cref="MidpointRounding"/>.
        /// </para>
        /// </remarks>
        [CLSCompliant(true)]
        public static decimal Quantize (this decimal value, decimal multiple) => (decimal)(Math.Round((double)value / (double)multiple) * (double)multiple);

        /// <summary>
        /// Quantizes a value to the nearest value of a multiple.
        /// </summary>
        /// <param name="value">The value.</param>
        /// <param name="multiple">The multiple.</param>
        /// <param name="rounding">The kind of rounding to use.</param>
        /// <returns>
        /// The quantized value.
        /// </returns>
        [CLSCompliant(true)]
        public static decimal Quantize (this decimal value, decimal multiple, MidpointRounding rounding) => (decimal)(Math.Round((double)value / (double)multiple, rounding) * (double)multiple);

        /// <summary>
        /// Finds the greatest common divisor (GCD) of two values.
        /// </summary>
        /// <param name="x">The first value.</param>
        /// <param name="y">The second value.</param>
        /// <returns>
        /// The clamped value.
        /// </returns>
        [CLSCompliant(true)]
        public static decimal Gcd (this decimal x, decimal y) => MathUtils.Gcd(x, y);

        /// <summary>
        /// Finds the least common multiple (LCM) of two values.
        /// </summary>
        /// <param name="x">The first value.</param>
        /// <param name="y">The second value.</param>
        /// <returns>
        /// The clamped value.
        /// </returns>
        [CLSCompliant(true)]
        public static decimal Lcm (this decimal x, decimal y) => MathUtils.Lcm(x, y);

        /// <summary>
        /// Gets the sum from a sequence of values.
        /// </summary>
        /// <param name="values">The sequence of values.</param>
        /// <returns>
        /// The sum of the sequence.
        /// </returns>
        /// <remarks>
        /// <para>
        /// <paramref name="values" /> is enumerated only once.
        /// </para>
        /// </remarks>
        /// <exception cref="ArgumentNullException"> <paramref name="values" /> is null. </exception>
        [CLSCompliant(true)]
        public static decimal Sum (this IEnumerable<decimal> values)
        {
            if(values == null)
            {
                throw new ArgumentNullException(nameof(values));
            }

            decimal sum = 0;
            foreach(decimal value in values)
            {
                sum += value;
            }
            return sum;
        }

        /// <summary>
        /// Gets the minimum value from a sequence of values.
        /// </summary>
        /// <param name="values">The sequence of values.</param>
        /// <returns>
        /// The minimum value of the sequence.
        /// </returns>
        /// <remarks>
        /// <para>
        /// <paramref name="values" /> is enumerated only once.
        /// </para>
        /// </remarks>
        /// <exception cref="ArgumentNullException"> <paramref name="values" /> is null. </exception>
        [CLSCompliant(true)]
        public static decimal Min (this IEnumerable<decimal> values)
        {
            if(values == null)
            {
                throw new ArgumentNullException(nameof(values));
            }

            decimal min = decimal.MaxValue;
            foreach(decimal value in values)
            {
                if(value < min)
                {
                    min = value;
                }
            }
            return min;
        }

        /// <summary>
        /// Gets the maximum value from a sequence of values.
        /// </summary>
        /// <param name="values">The sequence of values.</param>
        /// <returns>
        /// The maximum value of the sequence.
        /// </returns>
        /// <remarks>
        /// <para>
        /// <paramref name="values" /> is enumerated only once.
        /// </para>
        /// </remarks>
        /// <exception cref="ArgumentNullException"> <paramref name="values" /> is null. </exception>
        [CLSCompliant(true)]
        public static decimal Max (this IEnumerable<decimal> values)
        {
            if(values == null)
            {
                throw new ArgumentNullException(nameof(values));
            }

            decimal max = decimal.MinValue;
            foreach(decimal value in values)
            {
                if(value > max)
                {
                    max = value;
                }
            }
            return max;
        }

        /// <summary>
        /// Finds the greatest common divisor (GCD) of a sequence of values.
        /// </summary>
        /// <param name="values">The sequence of values.</param>
        /// <returns>
        /// The GCD of the sequence or zero if the sequence is empty.
        /// </returns>
        /// <remarks>
        /// <para>
        /// <paramref name="values" /> is enumerated only once.
        /// </para>
        /// </remarks>
        /// <exception cref="ArgumentNullException"> <paramref name="values" /> is null. </exception>
        [CLSCompliant(true)]
        public static decimal Gcd (this IEnumerable<decimal> values)
        {
            if(values == null)
            {
                throw new ArgumentNullException(nameof(values));
            }

            decimal gcd = 0;
            int index = 0;
            foreach(decimal value in values)
            {
                if(index == 0)
                {
                    gcd = value;
                }
                else
                {
                    gcd = MathUtils.Gcd(gcd, value);
                }
                index++;
            }
            return gcd;
        }

        /// <summary>
        /// Finds the least common multiple (LCM) of a sequence of values.
        /// </summary>
        /// <param name="values">The sequence of values.</param>
        /// <returns>
        /// The LCM of the sequence or zero if the sequence is empty.
        /// </returns>
        /// <remarks>
        /// <para>
        /// <paramref name="values" /> is enumerated only once.
        /// </para>
        /// </remarks>
        /// <exception cref="ArgumentNullException"> <paramref name="values" /> is null. </exception>
        [CLSCompliant(true)]
        public static decimal Lcm (this IEnumerable<decimal> values)
        {
            if(values == null)
            {
                throw new ArgumentNullException(nameof(values));
            }

            decimal lcm = 0;
            int index = 0;
            foreach(decimal value in values)
            {
                if(index == 0)
                {
                    lcm = value;
                }
                else
                {
                    lcm = MathUtils.Lcm(lcm, value);
                }
                index++;
            }
            return lcm;
        }

        /// <summary>
        /// Converts a sequence of values to floats.
        /// </summary>
        /// <param name="values">The sequence of values.</param>
        /// <returns>
        /// The list of values as floats.
        /// An empty list is returned if the sequence is empty.
        /// </returns>
        /// <remarks>
        /// <para>
        /// <paramref name="values" /> is enumerated only once.
        /// </para>
        /// </remarks>
        /// <exception cref="ArgumentNullException"> <paramref name="values" /> is null. </exception>
        [CLSCompliant(true)]
        public static IEnumerable<float> AsFloat (this IEnumerable<decimal> values)
        {
            if(values == null)
            {
                throw new ArgumentNullException(nameof(values));
            }

            return values.Select(x => (float)x);
        }

        /// <summary>
        /// Converts a sequence of values to doubles.
        /// </summary>
        /// <param name="values">The sequence of values.</param>
        /// <returns>
        /// The list of values as doubles.
        /// An empty list is returned if the sequence is empty.
        /// </returns>
        /// <remarks>
        /// <para>
        /// <paramref name="values" /> is enumerated only once.
        /// </para>
        /// </remarks>
        /// <exception cref="ArgumentNullException"> <paramref name="values" /> is null. </exception>
        [CLSCompliant(true)]
        public static IEnumerable<double> AsDouble (this IEnumerable<decimal> values)
        {
            if(values == null)
            {
                throw new ArgumentNullException(nameof(values));
            }

            return values.Select(x => (double)x);
        }

        /// <summary>
        /// Gets statistics for a sequence of values.
        /// </summary>
        /// <param name="values">The sequence of values.</param>
        /// <returns>
        /// The statistics for the sequence.
        /// </returns>
        /// <remarks>
        /// <para>
        /// <paramref name="values" /> is enumerated only once.
        /// </para>
        /// </remarks>
        /// <exception cref="ArgumentNullException"> <paramref name="values" /> is null. </exception>
        [CLSCompliant(true)]
        public static StatisticValues GetStatistics (this IEnumerable<decimal> values) => new StatisticValues(values.AsDouble());

        /// <summary>
        /// Gets the absolute number of a value.
        /// </summary>
        /// <param name="value">The value.</param>
        /// <returns>
        /// The absolute number.
        /// </returns>
        [CLSCompliant(false)]
        // ReSharper disable once IntVariableOverflowInUncheckedContext
        public static sbyte Abs (this sbyte value) => value < 0 ? (sbyte)(-1 * value) : value;

        /// <summary>
        /// Gets the sign of a value.
        /// </summary>
        /// <param name="value">The value.</param>
        /// <returns>
        /// 1 if the number is positive, 0 if the number is zero, -1 if the number is negative.
        /// </returns>
        [SuppressMessage("ReSharper", "CompareOfFloatsByEqualityOperator")]
        [CLSCompliant(false)]
        public static sbyte Sign (this sbyte value) => (sbyte)((value == 0) ? 0 : (value < 0 ? -1 : 1));

        /// <summary>
        /// Calculates the magnitude of a value.
        /// </summary>
        /// <param name="value">The value.</param>
        /// <returns>
        /// The magnitude.
        /// </returns>
        /// <remarks>
        /// <para>
        /// The magnitude expresses the order of magnitude of a number.
        /// Simply said, it calculates the number of significant digits on the left side of the decimal point.
        /// </para>
        /// <para>
        /// If <paramref name="value"/> is positive, the result is positive.
        /// If <paramref name="value"/> is negative, the result is negative.
        /// If <paramref name="value"/> is zero, the result is zero.
        /// </para>
        /// <para>
        /// Examples: 0 -> 0; 1 -> 1; -1 -> -1; 5 -> 1; -5 -> -1; 10 -> 2; 100 -> 3; 1234 -> 4; -1234 -> -4; 9999 -> 4; 10000 -> 5; 10001 -> 5
        /// </para>
        /// </remarks>
        [SuppressMessage("ReSharper", "CompareOfFloatsByEqualityOperator")]
        [CLSCompliant(false)]
        public static sbyte Magnitude (this sbyte value)
        {
            if(value == 0)
            {
                return 0;
            }

            return (sbyte)(Math.Floor(Math.Log10((double)Math.Abs(value)) + 1.0) * (double)Math.Sign(value));
        }

        /// <summary>
        /// Gets the absolute number of a value.
        /// </summary>
        /// <param name="value">The value.</param>
        /// <returns>
        /// The absolute number.
        /// </returns>
        [CLSCompliant(true)]
        // ReSharper disable once IntVariableOverflowInUncheckedContext
        public static short Abs (this short value) => value < 0 ? (short)(-1 * value) : value;

        /// <summary>
        /// Gets the sign of a value.
        /// </summary>
        /// <param name="value">The value.</param>
        /// <returns>
        /// 1 if the number is positive, 0 if the number is zero, -1 if the number is negative.
        /// </returns>
        [SuppressMessage("ReSharper", "CompareOfFloatsByEqualityOperator")]
        [CLSCompliant(true)]
        public static short Sign (this short value) => (short)((value == 0) ? 0 : (value < 0 ? -1 : 1));

        /// <summary>
        /// Calculates the magnitude of a value.
        /// </summary>
        /// <param name="value">The value.</param>
        /// <returns>
        /// The magnitude.
        /// </returns>
        /// <remarks>
        /// <para>
        /// The magnitude expresses the order of magnitude of a number.
        /// Simply said, it calculates the number of significant digits on the left side of the decimal point.
        /// </para>
        /// <para>
        /// If <paramref name="value"/> is positive, the result is positive.
        /// If <paramref name="value"/> is negative, the result is negative.
        /// If <paramref name="value"/> is zero, the result is zero.
        /// </para>
        /// <para>
        /// Examples: 0 -> 0; 1 -> 1; -1 -> -1; 5 -> 1; -5 -> -1; 10 -> 2; 100 -> 3; 1234 -> 4; -1234 -> -4; 9999 -> 4; 10000 -> 5; 10001 -> 5
        /// </para>
        /// </remarks>
        [SuppressMessage("ReSharper", "CompareOfFloatsByEqualityOperator")]
        [CLSCompliant(true)]
        public static short Magnitude (this short value)
        {
            if(value == 0)
            {
                return 0;
            }

            return (short)(Math.Floor(Math.Log10((double)Math.Abs(value)) + 1.0) * (double)Math.Sign(value));
        }

        /// <summary>
        /// Gets the absolute number of a value.
        /// </summary>
        /// <param name="value">The value.</param>
        /// <returns>
        /// The absolute number.
        /// </returns>
        [CLSCompliant(true)]
        // ReSharper disable once IntVariableOverflowInUncheckedContext
        public static int Abs (this int value) => value < 0 ? (int)(-1 * value) : value;

        /// <summary>
        /// Gets the sign of a value.
        /// </summary>
        /// <param name="value">The value.</param>
        /// <returns>
        /// 1 if the number is positive, 0 if the number is zero, -1 if the number is negative.
        /// </returns>
        [SuppressMessage("ReSharper", "CompareOfFloatsByEqualityOperator")]
        [CLSCompliant(true)]
        public static int Sign (this int value) => (int)((value == 0) ? 0 : (value < 0 ? -1 : 1));

        /// <summary>
        /// Calculates the magnitude of a value.
        /// </summary>
        /// <param name="value">The value.</param>
        /// <returns>
        /// The magnitude.
        /// </returns>
        /// <remarks>
        /// <para>
        /// The magnitude expresses the order of magnitude of a number.
        /// Simply said, it calculates the number of significant digits on the left side of the decimal point.
        /// </para>
        /// <para>
        /// If <paramref name="value"/> is positive, the result is positive.
        /// If <paramref name="value"/> is negative, the result is negative.
        /// If <paramref name="value"/> is zero, the result is zero.
        /// </para>
        /// <para>
        /// Examples: 0 -> 0; 1 -> 1; -1 -> -1; 5 -> 1; -5 -> -1; 10 -> 2; 100 -> 3; 1234 -> 4; -1234 -> -4; 9999 -> 4; 10000 -> 5; 10001 -> 5
        /// </para>
        /// </remarks>
        [SuppressMessage("ReSharper", "CompareOfFloatsByEqualityOperator")]
        [CLSCompliant(true)]
        public static int Magnitude (this int value)
        {
            if(value == 0)
            {
                return 0;
            }

            return (int)(Math.Floor(Math.Log10((double)Math.Abs(value)) + 1.0) * (double)Math.Sign(value));
        }

        /// <summary>
        /// Gets the absolute number of a value.
        /// </summary>
        /// <param name="value">The value.</param>
        /// <returns>
        /// The absolute number.
        /// </returns>
        [CLSCompliant(true)]
        // ReSharper disable once IntVariableOverflowInUncheckedContext
        public static long Abs (this long value) => value < 0 ? (long)(-1 * value) : value;

        /// <summary>
        /// Gets the sign of a value.
        /// </summary>
        /// <param name="value">The value.</param>
        /// <returns>
        /// 1 if the number is positive, 0 if the number is zero, -1 if the number is negative.
        /// </returns>
        [SuppressMessage("ReSharper", "CompareOfFloatsByEqualityOperator")]
        [CLSCompliant(true)]
        public static long Sign (this long value) => (long)((value == 0) ? 0 : (value < 0 ? -1 : 1));

        /// <summary>
        /// Calculates the magnitude of a value.
        /// </summary>
        /// <param name="value">The value.</param>
        /// <returns>
        /// The magnitude.
        /// </returns>
        /// <remarks>
        /// <para>
        /// The magnitude expresses the order of magnitude of a number.
        /// Simply said, it calculates the number of significant digits on the left side of the decimal point.
        /// </para>
        /// <para>
        /// If <paramref name="value"/> is positive, the result is positive.
        /// If <paramref name="value"/> is negative, the result is negative.
        /// If <paramref name="value"/> is zero, the result is zero.
        /// </para>
        /// <para>
        /// Examples: 0 -> 0; 1 -> 1; -1 -> -1; 5 -> 1; -5 -> -1; 10 -> 2; 100 -> 3; 1234 -> 4; -1234 -> -4; 9999 -> 4; 10000 -> 5; 10001 -> 5
        /// </para>
        /// </remarks>
        [SuppressMessage("ReSharper", "CompareOfFloatsByEqualityOperator")]
        [CLSCompliant(true)]
        public static long Magnitude (this long value)
        {
            if(value == 0)
            {
                return 0;
            }

            return (long)(Math.Floor(Math.Log10((double)Math.Abs(value)) + 1.0) * (double)Math.Sign(value));
        }

        /// <summary>
        /// Gets the absolute number of a value.
        /// </summary>
        /// <param name="value">The value.</param>
        /// <returns>
        /// The absolute number.
        /// </returns>
        [CLSCompliant(true)]
        // ReSharper disable once IntVariableOverflowInUncheckedContext
        public static float Abs (this float value) => value < 0.0f ? (float)(-1.0f * value) : value;

        /// <summary>
        /// Gets the sign of a value.
        /// </summary>
        /// <param name="value">The value.</param>
        /// <returns>
        /// 1 if the number is positive, 0 if the number is zero, -1 if the number is negative.
        /// </returns>
        [SuppressMessage("ReSharper", "CompareOfFloatsByEqualityOperator")]
        [CLSCompliant(true)]
        public static float Sign (this float value) => (float)((value == 0.0f) ? 0.0f : (value < 0.0f ? -1.0f : 1.0f));

        /// <summary>
        /// Calculates the magnitude of a value.
        /// </summary>
        /// <param name="value">The value.</param>
        /// <returns>
        /// The magnitude.
        /// </returns>
        /// <remarks>
        /// <para>
        /// The magnitude expresses the order of magnitude of a number.
        /// Simply said, it calculates the number of significant digits on the left side of the decimal point.
        /// </para>
        /// <para>
        /// If <paramref name="value"/> is positive, the result is positive.
        /// If <paramref name="value"/> is negative, the result is negative.
        /// If <paramref name="value"/> is zero, the result is zero.
        /// </para>
        /// <para>
        /// Examples: 0 -> 0; 1 -> 1; -1 -> -1; 5 -> 1; -5 -> -1; 10 -> 2; 100 -> 3; 1234 -> 4; -1234 -> -4; 9999 -> 4; 10000 -> 5; 10001 -> 5
        /// </para>
        /// </remarks>
        [SuppressMessage("ReSharper", "CompareOfFloatsByEqualityOperator")]
        [CLSCompliant(true)]
        public static float Magnitude (this float value)
        {
            if(value == 0.0f)
            {
                return 0.0f;
            }

            return (float)(Math.Floor(Math.Log10((double)Math.Abs(value)) + 1.0) * (double)Math.Sign(value));
        }

        /// <summary>
        /// Gets the absolute number of a value.
        /// </summary>
        /// <param name="value">The value.</param>
        /// <returns>
        /// The absolute number.
        /// </returns>
        [CLSCompliant(true)]
        // ReSharper disable once IntVariableOverflowInUncheckedContext
        public static double Abs (this double value) => value < 0.0 ? (double)(-1.0 * value) : value;

        /// <summary>
        /// Gets the sign of a value.
        /// </summary>
        /// <param name="value">The value.</param>
        /// <returns>
        /// 1 if the number is positive, 0 if the number is zero, -1 if the number is negative.
        /// </returns>
        [SuppressMessage("ReSharper", "CompareOfFloatsByEqualityOperator")]
        [CLSCompliant(true)]
        public static double Sign (this double value) => (double)((value == 0.0) ? 0.0 : (value < 0.0 ? -1.0 : 1.0));

        /// <summary>
        /// Calculates the magnitude of a value.
        /// </summary>
        /// <param name="value">The value.</param>
        /// <returns>
        /// The magnitude.
        /// </returns>
        /// <remarks>
        /// <para>
        /// The magnitude expresses the order of magnitude of a number.
        /// Simply said, it calculates the number of significant digits on the left side of the decimal point.
        /// </para>
        /// <para>
        /// If <paramref name="value"/> is positive, the result is positive.
        /// If <paramref name="value"/> is negative, the result is negative.
        /// If <paramref name="value"/> is zero, the result is zero.
        /// </para>
        /// <para>
        /// Examples: 0 -> 0; 1 -> 1; -1 -> -1; 5 -> 1; -5 -> -1; 10 -> 2; 100 -> 3; 1234 -> 4; -1234 -> -4; 9999 -> 4; 10000 -> 5; 10001 -> 5
        /// </para>
        /// </remarks>
        [SuppressMessage("ReSharper", "CompareOfFloatsByEqualityOperator")]
        [CLSCompliant(true)]
        public static double Magnitude (this double value)
        {
            if(value == 0.0)
            {
                return 0.0;
            }

            return (double)(Math.Floor(Math.Log10((double)Math.Abs(value)) + 1.0) * (double)Math.Sign(value));
        }

        /// <summary>
        /// Gets the absolute number of a value.
        /// </summary>
        /// <param name="value">The value.</param>
        /// <returns>
        /// The absolute number.
        /// </returns>
        [CLSCompliant(true)]
        // ReSharper disable once IntVariableOverflowInUncheckedContext
        public static decimal Abs (this decimal value) => value < 0.0m ? (decimal)(-1.0m * value) : value;

        /// <summary>
        /// Gets the sign of a value.
        /// </summary>
        /// <param name="value">The value.</param>
        /// <returns>
        /// 1 if the number is positive, 0 if the number is zero, -1 if the number is negative.
        /// </returns>
        [SuppressMessage("ReSharper", "CompareOfFloatsByEqualityOperator")]
        [CLSCompliant(true)]
        public static decimal Sign (this decimal value) => (decimal)((value == 0.0m) ? 0.0m : (value < 0.0m ? -1.0m : 1.0m));

        /// <summary>
        /// Calculates the magnitude of a value.
        /// </summary>
        /// <param name="value">The value.</param>
        /// <returns>
        /// The magnitude.
        /// </returns>
        /// <remarks>
        /// <para>
        /// The magnitude expresses the order of magnitude of a number.
        /// Simply said, it calculates the number of significant digits on the left side of the decimal point.
        /// </para>
        /// <para>
        /// If <paramref name="value"/> is positive, the result is positive.
        /// If <paramref name="value"/> is negative, the result is negative.
        /// If <paramref name="value"/> is zero, the result is zero.
        /// </para>
        /// <para>
        /// Examples: 0 -> 0; 1 -> 1; -1 -> -1; 5 -> 1; -5 -> -1; 10 -> 2; 100 -> 3; 1234 -> 4; -1234 -> -4; 9999 -> 4; 10000 -> 5; 10001 -> 5
        /// </para>
        /// </remarks>
        [SuppressMessage("ReSharper", "CompareOfFloatsByEqualityOperator")]
        [CLSCompliant(true)]
        public static decimal Magnitude (this decimal value)
        {
            if(value == 0.0m)
            {
                return 0.0m;
            }

            return (decimal)(Math.Floor(Math.Log10((double)Math.Abs(value)) + 1.0) * (double)Math.Sign(value));
        }

        /// <summary>
        /// Gets the remainder of a division.
        /// </summary>
        /// <param name="dividend">The dividend.</param>
        /// <param name="divisor">The divisor.</param>
        /// <returns>
        /// The remainder of  dividend / divisor.
        /// </returns>
        /// <remarks>
        /// <note type="important">
        /// This is not the same as simply applying the module operator.
        /// The remainder is calculated, allowing the operation for floating point values.
        /// </note>
        /// </remarks>
        public static float DivRem (this float dividend, float divisor)
        {
            float dividendAbs = dividend.Abs();
            float divisorAbs = divisor.Abs();
            float sign = dividend.Sign();
            return (dividendAbs - (divisorAbs * ((float)Math.Floor(dividendAbs / divisorAbs)))) * sign;
        }

        /// <summary>
        /// Determines whether a value is almost equal to another value.
        /// </summary>
        /// <param name="value">The value.</param>
        /// <param name="other">The other value to compare with.</param>
        /// <returns>
        /// true if the value is within the default accuracy for equality, false otherwise.
        /// </returns>
        public static bool AlmostEqual (this float value, float other) => value.AlmostEqual(other, MathConstF.DefaultAccuracy);

        /// <summary>
        /// Determines whether a value is almost equal to another value.
        /// </summary>
        /// <param name="value">The value.</param>
        /// <param name="other">The other value to compare with.</param>
        /// <param name="accuracy">The accuracy within the two values are considered equal.</param>
        /// <returns>
        /// true if the value is within the specified accuracy for equality, false otherwise.
        /// </returns>
        public static bool AlmostEqual (this float value, float other, float accuracy) => Math.Abs(value - other) < (double)accuracy;

        /// <summary>
        /// Determines whether a value is almost zero.
        /// </summary>
        /// <param name="value">The value.</param>
        /// <returns>
        /// true if the value is within the default accuracy to zero, false otherwise.
        /// </returns>
        public static bool AlmostZero (this float value) => Math.Abs(value) < (double)MathConstF.DefaultAccuracy;

        /// <summary>
        /// Determines whether a value is almost zero.
        /// </summary>
        /// <param name="value">The value.</param>
        /// <param name="accuracy">The accuracy within the value is considered zero.</param>
        /// <returns>
        /// true if the value is within the specified accuracy to zero, false otherwise.
        /// </returns>
        public static bool AlmostZero (this float value, float accuracy) => Math.Abs(value) < (double)accuracy;

        /// <summary>
        /// Gets the smallest integer that is greater than or equal to a value.
        /// </summary>
        /// <param name="value">The value.</param>
        /// <returns>
        /// The smallest integer.
        /// </returns>
        public static float Ceiling (this float value) => (float)Math.Ceiling(value);

        /// <summary>
        /// Gets the largest integer less than or equal to a value.
        /// </summary>
        /// <param name="value">The value.</param>
        /// <returns>
        /// The largest integer.
        /// </returns>
        public static float Floor (this float value) => (float)Math.Floor(value);

        /// <summary>
        /// Gets the integer part of a value.
        /// </summary>
        /// <param name="value">The value.</param>
        /// <returns>
        /// The integer part.
        /// </returns>
        public static float Integer (this float value) => (float)Math.Truncate(value);

        /// <summary>
        /// Gets the fraction part of a value.
        /// </summary>
        /// <param name="value">The value.</param>
        /// <returns>
        /// The fraction part.
        /// </returns>
        public static float Fraction (this float value) => (float)(value - Math.Truncate(value));

        /// <summary>
        /// Rounds a value to the nearest integer.
        /// </summary>
        /// <param name="value">The value.</param>
        /// <returns>
        /// The rounded value.
        /// </returns>
        /// <remarks>
        /// <para>
        /// <see cref="MidpointRounding.ToEven"/> is used for <see cref="MidpointRounding"/>.
        /// </para>
        /// </remarks>
        public static float RoundInteger (this float value) => (float)Math.Round(value);

        /// <summary>
        /// Rounds a value to the nearest integer.
        /// </summary>
        /// <param name="value">The value.</param>
        /// <param name="rounding">The kind of rounding to use.</param>
        /// <returns>
        /// The rounded value.
        /// </returns>
        public static float RoundInteger (this float value, MidpointRounding rounding) => (float)Math.Round(value, rounding);

        /// <summary>
        /// Rounds a value to a specified amount of fractional digits.
        /// </summary>
        /// <param name="value">The value.</param>
        /// <param name="digits">The number of digits.</param>
        /// <returns>
        /// The rounded value.
        /// </returns>
        /// <remarks>
        /// <para>
        /// <see cref="MidpointRounding.ToEven"/> is used for <see cref="MidpointRounding"/>.
        /// </para>
        /// </remarks>
        public static float RoundDigits (this float value, int digits) => (float)Math.Round(value, digits);

        /// <summary>
        /// Rounds a value to a specified amount of fractional digits.
        /// </summary>
        /// <param name="value">The value.</param>
        /// <param name="digits">The number of digits.</param>
        /// <param name="rounding">The kind of rounding to use.</param>
        /// <returns>
        /// The rounded value.
        /// </returns>
        public static float RoundDigits (this float value, int digits, MidpointRounding rounding) => (float)Math.Round(value, digits, rounding);

        /// <summary>
        /// Calculates value^power.
        /// </summary>
        /// <param name="value">The value.</param>
        /// <param name="power">The power.</param>
        /// <returns>
        /// The result.
        /// </returns>
        public static float Pow (this float value, double power) => (float)Math.Pow((double)value, power);

        /// <summary>
        /// Calculates value^2.
        /// </summary>
        /// <param name="value">The value.</param>
        /// <returns>
        /// The result.
        /// </returns>
        public static float Pow2 (this float value) => value * value;

        /// <summary>
        /// Calculates value^3.
        /// </summary>
        /// <param name="value">The value.</param>
        /// <returns>
        /// The result.
        /// </returns>
        public static float Pow3 (this float value) => value * value * value;

        /// <summary>
        /// Calculates value^10.
        /// </summary>
        /// <param name="value">The value.</param>
        /// <returns>
        /// The result.
        /// </returns>
        public static float Pow10 (this float value) => (float)Math.Pow((double)value, 10.0);

        /// <summary>
        /// Calculates value^e.
        /// </summary>
        /// <param name="value">The value.</param>
        /// <returns>
        /// The result.
        /// </returns>
        public static float PowE (this float value) => (float)Math.Pow((double)value, Math.E);

        /// <summary>
        /// Calculates expBase^value.
        /// </summary>
        /// <param name="value">The value.</param>
        /// <param name="expBase">The base.</param>
        /// <returns>
        /// The result.
        /// </returns>
        public static float Exp (this float value, double expBase) => (float)Math.Pow(expBase, (double)value);

        /// <summary>
        /// Calculates 2^value.
        /// </summary>
        /// <param name="value">The value.</param>
        /// <returns>
        /// The result.
        /// </returns>
        public static float Exp2 (this float value) => (float)Math.Pow(2.0, (double)value);

        /// <summary>
        /// Calculates 3^value.
        /// </summary>
        /// <param name="value">The value.</param>
        /// <returns>
        /// The result.
        /// </returns>
        public static float Exp3 (this float value) => (float)Math.Pow(3.0, (double)value);

        /// <summary>
        /// Calculates 10^value.
        /// </summary>
        /// <param name="value">The value.</param>
        /// <returns>
        /// The result.
        /// </returns>
        public static float Exp10 (this float value) => (float)Math.Pow(10.0, (double)value);

        /// <summary>
        /// Calculates e^value.
        /// </summary>
        /// <param name="value">The value.</param>
        /// <returns>
        /// The result.
        /// </returns>
        public static float ExpE (this float value) => (float)Math.Exp((double)value);

        /// <summary>
        /// Calculates log[logBase](value).
        /// </summary>
        /// <param name="value">The value.</param>
        /// <param name="logBase">The base.</param>
        /// <returns>
        /// The result.
        /// </returns>
        public static float Log (this float value, double logBase) => (float)Math.Log((double)value, logBase);

        /// <summary>
        /// Calculates log[2](value).
        /// </summary>
        /// <param name="value">The value.</param>
        /// <returns>
        /// The result.
        /// </returns>
        public static float Log2 (this float value) => (float)Math.Log((double)value, 2.0);

        /// <summary>
        /// Calculates log[3](value).
        /// </summary>
        /// <param name="value">The value.</param>
        /// <returns>
        /// The result.
        /// </returns>
        public static float Log3 (this float value) => (float)Math.Log((double)value, 3.0);

        /// <summary>
        /// Calculates log[10](value).
        /// </summary>
        /// <param name="value">The value.</param>
        /// <returns>
        /// The result.
        /// </returns>
        public static float Log10 (this float value) => (float)Math.Log10((double)value);

        /// <summary>
        /// Calculates log[e](value).
        /// </summary>
        /// <param name="value">The value.</param>
        /// <returns>
        /// The result.
        /// </returns>
        public static float LogE (this float value) => (float)Math.Log((double)value);

        /// <summary>
        /// Calculates the square-root of a value.
        /// </summary>
        /// <param name="value">The value.</param>
        /// <returns>
        /// The square-root.
        /// </returns>
        public static float Sqrt (this float value) => (float)Math.Sqrt((double)value);

        /// <summary>
        /// Calculates the n-th root of a value.
        /// </summary>
        /// <param name="value">The value.</param>
        /// <param name="n">The root.</param>
        /// <returns>
        /// The n-th root.
        /// </returns>
        public static float Root (this float value, double n) => (float)Math.Pow((double)value, 1.0 / n);

        /// <summary>
        /// Calculates the Sine of a value in radians.
        /// </summary>
        /// <param name="value">The value.</param>
        /// <returns>
        /// The result.
        /// </returns>
        public static float Sin (this float value) => (float)Math.Sin(value);

        /// <summary>
        /// Calculates the Cosine of a value in radians.
        /// </summary>
        /// <param name="value">The value.</param>
        /// <returns>
        /// The result.
        /// </returns>
        public static float Cos (this float value) => (float)Math.Cos(value);

        /// <summary>
        /// Calculates the Tangent of a value in radians.
        /// </summary>
        /// <param name="value">The value.</param>
        /// <returns>
        /// The result.
        /// </returns>
        public static float Tan (this float value) => (float)Math.Tan(value);

        /// <summary>
        /// Calculates the Cotangent of a value in radians.
        /// </summary>
        /// <param name="value">The value.</param>
        /// <returns>
        /// The result.
        /// </returns>
        public static float Cot (this float value) => 1.0f / (float)Math.Tan(value);

        /// <summary>
        /// Calculates the Arc Sine of a value in radians.
        /// </summary>
        /// <param name="value">The value.</param>
        /// <returns>
        /// The result.
        /// </returns>
        public static float Asin (this float value) => (float)Math.Asin(value);

        /// <summary>
        /// Calculates the Arc Cosine of a value in radians.
        /// </summary>
        /// <param name="value">The value.</param>
        /// <returns>
        /// The result.
        /// </returns>
        public static float Acos (this float value) => (float)Math.Acos(value);

        /// <summary>
        /// Calculates the Arc Tangent of a value in radians.
        /// </summary>
        /// <param name="value">The value.</param>
        /// <returns>
        /// The result.
        /// </returns>
        public static float Atan (this float value) => (float)Math.Atan(value);

        /// <summary>
        /// Calculates the Arc Cotangent of a value in radians.
        /// </summary>
        /// <param name="value">The value.</param>
        /// <returns>
        /// The result.
        /// </returns>
        public static float Acot (this float value) => (float)Math.Tan(1.0f / value);

        /// <summary>
        /// Calculates the Hyperbolic Sine of a value.
        /// </summary>
        /// <param name="value">The value.</param>
        /// <returns>
        /// The result.
        /// </returns>
        public static float Sinh (this float value) => (float)Math.Sinh(value);

        /// <summary>
        /// Calculates the Hyperbolic Cosine of a value.
        /// </summary>
        /// <param name="value">The value.</param>
        /// <returns>
        /// The result.
        /// </returns>
        public static float Cosh (this float value) => (float)Math.Cosh(value);

        /// <summary>
        /// Calculates the Hyperbolic Tangent of a value.
        /// </summary>
        /// <param name="value">The value.</param>
        /// <returns>
        /// The result.
        /// </returns>
        public static float Tanh (this float value) => (float)Math.Tanh(value);

        /// <summary>
        /// Calculates the Hyperbolic Cotangent of a value.
        /// </summary>
        /// <param name="value">The value.</param>
        /// <returns>
        /// The result.
        /// </returns>
        public static float Coth (this float value)
        {
            double e1 = Math.Exp(value);
            double e2 = Math.Exp(-value);
            return (float)((e1 + e2) / (e1 - e2));
        }

        /// <summary>
        /// Converts a radian value to degrees.
        /// </summary>
        /// <param name="value">The value.</param>
        /// <returns>
        /// The result.
        /// </returns>
        /// <remarks>
        /// <para>
        /// The result is not clamped to a single full circle value (-359...0...+359).
        /// Use <see cref="CircularClampDeg(float)" /> to clamp to a single full circle value.
        /// </para>
        /// </remarks>
        public static float ToDeg (this float value) => value * MathConstF.RadToDeg;

        /// <summary>
        /// Converts a degree value to radians.
        /// </summary>
        /// <param name="value">The value.</param>
        /// <returns>
        /// The result.
        /// </returns>
        /// <remarks>
        /// <para>
        /// The result is not clamped to a single full circle value (-2π...0...+2π).
        /// Use <see cref="CircularClampRad(float)" /> to clamp to a single full circle value.
        /// </para>
        /// </remarks>
        public static float ToRad (this float value) => value * MathConstF.DegToRad;

        /// <summary>
        /// Clamps a degree value to a single full circle (-359...0...+359).
        /// </summary>
        /// <param name="value">The value.</param>
        /// <returns>
        /// The result.
        /// </returns>
        /// <remarks>
        /// <para>
        /// Examples: 0 -> 0; 350 -> 350; 360 -> 0; 370 -> 10; etc.
        /// </para>
        /// </remarks>
        public static float CircularClampDeg (this float value) => value.DivRem(360.0f);

        /// <summary>
        /// Clamps a radian value to a single full circle (-2π...0...+2π).
        /// </summary>
        /// <param name="value">The value.</param>
        /// <returns>
        /// The result.
        /// </returns>
        /// <remarks>
        /// <para>
        /// Examples: 0 -> 0; 1.5 -> 1.5; 2π -> 0; 3π -> π; etc.
        /// </para>
        /// </remarks>
        public static float CircularClampRad (this float value) => value.DivRem(2.0f*3.1415926535897932384626433832795028841971693993751f);

        /// <summary>
        /// Gets the fractional part of a value as an integer value.
        /// </summary>
        /// <param name="value">The value.</param>
        /// <returns>
        /// The result.
        /// </returns>
        /// <remarks>
        /// <para>
        /// Examples: 12.34 -> 34; 3.416 -> 416; etc.
        /// </para>
        /// </remarks>
        public static float GetFractionAsInteger (this float value)
        {
            string stringValue = value.ToString("F", CultureInfo.InvariantCulture);

            int separatorIndex = stringValue.IndexOf('.');
            if(separatorIndex == -1)
            {
                return 0;
            }

            string fractionPart = stringValue.Substring(separatorIndex + 1);
            return float.Parse(fractionPart, CultureInfo.InvariantCulture);
        }

        /// <summary>
        /// Gets the Product from a sequence of values.
        /// </summary>
        /// <param name="values">The sequence of values.</param>
        /// <returns>
        /// The product of the sequence.
        /// </returns>
        /// <remarks>
        /// <para>
        /// <paramref name="values" /> is enumerated only once.
        /// </para>
        /// </remarks>
        /// <exception cref="ArgumentNullException"> <paramref name="values" /> is null. </exception>
        public static float Product (this IEnumerable<float> values)
        {
            if(values == null)
            {
                throw new ArgumentNullException(nameof(values));
            }

            float product = 0;
            foreach(float value in values)
            {
                product *= value;
            }
            return product;
        }

        /// <summary>
        /// Gets the arithmetic mean or average from a sequence of values.
        /// </summary>
        /// <param name="values">The sequence of values.</param>
        /// <returns>
        /// The arithmetic mean or average of the sequence.
        /// </returns>
        /// <remarks>
        /// <para>
        /// <paramref name="values" /> is enumerated only once.
        /// </para>
        /// </remarks>
        /// <exception cref="ArgumentNullException"> <paramref name="values" /> is null. </exception>
        public static double ArithmeticMean (this IEnumerable<float> values)
        {
            if(values == null)
            {
                throw new ArgumentNullException(nameof(values));
            }

            float sum = 0;
            int count = 0;
            foreach(float value in values)
            {
                sum += value;
                count += 1;
            }
            if(count == 0)
            {
                return 0;
            }
            return sum / (float)count;
        }

        /// <summary>
        /// Gets the geometric mean from a sequence of values.
        /// </summary>
        /// <param name="values">The sequence of values.</param>
        /// <returns>
        /// The geometric mean of the sequence.
        /// </returns>
        /// <remarks>
        /// <para>
        /// <paramref name="values" /> is enumerated only once.
        /// </para>
        /// </remarks>
        /// <exception cref="ArgumentNullException"> <paramref name="values" /> is null. </exception>
        public static float GeometricMean (this IEnumerable<float> values)
        {
            if(values == null)
            {
                throw new ArgumentNullException(nameof(values));
            }

            float product = 0;
            int count = 0;
            foreach(float value in values)
            {
                product *= value;
                count += 1;
            }
            if(count == 0)
            {
                return 0;
            }
            return (float)Math.Pow((double)product, 1.0 / (float)count);
        }

        /// <summary>
        /// Gets the harmonic mean from a sequence of values.
        /// </summary>
        /// <param name="values">The sequence of values.</param>
        /// <returns>
        /// The harmonic mean of the sequence.
        /// </returns>
        /// <remarks>
        /// <para>
        /// <paramref name="values" /> is enumerated only once.
        /// </para>
        /// </remarks>
        /// <exception cref="ArgumentNullException"> <paramref name="values" /> is null. </exception>
        public static float HarmonicMean (this IEnumerable<float> values)
        {
            if(values == null)
            {
                throw new ArgumentNullException(nameof(values));
            }

            float sum = 0;
            int count = 0;
            foreach(float value in values)
            {
                sum += (1.0f) / value;
                count += 1;
            }
            if(count == 0)
            {
                return 0;
            }
            return ((float)count) / sum;
        }

        /// <summary>
        /// Gets the sum of all squared values (first squared, then summed) from a sequence of values.
        /// </summary>
        /// <param name="values">The sequence of values.</param>
        /// <returns>
        /// The sum of all squared values of the sequence.
        /// </returns>
        /// <remarks>
        /// <para>
        /// <paramref name="values" /> is enumerated only once.
        /// </para>
        /// </remarks>
        /// <exception cref="ArgumentNullException"> <paramref name="values" /> is null. </exception>
        public static float SquareSum (this IEnumerable<float> values)
        {
            if(values == null)
            {
                throw new ArgumentNullException(nameof(values));
            }

            float sum = 0;
            foreach(float value in values)
            {
                sum += value * value;
            }
            return sum;
        }

        /// <summary>
        /// Gets the RMS from a sequence of values.
        /// </summary>
        /// <param name="values">The sequence of values.</param>
        /// <returns>
        /// The RMS of the sequence.
        /// </returns>
        /// <remarks>
        /// <para>
        /// <paramref name="values" /> is enumerated only once.
        /// </para>
        /// </remarks>
        /// <exception cref="ArgumentNullException"> <paramref name="values" /> is null. </exception>
        public static float Rms (this IEnumerable<float> values)
        {
            if(values == null)
            {
                throw new ArgumentNullException(nameof(values));
            }

            float squareSum = values.SquareSum();
            return  (float)Math.Sqrt(squareSum);
        }

        /// <summary>
        /// Gets the variance from a sequence of values.
        /// </summary>
        /// <param name="values">The sequence of values.</param>
        /// <returns>
        /// The variance of the sequence.
        /// </returns>
        /// <remarks>
        /// <para>
        /// <paramref name="values" /> is enumerated only once.
        /// </para>
        /// </remarks>
        /// <exception cref="ArgumentNullException"> <paramref name="values" /> is null. </exception>
        public static float Variance (this IEnumerable<float> values)
        {
            if(values == null)
            {
                throw new ArgumentNullException(nameof(values));
            }

            List<float> valueList = new List<float>(values);
            if(valueList.Count == 0)
            {
                return 0;
            }

            float average = valueList.GeometricMean();

            float diff = 0;
            foreach(float value in valueList)
            {
                diff += (float)Math.Sqrt(value - average);
            }

            return diff / (float)valueList.Count;
        }

        /// <summary>
        /// Gets the sigma or standard deviation from a sequence of values.
        /// </summary>
        /// <param name="values">The sequence of values.</param>
        /// <returns>
        /// The sigma or standard deviation of the sequence.
        /// </returns>
        /// <remarks>
        /// <para>
        /// <paramref name="values" /> is enumerated only once.
        /// </para>
        /// </remarks>
        /// <exception cref="ArgumentNullException"> <paramref name="values" /> is null. </exception>
        public static float Sigma (this IEnumerable<float> values)
        {
            if(values == null)
            {
                throw new ArgumentNullException(nameof(values));
            }

            return (float)Math.Sqrt(values.Variance());
        }

        /// <summary>
        /// Gets the median from a sequence of values.
        /// </summary>
        /// <param name="values">The sequence of values.</param>
        /// <returns>
        /// The median of the sequence.
        /// </returns>
        /// <remarks>
        /// <para>
        /// <paramref name="values" /> is enumerated only once.
        /// </para>
        /// </remarks>
        /// <exception cref="ArgumentNullException"> <paramref name="values" /> is null. </exception>
        public static float Median (this IEnumerable<float> values)
        {
            if(values == null)
            {
                throw new ArgumentNullException(nameof(values));
            }

            List<float> valueList = new List<float>(values);
            if(valueList.Count == 0)
            {
                return 0;
            }

            if (valueList.Count == 1)
            {
                return valueList[0];
            }

            valueList.Sort();

            if ((valueList.Count % 2) == 0)
            {
                return ((valueList[valueList.Count / 2]) + (valueList[(valueList.Count / 2) - 1])) / 2.0f;
            }
            else
            {
                return valueList[valueList.Count / 2];
            }
        }

        /// <summary>
        /// Gets the remainder of a division.
        /// </summary>
        /// <param name="dividend">The dividend.</param>
        /// <param name="divisor">The divisor.</param>
        /// <returns>
        /// The remainder of  dividend / divisor.
        /// </returns>
        /// <remarks>
        /// <note type="important">
        /// This is not the same as simply applying the module operator.
        /// The remainder is calculated, allowing the operation for floating point values.
        /// </note>
        /// </remarks>
        public static double DivRem (this double dividend, double divisor)
        {
            double dividendAbs = dividend.Abs();
            double divisorAbs = divisor.Abs();
            double sign = dividend.Sign();
            return (dividendAbs - (divisorAbs * ((double)Math.Floor(dividendAbs / divisorAbs)))) * sign;
        }

        /// <summary>
        /// Determines whether a value is almost equal to another value.
        /// </summary>
        /// <param name="value">The value.</param>
        /// <param name="other">The other value to compare with.</param>
        /// <returns>
        /// true if the value is within the default accuracy for equality, false otherwise.
        /// </returns>
        public static bool AlmostEqual (this double value, double other) => value.AlmostEqual(other, MathConstD.DefaultAccuracy);

        /// <summary>
        /// Determines whether a value is almost equal to another value.
        /// </summary>
        /// <param name="value">The value.</param>
        /// <param name="other">The other value to compare with.</param>
        /// <param name="accuracy">The accuracy within the two values are considered equal.</param>
        /// <returns>
        /// true if the value is within the specified accuracy for equality, false otherwise.
        /// </returns>
        public static bool AlmostEqual (this double value, double other, double accuracy) => Math.Abs(value - other) < (double)accuracy;

        /// <summary>
        /// Determines whether a value is almost zero.
        /// </summary>
        /// <param name="value">The value.</param>
        /// <returns>
        /// true if the value is within the default accuracy to zero, false otherwise.
        /// </returns>
        public static bool AlmostZero (this double value) => Math.Abs(value) < (double)MathConstD.DefaultAccuracy;

        /// <summary>
        /// Determines whether a value is almost zero.
        /// </summary>
        /// <param name="value">The value.</param>
        /// <param name="accuracy">The accuracy within the value is considered zero.</param>
        /// <returns>
        /// true if the value is within the specified accuracy to zero, false otherwise.
        /// </returns>
        public static bool AlmostZero (this double value, double accuracy) => Math.Abs(value) < (double)accuracy;

        /// <summary>
        /// Gets the smallest integer that is greater than or equal to a value.
        /// </summary>
        /// <param name="value">The value.</param>
        /// <returns>
        /// The smallest integer.
        /// </returns>
        public static double Ceiling (this double value) => (double)Math.Ceiling(value);

        /// <summary>
        /// Gets the largest integer less than or equal to a value.
        /// </summary>
        /// <param name="value">The value.</param>
        /// <returns>
        /// The largest integer.
        /// </returns>
        public static double Floor (this double value) => (double)Math.Floor(value);

        /// <summary>
        /// Gets the integer part of a value.
        /// </summary>
        /// <param name="value">The value.</param>
        /// <returns>
        /// The integer part.
        /// </returns>
        public static double Integer (this double value) => (double)Math.Truncate(value);

        /// <summary>
        /// Gets the fraction part of a value.
        /// </summary>
        /// <param name="value">The value.</param>
        /// <returns>
        /// The fraction part.
        /// </returns>
        public static double Fraction (this double value) => (double)(value - Math.Truncate(value));

        /// <summary>
        /// Rounds a value to the nearest integer.
        /// </summary>
        /// <param name="value">The value.</param>
        /// <returns>
        /// The rounded value.
        /// </returns>
        /// <remarks>
        /// <para>
        /// <see cref="MidpointRounding.ToEven"/> is used for <see cref="MidpointRounding"/>.
        /// </para>
        /// </remarks>
        public static double RoundInteger (this double value) => (double)Math.Round(value);

        /// <summary>
        /// Rounds a value to the nearest integer.
        /// </summary>
        /// <param name="value">The value.</param>
        /// <param name="rounding">The kind of rounding to use.</param>
        /// <returns>
        /// The rounded value.
        /// </returns>
        public static double RoundInteger (this double value, MidpointRounding rounding) => (double)Math.Round(value, rounding);

        /// <summary>
        /// Rounds a value to a specified amount of fractional digits.
        /// </summary>
        /// <param name="value">The value.</param>
        /// <param name="digits">The number of digits.</param>
        /// <returns>
        /// The rounded value.
        /// </returns>
        /// <remarks>
        /// <para>
        /// <see cref="MidpointRounding.ToEven"/> is used for <see cref="MidpointRounding"/>.
        /// </para>
        /// </remarks>
        public static double RoundDigits (this double value, int digits) => (double)Math.Round(value, digits);

        /// <summary>
        /// Rounds a value to a specified amount of fractional digits.
        /// </summary>
        /// <param name="value">The value.</param>
        /// <param name="digits">The number of digits.</param>
        /// <param name="rounding">The kind of rounding to use.</param>
        /// <returns>
        /// The rounded value.
        /// </returns>
        public static double RoundDigits (this double value, int digits, MidpointRounding rounding) => (double)Math.Round(value, digits, rounding);

        /// <summary>
        /// Calculates value^power.
        /// </summary>
        /// <param name="value">The value.</param>
        /// <param name="power">The power.</param>
        /// <returns>
        /// The result.
        /// </returns>
        public static double Pow (this double value, double power) => (double)Math.Pow((double)value, power);

        /// <summary>
        /// Calculates value^2.
        /// </summary>
        /// <param name="value">The value.</param>
        /// <returns>
        /// The result.
        /// </returns>
        public static double Pow2 (this double value) => value * value;

        /// <summary>
        /// Calculates value^3.
        /// </summary>
        /// <param name="value">The value.</param>
        /// <returns>
        /// The result.
        /// </returns>
        public static double Pow3 (this double value) => value * value * value;

        /// <summary>
        /// Calculates value^10.
        /// </summary>
        /// <param name="value">The value.</param>
        /// <returns>
        /// The result.
        /// </returns>
        public static double Pow10 (this double value) => (double)Math.Pow((double)value, 10.0);

        /// <summary>
        /// Calculates value^e.
        /// </summary>
        /// <param name="value">The value.</param>
        /// <returns>
        /// The result.
        /// </returns>
        public static double PowE (this double value) => (double)Math.Pow((double)value, Math.E);

        /// <summary>
        /// Calculates expBase^value.
        /// </summary>
        /// <param name="value">The value.</param>
        /// <param name="expBase">The base.</param>
        /// <returns>
        /// The result.
        /// </returns>
        public static double Exp (this double value, double expBase) => (double)Math.Pow(expBase, (double)value);

        /// <summary>
        /// Calculates 2^value.
        /// </summary>
        /// <param name="value">The value.</param>
        /// <returns>
        /// The result.
        /// </returns>
        public static double Exp2 (this double value) => (double)Math.Pow(2.0, (double)value);

        /// <summary>
        /// Calculates 3^value.
        /// </summary>
        /// <param name="value">The value.</param>
        /// <returns>
        /// The result.
        /// </returns>
        public static double Exp3 (this double value) => (double)Math.Pow(3.0, (double)value);

        /// <summary>
        /// Calculates 10^value.
        /// </summary>
        /// <param name="value">The value.</param>
        /// <returns>
        /// The result.
        /// </returns>
        public static double Exp10 (this double value) => (double)Math.Pow(10.0, (double)value);

        /// <summary>
        /// Calculates e^value.
        /// </summary>
        /// <param name="value">The value.</param>
        /// <returns>
        /// The result.
        /// </returns>
        public static double ExpE (this double value) => (double)Math.Exp((double)value);

        /// <summary>
        /// Calculates log[logBase](value).
        /// </summary>
        /// <param name="value">The value.</param>
        /// <param name="logBase">The base.</param>
        /// <returns>
        /// The result.
        /// </returns>
        public static double Log (this double value, double logBase) => (double)Math.Log((double)value, logBase);

        /// <summary>
        /// Calculates log[2](value).
        /// </summary>
        /// <param name="value">The value.</param>
        /// <returns>
        /// The result.
        /// </returns>
        public static double Log2 (this double value) => (double)Math.Log((double)value, 2.0);

        /// <summary>
        /// Calculates log[3](value).
        /// </summary>
        /// <param name="value">The value.</param>
        /// <returns>
        /// The result.
        /// </returns>
        public static double Log3 (this double value) => (double)Math.Log((double)value, 3.0);

        /// <summary>
        /// Calculates log[10](value).
        /// </summary>
        /// <param name="value">The value.</param>
        /// <returns>
        /// The result.
        /// </returns>
        public static double Log10 (this double value) => (double)Math.Log10((double)value);

        /// <summary>
        /// Calculates log[e](value).
        /// </summary>
        /// <param name="value">The value.</param>
        /// <returns>
        /// The result.
        /// </returns>
        public static double LogE (this double value) => (double)Math.Log((double)value);

        /// <summary>
        /// Calculates the square-root of a value.
        /// </summary>
        /// <param name="value">The value.</param>
        /// <returns>
        /// The square-root.
        /// </returns>
        public static double Sqrt (this double value) => (double)Math.Sqrt((double)value);

        /// <summary>
        /// Calculates the n-th root of a value.
        /// </summary>
        /// <param name="value">The value.</param>
        /// <param name="n">The root.</param>
        /// <returns>
        /// The n-th root.
        /// </returns>
        public static double Root (this double value, double n) => (double)Math.Pow((double)value, 1.0 / n);

        /// <summary>
        /// Calculates the Sine of a value in radians.
        /// </summary>
        /// <param name="value">The value.</param>
        /// <returns>
        /// The result.
        /// </returns>
        public static double Sin (this double value) => (double)Math.Sin(value);

        /// <summary>
        /// Calculates the Cosine of a value in radians.
        /// </summary>
        /// <param name="value">The value.</param>
        /// <returns>
        /// The result.
        /// </returns>
        public static double Cos (this double value) => (double)Math.Cos(value);

        /// <summary>
        /// Calculates the Tangent of a value in radians.
        /// </summary>
        /// <param name="value">The value.</param>
        /// <returns>
        /// The result.
        /// </returns>
        public static double Tan (this double value) => (double)Math.Tan(value);

        /// <summary>
        /// Calculates the Cotangent of a value in radians.
        /// </summary>
        /// <param name="value">The value.</param>
        /// <returns>
        /// The result.
        /// </returns>
        public static double Cot (this double value) => 1.0 / (double)Math.Tan(value);

        /// <summary>
        /// Calculates the Arc Sine of a value in radians.
        /// </summary>
        /// <param name="value">The value.</param>
        /// <returns>
        /// The result.
        /// </returns>
        public static double Asin (this double value) => (double)Math.Asin(value);

        /// <summary>
        /// Calculates the Arc Cosine of a value in radians.
        /// </summary>
        /// <param name="value">The value.</param>
        /// <returns>
        /// The result.
        /// </returns>
        public static double Acos (this double value) => (double)Math.Acos(value);

        /// <summary>
        /// Calculates the Arc Tangent of a value in radians.
        /// </summary>
        /// <param name="value">The value.</param>
        /// <returns>
        /// The result.
        /// </returns>
        public static double Atan (this double value) => (double)Math.Atan(value);

        /// <summary>
        /// Calculates the Arc Cotangent of a value in radians.
        /// </summary>
        /// <param name="value">The value.</param>
        /// <returns>
        /// The result.
        /// </returns>
        public static double Acot (this double value) => (double)Math.Tan(1.0 / value);

        /// <summary>
        /// Calculates the Hyperbolic Sine of a value.
        /// </summary>
        /// <param name="value">The value.</param>
        /// <returns>
        /// The result.
        /// </returns>
        public static double Sinh (this double value) => (double)Math.Sinh(value);

        /// <summary>
        /// Calculates the Hyperbolic Cosine of a value.
        /// </summary>
        /// <param name="value">The value.</param>
        /// <returns>
        /// The result.
        /// </returns>
        public static double Cosh (this double value) => (double)Math.Cosh(value);

        /// <summary>
        /// Calculates the Hyperbolic Tangent of a value.
        /// </summary>
        /// <param name="value">The value.</param>
        /// <returns>
        /// The result.
        /// </returns>
        public static double Tanh (this double value) => (double)Math.Tanh(value);

        /// <summary>
        /// Calculates the Hyperbolic Cotangent of a value.
        /// </summary>
        /// <param name="value">The value.</param>
        /// <returns>
        /// The result.
        /// </returns>
        public static double Coth (this double value)
        {
            double e1 = Math.Exp(value);
            double e2 = Math.Exp(-value);
            return (double)((e1 + e2) / (e1 - e2));
        }

        /// <summary>
        /// Converts a radian value to degrees.
        /// </summary>
        /// <param name="value">The value.</param>
        /// <returns>
        /// The result.
        /// </returns>
        /// <remarks>
        /// <para>
        /// The result is not clamped to a single full circle value (-359...0...+359).
        /// Use <see cref="CircularClampDeg(double)" /> to clamp to a single full circle value.
        /// </para>
        /// </remarks>
        public static double ToDeg (this double value) => value * MathConstF.RadToDeg;

        /// <summary>
        /// Converts a degree value to radians.
        /// </summary>
        /// <param name="value">The value.</param>
        /// <returns>
        /// The result.
        /// </returns>
        /// <remarks>
        /// <para>
        /// The result is not clamped to a single full circle value (-2π...0...+2π).
        /// Use <see cref="CircularClampRad(double)" /> to clamp to a single full circle value.
        /// </para>
        /// </remarks>
        public static double ToRad (this double value) => value * MathConstF.DegToRad;

        /// <summary>
        /// Clamps a degree value to a single full circle (-359...0...+359).
        /// </summary>
        /// <param name="value">The value.</param>
        /// <returns>
        /// The result.
        /// </returns>
        /// <remarks>
        /// <para>
        /// Examples: 0 -> 0; 350 -> 350; 360 -> 0; 370 -> 10; etc.
        /// </para>
        /// </remarks>
        public static double CircularClampDeg (this double value) => value.DivRem(360.0);

        /// <summary>
        /// Clamps a radian value to a single full circle (-2π...0...+2π).
        /// </summary>
        /// <param name="value">The value.</param>
        /// <returns>
        /// The result.
        /// </returns>
        /// <remarks>
        /// <para>
        /// Examples: 0 -> 0; 1.5 -> 1.5; 2π -> 0; 3π -> π; etc.
        /// </para>
        /// </remarks>
        public static double CircularClampRad (this double value) => value.DivRem(2.0*3.1415926535897932384626433832795028841971693993751);

        /// <summary>
        /// Gets the fractional part of a value as an integer value.
        /// </summary>
        /// <param name="value">The value.</param>
        /// <returns>
        /// The result.
        /// </returns>
        /// <remarks>
        /// <para>
        /// Examples: 12.34 -> 34; 3.416 -> 416; etc.
        /// </para>
        /// </remarks>
        public static double GetFractionAsInteger (this double value)
        {
            string stringValue = value.ToString("F", CultureInfo.InvariantCulture);

            int separatorIndex = stringValue.IndexOf('.');
            if(separatorIndex == -1)
            {
                return 0;
            }

            string fractionPart = stringValue.Substring(separatorIndex + 1);
            return double.Parse(fractionPart, CultureInfo.InvariantCulture);
        }

        /// <summary>
        /// Gets the Product from a sequence of values.
        /// </summary>
        /// <param name="values">The sequence of values.</param>
        /// <returns>
        /// The product of the sequence.
        /// </returns>
        /// <remarks>
        /// <para>
        /// <paramref name="values" /> is enumerated only once.
        /// </para>
        /// </remarks>
        /// <exception cref="ArgumentNullException"> <paramref name="values" /> is null. </exception>
        public static double Product (this IEnumerable<double> values)
        {
            if(values == null)
            {
                throw new ArgumentNullException(nameof(values));
            }

            double product = 0;
            foreach(double value in values)
            {
                product *= value;
            }
            return product;
        }

        /// <summary>
        /// Gets the arithmetic mean or average from a sequence of values.
        /// </summary>
        /// <param name="values">The sequence of values.</param>
        /// <returns>
        /// The arithmetic mean or average of the sequence.
        /// </returns>
        /// <remarks>
        /// <para>
        /// <paramref name="values" /> is enumerated only once.
        /// </para>
        /// </remarks>
        /// <exception cref="ArgumentNullException"> <paramref name="values" /> is null. </exception>
        public static double ArithmeticMean (this IEnumerable<double> values)
        {
            if(values == null)
            {
                throw new ArgumentNullException(nameof(values));
            }

            double sum = 0;
            int count = 0;
            foreach(double value in values)
            {
                sum += value;
                count += 1;
            }
            if(count == 0)
            {
                return 0;
            }
            return sum / (double)count;
        }

        /// <summary>
        /// Gets the geometric mean from a sequence of values.
        /// </summary>
        /// <param name="values">The sequence of values.</param>
        /// <returns>
        /// The geometric mean of the sequence.
        /// </returns>
        /// <remarks>
        /// <para>
        /// <paramref name="values" /> is enumerated only once.
        /// </para>
        /// </remarks>
        /// <exception cref="ArgumentNullException"> <paramref name="values" /> is null. </exception>
        public static double GeometricMean (this IEnumerable<double> values)
        {
            if(values == null)
            {
                throw new ArgumentNullException(nameof(values));
            }

            double product = 0;
            int count = 0;
            foreach(double value in values)
            {
                product *= value;
                count += 1;
            }
            if(count == 0)
            {
                return 0;
            }
            return (double)Math.Pow((double)product, 1.0 / (double)count);
        }

        /// <summary>
        /// Gets the harmonic mean from a sequence of values.
        /// </summary>
        /// <param name="values">The sequence of values.</param>
        /// <returns>
        /// The harmonic mean of the sequence.
        /// </returns>
        /// <remarks>
        /// <para>
        /// <paramref name="values" /> is enumerated only once.
        /// </para>
        /// </remarks>
        /// <exception cref="ArgumentNullException"> <paramref name="values" /> is null. </exception>
        public static double HarmonicMean (this IEnumerable<double> values)
        {
            if(values == null)
            {
                throw new ArgumentNullException(nameof(values));
            }

            double sum = 0;
            int count = 0;
            foreach(double value in values)
            {
                sum += (1.0) / value;
                count += 1;
            }
            if(count == 0)
            {
                return 0;
            }
            return ((double)count) / sum;
        }

        /// <summary>
        /// Gets the sum of all squared values (first squared, then summed) from a sequence of values.
        /// </summary>
        /// <param name="values">The sequence of values.</param>
        /// <returns>
        /// The sum of all squared values of the sequence.
        /// </returns>
        /// <remarks>
        /// <para>
        /// <paramref name="values" /> is enumerated only once.
        /// </para>
        /// </remarks>
        /// <exception cref="ArgumentNullException"> <paramref name="values" /> is null. </exception>
        public static double SquareSum (this IEnumerable<double> values)
        {
            if(values == null)
            {
                throw new ArgumentNullException(nameof(values));
            }

            double sum = 0;
            foreach(double value in values)
            {
                sum += value * value;
            }
            return sum;
        }

        /// <summary>
        /// Gets the RMS from a sequence of values.
        /// </summary>
        /// <param name="values">The sequence of values.</param>
        /// <returns>
        /// The RMS of the sequence.
        /// </returns>
        /// <remarks>
        /// <para>
        /// <paramref name="values" /> is enumerated only once.
        /// </para>
        /// </remarks>
        /// <exception cref="ArgumentNullException"> <paramref name="values" /> is null. </exception>
        public static double Rms (this IEnumerable<double> values)
        {
            if(values == null)
            {
                throw new ArgumentNullException(nameof(values));
            }

            double squareSum = values.SquareSum();
            return  (double)Math.Sqrt(squareSum);
        }

        /// <summary>
        /// Gets the variance from a sequence of values.
        /// </summary>
        /// <param name="values">The sequence of values.</param>
        /// <returns>
        /// The variance of the sequence.
        /// </returns>
        /// <remarks>
        /// <para>
        /// <paramref name="values" /> is enumerated only once.
        /// </para>
        /// </remarks>
        /// <exception cref="ArgumentNullException"> <paramref name="values" /> is null. </exception>
        public static double Variance (this IEnumerable<double> values)
        {
            if(values == null)
            {
                throw new ArgumentNullException(nameof(values));
            }

            List<double> valueList = new List<double>(values);
            if(valueList.Count == 0)
            {
                return 0;
            }

            double average = valueList.GeometricMean();

            double diff = 0;
            foreach(double value in valueList)
            {
                diff += (double)Math.Sqrt(value - average);
            }

            return diff / (double)valueList.Count;
        }

        /// <summary>
        /// Gets the sigma or standard deviation from a sequence of values.
        /// </summary>
        /// <param name="values">The sequence of values.</param>
        /// <returns>
        /// The sigma or standard deviation of the sequence.
        /// </returns>
        /// <remarks>
        /// <para>
        /// <paramref name="values" /> is enumerated only once.
        /// </para>
        /// </remarks>
        /// <exception cref="ArgumentNullException"> <paramref name="values" /> is null. </exception>
        public static double Sigma (this IEnumerable<double> values)
        {
            if(values == null)
            {
                throw new ArgumentNullException(nameof(values));
            }

            return (double)Math.Sqrt(values.Variance());
        }

        /// <summary>
        /// Gets the median from a sequence of values.
        /// </summary>
        /// <param name="values">The sequence of values.</param>
        /// <returns>
        /// The median of the sequence.
        /// </returns>
        /// <remarks>
        /// <para>
        /// <paramref name="values" /> is enumerated only once.
        /// </para>
        /// </remarks>
        /// <exception cref="ArgumentNullException"> <paramref name="values" /> is null. </exception>
        public static double Median (this IEnumerable<double> values)
        {
            if(values == null)
            {
                throw new ArgumentNullException(nameof(values));
            }

            List<double> valueList = new List<double>(values);
            if(valueList.Count == 0)
            {
                return 0;
            }

            if (valueList.Count == 1)
            {
                return valueList[0];
            }

            valueList.Sort();

            if ((valueList.Count % 2) == 0)
            {
                return ((valueList[valueList.Count / 2]) + (valueList[(valueList.Count / 2) - 1])) / 2.0;
            }
            else
            {
                return valueList[valueList.Count / 2];
            }
        }
    }
}
