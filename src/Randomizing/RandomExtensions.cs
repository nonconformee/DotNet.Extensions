using System;
using System.Collections.Generic;
using System.IO;
using System.Text;

using RI.Utilities.Exceptions;
using RI.Utilities.Numbers;




namespace RI.Utilities.Randomizing
{
    /// <summary>
    ///     Provides utility/extension methods for the <see cref="Random" /> type.
    /// </summary>
    /// <threadsafety static="false" instance="false" />
    public static class RandomExtensions
    {
        #region Constants

        private const string LoremIpsumRaw = "Lorem ipsum dolor sit amet, consectetur adipiscing elit. Integer nec odio. Praesent libero. Sed cursus ante dapibus diam. Sed nisi. Nulla quis sem at nibh elementum imperdiet. Duis sagittis ipsum. Praesent mauris. Fusce nec tellus sed augue semper porta. Mauris massa. Vestibulum lacinia arcu eget nulla. Class aptent taciti sociosqu ad litora torquent per conubia nostra, per inceptos himenaeos. Curabitur sodales ligula in libero. Sed dignissim lacinia nunc. Curabitur tortor. Pellentesque nibh. Aenean quam. In scelerisque sem at dolor. Maecenas mattis. Sed convallis tristique sem. Proin ut ligula vel nunc egestas porttitor. Morbi lectus risus, iaculis vel, suscipit quis, luctus non, massa. Fusce ac turpis quis ligula lacinia aliquet. Mauris ipsum. Nulla metus metus, ullamcorper vel, tincidunt sed, euismod in, nibh. Quisque volutpat condimentum velit. Class aptent taciti sociosqu ad litora torquent per conubia nostra, per inceptos himenaeos. Nam nec ante. Sed lacinia, urna non tincidunt mattis, tortor neque adipiscing diam, a cursus ipsum ante quis turpis. Nulla facilisi. Ut fringilla. Suspendisse potenti. Nunc feugiat mi a tellus consequat imperdiet. Vestibulum sapien. Proin quam. Etiam ultrices. Suspendisse in justo eu magna luctus suscipit. Sed lectus. Integer euismod lacus luctus magna. Quisque cursus, metus vitae pharetra auctor, sem massa mattis sem, at interdum magna augue eget diam. Vestibulum ante ipsum primis in faucibus orci luctus et ultrices posuere cubilia Curae; Morbi lacinia molestie dui. Praesent blandit dolor. Sed non quam. In vel mi sit amet augue congue elementum. Morbi in ipsum sit amet pede facilisis laoreet. Donec lacus nunc, viverra nec, blandit vel, egestas et, augue. Vestibulum tincidunt malesuada tellus. Ut ultrices ultrices enim. Curabitur sit amet mauris. Morbi in dui quis est pulvinar ullamcorper. Nulla facilisi. Integer lacinia sollicitudin massa. Cras metus. Sed aliquet risus a tortor. Integer id quam. Morbi mi. Quisque nisl felis, venenatis tristique, dignissim in, ultrices sit amet, augue. Proin sodales libero eget ante. Nulla quam. Aenean laoreet. Vestibulum nisi lectus, commodo ac, facilisis ac, ultricies eu, pede. Ut orci risus, accumsan porttitor, cursus quis, aliquet eget, justo. Sed pretium blandit orci. Ut eu diam at pede suscipit sodales. Aenean lectus elit, fermentum non, convallis id, sagittis at, neque. Nullam mauris orci, aliquet et, iaculis et, viverra vitae, ligula. Nulla ut felis in purus aliquam imperdiet. Maecenas aliquet mollis lectus. Vivamus consectetuer risus et tortor. Lorem ipsum dolor sit amet, consectetur adipiscing elit. Integer nec odio. Praesent libero. Sed cursus ante dapibus diam. Sed nisi. Nulla quis sem at nibh elementum imperdiet. Duis sagittis ipsum. Praesent mauris. Fusce nec tellus sed augue semper porta. Mauris massa. Vestibulum lacinia arcu eget nulla. Class aptent taciti sociosqu ad litora torquent per conubia nostra, per inceptos himenaeos. Curabitur sodales ligula in libero. Sed dignissim lacinia nunc. Curabitur tortor. Pellentesque nibh. Aenean quam. In scelerisque sem at dolor. Maecenas mattis. Sed convallis tristique sem. Proin ut ligula vel nunc egestas porttitor. Morbi lectus risus, iaculis vel, suscipit quis, luctus non, massa. Fusce ac turpis quis ligula lacinia aliquet. Mauris ipsum. Nulla metus metus, ullamcorper vel, tincidunt sed, euismod in, nibh. Quisque volutpat condimentum velit. Class aptent taciti sociosqu ad litora torquent per conubia nostra, per inceptos himenaeos. Nam nec ante. Sed lacinia, urna non tincidunt mattis, tortor neque adipiscing diam, a cursus ipsum ante quis turpis. Nulla facilisi. Ut fringilla. Suspendisse potenti. Nunc feugiat mi a tellus consequat imperdiet. Vestibulum sapien. Proin quam. Etiam ultrices. Suspendisse in justo eu magna luctus suscipit. Sed lectus. Integer euismod lacus luctus magna. Quisque cursus, metus vitae pharetra auctor, sem massa mattis sem, at interdum magna augue eget diam. Vestibulum ante ipsum primis in faucibus orci luctus et ultrices posuere cubilia Curae; Morbi lacinia molestie dui. Praesent blandit dolor. Sed non quam. In vel mi sit amet augue congue elementum. Morbi in ipsum sit amet pede facilisis laoreet. Donec lacus nunc, viverra nec, blandit vel, egestas et, augue. Vestibulum tincidunt malesuada tellus. Ut ultrices ultrices enim. Curabitur sit amet mauris. Morbi in dui quis est pulvinar ullamcorper. Nulla facilisi. Integer lacinia sollicitudin massa. Cras metus. Sed aliquet risus a tortor. Integer id quam. Morbi mi. Quisque nisl felis, venenatis tristique, dignissim in, ultrices sit amet, augue. Proin sodales libero eget ante. Nulla quam. Aenean laoreet. Vestibulum nisi lectus, commodo ac, facilisis ac, ultricies eu, pede. Ut orci risus, accumsan porttitor, cursus quis, aliquet eget, justo. Sed pretium blandit orci. Ut eu diam at pede suscipit sodales. Aenean lectus elit, fermentum non, convallis id, sagittis at, neque. Nullam mauris orci, aliquet et, iaculis et, viverra vitae, ligula. Nulla ut felis in purus aliquam imperdiet. Maecenas aliquet mollis lectus. Vivamus consectetuer risus et tortor. Lorem ipsum dolor sit amet, consectetur adipiscing elit. Integer nec odio. Praesent libero. Sed cursus ante dapibus diam. Sed nisi. Nulla quis sem at nibh elementum imperdiet. Duis sagittis ipsum. Praesent mauris. Fusce nec tellus sed augue semper porta. Mauris massa. Vestibulum lacinia arcu eget nulla. Class aptent taciti sociosqu ad litora torquent per conubia nostra, per inceptos himenaeos. Curabitur sodales ligula in libero. Sed dignissim lacinia nunc. Curabitur tortor. Pellentesque nibh. Aenean quam. In scelerisque sem at dolor. Maecenas mattis. Sed convallis tristique sem. Proin ut ligula vel nunc egestas porttitor. Morbi lectus risus, iaculis vel, suscipit quis, luctus non, massa. Fusce ac turpis quis ligula lacinia aliquet. Mauris ipsum. Nulla metus metus, ullamcorper vel, tincidunt sed, euismod in, nibh. Quisque volutpat condimentum velit. Class aptent taciti sociosqu ad litora torquent per conubia nostra, per inceptos himenaeos. Nam nec ante. Sed lacinia, urna non tincidunt mattis, tortor neque adipiscing diam, a cursus ipsum ante quis turpis. Nulla facilisi. Ut fringilla. Suspendisse potenti. Nunc feugiat mi a tellus consequat imperdiet. Vestibulum sapien. Proin quam. Etiam ultrices. Suspendisse in justo eu magna luctus suscipit. Sed lectus. Integer euismod lacus luctus magna. Quisque cursus, metus vitae pharetra auctor, sem massa mattis sem, at interdum magna augue eget diam. Vestibulum ante ipsum primis in faucibus orci luctus et ultrices posuere cubilia Curae; Morbi lacinia molestie dui. Praesent blandit dolor. Sed non quam. In vel mi sit amet augue congue elementum. Morbi in ipsum si.";

