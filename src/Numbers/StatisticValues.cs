using System;
using System.Collections.Generic;
using System.Linq;

using RI.Utilities.Exceptions;
using RI.Utilities.ObjectModel;




namespace RI.Utilities.Numbers
{
    /// <summary>
    ///     Contains statistic values for a sequence of numbers.
    /// </summary>
    /// <remarks>
    ///     <note type="note">
    ///         For performance sensitive scenarios, consider using <see cref="RunningValues" />.
    ///     </note>
    /// </remarks>
    /// <threadsafety static="false" instance="false" />
    [Serializable,]
    public struct StatisticValues : ICloneable<StatisticValues>, ICloneable, IEquatable<StatisticValues>
    {
        #region Instance Constructor/Destructor

        /// <summary>
        ///     Creates a new instance of <see cref="StatisticValues" />.
        /// </summary>
        /// <param name="values"> The sequence of numbers the statistics are calculated from. </param>
        /// <remarks>
        ///     <para>
        ///         <paramref name="values" /> is enumerated only once.
        ///     </para>
        ///     <para>
        ///         As no timestep is provided, the timestep is implicitly assumed to be 1.0 for each value.
        ///     </para>
        /// </remarks>
        /// <exception cref="ArgumentNullException"> <paramref name="values" /> is null. </exception>
        /// <exception cref="NotFiniteArgumentException"> <paramref name="values" /> contains one or more NaN or infinity value. </exception>
        public StatisticValues (IEnumerable<double> values)
            : this(values, null, null) { }

        /// <summary>
        ///     Creates a new instance of <see cref="StatisticValues" />.
        /// </summary>
        /// <param name="values"> The sequence of numbers the statistics are calculated from. </param>
        /// <param name="fixedTimestep"> The fixed timestep which is used for each value. </param>
        /// <remarks>
        ///     <para>
        ///         <paramref name="values" /> is enumerated only once.
        ///     </para>
        /// </remarks>
        /// <exception cref="ArgumentNullException"> <paramref name="values" /> is null. </exception>
        /// <exception cref="NotFiniteArgumentException"> <paramref name="values" /> contains one or more NaN or infinity value or <paramref name="fixedTimestep" /> is NaN or infinity. </exception>
        public StatisticValues (IEnumerable<double> values, double fixedTimestep)
            : this(values, null, fixedTimestep) { }

        /// <summary>
        ///     Creates a new instance of <see cref="StatisticValues" />.
        /// </summary>
        /// <param name="values"> The sequence of numbers the statistics are calculated from. </param>
        /// <param name="timesteps"> The sequence of timesteps for each value. </param>
        /// <remarks>
        ///     <para>
        ///         <paramref name="values" /> is enumerated only once.
        ///     </para>
        ///     <para>
        ///         <paramref name="timesteps" /> is enumerated only once.
        ///     </para>
        /// </remarks>
        /// <exception cref="ArgumentNullException"> <paramref name="values" /> or <paramref name="timesteps" /> is null. </exception>
        /// <exception cref="NotFiniteArgumentException"> <paramref name="values" /> or <paramref name="timesteps" /> contains one or more NaN or infinity value. </exception>
        /// <exception cref="ArgumentException"> The number of timesteps does not match the number of values. </exception>
        public StatisticValues (IEnumerable<double> values, IEnumerable<double> timesteps)
            : this(values, timesteps, null)
        {
            if (timesteps == null)
            {
                throw new ArgumentNullException(nameof(timesteps));
            }
        }

