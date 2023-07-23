using System;
using System.Diagnostics;

namespace UnityMugen.Drawing
{

    /// <summary>
    /// An immutable identifier of a fightEngine.Drawing.Palette.
    /// </summary>
    [Serializable]
    public struct PaletteId : IEquatable<PaletteId>
    {
        /// <summary>
        /// Initializes an instance of this class.
        /// </summary>
        /// <param name="group">The group of the represented Palette.</param>
        /// <param name="number">The number of the represented Palette.</param>
        [DebuggerStepThrough]
        public PaletteId(int group, int number)
        {
            Group = group;
            Number = number;
        }

        /// <summary>
        /// Generates a hash code based off the value of this instance.
        /// </summary>
        /// <returns>The hash code of this instance.</returns>
        [DebuggerStepThrough]
        public override int GetHashCode()
        {
            return Group ^ Number;
        }

        /// <summary>
        /// Determines whether the supplied PaletteId identifies the same fightEngine.Drawing.Palette as this instance.
        /// </summary>
        /// <param name="obj">The Palette to be compared to the current instance.</param>
        /// <returns>true if the supplied PaletteId is equal to this instance; false otherwise.</returns>
        [DebuggerStepThrough]
        public bool Equals(PaletteId other)
        {
            return this == other;
        }

        /// <summary>
        /// Determines whether the supplied object is an instance of this class identifing the same fightEngine.Drawing.Palette.
        /// </summary>
        /// <param name="obj">The object to be compared.</param>
        /// <returns>true if the supplied object is equal to this instance; false otherwise.</returns>
        [DebuggerStepThrough]
        public override bool Equals(object obj)
        {
            if (obj == null || obj.GetType() != GetType()) return false;

            return this == (PaletteId)obj;
        }

        /// <summary>
        /// Determines whether two PaletteIds identify the same fightEngine.Drawing.Palette.
        /// </summary>
        /// <param name="lhs">The first PaletteId to be compared.</param>
        /// <param name="rhs">The second PaletteId to be compared.</param>
        /// <returns>true if the two Points identify the same Palette; false otherwise.</returns>
        [DebuggerStepThrough]
        public static bool operator ==(PaletteId lhs, PaletteId rhs)
        {
            return lhs.Group == rhs.Group && lhs.Number == rhs.Number;
        }

        /// <summary>
        /// Determines whether the two PaletteIds do not identify the same fightEngine.Drawing.Palette.
        /// </summary>
        /// <param name="lhs">The first PaletteId to be compared.</param>
        /// <param name="rhs">The second PaletteId to be compared.</param>
        /// <returns>true if the two Points do not identify the same Palette; false otherwise.</returns>
        [DebuggerStepThrough]
        public static bool operator !=(PaletteId lhs, PaletteId rhs)
        {
            return lhs.Group != rhs.Group || lhs.Number != rhs.Number;
        }

        /// <summary>
        /// Generates a System.String whose value is an representation of this instance.
        /// </summary>
        /// <returns>A System.String representation of this instance.</returns>
        public override string ToString()
        {
            return this != Invalid ? Group + ", " + Number : "Invalid";
        }

        /// <summary>
        /// A PaletteId that does not identify a fightEngine.Drawing.Palette.
        /// </summary>
        /// <returns>A SpriteId that does not identify a Sprite.</returns>
        public static PaletteId Invalid => new PaletteId(int.MinValue, int.MinValue);

        /// <summary>
        /// The Group of this instance.
        /// </summary>
        /// <returns>The Group number.</returns>
        public int Group;

        /// <summary>
        /// The Number of this instance.
        /// </summary>
        /// <returns>The Image number.</returns>
        public int Number;
    }
}
