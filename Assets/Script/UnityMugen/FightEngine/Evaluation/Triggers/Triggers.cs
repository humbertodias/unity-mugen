using System;
using System.Collections.Generic;
using UnityEngine;
using UnityMugen.Combat;

namespace UnityMugen.Evaluation.Triggers
{

    [CustomFunction("Const240p")]
    internal static class Const240p
    {
        public static int Evaluate(Character character, ref bool error, int value)
        {
            return value;
        }

        public static float Evaluate(Character character, ref bool error, float value)
        {
            return value;
        }

        public static Node Parse(ParseState state)
        {
            return state.BuildParenNumberNode(true);
        }
    }

    [CustomFunction("Const480p")]
    internal static class Const480p
    {
        public static int Evaluate(Character character, ref bool error, int value)
        {
            return value;
        }

        public static float Evaluate(Character character, ref bool error, float value)
        {
            return value;
        }

        public static Node Parse(ParseState state)
        {
            return state.BuildParenNumberNode(true);
        }
    }

    [CustomFunction("Const720p")]
    internal static class Const720p
    {
        public static int Evaluate(Character character, ref bool error, int value)
        {
            return value;
        }

        public static float Evaluate(Character character, ref bool error, float value)
        {
            return value;
        }

        public static Node Parse(ParseState state)
        {
            return state.BuildParenNumberNode(true);
        }
    }

    [CustomFunction("Abs")]
    internal static class Abs
    {
        public static int Evaluate(Character character, ref bool error, int value)
        {
            return Math.Abs(value);
        }

        public static float Evaluate(Character character, ref bool error, float value)
        {
            return Math.Abs(value);
        }

        public static Node Parse(ParseState state)
        {
            return state.BuildParenNumberNode(true);
        }
    }

    [CustomFunction("Acos")]
    internal static class Acos
    {
        static bool KeepLog => LauncherEngine.Inst.initializationSettings.KeepLog;

        public static float Evaluate(Character character, ref bool error, float value)
        {
            if (value < -1 || value > 1)
            {
                if (KeepLog)
                    Debug.Log("Error | Acos");

                error = true;
                return 0;
            }

            return (float)Math.Acos(value);
        }

        public static Node Parse(ParseState state)
        {
            return state.BuildParenNumberNode(true);
        }
    }

    [CustomFunction("AiLevel")]
    internal static class AiLevel
    {
        public static int Evaluate(Character character, ref bool error)
        {
            if (character == null)
            {
                error = true;
                return 0;
            }

            if (character.PlayerMode == PlayerMode.Human)
                return 0;

            if (LauncherEngine.Inst.engineInitialization.Mode == UnityMugen.CombatMode.Training &&
                LauncherEngine.Inst.trainnerSettings.stanceType == UnityMugen.StanceType.COM &&
                character.PlayerMode == PlayerMode.Ai)
                return LauncherEngine.Inst.trainnerSettings.COMLevel;

            return LauncherEngine.Inst.initializationSettings.AiLevel;
        }

        public static Node Parse(ParseState state)
        {
            return state.BaseNode;
        }
    }

    [CustomFunction("Alive")]
    internal static class Alive
    {
        public static bool Evaluate(Character character, ref bool error)
        {
            if (character == null)
            {
                error = true;
                return false;
            }

            return character.Life > 0;
        }

        public static Node Parse(ParseState state)
        {
            return state.BaseNode;
        }
    }

    [CustomFunction("Anim")]
    internal static class Anim
    {
        public static int Evaluate(Character character, ref bool error)
        {
            if (character == null || character.AnimationManager.CurrentAnimation == null)
            {
                error = true;
                return 0;
            }

            return character.AnimationManager.CurrentAnimation.Number;
        }

        public static Node Parse(ParseState state)
        {
            return state.BaseNode;
        }
    }

    [CustomFunction("AnimElem")]
    internal static class AnimElem
    {
        static bool KeepLog => LauncherEngine.Inst.initializationSettings.KeepLog;
        public static bool Evaluate(Character character, ref bool error, int r1, int rhs, Operator compareType)
        {
            if (character == null)
            {
                error = true;
                return false;
            }

            var animation = character.AnimationManager.CurrentAnimation;
            if (animation == null)
            {
                error = true;
                if (KeepLog)
                    Debug.Log("Error | AnimElem");
                return false;
            }

            var elementindex = r1 - 1;
            if (elementindex < 0 || elementindex >= animation.Elements.Count)
            {
                return false;
            }

            var elementstarttime = animation.GetElementStartTime(elementindex);
            var animationtime = character.AnimationManager.TimeInAnimation;
            while (animation.TotalTime != -1 && animationtime >= animation.TotalTime)
            {
                var looptime = animation.TotalTime - animation.GetElementStartTime(animation.Loopstart);
                animationtime -= looptime;
            }

            var timeoffset = animationtime - elementstarttime;

            if (character.AnimationManager.IsAnimationFinished) return false;

            var result = SpecialFunctions.LogicalOperation(compareType, timeoffset, rhs);
            if (character.InHitPause)
                return result;
            else
                return compareType == Operator.Equals ? result && character.UpdatedAnimation : result;
        }

        public static bool Evaluate(Character character, ref bool error, int r1, int pre, int post, Operator compareType, Symbol preCheck, Symbol postCheck)
        {
            if (character == null)
            {
                if (KeepLog)
                    Debug.Log("Error | AnimElem");

                error = true;
                return false;
            }

            var animation = character.AnimationManager.CurrentAnimation;
            if (animation == null)
            {
                error = true;
                return false;
            }

            var elementindex = r1 - 1;
            if (elementindex < 0 || elementindex >= animation.Elements.Count)
            {
                if (KeepLog)
                    Debug.Log("Error | AnimElem");

                error = true;
                return false;
            }

            var elementstarttime = animation.GetElementStartTime(elementindex);
            var animationtime = character.AnimationManager.TimeInAnimation;
            while (animation.TotalTime != -1 && animationtime >= animation.TotalTime)
            {
                var looptime = animation.TotalTime - animation.GetElementStartTime(animation.Loopstart);
                animationtime -= looptime;
            }

            var timeoffset = animationtime - elementstarttime;

            if (character.AnimationManager.IsAnimationFinished) return false;

            return SpecialFunctions.Range(timeoffset, pre, post, compareType, preCheck, postCheck);
        }

        public static Node Parse(ParseState state)
        {
            var initoperator = state.CurrentOperator;
            switch (initoperator)
            {
#warning Compatability. Equals should be the only one that works.
                case Operator.Equals:
                case Operator.NotEquals:
                case Operator.GreaterEquals:
                case Operator.LesserEquals:

                case Operator.Greater: // Adicionado Por Tiago
                case Operator.Lesser: // Adicionado Por Tiago
                    ++state.TokenIndex;
                    break;

                default:
                    return null;
            }

            var arg1 = state.BuildNode(false);
            if (arg1 == null) return null;

            state.BaseNode.Children.Add(arg1);

//            if (state.CurrentSymbol != Symbol.Comma)
//            {
//#warning Hack
//                state.BaseNode.Children.Add(Node.ZeroNode);
//                state.BaseNode.Arguments.Add(Operator.Equals);

//                return state.BaseNode;
//            }

            if (state.CurrentSymbol != Symbol.Comma)
            {
                if (state.CurrentToken?.AsOperator != null)
                {
                    switch (state.CurrentToken.AsOperator)
                    {
                        case Operator.Equals:
                        case Operator.NotEquals:
                        case Operator.GreaterEquals:
                        case Operator.LesserEquals:
                        case Operator.Greater:
                        case Operator.Lesser:
                            --state.TokenIndex;
                            break;

                        default:
                            state.BaseNode.Children.Add(Node.ZeroNode);
                            state.BaseNode.Arguments.Add(Operator.Equals);
                            return state.BaseNode;
                    }
                }
                else
                {
                    state.BaseNode.Children.Add(Node.ZeroNode);
                    state.BaseNode.Arguments.Add(Operator.Equals);
                    return state.BaseNode;
                }
            }

            ++state.TokenIndex;

            var @operator = state.CurrentOperator;
            if (@operator == Operator.Equals || @operator == Operator.NotEquals)
            {
                ++state.TokenIndex;

                var rangenode = state.BuildRangeNode();
                if (rangenode != null)
                {
                    state.BaseNode.Children.Add(rangenode.Children[1]);
                    state.BaseNode.Children.Add(rangenode.Children[2]);
                    state.BaseNode.Arguments.Add(@operator);
                    state.BaseNode.Arguments.Add(rangenode.Arguments[1]);
                    state.BaseNode.Arguments.Add(rangenode.Arguments[2]);

                    return state.BaseNode;
                }

                --state.TokenIndex;
            }

            switch (@operator)
            {
                case Operator.Equals:
                case Operator.NotEquals:
                case Operator.GreaterEquals:
                case Operator.LesserEquals:
                case Operator.Lesser:
                case Operator.Greater:
                    ++state.TokenIndex;
                    break;

                default:
                    return null;
            }

            var arg = state.BuildNode(false);
            if (arg == null) return null;

            state.BaseNode.Arguments.Add(@operator);
            state.BaseNode.Children.Add(arg);

            return state.BaseNode;
        }
    }

    [CustomFunction("AnimElemNo")]
    internal static class AnimElemNo
    {
        static bool KeepLog => LauncherEngine.Inst.initializationSettings.KeepLog;

        public static int Evaluate(Character character, ref bool error, int timeoffset)
        {
            if (character == null)
            {
                error = true;
                return 0;
            }

            var animation = character.AnimationManager.CurrentAnimation;
            if (animation == null)
            {
                if (KeepLog)
                    Debug.Log("Error | AnimElemNo");

                error = true;
                return 0;
            }

            var animtime = character.AnimationManager.TimeInAnimation;

            var checktime = animtime + timeoffset;
            if (checktime < 0)
            {
                if (KeepLog)
                    Debug.Log("Error | AnimElemNo");

                error = true;
                return 0;
            }

            var elemIndex = animation.GetElementFromTime(checktime).Id;
            return elemIndex + 1;
        }

        public static Node Parse(ParseState state)
        {
            return state.BuildParenNumberNode(true);
        }
    }

    [CustomFunction("AnimElemTime")]
    internal static class AnimElemTime
    {
        static bool KeepLog => LauncherEngine.Inst.initializationSettings.KeepLog;
        public static int Evaluate(Character character, ref bool error, int value)
        {
            if (character == null)
            {
                if (KeepLog)
                    Debug.Log("Error | AnimElemTime");

                error = true;
                return 0;
            }

            var animation = character.AnimationManager.CurrentAnimation;
            if (animation == null)
            {
                if (KeepLog)
                    Debug.Log("Error | AnimElemTime");

                error = true;
                return 0;
            }

            var elementIndex = value - 1;
            if (animation.Elements.Count <= elementIndex) return 0;

            var animationTime = character.AnimationManager.TimeInAnimation;
            var timeLoop = character.AnimationManager.TimeLoop;
            var elementStarttime = animation.GetElementStartTime(elementIndex);

            var result = animationTime - elementStarttime;
            if (animation.TotalTime != -1 && animationTime > animation.TotalTime && !(value == 1 && timeLoop == 0))
            {
                var firstTickLoopElement = animation.GetElementStartTime(animation.Loopstart);
                //if (elementIndex != animation.Loopstart && timeLoop != firstTickLoopElement)
                if (!(elementIndex == animation.Loopstart && timeLoop == firstTickLoopElement))
                    result = timeLoop - elementStarttime;
            }
            return result;
        }

        public static Node Parse(ParseState state)
        {
            return state.BuildParenNumberNode(true);
        }
    }

    [CustomFunction("AnimExist")]
    internal static class AnimExist
    {
        static bool KeepLog => LauncherEngine.Inst.initializationSettings.KeepLog;
        public static bool Evaluate(Character character, ref bool error, int value)
        {
            if (character == null)
            {
                error = true;
                return false;
            }

            if (character.AnimationManager.IsForeignAnimation)
            {
                if (KeepLog)
                    Debug.Log("Error | AnimElemTime");

                error = true;
                return false;
            }

            return character.AnimationManager.HasAnimation(value);
        }

        public static Node Parse(ParseState state)
        {
            return state.BuildParenNumberNode(true);
        }
    }

    [CustomFunction("AnimTime")]
    internal static class AnimTime
    {
        static bool KeepLog => LauncherEngine.Inst.initializationSettings.KeepLog;
        public static int Evaluate(Character character, ref bool error)
        {
            if (character == null)
            {
                error = true;
                return 0;
            }

            var animation = character.AnimationManager.CurrentAnimation;
            if (animation == null)
            {
                if (KeepLog)
                    Debug.Log("Error | AnimTime");

                error = true;
                return 0;
            }

            var animtime = character.AnimationManager.TimeLoop;

            if (animation.TotalTime == -1)
            {
                return animtime + 1;
            }

            return animtime - animation.TotalTime;
        }

        public static Node Parse(ParseState parsestate)
        {
            return parsestate.BaseNode;
        }
    }

    [CustomFunction("Asin")]
    internal static class Asin
    {
        static bool KeepLog => LauncherEngine.Inst.initializationSettings.KeepLog;
        public static float Evaluate(Character character, ref bool error, float value)
        {
            if (value < -1 || value > 1)
            {
                if (KeepLog)
                    Debug.Log("Error | Asin");

                error = true;
                return 0;
            }

            return (float)Math.Asin(value);
        }

        public static Node Parse(ParseState state)
        {
            return state.BuildParenNumberNode(true);
        }
    }

