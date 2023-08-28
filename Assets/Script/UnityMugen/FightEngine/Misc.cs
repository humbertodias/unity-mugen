using System;
using System.ComponentModel;
using Unity.Collections;
using UnityEngine;
using UnityMugen.Combat;
using UnityMugen.Video;

namespace UnityMugen
{
    public static class Misc
    {
        static LauncherEngine Launcher => LauncherEngine.Inst;
        static FightEngine Engine => Launcher.mugen.Engine;

        public static void Swap<T>(ref T lhs, ref T rhs)
        {
            var temp = lhs;
            lhs = rhs;
            rhs = temp;
        }

        public static Facing FlipFacing(Facing input)
        {
            if (input == Facing.Left) return Facing.Right;

            if (input == Facing.Right) return Facing.Left;

            throw new ArgumentException("Not valid Facing", nameof(input));
        }

        public static int NextPowerOfTwo(int input)
        {
            if (input < 0) throw new ArgumentOutOfRangeException(nameof(input));

            var output = 1;
            while (output < input) output *= 2;

            return output;
        }

        public static float Center(float min, float max)
        {
            float width = max - min;
            return min + width / 2;
        }
        public static int Clamp(int value, int min, int max)
        {
            value = value > max ? max : value;
            value = value < min ? min : value;
            return value;
        }

        public static float Clamp(float value, float min, float max)
        {
            value = value > max ? max : value;
            value = value < min ? min : value;
            return value;
        }

        public static Vector2 GetOffset(Vector2 location, Facing facing, Vector2 offset)
        {
            var output = location + new Vector2(0, offset.y);

            if (facing == Facing.Right)
            {
                output.x += offset.x;
                return output;
            }

            if (facing == Facing.Left)
            {
                output.x -= offset.x;
                return output;
            }

            throw new ArgumentOutOfRangeException(nameof(facing));
        }

        public static float GetOffset(float location, Facing facing, float offset)
        {
            if (facing == Facing.Right)
            {
                location += offset;
                return location;
            }

            if (facing == Facing.Left)
            {
                location -= offset;
                return location;
            }

            throw new ArgumentOutOfRangeException(nameof(facing));
        }

        public static int FaceScalar(Facing facing, int value)
        {
            switch (facing)
            {
                case Facing.Left:
                    return -value;

                case Facing.Right:
                    return value;

                default:
                    throw new ArgumentOutOfRangeException(nameof(facing));
            }
        }

        public static float FaceScalar(Facing facing, float value)
        {
            switch (facing)
            {
                case Facing.Left:
                    return -value;

                case Facing.Right:
                    return value;

                default:
                    throw new ArgumentOutOfRangeException(nameof(facing));
            }
        }

        public static string GetPrefix(TeamSide side)
        {
            switch (side)
            {
                case TeamSide.Left:
                    return "p1";

                case TeamSide.Right:
                    return "p2";

                default:
                    throw new ArgumentOutOfRangeException(nameof(side));
            }
        }

        public static string GetMatePrefix(TeamMode mode, TeamSide side)
        {
            switch (side)
            {
                case TeamSide.Left:
                    return mode == TeamMode.Turns ? "p1.teammate" : "p3";

                case TeamSide.Right:
                    return mode == TeamMode.Turns ? "p2.teammate" : "p4";

                default:
                    throw new ArgumentOutOfRangeException(nameof(side));
            }
        }