        private StatisticValues (IEnumerable<double> values, IEnumerable<double> timesteps, double? fixedTimestep)
        {
            if (values == null)
            {
                throw new ArgumentNullException(nameof(values));
            }

            if (fixedTimestep.HasValue)
            {
                if (fixedTimestep.Value.IsNanOrInfinity())
                {
                    throw new NotFiniteArgumentException(nameof(fixedTimestep));
                }
            }

            Func<double, int, double[]> arrayCreator = (a, b) =>
            {
                double[] array = new double[b];

                for (int i1 = 0; i1 < b; i1++)
                {
                    array[i1] = a;
                }

                return array;
            };

            double[] valueArray = values.ToArray();
            double[] timestepArray = timesteps?.ToArray() ?? arrayCreator(fixedTimestep.GetValueOrDefault(1.0), valueArray.Length);

            if (valueArray.Length != timestepArray.Length)
            {
                throw new ArgumentException("The number of timesteps does not match the number of values.", nameof(timesteps));
            }

            this.Values = valueArray;
            this.Timesteps = timestepArray;

            this.WeightedValues = new double[this.Values.Length];

            this.Count = 0;
            this.Duration = 0.0;
            this.Sum = 0.0;
            this.ReciprocalSum = 0.0;
            this.SquareSum = 0.0;
            this.Product = 1.0;

            this.Min = double.MaxValue;
            this.Max = double.MinValue;

            for (int i1 = 0; i1 < this.Values.Length; i1++)
            {
                double value = this.Values[i1];
                double timestep = this.Timesteps[i1];

                if (value.IsNanOrInfinity())
                {
                    throw new NotFiniteArgumentException(nameof(values));
                }

                if (timestep.IsNanOrInfinity())
                {
                    throw new NotFiniteArgumentException(nameof(timesteps));
                }

                double weightedValue = value * timestep;

                this.WeightedValues[i1] = weightedValue;
                this.Count++;
                this.Duration += timestep;

                this.Sum += weightedValue;
                this.ReciprocalSum += 1.0 / weightedValue;
                this.SquareSum += weightedValue * weightedValue;
                this.Product *= weightedValue;

                if (this.Min > weightedValue)
                {
                    this.Min = weightedValue;
                }

                if (this.Max < weightedValue)
                {
                    this.Max = weightedValue;
                }
            }

            this.SortedValues = (double[])this.Values.Clone();
            Array.Sort(this.SortedValues);

            this.SortedTimesteps = (double[])this.Timesteps.Clone();
            Array.Sort(this.SortedTimesteps);

            this.WeightedSortedValues = (double[])this.WeightedValues.Clone();
            Array.Sort(this.WeightedSortedValues);

            if (this.Count == 0)
            {
                this.Product = 0.0;

                this.Min = double.NaN;
                this.Max = double.NaN;

                this.Rms = 0.0;
                this.ArithmeticMean = 0.0;
                this.GeometricMean = 0.0;
                this.HarmonicMean = 0.0;

                this.Variance = 0.0;
                this.Sigma = 0.0;
            }
            else
            {
                this.Rms = Math.Sqrt(this.SquareSum / this.Duration);
                this.ArithmeticMean = this.Sum / this.Duration;
                this.GeometricMean = Math.Pow(this.Product, 1.0 / this.Duration);
                this.HarmonicMean = this.Duration / this.ReciprocalSum;

                double diff = 0.0;

                foreach (double weightedValue in this.WeightedValues)
                {
                    diff += Math.Pow(weightedValue - this.ArithmeticMean, 2.0);
                }

                this.Variance = diff / this.Duration;

                this.Sigma = Math.Sqrt(this.Variance);
            }

            if (this.Count == 0)
            {
                this.Median = double.NaN;
            }
            else if (this.Count == 1)
            {
                this.Median = this.Sum;
            }
            else if ((this.Count % 2) == 0)
            {
                this.Median = (this.WeightedSortedValues[this.Count / 2] + this.WeightedSortedValues[(this.Count / 2) - 1]) / 2.0;
            }
            else
            {
                this.Median = this.WeightedSortedValues[this.Count / 2];
            }
        }

        #endregion




        #region Instance Fields

        /// <summary>
        ///     The arithmetic mean or average of all values.
        /// </summary>
        public readonly double ArithmeticMean;

        /// <summary>
        ///     The number of values.
        /// </summary>
        public readonly int Count;

        /// <summary>
        ///     The sum of all timesteps.
        /// </summary>
        public readonly double Duration;

        /// <summary>
        ///     The geometric mean of all values.
        /// </summary>
        public readonly double GeometricMean;

