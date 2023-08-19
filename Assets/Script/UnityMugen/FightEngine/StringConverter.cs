using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Globalization;
using System.Reflection;
using System.Text.RegularExpressions;
using UnityEngine;
using UnityMugen.Audio;
using UnityMugen.Collections;
using UnityMugen.Combat;
using UnityMugen.Drawing;
using UnityMugen.Evaluation;
using UnityMugen.Video;

namespace UnityMugen
{
    [AttributeUsage(AttributeTargets.Method)]
    public class StringConversionAttribute : Attribute
    {
        public StringConversionAttribute(Type type)
        {
            if (type == null) throw new ArgumentNullException(nameof(type));

            m_type = type;
        }

        public Type Type => m_type;

        #region Fields

        [DebuggerBrowsable(DebuggerBrowsableState.Never)]
        private readonly Type m_type;

        #endregion
    }

    public class StringConverter
    {

        public StringConverter()
        {
            m_blendingregex = new Regex(@"^AS(\d+)d(\d+)$", RegexOptions.IgnoreCase);
            m_printdataregex = new Regex(@"^(\d+)\s*,\s*(\d+)\s*,?\s*?(-?\d+)?$", RegexOptions.IgnoreCase);
            m_hitpriorityregex = new Regex(@"^(\d+),\s*(\w+)$", RegexOptions.IgnoreCase);
            m_failure = new object();
            m_conversionmap = BuildConversionMap();
            m_sc = StringComparer.OrdinalIgnoreCase;
        }

        private ReadOnlyDictionary<Type, Converter<string, object>> BuildConversionMap()
        {
            var conversionmap = new Dictionary<Type, Converter<string, object>>();

            foreach (var mi in GetType().GetMethods(BindingFlags.Instance | BindingFlags.NonPublic))
            {
                var attrib = (StringConversionAttribute)Attribute.GetCustomAttribute(mi, typeof(StringConversionAttribute));
                if (attrib == null) continue;

                var d = (Converter<string, object>)Delegate.CreateDelegate(typeof(Converter<string, object>), this, mi);
                conversionmap.Add(attrib.Type, d);

                if (attrib.Type.IsValueType)
                {
                    var nullableType = typeof(Nullable<>).MakeGenericType(attrib.Type);
                    conversionmap.Add(nullableType, d);
                }
            }

            return new ReadOnlyDictionary<Type, Converter<string, object>>(conversionmap);
        }

        public T Convert<T>(string str)
        {
            if (str == null) throw new ArgumentNullException(nameof(str));

            T output;
            if (TryConvert(str, out output)) return output;

            UnityEngine.Debug.LogWarning("Cannot convert " + str + " to type: " + typeof(T).Name);
            return default(T);
        }

        public bool TryConvert<T>(string str, out T output)
        {
            Converter<String, System.Object> converter = null;
            if (m_conversionmap.TryGetValue(typeof(T), out converter) == false)
            {
                output = default(T);
                return false;
            }

            var obj = converter(str);
            if (obj == Failure)
            {
                output = default(T);
                return false;
            }

            output = (T)obj;
            return true;
        }

        [StringConversion(typeof(KeyCode))]
        private object ToKeys(string s)
        {
            KeyCode key;
            if (!Enum.TryParse(s, true, out key)) return Failure;
            return key;
        }

        [StringConversion(typeof(string))]
        private object ToString(string s)
        {
            return s;
        }

        [StringConversion(typeof(CombatMode))]
        private object ToCombatMode(string s)
        {
            if (s.Length == 4 && m_sc.Equals(s, "None")) return CombatMode.None;
            if (s.Length == 6 && m_sc.Equals(s, "Versus")) return CombatMode.Versus;
            if (s.Length == 8)
            {
                if (m_sc.Equals(s, "TeamCoop")) return CombatMode.TeamCoop;
                if (m_sc.Equals(s, "Survival")) return CombatMode.Survival;
                if (m_sc.Equals(s, "Training")) return CombatMode.Training;
            }
            if (s.Length == 10)
            {
                if (m_sc.Equals(s, "TeamArcade")) return CombatMode.TeamArcade;
                if (m_sc.Equals(s, "TeamVersus")) return CombatMode.TeamVersus;
            }

            if (s.Length == 12 && m_sc.Equals(s, "SurvivalCoop")) return CombatMode.SurvivalCoop;

            return Failure;
        }