        public static string ColorConstom(string value)
        {
            int count = value.Split(',').Length - 1;
            if (count == 0)
                value += ", 0 , 0";
            else if (count == 1)
                value += ", 0";

            return "new Color(" + value + ")";
        }
        public static string Vector2Constom(string value, bool noScale = true)
        {
            if (noScale)
                return "new Vector2(" + (value.Contains(",") ? value : (value + ", 0")) + ")";
            else
                return "new Vector2(" + (value.Contains(",") ? value : (value + ", 1")) + ")";
        }
        public static string Vector3Constom(string value)
        {
            int count = value.Split(',').Length - 1;
            if (count == 0)
                value += ", 0 , 0";
            else if (count == 1)
                value += ", 0";

            return "new Vector3(" + value + ")";
        }
        public static string Vector4Constom(string value)
        {
            int count = value.Split(',').Length - 1;
            if (count == 0)
                value += ", 0 , 0, 0";
            else if (count == 1)
                value += ", 0, 0";
            else if (count == 2)
                value += ", 0";

            return "new Vector4(" + value + ")";
        }

        public static bool IsCorner(Character target)
        {
            var screenrect = Engine.CameraFE.ScreenBounds();
            return (target.GetLeftEdgePosition(true) - (1 * Constant.Scale) < screenrect.xMin && target.GetLeftEdgePosition(true) + (1 * Constant.Scale) > screenrect.xMin) ||
                    (target.GetRightEdgePosition(true) - (1 * Constant.Scale) < screenrect.xMax && target.GetRightEdgePosition(true) + (1 * Constant.Scale) > screenrect.xMax);
        }

        public static Vector3 ClampVector3(Vector3 value, Vector3 min, Vector3 max)
        {
            float x = Mathf.Clamp(value.x, min.x, max.x);
            float y = Mathf.Clamp(value.y, min.y, max.y);
            float z = Mathf.Clamp(value.z, min.z, max.z);
            return new Vector3(x, y, z);
        }


        public static int ERROR_CODE(Exception e)
        {
            var w32ex = e as Win32Exception;
            if (w32ex == null)
            {
                w32ex = e.InnerException as Win32Exception;
            }
            if (w32ex != null)
            {
                return w32ex.ErrorCode;
            }
            return 0;
        }

        public static void FreeBytes(NativeArray<byte> data)
        {
            if (data.IsCreated)
            {
                data.Dispose();
            }
        }

        public static bool HelperCheck(Character character)
        {
            if (character == null) throw new ArgumentNullException(nameof(character));

            var helper = character as Helper;
            if (helper == null) return false;

            return helper.Data.Type == HelperType.Normal;
        }

        public static Blending ToBlending(BlendType blendType)
        {
            if (blendType == BlendType.AddAlpha)
            {
                return new Blending(BlendType.Add, 0, 0);
            }
            else if (blendType == BlendType.Add)
            {
                return new Blending(BlendType.Add, 255, 255);
            }
            else if (blendType == BlendType.Add1)
            {
                return new Blending(BlendType.Add, 255, 127);
            }
            else if (blendType == BlendType.Subtract)
            {
                return new Blending(BlendType.Subtract, 255, 255);
            }

            return new Blending();
        }

        public static Vector3 Vector3(float value)
        {
            return new Vector3(value, value, value);
        }

        public static float ToRadians(float degrees)
        {
            return degrees * Mathf.Deg2Rad;
        }

        public static T[] Add<T>(this T[] target, T item)
        {
            if (target == null)
            {
                throw new ArgumentNullException(nameof(target));
            }
            T[] result = new T[target.Length + 1];
            target.CopyTo(result, 0);
            result[target.Length] = item;
            return result;
        }

        public static SpriteRenderer Shadow(string nameEntity, Color shadowColor)
        {
            GameObject shadow = new GameObject("Shadown_" + nameEntity);
            shadow.transform.SetParent(Engine.stageScreen.Entities);
            shadow.hideFlags = HideFlags.HideAndDontSave;// InInspector; 
            SpriteRenderer sprite = shadow.AddComponent<SpriteRenderer>();
            sprite.material = new Material(Shader.Find("UnityMugen/Sprites/Shadow"));
            sprite.material.SetColor("_Color", shadowColor);
            sprite.sortingLayerName = "Entity";
            sprite.sortingOrder = -1;
            return sprite;
        }