        #endregion




        #region Static Properties/Indexer

        private static object LoremIpsumGenerateLock { get; } = new object();

        private static List<string> LoremIpsumWords { get; } = new List<string>();

        #endregion




        #region Static Methods

        /// <summary>
        ///     Fills a <see cref="Stream" /> with random bytes.
        /// </summary>
        /// <param name="randomizer"> The randomizer to use. </param>
        /// <param name="stream"> The <see cref="Stream" /> to fill. </param>
        /// <param name="length"> The amount of bytes to fill in the <see cref="Stream" /> at its current position. </param>
        /// <returns>
        ///     The number of written bytes to the <see cref="Stream" />.
        /// </returns>
        /// <exception cref="ArgumentNullException"> <paramref name="randomizer" /> or <paramref name="stream" /> is null. </exception>
        /// <exception cref="NotWriteableStreamArgumentException"> <paramref name="stream" /> is not writeable. </exception>
        /// <exception cref="ArgumentOutOfRangeException"> <paramref name="length" /> is less than zero. </exception>
        public static int FillStream (this Random randomizer, Stream stream, int length)
        {
            if (randomizer == null)
            {
                throw new ArgumentNullException(nameof(randomizer));
            }

            if (stream == null)
            {
                throw new ArgumentNullException(nameof(stream));
            }

            if (!stream.CanWrite)
            {
                throw new NotWriteableStreamArgumentException(nameof(stream));
            }

            if (length < 0)
            {
                throw new ArgumentOutOfRangeException(nameof(length));
            }

            for (int i1 = 0; i1 < length; i1++)
            {
                stream.WriteByte(randomizer.NextByte());
            }

            return length;
        }