    [CustomFunction("Assertion")]
    internal static class AssertionT
    {
        static bool KeepLog => LauncherEngine.Inst.initializationSettings.KeepLog;
        public static bool Evaluate(Character character, ref bool error, UnityMugen.Assertion assertion)
        {
            if (character == null)
            {
                error = true;
                return false;
            }

            switch (assertion)
            {
                case UnityMugen.Assertion.Intro:
                    return character.Engine.Assertions.Intro;

                case UnityMugen.Assertion.Invisible:
                    return character.Assertions.Invisible;

                case UnityMugen.Assertion.RoundNotOver:
                    return character.Engine.Assertions.WinPose;

                case UnityMugen.Assertion.NoBarDisplay:
                    return character.Engine.Assertions.NoBarDisplay;

                case UnityMugen.Assertion.NoBackground:
                    return character.Engine.Assertions.NoBackLayer;

                case UnityMugen.Assertion.NoForeground:
                    return character.Engine.Assertions.NoFrontLayer;

                case UnityMugen.Assertion.NoStandGuard:
                    return character.Assertions.NoStandingGuard;

                case UnityMugen.Assertion.NoAirGuard:
                    return character.Assertions.NoAirGuard;

                case UnityMugen.Assertion.NoCrouchGuard:
                    return character.Assertions.NoCrouchingGuard;

                case UnityMugen.Assertion.NoAutoturn:
                    return character.Assertions.NoAutoTurn;

                case UnityMugen.Assertion.NoJuggleCheck:
                    return character.Assertions.NoJuggleCheck;

                case UnityMugen.Assertion.NoKOSound:
                    return character.Engine.Assertions.NoKOSound;

                case UnityMugen.Assertion.NoKOSlow:
                    return character.Engine.Assertions.NoKOSlow;

                case UnityMugen.Assertion.NoShadow:
                    return character.Assertions.NoShadow;

                case UnityMugen.Assertion.GlobalNoShadow:
                    return character.Engine.Assertions.GlobalNoShadow;

                case UnityMugen.Assertion.NoMusic:
                    return character.Engine.Assertions.NoMusic;

                case UnityMugen.Assertion.NoWalk:
                    return character.Assertions.NoWalk;

                case UnityMugen.Assertion.TimerFreeze:
                    return character.Engine.Assertions.TimerFreeze;

                case UnityMugen.Assertion.Unguardable:
                    return character.Assertions.UnGuardable;

                default:
                    if (KeepLog)
                        Debug.Log("Error | Assertion");

                    error = true;
                    return false;
            }
        }

        public static Node Parse(ParseState state)
        {
            if (state.CurrentSymbol != Symbol.LeftParen) return null;
            ++state.TokenIndex;

            if (state.CurrentUnknown == null) return null;
            var assert = state.ConvertCurrentToken<UnityMugen.Assertion>();

            state.BaseNode.Arguments.Add(assert);
            ++state.TokenIndex;

            if (state.CurrentSymbol != Symbol.RightParen) return null;
            ++state.TokenIndex;

            return state.BaseNode;
        }
    }

    [CustomFunction("Atan")]
    internal static class Atan
    {
        public static float Evaluate(Character character, ref bool error, float value)
        {
            return (float)Math.Atan(value);
        }

        public static Node Parse(ParseState state)
        {
            return state.BuildParenNumberNode(true);
        }
    }

    [CustomFunction("AuthorName")]
    internal static class AuthorName
    {
        static bool KeepLog => LauncherEngine.Inst.initializationSettings.KeepLog;
        public static bool Evaluate(Character character, ref bool error, Operator @operator, string text)
        {
            if (character == null)
            {
                error = true;
                return false;
            }

            var authorname = character.BasePlayer.profile.author;
            if (authorname == null)
            {
                error = true;
                if (KeepLog)
                    Debug.Log("Error | AuthorName");
                return false;
            }

            var result = string.Equals(authorname, text, StringComparison.OrdinalIgnoreCase);

            switch (@operator)
            {
                case Operator.Equals:
                    return result;

                case Operator.NotEquals:
                    return !result;

                default:
                    if (KeepLog)
                        Debug.Log("Error | AuthorName");

                    error = true;
                    return false;
            }
        }

        public static Node Parse(ParseState state)
        {
            var @operator = state.CurrentOperator;
            if (@operator != Operator.Equals && @operator != Operator.NotEquals) return null;
            ++state.TokenIndex;

            var text = state.CurrentText;
            if (text == null) return null;
            ++state.TokenIndex;

            state.BaseNode.Arguments.Add(@operator);
            state.BaseNode.Arguments.Add(text);
            return state.BaseNode;
        }
    }

    [CustomFunction("BackEdgeBodyDist")]
    internal static class BackEdgeBodyDist
    {
        static bool KeepLog => LauncherEngine.Inst.initializationSettings.KeepLog;
        public static float Evaluate(Character character, ref bool error)
        {
            if (character == null)
            {
                error = true;
                return 0;
            }

            var camerarect = character.Engine.CameraFE.ScreenBounds();
            var stage = character.Engine.stageScreen.Stage;

            switch (character.CurrentFacing)
            {
                case UnityMugen.Facing.Left:
                    return ((camerarect.xMax - character.GetRightEdgePosition(true)) * Constant.Scale2);

                case UnityMugen.Facing.Right:
                    return ((character.GetLeftEdgePosition(true) - camerarect.xMin) * Constant.Scale2);

                default:
                    if (KeepLog)
                        Debug.Log("Error | BackEdgeBodyDist");

                    error = true;
                    return 0;
            }
        }

        public static Node Parse(ParseState state)
        {
            return state.BaseNode;
        }
    }

    [CustomFunction("BackEdgeDist")]
    internal static class BackEdgeDist
    {
        static bool KeepLog => LauncherEngine.Inst.initializationSettings.KeepLog;
        public static float Evaluate(Character character, ref bool error)
        {
            if (character == null)
            {
                error = true;
                return 0;
            }

            var camerarect = character.Engine.CameraFE.ScreenBounds();
            var stage = character.Engine.stageScreen.Stage;

            switch (character.CurrentFacing)
            {
                case UnityMugen.Facing.Left:
                    return ((camerarect.xMax - character.GetRightEdgePosition(false)) * Constant.Scale2);

                case UnityMugen.Facing.Right:
                    return ((character.GetLeftEdgePosition(false) - camerarect.xMin) * Constant.Scale2);//Atenção aqui

                default:
                    if (KeepLog)
                        Debug.Log("Error | BackEdgeDist");

                    error = true;
                    return 0;
            }
        }

        public static Node Parse(ParseState state)
        {
            return state.BaseNode;
        }
    }

#warning esta faltando
    [CustomFunction("CameraPos")]
    internal static class CameraPos
    {
        public static float Evaluate(Character character, ref bool error, Axis axis)
        {
            if (character == null)
            {
                error = true;
                return 0;
            }
            return 0; 
            switch (axis)
            {
                case Axis.X:
                    return character.Engine.CameraFE.transform.position.x * Constant.Scale2;

                case Axis.Y:
                    return character.Engine.CameraFE.transform.position.y * Constant.Scale2;

                default:
                    error = true;
                    return 0;
            }
        }

        public static Node Parse(ParseState parsestate)
        {
            var axis = parsestate.ConvertCurrentToken<Axis>();
            if (axis == Axis.None) return null;

            ++parsestate.TokenIndex;

            parsestate.BaseNode.Arguments.Add(axis);
            return parsestate.BaseNode;
        }
    }


#warning esta faltando
    //CameraZoom

    [CustomFunction("CanRecover")]
    internal static class CanRecover
    {
        public static bool Evaluate(Character character, ref bool error)
        {
            if (character == null)
            {
                error = true;
                return false;
            }

            return character.DefensiveInfo.IsFalling == false ? false : character.DefensiveInfo.HitDef.FallCanRecover;
        }

        public static Node Parse(ParseState state)
        {
            return state.BaseNode;
        }
    }

    [CustomFunction("Ceil")]
    internal static class Ceil
    {
        public static int Evaluate(Character character, ref bool error, int value)
        {
            return value;
        }

        public static float Evaluate(Character character, ref bool error, float value)
        {
            return value;
            //return (int)Math.Ceiling(value);
        }

        public static Node Parse(ParseState state)
        {
            return state.BuildParenNumberNode(true);
        }
    }

    [CustomFunction("Command")]
    internal static class Command
    {
        static bool KeepLog => LauncherEngine.Inst.initializationSettings.KeepLog;
        public static bool Evaluate(Character character, ref bool error, Operator @operator, string text)
        {
            if (character == null)
            {
                error = true;
                return false;
            }

            var active = character.CommandManager.IsActive(text);
            switch (@operator)
            {
                case Operator.Equals:
                    return active;

                case Operator.NotEquals:
                    return !active;

                default:
                    if (KeepLog)
                        Debug.Log("Error | Command");

                    error = true;
                    return false;
            }
        }

        public static Node Parse(ParseState state)
        {
            var @operator = state.CurrentOperator;
            if (@operator != Operator.Equals && @operator != Operator.NotEquals) return null;
            ++state.TokenIndex;

            var text = state.CurrentText;
            if (text == null) return null;
            ++state.TokenIndex;

            state.BaseNode.Arguments.Add(@operator);
            state.BaseNode.Arguments.Add(text);
            return state.BaseNode;
        }
    }

    [CustomFunction("Cond")]
    internal static class Cond
    {
        public static int Evaluate(Character character, ref bool error, int r1, int r2, int r3)
        {
            if (r1 < 0)
                return r1;

            return r1 != 0 ? r2 : r3;
        }

        public static float Evaluate(Character character, ref bool error, float r1, float r2, float r3)
        {
            if (r1 < 0)
                return r1;

            return r1 != 0 ? r2 : r3;
        }

        public static Node Parse(ParseState parsestate)
        {
            if (parsestate.CurrentSymbol != Symbol.LeftParen) return null;
            ++parsestate.TokenIndex;

            var c1 = parsestate.BuildNode(true);
            if (c1 == null) return null;
            parsestate.BaseNode.Children.Add(c1);

            if (parsestate.CurrentSymbol != Symbol.Comma) return null;
            ++parsestate.TokenIndex;

            var c2 = parsestate.BuildNode(true);
            if (c2 == null) return null;
            parsestate.BaseNode.Children.Add(c2);

            if (parsestate.CurrentSymbol != Symbol.Comma) return null;
            ++parsestate.TokenIndex;

            var c3 = parsestate.BuildNode(true);
            if (c3 == null) return null;
            parsestate.BaseNode.Children.Add(c3);

            if (parsestate.CurrentSymbol != Symbol.RightParen) return null;
            ++parsestate.TokenIndex;

            return parsestate.BaseNode;
        }
    }

    [CustomFunction("Cos")]
    internal static class Cos
    {
        public static float Evaluate(Character character, ref bool error, float value)
        {
            return (float)Math.Cos(value);
        }

        public static Node Parse(ParseState state)
        {
            return state.BuildParenNumberNode(true);
        }
    }

    [CustomFunction("Ctrl")]
    internal static class Ctrl
    {
        public static bool Evaluate(Character character, ref bool error)
        {
            if (character == null)
            {
                error = true;
                return false;
            }

            return character.PlayerControl == PlayerControl.InControl;
        }

        public static Node Parse(ParseState state)
        {
            return state.BaseNode;
        }
    }

    [CustomFunction("DrawGame")]
    internal static class DrawGame
    {
        public static bool Evaluate(Character character, ref bool error)
        {
            if (character == null)
            {
                error = true;
                return false;
            }

            return character.Engine.Team1.VictoryStatus.Lose && character.Engine.Team2.VictoryStatus.Lose;
        }

        public static Node Parse(ParseState state)
        {
            return state.BaseNode;
        }
    }

    [CustomFunction("E")]
    internal static class E
    {
        public static float Evaluate(Character character, ref bool error)
        {
            return (float)Math.E;
        }

        public static Node Parse(ParseState state)
        {
            return state.BaseNode;
        }
    }

    [CustomFunction("Exp")]
    internal static class ExpT
    {
        public static float Evaluate(Character character, ref bool error, float value)
        {
            return (float)Math.Exp(value);
        }

        public static Node Parse(ParseState state)
        {
            return state.BuildParenNumberNode(true);
        }
    }

    [CustomFunction("Facing")]
    internal static class FacingTrigger
    {
        static bool KeepLog => LauncherEngine.Inst.initializationSettings.KeepLog;
        public static int Evaluate(Character character, ref bool error)
        {
            if (character == null)
            {
                error = true;
                return 0;
            }

            switch (character.CurrentFacing)
            {
                case Facing.Left:
                    return -1;

                case Facing.Right:
                    return 1;

                default:
                    if (KeepLog)
                        Debug.Log("Error | Facing");

                    error = true;
                    return 0;
            }
        }

        public static Node Parse(ParseState state)
        {
            return state.BaseNode;
        }
    }

    [CustomFunction("Floor")]
    internal static class Floor
    {
        public static int Evaluate(Character character, ref bool error, int value)
        {
            return value;
        }

        public static int Evaluate(Character character, ref bool error, float value)
        {
            return (int)Math.Floor(value);
        }

        public static Node Parse(ParseState state)
        {
            return state.BuildParenNumberNode(true);
        }
    }

    [CustomFunction("FrontEdgeBodyDist")]
    internal static class FrontEdgeBodyDist
    {
        static bool KeepLog => LauncherEngine.Inst.initializationSettings.KeepLog;
        public static int Evaluate(Character character, ref bool error)
        {
            if (character == null)
            {
                error = true;
                return 0;
            }

            var camerarect = character.Engine.CameraFE.ScreenBounds();
            var stage = character.Engine.stageScreen.Stage;

            switch (character.CurrentFacing)
            {
                case UnityMugen.Facing.Left:
                    return (int)((character.GetLeftEdgePosition(true) - camerarect.xMin) * Constant.Scale2);

                case UnityMugen.Facing.Right:
                    return (int)((camerarect.xMax - character.GetRightEdgePosition(true)) * Constant.Scale2);

                default:
                    if (KeepLog)
                        Debug.Log("Error | Facing");

                    error = true;
                    return 0;
            }
        }

        public static Node Parse(ParseState state)
        {
            return state.BaseNode;
        }
    }

    [CustomFunction("FrontEdgeDist")]
    internal static class FrontEdgeDist
    {
        static bool KeepLog => LauncherEngine.Inst.initializationSettings.KeepLog;
        public static int Evaluate(Character character, ref bool error)
        {
            if (character == null)
            {
                error = true;
                return 0;
            }

            var camerarect = character.Engine.CameraFE.ScreenBounds();

            switch (character.CurrentFacing)
            {
                case UnityMugen.Facing.Left:
                    return (int)((character.GetLeftEdgePosition(false) - camerarect.xMin) * Constant.Scale2);

                case UnityMugen.Facing.Right:
                    return (int)((camerarect.xMax - character.GetRightEdgePosition(false)) * Constant.Scale2);

                default:
                    if (KeepLog)
                        Debug.Log("Error | FrontEdgeDist");

                    error = true;
                    return 0;
            }
        }

        public static Node Parse(ParseState state)
        {
            return state.BaseNode;
        }
    }

    [CustomFunction("FVar")]
    internal static class FVar
    {
        static bool KeepLog => LauncherEngine.Inst.initializationSettings.KeepLog;
        public static float Evaluate(Character character, ref bool error, int value)
        {
            if (character == null)
            {
                error = true;
                return 0;
            }

            if (character.Variables.GetFloat(value, false, out float result))
                return result;

            if (KeepLog)
                Debug.Log("Error | FVar");

            error = true;
            return 0;
        }

        public static Node Parse(ParseState state)
        {
            return state.BuildParenNumberNode(true);
        }
    }