        [StringConversion(typeof(Rect))]
        private object ToRectangle(string s)
        {
            var expression = (Expression)ToExpression(s);

            var rectangle = EvaluationHelper.AsRectangle(null, expression, null);
            if (rectangle == null) return Failure;

            return rectangle.Value;
        }

        [StringConversion(typeof(Layer))]
        private object ToBackgroundLayer(string s)
        {
            if (TryConvert(s, out Int32 layernumber) == false) return Failure;

            if (layernumber == 0) return Layer.Back;
            if (layernumber == 1) return Layer.Front;
            if (layernumber == 2) return Layer.Full;

            return Failure;
        }

        [StringConversion(typeof(int))]
        private object ToInt32(string s)
        {
            s = RemoveTrailingGarbage(s);
            if (int.TryParse(s, NumberStyles.Integer, NumberFormatInfo.InvariantInfo, out Int32 val))
                return val;

            return Failure;
        }

        [StringConversion(typeof(float))]
        private object ToSingle(string s)
        {
            s = RemoveTrailingGarbage(s);
            if (float.TryParse(s, NumberStyles.Float, NumberFormatInfo.InvariantInfo, out Single val))
                return val;

            return Failure;
        }

        private static string RemoveTrailingGarbage(string s)
        {
            if (string.IsNullOrEmpty(s)) return s;
            for (var i = 0; i < s.Length; i++)
            {
                if (!char.IsDigit(s[i]) && s[i] != '.' && s[i] != '+' && s[i] != '-' && s[i] != 'e' && s[i] != 'E')
                {
                    return s.Substring(0, i);
                }
            }
            return s;
        }

        [StringConversion(typeof(bool))]
        private object ToBoolean(string s)
        {
            if (s.Length == 4 && m_sc.Equals("true", s)) return true;
            if (s.Length == 5 && m_sc.Equals("false", s)) return false;

            if (TryConvert(s, out Int32 intvalue))
            {
                return intvalue > 0;
            }

            return Failure;
        }

        [StringConversion(typeof(Expression))]
        private object ToExpression(string s)
        {
            if (LauncherEngine.Inst != null)
                return LauncherEngine.Inst.evaluationSystem.CreateExpression(s);
            else
                return new EvaluationSystem().CreateExpression(s);
        }

        [StringConversion(typeof(PrefixedExpression))]
        private object ToPrefixedExpression(string s)
        {
            if (LauncherEngine.Inst != null)
                return LauncherEngine.Inst.evaluationSystem.CreatePrefixedExpression(s);
            else
                return new EvaluationSystem().CreatePrefixedExpression(s);
        }

        [StringConversion(typeof(Vector2))]
        private object ToVector2(string s)
        {
            Expression expression;
            if (TryConvert(s, out expression) == false) return Failure;

            var vector = EvaluationHelper.AsVector2(null, expression, null);
            if (vector == null) return Failure;

            return vector.Value;
        }

        [StringConversion(typeof(Vector2Int))]
        private object ToPoint(string s)
        {
            if (TryConvert(s, out Expression expression) == false) return Failure;

            var point = EvaluationHelper.AsVector2(null, expression, null);
            if (point == null) return Failure;

            return point.Value;
        }

        [StringConversion(typeof(Vector3))]
        private object ToVector3(string s)
        {
            if (TryConvert(s, out Expression expression) == false)
                return Failure;

            var vector = EvaluationHelper.AsVector3(null, expression, null);
            if (vector == null) return Failure;

            return vector.Value;
        }

        [StringConversion(typeof(SoundId))]
        private object ToSoundId(string s)
        {
            if (TryConvert(s, out Vector2 p) == false)
                return Failure;

            return new SoundId((int)p.x, (int)p.y);
        }

        [StringConversion(typeof(Facing))]
        private object ToFacing(string s)
        {
            if (TryConvert(s, out Int32 intvalue) == false)
                return Failure;

            return intvalue >= 0 ? Facing.Right : Facing.Left;
        }

        [StringConversion(typeof(AttackStateType))]
        private object ToAttackStateType(string s)
        {
            var ast = AttackStateType.None;

            if (s.IndexOf("s", StringComparison.OrdinalIgnoreCase) != -1) ast |= AttackStateType.Standing;
            if (s.IndexOf("c", StringComparison.OrdinalIgnoreCase) != -1) ast |= AttackStateType.Crouching;
            if (s.IndexOf("a", StringComparison.OrdinalIgnoreCase) != -1) ast |= AttackStateType.Air;

