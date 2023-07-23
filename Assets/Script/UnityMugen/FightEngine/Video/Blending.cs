using System;
using System.Diagnostics;

namespace UnityMugen.Video
{

    /// <summary>
    /// Alpha blending information that is sent to the graphics shader.
    /// </summary>
    [Serializable]
    public struct Blending : IEquatable<Blending>
    {
        public BlendType BlendType;
        public byte SourceFactor;
        public byte DestinationFactor;

        [DebuggerStepThrough]
        public Blending(BlendType type, float source, float destination)
        {
            BlendType = type;
            SourceFactor = type != BlendType.None ? (byte)Misc.Clamp((int)source, 0, 255) : (byte)0;
            DestinationFactor = type != BlendType.None ? (byte)Misc.Clamp((int)destination, 0, 255) : (byte)0;
        }

        /// <summary>
        /// Initializes a new instance of this class.
        /// </summary>
        /// <param name="type">The type of color blending to be used.</param>
        /// <param name="source">The weight of the source color used in color blending.</param>
        /// <param name="destination">The weight of the destination color used in color blending.</param>
        [DebuggerStepThrough]
        public Blending(BlendType _type, byte _source, byte _destination)
        {
            BlendType = _type;
            SourceFactor = _type != BlendType.None ? _source : (byte)0;
            DestinationFactor = _type != BlendType.None ? _source : (byte)0;
        }
        /// <summary>
        /// Initializes a new instance of this class.
        /// </summary>
        /// <param name="type">The type of color blending to be used.</param>
        /// <param name="source">The weight of the source color used in color blending.</param>
        /// <param name="destination">The weight of the destination color used in color blending.</param>
        [DebuggerStepThrough]
        public Blending(BlendType _type, int _source, int destination)
        {
            BlendType = _type;
            SourceFactor = _type != BlendType.None ? (byte)Misc.Clamp(_source, 0, 255) : (byte)0;
            DestinationFactor = _type != BlendType.None ? (byte)Misc.Clamp(destination, 0, 255) : (byte)0;
        }

        [DebuggerStepThrough]
        public bool Equals(Blending other)
        {
            return this == other;
        }

        [DebuggerStepThrough]
        public override bool Equals(object obj)
        {
            if (obj == null || obj.GetType() != GetType()) return false;

            return this == (Blending)obj;
        }

        public static bool operator ==(Blending lhs, Blending rhs)
        {
            return lhs.BlendType == rhs.BlendType && lhs.SourceFactor == rhs.SourceFactor && lhs.DestinationFactor == rhs.DestinationFactor;
        }

        public static bool operator !=(Blending lhs, Blending rhs)
        {
            return lhs.BlendType != rhs.BlendType || lhs.SourceFactor != rhs.SourceFactor || lhs.DestinationFactor != rhs.DestinationFactor;
        }

        /// <summary>
        /// Generates a hash code based off the value of this instance.
        /// </summary>
        /// <returns>The hash code of this instance.</returns>
        [DebuggerStepThrough]
        public override int GetHashCode()
        {
            if (BlendType == BlendType.None) return 0;

            return BlendType.GetHashCode() ^ SourceFactor ^ DestinationFactor;
        }
    }
}