        /// <summary>
        ///     Returns random true or false.
        /// </summary>
        /// <param name="randomizer"> The randomizer to use. </param>
        /// <returns>
        ///     true or false.
        /// </returns>
        /// <exception cref="ArgumentNullException"> <paramref name="randomizer" /> is null. </exception>
        public static bool NextBoolean (this Random randomizer)
        {
            if (randomizer == null)
            {
                throw new ArgumentNullException(nameof(randomizer));
            }

            return randomizer.Next(0, 2) == 0;
        }

        /// <summary>
        ///     Gets a random byte value.
        /// </summary>
        /// <param name="randomizer"> The randomizer to use. </param>
        /// <returns>
        ///     A random byte value between 0 (inclusive) and 255 (inclusive).
        /// </returns>
        /// <exception cref="ArgumentNullException"> <paramref name="randomizer" /> is null. </exception>
        public static byte NextByte (this Random randomizer)
        {
            return (byte)randomizer.Next(0, 256);
        }

        /// <summary>
        ///     Gets a random byte value.
        /// </summary>
        /// <param name="randomizer"> The randomizer to use. </param>
        /// <param name="max"> The allowed maximum value (inclusive). </param>
        /// <returns>
        ///     A random byte value between 0 (inclusive) and <paramref name="max" /> (inclusive).
        /// </returns>
        /// <exception cref="ArgumentNullException"> <paramref name="randomizer" /> is null. </exception>
        public static byte NextByte (this Random randomizer, byte max)
        {
            return (byte)randomizer.Next(0, max + 1);
        }

        /// <summary>
        ///     Gets a random byte value.
        /// </summary>
        /// <param name="randomizer"> The randomizer to use. </param>
        /// <param name="min"> The allowed minimum value (inclusive). </param>
        /// <param name="max"> The allowed maximum value (inclusive). </param>
        /// <returns>
        ///     A random byte value between <paramref name="min" /> (inclusive) and <paramref name="max" /> (inclusive).
        /// </returns>
        /// <exception cref="ArgumentNullException"> <paramref name="randomizer" /> is null. </exception>
        public static byte NextByte (this Random randomizer, byte min, byte max)
        {
            if (randomizer == null)
            {
                throw new ArgumentNullException(nameof(randomizer));
            }

            return (byte)randomizer.Next(min, max + 1);
        }

        /// <summary>
        ///     Fills a byte array with random values.
        /// </summary>
        /// <param name="randomizer"> The randomizer to use. </param>
        /// <param name="buffer"> The byte array to fill. </param>
        /// <param name="offset"> The offset in the byte array at which the random fill starts. </param>
        /// <exception cref="ArgumentNullException"> <paramref name="randomizer" /> is null. </exception>
        /// <exception cref="ArgumentOutOfRangeException"> <paramref name="offset" /> is less than zero or outside the length of the array. </exception>
        public static void NextBytes (this Random randomizer, byte[] buffer, int offset)
        {
            if (randomizer == null)
            {
                throw new ArgumentNullException(nameof(randomizer));
            }

            if ((offset < 0) || (offset >= buffer.Length))
            {
                throw new ArgumentOutOfRangeException(nameof(offset));
            }

            randomizer.NextBytes(buffer, offset, buffer.Length - offset);
        }