            if (ast == AttackStateType.None && s != string.Empty) return Failure;

            return ast;
        }

        [StringConversion(typeof(PositionType))]
        private object ToPositionType(string s)
        {
            if (s.Length == 2)
            {
                if (m_sc.Equals(s, "p1")) return PositionType.P1;
                if (m_sc.Equals(s, "p2")) return PositionType.P2;
            }
            else if (s.Length == 5)
            {
                if (m_sc.Equals(s, "front")) return PositionType.Front;
                if (m_sc.Equals(s, "right")) return PositionType.Right;
            }
            else if (s.Length == 4)
            {
                if (m_sc.Equals(s, "back")) return PositionType.Back;
                if (m_sc.Equals(s, "left")) return PositionType.Left;
            }
            else if (s.Length == 6 && m_sc.Equals(s, "center")) return PositionType.Center;

            return Failure;
        }


        [StringConversion(typeof(PositionTypeUI))]
        private object ToPositionTypeUI(string s)
        {
            if (m_sc.Equals(s, "center")) return PositionTypeUI.Center;
            if (m_sc.Equals(s, "left")) return PositionTypeUI.Left;
            if (m_sc.Equals(s, "right")) return PositionTypeUI.Right;
            if (m_sc.Equals(s, "top")) return PositionTypeUI.Top;
            if (m_sc.Equals(s, "botton")) return PositionTypeUI.Botton;
            if (m_sc.Equals(s, "lefttop")) return PositionTypeUI.LeftTop;
            if (m_sc.Equals(s, "leftBotton")) return PositionTypeUI.LeftBotton;
            if (m_sc.Equals(s, "righttop")) return PositionTypeUI.RightTop;
            if (m_sc.Equals(s, "rightbotton")) return PositionTypeUI.RightBotton;

            return Failure;
        }

        [StringConversion(typeof(HitType))]
        private object ToHitType(string s)
        {
            if (s.Length > 2) return Failure;

            AttackClass aclass;
            AttackPower apower;

            switch (char.ToUpper(s[0]))
            {
                case 'N':
                    apower = AttackPower.Normal;
                    break;
                case 'H':
                    apower = AttackPower.Hyper;
                    break;
                case 'S':
                    apower = AttackPower.Special;
                    break;
                case 'A':
                    apower = AttackPower.All;
                    break;
                default:
                    return Failure;
            }

            if (s.Length == 1) aclass = AttackClass.All;
            else
            {
                switch (char.ToUpper(s[1]))
                {
                    case 'T':
                        aclass = AttackClass.Throw;
                        break;
                    case 'P':
                        aclass = AttackClass.Projectile;
                        break;
                    case 'A':
                        aclass = AttackClass.Normal;
                        break;
                    default:
                        return Failure;
                }
            }

            return new HitType(aclass, apower);
        }

        public HitAttribute MiscToHitAttribute(string s)
        {
            return (HitAttribute)ToHitAttribute(s);
        }

        [StringConversion(typeof(HitAttribute))]
        private object ToHitAttribute(string s)
        {
            var attackheight = AttackStateType.None;
            var attackdata = new List<HitType>();

            var first = true;
            var regex = Regex.Split(s, @"\s*,\s*", RegexOptions.IgnoreCase);
            foreach (var str in regex)
            {
                if (first)
                {
                    first = false;

                    if (TryConvert(str, out attackheight) == false)
                        return Failure;
                }
                else
                {
                    if (TryConvert(str, out HitType hittype) == false)
                        return Failure;

                    attackdata.Add(hittype);
                }
            }

            return new HitAttribute(attackheight, new ReadOnlyList<HitType>(attackdata));
        }

        [StringConversion(typeof(SpriteEffects))]
        private object ToSpriteEffects(string s)
        {
            if (TryConvert(s, out int intvalue) == false) return Failure;

            return intvalue >= 0 ? SpriteEffects.None : SpriteEffects.FlipHorizontally;
        }

        [StringConversion(typeof(Blending))]
        private object ToBlending(string s)
        {
            if (string.IsNullOrEmpty(s))
                return new Blending();

            if (m_sc.Equals(s, "none"))
                return new Blending();

            if (m_sc.Equals(s, "addalpha"))
                return new Blending(BlendType.Add, 0, 0);

            if (m_sc.Equals(s, "add") || m_sc.Equals(s, "a"))
                return new Blending(BlendType.Add, 255, 255);

