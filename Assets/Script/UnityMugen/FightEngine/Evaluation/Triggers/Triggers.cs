using System;
using System.Collections.Generic;
using UnityEngine;
using UnityMugen.Combat;

namespace UnityMugen.Evaluation.Triggers
{

    [CustomFunction("Const240p")]
    class Const240p : Function
    {
        public Const240p(List<IFunction> children, List<System.Object> arguments)
            : base(children, arguments)
        {
        }
        public override Number Evaluate(Character state)
        {
            if (Children.Count != 1) return new Number();

            Number number = Children[0].Evaluate(state);

            if (number.NumberType == NumberType.Int) return new Number(number.IntValue);
            if (number.NumberType == NumberType.Float) return new Number(number.FloatValue);

            return new Number();
        }
        public static Node Parse(ParseState state)
        {
            return state.BuildParenNumberNode(true);
        }
    }

    [CustomFunction("Const480p")]
    class Const480p : Function
    {
        public Const480p(List<IFunction> children, List<System.Object> arguments)
            : base(children, arguments)
        {
        }
        public override Number Evaluate(Character state)
        {
            if (Children.Count != 1) return new Number();

            Number number = Children[0].Evaluate(state);

            if (number.NumberType == NumberType.Int) return new Number(number.IntValue);
            if (number.NumberType == NumberType.Float) return new Number(number.FloatValue);

            return new Number();
        }
        public static Node Parse(ParseState state)
        {
            return state.BuildParenNumberNode(true);
        }
    }

    [CustomFunction("Const720p")]
    class Const720p : Function
    {
        public Const720p(List<IFunction> children, List<System.Object> arguments)
            : base(children, arguments)
        {
        }
        public override Number Evaluate(Character state)
        {
            if (Children.Count != 1) return new Number();

            Number number = Children[0].Evaluate(state);

            if (number.NumberType == NumberType.Int) return new Number(number.IntValue);
            if (number.NumberType == NumberType.Float) return new Number(number.FloatValue);

            return new Number();
        }
        public static Node Parse(ParseState state)
        {
            return state.BuildParenNumberNode(true);
        }
    }

    [CustomFunction("Abs")]
    class Abs : Function
    {
        public Abs(List<IFunction> children, List<System.Object> arguments)
               : base(children, arguments)
        {
        }
        public override Number Evaluate(Character state)
        {
            if (Children.Count != 1) return new Number();

            Number number = Children[0].Evaluate(state);

            if (number.NumberType == NumberType.Int) return new Number(Math.Abs(number.IntValue));
            if (number.NumberType == NumberType.Float) return new Number(Math.Abs(number.FloatValue));

            return new Number();
        }

        public static Node Parse(ParseState state)
        {
            return state.BuildParenNumberNode(true);
        }
    }

    [CustomFunction("Acos")]
    class Acos : Function
    {
        public Acos(List<IFunction> children, List<System.Object> arguments)
            : base(children, arguments) { }
        public override Number Evaluate(Character state)
        {
            if (Children.Count != 1) return new Number();

            Number number = Children[0].Evaluate(state);
            if (number.NumberType == NumberType.None) return new Number();

            if (number.FloatValue < -1 || number.FloatValue > 1)
            {
                if (KeepLog)
                    Debug.Log("Error | Acos");

                return new Number();
            }

            return new Number(Math.Acos(number.FloatValue));
        }

        public static Node Parse(ParseState state)
        {
            return state.BuildParenNumberNode(true);
        }
    }

    [CustomFunction("AiLevel")]
    class AiLevel : Function
    {
        public AiLevel(List<IFunction> children, List<System.Object> arguments)
               : base(children, arguments) { }
        public override Number Evaluate(Character character)
        {
            if (character == null)
            {
                if (KeepLog)
                    Debug.Log("Error | Acos");

                return new Number();
            }
            if (character.PlayerMode == PlayerMode.Human)
                return new Number(0);

            if (LauncherEngine.Inst.engineInitialization.Mode == UnityMugen.CombatMode.Training &&
                LauncherEngine.Inst.trainnerSettings.stanceType == UnityMugen.StanceType.COM &&
                character.PlayerMode == PlayerMode.Ai)
                return new Number(LauncherEngine.Inst.trainnerSettings.COMLevel);

            return new Number(LauncherEngine.Inst.initializationSettings.AiLevel);
        }

        public static Node Parse(ParseState state)
        {
            return state.BaseNode;
        }
    }

    [CustomFunction("Alive")]
    class Alive : Function
    {
        public Alive(List<IFunction> children, List<System.Object> arguments)
            : base(children, arguments)
        {
        }
        public override Number Evaluate(Character character)
        {
            if (character == null)
            {
                if (KeepLog)
                    Debug.Log("Error | Acos");

                return new Number();
            }

            return new Number(character.Life > 0);
        }

        public static Node Parse(ParseState state)
        {
            return state.BaseNode;
        }
    }

    [CustomFunction("Anim")]
    class Anim : Function
    {
        public Anim(List<IFunction> children, List<System.Object> arguments)
            : base(children, arguments)
        {
        }
        public override Number Evaluate(Character character)
        {
            if (character == null || character.AnimationManager.CurrentAnimation == null)
            {
                if (KeepLog)
                    Debug.Log("Error | Acos");

                return new Number();
            }

            return new Number(character.AnimationManager.CurrentAnimation.Number);
        }

        public static Node Parse(ParseState state)
        {
            return state.BaseNode;
        }
    }

    [CustomFunction("AnimElem")]
    class AnimElem : Function
    {
        
        public AnimElem(List<IFunction> children, List<System.Object> arguments)
            : base(children, arguments)
        {
        }
        public override Number Evaluate(Character character)
        {
            if (character == null) return new Number();

            if (Children.Count == 2 && Arguments.Count == 1) 
            {

                Number r1 = Children[0].Evaluate(character);
                Number rhs = Children[1].Evaluate(character);
                Operator compareType = (Operator)Arguments[0];

                var animation = character.AnimationManager.CurrentAnimation;
                if (animation == null)
                {
                    if (KeepLog)
                        Debug.Log("Error | AnimElem");
                    return new Number();
                }

                var elementindex = r1.IntValue - 1;
                if (elementindex < 0 || elementindex >= animation.Elements.Count)
                {
                    return new Number(false);
                }

                var elementstarttime = animation.GetElementStartTime(elementindex);
                var animationtime = character.AnimationManager.TimeInAnimation;
                while (animation.TotalTime != -1 && animationtime >= animation.TotalTime)
                {
                    var looptime = animation.TotalTime - animation.GetElementStartTime(animation.Loopstart);
                    animationtime -= looptime;
                }

                var timeoffset = animationtime - elementstarttime;

                if (character.AnimationManager.IsAnimationFinished) return new Number(false);

                var result = SpecialFunctions.LogicalOperation(compareType, timeoffset, rhs.IntValue);
                if (character.InHitPause)
                    return new Number(result);
                else
                    return new Number(compareType == Operator.Equals ? result && character.UpdatedAnimation : result);
            }
            else if(Children.Count == 3 && Arguments.Count == 3)
            {
                if (KeepLog)
                    Debug.Log("Error | AnimElem - Implementar o Evaluat Abaixo");

                Debug.LogError("AnimElem : Function");
                return new Number();
            }
            else
            {
                Debug.LogError("AnimElem : Function");
                return new Number();
            }
        }
        
        /*
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
        */
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

            var ope = state.CurrentOperator;
            if (ope == Operator.Equals || ope == Operator.NotEquals)
            {
                ++state.TokenIndex;

                var rangenode = state.BuildRangeNode();
                if (rangenode != null)
                {
                    state.BaseNode.Children.Add(rangenode.Children[1]);
                    state.BaseNode.Children.Add(rangenode.Children[2]);
                    state.BaseNode.Arguments.Add(ope);
                    state.BaseNode.Arguments.Add(rangenode.Arguments[1]);
                    state.BaseNode.Arguments.Add(rangenode.Arguments[2]);

                    return state.BaseNode;
                }

                --state.TokenIndex;
            }

            switch (ope)
            {
                case Operator.Equals:
                case Operator.NotEquals:
                case Operator.GreaterEquals:
                case Operator.LesserEquals:
                case Operator.Lesser:
                case Operator.Greater:
                    ++state.TokenIndex;
                    break;

                case Operator.None:
#warning em analize - Tiago Solution Problem - AnimElem = 6, 1
                    if (state.TokenIndex <= state.TokenCount)
                    {
                        var arg2 = state.BuildNode(false);
                        if (arg2.Token.Data is NumberData)
                        {
                            ope = Operator.Equals;
                            --state.TokenIndex;
                            break;
                        }
                        return null;
                    }
                    else return null;

                default:
                    return null;
            }

            var arg = state.BuildNode(false);
            if (arg == null) return null;

            state.BaseNode.Arguments.Add(ope);
            state.BaseNode.Children.Add(arg);