    [CustomFunction("GameTime")]
    internal static class GameTime
    {
        public static int Evaluate(Character character, ref bool error)
        {
            if (character == null)
            {
                error = true;
                return 0;
            }

            return character.Engine.TickCount;
        }

        public static Node Parse(ParseState state)
        {
            return state.BaseNode;
        }
    }

    [CustomFunction("HitCount")]
    internal static class HitCount
    {
        public static int Evaluate(Character character, ref bool error)
        {
            if (character == null)
            {
                error = true;
                return 0;
            }

            return character.OffensiveInfo.HitCount;
        }

        public static Node Parse(ParseState parsestate)
        {
            return parsestate.BaseNode;
        }
    }

    [CustomFunction("HitDefAttr")]
    internal static class HitDefAttr
    {
        static bool KeepLog => LauncherEngine.Inst.initializationSettings.KeepLog;
        public static bool Evaluate(Character character, ref bool error, Operator @operator, AttackStateType ast, HitType[] hittypes)
        {
            if (character == null)
            {
                error = true;
                return false;
            }

            if (character.MoveType != UnityMugen.MoveType.Attack) return false;

            var attr = character.OffensiveInfo.HitDef.HitAttribute;

            var heightmatch = (attr.AttackHeight & ast) != AttackStateType.None;

            var datamatch = false;
            foreach (var hittype in hittypes)
            {
                if (attr.HasData(hittype) == false) continue;

                datamatch = true;
                break;
            }

            switch (@operator)
            {
                case Operator.Equals:
                    return heightmatch && datamatch;

                case Operator.NotEquals:
                    return !heightmatch || !datamatch;

                default:
                    if (KeepLog)
                        Debug.Log("Error | HitDefAttr");

                    error = true;
                    return false;
            }
        }

        public static Node Parse(ParseState parsestate)
        {
            var @operator = parsestate.CurrentOperator;
            if (@operator != Operator.Equals && @operator != Operator.NotEquals) return null;

            parsestate.BaseNode.Arguments.Add(@operator);
            ++parsestate.TokenIndex;

            var ast = parsestate.ConvertCurrentToken<AttackStateType>();
            if (ast == AttackStateType.None) return null;

            parsestate.BaseNode.Arguments.Add(ast);
            ++parsestate.TokenIndex;

            var hittypes = new List<HitType>();

            while (true)
            {
                if (parsestate.CurrentSymbol != Symbol.Comma) break;
                ++parsestate.TokenIndex;

                var hittype = parsestate.ConvertCurrentToken<HitType?>();
                if (hittype == null)
                {
                    --parsestate.TokenIndex;
                    break;
                }

                hittypes.Add(hittype.Value);
                ++parsestate.TokenIndex;
            }

            parsestate.BaseNode.Arguments.Add(hittypes.ToArray());
            return parsestate.BaseNode;
        }
    }

    [CustomFunction("HitFall")]
    internal static class HitFall
    {
        public static bool Evaluate(Character character, ref bool error)
        {
            if (character == null)
            {
                error = true;
                return false;
            }

            return character.DefensiveInfo.IsFalling;
        }

        public static Node Parse(ParseState parsestate)
        {
            return parsestate.BaseNode;
        }
    }

    [CustomFunction("HitOver")]
    internal static class HitOver
    {
        public static bool Evaluate(Character character, ref bool error)
        {
            if (character == null)
            {
                error = true;
                return false;
            }

            return character.DefensiveInfo.HitTime <= 0;
        }

        public static Node Parse(ParseState parsestate)
        {
            return parsestate.BaseNode;
        }
    }

    [CustomFunction("HitPauseTime")]
    internal static class HitPauseTime
    {
        public static int Evaluate(Character character, ref bool error)
        {
            if (character == null)
            {
                error = true;
                return 0;
            }

            return character.OffensiveInfo.HitPauseTime;
        }

        public static Node Parse(ParseState parsestate)
        {
            return parsestate.BaseNode;
        }
    }

    [CustomFunction("HitShakeOver")]
    internal static class HitShakeOver
    {
        public static bool Evaluate(Character character, ref bool error)
        {
            if (character == null)
            {
                error = true;
                return false;
            }

            return character.DefensiveInfo.HitShakeTime <= 0;
        }

        public static Node Parse(ParseState parsestate)
        {
            return parsestate.BaseNode;
        }
    }

    [CustomFunction("HitVel")]
    internal static class HitVel
    {
        static bool KeepLog => LauncherEngine.Inst.initializationSettings.KeepLog;
        public static float Evaluate(Character character, ref bool error, Axis axis)
        {
            if (character == null)
            {
                error = true;
                return 0;
            }

            switch (axis)
            {
                case Axis.X:
                    return character.DefensiveInfo.GetHitVelocity().x * Constant.Scale2;

                case Axis.Y:
                    return character.DefensiveInfo.GetHitVelocity().y * Constant.Scale2;

                default:
                    if (KeepLog)
                        Debug.Log("Error | HitVel");

                    error = true;
                    return 0;
            }
        }

        public static Node Parse(ParseState parsestate)
        {
            var axis = parsestate.ConvertCurrentToken<Axis>();
            if (axis == Axis.None) return null;

            ++parsestate.TokenIndex;

            parsestate.BaseNode.Arguments.Add(axis);
            return parsestate.BaseNode;
        }
    }

    [CustomFunction("ID")]
    internal static class ID
    {
        public static int Evaluate(Character character, ref bool error)
        {
            if (character == null)
            {
                error = true;
                return 0;
            }

            return (int)character.Id;
        }

        public static Node Parse(ParseState parsestate)
        {
            return parsestate.BaseNode;
        }
    }

    [CustomFunction("IfElse")]
    internal static class IfElse
    {
        public static int Evaluate(Character character, ref bool error, int r1, int r2, int r3)
        {
            return r1 != 0 ? r2 : r3;
        }

        public static float Evaluate(Character character, ref bool error, float r1, float r2, float r3)
        {
            return r1 != 0 ? r2 : r3;
        }

        public static Node Parse(ParseState parsestate)
        {
            if (parsestate.CurrentSymbol != Symbol.LeftParen) return null;
            ++parsestate.TokenIndex;

            var c1 = parsestate.BuildNode(true);
            if (c1 == null) return null;
            parsestate.BaseNode.Children.Add(c1);

            if (parsestate.CurrentSymbol != Symbol.Comma) return null;
            ++parsestate.TokenIndex;

            var c2 = parsestate.BuildNode(true);
            if (c2 == null) return null;
            parsestate.BaseNode.Children.Add(c2);

            if (parsestate.CurrentSymbol != Symbol.Comma) return null;
            ++parsestate.TokenIndex;

            var c3 = parsestate.BuildNode(true);
            if (c3 == null) return null;
            parsestate.BaseNode.Children.Add(c3);

            if (parsestate.CurrentSymbol != Symbol.RightParen) return null;
            ++parsestate.TokenIndex;

            return parsestate.BaseNode;
        }
    }

    [CustomFunction("InGuardDist")]
    internal static class InGuardDist
    {
        public static bool Evaluate(Character character, ref bool error)
        {
            if (character == null)
            {
                error = true;
                return false;
            }

            foreach (var entity in character.Engine.Entities)
            {
                var opp = character.FilterEntityAsCharacter(entity, AffectTeam.Enemy);
                if (opp != null && opp.OffensiveInfo.ActiveHitDef && InGuardDistance(opp.OffensiveInfo.HitDef, opp, character))
                {
                    return true;
                }

                var projectile = entity as UnityMugen.Combat.Projectile;
                if (projectile != null && projectile.Team != character.Team && InGuardDistance(projectile.Data.HitDef, projectile, character))
                {
                    return true;
                }
            }

            return false;
        }

        private static bool InGuardDistance(HitDefinition hitdef, Entity attacker, Character target)
        {
            if (attacker == null) throw new ArgumentNullException(nameof(attacker));
            if (target == null) throw new ArgumentNullException(nameof(target));
            if (hitdef == null) throw new ArgumentNullException(nameof(hitdef));

            // Não usar Math.Abs pois as coordenados do mundo em unity estao em ponto flutuante
            var distance = /*Math.Abs(*/attacker.CurrentLocation.x - target.CurrentLocation.x/*)*/;

            return distance <= hitdef.GuardDistance;
        }

        public static Node Parse(ParseState parsestate)
        {
            return parsestate.BaseNode;
        }
    }

    [CustomFunction("IsHelper")]
    internal static class IsHelper
    {
        public static bool Evaluate(Character character, ref bool error, int helperid)
        {
            if (character == null)
            {
                error = true;
                return false;
            }

            if (character.typeEntity != TypeEntity.Helper)
                return false;

            return helperid >= 0 ? character.Id == helperid : true;
        }

        public static Node Parse(ParseState parsestate)
        {
            var node = parsestate.BuildParenNumberNode(true);
            if (node != null)
            {
                return node;
            }

            parsestate.BaseNode.Children.Add(Node.NegativeOneNode);
            return parsestate.BaseNode;
        }
    }

    [CustomFunction("IsHomeTeam")]
    internal static class IsHomeTeam
    {
        public static bool Evaluate(Character character, ref bool error)
        {
            if (character == null)
            {
                error = true;
                return false;
            }

            return character.Team.Side == UnityMugen.TeamSide.Left;
        }

        public static Node Parse(ParseState parsestate)
        {
            return parsestate.BaseNode;
        }
    }

    [CustomFunction("Life")]
    internal static class Life
    {
        public static float Evaluate(Character character, ref bool error)
        {
            if (character == null)
            {
                error = true;
                return 0;
            }

            return character.Life;
        }

        public static Node Parse(ParseState state)
        {
            return state.BaseNode;
        }
    }

    [CustomFunction("LifeMax")]
    internal static class LifeMax
    {
        public static float Evaluate(Character character, ref bool error)
        {
            if (character == null)
            {
                error = true;
                return 0;
            }

            return character.BasePlayer.playerConstants.MaximumLife;
        }

        public static Node Parse(ParseState state)
        {
            return state.BaseNode;
        }
    }

    [CustomFunction("Ln")]
    internal static class Ln
    {
        public static float Evaluate(Character character, ref bool error, float value)
        {
            return (float)Math.Log10(value);
        }

        public static Node Parse(ParseState state)
        {
            return state.BuildParenNumberNode(true);
        }
    }

    [CustomFunction("Log")]
    internal static class Log
    {
        public static float Evaluate(Character character, ref bool error, float lhs, float rhs)
        {
            return (float)Math.Log(lhs, rhs);
        }

        public static Node Parse(ParseState state)
        {
            return state.BuildParenNumberNode(true);
        }
    }

    [CustomFunction("Lose")]
    internal static class Lose
    {
        public static bool Evaluate(Character character, ref bool error)
        {
            if (character == null)
            {
                error = true;
                return false;
            }

            return character.Team.VictoryStatus.Lose;
        }

        public static Node Parse(ParseState parsestate)
        {
            return parsestate.BaseNode;
        }
    }

    [CustomFunction("LoseKO")]
    internal static class LoseKO
    {
        public static bool Evaluate(Character character, ref bool error)
        {
            if (character == null)
            {
                error = true;
                return false;
            }

            return character.Team.VictoryStatus.LoseKO;
        }

        public static Node Parse(ParseState parsestate)
        {
            return parsestate.BaseNode;
        }
    }

    [CustomFunction("LoseTime")]
    internal static class LoseTime
    {
        public static bool Evaluate(Character character, ref bool error)
        {
            if (character == null)
            {
                error = true;
                return false;
            }

            return character.Team.VictoryStatus.LoseTime;
        }

        public static Node Parse(ParseState parsestate)
        {
            return parsestate.BaseNode;
        }
    }

    [CustomFunction("MatchNo")]
    internal static class MatchNo
    {
        public static int Evaluate(Character character, ref bool error)
        {
            if (character == null)
            {
                error = true;
                return 0;
            }

            return character.Engine.MatchNumber;
        }

        public static Node Parse(ParseState parsestate)
        {
            return parsestate.BaseNode;
        }
    }

    [CustomFunction("MatchOver")]
    internal static class MatchOver
    {
        public static bool Evaluate(Character character, ref bool error)
        {
            if (character == null)
            {
                error = true;
                return false;
            }

            return character.Engine.IsMatchOver();
        }

        public static Node Parse(ParseState parsestate)
        {
            return parsestate.BaseNode;
        }
    }

    [CustomFunction("MoveContact")]
    internal static class MoveContact
    {
        public static int Evaluate(Character character, ref bool error)
        {
            if (character == null)
            {
                error = true;
                return 0;
            }

            //int result = character.MoveType == UnityMugen.MoveType.Attack ? character.OffensiveInfo.MoveContact : 0;
            //return result != 0; // Atualizado Tiago
#warning em teste
            return character.OffensiveInfo.MoveContact; // Baseado no IK
        }

        public static Node Parse(ParseState parsestate)
        {
            return parsestate.BaseNode;
        }
    }

    [CustomFunction("MoveGuarded")]
    internal static class MoveGuarded
    {
        public static int Evaluate(Character character, ref bool error)
        {
            if (character == null)
            {
                error = true;
                return 0;
            }

            //int result = character.MoveType == UnityMugen.MoveType.Attack ? character.OffensiveInfo.MoveGuarded : 0;
            //return result != 0; // Atualizado Tiago
#warning em teste
            return character.OffensiveInfo.MoveGuarded; // Baseado no IK
        }

        public static Node Parse(ParseState parsestate)
        {
            return parsestate.BaseNode;
        }
    }


    [CustomFunction("MoveReversed")]
    internal static class MoveReversed
    {
        public static int Evaluate(Character character, ref bool error)
        {
            if (character == null)
            {
                error = true;
                return 0;
            }

            int result = character.MoveType == UnityMugen.MoveType.Attack ? character.OffensiveInfo.MoveReversed : 0;
            return result; // Atualizado Tiago
        }

        public static Node Parse(ParseState parsestate)
        {
            return parsestate.BaseNode;
        }
    }

    [CustomFunction("MoveType")]
    internal static class MoveTypeT
    {
        static bool KeepLog => LauncherEngine.Inst.initializationSettings.KeepLog;
        public static bool Evaluate(Character character, ref bool error, Operator @operator, UnityMugen.MoveType movetype)
        {
            if (character == null)
            {
                error = true;
                return false;
            }

            if (movetype == UnityMugen.MoveType.Unchanged || movetype == UnityMugen.MoveType.None)
            {
                error = true;
                return false;
            }

            switch (@operator)
            {
                case Operator.Equals:
                    return movetype == character.MoveType;

                case Operator.NotEquals:
                    return movetype != character.MoveType;

                default:
                    if (KeepLog)
                        Debug.Log("Error | MoveType");

                    error = true;
                    return false;
            }
        }