            if (m_sc.Equals(s, "add1") || m_sc.Equals(s, "a1"))
                return new Blending(BlendType.Add1, 255, 127);

            if (char.ToUpper(s[0]) == 'S')
                return new Blending(BlendType.Subtract, 255, 255);

            var m = m_blendingregex.Match(s);
            if (!m.Success) return Failure;

            if (int.TryParse(m.Groups[1].Value, out var source) && int.TryParse(m.Groups[2].Value, out var destination))
            {
                return new Blending(BlendType.Add, source, destination);
            }

            return Failure;
        }

        [StringConversion(typeof(SpriteId))]
        private object ToSpriteId(string s)
        {
            if (TryConvert(s, out Vector2 p) == false) return Failure;

            return new SpriteId((int)p.x, (int)p.y);
        }

        [StringConversion(typeof(StateType))]
        private object ToStateType(string s)
        {
            var sc = StringComparer.OrdinalIgnoreCase;

            if (sc.Equals(s, "a")) return StateType.Airborne;
            if (sc.Equals(s, "c")) return StateType.Crouching;
            if (sc.Equals(s, "s")) return StateType.Standing;
            if (sc.Equals(s, "l")) return StateType.Liedown;
            if (sc.Equals(s, "u")) return StateType.Unchanged;

            return Failure;
        }

        [StringConversion(typeof(HelperType))]
        private object ToHelperType(string s)
        {
            var sc = StringComparer.OrdinalIgnoreCase;

            if (sc.Equals(s, "Normal")) return HelperType.Normal;
            if (sc.Equals(s, "Player")) return HelperType.Player;
            //if (sc.Equals(s, "Projectile")) return HelperType.Projectile;
            return Failure;
        }

        [StringConversion(typeof(MoveType))]
        private object ToMoveType(string s)
        {
            var sc = StringComparer.OrdinalIgnoreCase;

            if (sc.Equals(s, "a")) return MoveType.Attack;
            if (sc.Equals(s, "i")) return MoveType.Idle;
            if (sc.Equals(s, "h")) return MoveType.BeingHit;
            if (sc.Equals(s, "u")) return MoveType.Unchanged;

            return Failure;
        }

        [StringConversion(typeof(Physic))]
        private object ToPhsyics(string s)
        {
            var sc = StringComparer.OrdinalIgnoreCase;

            if (sc.Equals(s, "s")) return UnityMugen.Physic.Standing;
            if (sc.Equals(s, "c")) return UnityMugen.Physic.Crouching;
            if (sc.Equals(s, "a")) return UnityMugen.Physic.Airborne;
            if (sc.Equals(s, "n")) return UnityMugen.Physic.None;
            if (sc.Equals(s, "u")) return UnityMugen.Physic.Unchanged;

            return Failure;
        }

        [StringConversion(typeof(HitFlag))]
        private object ToHitFlag(string s)
        {
            var high = false;
            var low = false;
            var air = false;
            var falling = false;
            var down = false;
            var combo = HitFlagCombo.DontCare;

            if (s.IndexOf("H", StringComparison.InvariantCultureIgnoreCase) != -1) high = true;
            if (s.IndexOf("L", StringComparison.InvariantCultureIgnoreCase) != -1) low = true;
            if (s.IndexOf("M", StringComparison.InvariantCultureIgnoreCase) != -1) { high = true; low = true; }
            if (s.IndexOf("A", StringComparison.InvariantCultureIgnoreCase) != -1) air = true;
            if (s.IndexOf("D", StringComparison.InvariantCultureIgnoreCase) != -1) down = true;
            if (s.IndexOf("F", StringComparison.InvariantCultureIgnoreCase) != -1) falling = true;
            if (s.IndexOf("+", StringComparison.InvariantCultureIgnoreCase) != -1) combo = HitFlagCombo.Yes;
            if (s.IndexOf("-", StringComparison.InvariantCultureIgnoreCase) != -1) combo = HitFlagCombo.No;

            return new HitFlag(combo, high, low, air, falling, down);
        }

        [StringConversion(typeof(HitAnimationType))]
        private object ToHitAnimationType(string s)
        {
            if (s.Length == 0) return Failure;