        /// <summary>
        ///     Fills a byte array with random values.
        /// </summary>
        /// <param name="randomizer"> The randomizer to use. </param>
        /// <param name="buffer"> The byte array to fill. </param>
        /// <param name="offset"> The offset in the byte array at which the random fill starts. </param>
        /// <param name="count"> The number of bytes to fill with random values, beginning at <paramref name="offset" />. </param>
        /// <exception cref="ArgumentNullException"> <paramref name="randomizer" /> is null. </exception>
        /// <exception cref="ArgumentOutOfRangeException"> <paramref name="offset" /> or <paramref name="count" /> is less than zero or the range specified by <paramref name="offset" /> and <paramref name="count" /> is outside the length of the array. </exception>
        public static void NextBytes (this Random randomizer, byte[] buffer, int offset, int count)
        {
            if (randomizer == null)
            {
                throw new ArgumentNullException(nameof(randomizer));
            }

            if ((offset < 0) || (offset >= buffer.Length))
            {
                throw new ArgumentOutOfRangeException(nameof(offset));
            }

            if ((count < 0) || ((offset + count) > buffer.Length))
            {
                throw new ArgumentOutOfRangeException(nameof(count));
            }

            for (int i1 = offset; i1 < (offset + count); i1++)
            {
                buffer[i1] = (byte)randomizer.Next(0, 256);
            }
        }

        /// <summary>
        ///     Gets a byte array of a specified length filled with random bytes.
        /// </summary>
        /// <param name="randomizer"> The randomizer to use. </param>
        /// <param name="length"> The number of randomized bytes in the array. </param>
        /// <returns>
        ///     The byte array which contains the specified number of randomized bytes.
        /// </returns>
        /// <exception cref="ArgumentNullException"> <paramref name="randomizer" /> is null. </exception>
        /// <exception cref="ArgumentOutOfRangeException"> <paramref name="length" /> is less than zero. </exception>
        public static byte[] NextBytes (this Random randomizer, int length)
        {
            if (randomizer == null)
            {
                throw new ArgumentNullException(nameof(randomizer));
            }

            if (length < 0)
            {
                throw new ArgumentOutOfRangeException(nameof(length));
            }

            byte[] array = new byte[length];
            randomizer.NextBytes(array);
            return array;
        }

        /// <summary>
        ///     Returns random true or false depending on a specified chance.
        /// </summary>
        /// <param name="randomizer"> The randomizer to use. </param>
        /// <param name="chance"> The chance to return true, between 0.0 (never) and 1.0 (always). </param>
        /// <returns>
        ///     true or false based on the specified chance.
        /// </returns>
        /// <exception cref="ArgumentNullException"> <paramref name="randomizer" /> is null. </exception>
        /// <exception cref="NotFiniteNumberException"> <paramref name="chance" /> is either "NaN"/"Not-a-Number" or infinity (positive or negative). </exception>
        public static bool NextChance (this Random randomizer, double chance)
        {
            if (randomizer == null)
            {
                throw new ArgumentNullException(nameof(randomizer));
            }

            if (chance.IsNanOrInfinity())
            {
                throw new NotFiniteNumberException(nameof(chance));
            }

            double value = randomizer.NextDouble();

            return value < chance;
        }

        /// <summary>
        ///     Gets a random date and time value.
        /// </summary>
        /// <param name="randomizer"> The randomizer to use. </param>
        /// <returns>
        ///     A random date and time value between 0001-01-01 00:00:00 (inclusive) and 9999-12-31 23:59:59 (exclusive).
        /// </returns>
        /// <exception cref="ArgumentNullException"> <paramref name="randomizer" /> is null. </exception>
        public static DateTime NextDateTime (this Random randomizer)
        {
            return randomizer.NextDateTime(DateTime.MinValue, DateTime.MaxValue);
        }

        /// <summary>
        ///     Gets a random date and time value.
        /// </summary>
        /// <param name="randomizer"> The randomizer to use. </param>
        /// <param name="min"> The allowed minimum date and time (inclusive). </param>
        /// <param name="max"> The allowed maximum date and time (exclusive). </param>
        /// <returns>
        ///     A random date and time value between <paramref name="min" /> (inclusive) and <paramref name="max" /> (exclusive).
        /// </returns>
        /// <exception cref="ArgumentNullException"> <paramref name="randomizer" /> is null. </exception>
        public static DateTime NextDateTime (this Random randomizer, DateTime min, DateTime max)
        {
            if (randomizer == null)
            {
                throw new ArgumentNullException(nameof(randomizer));
            }

            double ticks = randomizer.NextDouble(min.Ticks, max.Ticks);
            return new DateTime((long)ticks);
        }