        public static SpriteRenderer Reflection(string nameEntity)
        {
            GameObject reflection = new GameObject("Reflection_" + nameEntity);
            reflection.transform.SetParent(Engine.stageScreen.Reflections);
            reflection.hideFlags = HideFlags.HideAndDontSave;// InInspector;
            SpriteRenderer sprite = reflection.AddComponent<SpriteRenderer>();
            sprite.material = new Material(Shader.Find("UnityMugen/Sprites/ColorSwap")); //  | UnityMugen/Sprites/Shadow
            sprite.material.SetColor("_Color", new Color32(0, 0, 0, 145));
            sprite.sortingLayerName = "Entity";
            sprite.sortingOrder = -5;
            return sprite;
        }

        public static Transform PointEntity(GameObject gameObject)
        {
            GameObject pointEntity = new GameObject();
            pointEntity.name = "PointEntity";
            pointEntity.hideFlags = HideFlags.HideAndDontSave;
            pointEntity.transform.localScale = new Vector3(0.4106072f, 0.4106072f, 0.4106072f);
            SpriteRenderer sr = pointEntity.AddComponent<SpriteRenderer>();
            sr.sprite = Launcher.trainnerSettings.pointEntity;
            sr.color = Launcher.trainnerSettings.pointEntityColor;
            sr.sortingLayerName = "Entity";
            sr.sortingOrder = 100;
            pointEntity.transform.SetParent(gameObject.transform);
            return pointEntity.transform;
        }


        public static int AddScore(HitDefinition hitdef)
        {
            if (hitdef.Score != 0) return hitdef.Score;

            int dmgMul = 0;

            // Normal Attacks
            if (hitdef.HitAttribute.Equals(Launcher.stringConverter.MiscToHitAttribute("S,HA")) ||
                hitdef.HitAttribute.Equals(Launcher.stringConverter.MiscToHitAttribute("A,HA")) ||
                hitdef.HitAttribute.Equals(Launcher.stringConverter.MiscToHitAttribute("C,HA")))
                dmgMul = 10;
            else if (hitdef.HitAttribute.Equals(Launcher.stringConverter.MiscToHitAttribute("S,SA")) ||
                hitdef.HitAttribute.Equals(Launcher.stringConverter.MiscToHitAttribute("A,SA")) ||
                hitdef.HitAttribute.Equals(Launcher.stringConverter.MiscToHitAttribute("C,SA")))
                dmgMul = 9;
            else if (hitdef.HitAttribute.Equals(Launcher.stringConverter.MiscToHitAttribute("S,NA")) ||
                hitdef.HitAttribute.Equals(Launcher.stringConverter.MiscToHitAttribute("A,NA")) ||
                hitdef.HitAttribute.Equals(Launcher.stringConverter.MiscToHitAttribute("C,NA")))
                dmgMul = 8;

            // Throws
            else if (hitdef.HitAttribute.Equals(Launcher.stringConverter.MiscToHitAttribute("S,HT")) ||
                hitdef.HitAttribute.Equals(Launcher.stringConverter.MiscToHitAttribute("A,HT")) ||
                hitdef.HitAttribute.Equals(Launcher.stringConverter.MiscToHitAttribute("C,HT")))
                dmgMul = 10;
            else if (hitdef.HitAttribute.Equals(Launcher.stringConverter.MiscToHitAttribute("S,ST")) ||
                hitdef.HitAttribute.Equals(Launcher.stringConverter.MiscToHitAttribute("A,ST")) ||
                hitdef.HitAttribute.Equals(Launcher.stringConverter.MiscToHitAttribute("C,ST")))
                dmgMul = 9;
            else if (hitdef.HitAttribute.Equals(Launcher.stringConverter.MiscToHitAttribute("S,NT")) ||
                hitdef.HitAttribute.Equals(Launcher.stringConverter.MiscToHitAttribute("A,NT")) ||
                hitdef.HitAttribute.Equals(Launcher.stringConverter.MiscToHitAttribute("C,NT")))
                dmgMul = 8;

            // Projectiles
            else if (hitdef.HitAttribute.Equals(Launcher.stringConverter.MiscToHitAttribute("S,HP")) ||
                hitdef.HitAttribute.Equals(Launcher.stringConverter.MiscToHitAttribute("A,HP")) ||
                hitdef.HitAttribute.Equals(Launcher.stringConverter.MiscToHitAttribute("C,HP")))
                dmgMul = 10;
            else if (hitdef.HitAttribute.Equals(Launcher.stringConverter.MiscToHitAttribute("S,SP")) ||
                hitdef.HitAttribute.Equals(Launcher.stringConverter.MiscToHitAttribute("A,SP")) ||
                hitdef.HitAttribute.Equals(Launcher.stringConverter.MiscToHitAttribute("C,SP")))
                dmgMul = 9;
            else if (hitdef.HitAttribute.Equals(Launcher.stringConverter.MiscToHitAttribute("S,NP")) ||
                hitdef.HitAttribute.Equals(Launcher.stringConverter.MiscToHitAttribute("A,NP")) ||
                hitdef.HitAttribute.Equals(Launcher.stringConverter.MiscToHitAttribute("C,NP")))
                dmgMul = 8;

            if (dmgMul > 0)
                return (int)(hitdef.HitDamage * dmgMul);
            else
                return 0;
        }