        public static Node Parse(ParseState parsestate)
        {
            var @operator = parsestate.CurrentOperator;
            if (@operator != Operator.Equals && @operator != Operator.NotEquals) return null;

            ++parsestate.TokenIndex;

            var movetype = parsestate.ConvertCurrentToken<UnityMugen.MoveType>();
            if (movetype == UnityMugen.MoveType.Unchanged || movetype == UnityMugen.MoveType.None) return null;

            ++parsestate.TokenIndex;

            parsestate.BaseNode.Arguments.Add(@operator);
            parsestate.BaseNode.Arguments.Add(movetype);
            return parsestate.BaseNode;
        }
    }


    [CustomFunction("MoveHit")]
    internal static class MoveHit
    {
        public static int Evaluate(Character character, ref bool error)
        {
            if (character == null)
            {
                error = true;
                return 0;
            }

            //return character.MoveType == UnityMugen.MoveType.Attack ? character.OffensiveInfo.MoveHit : 0;
            //character.MoveContact == MoveContact.
#warning em teste
            return character.OffensiveInfo.MoveHit; // Baseado no IK
        }

        public static Node Parse(ParseState parsestate)
        {
            return parsestate.BaseNode;
        }
    }

    [CustomFunction("Name")]
    internal static class Name
    {
        static bool KeepLog => LauncherEngine.Inst.initializationSettings.KeepLog;
        public static bool Evaluate(Character character, ref bool error, Operator @operator, string text)
        {
            if (character == null)
            {
                error = true;
                return false;
            }

            string name = null;
            if (character is Player)
            {
                var player = (Player)character;
                name = player.profile.charName;
            }
            else if (character is UnityMugen.Combat.Helper)
            {
                var helper = (UnityMugen.Combat.Helper)character;
                name = helper.Data.Name;
            }

            if (name == null)
            {
                if (KeepLog)
                    Debug.Log("Error | Name");

                error = true;
                return false;
            }

            switch (@operator)
            {
                case Operator.Equals:
                    return string.Equals(name, text, StringComparison.OrdinalIgnoreCase);

                case Operator.NotEquals:
                    return !string.Equals(name, text, StringComparison.OrdinalIgnoreCase);

                default:
                    if (KeepLog)
                        Debug.Log("Error | Name");

                    error = true;
                    return false;
            }
        }


        public static Node Parse(ParseState parsestate)
        {
            var @operator = parsestate.CurrentOperator;
            if (@operator != Operator.Equals && @operator != Operator.NotEquals) return null;
            ++parsestate.TokenIndex;

            var text = parsestate.CurrentText;
            if (text == null) return null;
            ++parsestate.TokenIndex;

            parsestate.BaseNode.Arguments.Add(@operator);
            parsestate.BaseNode.Arguments.Add(text);
            return parsestate.BaseNode;
        }
    }

    [CustomFunction("NumEnemy")]
    internal static class NumEnemy
    {
        public static int Evaluate(Character character, ref bool error)
        {
            if (character == null)
            {
                error = true;
                return 0;
            }

            var count = 0;
            Action<Player> func = player =>
            {
                if (player != null && character.FilterEntityAsCharacter(player, AffectTeam.Enemy) != null)
                    ++count;
            };

            func(character.Engine.Team1.MainPlayer);
            func(character.Engine.Team2.MainPlayer);
            func(character.Engine.Team1.TeamMate);
            func(character.Engine.Team2.TeamMate);

            return count;
        }

        public static Node Parse(ParseState parsestate)
        {
            return parsestate.BaseNode;
        }
    }

    [CustomFunction("NumExplod")]
    internal static class NumExplod
    {
        public static int Evaluate(Character character, ref bool error, int explodId)
        {
            if (character == null)
            {
                error = true;
                return 0;
            }

            var count = 0;

            foreach (var explod in character.GetExplods(explodId))
                ++count;

            return count;
        }

        public static Node Parse(ParseState parsestate)
        {
            var node = parsestate.BuildParenNumberNode(true);
            if (node != null)
            {
                return node;
            }

            parsestate.BaseNode.Children.Add(Node.NegativeOneNode);
            return parsestate.BaseNode;
        }
    }

    [CustomFunction("NumHelper")]
    internal static class NumHelper
    {
        public static int Evaluate(Character character, ref bool error, int helperId)
        {
            if (character == null)
            {
                error = true;
                return 0;
            }

            if (helperId >= 0)
            {
                List<UnityMugen.Combat.Helper> helpers;
                if (character.BasePlayer.Helpers.TryGetValue(helperId, out helpers))
                    return helpers.Count;

                return 0;
            }

            var count = 0;

            foreach (var data in character.BasePlayer.Helpers) count += data.Value.Count;

            return count;
        }

        public static Node Parse(ParseState parsestate)
        {
            var node = parsestate.BuildParenNumberNode(true);
            if (node != null)
            {
                return node;
            }

            parsestate.BaseNode.Children.Add(Node.NegativeOneNode);
            return parsestate.BaseNode;
        }
    }

    [CustomFunction("NumPartner")]
    internal static class NumPartner
    {
        public static int Evaluate(Character character, ref bool error)
        {
            if (character == null)
            {
                error = true;
                return 0;
            }

            var count = 0;
            foreach (var entity in character.Engine.Entities)
            {
                var partner = character.BasePlayer.FilterEntityAsPartner(entity);
                if (partner == null) continue;

                ++count;
            }

            return count;
        }

        public static Node Parse(ParseState parsestate)
        {
            return parsestate.BaseNode;
        }
    }

    [CustomFunction("NumProj")]
    internal static class NumProj
    {
        public static int Evaluate(Character character, ref bool error)
        {
            if (character == null)
            {
                error = true;
                return 0;
            }

            var count = 0;
            foreach (var entity in character.Engine.Entities)
            {
                var projectile = character.FilterEntityAsProjectile(entity, int.MinValue);
                if (projectile == null) continue;

                ++count;
            }

            return count;
        }

        public static Node Parse(ParseState parsestate)
        {
            return parsestate.BaseNode;
        }
    }

    [CustomFunction("NumProjID")]
    internal static class NumProjID
    {
        public static int Evaluate(Character character, ref bool error, int projectileId)
        {
            if (character == null)
            {
                error = true;
                return 0;
            }

            var count = 0;
            foreach (var entity in character.Engine.Entities)
            {
                var projectile = character.FilterEntityAsProjectile(entity, projectileId);
                if (projectile == null) continue;

                ++count;
            }

            return count;
        }

        public static Node Parse(ParseState parsestate)
        {
            return parsestate.BuildParenNumberNode(true);
        }
    }

    [CustomFunction("NumTarget")]
    internal static class NumTarget
    {
        public static int Evaluate(Character character, ref bool error, int targetId)
        {
            if (character == null)
            {
                error = true;
                return 0;
            }

            var count = 0;
            foreach (var target in character.GetTargets(targetId)) ++count;

            return count;
        }

        public static Node Parse(ParseState parsestate)
        {
            var node = parsestate.BuildParenNumberNode(true);
            if (node != null)
            {
                return node;
            }

            parsestate.BaseNode.Children.Add(Node.NegativeOneNode);
            return parsestate.BaseNode;
        }
    }

    [CustomFunction("NumGraphicUI")]
    internal static class NumGraphicUI
    {
        public static int Evaluate(Character character, ref bool error, int graphicUiID)
        {
            if (character == null)
            {
                error = true;
                return 0;
            }

            var count = 0;
            foreach (var graphicUI in character.GetGraphicUIs(graphicUiID))
                ++count;

            return count;
        }

        public static Node Parse(ParseState parsestate)
        {
            var node = parsestate.BuildParenNumberNode(true);
            if (node != null)
            {
                return node;
            }

            parsestate.BaseNode.Children.Add(Node.NegativeOneNode);
            return parsestate.BaseNode;
        }
    }

    [CustomFunction("P1Name")]
    internal static class P1Name
    {
        static bool KeepLog => LauncherEngine.Inst.initializationSettings.KeepLog;
        public static bool Evaluate(Character character, ref bool error, Operator @operator, string text)
        {
            if (character == null)
            {
                error = true;
                return false;
            }

            string name = null;
            if (character is Player)
            {
                var player = (Player)character;
                name = player.profile.charName;
            }

            if (character is UnityMugen.Combat.Helper)
            {
                var helper = (UnityMugen.Combat.Helper)character;
                name = helper.Data.Name;
            }
            if (name == null)
            {
                if (KeepLog)
                    Debug.Log("Error | P1Name");

                error = true;
                return false;
            }

            switch (@operator)
            {
                case Operator.Equals:
                    return string.Equals(name, text, StringComparison.OrdinalIgnoreCase);

                case Operator.NotEquals:
                    return !string.Equals(name, text, StringComparison.OrdinalIgnoreCase);

                default:
                    error = true;
                    return false;
            }
        }

        public static Node Parse(ParseState parsestate)
        {
            var @operator = parsestate.CurrentOperator;
            if (@operator != Operator.Equals && @operator != Operator.NotEquals) return null;
            ++parsestate.TokenIndex;

            var text = parsestate.CurrentText;
            if (text == null) return null;
            ++parsestate.TokenIndex;

            parsestate.BaseNode.Arguments.Add(@operator);
            parsestate.BaseNode.Arguments.Add(text);
            return parsestate.BaseNode;
        }
    }

    [CustomFunction("P2BodyDist")]
    internal static class P2BodyDist
    {
        static bool KeepLog => LauncherEngine.Inst.initializationSettings.KeepLog;
        public static float Evaluate(Character character, ref bool error, Axis axis)
        {
            if (character == null)
            {
                error = true;
                return 0;
            }

            var opponent = character.GetOpponent();
            if (opponent == null)
            {
                if (KeepLog)
                    Debug.Log("Error | P2BodyDist");

                error = true;
                return 0;
            }

            switch (axis)
            {
                case Axis.X:
                    var mylocation = character.GetFrontLocation();
                    var opplocation = opponent.GetFrontLocation();
                    var distance = Math.Abs(mylocation - opplocation);
                    if (character.CurrentFacing == UnityMugen.Facing.Right)
                    {
                        return (opponent.CurrentLocation.x >= character.CurrentLocation.x ? distance : -distance) * Constant.Scale2;
                    }
                    else
                    {
                        return (opponent.CurrentLocation.x >= character.CurrentLocation.x ? -distance : distance) * Constant.Scale2;
                    }

                case Axis.Y:
                    return (opponent.CurrentLocation.y - character.CurrentLocation.y) * Constant.Scale2;

                default:
                    if (KeepLog)
                        Debug.Log("Error | P2BodyDist");

                    error = true;
                    return 0;
            }
        }

        public static Node Parse(ParseState parsestate)
        {
            var axis = parsestate.ConvertCurrentToken<Axis>();
            if (axis == Axis.None) return null;

            ++parsestate.TokenIndex;

            parsestate.BaseNode.Arguments.Add(axis);
            return parsestate.BaseNode;
        }
    }

    [CustomFunction("P2Dist")]
    internal static class P2Dist
    {
        static bool KeepLog => LauncherEngine.Inst.initializationSettings.KeepLog;
        public static float Evaluate(Character character, ref bool error, Axis axis)
        {
            if (character == null)
            {
                error = true;
                return 0;
            }

            var opponent = character.GetOpponent();
            if (opponent == null)
            {
                if (KeepLog)
                    Debug.Log("Error | P2Dist");

                error = true;
                return 0;
            }

            switch (axis)
            {
                case Axis.X:
                    var distance = Math.Abs(character.CurrentLocation.x - opponent.CurrentLocation.x);
                    if (character.CurrentFacing == UnityMugen.Facing.Right)
                    {
                        return (opponent.CurrentLocation.x >= character.CurrentLocation.x ? distance : -distance) * Constant.Scale2;
                    }
                    else
                    {
                        return (opponent.CurrentLocation.x >= character.CurrentLocation.x ? -distance : distance) * Constant.Scale2;
                    }

                case Axis.Y:
                    return (opponent.CurrentLocation.y - character.CurrentLocation.y) * Constant.Scale2;

                default:
                    if (KeepLog)
                        Debug.Log("Error | P2Dist");

                    error = true;
                    return 0;
            }
        }

        public static Node Parse(ParseState parsestate)
        {
            var axis = parsestate.ConvertCurrentToken<Axis>();
            if (axis == Axis.None) return null;

            ++parsestate.TokenIndex;

            parsestate.BaseNode.Arguments.Add(axis);
            return parsestate.BaseNode;
        }
    }

    [CustomFunction("P2Life")]
    internal static class P2Life
    {
        static bool KeepLog => LauncherEngine.Inst.initializationSettings.KeepLog;
        public static float Evaluate(Character character, ref bool error)
        {
            if (character == null)
            {
                error = true;
                return 0;
            }

            var opponent = character.GetOpponent();
            if (opponent == null)
            {
                if (KeepLog)
                    Debug.Log("Error | P2Life");

                error = true;
                return 0;
            }

            return opponent.Life;
        }

        public static Node Parse(ParseState parsestate)
        {
            return parsestate.BaseNode;
        }
    }

    [CustomFunction("P2MoveType")]
    internal static class P2MoveType
    {
        static bool KeepLog => LauncherEngine.Inst.initializationSettings.KeepLog;
        public static bool Evaluate(Character character, ref bool error, Operator @operator, UnityMugen.MoveType movetype)
        {
            if (character == null)
            {
                if (KeepLog)
                    Debug.Log("Error | P2MoveType");

                error = true;
                return false;
            }

            var opponent = character.GetOpponent();
            if (opponent == null)
            {
                if (KeepLog)
                    Debug.Log("Error | P2MoveType");

                error = true;
                return false;
            }

            if (movetype == UnityMugen.MoveType.Unchanged || movetype == UnityMugen.MoveType.None)
            {
                if (KeepLog)
                    Debug.Log("Error | P2MoveType");

                error = true;
                return false;
            }

            switch (@operator)
            {
                case Operator.Equals:
                    return movetype == opponent.MoveType;

                case Operator.NotEquals:
                    return movetype != opponent.MoveType;

                default:
                    if (KeepLog)
                        Debug.Log("Error | P2MoveType");

                    error = true;
                    return false;
            }
        }