        /// <summary>
        ///     Gets a random double precision floating point value.
        /// </summary>
        /// <param name="randomizer"> The randomizer to use. </param>
        /// <param name="max"> The allowed maximum value (exclusive). </param>
        /// <returns>
        ///     A random double precision floating point value between 0.0 (inclusive) and <paramref name="max" /> (exclusive).
        /// </returns>
        /// <exception cref="ArgumentNullException"> <paramref name="randomizer" /> is null. </exception>
        public static double NextDouble (this Random randomizer, double max)
        {
            return randomizer.NextDouble(0.0, max);
        }

        /// <summary>
        ///     Gets a random double precision floating point value.
        /// </summary>
        /// <param name="randomizer"> The randomizer to use. </param>
        /// <param name="min"> The allowed minimum value (inclusive). </param>
        /// <param name="max"> The allowed maximum value (exclusive). </param>
        /// <returns>
        ///     A random double precision floating point value between <paramref name="min" /> (inclusive) and <paramref name="max" /> (exclusive).
        /// </returns>
        /// <exception cref="ArgumentNullException"> <paramref name="randomizer" /> is null. </exception>
        /// <exception cref="NotFiniteNumberException"> <paramref name="min" /> or <paramref name="max" /> is either "NaN"/"Not-a-Number" or infinity (positive or negative). </exception>
        public static double NextDouble (this Random randomizer, double min, double max)
        {
            if (randomizer == null)
            {
                throw new ArgumentNullException(nameof(randomizer));
            }

            if (min.IsNanOrInfinity())
            {
                throw new NotFiniteNumberException(nameof(min));
            }

            if (max.IsNanOrInfinity())
            {
                throw new NotFiniteNumberException(nameof(max));
            }

            return min + (randomizer.NextDouble() * (max - min));
        }

        /// <summary>
        ///     Gets a random single precision floating point value.
        /// </summary>
        /// <param name="randomizer"> The randomizer to use. </param>
        /// <param name="max"> The allowed maximum value (exclusive). </param>
        /// <returns>
        ///     A random single precision floating point value between 0.0 (inclusive) and <paramref name="max" /> (exclusive).
        /// </returns>
        /// <exception cref="ArgumentNullException"> <paramref name="randomizer" /> is null. </exception>
        public static float NextFloat (this Random randomizer, float max)
        {
            return randomizer.NextFloat(0.0f, max);
        }

        /// <summary>
        ///     Gets a random single precision floating point value.
        /// </summary>
        /// <param name="randomizer"> The randomizer to use. </param>
        /// <param name="min"> The allowed minimum value (inclusive). </param>
        /// <param name="max"> The allowed maximum value (exclusive). </param>
        /// <returns>
        ///     A random single precision floating point value between <paramref name="min" /> (inclusive) and <paramref name="max" /> (exclusive).
        /// </returns>
        /// <exception cref="ArgumentNullException"> <paramref name="randomizer" /> is null. </exception>
        /// <exception cref="NotFiniteNumberException"> <paramref name="min" /> or <paramref name="max" /> is either "NaN"/"Not-a-Number" or infinity (positive or negative). </exception>
        public static float NextFloat (this Random randomizer, float min, float max)
        {
            if (randomizer == null)
            {
                throw new ArgumentNullException(nameof(randomizer));
            }

            if (min.IsNanOrInfinity())
            {
                throw new NotFiniteNumberException(nameof(min));
            }

            if (max.IsNanOrInfinity())
            {
                throw new NotFiniteNumberException(nameof(max));
            }

            return min + (randomizer.NextFloat() * (max - min));
        }

        /// <summary>
        ///     Gets a random single precision floating point value.
        /// </summary>
        /// <param name="randomizer"> The randomizer to use. </param>
        /// <returns>
        ///     A random single precision floating point value between 0.0 (inclusive) and 1.0 (exclusive).
        /// </returns>
        /// <exception cref="ArgumentNullException"> <paramref name="randomizer" /> is null. </exception>
        public static float NextFloat (this Random randomizer)
        {
            if (randomizer == null)
            {
                throw new ArgumentNullException(nameof(randomizer));
            }

            return (float)randomizer.NextDouble();
        }

