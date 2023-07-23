using System;
using System.Diagnostics;

namespace UnityMugen.Audio
{
    /// <summary>
    /// An immutable identifier of a sound in a fightEngine.Audio.SoundManager.
    /// </summary>
    public struct SoundId : IEquatable<SoundId>
    {
        [DebuggerStepThrough]
        public SoundId(Int32 group)
        {
            Group = group;
            Sample = 0;
        }

        /// <summary>
        /// Initializes a new instance of this class.
        /// </summary>
        /// <param name="group">The group number of the identified sound.</param>
        /// <param name="sample">The sample number of the identified sound.</param>
        [DebuggerStepThrough]
        public SoundId(Int32 group, Int32 sample)
        {
            Group = group;
            Sample = sample;
        }

        /// <summary>
        /// Determines whether the supplied SoundId identifies the same sound as this instance.
        /// </summary>
        /// <param name="obj">The SoundId to be compared to the current instance.</param>
        /// <returns>true if the supplied SoundId is equal to this instance; false otherwise.</returns>
        [DebuggerStepThrough]
        public Boolean Equals(SoundId other)
        {
            return this == other;
        }

        /// <summary>
        /// Determines whether the supplied object is a SoundId that identifies the same sound as this instance.
        /// </summary>
        /// <param name="obj">The object to be compared to the current instance.</param>
        /// <returns>true if the supplied object is equal to this instance; false otherwise.</returns>
        [DebuggerStepThrough]
        public override Boolean Equals(Object obj)
        {
            if (obj == null || obj.GetType() != this.GetType()) return false;

            return this == (SoundId)obj;
        }

        /// <summary>
        /// Determines whether two SoundIds identify the same sound.
        /// </summary>
        /// <param name="lhs">The first SoundId to be compared.</param>
        /// <param name="rhs">The second SoundId to be compared.</param>
        /// <returns>true if the two SoundIds identify the same sound; false otherwise.</returns>
        [DebuggerStepThrough]
        public static Boolean operator ==(SoundId lhs, SoundId rhs)
        {
            return lhs.Group == rhs.Group && lhs.Sample == rhs.Sample;
        }

        /// <summary>
        /// Determines whether two SoundIds do not identify the same sound.
        /// </summary>
        /// <param name="lhs">The first SoundId to be compared.</param>
        /// <param name="rhs">The second SoundId to be compared.</param>
        /// <returns>true if the two SoundIds do not identify the same sound; false otherwise.</returns>
        [DebuggerStepThrough]
        public static Boolean operator !=(SoundId lhs, SoundId rhs)
        {
            return lhs.Group != rhs.Group || lhs.Sample != rhs.Sample;
        }

        /// <summary>
        /// Generates a hash code based off the value of this instance.
        /// </summary>
        /// <returns>The hash code of this instance.</returns>
        [DebuggerStepThrough]
        public override Int32 GetHashCode()
        {
            return Group ^ Sample;
        }

        /// <summary>
        /// Generates a System.String whose value is an representation of this instance.
        /// </summary>
        /// <returns>A System.String representation of this instance.</returns>
        [DebuggerStepThrough]
        public override String ToString()
        {
            return (this != Invalid) ? Group + ", " + Sample : "Invalid";
        }

        /// <summary>
        /// The Group number of this instance.
        /// </summary>
        /// <returns>The Group number.</returns>
        public Int32 Group;

        /// <summary>
        /// The Group Sample of this instance.
        /// </summary>
        /// <returns>The Sample number.</returns>
        public Int32 Sample;

        /// <summary>
        /// A SoundId that does not identify a sound.
        /// </summary>
        /// <returns>A SoundId that does not identify a sound.</returns>
        public static SoundId Invalid
        {
            get { return new SoundId(Int32.MinValue, Int32.MinValue); }
        }

    }
}
