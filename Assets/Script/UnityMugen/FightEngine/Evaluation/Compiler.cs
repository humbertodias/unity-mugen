using System;
using System.Collections.Generic;
using System.Diagnostics;
using UnityMugen.Collections;

namespace UnityMugen.Evaluation
{
    public class Compiler
    {
        public Compiler()
        {
            m_numbercache = new KeyedCollection<Number, NumberReturn>(x => x.Evaluate(null));
        }

        public IFunction BuildNode(Node node)
        {
            if (node == null) throw new ArgumentNullException("node");

            if (node.Token.Data is NumberData)
            {
                Number number = (node.Token.Data as NumberData).GetNumber(node.Token.ToString());

                if (m_numbercache.Contains(number) == true) return m_numbercache[number];

                NumberReturn @return = new NumberReturn(number);
                m_numbercache.Add(@return);

                return @return;
            }

            List<IFunction> children = new List<IFunction>();
            foreach (Node child in node.Children) children.Add(BuildNode(child));

            if (node.Token.Data is RangeData) 
				return new Operations.Range(children, node.Arguments);

            return CreateCallBack(GetFunctionName(node), children, node.Arguments);
        }

        static string GetFunctionName(Node node)
        {
            if (node == null) throw new ArgumentNullException("node");

            NodeData data = node.Token.Data as NodeData;
            return (data != null) ? data.Name : string.Empty;
        }