        public static int AddScoreComboCount(int count)
        {
            if (count == 2) return 300;
            if (count == 3) return 500;
            if (count == 4) return 1000;
            if (count == 5) return 1200;
            if (count == 6) return 1500;
            if (count == 7) return 2000;
            if (count == 8) return 2300;
            if (count == 9) return 2600;
            if (count == 10) return 3000;
            if (count == 11) return 3300;
            if (count == 12) return 3600;
            if (count == 13) return 4000;
            if (count == 14) return 4500;
            else
                return Misc.Clamp(0, 10000, 5000 + (count - 15) * 1000);
        }

        public static Vector2 P2Dist(Character character)
        {
            var opponent = character.GetOpponent();
            if (opponent == null)
            {
                return Vector2.zero;
            }
            Vector2 vector2;
            var distance = Math.Abs(character.CurrentLocation.x - opponent.CurrentLocation.x);
            if (character.CurrentFacing == UnityMugen.Facing.Right)
            {
                vector2.x = (opponent.CurrentLocation.x >= character.CurrentLocation.x ? distance : -distance) * Constant.Scale2;
            }
            else
            {
                vector2.x = (opponent.CurrentLocation.x >= character.CurrentLocation.x ? -distance : distance) * Constant.Scale2;
            }

            vector2.y = (opponent.CurrentLocation.y - character.CurrentLocation.y) * Constant.Scale2;

            return vector2;
        }

        //Triggers//
        public static Vector2 ScreenPos(Entity entity)
        {
            var drawlocation = Camera.main.WorldToScreenPoint(entity.CurrentLocationYTransform());
            float x = drawlocation.x / (Screen.width / Constant.LocalCoord.x);
            float y = (Screen.height - drawlocation.y) / (Screen.height / Constant.LocalCoord.y);
            return new Vector2(x, y);
        }

        public static Vector2 Pos(Entity entity)
        {
            var drawlocationCenterCam = Camera.main.WorldToScreenPoint(entity.Engine.CameraFE.transCenter.position);
            float xCam = drawlocationCenterCam.x / (Screen.width / Constant.LocalCoord.x);

            var drawlocationEntity = Camera.main.WorldToScreenPoint(entity.CurrentLocationYTransform());
            float xEntity = drawlocationEntity.x / (Screen.width / Constant.LocalCoord.x);

            return new Vector2(xEntity - xCam, entity.CurrentLocation.y * Constant.Scale2);
        }
        ////////////
    }

}