            if (char.ToUpper(s[0]) == 'L') return HitAnimationType.Light;
            if (char.ToUpper(s[0]) == 'M') return HitAnimationType.Medium;
            if (char.ToUpper(s[0]) == 'H') return HitAnimationType.Hard;
            if (char.ToUpper(s[0]) == 'B') return HitAnimationType.Back;
            if (char.ToUpper(s[0]) == 'U') return HitAnimationType.Up;
            if (char.ToUpper(s[0]) == 'D') return HitAnimationType.DiagUp;

            return Failure;
        }

        [StringConversion(typeof(PriorityType))]
        private object ToPriorityType(string s)
        {
            var sc = StringComparer.OrdinalIgnoreCase;

            if (char.ToUpper(s[0]) == 'D') return PriorityType.Dodge;
            if (char.ToUpper(s[0]) == 'H') return PriorityType.Hit;
            if (char.ToUpper(s[0]) == 'M') return PriorityType.Miss;

            return Failure;
        }

        [StringConversion(typeof(HitPriority))]
        private object ToHitPriority(string s)
        {
            var m = m_hitpriorityregex.Match(s);
            if (m.Success)
            {
                if (TryConvert(m.Groups[1].Value, out int power) == false || TryConvert(m.Groups[2].Value, out PriorityType pt) == false) return Failure;

                return new HitPriority(pt, power);
            }
            else
            {
                if (TryConvert(s, out int power) == false) return Failure;

                return new HitPriority(PriorityType.Miss, power);
            }
        }

        [StringConversion(typeof(AttackEffect))]
        private object ToAttackEffect(string s)
        {
            if (s.Length == 0) return Failure;

            if (char.ToUpper(s[0]) == 'H') return AttackEffect.High;
            if (char.ToUpper(s[0]) == 'L') return AttackEffect.Low;
            if (char.ToUpper(s[0]) == 'T') return AttackEffect.Trip;
            if (char.ToUpper(s[0]) == 'N') return AttackEffect.None;

            return Failure;
        }

        [StringConversion(typeof(ForceFeedbackType))]
        private object ToForceFeedbackType(string s)
        {
            var fft = ForceFeedbackType.None;

            if (s.Length == 4 && s.IndexOf("sine", StringComparison.OrdinalIgnoreCase) != -1) fft |= ForceFeedbackType.Sine;
            if (s.Length == 6 && s.IndexOf("square", StringComparison.OrdinalIgnoreCase) != -1) fft |= ForceFeedbackType.Square;
            return fft;
        }

        [StringConversion(typeof(AffectTeam))]
        private object ToAffectTeam(string s)
        {
            if (s.StartsWith("F", StringComparison.OrdinalIgnoreCase)) return AffectTeam.Friendly;
            if (s.StartsWith("E", StringComparison.OrdinalIgnoreCase)) return AffectTeam.Enemy;
            if (s.StartsWith("B", StringComparison.OrdinalIgnoreCase)) return AffectTeam.Both;

            return Failure;
        }

        [StringConversion(typeof(BindToTargetPostion))]
        private object ToBindToTargetPostion(string s)
        {
            if (s.Length == 4)
            {
                if (m_sc.Equals(s, "foot")) return BindToTargetPostion.Foot;
                if (m_sc.Equals(s, "head")) return BindToTargetPostion.Head;
            }
            if (s.Length == 3 && m_sc.Equals(s, "mid")) return BindToTargetPostion.Mid;

            return Failure;
        }

        [StringConversion(typeof(Assertion))]
        private object ToAssertion(string s)
        {
            var sc = StringComparer.OrdinalIgnoreCase;