        public static Node Parse(ParseState parsestate)
        {
            var @operator = parsestate.CurrentOperator;
            if (@operator != Operator.Equals && @operator != Operator.NotEquals) return null;

            ++parsestate.TokenIndex;

            var movetype = parsestate.ConvertCurrentToken<UnityMugen.MoveType>();
            if (movetype == UnityMugen.MoveType.Unchanged || movetype == UnityMugen.MoveType.None) return null;

            ++parsestate.TokenIndex;

            parsestate.BaseNode.Arguments.Add(@operator);
            parsestate.BaseNode.Arguments.Add(movetype);
            return parsestate.BaseNode;
        }
    }

    [CustomFunction("P2Name")]
    internal static class P2Name
    {
        static bool KeepLog => LauncherEngine.Inst.initializationSettings.KeepLog;
        public static bool Evaluate(Character character, ref bool error, Operator @operator, string text)
        {
            if (character == null)
            {
                error = true;
                return false;
            }

            var p2 = character.Team.OtherTeam.MainPlayer;

            switch (@operator)
            {
                case Operator.Equals:
                    return p2 != null ? string.Equals(p2.profile.charName, text, StringComparison.OrdinalIgnoreCase) : false;

                case Operator.NotEquals:
                    return p2 != null ? !string.Equals(p2.profile.charName, text, StringComparison.OrdinalIgnoreCase) : true;

                default:
                    if (KeepLog)
                        Debug.Log("Error | P2Name");

                    error = true;
                    return false;
            }
        }

        public static Node Parse(ParseState parsestate)
        {
            var @operator = parsestate.CurrentOperator;
            if (@operator != Operator.Equals && @operator != Operator.NotEquals) return null;
            ++parsestate.TokenIndex;

            var text = parsestate.CurrentText;
            if (text == null) return null;
            ++parsestate.TokenIndex;

            parsestate.BaseNode.Arguments.Add(@operator);
            parsestate.BaseNode.Arguments.Add(text);
            return parsestate.BaseNode;
        }
    }

    [CustomFunction("P2StateNo")]
    internal static class P2StateNo
    {
        static bool KeepLog => LauncherEngine.Inst.initializationSettings.KeepLog;
        public static int Evaluate(Character character, ref bool error)
        {
            if (character == null)
            {
                error = true;
                return 0;
            }

            var opponent = character.GetOpponent();
            if (opponent == null)
            {
                if (KeepLog)
                    Debug.Log("Error | P2StateNo");

                error = true;
                return 0;
            }

            var currentstate = opponent.StateManager.CurrentState;
            return currentstate != null ? currentstate.number : 0;
        }

        public static Node Parse(ParseState parsestate)
        {
            return parsestate.BaseNode;
        }
    }

    [CustomFunction("P2StateType")]
    internal static class P2StateType
    {
        static bool KeepLog => LauncherEngine.Inst.initializationSettings.KeepLog;
        public static bool Evaluate(Character character, ref bool error, Operator @operator, UnityMugen.StateType statetype)
        {
            if (character == null)
            {
                if (KeepLog)
                    Debug.Log("Error | P2StateType");

                error = true;
                return false;
            }

            var opponent = character.GetOpponent();
            if (opponent == null)
            {
                if (KeepLog)
                    Debug.Log("Error | P2StateType");

                error = true;
                return false;
            }

            if (statetype == UnityMugen.StateType.Unchanged || statetype == UnityMugen.StateType.None)
            {
                if (KeepLog)
                    Debug.Log("Error | P2StateType");

                error = true;
                return false;
            }

            switch (@operator)
            {
                case Operator.Equals:
                    return statetype == opponent.StateType;

                case Operator.NotEquals:
                    return statetype != opponent.StateType;

                default:
                    if (KeepLog)
                        Debug.Log("Error | P2StateType");

                    error = true;
                    return false;
            }
        }

        public static Node Parse(ParseState parsestate)
        {
            var @operator = parsestate.CurrentOperator;
            if (@operator != Operator.Equals && @operator != Operator.NotEquals) return null;

            ++parsestate.TokenIndex;

            var statetype = parsestate.ConvertCurrentToken<UnityMugen.StateType>();
            if (statetype == UnityMugen.StateType.Unchanged || statetype == UnityMugen.StateType.None) return null;

            ++parsestate.TokenIndex;

            parsestate.BaseNode.Arguments.Add(@operator);
            parsestate.BaseNode.Arguments.Add(statetype);
            return parsestate.BaseNode;
        }
    }

    [CustomFunction("P3Name")]
    internal static class P3Name
    {
        static bool KeepLog => LauncherEngine.Inst.initializationSettings.KeepLog;
        public static bool Evaluate(Character character, ref bool error, Operator @operator, string text)
        {
            if (character == null)
            {
                error = true;
                return false;
            }

            var p3 = character.Team.TeamMate;

            switch (@operator)
            {
                case Operator.Equals:
                    return p3 != null ? string.Equals(p3.profile.charName, text, StringComparison.OrdinalIgnoreCase) : false;

                case Operator.NotEquals:
                    return p3 != null ? !string.Equals(p3.profile.charName, text, StringComparison.OrdinalIgnoreCase) : true;

                default:
                    if (KeepLog)
                        Debug.Log("Error | P3Name");

                    error = true;
                    return false;
            }
        }

        public static Node Parse(ParseState parsestate)
        {
            var @operator = parsestate.CurrentOperator;
            if (@operator != Operator.Equals && @operator != Operator.NotEquals) return null;
            ++parsestate.TokenIndex;

            var text = parsestate.CurrentText;
            if (text == null) return null;
            ++parsestate.TokenIndex;

            parsestate.BaseNode.Arguments.Add(@operator);
            parsestate.BaseNode.Arguments.Add(text);
            return parsestate.BaseNode;
        }
    }

    [CustomFunction("P4Name")]
    internal static class P4Name
    {
        static bool KeepLog => LauncherEngine.Inst.initializationSettings.KeepLog;
        public static bool Evaluate(Character character, ref bool error, Operator @operator, string text)
        {
            if (character == null)
            {
                error = true;
                return false;
            }

            var p4 = character.Team.OtherTeam.TeamMate;

            switch (@operator)
            {
                case Operator.Equals:
                    return p4 != null ? string.Equals(p4.profile.charName, text, StringComparison.OrdinalIgnoreCase) : false;

                case Operator.NotEquals:
                    return p4 != null ? !string.Equals(p4.profile.charName, text, StringComparison.OrdinalIgnoreCase) : true;

                default:
                    if (KeepLog)
                        Debug.Log("Error | P4Name");

                    error = true;
                    return false;
            }
        }

        public static Node Parse(ParseState parsestate)
        {
            var @operator = parsestate.CurrentOperator;
            if (@operator != Operator.Equals && @operator != Operator.NotEquals) return null;
            ++parsestate.TokenIndex;

            var text = parsestate.CurrentText;
            if (text == null) return null;
            ++parsestate.TokenIndex;

            parsestate.BaseNode.Arguments.Add(@operator);
            parsestate.BaseNode.Arguments.Add(text);
            return parsestate.BaseNode;
        }
    }

    [CustomFunction("PalNo")]
    internal static class PalNo
    {
        public static int Evaluate(Character character, ref bool error)
        {
            if (character == null)
            {
                error = true;
                return 0;
            }

            return character.BasePlayer.PaletteNumber;
        }

        public static Node Parse(ParseState parsestate)
        {
            return parsestate.BaseNode;
        }
    }

    [CustomFunction("ParentDist")]
    internal static class ParentDist
    {
        static bool KeepLog => LauncherEngine.Inst.initializationSettings.KeepLog;
        public static float Evaluate(Character character, ref bool error, Axis axis)
        {
            if (character == null)
            {
                error = true;
                return 0;
            }

            var helper = character as UnityMugen.Combat.Helper;
            if (helper == null)
            {
                if (KeepLog)
                    Debug.Log("Error | ParentDist");

                error = true;
                return 0;
            }

            switch (axis)
            {
                case Axis.X:
                    var distance = Math.Abs(helper.CurrentLocation.x - helper.Creator.CurrentLocation.x);
                    if (helper.CurrentFacing == UnityMugen.Facing.Right)
                    {
                        return helper.Creator.CurrentLocation.x >= helper.CurrentLocation.x ? distance : -distance;
                    }
                    else
                    {
                        return helper.Creator.CurrentLocation.x >= helper.CurrentLocation.x ? -distance : distance;
                    }

                case Axis.Y:
                    return helper.Creator.CurrentLocation.y - helper.CurrentLocation.y;

                default:
                    if (KeepLog)
                        Debug.Log("Error | ParentDist");

                    error = true;
                    return 0;
            }
        }

        public static Node Parse(ParseState parsestate)
        {
            var axis = parsestate.ConvertCurrentToken<Axis>();
            if (axis == Axis.None) return null;

            ++parsestate.TokenIndex;

            parsestate.BaseNode.Arguments.Add(axis);
            return parsestate.BaseNode;
        }
    }

    [CustomFunction("Physics")]
    internal static class Physics
    {
        static bool KeepLog => LauncherEngine.Inst.initializationSettings.KeepLog;
        public static bool Evaluate(Character character, ref bool error, Operator @operator, UnityMugen.Physic physics)
        {
            if (character == null)
            {
                if (KeepLog)
                    Debug.Log("Error | Physics");

                error = true;
                return false;
            }

            if (physics == UnityMugen.Physic.Unchanged || physics == UnityMugen.Physic.None)
            {
                if (KeepLog)
                    Debug.Log("Error | Physics");

                error = true;
                return false;
            }

            switch (@operator)
            {
                case Operator.Equals:
                    return physics == character.Physics;

                case Operator.NotEquals:
                    return physics != character.Physics;

                default:
                    if (KeepLog)
                        Debug.Log("Error | Physics");

                    error = true;
                    return false;
            }
        }

        public static Node Parse(ParseState parsestate)
        {
            var @operator = parsestate.CurrentOperator;
            if (@operator != Operator.Equals && @operator != Operator.NotEquals) return null;

            ++parsestate.TokenIndex;

            var physics = parsestate.ConvertCurrentToken<UnityMugen.Physic>();
            if (physics == UnityMugen.Physic.Unchanged || physics == UnityMugen.Physic.None) return null;

            ++parsestate.TokenIndex;

            parsestate.BaseNode.Arguments.Add(@operator);
            parsestate.BaseNode.Arguments.Add(physics);
            return parsestate.BaseNode;
        }
    }

    [CustomFunction("Pi")]
    internal static class Pi
    {
        public static float Evaluate(Character character, ref bool error)
        {
            return (float)Math.PI;
        }

        public static Node Parse(ParseState state)
        {
            return state.BaseNode;
        }
    }

    [CustomFunction("Pos")]
    internal static class Pos
    {
        static bool KeepLog => LauncherEngine.Inst.initializationSettings.KeepLog;
        public static float Evaluate(Character character, ref bool error, Axis axis)
        {
            if (character == null)
            {
                error = true;
                return 0;
            }

            switch (axis)
            {
                case Axis.X:
                    var screenrect = character.Engine.CameraFE.ScreenBounds();
                    //return (character.CurrentLocation.x - screenrect.xMin - screenrect.width / 2) * Constant.Scale2;
                    return (character.CurrentLocation.x - screenrect.center.x) * Constant.Scale2;
                case Axis.Y:
                    return character.CurrentLocation.y * Constant.Scale2;

                default:
                    if (KeepLog)
                        Debug.Log("Error | Pos");

                    error = true;
                    return 0;
            }
        }

        public static Node Parse(ParseState parsestate)
        {
            var axis = parsestate.ConvertCurrentToken<Axis>();
            if (axis == Axis.None) return null;

            ++parsestate.TokenIndex;

            parsestate.BaseNode.Arguments.Add(axis);
            return parsestate.BaseNode;
        }
    }

    [CustomFunction("Power")]
    internal static class Power
    {
        public static float Evaluate(Character character, ref bool error)
        {
            if (character == null)
            {
                error = true;
                return 0;
            }

            return character.BasePlayer.Power;
        }

        public static Node Parse(ParseState parsestate)
        {
            return parsestate.BaseNode;
        }
    }

    [CustomFunction("PowerMax")]
    internal static class PowerMax
    {
        public static float Evaluate(Character character, ref bool error)
        {
            if (character == null)
            {
                error = true;
                return 0;
            }

            return character.BasePlayer.playerConstants.MaximumPower;
        }

        public static Node Parse(ParseState parsestate)
        {
            return parsestate.BaseNode;
        }
    }

    [CustomFunction("PlayerIDExist")]
    internal static class PlayerIDExist
    {
        public static bool Evaluate(Character character, ref bool error, int id)
        {
            if (character == null)
            {
                error = true;
                return false;
            }

            foreach (var entity in character.Engine.Entities)
            {
                var c = entity as Character;
                if (c == null) continue;

                if (c.Id == id) return true;
            }

            return false;
        }

        public static Node Parse(ParseState parsestate)
        {
            return parsestate.BuildParenNumberNode(true);
        }
    }

    [CustomFunction("PrevStateNo")]
    internal static class PrevStateNo
    {
        public static int Evaluate(Character character, ref bool error)
        {
            if (character == null)
            {
                error = true;
                return 0;
            }

            var prevstate = character.StateManager.PreviousState;
            return prevstate != null ? prevstate.number : 0;
        }

        public static Node Parse(ParseState parsestate)
        {
            return parsestate.BaseNode;
        }
    }

    [CustomFunction("ProjCancelTime")]
    internal static class ProjCancelTime
    {
        public static int Evaluate(Character character, ref bool error, int projectileId)
        {
            if (character == null)
            {
                error = true;
                return 0;
            }

            var projinfo = character.OffensiveInfo.ProjectileInfo;
            if (projinfo.Type == ProjectileDataType.Cancel && (projectileId <= 0 || projectileId == projinfo.ProjectileId))
            {
                return projinfo.Time;
            }

            return -1;
        }

        public static Node Parse(ParseState parsestate)
        {
            return parsestate.BuildParenNumberNode(true);
        }
    }

    [CustomFunction("ProjContact")]
    internal static class ProjContact
    {
        public static bool Evaluate(Character character, ref bool error, int projId, int r2, int rhs, Operator compareType)
        {
            if (character == null)
            {
                error = true;
                return false;
            }

            var lookingfor = r2 > 0;

            var projinfo = character.OffensiveInfo.ProjectileInfo;

            var found = (projinfo.Type == ProjectileDataType.Hit || projinfo.Type == ProjectileDataType.Guarded) && (projId <= 0 || projId == projinfo.ProjectileId);
            if (found) found = SpecialFunctions.LogicalOperation(compareType, projinfo.Time, rhs);

            return lookingfor == found;
        }