        /// <summary>
        ///     Gets a normally distributed random number.
        /// </summary>
        /// <param name="randomizer"> The randomizer to use. </param>
        /// <param name="mu"> The distributions mean. </param>
        /// <param name="sigma"> The standard deviation of the distribution. </param>
        /// <returns>
        ///     A normally distributed random double precision floating point value.
        /// </returns>
        /// <remarks>
        ///     <para>
        ///         See <see href="https://en.wikipedia.org/wiki/Box-Muller_transform"> https://en.wikipedia.org/wiki/Box-Muller_transform </see> for details about the used algorithm.
        ///     </para>
        /// </remarks>
        /// <exception cref="ArgumentNullException"> <paramref name="randomizer" /> is null. </exception>
        /// <exception cref="NotFiniteNumberException"> <paramref name="mu" /> or <paramref name="sigma" /> is either "NaN"/"Not-a-Number" or infinity (positive or negative). </exception>
        public static double NextGaussian (this Random randomizer, double mu, double sigma)
        {
            if (randomizer == null)
            {
                throw new ArgumentNullException(nameof(randomizer));
            }

            if (mu.IsNanOrInfinity())
            {
                throw new NotFiniteNumberException(nameof(mu));
            }

            if (sigma.IsNanOrInfinity())
            {
                throw new NotFiniteNumberException(nameof(sigma));
            }

            double u1 = randomizer.NextDouble();
            double u2 = randomizer.NextDouble();
            double distribution = Math.Sqrt(-2.0 * Math.Log(u1)) * Math.Sin(2.0 * Math.PI * u2);
            return mu + (sigma * distribution);
        }

        /// <summary>
        ///     Generates a random sentence of readable text.
        /// </summary>
        /// <param name="randomizer"> The randomizer to use. </param>
        /// <param name="words"> The number of words in the sentence. </param>
        /// <param name="startWithLoremIpsum"> Indicates whether the sentence should start with &quot;lorem ipsum dolor sit amet&quot;. </param>
        /// <param name="startWithCapital"> Indicates whether the first letter of the sentence should be a capital letter. </param>
        /// <param name="endWithPeriod"> Indicates whether the sentence should end with a period. </param>
        /// <returns>
        ///     The string with the amount of specified words.
        /// </returns>
        /// <remarks>
        ///     <para>
        ///         The returned string is a text in a pseudo-language which has no real meaning but appears to be &quot;real&quot; (somewhat resembling latin).
        ///         For example: Sed cursus ante dapibus diam.
        ///         See <see href="https://en.wikipedia.org/wiki/Lorem_ipsum"> https://en.wikipedia.org/wiki/Lorem_ipsum </see> for details about &quot;Lorem ipsum&quot;.
        ///     </para>
        ///     <para>
        ///         The first call of <see cref="NextLoremIpsum(Random, int, bool, bool, bool)" /> takes longer time as subsequent calls because the list of available words is prepared during the first call.
        ///     </para>
        /// </remarks>
        /// <exception cref="ArgumentNullException"> <paramref name="randomizer" /> is null. </exception>
        /// <exception cref="ArgumentOutOfRangeException"> <paramref name="words" /> is less than zero or <paramref name="startWithLoremIpsum" /> is true and <paramref name="words" /> is less than five. </exception>
        public static string NextLoremIpsum (this Random randomizer, int words, bool startWithLoremIpsum, bool startWithCapital, bool endWithPeriod)
        {
            if (randomizer == null)
            {
                throw new ArgumentNullException(nameof(randomizer));
            }

            if (words < 0)
            {
                throw new ArgumentOutOfRangeException(nameof(words));
            }

            if (startWithLoremIpsum && (words < 5))
            {
                throw new ArgumentOutOfRangeException(nameof(words));
            }

            if (words == 0)
            {
                return string.Empty;
            }

            lock (RandomExtensions.LoremIpsumGenerateLock)
            {
                if (RandomExtensions.LoremIpsumWords.Count == 0)
                {
                    StringBuilder currentWord = new StringBuilder();

                    for (int i1 = 0; i1 < RandomExtensions.LoremIpsumRaw.Length; i1++)
                    {
                        char currentChar = RandomExtensions.LoremIpsumRaw[i1];

                        if (char.IsLetter(currentChar))
                        {
                            currentWord.Append(char.ToLowerInvariant(currentChar));
                        }
                        else
                        {
                            if (currentWord.Length >= 2)
                            {
                                RandomExtensions.LoremIpsumWords.Add(currentWord.ToString());
                            }

                            currentWord.Remove(0, currentWord.Length);
                        }
                    }
                }
            }

            StringBuilder result = new StringBuilder();
            int remainingWords = words;
            bool started = false;

            if (startWithLoremIpsum)
            {
                string start = "lorem ipsum dolor sit amet";

                if (startWithCapital)
                {
                    start = char.ToUpperInvariant(start[0]) + start.Substring(1);
                }

                result.Append(start);
                remainingWords -= 5;
                started = true;
            }

            for (int i1 = 0; i1 < remainingWords; i1++)
            {
                string word;

                lock (RandomExtensions.LoremIpsumGenerateLock)
                {
                    word = RandomExtensions.LoremIpsumWords[randomizer.Next(0, RandomExtensions.LoremIpsumWords.Count)];
                }

                if (!started && startWithCapital)
                {
                    word = char.ToUpperInvariant(word[0]) + word.Substring(1);
                }

                if (started)
                {
                    result.Append(" ");
                }

                result.Append(word);
                started = true;
            }

            if (endWithPeriod)
            {
                result.Append(".");
            }

            return result.ToString();
        }