            if (s.Length == 4)
            {
                if (m_sc.Equals(s, "noBG")) return Assertion.NoBackground;
                if (m_sc.Equals(s, "noFG")) return Assertion.NoForeground;
                if (m_sc.Equals(s, "NoKO")) return Assertion.NoKO;
            }
            else if (s.Length == 5)
            {
                if (m_sc.Equals(s, "intro")) return Assertion.Intro;
            }
            else if (s.Length == 6)
            {

                if (m_sc.Equals(s, "nowalk")) return Assertion.NoWalk;
            }
            else if (s.Length == 7)
            {
                if (m_sc.Equals(s, "nokosnd")) return Assertion.NoKOSound;
                if (m_sc.Equals(s, "nomusic")) return Assertion.NoMusic;
            }
            else if (s.Length == 8)
            {
                if (m_sc.Equals(s, "nokoslow")) return Assertion.NoKOSlow;
                if (m_sc.Equals(s, "noshadow")) return Assertion.NoShadow;
            }
            else if (s.Length == 9)
            {
                if (m_sc.Equals(s, "invisible")) return Assertion.Invisible;
            }
            if (s.Length == 10)
            {
                if (m_sc.Equals(s, "noairguard")) return Assertion.NoAirGuard;
                if (m_sc.Equals(s, "noautoturn")) return Assertion.NoAutoturn;
            }
            else if (s.Length == 11)
            {
                if (m_sc.Equals(s, "timerfreeze")) return Assertion.TimerFreeze;
                if (m_sc.Equals(s, "unguardable")) return Assertion.Unguardable;
            }
            else if (s.Length == 12)
            {
                if (m_sc.Equals(s, "roundnotover")) return Assertion.RoundNotOver;
                if (m_sc.Equals(s, "nobardisplay")) return Assertion.NoBarDisplay;
                if (m_sc.Equals(s, "nostandguard")) return Assertion.NoStandGuard;
            }
            else if (s.Length == 13)
            {
                if (m_sc.Equals(s, "nocrouchguard")) return Assertion.NoCrouchGuard;
                if (m_sc.Equals(s, "nojugglecheck")) return Assertion.NoJuggleCheck;
            }
            else if (s.Length == 14 && m_sc.Equals(s, "GlobalNoShadow")) return Assertion.GlobalNoShadow;


            return Failure;
        }

        //[StringConversion(typeof(ScreenShotFormat))]
        //private object ToScreenShotFormat(string s)
        //{
        //    var sc = StringComparer.OrdinalIgnoreCase;

        //    if (sc.Equals(s, "bmp")) return ScreenShotFormat.Bmp;
        //    if (sc.Equals(s, "jpg")) return ScreenShotFormat.Jpg;
        //    if (sc.Equals(s, "png")) return ScreenShotFormat.Png;

        //    return Failure;
        //}

        [StringConversion(typeof(Axis))]
        private object ToAxis(string s)
        {
            var sc = StringComparer.OrdinalIgnoreCase;

            if (sc.Equals(s, "X")) return Axis.X;
            if (sc.Equals(s, "Y")) return Axis.Y;

            return Failure;
        }

        [StringConversion(typeof(PlayerButton))]
        private object ToPlayerButton(string s)
        {
            PlayerButton value = PlayerButton.None;

            if (s.IndexOf("U", StringComparison.InvariantCultureIgnoreCase) != -1) value |= PlayerButton.Up;
            if (s.IndexOf("D", StringComparison.InvariantCultureIgnoreCase) != -1) value |= PlayerButton.Down;
            if (s.IndexOf("L", StringComparison.InvariantCultureIgnoreCase) != -1) value |= PlayerButton.Left;
            if (s.IndexOf("R", StringComparison.InvariantCultureIgnoreCase) != -1) value |= PlayerButton.Right;

            if (s.IndexOf("X", StringComparison.InvariantCultureIgnoreCase) != -1) value |= PlayerButton.X;
            if (s.IndexOf("Y", StringComparison.InvariantCultureIgnoreCase) != -1) value |= PlayerButton.Y;
            if (s.IndexOf("Z", StringComparison.InvariantCultureIgnoreCase) != -1) value |= PlayerButton.Z;

            if (s.IndexOf("A", StringComparison.InvariantCultureIgnoreCase) != -1) value |= PlayerButton.A;
            if (s.IndexOf("B", StringComparison.InvariantCultureIgnoreCase) != -1) value |= PlayerButton.B;
            if (s.IndexOf("C", StringComparison.InvariantCultureIgnoreCase) != -1) value |= PlayerButton.C;

            if (s.IndexOf("T", StringComparison.InvariantCultureIgnoreCase) != -1) value |= PlayerButton.Taunt;

            return value;
        }

        private object Failure => m_failure;
        private StringComparer m_sc;

        #region Fields

        [DebuggerBrowsable(DebuggerBrowsableState.Never)]
        private ReadOnlyDictionary<Type, Converter<string, object>> m_conversionmap;

        [DebuggerBrowsable(DebuggerBrowsableState.Never)]
        private Regex m_blendingregex;

        [DebuggerBrowsable(DebuggerBrowsableState.Never)]
        private Regex m_printdataregex;

        [DebuggerBrowsable(DebuggerBrowsableState.Never)]
        private Regex m_hitpriorityregex;

        [DebuggerBrowsable(DebuggerBrowsableState.Never)]
        private object m_failure;

        #endregion
    }
}