using System;
using System.Diagnostics;

namespace UnityMugen
{
    /// <summary>
    /// A random number generator.
    /// </summary>
    public class Random
    {
        public int CurrentSeed => m_seed;

        private System.Random m_random;
        private int m_seed;

        /// <summary>
        /// Initializes a new instance of this class with a time dependant seed.
        /// </summary>
        public Random()
        {
            Seed(Environment.TickCount);
        }

        /// <summary>
        /// Re-initializes random number generation with a specified seed.
        /// </summary>
        /// <param name="seed">Number used to start generation of random numbers.</param>
        public void Seed(int seed)
        {
            m_seed = seed;
            m_random = new System.Random(seed);
        }

        /// <summary>
        /// Returns a randomly generated number within a specified range.
        /// </summary>
        /// <param name="min">The lowest number that can be generated, inclusive.</param>
        /// <param name="max">The highest number that can be generated, exclusive.</param>
        /// <returns>An Int32 that is greater than or equal to min but less than max.</returns>
        /// <exception cref="System.ArgumentOutOfRangeException">min is greater than max.</exception> 
        public int NewInt(int min, int max)
        {
            return m_random.Next(min, max);
        }

        public float NewFloat(float min, float max)
        {
            return m_random.Next((int)(min *Constant.Scale2), (int)(max * Constant.Scale2)) * 0.01f;
        }

        /// <summary>
        /// Return a randomly generated number.
        /// </summary>
        /// <returns>A Single that is greater than or equal to 0.0f and less than 1.0f.</returns>
        public float NewSingle()
        {
            return (float)m_random.NextDouble();
        }

    }
}