        static IFunction CreateCallBack(string name, List<IFunction> children, List<System.Object> arguments)
        {
            if (name == null) throw new ArgumentNullException("name");
            if (children == null) throw new ArgumentNullException("children");
            if (arguments == null) throw new ArgumentNullException("arguments");

			switch (name)
			{
				case "UnityMugen.Evaluation.Operations.LogicOr":
					return new UnityMugen.Evaluation.Operations.LogicOr(children, arguments);

				case "UnityMugen.Evaluation.Operations.LogicXor":
					return new UnityMugen.Evaluation.Operations.LogicXor(children, arguments);

				case "UnityMugen.Evaluation.Operations.LogicAnd":
					return new UnityMugen.Evaluation.Operations.LogicAnd(children, arguments);

				case "UnityMugen.Evaluation.Operations.BitOr":
					return new UnityMugen.Evaluation.Operations.BitOr(children, arguments);

				case "UnityMugen.Evaluation.Operations.BitXor":
					return new UnityMugen.Evaluation.Operations.BitXor(children, arguments);

				case "UnityMugen.Evaluation.Operations.BitAnd":
					return new UnityMugen.Evaluation.Operations.BitAnd(children, arguments);

				case "UnityMugen.Evaluation.Operations.Equality":
					return new UnityMugen.Evaluation.Operations.Equality(children, arguments);

				case "UnityMugen.Evaluation.Operations.Inequality":
					return new UnityMugen.Evaluation.Operations.Inequality(children, arguments);

				case "UnityMugen.Evaluation.Operations.LesserThan":
					return new UnityMugen.Evaluation.Operations.LesserThan(children, arguments);

				case "UnityMugen.Evaluation.Operations.LesserEquals":
					return new UnityMugen.Evaluation.Operations.LesserEquals(children, arguments);

				case "UnityMugen.Evaluation.Operations.GreaterThan":
					return new UnityMugen.Evaluation.Operations.GreaterThan(children, arguments);

				case "UnityMugen.Evaluation.Operations.GreaterEquals":
					return new UnityMugen.Evaluation.Operations.GreaterEquals(children, arguments);

				case "UnityMugen.Evaluation.Operations.Addition":
					return new UnityMugen.Evaluation.Operations.Addition(children, arguments);

				case "UnityMugen.Evaluation.Operations.Substraction":
					return new UnityMugen.Evaluation.Operations.Substraction(children, arguments);

				case "UnityMugen.Evaluation.Operations.Division":
					return new UnityMugen.Evaluation.Operations.Division(children, arguments);

				case "UnityMugen.Evaluation.Operations.Multiplication":
					return new UnityMugen.Evaluation.Operations.Multiplication(children, arguments);

				case "UnityMugen.Evaluation.Operations.Modulus":
					return new UnityMugen.Evaluation.Operations.Modulus(children, arguments);

				case "UnityMugen.Evaluation.Operations.Exponent":
					return new UnityMugen.Evaluation.Operations.Exponent(children, arguments);

				case "UnityMugen.Evaluation.Operations.Null":
					return new UnityMugen.Evaluation.Operations.Null(children, arguments);

				case "UnityMugen.Evaluation.Operations.LogicNot":
					return new UnityMugen.Evaluation.Operations.LogicNot(children, arguments);

				case "UnityMugen.Evaluation.Operations.BitNot":
					return new UnityMugen.Evaluation.Operations.BitNot(children, arguments);


				/////

				case "UnityMugen.Evaluation.Triggers.Const240p":
					return new UnityMugen.Evaluation.Triggers.Const240p(children, arguments);

				case "UnityMugen.Evaluation.Triggers.Const480p":
					return new UnityMugen.Evaluation.Triggers.Const480p(children, arguments);

				case "UnityMugen.Evaluation.Triggers.Const720p":
					return new UnityMugen.Evaluation.Triggers.Const720p(children, arguments);

				case "UnityMugen.Evaluation.Triggers.StanceType":
					return new UnityMugen.Evaluation.Triggers.StanceType(children, arguments);

				case "UnityMugen.Evaluation.Triggers.AiLevel":
					return new UnityMugen.Evaluation.Triggers.AiLevel(children, arguments);

				case "UnityMugen.Evaluation.Triggers.Cond":
					return new UnityMugen.Evaluation.Triggers.Cond(children, arguments);

				case "UnityMugen.Evaluation.Triggers.GameHeight":
					return new UnityMugen.Evaluation.Triggers.GameHeight(children, arguments);

				case "UnityMugen.Evaluation.Triggers.GameWidth":
					return new UnityMugen.Evaluation.Triggers.GameWidth(children, arguments);

				case "UnityMugen.Evaluation.Triggers.CameraPos":
					return new UnityMugen.Evaluation.Triggers.CameraPos(children, arguments);

				case "UnityMugen.Evaluation.Triggers.NumGraphicUI":
					return new UnityMugen.Evaluation.Triggers.NumGraphicUI(children, arguments);

				case "UnityMugen.Evaluation.Triggers.StageVar":
					return new UnityMugen.Evaluation.Triggers.StageVar(children, arguments);

				case "UnityMugen.Evaluation.Triggers.ScreenHeight":
					return new UnityMugen.Evaluation.Triggers.ScreenHeight(children, arguments);

				case "UnityMugen.Evaluation.Triggers.ScreenWidth":
					return new UnityMugen.Evaluation.Triggers.ScreenWidth(children, arguments);

				case "UnityMugen.Evaluation.Triggers.TopEdge":
					return new UnityMugen.Evaluation.Triggers.TopEdge(children, arguments);

				case "UnityMugen.Evaluation.Triggers.BottomEdge":
					return new UnityMugen.Evaluation.Triggers.BottomEdge(children, arguments);

				case "UnityMugen.Evaluation.Triggers.RightEdge":
					return new UnityMugen.Evaluation.Triggers.RightEdge(children, arguments);

				case "UnityMugen.Evaluation.Triggers.LeftEdge":
					return new UnityMugen.Evaluation.Triggers.LeftEdge(children, arguments);

				case "UnityMugen.Evaluation.Triggers.FrontEdge":
					return new UnityMugen.Evaluation.Triggers.FrontEdge(children, arguments);

				case "UnityMugen.Evaluation.Triggers.BackEdge":
					return new UnityMugen.Evaluation.Triggers.BackEdge(children, arguments);

				case "UnityMugen.Evaluation.Triggers.WinHyper":
					return new UnityMugen.Evaluation.Triggers.WinHyper(children, arguments);

				case "UnityMugen.Evaluation.Triggers.WinSpecial":
					return new UnityMugen.Evaluation.Triggers.WinSpecial(children, arguments);

				case "UnityMugen.Evaluation.Triggers.ComboCounter":
					return new UnityMugen.Evaluation.Triggers.ComboCounter(children, arguments);

				case "UnityMugen.Evaluation.Triggers.ReceivedDamage":
					return new UnityMugen.Evaluation.Triggers.ReceivedDamage(children, arguments);

				case "UnityMugen.Evaluation.Triggers.CombatMode":
					return new UnityMugen.Evaluation.Triggers.CombatMode(children, arguments);

				case "UnityMugen.Evaluation.Triggers.CameraZoom":
					return new UnityMugen.Evaluation.Triggers.CameraZoom(children, arguments);

				/////

				case "UnityMugen.Evaluation.Triggers.Vel":
					return new UnityMugen.Evaluation.Triggers.Vel(children, arguments);

				case "UnityMugen.Evaluation.Triggers.UniqHitCount":
					return new UnityMugen.Evaluation.Triggers.UniqHitCount(children, arguments);

				case "UnityMugen.Evaluation.Triggers.TimeMod":
					return new UnityMugen.Evaluation.Triggers.TimeMod(children, arguments);

				case "UnityMugen.Evaluation.Triggers.ProjCancelTime":
					return new UnityMugen.Evaluation.Triggers.ProjCancelTime(children, arguments);

				case "UnityMugen.Evaluation.Triggers.Win":
					return new UnityMugen.Evaluation.Triggers.Win(children, arguments);

				case "UnityMugen.Evaluation.Triggers.WinKO":
					return new UnityMugen.Evaluation.Triggers.WinKO(children, arguments);

				case "UnityMugen.Evaluation.Triggers.WinTime":
					return new UnityMugen.Evaluation.Triggers.WinTime(children, arguments);

				case "UnityMugen.Evaluation.Triggers.WinPerfect":
					return new UnityMugen.Evaluation.Triggers.WinPerfect(children, arguments);

				case "UnityMugen.Evaluation.Triggers.ProjHitTime":
					return new UnityMugen.Evaluation.Triggers.ProjHitTime(children, arguments);

				case "UnityMugen.Evaluation.Triggers.P2Dist":
					return new UnityMugen.Evaluation.Triggers.P2Dist(children, arguments);

				case "UnityMugen.Evaluation.Triggers.P2BodyDist":
					return new UnityMugen.Evaluation.Triggers.P2BodyDist(children, arguments);

				case "UnityMugen.Evaluation.Triggers.NumEnemy":
					return new UnityMugen.Evaluation.Triggers.NumEnemy(children, arguments);

				case "UnityMugen.Evaluation.Triggers.AnimTime":
					return new UnityMugen.Evaluation.Triggers.AnimTime(children, arguments);

				case "UnityMugen.Evaluation.Triggers.PalNo":
					return new UnityMugen.Evaluation.Triggers.PalNo(children, arguments);

				case "UnityMugen.Evaluation.Triggers.P2StateType":
					return new UnityMugen.Evaluation.Triggers.P2StateType(children, arguments);

				case "UnityMugen.Evaluation.Triggers.P2MoveType":
					return new UnityMugen.Evaluation.Triggers.P2MoveType(children, arguments);

				case "UnityMugen.Evaluation.Triggers.P1Name":
					return new UnityMugen.Evaluation.Triggers.P1Name(children, arguments);

				case "UnityMugen.Evaluation.Triggers.HitFall":
					return new UnityMugen.Evaluation.Triggers.HitFall(children, arguments);

				case "UnityMugen.Evaluation.Triggers.AnimElemNo":
					return new UnityMugen.Evaluation.Triggers.AnimElemNo(children, arguments);

				case "UnityMugen.Evaluation.Triggers.P4Name":
					return new UnityMugen.Evaluation.Triggers.P4Name(children, arguments);

				case "UnityMugen.Evaluation.Triggers.MoveType":
					return new UnityMugen.Evaluation.Triggers.MoveType(children, arguments);

				case "UnityMugen.Evaluation.Triggers.LifeMax":
					return new UnityMugen.Evaluation.Triggers.LifeMax(children, arguments);

				case "UnityMugen.Evaluation.Triggers.ID":
					return new UnityMugen.Evaluation.Triggers.ID(children, arguments);

				case "UnityMugen.Evaluation.Triggers.GetHitVar":
					return new UnityMugen.Evaluation.Triggers.GetHitVar(children, arguments);

				case "UnityMugen.Evaluation.Triggers.Ctrl":
					return new UnityMugen.Evaluation.Triggers.Ctrl(children, arguments);

				case "UnityMugen.Evaluation.Triggers.RoundNo":
					return new UnityMugen.Evaluation.Triggers.RoundNo(children, arguments);

				case "UnityMugen.Evaluation.Triggers.ProjContact":
					return new UnityMugen.Evaluation.Triggers.ProjContact(children, arguments);

				case "UnityMugen.Evaluation.Triggers.PrevStateNo":
					return new UnityMugen.Evaluation.Triggers.PrevStateNo(children, arguments);

				case "UnityMugen.Evaluation.Triggers.HitPauseTime":
					return new UnityMugen.Evaluation.Triggers.HitPauseTime(children, arguments);

				case "UnityMugen.Evaluation.Triggers.AnimElemTime":
					return new UnityMugen.Evaluation.Triggers.AnimElemTime(children, arguments);

				case "UnityMugen.Evaluation.Triggers.TeamSide":
					return new UnityMugen.Evaluation.Triggers.TeamSide(children, arguments);

				case "UnityMugen.Evaluation.Triggers.Sin":
					return new UnityMugen.Evaluation.Triggers.Sin(children, arguments);

				case "UnityMugen.Evaluation.Triggers.Redirection.Parent":
					return new UnityMugen.Evaluation.Triggers.Redirection.Parent(children, arguments);

				case "UnityMugen.Evaluation.Triggers.Redirection.Root":
					return new UnityMugen.Evaluation.Triggers.Redirection.Root(children, arguments);

				case "UnityMugen.Evaluation.Triggers.Redirection.Helper":
					return new UnityMugen.Evaluation.Triggers.Redirection.Helper(children, arguments);

				case "UnityMugen.Evaluation.Triggers.Redirection.Target":
					return new UnityMugen.Evaluation.Triggers.Redirection.Target(children, arguments);

				case "UnityMugen.Evaluation.Triggers.Redirection.Partner":
					return new UnityMugen.Evaluation.Triggers.Redirection.Partner(children, arguments);

				case "UnityMugen.Evaluation.Triggers.Redirection.Enemy":
					return new UnityMugen.Evaluation.Triggers.Redirection.Enemy(children, arguments);

				case "UnityMugen.Evaluation.Triggers.Redirection.EnemyNear":
					return new UnityMugen.Evaluation.Triggers.Redirection.EnemyNear(children, arguments);

				case "UnityMugen.Evaluation.Triggers.Redirection.PlayerID":
					return new UnityMugen.Evaluation.Triggers.Redirection.PlayerID(children, arguments);

				case "UnityMugen.Evaluation.Triggers.Random":
					return new UnityMugen.Evaluation.Triggers.Random(children, arguments);

				case "UnityMugen.Evaluation.Triggers.Pi":
					return new UnityMugen.Evaluation.Triggers.Pi(children, arguments);

				case "UnityMugen.Evaluation.Triggers.NumProjID":
					return new UnityMugen.Evaluation.Triggers.NumProjID(children, arguments);

				case "UnityMugen.Evaluation.Triggers.Lose":
					return new UnityMugen.Evaluation.Triggers.Lose(children, arguments);

				case "UnityMugen.Evaluation.Triggers.LoseKO":
					return new UnityMugen.Evaluation.Triggers.LoseKO(children, arguments);

				case "UnityMugen.Evaluation.Triggers.LoseTime":
					return new UnityMugen.Evaluation.Triggers.LoseTime(children, arguments);

				case "UnityMugen.Evaluation.Triggers.IsHelper":
					return new UnityMugen.Evaluation.Triggers.IsHelper(children, arguments);

				case "UnityMugen.Evaluation.Triggers.HitDefAttr":
					return new UnityMugen.Evaluation.Triggers.HitDefAttr(children, arguments);

				case "UnityMugen.Evaluation.Triggers.Facing":
					return new UnityMugen.Evaluation.Triggers.Facing(children, arguments);

				case "UnityMugen.Evaluation.Triggers.Cos":
					return new UnityMugen.Evaluation.Triggers.Cos(children, arguments);

				case "UnityMugen.Evaluation.Triggers.Assertion":
					return new UnityMugen.Evaluation.Triggers.Assertion(children, arguments);

				case "UnityMugen.Evaluation.Triggers.AnimElem":
					return new UnityMugen.Evaluation.Triggers.AnimElem(children, arguments);

				case "UnityMugen.Evaluation.Triggers.TicksPerSecond":
					return new UnityMugen.Evaluation.Triggers.TicksPerSecond(children, arguments);

				case "UnityMugen.Evaluation.Triggers.ProjGuarded":
					return new UnityMugen.Evaluation.Triggers.ProjGuarded(children, arguments);

				case "UnityMugen.Evaluation.Triggers.Power":
					return new UnityMugen.Evaluation.Triggers.Power(children, arguments);

				case "UnityMugen.Evaluation.Triggers.HitShakeOver":
					return new UnityMugen.Evaluation.Triggers.HitShakeOver(children, arguments);

				case "UnityMugen.Evaluation.Triggers.Var":
					return new UnityMugen.Evaluation.Triggers.Var(children, arguments);

				case "UnityMugen.Evaluation.Triggers.SelfAnimExist":
					return new UnityMugen.Evaluation.Triggers.SelfAnimExist(children, arguments);

				case "UnityMugen.Evaluation.Triggers.ParentDist":
					return new UnityMugen.Evaluation.Triggers.ParentDist(children, arguments);

				case "UnityMugen.Evaluation.Triggers.IsHomeTeam":
					return new UnityMugen.Evaluation.Triggers.IsHomeTeam(children, arguments);

				case "UnityMugen.Evaluation.Triggers.DrawGame":
					return new UnityMugen.Evaluation.Triggers.DrawGame(children, arguments);

				case "UnityMugen.Evaluation.Triggers.SysFVar":
					return new UnityMugen.Evaluation.Triggers.SysFVar(children, arguments);

				case "UnityMugen.Evaluation.Triggers.RoundState":
					return new UnityMugen.Evaluation.Triggers.RoundState(children, arguments);

				case "UnityMugen.Evaluation.Triggers.P2Name":
					return new UnityMugen.Evaluation.Triggers.P2Name(children, arguments);

				case "UnityMugen.Evaluation.Triggers.P2Life":
					return new UnityMugen.Evaluation.Triggers.P2Life(children, arguments);

				case "UnityMugen.Evaluation.Triggers.Log":
					return new UnityMugen.Evaluation.Triggers.Log(children, arguments);

				case "UnityMugen.Evaluation.Triggers.HitOver":
					return new UnityMugen.Evaluation.Triggers.HitOver(children, arguments);

				case "UnityMugen.Evaluation.Triggers.Asin":
					return new UnityMugen.Evaluation.Triggers.Asin(children, arguments);

				case "UnityMugen.Evaluation.Triggers.Alive":
					return new UnityMugen.Evaluation.Triggers.Alive(children, arguments);

				case "UnityMugen.Evaluation.Triggers.Acos":
					return new UnityMugen.Evaluation.Triggers.Acos(children, arguments);

				case "UnityMugen.Evaluation.Triggers.Pos":
					return new UnityMugen.Evaluation.Triggers.Pos(children, arguments);

				case "UnityMugen.Evaluation.Triggers.NumTarget":
					return new UnityMugen.Evaluation.Triggers.NumTarget(children, arguments);

				case "UnityMugen.Evaluation.Triggers.Ceil":
					return new UnityMugen.Evaluation.Triggers.Ceil(children, arguments);

				case "UnityMugen.Evaluation.Triggers.StateNo":
					return new UnityMugen.Evaluation.Triggers.StateNo(children, arguments);

				case "UnityMugen.Evaluation.Triggers.NumProj":
					return new UnityMugen.Evaluation.Triggers.NumProj(children, arguments);

				case "UnityMugen.Evaluation.Triggers.MatchNo":
					return new UnityMugen.Evaluation.Triggers.MatchNo(children, arguments);

				case "UnityMugen.Evaluation.Triggers.Time":
					return new UnityMugen.Evaluation.Triggers.Time(children, arguments);

				case "UnityMugen.Evaluation.Triggers.StateTime":
					return new UnityMugen.Evaluation.Triggers.StateTime(children, arguments);

				case "UnityMugen.Evaluation.Triggers.ScreenPos":
					return new UnityMugen.Evaluation.Triggers.ScreenPos(children, arguments);

				case "UnityMugen.Evaluation.Triggers.PowerMax":
					return new UnityMugen.Evaluation.Triggers.PowerMax(children, arguments);

				case "UnityMugen.Evaluation.Triggers.P2StateNo":
					return new UnityMugen.Evaluation.Triggers.P2StateNo(children, arguments);

				case "UnityMugen.Evaluation.Triggers.MoveReversed":
					return new UnityMugen.Evaluation.Triggers.MoveReversed(children, arguments);

				case "UnityMugen.Evaluation.Triggers.GameTime":
					return new UnityMugen.Evaluation.Triggers.GameTime(children, arguments);

				case "UnityMugen.Evaluation.Triggers.E":
					return new UnityMugen.Evaluation.Triggers.E(children, arguments);

				case "UnityMugen.Evaluation.Triggers.BackEdgeBodyDist":
					return new UnityMugen.Evaluation.Triggers.BackEdgeBodyDist(children, arguments);

				case "UnityMugen.Evaluation.Triggers.Abs":
					return new UnityMugen.Evaluation.Triggers.Abs(children, arguments);

				case "UnityMugen.Evaluation.Triggers.TeamMode":
					return new UnityMugen.Evaluation.Triggers.TeamMode(children, arguments);

				case "UnityMugen.Evaluation.Triggers.SysVar":
					return new UnityMugen.Evaluation.Triggers.SysVar(children, arguments);

				case "UnityMugen.Evaluation.Triggers.StateType":
					return new UnityMugen.Evaluation.Triggers.StateType(children, arguments);

				case "UnityMugen.Evaluation.Triggers.RoundsExisted":
					return new UnityMugen.Evaluation.Triggers.RoundsExisted(children, arguments);

				case "UnityMugen.Evaluation.Triggers.PlayerIDExist":
					return new UnityMugen.Evaluation.Triggers.PlayerIDExist(children, arguments);

				case "UnityMugen.Evaluation.Triggers.NumHelper":
					return new UnityMugen.Evaluation.Triggers.NumHelper(children, arguments);

				case "UnityMugen.Evaluation.Triggers.MoveContact":
					return new UnityMugen.Evaluation.Triggers.MoveContact(children, arguments);

				case "UnityMugen.Evaluation.Triggers.Floor":
					return new UnityMugen.Evaluation.Triggers.Floor(children, arguments);

				case "UnityMugen.Evaluation.Triggers.Tan":
					return new UnityMugen.Evaluation.Triggers.Tan(children, arguments);

				case "UnityMugen.Evaluation.Triggers.Ln":
					return new UnityMugen.Evaluation.Triggers.Ln(children, arguments);

				case "UnityMugen.Evaluation.Triggers.InGuardDist":
					return new UnityMugen.Evaluation.Triggers.InGuardDist(children, arguments);

				case "UnityMugen.Evaluation.Triggers.HitCount":
					return new UnityMugen.Evaluation.Triggers.HitCount(children, arguments);

				case "UnityMugen.Evaluation.Triggers.Command":
					return new UnityMugen.Evaluation.Triggers.Command(children, arguments);

				case "UnityMugen.Evaluation.Triggers.BackEdgeDist":
					return new UnityMugen.Evaluation.Triggers.BackEdgeDist(children, arguments);

				case "UnityMugen.Evaluation.Triggers.AuthorName":
					return new UnityMugen.Evaluation.Triggers.AuthorName(children, arguments);

				case "UnityMugen.Evaluation.Triggers.AnimExist":
					return new UnityMugen.Evaluation.Triggers.AnimExist(children, arguments);

				case "UnityMugen.Evaluation.Triggers.ProjGuardedTime":
					return new UnityMugen.Evaluation.Triggers.ProjGuardedTime(children, arguments);

				case "UnityMugen.Evaluation.Triggers.P3Name":
					return new UnityMugen.Evaluation.Triggers.P3Name(children, arguments);

				case "UnityMugen.Evaluation.Triggers.NumPartner":
					return new UnityMugen.Evaluation.Triggers.NumPartner(children, arguments);

				case "UnityMugen.Evaluation.Triggers.Name":
					return new UnityMugen.Evaluation.Triggers.Name(children, arguments);

				case "UnityMugen.Evaluation.Triggers.MoveGuarded":
					return new UnityMugen.Evaluation.Triggers.MoveGuarded(children, arguments);

				case "UnityMugen.Evaluation.Triggers.MatchOver":
					return new UnityMugen.Evaluation.Triggers.MatchOver(children, arguments);

				case "UnityMugen.Evaluation.Triggers.IfElse":
					return new UnityMugen.Evaluation.Triggers.IfElse(children, arguments);

				case "UnityMugen.Evaluation.Triggers.FVar":
					return new UnityMugen.Evaluation.Triggers.FVar(children, arguments);

				case "UnityMugen.Evaluation.Triggers.FrontEdgeDist":
					return new UnityMugen.Evaluation.Triggers.FrontEdgeDist(children, arguments);

				case "UnityMugen.Evaluation.Triggers.Anim":
					return new UnityMugen.Evaluation.Triggers.Anim(children, arguments);

				case "UnityMugen.Evaluation.Triggers.ProjContactTime":
					return new UnityMugen.Evaluation.Triggers.ProjContactTime(children, arguments);

				case "UnityMugen.Evaluation.Triggers.Life":
					return new UnityMugen.Evaluation.Triggers.Life(children, arguments);

				case "UnityMugen.Evaluation.Triggers.HitVel":
					return new UnityMugen.Evaluation.Triggers.HitVel(children, arguments);

				case "UnityMugen.Evaluation.Triggers.FrontEdgeBodyDist":
					return new UnityMugen.Evaluation.Triggers.FrontEdgeBodyDist(children, arguments);

				case "UnityMugen.Evaluation.Triggers.Exp":
					return new UnityMugen.Evaluation.Triggers.Exp(children, arguments);

				case "UnityMugen.Evaluation.Triggers.CanRecover":
					return new UnityMugen.Evaluation.Triggers.CanRecover(children, arguments);

				case "UnityMugen.Evaluation.Triggers.RootDist":
					return new UnityMugen.Evaluation.Triggers.RootDist(children, arguments);

				case "UnityMugen.Evaluation.Triggers.ProjHit":
					return new UnityMugen.Evaluation.Triggers.ProjHit(children, arguments);

				case "UnityMugen.Evaluation.Triggers.NumExplod":
					return new UnityMugen.Evaluation.Triggers.NumExplod(children, arguments);

				case "UnityMugen.Evaluation.Triggers.MoveHit":
					return new UnityMugen.Evaluation.Triggers.MoveHit(children, arguments);

				case "UnityMugen.Evaluation.Triggers.Const":
					return new UnityMugen.Evaluation.Triggers.Const(children, arguments);

				case "UnityMugen.Evaluation.Triggers.Atan":
					return new UnityMugen.Evaluation.Triggers.Atan(children, arguments);

				case "UnityMugen.Evaluation.Operations.Assignment":
					return new UnityMugen.Evaluation.Operations.Assignment(children, arguments);

				case "UnityMugen.Evaluation.Triggers.Physics":
					return new UnityMugen.Evaluation.Triggers.Physics(children, arguments);

				default:
					return new Operations.Null(children, arguments);
			}
        }

        #region Fields

        [DebuggerBrowsable(DebuggerBrowsableState.Never)]
        readonly KeyedCollection<Number, NumberReturn> m_numbercache;

        #endregion
    }

}