        public static bool Evaluate(Character character, ref bool error, int projId, int r2, int pre, int post, Operator compareType, Symbol preCheck, Symbol postCheck)
        {
            if (character == null)
            {
                error = true;
                return false;
            }

            var lookingfor = r2 > 0;

            var projinfo = character.OffensiveInfo.ProjectileInfo;

            var found = (projinfo.Type == ProjectileDataType.Hit || projinfo.Type == ProjectileDataType.Guarded) && (projId <= 0 || projId == projinfo.ProjectileId);
            if (found) found = SpecialFunctions.Range(projinfo.Time, pre, post, compareType, preCheck, postCheck);

            return lookingfor == found;
        }

        public static Node Parse(ParseState parsestate)
        {
            var basenode = parsestate.BuildParenNumberNode(true);
            if (basenode == null)
            {
#warning Hack
                parsestate.BaseNode.Children.Add(Node.ZeroNode);
                basenode = parsestate.BaseNode;
            }

            if (parsestate.CurrentOperator != Operator.Equals) return null;
            ++parsestate.TokenIndex;

            var arg1 = parsestate.BuildNode(false);
            if (arg1 == null) return null;

            basenode.Children.Add(arg1);

            if (parsestate.CurrentSymbol != Symbol.Comma)
            {
#warning Hack
                basenode.Children.Add(Node.ZeroNode);
                basenode.Arguments.Add(Operator.Equals);

                return basenode;
            }

            ++parsestate.TokenIndex;

            var @operator = parsestate.CurrentOperator;
            if (@operator == Operator.Equals || @operator == Operator.NotEquals)
            {
                ++parsestate.TokenIndex;

                var rangenode = parsestate.BuildRangeNode();
                if (rangenode != null)
                {
                    basenode.Children.Add(rangenode.Children[1]);
                    basenode.Children.Add(rangenode.Children[2]);
                    basenode.Arguments.Add(@operator);
                    basenode.Arguments.Add(rangenode.Arguments[1]);
                    basenode.Arguments.Add(rangenode.Arguments[2]);

                    return basenode;
                }

                --parsestate.TokenIndex;
            }

            switch (@operator)
            {
                case Operator.Equals:
                case Operator.NotEquals:
                case Operator.GreaterEquals:
                case Operator.LesserEquals:
                case Operator.Lesser:
                case Operator.Greater:
                    ++parsestate.TokenIndex;
                    break;

                default:
                    return null;
            }

            var arg = parsestate.BuildNode(false);
            if (arg == null) return null;

            basenode.Arguments.Add(@operator);
            basenode.Children.Add(arg);

            return basenode;
        }
    }

    [CustomFunction("ProjContactTime")]
    internal static class ProjContactTime
    {
        public static int Evaluate(Character character, ref bool error, int projectileId)
        {
            if (character == null)
            {
                error = true;
                return 0;
            }

            var projinfo = character.OffensiveInfo.ProjectileInfo;
            if ((projinfo.Type == ProjectileDataType.Hit || projinfo.Type == ProjectileDataType.Guarded) && (projectileId <= 0 || projectileId == projinfo.ProjectileId))
            {
                return projinfo.Time;
            }

            return -1;
        }

        public static Node Parse(ParseState parsestate)
        {
            return parsestate.BuildParenNumberNode(true);
        }
    }

    [CustomFunction("ProjGuarded")]
    internal static class ProjGuarded
    {
        public static bool Evaluate(Character character, ref bool error, int projId, int r2, int rhs, Operator compareType)
        {
            if (character == null)
            {
                error = true;
                return false;
            }

            var lookingfor = r2 > 0;

            var projinfo = character.OffensiveInfo.ProjectileInfo;

            var found = projinfo.Type == ProjectileDataType.Guarded && (projId <= 0 || projId == projinfo.ProjectileId);
            if (found) found = SpecialFunctions.LogicalOperation(compareType, projinfo.Time, rhs);

            return lookingfor == found;
        }

        public static bool Evaluate(Character character, ref bool error, int projId, int r2, int pre, int post, Operator compareType, Symbol preCheck, Symbol postCheck)
        {
            if (character == null)
            {
                error = true;
                return false;
            }

            var lookingfor = r2 > 0;

            var projinfo = character.OffensiveInfo.ProjectileInfo;

            var found = projinfo.Type == ProjectileDataType.Guarded && (projId <= 0 || projId == projinfo.ProjectileId);
            if (found) found = SpecialFunctions.Range(projinfo.Time, pre, post, compareType, preCheck, postCheck);

            return lookingfor == found;
        }

        public static Node Parse(ParseState parsestate)
        {
            var basenode = parsestate.BuildParenNumberNode(true);
            if (basenode == null)
            {
#warning Hack
                parsestate.BaseNode.Children.Add(Node.ZeroNode);
                basenode = parsestate.BaseNode;
            }

            if (parsestate.CurrentOperator != Operator.Equals) return null;
            ++parsestate.TokenIndex;

            var arg1 = parsestate.BuildNode(false);
            if (arg1 == null) return null;

            basenode.Children.Add(arg1);

            if (parsestate.CurrentSymbol != Symbol.Comma)
            {
#warning Hack
                basenode.Children.Add(Node.ZeroNode);
                basenode.Arguments.Add(Operator.Equals);

                return basenode;
            }

            ++parsestate.TokenIndex;

            var @operator = parsestate.CurrentOperator;
            if (@operator == Operator.Equals || @operator == Operator.NotEquals)
            {
                ++parsestate.TokenIndex;

                var rangenode = parsestate.BuildRangeNode();
                if (rangenode != null)
                {
                    basenode.Children.Add(rangenode.Children[1]);
                    basenode.Children.Add(rangenode.Children[2]);
                    basenode.Arguments.Add(@operator);
                    basenode.Arguments.Add(rangenode.Arguments[1]);
                    basenode.Arguments.Add(rangenode.Arguments[2]);

                    return basenode;
                }

                --parsestate.TokenIndex;
            }

            switch (@operator)
            {
                case Operator.Equals:
                case Operator.NotEquals:
                case Operator.GreaterEquals:
                case Operator.LesserEquals:
                case Operator.Lesser:
                case Operator.Greater:
                    ++parsestate.TokenIndex;
                    break;

                default:
                    return null;
            }

            var arg = parsestate.BuildNode(false);
            if (arg == null) return null;

            basenode.Arguments.Add(@operator);
            basenode.Children.Add(arg);

            return basenode;
        }
    }

    [CustomFunction("ProjGuardedTime")]
    internal static class ProjGuardedTime
    {
        public static int Evaluate(Character character, ref bool error, int projectileId)
        {
            if (character == null)
            {
                error = true;
                return 0;
            }

            var projinfo = character.OffensiveInfo.ProjectileInfo;
            if (projinfo.Type == ProjectileDataType.Guarded && (projectileId <= 0 || projectileId == projinfo.ProjectileId))
            {
                return projinfo.Time;
            }

            return -1;
        }

        public static Node Parse(ParseState parsestate)
        {
            return parsestate.BuildParenNumberNode(true);
        }
    }

    [CustomFunction("ProjHit")]
    internal static class ProjHit
    {
        public static bool Evaluate(Character character, ref bool error, int projId, int r2, int rhs, Operator compareType)
        {
            if (character == null)
            {
                error = true;
                return false;
            }

            var lookingfor = r2 > 0;

            var projinfo = character.OffensiveInfo.ProjectileInfo;

            var found = projinfo.Type == ProjectileDataType.Hit && (projId <= 0 || projId == projinfo.ProjectileId);
            if (found) found = SpecialFunctions.LogicalOperation(compareType, projinfo.Time, rhs);

            return lookingfor == found;
        }

        public static bool Evaluate(Character character, ref bool error, int projId, int r2, int pre, int post, Operator compareType, Symbol preCheck, Symbol postCheck)
        {
            if (character == null)
            {
                error = true;
                return false;
            }

            var lookingfor = r2 > 0;

            var projinfo = character.OffensiveInfo.ProjectileInfo;

            var found = projinfo.Type == ProjectileDataType.Hit && (projId <= 0 || projId == projinfo.ProjectileId);
            if (found) found = SpecialFunctions.Range(projinfo.Time, pre, post, compareType, preCheck, postCheck);

            return lookingfor == found;
        }

        public static Node Parse(ParseState parsestate)
        {
            var basenode = parsestate.BuildParenNumberNode(true);
            if (basenode == null)
            {
#warning Hack
                parsestate.BaseNode.Children.Add(Node.ZeroNode);
                basenode = parsestate.BaseNode;
            }

            if (parsestate.CurrentOperator != Operator.Equals) return null;
            ++parsestate.TokenIndex;

            var arg1 = parsestate.BuildNode(false);
            if (arg1 == null) return null;

            basenode.Children.Add(arg1);

            if (parsestate.CurrentSymbol != Symbol.Comma)
            {
#warning Hack
                basenode.Children.Add(Node.ZeroNode);
                basenode.Arguments.Add(Operator.Equals);

                return basenode;
            }

            ++parsestate.TokenIndex;

            var @operator = parsestate.CurrentOperator;
            if (@operator == Operator.Equals || @operator == Operator.NotEquals)
            {
                ++parsestate.TokenIndex;

                var rangenode = parsestate.BuildRangeNode();
                if (rangenode != null)
                {
                    basenode.Children.Add(rangenode.Children[1]);
                    basenode.Children.Add(rangenode.Children[2]);
                    basenode.Arguments.Add(@operator);
                    basenode.Arguments.Add(rangenode.Arguments[1]);
                    basenode.Arguments.Add(rangenode.Arguments[2]);

                    return basenode;
                }

                --parsestate.TokenIndex;
            }

            switch (@operator)
            {
                case Operator.Equals:
                case Operator.NotEquals:
                case Operator.GreaterEquals:
                case Operator.LesserEquals:
                case Operator.Lesser:
                case Operator.Greater:
                    ++parsestate.TokenIndex;
                    break;

                default:
                    return null;
            }

            var arg = parsestate.BuildNode(false);
            if (arg == null) return null;

            basenode.Arguments.Add(@operator);
            basenode.Children.Add(arg);

            return basenode;
        }
    }

    [CustomFunction("ProjHitTime")]
    internal static class ProjHitTime
    {
        public static int Evaluate(Character character, ref bool error, int projectileId)
        {
            if (character == null)
            {
                error = true;
                return 0;
            }

            var projinfo = character.OffensiveInfo.ProjectileInfo;
            if (projinfo.Type == ProjectileDataType.Hit && (projectileId <= 0 || projectileId == projinfo.ProjectileId))
            {
                return projinfo.Time;
            }

            return -1;
        }

        public static Node Parse(ParseState parsestate)
        {
            return parsestate.BuildParenNumberNode(true);
        }
    }

    [CustomFunction("Random")]
    internal static class RandomT
    {
        public static int Evaluate(Character character, ref bool error)
        {
            if (character == null)
            {
                error = true;
                return 0;
            }
#warning analizar posteriormente game online
            var rng = LauncherEngine.Inst.random;
            return rng.NewInt(0, 999);
        }

        public static Node Parse(ParseState parsestate)
        {
            return parsestate.BaseNode;
        }
    }

    [CustomFunction("RootDist")]
    internal static class RootDist
    {
        static bool KeepLog => LauncherEngine.Inst.initializationSettings.KeepLog;
        public static float Evaluate(Character character, ref bool error, Axis axis)
        {
            if (character == null)
            {
                error = true;
                return 0;
            }

            var helper = character as UnityMugen.Combat.Helper;
            if (helper == null)
            {
                if (KeepLog)
                    Debug.Log("Error | RootDist");

                error = true;
                return 0;
            }

            switch (axis)
            {
                case Axis.X:
                    var distance = Math.Abs(helper.CurrentLocation.x - helper.BasePlayer.CurrentLocation.x);
                    if (helper.CurrentFacing == UnityMugen.Facing.Right)
                    {
                        float valueX = helper.BasePlayer.CurrentLocation.x >= helper.CurrentLocation.x ? distance : -distance;
                        return valueX * Constant.Scale2;
                    }
                    else
                    {
                        float valueX = helper.BasePlayer.CurrentLocation.x >= helper.CurrentLocation.x ? -distance : distance;
                        return valueX * Constant.Scale2;
                    }

                case Axis.Y:
                    float valueY = helper.BasePlayer.CurrentLocation.y - helper.CurrentLocation.y;
                    return valueY * Constant.Scale2;

                default:
                    if (KeepLog)
                        Debug.Log("Error | RootDist");

                    error = true;
                    return 0;
            }
        }

        public static Node Parse(ParseState parsestate)
        {
            var axis = parsestate.ConvertCurrentToken<Axis>();
            if (axis == Axis.None) return null;

            ++parsestate.TokenIndex;

            parsestate.BaseNode.Arguments.Add(axis);
            return parsestate.BaseNode;
        }
    }

    [CustomFunction("RoundNo")]
    internal static class RoundNo
    {
        public static int Evaluate(Character character, ref bool error)
        {
            if (character == null)
            {
                error = true;
                return 0;
            }

            return character.Engine.RoundNumber;
        }

        public static Node Parse(ParseState parsestate)
        {
            return parsestate.BaseNode;
        }
    }

    [CustomFunction("RoundsExisted")]
    internal static class RoundsExisted
    {
        public static int Evaluate(Character character, ref bool error)
        {
            if (character == null)
            {
                error = true;
                return 0;
            }

            return character.RoundsExisted;
        }

        public static Node Parse(ParseState parsestate)
        {
            return parsestate.BaseNode;
        }
    }

    [CustomFunction("RoundState")]
    internal static class RoundStateT
    {
        public static int Evaluate(Character character, ref bool error)
        {
            if (character == null)
            {
                error = true;
                return 0;
            }

            switch (character.Engine.RoundState)
            {
                case UnityMugen.RoundState.PreIntro:
                    return 0;

                case UnityMugen.RoundState.Intro:
                    return 1;

                case UnityMugen.RoundState.Fight:
                    return 2;

                case UnityMugen.RoundState.PreOver:
                    return 3;

                case UnityMugen.RoundState.Over:
                    return 4;

                default:
                    return -1;
            }
        }

        public static Node Parse(ParseState parsestate)
        {
            return parsestate.BaseNode;
        }
    }