        /// <summary>
        ///     Gets a random time span value.
        /// </summary>
        /// <param name="randomizer"> The randomizer to use. </param>
        /// <returns>
        ///     A random time span value between 0 (inclusive) and approx. 10'675'199 days (exclusive).
        /// </returns>
        /// <exception cref="ArgumentNullException"> <paramref name="randomizer" /> is null. </exception>
        public static TimeSpan NextTimeSpan (this Random randomizer)
        {
            return randomizer.NextTimeSpan(TimeSpan.Zero, TimeSpan.MaxValue);
        }

        /// <summary>
        ///     Gets a random time span value.
        /// </summary>
        /// <param name="randomizer"> The randomizer to use. </param>
        /// <param name="min"> The allowed minimum time span (inclusive). </param>
        /// <param name="max"> The allowed maximum time span (exclusive). </param>
        /// <returns>
        ///     A random time span value between <paramref name="min" /> (inclusive) and <paramref name="max" /> (exclusive).
        /// </returns>
        /// <exception cref="ArgumentNullException"> <paramref name="randomizer" /> is null. </exception>
        public static TimeSpan NextTimeSpan (this Random randomizer, TimeSpan min, TimeSpan max)
        {
            if (randomizer == null)
            {
                throw new ArgumentNullException(nameof(randomizer));
            }

            double ticks = randomizer.NextDouble(min.Ticks, max.Ticks);
            return new TimeSpan((long)ticks);
        }

        /// <summary>
        ///     Gets a triangular distributed random number.
        /// </summary>
        /// <param name="randomizer"> The randomizer to use. </param>
        /// <param name="min"> The allowed minimum value. </param>
        /// <param name="max"> The allowed maximum value. </param>
        /// <param name="mode"> The most frequent value. </param>
        /// <returns>
        ///     A triangular distributed random double precision floating point value.
        /// </returns>
        /// <remarks>
        ///     See <see href="https://en.wikipedia.org/wiki/Triangular_distribution"> https://en.wikipedia.org/wiki/Triangular_distribution </see> for details about the used algorithm.
        /// </remarks>
        /// <exception cref="ArgumentNullException"> <paramref name="randomizer" /> is null. </exception>
        /// <exception cref="NotFiniteNumberException"> <paramref name="min" />, <paramref name="max" />, or <paramref name="mode" /> is either "NaN"/"Not-a-Number" or infinity (positive or negative). </exception>
        public static double NextTriangular (this Random randomizer, double min, double max, double mode)
        {
            if (randomizer == null)
            {
                throw new ArgumentNullException(nameof(randomizer));
            }

            if (min.IsNanOrInfinity())
            {
                throw new NotFiniteNumberException(nameof(min));
            }

            if (max.IsNanOrInfinity())
            {
                throw new NotFiniteNumberException(nameof(max));
            }

            if (mode.IsNanOrInfinity())
            {
                throw new NotFiniteNumberException(nameof(mode));
            }

            double u = randomizer.NextDouble();

            if (u < ((mode - min) / (max - min)))
            {
                return min + Math.Sqrt(u * (max - min) * (mode - min));
            }

            return max - Math.Sqrt((1 - u) * (max - min) * (max - mode));
        }