        /// <summary>
        ///     The harmonic mean of all values.
        /// </summary>
        public readonly double HarmonicMean;

        /// <summary>
        ///     The largest value of all values.
        /// </summary>
        /// <remarks>
        ///     <note type="note">
        ///         This value is <see cref="double.NaN" /> after one of the parameterized constructors is used with an empty sequence of numbers.
        ///     </note>
        /// </remarks>
        public readonly double Max;

        /// <summary>
        ///     The median of all values.
        /// </summary>
        /// <remarks>
        ///     <note type="note">
        ///         This value is <see cref="double.NaN" /> after one of the parameterized constructors is used with a sequence of numbers of less than two numbers.
        ///     </note>
        /// </remarks>
        public readonly double Median;

        /// <summary>
        ///     The smallest value of all values.
        /// </summary>
        /// <remarks>
        ///     <note type="note">
        ///         This value is <see cref="double.NaN" /> after one of the parameterized constructors is used with an empty sequence of numbers.
        ///     </note>
        /// </remarks>
        public readonly double Min;

        /// <summary>
        ///     The product of all values.
        /// </summary>
        public readonly double Product;

        /// <summary>
        ///     The sum of all reciprocal values.
        /// </summary>
        public readonly double ReciprocalSum;

        /// <summary>
        ///     The root-mean-square (RMS) of all values.
        /// </summary>
        public readonly double Rms;

        /// <summary>
        ///     The sigma or standard deviation of all values.
        /// </summary>
        public readonly double Sigma;

        /// <summary>
        ///     All timesteps, sorted by their numeric value.
        /// </summary>
        public readonly double[] SortedTimesteps;

        /// <summary>
        ///     All values, sorted by their numeric value.
        /// </summary>
        public readonly double[] SortedValues;

        /// <summary>
        ///     The sum of all squared values (first squared, then summed).
        /// </summary>
        public readonly double SquareSum;

        /// <summary>
        ///     The sum of all values.
        /// </summary>
        public readonly double Sum;

        /// <summary>
        ///     The timesteps of each value.
        /// </summary>
        public readonly double[] Timesteps;

        /// <summary>
        ///     All values.
        /// </summary>
        public readonly double[] Values;

        /// <summary>
        ///     The variance of all values.
        /// </summary>
        public readonly double Variance;

        /// <summary>
        ///     All values, multiplied by their corresponding timestep and then sorted by their numeric value.
        /// </summary>
        public readonly double[] WeightedSortedValues;

        /// <summary>
        ///     All values, multiplied by their corresponding timestep.
        /// </summary>
        public readonly double[] WeightedValues;

        #endregion




        #region Overrides

        /// <inheritdoc />
        public override bool Equals (object obj)
        {
            return (obj != null) && this.Equals((StatisticValues)obj);
        }

        /// <inheritdoc />
        public override int GetHashCode ()
        {
            return unchecked((int)(this.Timesteps.Sum() + this.Values.Sum()));
        }

        #endregion




        #region Interface: ICloneable<StatisticValues>

        /// <inheritdoc />
        public StatisticValues Clone ()
        {
            return new StatisticValues(this.Values, this.Timesteps);
        }

        /// <inheritdoc />
        object ICloneable.Clone ()
        {
            return this.Clone();
        }

        #endregion




        #region Interface: IEquatable<StatisticValues>

        /// <inheritdoc />
        public bool Equals (StatisticValues other)
        {
            if ((other.Timesteps.Length != this.Timesteps.Length) || (other.Values.Length != this.Values.Length))
            {
                return false;
            }

            for (int i1 = 0; i1 < this.Timesteps.Length; i1++)
            {
                if (Math.Abs(other.Timesteps[i1] - this.Timesteps[i1]) > double.Epsilon)
                {
                    return false;
                }
            }

            for (int i1 = 0; i1 < this.Values.Length; i1++)
            {
                if (Math.Abs(other.Values[i1] - this.Values[i1]) > double.Epsilon)
                {
                    return false;
                }
            }

            return true;
        }

        #endregion
    }
}