    [CustomFunction("ScreenPos")]
    internal static class ScreenPos
    {
        static bool KeepLog => LauncherEngine.Inst.initializationSettings.KeepLog;
        public static float Evaluate(Character character, ref bool error, Axis axis)
        {
            if (character == null)
            {
                error = true;
                return 0;
            }

            Vector2 invertYChar = character.GetDrawLocationYTransform();
            var drawlocation = invertYChar - (Vector2)character.Engine.CameraFE.Location;

            //var drawlocation = character.GetDrawLocation() - (Vector2)character.Engine.CameraFE.Location; Original
            //analizar melhor
            drawlocation = Camera.main.WorldToScreenPoint(character.transform.position);
            //AudioImporter não deveria ser de screenpointo para word?
            //((720 / 2) + 30) - screenPos.y

            drawlocation.y = Screen.height - drawlocation.y;
            switch (axis)
            {
                case Axis.X:
                    return drawlocation.x /** Constant.Scale2*/;

                case Axis.Y:
                    return drawlocation.y /** Constant.Scale2*/;

                default:
                    if (KeepLog)
                        Debug.Log("Error | ScreenPos");

                    error = true;
                    return 0;
            }
        }

        public static Node Parse(ParseState parsestate)
        {
            var axis = parsestate.ConvertCurrentToken<Axis>();
            if (axis == Axis.None) return null;

            ++parsestate.TokenIndex;

            parsestate.BaseNode.Arguments.Add(axis);
            return parsestate.BaseNode;
        }
    }

    [CustomFunction("SelfAnimExist")]
    internal static class SelfAnimExist
    {
        public static bool Evaluate(Character character, ref bool error, int animationnumber)
        {
            if (character == null)
            {
                error = true;
                return false;
            }

            return character.AnimationManager.HasAnimation(animationnumber);
        }

        public static Node Parse(ParseState parsestate)
        {
            return parsestate.BuildParenNumberNode(true);
        }
    }

    [CustomFunction("Sin")]
    internal static class Sin
    {
        public static float Evaluate(Character character, ref bool error, float value)
        {
            return (float)Math.Sin(value);
        }

        public static Node Parse(ParseState state)
        {
            return state.BuildParenNumberNode(true);
        }
    }

    [CustomFunction("StateNo")]
    internal static class StateNo
    {
        static bool KeepLog => LauncherEngine.Inst.initializationSettings.KeepLog;
        public static int Evaluate(Character character, ref bool error)
        {
            if (character == null)
            {
                if (KeepLog)
                    Debug.Log("Error | StateNo");

                error = true;
                return 0;
            }

            var currentstate = character.StateManager.CurrentState;
            return currentstate != null ? currentstate.number : 0;
        }

        public static Node Parse(ParseState parsestate)
        {
            return parsestate.BaseNode;
        }
    }

    [CustomFunction("StateType")]
    internal static class StateTypeT
    {
        static bool KeepLog => LauncherEngine.Inst.initializationSettings.KeepLog;
        public static bool Evaluate(Character character, ref bool error, Operator @operator, UnityMugen.StateType statetype)
        {
            if (character == null)
            {
                if (KeepLog)
                    Debug.Log("Error | StateType");

                error = true;
                return false;
            }

            if (statetype == UnityMugen.StateType.Unchanged || statetype == UnityMugen.StateType.None)
            {
                if (KeepLog)
                    Debug.Log("Error | StateType");

                error = true;
                return false;
            }

            switch (@operator)
            {
                case Operator.Equals:
                    return statetype == character.StateType;

                case Operator.NotEquals:
                    return statetype != character.StateType;

                default:
                    if (KeepLog)
                        Debug.Log("Error | StateType");

                    error = true;
                    return false;
            }
        }

        public static Node Parse(ParseState parsestate)
        {
            var @operator = parsestate.CurrentOperator;
            if (@operator != Operator.Equals && @operator != Operator.NotEquals) return null;

            ++parsestate.TokenIndex;

            var statetype = parsestate.ConvertCurrentToken<UnityMugen.StateType>();
            if (statetype == UnityMugen.StateType.Unchanged || statetype == UnityMugen.StateType.None) return null;

            ++parsestate.TokenIndex;

            parsestate.BaseNode.Arguments.Add(@operator);
            parsestate.BaseNode.Arguments.Add(statetype);
            return parsestate.BaseNode;
        }
    }


    [CustomFunction("StageVar")]
    internal static class StageVar
    {
        public static bool Evaluate(Character character, ref bool error, Operator @operator, string text)
        {
            if (character == null)
            {
                error = true;
                return false;
            }

            StageProfile prof = LauncherEngine.Inst.profileLoader.stageProfiles[character.Engine.stageScreen.Stage.stageID];
            Stage stage = character.Engine.stageScreen.Stage;
            string compare = "";
            /*    switch (typeStageVar)
                {
                    case TypeStageVar.InfoName:
                        compare = prof.Name; break;
                    case TypeStageVar.InfoDisplayname:
                        compare = prof.DisplayName; break;
                    case TypeStageVar.InfoAuthor:
                        compare = prof.Author; break;
                        //
                        case TypeStageVar.CameraBoundLeft:
                            compare = stage.boundLeft; break;
                        case TypeStageVar.CameraBoundRight:
                            compare = stage.boundRight; break;
                        case TypeStageVar.CameraBoundHigh:
                            compare = stage.boundHigh; break;
                        case TypeStageVar.CameraBoundLow:
                            compare = stage.boundLow; break;
                        case TypeStageVar.CameraVerticalFollow:
                            compare = stage.VerticalFollow; break;
                        case TypeStageVar.CameraFloorTension:
                            compare = stage.FloorTension; break;
                        case TypeStageVar.CameraTensionHigh:
                            compare = stage.TensionHigh; break;
                        case TypeStageVar.CameraTensionLow:
                            compare = stage.TensionLow; break;
                        case TypeStageVar.CameraTension:
                            compare = stage.Tension; break;
                        case TypeStageVar.CameraStartZoom:
                            compare = stage.StartZoom; break;
                        case TypeStageVar.CameraZoomOut:
                            compare = stage.ZoomOut; break;
                        case TypeStageVar.CameraZooMin:
                            compare = stage.ZooMin; break;
                        case TypeStageVar.CameraUTensionEnable:
                            compare = stage.UTensionEnable; break;
                        case TypeStageVar.PlayerInfoLeftBound:
                            compare = stage.; break;
                        case TypeStageVar.PlayerInfoRightbound:
                            compare = stage.; break;
                        case TypeStageVar.ScalingTopScale:
                            compare = stage.; break;
                        case TypeStageVar.BoundScreenLeft:
                            compare = stage.; break;
                        case TypeStageVar.BoundScreenRight:
                            compare = stage.; break;
                        case TypeStageVar.StageInfoZoffSet:
                            compare = stage.; break;
                        case TypeStageVar.StageInfoZoffSetLink:
                            compare = stage.; break;
                        case TypeStageVar.StageInfoXScale:
                            compare = stage.; break;
                        case TypeStageVar.StageInfoYScale:
                            compare = stage.; break;
                        case TypeStageVar.ShadowIntensity:
                            compare = stage.; break;
                        case TypeStageVar.ShadowColorR:
                            compare = stage.; break;
                        case TypeStageVar.ShadowColorG:
                            compare = stage.; break;
                        case TypeStageVar.ShadowColorB:
                            compare = stage.; break;
                        case TypeStageVar.ShadowYScale:
                            compare = stage.; break;
                        case TypeStageVar.ShadowFadeRangeBegin:
                            compare = stage.; break;
                        case TypeStageVar.ShadowFadeRangeEnd:
                            compare = stage.; break;
                        //case TypeStageVar.ShadowXshear:
                        //    compare = stage.; break;
                        case TypeStageVar.ReflectionIntensity:
                            compare = stage.; break;
                        //
                }

                var sc = StringComparer.OrdinalIgnoreCase;
                switch (@operator)
                {
                    case Operator.Equals:
                        return sc.Equals(compare, value);

                    case Operator.NotEquals:
                        return !sc.Equals(compare, value);

                    default:
                        if (KeepLog)
                            Debug.Log("Error | StageVar");
                        return false;
                }*/

            return false;
        }

        public static Node Parse(ParseState parsestate)
        {
            var @operator = parsestate.CurrentOperator;
            if (@operator != Operator.Equals && @operator != Operator.NotEquals) return null;
            ++parsestate.TokenIndex;

            var text = parsestate.CurrentUnknown;
            if (text == null) return null;
            ++parsestate.TokenIndex;

            parsestate.BaseNode.Arguments.Add(@operator);
            parsestate.BaseNode.Arguments.Add(text);
            return parsestate.BaseNode;
        }
    }



    [CustomFunction("SysFVar")]
    internal static class SysFVar
    {
        static bool KeepLog => LauncherEngine.Inst.initializationSettings.KeepLog;
        public static float Evaluate(Character character, ref bool error, int value)
        {
            if (character == null)
            {
                error = true;
                return 0;
            }

            float result;
            if (character.Variables.GetFloat(value, true, out result)) return result;

            if (KeepLog)
                Debug.Log("Error | SysFVar");

            error = true;
            return 0;
        }

        public static Node Parse(ParseState parsestate)
        {
            return parsestate.BuildParenNumberNode(true);
        }
    }

    [CustomFunction("SysVar")]
    internal static class SysVar
    {
        static bool KeepLog => LauncherEngine.Inst.initializationSettings.KeepLog;
        public static int Evaluate(Character character, ref bool error, int value)
        {
            if (character == null)
            {
                error = true;
                return 0;
            }

            int result;
            if (character.Variables.GetInteger(value, true, out result)) return result;

            if (KeepLog)
                Debug.Log("Error | SysFVar");

            error = true;
            return 0;
        }

        public static Node Parse(ParseState parsestate)
        {
            return parsestate.BuildParenNumberNode(true);
        }
    }

    [CustomFunction("Tan")]
    internal static class Tan
    {
        public static float Evaluate(Character character, ref bool error, float value)
        {
            return (float)Math.Tan(value);
        }

        public static Node Parse(ParseState state)
        {
            return state.BuildParenNumberNode(true);
        }
    }

    [CustomFunction("TeamMode")]
    internal static class TeamModeT
    {
        static bool KeepLog => LauncherEngine.Inst.initializationSettings.KeepLog;
        public static bool Evaluate(Character character, ref bool error, Operator @operator, string text)
        {
            if (character == null)
            {
                error = true;
                return false;
            }

            var mode = character.Team.Mode;
            bool result;
            switch (mode)
            {
                case UnityMugen.TeamMode.Simul:
                    result = string.Equals(text, "simul", StringComparison.OrdinalIgnoreCase);
                    break;
                case UnityMugen.TeamMode.Single:
                    result = string.Equals(text, "single", StringComparison.OrdinalIgnoreCase);
                    break;
                case UnityMugen.TeamMode.Turns:
                    result = string.Equals(text, "turns", StringComparison.OrdinalIgnoreCase);
                    break;
                default:
                    if (KeepLog)
                        Debug.Log("Error | TeamMode");

                    error = true;
                    return false;
            }

            switch (@operator)
            {
                case Operator.Equals:
                    return result;

                case Operator.NotEquals:
                    return !result;

                default:
                    if (KeepLog)
                        Debug.Log("Error | TeamMode");

                    error = true;
                    return false;
            }
        }

        public static Node Parse(ParseState parsestate)
        {
            var @operator = parsestate.CurrentOperator;
            if (@operator != Operator.Equals && @operator != Operator.NotEquals) return null;
            ++parsestate.TokenIndex;

            var text = parsestate.CurrentUnknown;
            if (text == null) return null;
            ++parsestate.TokenIndex;

            parsestate.BaseNode.Arguments.Add(@operator);
            parsestate.BaseNode.Arguments.Add(text);
            return parsestate.BaseNode;
        }
    }

    [CustomFunction("TeamSide")]
    internal static class TeamSideT
    {
        static bool KeepLog => LauncherEngine.Inst.initializationSettings.KeepLog;
        public static int Evaluate(Character character, ref bool error)
        {
            if (character == null)
            {
                error = true;
                return 0;
            }

            switch (character.Team.Side)
            {
                case UnityMugen.TeamSide.Left:
                    return 1;

                case UnityMugen.TeamSide.Right:
                    return 2;

                default:
                    if (KeepLog)
                        Debug.Log("Error | TeamSide");

                    error = true;
                    return 0;
            }
        }

        public static Node Parse(ParseState parsestate)
        {
            return parsestate.BaseNode;
        }
    }

    [CustomFunction("TicksPerSecond")]
    internal static class TicksPerSecond
    {
        public static int Evaluate(Character character, ref bool error)
        {
            if (character == null)
            {
                error = true;
                return 0;
            }

            return LauncherEngine.Inst.initializationSettings.GameSpeed;
        }

        public static Node Parse(ParseState parsestate)
        {
            return parsestate.BaseNode;
        }
    }

    [CustomFunction("Time")]
    internal static class TimeT
    {
        public static int Evaluate(Character character, ref bool error)
        {
            if (character == null)
            {
                error = true;
                return 0;
            }

            var time = character.StateManager.StateTime;
            if (time < 0) time = 0;

            return time;
        }

        public static Node Parse(ParseState parsestate)
        {
            return parsestate.BaseNode;
        }
    }

    [CustomFunction("StateTime")]
    internal static class StateTime
    {
#warning isso nao é usado mais acho que futuramente vou apagar o Time(Character character) para usar esse
        public static int Evaluate(Character character, ref bool error)
        {
            if (character == null)
            {
                error = true;
                return 0;
            }

            var time = character.StateManager.StateTime;
            if (time < 0) time = 0;

            return time;
        }

        public static Node Parse(ParseState parsestate)
        {
            return parsestate.BaseNode;
        }
    }

    [CustomFunction("TimeMod")]
    internal static class TimeMod
    {
        public static bool Evaluate(Character character, ref bool error, int r1, int r2, Operator compareType)
        {
            if (character == null)
            {
                error = true;
                return false;
            }

            var statetimeRemander = character.StateManager.StateTime % r1;
            //return statetimeRemander == r2; // Modificado Tiago

            bool result = SpecialFunctions.LogicalOperation(compareType, statetimeRemander, r2);// Modificado Tiago
            return result;// Modificado Tiago
        }

        public static Node Parse(ParseState parsestate)
        {
            var @operator = parsestate.CurrentOperator;
            switch (@operator)
            {
                case Operator.Equals:
                case Operator.NotEquals:
                case Operator.Greater:
                case Operator.GreaterEquals:
                case Operator.Lesser:
                case Operator.LesserEquals:
                    ++parsestate.TokenIndex;
                    break;

                default:
                    return null;
            }

            var child1 = parsestate.BuildNode(true);
            if (child1 == null) return null;

            if (parsestate.CurrentSymbol != Symbol.Comma) return null;
            ++parsestate.TokenIndex;

            var child2 = parsestate.BuildNode(true);
            if (child2 == null) return null;

            parsestate.BaseNode.Children.Add(child1);
            parsestate.BaseNode.Children.Add(child2);

            var basenode = parsestate.BuildParenNumberNode(true);
            if (basenode == null)
            {
                basenode = parsestate.BaseNode;
            }
            basenode.Arguments.Add(@operator);

            return parsestate.BaseNode;
        }
    }

