using System;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

namespace UnityMugen.Combat
{

    public class Collision
    {

        public static bool HasCollision(Entity lhs, ClsnType lhstype, Entity rhs, ClsnType rhstype)
        {
            if (lhs == null) throw new ArgumentNullException(nameof(lhs));
            if (rhs == null) throw new ArgumentNullException(nameof(rhs));

            if ((lhstype == ClsnType.Type1Attack && rhstype == ClsnType.Type2Normal) ||
                (lhstype == ClsnType.Type2Normal && rhstype == ClsnType.Type1Attack))
                return DetectAttackOponent(lhs, rhs);

            else if (lhstype == ClsnType.Type1Attack && rhstype == ClsnType.Type1Attack)
                return DetectAttack(lhs, rhs);

            else if (lhstype == ClsnType.Type2Normal && rhstype == ClsnType.Type2Normal)
                return DetectOponent(lhs, rhs);

            return false;
        }

        public static bool DetectOponent(Entity lhs, Entity rhs)
        {
            if (lhs == null) throw new ArgumentNullException(nameof(lhs));
            if (rhs == null) throw new ArgumentNullException(nameof(rhs));

            List<Rect> player = lhs.AnimationManager.CurrentElement.ClsnsTipe2Normal;
            List<Rect> playerMix = new List<Rect>();
            if (player != null)
                playerMix.AddRange(player.OfType<Rect>().ToList());

            foreach (var lhs_clsn in playerMix)
            {
                Bounds b1 = Make(lhs/*.CurrentLocation*//*transform.localPosition*/, lhs.CurrentFacing, lhs_clsn);

                List<Rect> oponentMix = new List<Rect>();
                if (rhs.AnimationManager.CurrentElement != null)
                {
                    List<Rect> oponentNormal = rhs.AnimationManager.CurrentElement.ClsnsTipe2Normal;
                    if (oponentNormal != null)
                        oponentMix.AddRange(oponentNormal.OfType<Rect>().ToList());
                }
                foreach (var rhs_clsn in oponentMix)
                {
                    Bounds b2 = Make(rhs/*.CurrentLocation*//*transform.localPosition*/, rhs.CurrentFacing, rhs_clsn);
                    if (b1.Intersects(b2))
                        return true;
                }
            }
            return false;
        }

        public static bool DetectAttack(Entity lhs, Entity rhs)
        {
            if (lhs == null) throw new ArgumentNullException(nameof(lhs));
            if (rhs == null) throw new ArgumentNullException(nameof(rhs));

            List<Rect> player = lhs.AnimationManager.CurrentElement.ClsnsTipe1Attack;
            List<Rect> playerMix = new List<Rect>();
            if (player != null)
                playerMix.AddRange(player.OfType<Rect>().ToList());

            foreach (var lhs_clsn in playerMix)
            {
                Bounds b1 = Make(lhs/*.CurrentLocation*//*.transform.localPosition*/, lhs.CurrentFacing, lhs_clsn);

                List<Rect> oponentMix = new List<Rect>();
                if (rhs.AnimationManager.CurrentElement != null)
                {
                    List<Rect> oponentNormal = rhs.AnimationManager.CurrentElement.ClsnsTipe1Attack;
                    if (oponentNormal != null)
                        oponentMix.AddRange(oponentNormal.OfType<Rect>().ToList());
                }
                foreach (var rhs_clsn in oponentMix)
                {
                    Bounds b2 = Make(rhs/*.CurrentLocation*//*.transform.localPosition*/, rhs.CurrentFacing, rhs_clsn);
                    if (b1.Intersects(b2))
                        return true;
                }
            }
            return false;
        }

        public static bool DetectAttackOponent(Entity lhs, Entity rhs)
        {
            if (lhs == null) throw new ArgumentNullException(nameof(lhs));
            if (rhs == null) throw new ArgumentNullException(nameof(rhs));

            List<Rect> playerOfensive = lhs.AnimationManager.CurrentElement.ClsnsTipe1Attack;
            List<Rect> playerMix = new List<Rect>();
            if (playerOfensive != null)
                playerMix.AddRange(playerOfensive.OfType<Rect>().ToList());

            foreach (var lhs_clsn in playerMix)
            {
                Bounds b1 = Make(lhs/*.CurrentLocation*//*.transform.localPosition*/, lhs.CurrentFacing, lhs_clsn);

                List<Rect> oponentMix = new List<Rect>();
                if (rhs.AnimationManager.CurrentElement != null)
                {
                    List<Rect> oponentAttack = rhs.AnimationManager.CurrentElement.ClsnsTipe1Attack;
                    List<Rect> oponentNormal = rhs.AnimationManager.CurrentElement.ClsnsTipe2Normal;
                    if (oponentAttack != null)
                        oponentMix.AddRange(oponentAttack.OfType<Rect>().ToList());
                    if (oponentNormal != null)
                        oponentMix.AddRange(oponentNormal.OfType<Rect>().ToList());
                }
                foreach (var rhs_clsn in oponentMix)
                {
                    Bounds b2 = Make(rhs/*.CurrentLocation*//*.transform.localPosition*/, rhs.CurrentFacing, rhs_clsn);
                    if (b1.Intersects(b2))
                        return true;
                }
            }
            return false;
        }

        public static Bounds Make(Entity entity/*Vector2 position*/, Facing facing, Rect boxCollider2D)
        {
            Bounds bounds = new Bounds();
            if (facing == Facing.Right)
            {
                float xMin = -((boxCollider2D.width / 2) - boxCollider2D.x);
                float xMax = (boxCollider2D.width / 2) + boxCollider2D.x;

                float yMin = (boxCollider2D.height / 2) - boxCollider2D.y;
                float yMax = -((boxCollider2D.height / 2) + boxCollider2D.y);

                var currentelement = entity.AnimationManager.CurrentElement;
                Vector2 vec = new Vector2(facing == Facing.Right ? -currentelement.Offset.x : currentelement.Offset.x, currentelement.Offset.y);

                bounds.SetMinMax(new Vector3(xMin + entity.CurrentLocation.x + vec.x, -yMin + entity.CurrentLocation.y + vec.y), new Vector3(xMax + entity.CurrentLocation.x + vec.x, -yMax + entity.CurrentLocation.y + vec.y));
            }
            else
            {
                float xMin = ((boxCollider2D.width / 2) - boxCollider2D.x);
                float xMax = (boxCollider2D.width / 2) + boxCollider2D.x;

                float yMin = (boxCollider2D.height / 2) - boxCollider2D.y;
                float yMax = ((boxCollider2D.height / 2) + boxCollider2D.y);

                var currentelement = entity.AnimationManager.CurrentElement;
                Vector2 vec = new Vector2(facing == Facing.Right ? -currentelement.Offset.x : currentelement.Offset.x, currentelement.Offset.y);

                bounds.SetMinMax(new Vector3(-xMax + entity.CurrentLocation.x + vec.x, -yMin + entity.CurrentLocation.y + vec.y), new Vector3(xMin + entity.CurrentLocation.x + vec.x, yMax + entity.CurrentLocation.y + vec.y));
            }

            return bounds;
        }

    }
}