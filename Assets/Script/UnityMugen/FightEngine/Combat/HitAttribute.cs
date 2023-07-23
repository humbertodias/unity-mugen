using System;
using System.Diagnostics;
using UnityMugen.Collections;

namespace UnityMugen.Combat
{

    public class HitAttribute : IEquatable<HitAttribute>
    {
        static HitAttribute()
        {
            s_default = new HitAttribute(AttackStateType.None, new ReadOnlyList<HitType>());
        }

        public HitAttribute(AttackStateType height, ReadOnlyList<HitType> attackdata)
        {
            if (attackdata == null) throw new ArgumentNullException(nameof(attackdata));

            m_attackheight = height;
            m_attackdata = attackdata;
        }

        public bool HasHeight(AttackStateType height)
        {
            if (height == AttackStateType.None) return false;

            return (AttackHeight & height) == height;
        }

        public bool HasData(HitType hittype)
        {
            if (hittype.Class == AttackClass.None || hittype.Power == AttackPower.None) return false;

            foreach (var type in AttackData)
            {
                if (HitType.Match(hittype, type)) return true;
            }

            return false;
        }



        public ReadOnlyList<HitType> AttackData => m_attackdata;

        public AttackStateType AttackHeight => m_attackheight;

        public static HitAttribute Default => s_default;

        //[DebuggerStepThrough]
        //public static Boolean operator ==(HitAttribute lhs, HitAttribute rhs)
        //{
        //    return !(lhs != rhs);
        //}

        //[DebuggerStepThrough]
        //public static Boolean operator !=(HitAttribute lhs, HitAttribute rhs)
        //{
        //    if(lhs.AttackHeight != rhs.AttackHeight) return true;
        //    if (lhs.AttackData.Count != rhs.AttackData.Count) return true;

        //    for(int i = 0; i < lhs.AttackData.Count; i++)
        //    {
        //        if (lhs.AttackData[i].Power != rhs.AttackData[i].Power) return true;
        //        if (lhs.AttackData[i].Class != rhs.AttackData[i].Class) return true;
        //    }

        //    return false;
        //}

        public bool Equals(HitAttribute other)
        {
            if (this.AttackHeight != other.AttackHeight) return false;
            if (this.AttackData.Count != other.AttackData.Count) return false;

            for (int i = 0; i < this.AttackData.Count; i++)
            {
                if (this.AttackData[i].Power != other.AttackData[i].Power) return false;
                if (this.AttackData[i].Class != other.AttackData[i].Class) return false;
            }

            return true;
        }

        #region Fields

        [DebuggerBrowsable(DebuggerBrowsableState.Never)]
        private static readonly HitAttribute s_default;

        [DebuggerBrowsable(DebuggerBrowsableState.Never)]
        private readonly AttackStateType m_attackheight;

        [DebuggerBrowsable(DebuggerBrowsableState.Never)]
        private readonly ReadOnlyList<HitType> m_attackdata;

        #endregion
    }
}