        /// <summary>
        ///     Shuffles all items of a list randomly.
        /// </summary>
        /// <typeparam name="T"> The type of items in <paramref name="list" />. </typeparam>
        /// <param name="randomizer"> The randomizer to use. </param>
        /// <param name="list"> The list to be shuffled. </param>
        /// <returns>
        ///     The number of shuffled items.
        ///     Zero if the list contains no items.
        /// </returns>
        /// <remarks>
        ///     <para>
        ///         Shuffling of items is done in-place, so during shuffling some items might appear twice in the list.
        ///     </para>
        ///     <para>
        ///         This is a O(n) operation where n is the number of items in <paramref name="list" />.
        ///     </para>
        /// </remarks>
        /// <exception cref="ArgumentNullException"> <paramref name="randomizer" /> or <paramref name="list" /> is null. </exception>
        public static int Shuffle <T> (this Random randomizer, IList<T> list)
        {
            if (randomizer == null)
            {
                throw new ArgumentNullException(nameof(randomizer));
            }

            if (list == null)
            {
                throw new ArgumentNullException(nameof(list));
            }

            return randomizer.Shuffle(list, 0, list.Count);
        }

        /// <summary>
        ///     Shuffles all items of a list, starting at a specified index, randomly.
        /// </summary>
        /// <typeparam name="T"> The type of items in <paramref name="list" />. </typeparam>
        /// <param name="randomizer"> The randomizer to use. </param>
        /// <param name="list"> The list to be shuffled. </param>
        /// <param name="index"> The index at which the shuffling starts. </param>
        /// <returns>
        ///     The number of shuffled items.
        ///     Zero if <paramref name="index" /> points to the end of the list (or is the same as the number of items in the list respectively).
        /// </returns>
        /// <remarks>
        ///     <para>
        ///         Shuffling of items is done in-place, so during shuffling some items might appear twice in the list.
        ///     </para>
        ///     <para>
        ///         This is a O(n) operation where n is the number of items in <paramref name="list" /> minus <paramref name="index" />.
        ///     </para>
        /// </remarks>
        /// <exception cref="ArgumentNullException"> <paramref name="randomizer" /> or <paramref name="list" /> is null. </exception>
        /// <exception cref="ArgumentOutOfRangeException"> <paramref name="index" /> is less than zero or specifies an index outside the size of the list. </exception>
        public static int Shuffle <T> (this Random randomizer, IList<T> list, int index)
        {
            if (randomizer == null)
            {
                throw new ArgumentNullException(nameof(randomizer));
            }

            if (list == null)
            {
                throw new ArgumentNullException(nameof(list));
            }

            return randomizer.Shuffle(list, index, list.Count - index);
        }

        /// <summary>
        ///     Shuffles items of a list in a specified range randomly.
        /// </summary>
        /// <typeparam name="T"> The type of items in <paramref name="list" />. </typeparam>
        /// <param name="randomizer"> The randomizer to use. </param>
        /// <param name="list"> The list to be shuffled. </param>
        /// <param name="index"> The index at which the shuffling starts. </param>
        /// <param name="count"> The number of items to shuffle, starting at <paramref name="index" />. </param>
        /// <returns>
        ///     The number of shuffled items.
        ///     Zero if <paramref name="count" /> is zero.
        /// </returns>
        /// <remarks>
        ///     <para>
        ///         Shuffling of items is done in-place, so during shuffling some items might appear twice in the list.
        ///     </para>
        ///     <para>
        ///         This is a O(n) operation where n is <paramref name="count" />.
        ///     </para>
        /// </remarks>
        /// <exception cref="ArgumentNullException"> <paramref name="randomizer" /> or <paramref name="list" /> is null. </exception>
        /// <exception cref="ArgumentOutOfRangeException"> <paramref name="index" /> or <paramref name="count" /> is less than zero or specifies a range outside the size of the list. </exception>
        public static int Shuffle <T> (this Random randomizer, IList<T> list, int index, int count)
        {
            if (randomizer == null)
            {
                throw new ArgumentNullException(nameof(randomizer));
            }

            if (list == null)
            {
                throw new ArgumentNullException(nameof(list));
            }

            if (count == 0)
            {
                return 0;
            }

            if ((index < 0) || (index >= list.Count))
            {
                throw new ArgumentOutOfRangeException(nameof(index));
            }

            if ((count < 0) || ((index + count) > list.Count))
            {
                throw new ArgumentOutOfRangeException(nameof(count));
            }

            for (int i1 = index; i1 < (index + count); i1++)
            {
                int newIndex = randomizer.Next(index, i1 + 1);
                T temp = list[newIndex];
                list[newIndex] = list[i1];
                list[i1] = temp;
            }

            return count;
        }

        #endregion
    }
}