    [CustomFunction("UniqHitCount")]
    internal static class UniqHitCount
    {
        public static int Evaluate(Character character, ref bool error)
        {
            if (character == null)
            {
                error = true;
                return 0;
            }

            return character.OffensiveInfo.UniqueHitCount;
        }

        public static Node Parse(ParseState parsestate)
        {
            return parsestate.BaseNode;
        }
    }

    [CustomFunction("Var")]
    internal static class Var
    {
        static bool KeepLog => LauncherEngine.Inst.initializationSettings.KeepLog;
        public static int Evaluate(Character character, ref bool error, int value)
        {
            if (character == null)
            {
                error = true;
                return 0;
            }

            if (character.Variables.GetInteger(value, false, out int result))
                return result;

            if (KeepLog)
                Debug.Log("Error | Var");

            error = true;
            return 0;
        }

        public static Node Parse(ParseState parsestate)
        {
            return parsestate.BuildParenNumberNode(true);
        }
    }

    ////Set Var -- New Tiago -- isso é igual a [var(ixdex) := valor]
    //public static int SetVar(this Character character, int index, int value)
    //{
    //    if (character == null)
    //    {
    //        return 0;
    //    }

    //    character.Variables.SetInteger(index, false, value);
    //    return value;
    //}

    ////Set Var -- New Tiago -- isso é igual a [var(ixdex) := valor]
    //public static float SetVar(this Character character, int index, float value)
    //{
    //    if (character == null)
    //    {
    //        return 0;
    //    }

    //    character.Variables.SetFloat(index, false, value);
    //    return value;
    //}

    [CustomFunction("Vel")]
    internal static class Vel
    {
        static bool KeepLog => LauncherEngine.Inst.initializationSettings.KeepLog;
        public static float Evaluate(Character character, ref bool error, Axis axis)
        {
            if (character == null)
            {
                error = true;
                return 0;
            }

            switch (axis)
            {
                case Axis.X:
                    return character.CurrentVelocity.x * Constant.Scale2;

                case Axis.Y:
                    return character.CurrentVelocity.y * Constant.Scale2;

                default:
                    if (KeepLog)
                        Debug.Log("Error | Vel");

                    error = true;
                    return 0;
            }
        }

        public static Node Parse(ParseState parsestate)
        {
            var axis = parsestate.ConvertCurrentToken<Axis>();
            if (axis == Axis.None) return null;

            ++parsestate.TokenIndex;

            parsestate.BaseNode.Arguments.Add(axis);
            return parsestate.BaseNode;
        }
    }

    [CustomFunction("Win")]
    internal static class Win
    {
        public static bool Evaluate(Character character, ref bool error)
        {
            if (character == null)
            {
                error = true;
                return false;
            }

            return character.Team.VictoryStatus.Win;
        }

        public static Node Parse(ParseState parsestate)
        {
            return parsestate.BaseNode;
        }
    }

    [CustomFunction("WinKO")]
    internal static class WinKO
    {
        public static bool Evaluate(Character character, ref bool error)
        {
            if (character == null)
            {
                error = true;
                return false;
            }

            return character.Team.VictoryStatus.WinKO;
        }

        public static Node Parse(ParseState parsestate)
        {
            return parsestate.BaseNode;
        }
    }

    [CustomFunction("WinTime")]
    internal static class WinTime
    {
        public static bool Evaluate(Character character, ref bool error)
        {
            if (character == null)
            {
                error = true;
                return false;
            }

            return character.Team.VictoryStatus.WinTime;
        }

        public static Node Parse(ParseState parsestate)
        {
            return parsestate.BaseNode;
        }
    }

    [CustomFunction("WinPerfect")]
    internal static class WinPerfect
    {
        public static bool Evaluate(Character character, ref bool error)
        {
            if (character == null)
            {
                error = true;
                return false;
            }

            return character.Team.VictoryStatus.WinPerfect;
        }

        public static Node Parse(ParseState parsestate)
        {
            return parsestate.BaseNode;
        }
    }



#warning falta testar
    [CustomFunction("ScreenHeight")]
    internal static class ScreenHeight
    {
        public static float Evaluate(Character character, ref bool error)
        {
            if (character == null)
            {
                error = true;
                return 0;
            }

            return character.Engine.CameraFE.ConverterBound().height * Constant.Scale2;
        }

        public static Node Parse(ParseState state)
        {
            return state.BaseNode;
        }
    }

#warning falta testar
    [CustomFunction("ScreenWidth")]
    internal static class ScreenWidth
    {
        public static float Evaluate(Character character, ref bool error)
        {
            if (character == null)
            {
                error = true;
                return 0;
            }

            return character.Engine.CameraFE.ConverterBound().width * Constant.Scale2;
        }

        public static Node Parse(ParseState state)
        {
            return state.BaseNode;
        }
    }

    [CustomFunction("GameHeight")]
    internal static class GameHeight
    {
        public static float Evaluate(Character character, ref bool error)
        {
            if (character == null)
            {
                error = true;
                return 0;
            }
            //return character.Engine.CameraFE.CameraBounds.up - character.Engine.CameraFE.CameraBounds.down;
            //return Screen.height;
            return character.Engine.CameraFE.ConverterBound().height * Constant.Scale2;
        }

        public static Node Parse(ParseState state)
        {
            return state.BaseNode;
        }
    }

    [CustomFunction("GameWidth")]
    internal static class GameWidth
    {
        public static float Evaluate(Character character, ref bool error)
        {
            if (character == null)
            {
                error = true;
                return 0;
            }
            //return character.Engine.CameraFE.CameraBounds.right - character.Engine.CameraFE.CameraBounds.left;
            //return Screen.width;
            return character.Engine.CameraFE.ConverterBound().width * Constant.Scale2;
        }

        public static Node Parse(ParseState state)
        {
            return state.BaseNode;
        }
    }

    [CustomFunction("TopEdge")]
    internal static class TopEdge
    {
        public static float Evaluate(Character character, ref bool error)
        {
            if (character == null)
            {
                error = true;
                return 0;
            }

#warning não tenho certeza sobre usar yMax, analizar
            // Nao preciso mecher com scale pois Pos e ScreenPos já possuem seus valores alterados pos Constant.Scale2
            // return TE.Pos(character, Axis.Y) - character.ScreenPos(Axis.Y);
            return character.Engine.CameraFE.ConverterBound().yMin * Constant.Scale2;
        }

        public static Node Parse(ParseState state)
        {
            return state.BaseNode;
        }
    }

    [CustomFunction("BottomEdge")]
    internal static class BottomEdge
    {
        public static float Evaluate(Character character, ref bool error)
        {
            if (character == null)
            {
                error = true;
                return 0;
            }

            // Nao preciso mecher com scale pois Pos e ScreenPos já possuem seus valores alterados pos Constant.Scale2
            //return TE.Pos(character, Axis.Y) - TE.ScreenPos(character, Axis.Y) + TE.GameHeight();
#warning não tenho certeza sobre usar yMax, analizar
            return character.Engine.CameraFE.ConverterBound().yMax * Constant.Scale2;
        }

        public static Node Parse(ParseState state)
        {
            return state.BaseNode;
        }
    }

    [CustomFunction("RightEdge")]
    internal static class RightEdge
    {
        public static float Evaluate(Character character, ref bool error)
        {
            if (character == null)
            {
                error = true;
                return 0;
            }

#warning não tenho certeza sobre usar yMin, analizar
            return character.Engine.CameraFE.ConverterBound().xMax * Constant.Scale2;
        }

        public static Node Parse(ParseState state)
        {
            return state.BaseNode;
        }
    }

    [CustomFunction("LeftEdge")]
    internal static class LeftEdge
    {
        public static float Evaluate(Character character, ref bool error)
        {
            if (character == null)
            {
                error = true;
                return 0;
            }

            return character.Engine.CameraFE.ConverterBound().xMin * Constant.Scale2;
        }

        public static Node Parse(ParseState state)
        {
            return state.BaseNode;
        }
    }

    [CustomFunction("FrontEdge")]
    internal static class FrontEdge
    {
        public static float Evaluate(Character character, ref bool error)
        {
            if (character == null)
            {
                error = true;
                return 0;
            }

            if (character.CurrentFacing == Facing.Left)
                return character.Engine.CameraFE.ConverterBound().xMin * Constant.Scale2;
            else
                return character.Engine.CameraFE.ConverterBound().xMax * Constant.Scale2;
        }

        public static Node Parse(ParseState state)
        {
            return state.BaseNode;
        }
    }

    [CustomFunction("BackEdge")]
    internal static class BackEdge
    {
        public static float Evaluate(Character character, ref bool error)
        {
            if (character == null)
            {
                error = true;
                return 0;
            }
            //return character.CurrentFacing == UnityMugen.Facing.Right ? TE.LeftEdge() : TE.RightEdge();
            if (character.CurrentFacing == Facing.Left)
                return character.Engine.CameraFE.ConverterBound().xMax * Constant.Scale2;
            else
                return character.Engine.CameraFE.ConverterBound().xMin * Constant.Scale2;
        }

        public static Node Parse(ParseState state)
        {
            return state.BaseNode;
        }
    }

    [CustomFunction("WinHyper")]
    internal static class WinHyper
    {
        public static bool Evaluate(Character character, ref bool error)
        {
            if (character == null)
            {
                error = true;
                return false;
            }

            return character.Team.VictoryStatus.WinHyper;
        }

        public static Node Parse(ParseState parsestate)
        {
            return parsestate.BaseNode;
        }
    }

    [CustomFunction("WinSpecial")]
    internal static class WinSpecial
    {
        public static bool Evaluate(Character character, ref bool error)
        {
            if (character == null)
            {
                error = true;
                return false;
            }

            return character.Team.VictoryStatus.WinSpecial;
        }

        public static Node Parse(ParseState parsestate)
        {
            return parsestate.BaseNode;
        }
    }

    [CustomFunction("ComboCounter")]
    internal static class ComboCounterT
    {
        public static int Evaluate(Character character, ref bool error)
        {
            if (character == null)
            {
                error = true;
                return 0;
            }

            return character.Team.ComboCounter.HitCount;
        }

        public static Node Parse(ParseState state)
        {
            return state.BaseNode;
        }
    }

    [CustomFunction("ReceivedDamage")]
    internal static class ReceivedDamage
    {
        public static bool Evaluate(Character character, ref bool error)
        {
            if (character == null)
            {
                error = true;
                return false;
            }

            foreach (Contact contact in character.Engine.m_combatcheck.Attacks)
            {
                return character.Engine.m_combatcheck.ReceivedDamage(character);
                //if (contact.Type == ContactType.Hit) return true;
            }
            return false;
        }

        public static Node Parse(ParseState state)
        {
            return state.BaseNode;
        }
    }







    [CustomFunction("CombatMode")]
    internal static class CombatMode
    {
        static bool KeepLog => LauncherEngine.Inst.initializationSettings.KeepLog;
        public static bool Evaluate(Character character, ref bool error, Operator @operator, string text)
        {
            if (character == null)
            {
                error = true;
                return false;
            }

            var mode = character.Team.Mode;
            bool result;
            switch (LauncherEngine.Inst.engineInitialization.Mode)
            {
                case UnityMugen.CombatMode.Training:
                    result = string.Equals(text, "training", StringComparison.OrdinalIgnoreCase);
                    break;
                
                default:
                    if (KeepLog)
                        Debug.Log("Error | CombatMode");

                    error = true;
                    return false;
            }

            switch (@operator)
            {
                case Operator.Equals:
                    return result;

                case Operator.NotEquals:
                    return !result;

                default:
                    if (KeepLog)
                        Debug.Log("Error | CombatMode");

                    error = true;
                    return false;
            }
        }

        public static Node Parse(ParseState parsestate)
        {
            var @operator = parsestate.CurrentOperator;
            if (@operator != Operator.Equals && @operator != Operator.NotEquals) return null;
            ++parsestate.TokenIndex;

            var text = parsestate.CurrentUnknown;
            if (text == null) return null;
            ++parsestate.TokenIndex;

            parsestate.BaseNode.Arguments.Add(@operator);
            parsestate.BaseNode.Arguments.Add(text);
            return parsestate.BaseNode;
        }
    }

    [CustomFunction("StanceType")]
    internal static class StanceType
    {
        static bool KeepLog => LauncherEngine.Inst.initializationSettings.KeepLog;
        public static bool Evaluate(Character character, ref bool error, Operator @operator, string text)
        {
            if (character == null)
            {
                error = true;
                return false;
            }

            var mode = character.Team.Mode;
            bool result;
            switch (LauncherEngine.Inst.trainnerSettings.stanceType)
            {
                case UnityMugen.StanceType.Standing:
                    result = string.Equals(text, "Standing", StringComparison.OrdinalIgnoreCase);
                    break;
                case UnityMugen.StanceType.Crouching:
                    result = string.Equals(text, "Crouching", StringComparison.OrdinalIgnoreCase);
                    break;
                case UnityMugen.StanceType.Jumping:
                    result = string.Equals(text, "Jumping", StringComparison.OrdinalIgnoreCase);
                    break;
                case UnityMugen.StanceType.COM:
                    result = string.Equals(text, "COM", StringComparison.OrdinalIgnoreCase);
                    break;
                case UnityMugen.StanceType.Controller:
                    result = string.Equals(text, "Controller", StringComparison.OrdinalIgnoreCase);
                    break;

                default:
                    if (KeepLog)
                        Debug.Log("Error | StanceType");

                    error = true;
                    return false;
            }

            switch (@operator)
            {
                case Operator.Equals:
                    return result;

                case Operator.NotEquals:
                    return !result;

                default:
                    if (KeepLog)
                        Debug.Log("Error | StanceType");

                    error = true;
                    return false;
            }
        }

        public static Node Parse(ParseState parsestate)
        {
            var @operator = parsestate.CurrentOperator;
            if (@operator != Operator.Equals && @operator != Operator.NotEquals) return null;
            ++parsestate.TokenIndex;

            var text = parsestate.CurrentUnknown;
            if (text == null) return null;
            ++parsestate.TokenIndex;

            parsestate.BaseNode.Arguments.Add(@operator);
            parsestate.BaseNode.Arguments.Add(text);
            return parsestate.BaseNode;
        }
    }


    // Só IK TEM ISSO, COISA NOVA
    // Talves nem valha apena aplica isso
    // moveCountered
    // selfStatenoExist
    // stageFrontEdge
    // stageBackEdge
    // teamLeader
}