            return state.BaseNode;
        }
    }

    [CustomFunction("AnimElemNo")]
    class AnimElemNo : Function
    {
        public AnimElemNo(List<IFunction> children, List<System.Object> arguments)
            : base(children, arguments)
        {
        }
        public override Number Evaluate(Character character)
        {
            if (character == null || Children.Count != 1) return new Number();

            Number timeoffset = Children[0].Evaluate(character);
            if (timeoffset.NumberType == NumberType.None) return new Number();

            var animation = character.AnimationManager.CurrentAnimation;
            if (animation == null)
            {
                if (KeepLog)
                    Debug.Log("Error | AnimElemNo");

                return new Number();
            }

            var animtime = character.AnimationManager.TimeInAnimation;

            var checktime = animtime + timeoffset.IntValue;
            if (checktime < 0)
            {
                if (KeepLog)
                    Debug.Log("Error | AnimElemNo");

                return new Number();
            }

            var elemIndex = animation.GetElementFromTime(checktime).Id;
            return new Number(elemIndex + 1);
        }

        public static Node Parse(ParseState state)
        {
            return state.BuildParenNumberNode(true);
        }
    }

    [CustomFunction("AnimElemTime")]
    class AnimElemTime : Function
    {
        public AnimElemTime(List<IFunction> children, List<System.Object> arguments)
            : base(children, arguments)
        {
        }
        public override Number Evaluate(Character character)
        {
            if (character == null || Children.Count != 1)
            {
                if (KeepLog)
                    Debug.Log("Error | AnimElemTime");

                return new Number();
            }

            Number value = Children[0].Evaluate(character);
            if (value.NumberType == NumberType.None) return new Number();

            var animation = character.AnimationManager.CurrentAnimation;
            if (animation == null)
            {
                if (KeepLog)
                    Debug.Log("Error | AnimElemTime");

                return new Number();
            }

            var elementIndex = value.IntValue - 1;
            if (animation.Elements.Count <= elementIndex) return new Number(0);

            var animationTime = character.AnimationManager.TimeInAnimation;
            var timeLoop = character.AnimationManager.TimeLoop;
            var elementStarttime = animation.GetElementStartTime(elementIndex);

            var result = animationTime - elementStarttime;
            if (animation.TotalTime != -1 && animationTime > animation.TotalTime && !(value.IntValue == 1 && timeLoop == 0))
            {
                var firstTickLoopElement = animation.GetElementStartTime(animation.Loopstart);
                //if (elementIndex != animation.Loopstart && timeLoop != firstTickLoopElement)
                if (!(elementIndex == animation.Loopstart && timeLoop == firstTickLoopElement))
                    result = timeLoop - elementStarttime;
            }
            return new Number(result);
        }

        public static Node Parse(ParseState state)
        {
            return state.BuildParenNumberNode(true);
        }
    }

    [CustomFunction("AnimExist")]
    class AnimExist : Function
    {
        public AnimExist(List<IFunction> children, List<System.Object> arguments)
            : base(children, arguments)
        {
        }
        public override Number Evaluate(Character character)
        {
            if (character == null || Children.Count != 1) return new Number();

            if (character.AnimationManager.IsForeignAnimation)
            {
                if (KeepLog)
                    Debug.Log("Error | AnimElemTime");

                return new Number();
            }

            Number value = Children[0].Evaluate(character);
            if (value.NumberType == NumberType.None) return new Number();

            return new Number(character.AnimationManager.HasAnimation(value.IntValue));
        }

        public static Node Parse(ParseState state)
        {
            return state.BuildParenNumberNode(true);
        }
    }

    [CustomFunction("AnimTime")]
    class AnimTime : Function
    {
        public AnimTime(List<IFunction> children, List<System.Object> arguments)
            : base(children, arguments)
        {
        }
        public override Number Evaluate(Character character)
        {
            if (character == null) return new Number();

            var animation = character.AnimationManager.CurrentAnimation;
            if (animation == null)
            {
                if (KeepLog)
                    Debug.Log("Error | AnimTime");

                return new Number();
            }

            var animtime = character.AnimationManager.TimeLoop;

            if (animation.TotalTime == -1)
            {
                return new Number(animtime + 1);
            }

            return new Number(animtime - animation.TotalTime);
        }

        public static Node Parse(ParseState parsestate)
        {
            return parsestate.BaseNode;
        }
    }

    [CustomFunction("Asin")]
    class Asin : Function
    {
        public Asin(List<IFunction> children, List<System.Object> arguments)
            : base(children, arguments)
        {
        }
        public override Number Evaluate(Character character)
        {
            if (Children.Count != 1) return new Number();

            Number value = Children[0].Evaluate(character);
            if (value.NumberType == NumberType.None) return new Number();

            if (value.FloatValue < -1 || value.FloatValue > 1)
            {
                if (KeepLog)
                    Debug.Log("Error | Asin");

                return new Number();
            }
#warning analizar este retorno
            return new Number(Math.Asin(value.FloatValue));//(float)Math.Asin(value);
        }

        public static Node Parse(ParseState state)
        {
            return state.BuildParenNumberNode(true);
        }
    }

    [CustomFunction("Assertion")]
    class Assertion : Function
    {
        public Assertion(List<IFunction> children, List<System.Object> arguments)
            : base(children, arguments)
        {
        }

        public override Number Evaluate(Character character)
        {
            if (character == null || Arguments.Count != 1) return new Number();

            switch ((UnityMugen.Assertion)Arguments[0])
            {
                case UnityMugen.Assertion.Intro:
                    return new Number(character.Engine.Assertions.Intro);

                case UnityMugen.Assertion.Invisible:
                    return new Number(character.Assertions.Invisible);

                case UnityMugen.Assertion.RoundNotOver:
                    return new Number(character.Engine.Assertions.WinPose);

                case UnityMugen.Assertion.NoBarDisplay:
                    return new Number(character.Engine.Assertions.NoBarDisplay);

                case UnityMugen.Assertion.NoBackground:
                    return new Number(character.Engine.Assertions.NoBackLayer);

                case UnityMugen.Assertion.NoForeground:
                    return new Number(character.Engine.Assertions.NoFrontLayer);

                case UnityMugen.Assertion.NoStandGuard:
                    return new Number(character.Assertions.NoStandingGuard);

                case UnityMugen.Assertion.NoAirGuard:
                    return new Number(character.Assertions.NoAirGuard);

                case UnityMugen.Assertion.NoCrouchGuard:
                    return new Number(character.Assertions.NoCrouchingGuard);

                case UnityMugen.Assertion.NoAutoturn:
                    return new Number(character.Assertions.NoAutoTurn);

                case UnityMugen.Assertion.NoJuggleCheck:
                    return new Number(character.Assertions.NoJuggleCheck);

                case UnityMugen.Assertion.NoKOSound:
                    return new Number(character.Engine.Assertions.NoKOSound);

                case UnityMugen.Assertion.NoKOSlow:
                    return new Number(character.Engine.Assertions.NoKOSlow);

                case UnityMugen.Assertion.NoShadow:
                    return new Number(character.Assertions.NoShadow);

                case UnityMugen.Assertion.GlobalNoShadow:
                    return new Number(character.Engine.Assertions.GlobalNoShadow);

                case UnityMugen.Assertion.NoMusic:
                    return new Number(character.Engine.Assertions.NoMusic);

                case UnityMugen.Assertion.NoWalk:
                    return new Number(character.Assertions.NoWalk);

                case UnityMugen.Assertion.TimerFreeze:
                    return new Number(character.Engine.Assertions.TimerFreeze);

                case UnityMugen.Assertion.Unguardable:
                    return new Number(character.Assertions.UnGuardable);

                case UnityMugen.Assertion.None:
                default:
                    return new Number(0);
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
    class Atan : Function
    {
        public Atan(List<IFunction> children, List<System.Object> arguments)
               : base(children, arguments)
        {
        }
        public override Number Evaluate(Character character)
        {
            if (Children.Count != 1) return new Number();

            Number value = Children[0].Evaluate(character);
            if (value.NumberType == NumberType.None) return new Number();

            return new Number(Math.Atan(value.FloatValue));
        }

        public static Node Parse(ParseState state)
        {
            return state.BuildParenNumberNode(true);
        }
    }

    [CustomFunction("AuthorName")]
    class AuthorName : Function
    {
        public AuthorName(List<IFunction> children, List<System.Object> arguments)
            : base(children, arguments)
        {
        }
        public override Number Evaluate(Character character)
        {
            if (character == null || Arguments.Count != 2) return new Number();

            Operator @operator = (Operator)Arguments[0];
            string text = (string)Arguments[1];

            var authorname = character.BasePlayer.profile.author;
            if (authorname == null)
            {
                if (KeepLog)
                    Debug.Log("Error | AuthorName");
                return new Number();
            }

            Number result = new Number(string.Equals(authorname, text, StringComparison.OrdinalIgnoreCase));

            switch (@operator)
            {
                case Operator.Equals:
                    return result;

                case Operator.NotEquals:
                    return !result;

                default:
                    if (KeepLog)
                        Debug.Log("Error | AuthorName");

                    return new Number();
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
    class BackEdgeBodyDist : Function
    {
        public BackEdgeBodyDist(List<IFunction> children, List<System.Object> arguments)
            : base(children, arguments)
        {
        }
        public override Number Evaluate(Character character)
        {
            if (character == null) return new Number();

            var camerarect = character.Engine.CameraFE.ScreenBounds();

            switch (character.CurrentFacing)
            {
                case UnityMugen.Facing.Left:
                    return new Number((camerarect.xMax - character.GetRightEdgePosition(true)) * Constant.Scale2);

                case UnityMugen.Facing.Right:
                    return new Number((character.GetLeftEdgePosition(true) - camerarect.xMin) * Constant.Scale2);

                default:
                    if (KeepLog)
                        Debug.Log("Error | BackEdgeBodyDist");

                    return new Number();
            }
        }

        public static Node Parse(ParseState state)
        {
            return state.BaseNode;
        }
    }

    [CustomFunction("BackEdgeDist")]
    class BackEdgeDist : Function
    {
        public BackEdgeDist(List<IFunction> children, List<System.Object> arguments)
            : base(children, arguments)
        {
        }
        public override Number Evaluate(Character character)
        {
            if (character == null) return new Number();

            var camerarect = character.Engine.CameraFE.ScreenBounds();

            switch (character.CurrentFacing)
            {
                case UnityMugen.Facing.Left:
                    return new Number((camerarect.xMax - character.GetRightEdgePosition(false)) * Constant.Scale2);

                case UnityMugen.Facing.Right:
                    return new Number((character.GetLeftEdgePosition(false) - camerarect.xMin) * Constant.Scale2);//Atenção aqui

                default:
                    if (KeepLog)
                        Debug.Log("Error | BackEdgeDist");

                    return new Number();
            }
        }

        public static Node Parse(ParseState state)
        {
            return state.BaseNode;
        }
    }

#warning esta faltando
    [CustomFunction("CameraPos")]
    class CameraPos : Function
    {
        public CameraPos(List<IFunction> children, List<System.Object> arguments)
               : base(children, arguments)
        {
        }
        public override Number Evaluate(Character character)
        {
            if (character == null || Arguments.Count != 1) return new Number();

            return new Number(0);

            Axis axis = (Axis)Arguments[0];
            switch (axis)
            {
                case Axis.X:
                    return new Number(character.Engine.CameraFE.transform.position.x * Constant.Scale2);

                case Axis.Y:
                    return new Number(character.Engine.CameraFE.transform.position.y * Constant.Scale2);

                default:
                    return new Number();
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


#warning Em teste
    [CustomFunction("CameraZoom")]
    class CameraZoom : Function
    {
        public CameraZoom(List<IFunction> children, List<System.Object> arguments)
               : base(children, arguments)
        {
        }
        public override Number Evaluate(Character character)
        {
            if (character == null) return new Number();

            return new Number(character.Engine.CameraFE.ScaleCamera);
        }

        public static Node Parse(ParseState state)
        {
            return state.BaseNode;
        }
    }

    [CustomFunction("CanRecover")]
    class CanRecover : Function
    {
        public CanRecover(List<IFunction> children, List<System.Object> arguments)
               : base(children, arguments)
        {
        }
        public override Number Evaluate(Character character)
        {
            if (character == null) return new Number();

            return new Number(character.DefensiveInfo.IsFalling == false ?
                false :
                character.DefensiveInfo.HitDef.FallCanRecover);
        }

        public static Node Parse(ParseState state)
        {
            return state.BaseNode;
        }
    }

    [CustomFunction("Ceil")]
    class Ceil : Function
    {
        public Ceil(List<IFunction> children, List<System.Object> arguments)
            : base(children, arguments)
        {
        }
        public override Number Evaluate(Character character)
        {
            if (Children.Count != 1) return new Number();

            Number number = Children[0].Evaluate(character);

            switch (number.NumberType)
            {
                case NumberType.Int:
                    return new Number(number.IntValue);

                case NumberType.Float:
                    return new Number(/*Math.Ceiling(*/number.FloatValue/*)*/);

                default:
                    return new Number();
            }
        }

        public static Node Parse(ParseState state)
        {
            return state.BuildParenNumberNode(true);
        }
    }

    [CustomFunction("Command")]
    class Command : Function
    {
        public Command(List<IFunction> children, List<System.Object> arguments)
            : base(children, arguments)
        {
        }

        public override Number Evaluate(Character character)
        {
            if (character == null || Arguments.Count != 2) return new Number();

            Operator @operator = (Operator)Arguments[0];
            string text = (string)Arguments[1];

            var active = character.CommandManager.IsActive(text);
            switch (@operator)
            {
                case Operator.Equals:
                    return new Number(active);

                case Operator.NotEquals:
                    return new Number(!active);

                default:
                    if (KeepLog)
                        Debug.Log("Error | Command");

                    return new Number();
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
    class Cond : Function
    {
        public Cond(List<IFunction> children, List<System.Object> arguments)
            : base(children, arguments)
        {
        }
        public override Number Evaluate(Character character)
        {
            if (character == null || Children.Count != 3) return new Number();

            Number r1 = Children[0].Evaluate(character);
            Number r2 = Children[1].Evaluate(character);
            Number r3 = Children[2].Evaluate(character);

            if (r1.NumberType == NumberType.None || r2.NumberType == NumberType.None || r3.NumberType == NumberType.None) return new Number();

            if (r2.NumberType == NumberType.Float || r3.NumberType == NumberType.Float)
            {
                if (r1.FloatValue < 0)
                    return r1;

                return new Number(r1.FloatValue != 0 ? r2.FloatValue : r3.FloatValue);
            }
            else if (r2.NumberType == NumberType.Int && r3.NumberType == NumberType.Int)
            {
                if (r1.IntValue < 0)
                    return r1;

                return new Number(r1.IntValue != 0 ? r2.IntValue : r3.IntValue);
            }
            else
            {
                return new Number();
            }
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
    class Cos : Function
    {
        public Cos(List<IFunction> children, List<System.Object> arguments)
            : base(children, arguments)
        {
        }
        public override Number Evaluate(Character character)
        {
            if (Children.Count != 1) return new Number();

            Number number = Children[0].Evaluate(character);

            if (number.NumberType == NumberType.Int) return new Number((Single)Math.Cos(number.IntValue));
            if (number.NumberType == NumberType.Float) return new Number((Single)Math.Cos(number.FloatValue));

            return new Number();
        }

        public static Node Parse(ParseState state)
        {
            return state.BuildParenNumberNode(true);
        }
    }

    [CustomFunction("Ctrl")]
    internal class Ctrl : Function
    {
        public Ctrl(List<IFunction> children, List<System.Object> arguments)
            : base(children, arguments)
        {
        }
        public override Number Evaluate(Character character)
        {
            if (character == null) return new Number();
            return new Number(character.PlayerControl == PlayerControl.InControl);
        }

        public static Node Parse(ParseState state)
        {
            return state.BaseNode;
        }
    }

    [CustomFunction("DrawGame")]
    class DrawGame : Function
    {
        public DrawGame(List<IFunction> children, List<System.Object> arguments)
               : base(children, arguments)
        {
        }
        public override Number Evaluate(Character character)
        {
            if (character == null) return new Number();

            return new Number(character.Engine.Team1.VictoryStatus.Lose && character.Engine.Team2.VictoryStatus.Lose);
        }

        public static Node Parse(ParseState state)
        {
            return state.BaseNode;
        }
    }

    [CustomFunction("E")]
    class E : Function
    {
        public E(List<IFunction> children, List<System.Object> arguments)
            : base(children, arguments)
        {
        }
        public override Number Evaluate(Character character)
        {
            return new Number(Math.E);
        }

        public static Node Parse(ParseState state)
        {
            return state.BaseNode;
        }
    }

    [CustomFunction("Exp")]
    class Exp : Function
    {
        public Exp(List<IFunction> children, List<System.Object> arguments)
               : base(children, arguments)
        {
        }
        public override Number Evaluate(Character character)
        {
            if (Children.Count != 1) return new Number();

            Number value = Children[0].Evaluate(character);
            if (value.NumberType == NumberType.None) return new Number();

            return new Number(Math.Exp(value.FloatValue));
        }

        public static Node Parse(ParseState state)
        {
            return state.BuildParenNumberNode(true);
        }
    }

    [CustomFunction("Facing")]
    class Facing : Function
    {
        public Facing(List<IFunction> children, List<System.Object> arguments)
            : base(children, arguments)
        {
        }
        public override Number Evaluate(Character character)
        {
            if (character == null) return new Number();

            switch (character.CurrentFacing)
            {
                case UnityMugen.Facing.Left:
                    return new Number(-1);

                case UnityMugen.Facing.Right:
                    return new Number(1);

                default:
                    if (KeepLog)
                        Debug.Log("Error | Facing");
                    return new Number();
            }
        }

        public static Node Parse(ParseState parsestate)
        {
            return parsestate.BaseNode;
        }
    }

    [CustomFunction("Floor")]
    class Floor : Function
    {
        public Floor(List<IFunction> children, List<System.Object> arguments)
               : base(children, arguments)
        {
        }
        public override Number Evaluate(Character character)
        {
            if (Children.Count != 1) return new Number();

            Number value = Children[0].Evaluate(character);

            switch (value.NumberType)
            {
                case NumberType.Int:
                    return new Number(value.IntValue);

                case NumberType.Float:
                    return new Number(Math.Floor(value.FloatValue));

                default:
                    return new Number();
            }
        }

        public static Node Parse(ParseState state)
        {
            return state.BuildParenNumberNode(true);
        }
    }

    [CustomFunction("FrontEdgeBodyDist")]
    class FrontEdgeBodyDist : Function
    {
        public FrontEdgeBodyDist(List<IFunction> children, List<System.Object> arguments)
            : base(children, arguments)
        {
        }
        public override Number Evaluate(Character character)
        {
            if (character == null) return new Number();

            var camerarect = character.Engine.CameraFE.ScreenBounds();

            switch (character.CurrentFacing)
            {
                case UnityMugen.Facing.Left:
                    return new Number((int)((character.GetLeftEdgePosition(true) - camerarect.xMin) * Constant.Scale2));

                case UnityMugen.Facing.Right:
                    return new Number((int)((camerarect.xMax - character.GetRightEdgePosition(true)) * Constant.Scale2));

                default:
                    if (KeepLog)
                        Debug.Log("Error | Facing");

                    return new Number();
            }
        }

        public static Node Parse(ParseState state)
        {
            return state.BaseNode;
        }
    }

    [CustomFunction("FrontEdgeDist")]
    class FrontEdgeDist : Function
    {
        public FrontEdgeDist(List<IFunction> children, List<System.Object> arguments)
            : base(children, arguments)
        {
        }
        public override Number Evaluate(Character character)
        {
            if (character == null) return new Number();

            var camerarect = character.Engine.CameraFE.ScreenBounds();

            switch (character.CurrentFacing)
            {
                case UnityMugen.Facing.Left:
                    return new Number((int)((character.GetLeftEdgePosition(false) - camerarect.xMin) * Constant.Scale2));

                case UnityMugen.Facing.Right:
                    return new Number((int)((camerarect.xMax - character.GetRightEdgePosition(false)) * Constant.Scale2));

                default:
                    if (KeepLog)
                        Debug.Log("Error | FrontEdgeDist");

                    return new Number();
            }
        }

        public static Node Parse(ParseState state)
        {
            return state.BaseNode;
        }
    }

    [CustomFunction("FVar")]
    class FVar : Function
    {
        public FVar(List<IFunction> children, List<System.Object> arguments)
            : base(children, arguments)
        {
        }
        public override Number Evaluate(Character character)
        {
            if (character == null || Children.Count != 1) return new Number();

            Number value = Children[0].Evaluate(character);
            if (value.NumberType == NumberType.None) return new Number();

            if (character.Variables.GetFloat(value.IntValue, false, out float result))
                return new Number(result);

            if (KeepLog)
                Debug.Log("Error | FVar");

            return new Number();
        }

        public static Node Parse(ParseState state)
        {
            return state.BuildParenNumberNode(true);
        }
    }

    [CustomFunction("GameTime")]
    class GameTime : Function
    {
        public GameTime(List<IFunction> children, List<System.Object> arguments)
               : base(children, arguments)
        {
        }
        public override Number Evaluate(Character character)
        {
            if (character == null) return new Number();

            return new Number(character.Engine.TickCount);
        }

        public static Node Parse(ParseState state)
        {
            return state.BaseNode;
        }
    }

    [CustomFunction("HitCount")]
    class HitCount : Function
    {
        public HitCount(List<IFunction> children, List<System.Object> arguments)
               : base(children, arguments)
        {
        }
        public override Number Evaluate(Character character)
        {
            if (character == null) return new Number();

            return new Number(character.OffensiveInfo.HitCount);
        }

        public static Node Parse(ParseState parsestate)
        {
            return parsestate.BaseNode;
        }
    }

    [CustomFunction("HitDefAttr")]
    class HitDefAttr : Function
    {
        public HitDefAttr(List<IFunction> children, List<System.Object> arguments)
            : base(children, arguments)
        {
        }
        public override Number Evaluate(Character character)
        {
            if (character == null || Arguments.Count <= 2) return new Number();

            if (character.MoveType != UnityMugen.MoveType.Attack) return new Number(false);

            var attr = character.OffensiveInfo.HitDef.HitAttribute;

            Operator @operator = (Operator)Arguments[0];
            AttackStateType ast = (AttackStateType)Arguments[1];

            var heightmatch = (attr.AttackHeight & ast) != AttackStateType.None;

            var datamatch = false;
            for (int i = 2; i != Arguments.Count; ++i)
            {
                Combat.HitType hittype = (Combat.HitType)Arguments[i];
                if (attr.HasData(hittype) == false) continue;

                datamatch = true;
                break;
            }

            switch (@operator)
            {
                case Operator.Equals:
                    return new Number(heightmatch && datamatch);

                case Operator.NotEquals:
                    return new Number(!heightmatch || !datamatch);

                default:
                    if (KeepLog)
                        Debug.Log("Error | HitDefAttr");

                    return new Number();
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
    class HitFall : Function
    {
        public HitFall(List<IFunction> children, List<System.Object> arguments)
               : base(children, arguments)
        {
        }
        public override Number Evaluate(Character character)
        {
            if (character == null) return new Number();

            return new Number(character.DefensiveInfo.IsFalling);
        }

        public static Node Parse(ParseState parsestate)
        {
            return parsestate.BaseNode;
        }
    }

    [CustomFunction("HitOver")]
    class HitOver : Function
    {
        public HitOver(List<IFunction> children, List<System.Object> arguments)
                  : base(children, arguments)
        {
        }
        public override Number Evaluate(Character character)
        {
            if (character == null) return new Number();

            return new Number(character.DefensiveInfo.HitTime <= 0);
        }

        public static Node Parse(ParseState parsestate)
        {
            return parsestate.BaseNode;
        }
    }

    [CustomFunction("HitPauseTime")]
    class HitPauseTime : Function
    {
        public HitPauseTime(List<IFunction> children, List<System.Object> arguments)
               : base(children, arguments)
        {
        }
        public override Number Evaluate(Character character)
        {
            if (character == null) return new Number();

            return new Number(character.OffensiveInfo.HitPauseTime);
        }

        public static Node Parse(ParseState parsestate)
        {
            return parsestate.BaseNode;
        }
    }

    [CustomFunction("HitShakeOver")]
    class HitShakeOver : Function
    {
        public HitShakeOver(List<IFunction> children, List<System.Object> arguments)
               : base(children, arguments)
        {
        }
        public override Number Evaluate(Character character)
        {
            if (character == null) return new Number();

            return new Number(character.DefensiveInfo.HitShakeTime <= 0);
        }

        public static Node Parse(ParseState parsestate)
        {
            return parsestate.BaseNode;
        }
    }

    [CustomFunction("HitVel")]
    class HitVel : Function
    {
        public HitVel(List<IFunction> children, List<System.Object> arguments)
            : base(children, arguments)
        {
        }
        public override Number Evaluate(Character character)
        {
            if (character == null || Arguments.Count != 1) return new Number();

            Axis axis = (Axis)Arguments[0];

            switch (axis)
            {
                case Axis.X:
                    return new Number(character.DefensiveInfo.GetHitVelocity().x * Constant.Scale2);

                case Axis.Y:
                    return new Number(character.DefensiveInfo.GetHitVelocity().y * Constant.Scale2);

                default:
                    if (KeepLog)
                        Debug.Log("Error | HitVel");

                    return new Number();
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
    class ID : Function
    {
        public ID(List<IFunction> children, List<System.Object> arguments)
               : base(children, arguments)
        {
        }
        public override Number Evaluate(Character character)
        {
            if (character == null) return new Number();

            return new Number((int)character.Id);
        }

        public static Node Parse(ParseState parsestate)
        {
            return parsestate.BaseNode;
        }
    }

    [CustomFunction("IfElse")]
    class IfElse : Function
    {
        public IfElse(List<IFunction> children, List<System.Object> arguments)
               : base(children, arguments)
        {
        }
        public override Number Evaluate(Character character)
        {
            if (character == null || Children.Count != 3) return new Number();

            Number r1 = Children[0].Evaluate(character);
            Number r2 = Children[1].Evaluate(character);
            Number r3 = Children[2].Evaluate(character);

            if (r1.NumberType == NumberType.None || r2.NumberType == NumberType.None || r3.NumberType == NumberType.None) return new Number();

            if(r2.NumberType == NumberType.Float || r3.NumberType == NumberType.Float)
            {
                if (r1.FloatValue < 0)
                    return r1;

                return new Number(r1.FloatValue != 0 ? r2.FloatValue : r3.FloatValue);
            }
            else if (r2.NumberType == NumberType.Int && r3.NumberType == NumberType.Int)
            {
                if (r1.IntValue < 0)
                    return r1;

                return new Number(r1.IntValue != 0 ? r2.IntValue : r3.IntValue);
            }
            else 
            { 
                return new Number();
            }
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
    class InGuardDist : Function
    {
        public InGuardDist(List<IFunction> children, List<System.Object> arguments)
            : base(children, arguments)
        {
        }
        public override Number Evaluate(Character character)
        {
            if (character == null) return new Number();

            foreach (var entity in character.Engine.Entities)
            {
                var opp = character.FilterEntityAsCharacter(entity, AffectTeam.Enemy);
                if (opp != null && opp.OffensiveInfo.ActiveHitDef && InGuardDistance(opp.OffensiveInfo.HitDef, opp, character))
                {
                    return new Number(true);
                }

                var projectile = entity as Combat.Projectile;
                if (projectile != null && projectile.Team != character.Team && InGuardDistance(projectile.Data.HitDef, projectile, character))
                {
                    return new Number(true);
                }
            }

            return new Number(false);
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
    class IsHelper : Function
    {
        public IsHelper(List<IFunction> children, List<System.Object> arguments)
               : base(children, arguments)
        {
        }
        public override Number Evaluate(Character character)
        {
            if (character == null) return new Number();
            if (Children.Count == 0 || Children.Count != 1) return new Number(true);

            Number helperid = Children[0].Evaluate(character);
            if (character.typeEntity != TypeEntity.Helper)
                return new Number(false);

            return new Number(helperid.IntValue >= 0 ? character.Id == helperid.IntValue : true);
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
    class IsHomeTeam : Function
    {
        public IsHomeTeam(List<IFunction> children, List<System.Object> arguments)
               : base(children, arguments)
        {
        }
        public override Number Evaluate(Character character)
        {
            if (character == null) return new Number();

            return new Number(character.Team.Side == UnityMugen.TeamSide.Left);
        }

        public static Node Parse(ParseState parsestate)
        {
            return parsestate.BaseNode;
        }
    }

    [CustomFunction("Life")]
    class Life : Function
    {
        public Life(List<IFunction> children, List<System.Object> arguments)
               : base(children, arguments)
        {
        }
        public override Number Evaluate(Character character)
        {
            if (character == null) return new Number();

            return new Number(character.Life);
        }

        public static Node Parse(ParseState state)
        {
            return state.BaseNode;
        }
    }

    [CustomFunction("LifeMax")]
    class LifeMax : Function
    {
        public LifeMax(List<IFunction> children, List<System.Object> arguments)
               : base(children, arguments)
        {
        }
        public override Number Evaluate(Character character)
        {
            if (character == null) return new Number();

            return new Number(character.BasePlayer.playerConstants.MaximumLife);
        }

        public static Node Parse(ParseState state)
        {
            return state.BaseNode;
        }
    }

    [CustomFunction("Ln")]
    class Ln : Function
    {
        public Ln(List<IFunction> children, List<System.Object> arguments)
               : base(children, arguments)
        {
        }
        public override Number Evaluate(Character character)
        {
            if (Children.Count != 1) return new Number();

            Number number = Children[0].Evaluate(character);
            if (number.NumberType == NumberType.None) return new Number();

            return new Number(Math.Log10(number.FloatValue));
        }

        public static Node Parse(ParseState state)
        {
            return state.BuildParenNumberNode(true);
        }
    }

    [CustomFunction("Log")]
    class Log : Function
    {
        public Log(List<IFunction> children, List<System.Object> arguments)
               : base(children, arguments)
        {
        }
        public override Number Evaluate(Character character)
        {
            if (Children.Count != 2) return new Number();

            Number n1 = Children[0].Evaluate(character);
            Number n2 = Children[1].Evaluate(character);
            if (n1.NumberType == NumberType.None || n2.NumberType == NumberType.None) return new Number();

            return new Number(Math.Log(n1.FloatValue, n2.FloatValue));
        }

        public static Node Parse(ParseState state)
        {
            return state.BuildParenNumberNode(true);
        }
    }

    [CustomFunction("Lose")]
    class Lose : Function
    {
        public Lose(List<IFunction> children, List<System.Object> arguments)
               : base(children, arguments)
        {
        }
        public override Number Evaluate(Character character)
        {
            if (character == null) return new Number();

            return new Number(character.Team.VictoryStatus.Lose);
        }

        public static Node Parse(ParseState parsestate)
        {
            return parsestate.BaseNode;
        }
    }

    [CustomFunction("LoseKO")]
    class LoseKO : Function
    {
        public LoseKO(List<IFunction> children, List<System.Object> arguments)
               : base(children, arguments)
        {
        }
        public override Number Evaluate(Character character)
        {
            if (character == null) return new Number();

            return new Number(character.Team.VictoryStatus.LoseKO);
        }

        public static Node Parse(ParseState parsestate)
        {
            return parsestate.BaseNode;
        }
    }

    [CustomFunction("LoseTime")]
    class LoseTime : Function
    {
        public LoseTime(List<IFunction> children, List<System.Object> arguments)
               : base(children, arguments)
        {
        }
        public override Number Evaluate(Character character)
        {
            if (character == null) return new Number();

            return new Number(character.Team.VictoryStatus.LoseTime);
        }

        public static Node Parse(ParseState parsestate)
        {
            return parsestate.BaseNode;
        }
    }

    [CustomFunction("MatchNo")]
    class MatchNo : Function
    {
        public MatchNo(List<IFunction> children, List<System.Object> arguments)
               : base(children, arguments)
        {
        }
        public override Number Evaluate(Character character)
        {
            if (character == null) return new Number();

            return new Number(character.Engine.MatchNumber);
        }

        public static Node Parse(ParseState parsestate)
        {
            return parsestate.BaseNode;
        }
    }

    [CustomFunction("MatchOver")]
    class MatchOver : Function
    {
        public MatchOver(List<IFunction> children, List<System.Object> arguments)
            : base(children, arguments)
        {
        }
        public override Number Evaluate(Character character)
        {
            if (character == null) return new Number();

            return new Number(character.Engine.IsMatchOver());
        }

        public static Node Parse(ParseState parsestate)
        {
            return parsestate.BaseNode;
        }
    }

    [CustomFunction("MoveContact")]
    class MoveContact : Function
    {
        public MoveContact(List<IFunction> children, List<System.Object> arguments)
               : base(children, arguments)
        {
        }
        public override Number Evaluate(Character character)
        {
            if (character == null) return new Number();

            //int result = character.MoveType == UnityMugen.MoveType.Attack ? character.OffensiveInfo.MoveContact : 0;
            //return result != 0; // Atualizado Tiago
#warning em teste
            return new Number(character.OffensiveInfo.MoveContact); // Baseado no IK
        }

        public static Node Parse(ParseState parsestate)
        {
            return parsestate.BaseNode;
        }
    }

    [CustomFunction("MoveGuarded")]
    class MoveGuarded : Function
    {
        public MoveGuarded(List<IFunction> children, List<System.Object> arguments)
               : base(children, arguments)
        {
        }
        public override Number Evaluate(Character character)
        {
            if (character == null) return new Number();

            //int result = character.MoveType == UnityMugen.MoveType.Attack ? character.OffensiveInfo.MoveGuarded : 0;
            //return result != 0; // Atualizado Tiago
#warning em teste
            return new Number(character.OffensiveInfo.MoveGuarded); // Baseado no IK
        }

        public static Node Parse(ParseState parsestate)
        {
            return parsestate.BaseNode;
        }
    }


    [CustomFunction("MoveReversed")]
    class MoveReversed : Function
    {
        public MoveReversed(List<IFunction> children, List<System.Object> arguments)
                  : base(children, arguments)
        {
        }
        public override Number Evaluate(Character character)
        {
            if (character == null) return new Number();

            int result = character.MoveType == UnityMugen.MoveType.Attack ? character.OffensiveInfo.MoveReversed : 0;
            return new Number(result); // Atualizado Tiago
        }

        public static Node Parse(ParseState parsestate)
        {
            return parsestate.BaseNode;
        }
    }

    [CustomFunction("MoveType")]
    class MoveType : Function
    {
        public MoveType(List<IFunction> children, List<System.Object> arguments)
                  : base(children, arguments)
        {
        }
        public override Number Evaluate(Character character)
        {
            if (character == null || Arguments.Count != 2) return new Number();

            Operator @operator = (Operator)Arguments[0];
            UnityMugen.MoveType movetype = (UnityMugen.MoveType)Arguments[1];

            if (movetype == UnityMugen.MoveType.Unchanged || movetype == UnityMugen.MoveType.None)
            {
                return new Number();
            }

            switch (@operator)
            {
                case Operator.Equals:
                    return new Number(movetype == character.MoveType);

                case Operator.NotEquals:
                    return new Number(movetype != character.MoveType);

                default:
                    if (KeepLog)
                        Debug.Log("Error | MoveType");

                    return new Number();
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
    class MoveHit : Function
    {
        public MoveHit(List<IFunction> children, List<System.Object> arguments)
               : base(children, arguments)
        {
        }
        public override Number Evaluate(Character character)
        {
            if (character == null) return new Number();

            //return character.MoveType == UnityMugen.MoveType.Attack ? character.OffensiveInfo.MoveHit : 0;
            //character.MoveContact == MoveContact.
#warning em teste
            return new Number(character.OffensiveInfo.MoveHit); // Baseado no IK
        }

        public static Node Parse(ParseState parsestate)
        {
            return parsestate.BaseNode;
        }
    }

    [CustomFunction("Name")]
    class Name : Function
    {
        public Name(List<IFunction> children, List<System.Object> arguments)
            : base(children, arguments)
        {
        }
        public override Number Evaluate(Character character)
        {
            if (character == null || Arguments.Count != 2) return new Number();

            Operator @operator = (Operator)Arguments[0];
            string text = (string)Arguments[1];

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

                return new Number();
            }

            switch (@operator)
            {
                case Operator.Equals:
                    return new Number(string.Equals(name, text, StringComparison.OrdinalIgnoreCase));

                case Operator.NotEquals:
                    return new Number(!string.Equals(name, text, StringComparison.OrdinalIgnoreCase));

                default:
                    if (KeepLog)
                        Debug.Log("Error | Name");

                    return new Number();
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
    class NumEnemy : Function
    {
        public NumEnemy(List<IFunction> children, List<System.Object> arguments)
               : base(children, arguments)
        {
        }
        public override Number Evaluate(Character character)
        {
            if (character == null) return new Number();

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

            return new Number(count);
        }

        public static Node Parse(ParseState parsestate)
        {
            return parsestate.BaseNode;
        }
    }

    [CustomFunction("NumExplod")]
    class NumExplod : Function
    {
        public NumExplod(List<IFunction> children, List<System.Object> arguments)
               : base(children, arguments)
        {
        }
        public override Number Evaluate(Character character)
        {
            if (character == null || Children.Count != 1) return new Number();

            Number explodId = Children[0].Evaluate(character);
            if (explodId.NumberType != NumberType.Int) return new Number();

            var count = 0;

            foreach (var explod in character.GetExplods(explodId.IntValue))
                ++count;

            return new Number(count);
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
    class NumHelper : Function
    {
        public NumHelper(List<IFunction> children, List<System.Object> arguments)
               : base(children, arguments)
        {
        }
        public override Number Evaluate(Character character)
        {
            if (character == null || Children.Count != 1) return new Number();

            Number helperId = Children[0].Evaluate(character);
            if (helperId.NumberType != NumberType.Int) return new Number();

            if (helperId.IntValue >= 0)
            {
                List<UnityMugen.Combat.Helper> helpers;
                if (character.BasePlayer.Helpers.TryGetValue(helperId.IntValue, out helpers))
                    return new Number(helpers.Count);

                return new Number(0);
            }

            var count = 0;

            foreach (var data in character.BasePlayer.Helpers) count += data.Value.Count;

            return new Number(count);
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
    class NumPartner : Function
    {
        public NumPartner(List<IFunction> children, List<System.Object> arguments)
               : base(children, arguments)
        {
        }
        public override Number Evaluate(Character character)
        {
            if (character == null) return new Number();

            var count = 0;
            foreach (var entity in character.Engine.Entities)
            {
                var partner = character.BasePlayer.FilterEntityAsPartner(entity);
                if (partner == null) continue;

                ++count;
            }

            return new Number(count);
        }

        public static Node Parse(ParseState parsestate)
        {
            return parsestate.BaseNode;
        }
    }

    [CustomFunction("NumProj")]
    class NumProj : Function
    {
        public NumProj(List<IFunction> children, List<System.Object> arguments)
               : base(children, arguments)
        {
        }
        public override Number Evaluate(Character character)
        {
            if (character == null) return new Number();

            var count = 0;
            foreach (var entity in character.Engine.Entities)
            {
                var projectile = character.FilterEntityAsProjectile(entity, int.MinValue);
                if (projectile == null) continue;

                ++count;
            }

            return new Number(count);
        }

        public static Node Parse(ParseState parsestate)
        {
            return parsestate.BaseNode;
        }
    }

    [CustomFunction("NumProjID")]
    class NumProjID : Function
    {
        public NumProjID(List<IFunction> children, List<System.Object> arguments)
               : base(children, arguments)
        {
        }
        public override Number Evaluate(Character character)
        {
            if (character == null || Children.Count != 1) return new Number();

            Number projectileId = Children[0].Evaluate(character);
            if (projectileId.NumberType != NumberType.Int) return new Number();

            var count = 0;
            foreach (var entity in character.Engine.Entities)
            {
                var projectile = character.FilterEntityAsProjectile(entity, projectileId.IntValue);
                if (projectile == null) continue;

                ++count;
            }

            return new Number(count);
        }

        public static Node Parse(ParseState parsestate)
        {
            return parsestate.BuildParenNumberNode(true);
        }
    }

    [CustomFunction("NumTarget")]
    class NumTarget : Function
    {
        public NumTarget(List<IFunction> children, List<System.Object> arguments)
               : base(children, arguments)
        {
        }
        public override Number Evaluate(Character character)
        {
            if (character == null || Children.Count != 1) return new Number();

            Number targetId = Children[0].Evaluate(character);
            if (targetId.NumberType != NumberType.Int) return new Number();

            var count = 0;
            foreach (var target in character.GetTargets(targetId.IntValue)) ++count;

            return new Number(count);
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
    class NumGraphicUI : Function
    {
        public NumGraphicUI(List<IFunction> children, List<System.Object> arguments)
                  : base(children, arguments)
        {
        }
        public override Number Evaluate(Character character)
        {
            if (character == null || Children.Count != 1) return new Number();

            Number graphicUiID = Children[0].Evaluate(character);
            if (graphicUiID.NumberType != NumberType.Int) return new Number();

            var count = 0;
            foreach (var graphicUI in character.GetGraphicUIs(graphicUiID.IntValue))
                ++count;

            return new Number(count);
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
    class P1Name : Function
    {
        public P1Name(List<IFunction> children, List<System.Object> arguments)
            : base(children, arguments)
        {
        }
        public override Number Evaluate(Character character)
        {
            if (character == null || Arguments.Count != 2) return new Number();

            Operator @operator = (Operator)Arguments[0];
            string text = (string)Arguments[1];

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

                return new Number();
            }

            switch (@operator)
            {
                case Operator.Equals:
                    return new Number(string.Equals(name, text, StringComparison.OrdinalIgnoreCase));

                case Operator.NotEquals:
                    return new Number(!string.Equals(name, text, StringComparison.OrdinalIgnoreCase));

                default:
                    return new Number();
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
    class P2BodyDist : Function
    {
        public P2BodyDist(List<IFunction> children, List<System.Object> arguments)
            : base(children, arguments)
        {
        }
        public override Number Evaluate(Character character)
        {
            if (character == null || Arguments.Count != 1) return new Number();

            var opponent = character.GetOpponent();
            if (opponent == null)
            {
                if (KeepLog)
                    Debug.Log("Error | P2BodyDist");

                return new Number();
            }

            Axis axis = (Axis)Arguments[0];

            switch (axis)
            {
                case Axis.X:
                    var mylocation = character.GetFrontLocation();
                    var opplocation = opponent.GetFrontLocation();
                    var distance = Math.Abs(mylocation - opplocation);
                    if (character.CurrentFacing == UnityMugen.Facing.Right)
                    {
                        return new Number((opponent.CurrentLocation.x >= character.CurrentLocation.x ? distance : -distance) * Constant.Scale2);
                    }
                    else
                    {
                        return new Number((opponent.CurrentLocation.x >= character.CurrentLocation.x ? -distance : distance) * Constant.Scale2);
                    }

                case Axis.Y:
                    return new Number((opponent.CurrentLocation.y - character.CurrentLocation.y) * Constant.Scale2);

                default:
                    if (KeepLog)
                        Debug.Log("Error | P2BodyDist");

                    return new Number();
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
    class P2Dist : Function
    {
        public P2Dist(List<IFunction> children, List<System.Object> arguments)
            : base(children, arguments)
        {
        }
        public override Number Evaluate(Character character)
        {
            if (character == null || Arguments.Count != 1) return new Number();

            var opponent = character.GetOpponent();
            if (opponent == null)
            {
                if (KeepLog)
                    Debug.Log("Error | P2Dist");

                return new Number();
            }

            Axis axis = (Axis)Arguments[0];

            switch (axis)
            {
                case Axis.X:
                    var distance = Math.Abs(character.CurrentLocation.x - opponent.CurrentLocation.x);
                    if (character.CurrentFacing == UnityMugen.Facing.Right)
                    {
                        return new Number((opponent.CurrentLocation.x >= character.CurrentLocation.x ? distance : -distance) * Constant.Scale2);
                    }
                    else
                    {
                        return new Number((opponent.CurrentLocation.x >= character.CurrentLocation.x ? -distance : distance) * Constant.Scale2);
                    }

                case Axis.Y:
                    return new Number((opponent.CurrentLocation.y - character.CurrentLocation.y) * Constant.Scale2);

                default:
                    if (KeepLog)
                        Debug.Log("Error | P2Dist");

                    return new Number();
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
    class P2Life : Function
    {
        public P2Life(List<IFunction> children, List<System.Object> arguments)
            : base(children, arguments)
        {
        }
        public override Number Evaluate(Character character)
        {
            if (character == null) return new Number();

            var opponent = character.GetOpponent();
            if (opponent == null)
            {
                if (KeepLog)
                    Debug.Log("Error | P2Life");

                return new Number();
            }

            return new Number(opponent.Life);
        }

        public static Node Parse(ParseState parsestate)
        {
            return parsestate.BaseNode;
        }
    }

    [CustomFunction("P2MoveType")]
    class P2MoveType : Function
    {
        public P2MoveType(List<IFunction> children, List<System.Object> arguments)
            : base(children, arguments)
        {
        }
        public override Number Evaluate(Character character)
        {
            if (character == null || Arguments.Count != 2)
            {
                if (KeepLog)
                    Debug.Log("Error | P2MoveType");

                return new Number();
            }

            var opponent = character.GetOpponent();
            if (opponent == null)
            {
                if (KeepLog)
                    Debug.Log("Error | P2MoveType");

                return new Number();
            }


            Operator @operator = (Operator)Arguments[0];
            UnityMugen.MoveType movetype = (UnityMugen.MoveType)Arguments[1];

            if (movetype == UnityMugen.MoveType.Unchanged || movetype == UnityMugen.MoveType.None)
            {
                if (KeepLog)
                    Debug.Log("Error | P2MoveType");

                return new Number();
            }

            switch (@operator)
            {
                case Operator.Equals:
                    return new Number(movetype == opponent.MoveType);

                case Operator.NotEquals:
                    return new Number(movetype != opponent.MoveType);

                default:
                    if (KeepLog)
                        Debug.Log("Error | P2MoveType");

                    return new Number();
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
    class P2Name : Function
    {
        public P2Name(List<IFunction> children, List<System.Object> arguments)
            : base(children, arguments)
        {
        }
        public override Number Evaluate(Character character)
        {
            if (character == null || Arguments.Count != 2) return new Number();

            var p2 = character.Team.OtherTeam.MainPlayer;
            Operator @operator = (Operator)Arguments[0];
            string text = (string)Arguments[1];

            switch (@operator)
            {
                case Operator.Equals:
                    return new Number(p2 != null ? string.Equals(p2.profile.charName, text, StringComparison.OrdinalIgnoreCase) : false);

                case Operator.NotEquals:
                    return new Number(p2 != null ? !string.Equals(p2.profile.charName, text, StringComparison.OrdinalIgnoreCase) : true);

                default:
                    if (KeepLog)
                        Debug.Log("Error | P2Name");

                    return new Number();
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
    class P2StateNo : Function
    {
        public P2StateNo(List<IFunction> children, List<System.Object> arguments)
            : base(children, arguments)
        {
        }
        public override Number Evaluate(Character character)
        {
            if (character == null) return new Number();

            var opponent = character.GetOpponent();
            if (opponent == null)
            {
                if (KeepLog)
                    Debug.Log("Error | P2StateNo");

                return new Number();
            }

            var currentstate = opponent.StateManager.CurrentState;
            return new Number(currentstate != null ? currentstate.number : 0);
        }

        public static Node Parse(ParseState parsestate)
        {
            return parsestate.BaseNode;
        }
    }

    [CustomFunction("P2StateType")]
    class P2StateType : Function
    {
        public P2StateType(List<IFunction> children, List<System.Object> arguments)
            : base(children, arguments)
        {
        }
        public override Number Evaluate(Character character)
        {
            if (character == null || Arguments.Count != 2)
            {
                if (KeepLog)
                    Debug.Log("Error | P2StateType");

                return new Number();
            }

            var opponent = character.GetOpponent();
            if (opponent == null)
            {
                if (KeepLog)
                    Debug.Log("Error | P2StateType");

                return new Number();
            }

            Operator @operator = (Operator)Arguments[0];
            UnityMugen.StateType statetype = (UnityMugen.StateType)Arguments[1];

            if (statetype == UnityMugen.StateType.Unchanged || statetype == UnityMugen.StateType.None)
            {
                if (KeepLog)
                    Debug.Log("Error | P2StateType");

                return new Number();
            }

            switch (@operator)
            {
                case Operator.Equals:
                    return new Number(statetype == opponent.StateType);

                case Operator.NotEquals:
                    return new Number(statetype != opponent.StateType);

                default:
                    if (KeepLog)
                        Debug.Log("Error | P2StateType");

                    return new Number();
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
    class P3Name : Function
    {
        public P3Name(List<IFunction> children, List<System.Object> arguments)
            : base(children, arguments)
        {
        }
        public override Number Evaluate(Character character)
        {
            if (character == null || Arguments.Count != 2) return new Number();

            var p3 = character.Team.TeamMate;
            Operator @operator = (Operator)Arguments[0];
            string text = (string)Arguments[1];

            switch (@operator)
            {
                case Operator.Equals:
                    return new Number(p3 != null ? string.Equals(p3.profile.charName, text, StringComparison.OrdinalIgnoreCase) : false);

                case Operator.NotEquals:
                    return new Number(p3 != null ? !string.Equals(p3.profile.charName, text, StringComparison.OrdinalIgnoreCase) : true);

                default:
                    if (KeepLog)
                        Debug.Log("Error | P3Name");

                    return new Number();
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
    class P4Name : Function
    {
        public P4Name(List<IFunction> children, List<System.Object> arguments)
            : base(children, arguments)
        {
        }
        public override Number Evaluate(Character character)
        {
            if (character == null || Arguments.Count != 2) return new Number();

            var p4 = character.Team.OtherTeam.TeamMate;
            Operator @operator = (Operator)Arguments[0];
            string text = (string)Arguments[1];

            switch (@operator)
            {
                case Operator.Equals:
                    return new Number(p4 != null ? string.Equals(p4.profile.charName, text, StringComparison.OrdinalIgnoreCase) : false);

                case Operator.NotEquals:
                    return new Number(p4 != null ? !string.Equals(p4.profile.charName, text, StringComparison.OrdinalIgnoreCase) : true);

                default:
                    if (KeepLog)
                        Debug.Log("Error | P4Name");

                    return new Number();
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
    class PalNo : Function
    {
        public PalNo(List<IFunction> children, List<System.Object> arguments)
               : base(children, arguments)
        {
        }
        public override Number Evaluate(Character character)
        {
            if (character == null) return new Number();

            return new Number(character.BasePlayer.PaletteNumber);
        }

        public static Node Parse(ParseState parsestate)
        {
            return parsestate.BaseNode;
        }
    }

    [CustomFunction("ParentDist")]
    class ParentDist : Function
    {
        public ParentDist(List<IFunction> children, List<System.Object> arguments)
            : base(children, arguments)
        {
        }
        public override Number Evaluate(Character character)
        {
            if (character == null || Arguments.Count != 1) return new Number();

            var helper = character as UnityMugen.Combat.Helper;
            if (helper == null)
            {
                if (KeepLog)
                    Debug.Log("Error | ParentDist");

                return new Number();
            }

            Axis axis = (Axis)Arguments[0];

            switch (axis)
            {
                case Axis.X:
                    var distance = Math.Abs(helper.CurrentLocation.x - helper.Creator.CurrentLocation.x) * Constant.Scale2;
                    if (helper.CurrentFacing == UnityMugen.Facing.Right)
                    {
                        return new Number(helper.Creator.CurrentLocation.x >= helper.CurrentLocation.x ? distance : -distance);
                    }
                    else
                    {
                        return new Number(helper.Creator.CurrentLocation.x >= helper.CurrentLocation.x ? -distance : distance);
                    }

                case Axis.Y:
                    return new Number(helper.Creator.CurrentLocation.y - helper.CurrentLocation.y);

                default:
                    if (KeepLog)
                        Debug.Log("Error | ParentDist");

                    return new Number();
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
    class Physics : Function
    {
        public Physics(List<IFunction> children, List<System.Object> arguments)
            : base(children, arguments)
        {
        }
        public override Number Evaluate(Character character)
        {
            if (character == null || Arguments.Count != 2)
            {
                if (KeepLog)
                    Debug.Log("Error | Physics");

                return new Number();
            }

            Operator @operator = (Operator)Arguments[0];
            UnityMugen.Physic physics = (UnityMugen.Physic)Arguments[1];

            if (physics == UnityMugen.Physic.Unchanged || physics == UnityMugen.Physic.None)
            {
                if (KeepLog)
                    Debug.Log("Error | Physics");

                return new Number();
            }

            switch (@operator)
            {
                case Operator.Equals:
                    return new Number(physics == character.Physics);

                case Operator.NotEquals:
                    return new Number(physics != character.Physics);

                default:
                    if (KeepLog)
                        Debug.Log("Error | Physics");

                    return new Number();
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
    class Pi : Function
    {
        public Pi(List<IFunction> children, List<System.Object> arguments)
               : base(children, arguments)
        {
        }
        public override Number Evaluate(Character character)
        {
            return new Number(Math.PI);
        }

        public static Node Parse(ParseState state)
        {
            return state.BaseNode;
        }
    }

    [CustomFunction("Pos")]
    class Pos : Function
    {
        public Pos(List<IFunction> children, List<System.Object> arguments)
            : base(children, arguments)
        {
        }
        public override Number Evaluate(Character character)
        {
            if (character == null || Arguments.Count != 1) return new Number();

            Axis axis = (Axis)Arguments[0];

            switch (axis)
            {
                case Axis.X:
                    ////var screenrect = character.Engine.CameraFE.ScreenBounds();
                    //var screenrect = character.Engine.CameraFE.CornerCamera();
                    ////return (character.CurrentLocation.x - screenrect.xMin - screenrect.width / 2) * Constant.Scale2;
                    //return new Number((character.CurrentLocation.x - screenrect.center.x) * Constant.Scale2);

                    var drawlocationCenterCam = Camera.main.WorldToScreenPoint(character.Engine.CameraFE.transCenter.position);
                    float xCam = drawlocationCenterCam.x / (Screen.width / Constant.LocalCoord.x);

                    var drawlocationEntity = Camera.main.WorldToScreenPoint(character.CurrentLocationYTransform());
                    float xEntity = drawlocationEntity.x / (Screen.width / Constant.LocalCoord.x);
                    return new Number(xEntity - xCam);
                case Axis.Y:
                    return new Number(character.CurrentLocation.y * Constant.Scale2);

                default:
                    if (KeepLog)
                        Debug.Log("Error | Pos");

                    return new Number();
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
    class Power : Function
    {
        public Power(List<IFunction> children, List<System.Object> arguments)
            : base(children, arguments)
        {
        }
        public override Number Evaluate(Character character)
        {
            if (character == null) return new Number();

            return new Number(character.BasePlayer.Power);
        }

        public static Node Parse(ParseState parsestate)
        {
            return parsestate.BaseNode;
        }
    }

    [CustomFunction("PowerMax")]
    class PowerMax : Function
    {
        public PowerMax(List<IFunction> children, List<System.Object> arguments)
               : base(children, arguments)
        {
        }
        public override Number Evaluate(Character character)
        {
            if (character == null) return new Number();

            return new Number(character.BasePlayer.playerConstants.MaximumPower);
        }

        public static Node Parse(ParseState parsestate)
        {
            return parsestate.BaseNode;
        }
    }

    [CustomFunction("PlayerIDExist")]
    class PlayerIDExist : Function
    {
        public PlayerIDExist(List<IFunction> children, List<System.Object> arguments)
               : base(children, arguments)
        {
        }
        public override Number Evaluate(Character character)
        {
            if (character == null || Children.Count != 1) return new Number();

            Number id = Children[0].Evaluate(character);
            if (id.NumberType != NumberType.Int) return new Number();

            foreach (var entity in character.Engine.Entities)
            {
                var c = entity as Character;
                if (c == null) continue;

                if (c.Id == id.IntValue) return new Number(true);
            }

            return new Number(false);
        }

        public static Node Parse(ParseState parsestate)
        {
            return parsestate.BuildParenNumberNode(true);
        }
    }

    [CustomFunction("PrevStateNo")]
    class PrevStateNo : Function
    {
        public PrevStateNo(List<IFunction> children, List<System.Object> arguments)
               : base(children, arguments)
        {
        }
        public override Number Evaluate(Character character)
        {
            if (character == null) return new Number();

            var prevstate = character.StateManager.PreviousState;
            return new Number(prevstate != null ? prevstate.number : 0);
        }

        public static Node Parse(ParseState parsestate)
        {
            return parsestate.BaseNode;
        }
    }

    [CustomFunction("ProjCancelTime")]
    class ProjCancelTime : Function
    {
        public ProjCancelTime(List<IFunction> children, List<System.Object> arguments)
               : base(children, arguments)
        {
        }
        public override Number Evaluate(Character character)
        {
            if (character == null || Children.Count != 1) return new Number();

            Number projectileId = Children[0].Evaluate(character);
            if (projectileId.NumberType != NumberType.Int) return new Number();

            var projinfo = character.OffensiveInfo.ProjectileInfo;
            if (projinfo.Type == ProjectileDataType.Cancel && (projectileId.IntValue <= 0 || projectileId.IntValue == projinfo.ProjectileId))
            {
                return new Number(projinfo.Time);
            }

            return new Number(-1);
        }

        public static Node Parse(ParseState parsestate)
        {
            return parsestate.BuildParenNumberNode(true);
        }
    }

    [CustomFunction("ProjContact")]
    class ProjContact : Function
    {
        public ProjContact(List<IFunction> children, List<System.Object> arguments)
                  : base(children, arguments)
        {
        }
        public override Number Evaluate(Character character)
        {
            if (character == null) return new Number();

            if (Children.Count == 3 && Arguments.Count == 1)
            {
                Number projId = Children[0].Evaluate(character);
                Number r2 = Children[1].Evaluate(character);
                Number rhs = Children[2].Evaluate(character);
                Operator compareType = (Operator)Arguments[0];

                var lookingfor = r2.IntValue > 0;
                var projinfo = character.OffensiveInfo.ProjectileInfo;
                var found = (projinfo.Type == ProjectileDataType.Hit || projinfo.Type == ProjectileDataType.Guarded) && (projId.IntValue <= 0 || projId.IntValue == projinfo.ProjectileId);
                if (found) found = SpecialFunctions.LogicalOperation(compareType, projinfo.Time, rhs.IntValue);

                return new Number(lookingfor == found);
            }
            else
            {
                Debug.LogError("ProjContact : Function");
                return new Number();
            }
                
        }
        /*
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
        */
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
    class ProjContactTime : Function
    {
        public ProjContactTime(List<IFunction> children, List<System.Object> arguments)
               : base(children, arguments)
        {
        }
        public override Number Evaluate(Character character)
        {
            if (character == null || Children.Count != 1) return new Number();

            Number projectileId = Children[0].Evaluate(character);
            if (projectileId.NumberType != NumberType.Int) return new Number();

            var projinfo = character.OffensiveInfo.ProjectileInfo;
            if ((projinfo.Type == ProjectileDataType.Hit || projinfo.Type == ProjectileDataType.Guarded) && (projectileId.IntValue <= 0 || projectileId.IntValue == projinfo.ProjectileId))
            {
                return new Number(projinfo.Time);
            }

            return new Number(-1);
        }

        public static Node Parse(ParseState parsestate)
        {
            return parsestate.BuildParenNumberNode(true);
        }
    }

    [CustomFunction("ProjGuarded")]
    class ProjGuarded : Function
    {
        public ProjGuarded(List<IFunction> children, List<System.Object> arguments)
                  : base(children, arguments)
        {
        }
        public override Number Evaluate(Character character)
        {
            if (character == null) return new Number();

            if (Children.Count == 3 && Arguments.Count == 1)
            {
                Number projId = Children[0].Evaluate(character);
                Number r2 = Children[1].Evaluate(character);
                Number rhs = Children[2].Evaluate(character);
                Operator compareType = (Operator)Arguments[0];

                var lookingfor = r2.IntValue > 0;
                var projinfo = character.OffensiveInfo.ProjectileInfo;
                var found = projinfo.Type == ProjectileDataType.Guarded && (projId.IntValue <= 0 || projId.IntValue == projinfo.ProjectileId);
                if (found) found = SpecialFunctions.LogicalOperation(compareType, projinfo.Time, rhs.IntValue);
                return new Number(lookingfor == found);
            }
            else
            {
                Debug.LogError("ProjGuarded : Function");
                return new Number();
            }
        }
        /*
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
        */
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
    class ProjGuardedTime : Function
    {
        public ProjGuardedTime(List<IFunction> children, List<System.Object> arguments)
               : base(children, arguments)
        {
        }
        public override Number Evaluate(Character character)
        {
            if (character == null || Children.Count != 1) return new Number();

            Number projectileId = Children[0].Evaluate(character);
            if (projectileId.NumberType != NumberType.Int) return new Number();

            var projinfo = character.OffensiveInfo.ProjectileInfo;
            if (projinfo.Type == ProjectileDataType.Guarded && (projectileId.IntValue <= 0 || projectileId.IntValue == projinfo.ProjectileId))
            {
                return new Number(projinfo.Time);
            }

            return new Number(-1);
        }

        public static Node Parse(ParseState parsestate)
        {
            return parsestate.BuildParenNumberNode(true);
        }
    }

    [CustomFunction("ProjHit")]
    class ProjHit : Function
    {
        public ProjHit(List<IFunction> children, List<System.Object> arguments)
                  : base(children, arguments)
        {
        }
        public override Number Evaluate(Character character)
        {
            if (character == null) return new Number();

            if (Children.Count == 3 && Arguments.Count == 1)
            {
                Number projId = Children[0].Evaluate(character);
                Number r2 = Children[1].Evaluate(character);
                Number rhs = Children[2].Evaluate(character);
                Operator compareType = (Operator)Arguments[0];

                var lookingfor = r2.IntValue > 0;

                var projinfo = character.OffensiveInfo.ProjectileInfo;

                var found = projinfo.Type == ProjectileDataType.Hit && (projId.IntValue <= 0 || projId.IntValue == projinfo.ProjectileId);
                if (found) found = SpecialFunctions.LogicalOperation(compareType, projinfo.Time, rhs.IntValue);

                return new Number(lookingfor == found);
            }
            else
            {
                Debug.LogError("ProjHit : Function");
                return new Number();
            }
        }
        /*
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
        */
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
    class ProjHitTime : Function
    {
        public ProjHitTime(List<IFunction> children, List<System.Object> arguments)
               : base(children, arguments)
        {
        }
        public override Number Evaluate(Character character)
        {
            if (character == null || Children.Count != 1) return new Number();

            Number projectileId = Children[0].Evaluate(character);
            if (projectileId.NumberType != NumberType.Int) return new Number();

            var projinfo = character.OffensiveInfo.ProjectileInfo;
            if (projinfo.Type == ProjectileDataType.Hit && (projectileId.IntValue <= 0 || projectileId.IntValue == projinfo.ProjectileId))
            {
                return new Number(projinfo.Time);
            }

            return new Number(-1);
        }

        public static Node Parse(ParseState parsestate)
        {
            return parsestate.BuildParenNumberNode(true);
        }
    }

    [CustomFunction("Random")]
    class Random : Function
    {
        public Random(List<IFunction> children, List<System.Object> arguments)
               : base(children, arguments)
        {
        }
        public override Number Evaluate(Character character)
        {
            if (character == null) return new Number();

#warning analizar posteriormente game online
            var rng = LauncherEngine.Inst.random;
            return new Number(rng.NewInt(0, 999));
        }

        public static Node Parse(ParseState parsestate)
        {
            return parsestate.BaseNode;
        }
    }

    [CustomFunction("RootDist")]
    class RootDist : Function
    {
        public RootDist(List<IFunction> children, List<System.Object> arguments)
            : base(children, arguments)
        {
        }
        public override Number Evaluate(Character character)
        {
            if (character == null || Arguments.Count != 1) return new Number();

            var helper = character as UnityMugen.Combat.Helper;
            if (helper == null)
            {
                if (KeepLog)
                    Debug.Log("Error | RootDist");

                return new Number();
            }

            Axis axis = (Axis)Arguments[0];

            switch (axis)
            {
                case Axis.X:
                    var distance = Math.Abs(helper.CurrentLocation.x - helper.BasePlayer.CurrentLocation.x);
                    if (helper.CurrentFacing == UnityMugen.Facing.Right)
                    {
                        float valueX = helper.BasePlayer.CurrentLocation.x >= helper.CurrentLocation.x ? distance : -distance;
                        return new Number(valueX * Constant.Scale2);
                    }
                    else
                    {
                        float valueX = helper.BasePlayer.CurrentLocation.x >= helper.CurrentLocation.x ? -distance : distance;
                        return new Number(valueX * Constant.Scale2);
                    }

                case Axis.Y:
                    float valueY = helper.BasePlayer.CurrentLocation.y - helper.CurrentLocation.y;
                    return new Number(valueY * Constant.Scale2);

                default:
                    if (KeepLog)
                        Debug.Log("Error | RootDist");

                    return new Number();
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
    class RoundNo : Function
    {
        public RoundNo(List<IFunction> children, List<System.Object> arguments)
               : base(children, arguments)
        {
        }
        public override Number Evaluate(Character character)
        {
            if (character == null) return new Number();

            return new Number(character.Engine.RoundNumber);
        }

        public static Node Parse(ParseState parsestate)
        {
            return parsestate.BaseNode;
        }
    }

    [CustomFunction("RoundsExisted")]
    class RoundsExisted : Function
    {
        public RoundsExisted(List<IFunction> children, List<System.Object> arguments)
            : base(children, arguments)
        {
        }
        public override Number Evaluate(Character character)
        {
            if (character == null) return new Number();

            return new Number(character.RoundsExisted);
        }

        public static Node Parse(ParseState parsestate)
        {
            return parsestate.BaseNode;
        }
    }

    [CustomFunction("RoundState")]
    class RoundState : Function
    {
        public RoundState(List<IFunction> children, List<System.Object> arguments)
               : base(children, arguments)
        {
        }
        public override Number Evaluate(Character character)
        {
            if (character == null) return new Number();

            switch (character.Engine.RoundState)
            {
                case UnityMugen.RoundState.PreIntro:
                    return new Number(0);

                case UnityMugen.RoundState.Intro:
                    return new Number(1);

                case UnityMugen.RoundState.Fight:
                    return new Number(2);

                case UnityMugen.RoundState.PreOver:
                    return new Number(3);

                case UnityMugen.RoundState.Over:
                    return new Number(4);

                default:
                    return new Number(-1);
            }
        }

        public static Node Parse(ParseState parsestate)
        {
            return parsestate.BaseNode;
        }
    }

    [CustomFunction("ScreenPos")]
    class ScreenPos : Function
    {
        public ScreenPos(List<IFunction> children, List<System.Object> arguments)
            : base(children, arguments)
        {
        }
        public override Number Evaluate(Character character)
        {
            if (character == null || Arguments.Count != 1) return new Number();

            Axis axis = (Axis)Arguments[0];
            var drawlocation = Camera.main.WorldToScreenPoint(character.CurrentLocationYTransform());

            switch (axis)
            {
                case Axis.X:
                    return new Number(drawlocation.x / (Screen.width / Constant.LocalCoord.x));

                case Axis.Y:
                    return new Number((Screen.height - drawlocation.y) / (Screen.height / Constant.LocalCoord.y));

                default:
                    if (KeepLog)
                        Debug.Log("Error | ScreenPos");

                    return new Number();
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
    class SelfAnimExist : Function
    {
        public SelfAnimExist(List<IFunction> children, List<System.Object> arguments)
               : base(children, arguments)
        {
        }
        public override Number Evaluate(Character character)
        {
            if (character == null || Children.Count != 1) return new Number();

            Number animationnumber = Children[0].Evaluate(character);
            if (animationnumber.NumberType != NumberType.Int) return new Number();

            return new Number(character.AnimationManager.HasAnimation(animationnumber.IntValue));
        }

        public static Node Parse(ParseState parsestate)
        {
            return parsestate.BuildParenNumberNode(true);
        }
    }

    [CustomFunction("Sin")]
    class Sin : Function
    {
        public Sin(List<IFunction> children, List<System.Object> arguments)
               : base(children, arguments)
        {
        }
        public override Number Evaluate(Character character)
        {
            Number number = Children[0].Evaluate(character);
            if (number.NumberType == NumberType.None) return new Number();

            return new Number(Math.Sin(number.FloatValue));
        }

        public static Node Parse(ParseState state)
        {
            return state.BuildParenNumberNode(true);
        }
    }

    [CustomFunction("StateNo")]
    class StateNo : Function
    {
        public StateNo(List<IFunction> children, List<System.Object> arguments)
            : base(children, arguments)
        {
        }
        public override Number Evaluate(Character character)
        {
            if (character == null)
            {
                if (KeepLog)
                    Debug.Log("Error | StateNo");

                return new Number();
            }

            var currentstate = character.StateManager.CurrentState;
            return new Number(currentstate != null ? currentstate.number : 0);
        }

        public static Node Parse(ParseState parsestate)
        {
            return parsestate.BaseNode;
        }
    }

    [CustomFunction("StateType")]
    class StateType : Function
    {
        public StateType(List<IFunction> children, List<System.Object> arguments)
            : base(children, arguments)
        {
        }
        public override Number Evaluate(Character character)
        {
            if (character == null || Arguments.Count != 2)
            {
                if (KeepLog)
                    Debug.Log("Error | StateType");

                return new Number();
            }

            Operator @operator = (Operator)Arguments[0];
            UnityMugen.StateType statetype = (UnityMugen.StateType)Arguments[1];

            if (statetype == UnityMugen.StateType.Unchanged || statetype == UnityMugen.StateType.None)
            {
                if (KeepLog)
                    Debug.Log("Error | StateType");

                return new Number();
            }

            switch (@operator)
            {
                case Operator.Equals:
                    return new Number(statetype == character.StateType);

                case Operator.NotEquals:
                    return new Number(statetype != character.StateType);

                default:
                    if (KeepLog)
                        Debug.Log("Error | StateType");

                    return new Number();
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
    class StageVar : Function
    {
        public StageVar(List<IFunction> children, List<System.Object> arguments)
               : base(children, arguments)
        {
        }
        public override Number Evaluate(Character character)
        {
            if (character == null || Arguments.Count != 2) return new Number();

            Operator @operator = (Operator)Arguments[0];
            string text = (string)Arguments[1];

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

            return new Number(false);
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
    class SysFVar : Function
    {
        public SysFVar(List<IFunction> children, List<System.Object> arguments)
            : base(children, arguments)
        {
        }
        public override Number Evaluate(Character character)
        {
            if (character == null || Children.Count != 1) return new Number();

            Number value = Children[0].Evaluate(character);
            if (value.NumberType == NumberType.None) return new Number();

            if (character.Variables.GetFloat(value.IntValue, true, out float result)) return new Number(result);

            if (KeepLog)
                Debug.Log("Error | SysFVar");

            return new Number();
        }

        public static Node Parse(ParseState parsestate)
        {
            return parsestate.BuildParenNumberNode(true);
        }
    }

    [CustomFunction("SysVar")]
    class SysVar : Function
    {
        public SysVar(List<IFunction> children, List<System.Object> arguments)
            : base(children, arguments)
        {
        }
        public override Number Evaluate(Character character)
        {
            if (character == null || Children.Count != 1) return new Number();

            Number value = Children[0].Evaluate(character);
            if (value.NumberType == NumberType.None) return new Number();

            if (character.Variables.GetInteger(value.IntValue, true, out int result)) return new Number(result);

            if (KeepLog)
                Debug.Log("Error | SysFVar");

            return new Number();
        }

        public static Node Parse(ParseState parsestate)
        {
            return parsestate.BuildParenNumberNode(true);
        }
    }

    [CustomFunction("Tan")]
    class Tan : Function
    {
        public Tan(List<IFunction> children, List<System.Object> arguments)
            : base(children, arguments)
        {
        }
        public override Number Evaluate(Character character)
        {
            if (Children.Count != 1) return new Number();

            Number number = Children[0].Evaluate(character);
            if (number.NumberType == NumberType.None) return new Number();

            return new Number(Math.Tan(number.FloatValue));
        }

        public static Node Parse(ParseState state)
        {
            return state.BuildParenNumberNode(true);
        }
    }

    [CustomFunction("TeamMode")]
    class TeamMode : Function
    {
        public TeamMode(List<IFunction> children, List<System.Object> arguments)
            : base(children, arguments)
        {
        }
        public override Number Evaluate(Character character)
        {
            if (character == null || Arguments.Count != 2) return new Number();

            Operator @operator = (Operator)Arguments[0];
            string text = (string)Arguments[1];

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

                    return new Number();
            }

            switch (@operator)
            {
                case Operator.Equals:
                    return new Number(result);

                case Operator.NotEquals:
                    return new Number(!result);

                default:
                    if (KeepLog)
                        Debug.Log("Error | TeamMode");

                    return new Number();
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
    class TeamSide : Function
    {
        public TeamSide(List<IFunction> children, List<System.Object> arguments)
            : base(children, arguments)
        {
        }
        public override Number Evaluate(Character character)
        {
            if (character == null) return new Number();

            switch (character.Team.Side)
            {
                case UnityMugen.TeamSide.Left:
                    return new Number(1);

                case UnityMugen.TeamSide.Right:
                    return new Number(2);

                default:
                    if (KeepLog)
                        Debug.Log("Error | TeamSide");

                    return new Number();
            }
        }

        public static Node Parse(ParseState parsestate)
        {
            return parsestate.BaseNode;
        }
    }

    [CustomFunction("TicksPerSecond")]
    class TicksPerSecond : Function
    {
        public TicksPerSecond(List<IFunction> children, List<System.Object> arguments)
               : base(children, arguments)
        {
        }
        public override Number Evaluate(Character character)
        {
            if (character == null) return new Number();

            return new Number(LauncherEngine.Inst.initializationSettings.GameSpeed);
        }

        public static Node Parse(ParseState parsestate)
        {
            return parsestate.BaseNode;
        }
    }

    [CustomFunction("Time")]
    class Time : Function
    {
        public Time(List<IFunction> children, List<System.Object> arguments)
               : base(children, arguments)
        {
        }
        public override Number Evaluate(Character character)
        {
            if (character == null) return new Number();

            var time = character.StateManager.StateTime;
            if (time < 0) time = 0;

            return new Number(time);
        }

        public static Node Parse(ParseState parsestate)
        {
            return parsestate.BaseNode;
        }
    }

    [CustomFunction("StateTime")]
    class StateTime : Function
    {
#warning isso nao é usado mais acho que futuramente vou apagar o Time(Character character) para usar esse
        public StateTime(List<IFunction> children, List<System.Object> arguments)
               : base(children, arguments)
        {
        }
        public override Number Evaluate(Character character)
        {
            if (character == null) return new Number();

            var time = character.StateManager.StateTime;
            if (time < 0) time = 0;

            return new Number(time);
        }

        public static Node Parse(ParseState parsestate)
        {
            return parsestate.BaseNode;
        }
    }

    [CustomFunction("TimeMod")]
    class TimeMod : Function
    {
        public TimeMod(List<IFunction> children, List<System.Object> arguments)
               : base(children, arguments)
        {
        }
        public override Number Evaluate(Character character)
        {
            if (character == null || Children.Count != 2) return new Number();

            Number r1 = Children[0].Evaluate(character);
            Number r2 = Children[1].Evaluate(character);
            Operator compareType = (Operator)Arguments[0];
            if (r1.NumberType != NumberType.Int || r2.NumberType != NumberType.Int) return new Number();

            var statetimeRemander = character.StateManager.StateTime % r1.IntValue;
            //return statetimeRemander == r2; // Modificado Tiago

            bool result = SpecialFunctions.LogicalOperation(compareType, statetimeRemander, r2.IntValue);// Modificado Tiago
            return new Number(result);// Modificado Tiago
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

            var child2 = parsestate.BuildNode(false);
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
    class UniqHitCount : Function
    {
        public UniqHitCount(List<IFunction> children, List<System.Object> arguments)
               : base(children, arguments)
        {
        }
        public override Number Evaluate(Character character)
        {
            if (character == null) return new Number();

            return new Number(character.OffensiveInfo.UniqueHitCount);
        }

        public static Node Parse(ParseState parsestate)
        {
            return parsestate.BaseNode;
        }
    }

    [CustomFunction("Var")]
    class Var : Function
    {
        public Var(List<IFunction> children, List<System.Object> arguments)
            : base(children, arguments)
        {
        }
        public override Number Evaluate(Character character)
        {
            if (character == null || Children.Count != 1) return new Number();

            Number value = Children[0].Evaluate(character);
            if (value.NumberType == NumberType.None) return new Number();

            if (character.Variables.GetInteger(value.IntValue, false, out int result))
                return new Number(result);

            if (KeepLog)
                Debug.Log("Error | Var");

            return new Number();
        }

        public static Node Parse(ParseState parsestate)
        {
            return parsestate.BuildParenNumberNode(true);
        }
    }

    [CustomFunction("Vel")]
    internal class Vel : Function
    {
        public Vel(List<IFunction> children, List<System.Object> arguments)
            : base(children, arguments)
        {

        }
        public override Number Evaluate(Character character)
        {
            Axis axis = (Axis)Arguments[0];

            switch (axis)
            {
                case Axis.X:
                    return new Number(character.CurrentVelocity.x * Constant.Scale2);

                case Axis.Y:
                    return new Number(character.CurrentVelocity.y * Constant.Scale2);

                default:
                    if (KeepLog)
                        Debug.Log("Error | Vel");

                    return new Number();
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
    class Win : Function
    {
        public Win(List<IFunction> children, List<System.Object> arguments)
               : base(children, arguments)
        {
        }
        public override Number Evaluate(Character character)
        {
            if (character == null) return new Number();

            return new Number(character.Team.VictoryStatus.Win);
        }

        public static Node Parse(ParseState parsestate)
        {
            return parsestate.BaseNode;
        }
    }

    [CustomFunction("WinKO")]
    class WinKO : Function
    {
        public WinKO(List<IFunction> children, List<System.Object> arguments)
               : base(children, arguments)
        {
        }
        public override Number Evaluate(Character character)
        {
            if (character == null) return new Number();

            return new Number(character.Team.VictoryStatus.WinKO);
        }

        public static Node Parse(ParseState parsestate)
        {
            return parsestate.BaseNode;
        }
    }

    [CustomFunction("WinTime")]
    class WinTime : Function
    {
        public WinTime(List<IFunction> children, List<System.Object> arguments)
            : base(children, arguments)
        {
        }
        public override Number Evaluate(Character character)
        {
            if (character == null) return new Number();

            return new Number(character.Team.VictoryStatus.WinTime);
        }

        public static Node Parse(ParseState parsestate)
        {
            return parsestate.BaseNode;
        }
    }

    [CustomFunction("WinPerfect")]
    class WinPerfect : Function
    {
        public WinPerfect(List<IFunction> children, List<System.Object> arguments)
               : base(children, arguments)
        {
        }
        public override Number Evaluate(Character character)
        {
            if (character == null) return new Number();

            return new Number(character.Team.VictoryStatus.WinPerfect);
        }

        public static Node Parse(ParseState parsestate)
        {
            return parsestate.BaseNode;
        }
    }



#warning falta testar
    [CustomFunction("ScreenHeight")]
    class ScreenHeight : Function
    {
        public ScreenHeight(List<IFunction> children, List<System.Object> arguments)
                  : base(children, arguments)
        {
        }
        public override Number Evaluate(Character character)
        {
            if (character == null) return new Number();

            return new Number(character.Engine.CameraFE.ConverterBound().height * Constant.Scale2);
        }

        public static Node Parse(ParseState state)
        {
            return state.BaseNode;
        }
    }

#warning falta testar
    [CustomFunction("ScreenWidth")]
    class ScreenWidth : Function
    {
        public ScreenWidth(List<IFunction> children, List<System.Object> arguments)
                     : base(children, arguments)
        {
        }
        public override Number Evaluate(Character character)
        {
            if (character == null) return new Number();

            return new Number(character.Engine.CameraFE.ConverterBound().width * Constant.Scale2);
        }

        public static Node Parse(ParseState state)
        {
            return state.BaseNode;
        }
    }

    [CustomFunction("GameHeight")]
    class GameHeight : Function
    {
        public GameHeight(List<IFunction> children, List<System.Object> arguments)
                     : base(children, arguments)
        {
        }
        public override Number Evaluate(Character character)
        {
            if (character == null) return new Number();
            //return character.Engine.CameraFE.CameraBounds.up - character.Engine.CameraFE.CameraBounds.down;
            //return Screen.height;
            return new Number(character.Engine.CameraFE.ConverterBound().height * Constant.Scale2);
        }

        public static Node Parse(ParseState state)
        {
            return state.BaseNode;
        }
    }

    [CustomFunction("GameWidth")]
    class GameWidth : Function
    {
        public GameWidth(List<IFunction> children, List<System.Object> arguments)
                     : base(children, arguments)
        {
        }
        public override Number Evaluate(Character character)
        {
            if (character == null) return new Number();
            //return character.Engine.CameraFE.CameraBounds.right - character.Engine.CameraFE.CameraBounds.left;
            //return Screen.width;
            return new Number(character.Engine.CameraFE.ConverterBound().width * Constant.Scale2);
        }

        public static Node Parse(ParseState state)
        {
            return state.BaseNode;
        }
    }

    [CustomFunction("TopEdge")]
    class TopEdge : Function
    {
        public TopEdge(List<IFunction> children, List<System.Object> arguments)
                     : base(children, arguments)
        {
        }
        public override Number Evaluate(Character character)
        {
            if (character == null) return new Number();

#warning não tenho certeza sobre usar yMax, analizar
            // Nao preciso mecher com scale pois Pos e ScreenPos já possuem seus valores alterados pos Constant.Scale2
            // return TE.Pos(character, Axis.Y) - character.ScreenPos(Axis.Y);
            return new Number(character.Engine.CameraFE.ConverterBound().yMin * Constant.Scale2);
        }

        public static Node Parse(ParseState state)
        {
            return state.BaseNode;
        }
    }

    [CustomFunction("BottomEdge")]
    class BottomEdge : Function
    {
        public BottomEdge(List<IFunction> children, List<System.Object> arguments)
                     : base(children, arguments)
        {
        }
        public override Number Evaluate(Character character)
        {
            if (character == null) return new Number();

            // Nao preciso mecher com scale pois Pos e ScreenPos já possuem seus valores alterados pos Constant.Scale2
            //return TE.Pos(character, Axis.Y) - TE.ScreenPos(character, Axis.Y) + TE.GameHeight();
#warning não tenho certeza sobre usar yMax, analizar
            return new Number(character.Engine.CameraFE.ConverterBound().yMax * Constant.Scale2);
        }

        public static Node Parse(ParseState state)
        {
            return state.BaseNode;
        }
    }

    [CustomFunction("RightEdge")]
    class RightEdge : Function
    {
        public RightEdge(List<IFunction> children, List<System.Object> arguments)
                     : base(children, arguments)
        {
        }
        public override Number Evaluate(Character character)
        {
            if (character == null) return new Number();

#warning não tenho certeza sobre usar yMin, analizar
            return new Number(character.Engine.CameraFE.ConverterBound().xMax * Constant.Scale2);
        }

        public static Node Parse(ParseState state)
        {
            return state.BaseNode;
        }
    }

    [CustomFunction("LeftEdge")]
    class LeftEdge : Function
    {
        public LeftEdge(List<IFunction> children, List<System.Object> arguments)
                     : base(children, arguments)
        {
        }
        public override Number Evaluate(Character character)
        {
            if (character == null) return new Number();

            return new Number(character.Engine.CameraFE.ConverterBound().xMin * Constant.Scale2);
        }

        public static Node Parse(ParseState state)
        {
            return state.BaseNode;
        }
    }

    [CustomFunction("FrontEdge")]
    class FrontEdge : Function
    {
        public FrontEdge(List<IFunction> children, List<System.Object> arguments)
                     : base(children, arguments)
        {
        }
        public override Number Evaluate(Character character)
        {
            if (character == null) return new Number();

            if (character.CurrentFacing == UnityMugen.Facing.Left)
                return new Number(character.Engine.CameraFE.ConverterBound().xMin * Constant.Scale2);
            else
                return new Number(character.Engine.CameraFE.ConverterBound().xMax * Constant.Scale2);
        }

        public static Node Parse(ParseState state)
        {
            return state.BaseNode;
        }
    }

    [CustomFunction("BackEdge")]
    class BackEdge : Function
    {
        public BackEdge(List<IFunction> children, List<System.Object> arguments)
                     : base(children, arguments)
        {
        }
        public override Number Evaluate(Character character)
        {
            if (character == null) return new Number();
            //return character.CurrentFacing == UnityMugen.Facing.Right ? TE.LeftEdge() : TE.RightEdge();
            if (character.CurrentFacing == UnityMugen.Facing.Left)
                return new Number(character.Engine.CameraFE.ConverterBound().xMax * Constant.Scale2);
            else
                return new Number(character.Engine.CameraFE.ConverterBound().xMin * Constant.Scale2);
        }

        public static Node Parse(ParseState state)
        {
            return state.BaseNode;
        }
    }

    [CustomFunction("WinHyper")]
    class WinHyper : Function
    {
        public WinHyper(List<IFunction> children, List<System.Object> arguments)
                     : base(children, arguments)
        {
        }
        public override Number Evaluate(Character character)
        {
            if (character == null) return new Number();

            return new Number(character.Team.VictoryStatus.WinHyper);
        }

        public static Node Parse(ParseState parsestate)
        {
            return parsestate.BaseNode;
        }
    }

    [CustomFunction("WinSpecial")]
    class WinSpecial : Function
    {
        public WinSpecial(List<IFunction> children, List<System.Object> arguments)
                     : base(children, arguments)
        {
        }
        public override Number Evaluate(Character character)
        {
            if (character == null) return new Number();

            return new Number(character.Team.VictoryStatus.WinSpecial);
        }

        public static Node Parse(ParseState parsestate)
        {
            return parsestate.BaseNode;
        }
    }

    [CustomFunction("ComboCounter")]
    class ComboCounter : Function
    {
        public ComboCounter(List<IFunction> children, List<System.Object> arguments)
                     : base(children, arguments)
        {
        }
        public override Number Evaluate(Character character)
        {
            if (character == null) return new Number();

            return new Number(character.Team.ComboCounter.HitCount);
        }

        public static Node Parse(ParseState state)
        {
            return state.BaseNode;
        }
    }

    [CustomFunction("ReceivedDamage")]
    class ReceivedDamage : Function
    {
        public ReceivedDamage(List<IFunction> children, List<System.Object> arguments)
                     : base(children, arguments)
        {
        }
        public override Number Evaluate(Character character)
        {
            if (character == null) return new Number();

            foreach (Contact contact in character.Engine.m_combatcheck.Attacks)
            {
                return new Number(character.Engine.m_combatcheck.ReceivedDamage(character));
                //if (contact.Type == ContactType.Hit) return true;
            }
            return new Number(false);
        }

        public static Node Parse(ParseState state)
        {
            return state.BaseNode;
        }
    }







    [CustomFunction("CombatMode")]
    class CombatMode : Function
    {
        public CombatMode(List<IFunction> children, List<System.Object> arguments)
                     : base(children, arguments)
        {
        }
        public override Number Evaluate(Character character)
        {
            if (character == null || Arguments.Count != 2) return new Number();

            Operator @operator = (Operator)Arguments[0];
            string text = (string)Arguments[1];

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

                    return new Number();
            }

            switch (@operator)
            {
                case Operator.Equals:
                    return new Number(result);

                case Operator.NotEquals:
                    return new Number(!result);

                default:
                    if (KeepLog)
                        Debug.Log("Error | CombatMode");

                    return new Number();
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
    class StanceType : Function
    {
        public StanceType(List<IFunction> children, List<System.Object> arguments)
            : base(children, arguments)
        {
        }
        public override Number Evaluate(Character character)
        {
            if (character == null || Arguments.Count != 2) return new Number();

            Operator @operator = (Operator)Arguments[0];
            string text = (string)Arguments[1];

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

                    return new Number();
            }

            switch (@operator)
            {
                case Operator.Equals:
                    return new Number(result);

                case Operator.NotEquals:
                    return new Number(!result);

                default:
                    if (KeepLog)
                        Debug.Log("Error | StanceType");

                    return new Number();
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