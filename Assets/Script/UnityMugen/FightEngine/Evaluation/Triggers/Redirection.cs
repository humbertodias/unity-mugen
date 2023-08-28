using System;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityMugen.Combat;

namespace UnityMugen.Evaluation.Triggers.Redirection
{

    [StateRedirection("Parent")]
    class Parent : Function
    {
        public Parent(List<IFunction> children, List<System.Object> arguments)
               : base(children, arguments)
        {
        } 

        public override Number Evaluate(Character character)
        {
            if (character == null || Children.Count != 1) return new Number();

            var helper = character as UnityMugen.Combat.Helper;
            if (helper == null)
            {
                Debug.Log("Error | Parent");
                return new Number();
            }

            return Children[0].Evaluate(helper.Creator);
        }

        public static Node Parse(ParseState parsestate)
        {
            if (parsestate.CurrentSymbol != Symbol.Comma) return null;
            ++parsestate.TokenIndex;

            var child = parsestate.BuildNode(false);
            if (child == null) return null;
            parsestate.BaseNode.Children.Add(child);

            return parsestate.BaseNode;
        }
    }
     
    [StateRedirection("Root")]
    class Root : Function
    {
        public Root(List<IFunction> children, List<System.Object> arguments)
               : base(children, arguments)
        {
        }
        public override Number Evaluate(Character character)
        {
            if (character == null || Children.Count != 1) return new Number();

            var helper = character as UnityMugen.Combat.Helper;
            if (helper == null)
            {
                Debug.Log("Error | Root");
                return new Number();
            }

            return Children[0].Evaluate(helper.BasePlayer);
        }

        public static Node Parse(ParseState parsestate)
        {
            if (parsestate.CurrentSymbol != Symbol.Comma) return null;
            ++parsestate.TokenIndex;

            var child = parsestate.BuildNode(false);
            if (child == null) return null;
            parsestate.BaseNode.Children.Add(child);

            return parsestate.BaseNode;
        }
    }

    [StateRedirection("Helper")]
    class Helper : Function
    {
        public Helper(List<IFunction> children, List<System.Object> arguments)
               : base(children, arguments) { }
        public override Number Evaluate(Character character)
        {
            if (character == null || Children.Count != 2) return new Number();

            Number helperId = Children[0].Evaluate(character);
            if (helperId.NumberType != NumberType.Int) return new Number();

            List<UnityMugen.Combat.Helper> helpers;
            if (character.BasePlayer.Helpers.TryGetValue(helperId.IntValue, out helpers) &&
                helpers.Count > 0)
                return Children[1].Evaluate(helpers[0]);

            return new Number();
        }

        public static Node Parse(ParseState parsestate)
        {
            var node = parsestate.BuildParenNumberNode(true);
            if (node == null)
            {
                node = parsestate.BaseNode;

#warning Hack
                node.Children.Add(new Node(new Token("-1", new IntData())));
            }

            if (parsestate.CurrentSymbol != Symbol.Comma) return null;
            ++parsestate.TokenIndex;

            var child = parsestate.BuildNode(false);
            if (child == null) return null;

            node.Children.Add(child);

            return node;
        }
    }

    [StateRedirection("Target")]
    class Target : Function
    {
        public Target(List<IFunction> children, List<System.Object> arguments)
                  : base(children, arguments)
        {
        }
        // Se Valor <= -1 ele Redireciona o gatilho para o primeiro alvo encontrado.
        public override Number Evaluate(Character character)
        {
            if (character == null || Children.Count != 2) return new Number();

            Number target_id = Children[0].Evaluate(character);
            if (target_id.NumberType != NumberType.Int) return new Number();

            foreach (var target in character.GetTargets(target_id.IntValue))
            {
                return Children[1].Evaluate(target);
            }

            return new Number();
        }

        public static Node Parse(ParseState parsestate)
        {
            var node = parsestate.BuildParenNumberNode(true);
            if (node == null)
            {
                node = parsestate.BaseNode;

                // Se target_id ==  null, target_id recebe -1
                node.Children.Add(new Node(new Token("-1", new IntData())));
            }

            if (parsestate.CurrentSymbol != Symbol.Comma) return null;
            ++parsestate.TokenIndex;

            var child = parsestate.BuildNode(false);
            if (child == null) return null;

            node.Children.Add(child);

            return node;
        }
    }

    [StateRedirection("Partner")]
    class Partner : Function
    {
        public Partner(List<IFunction> children, List<System.Object> arguments)
               : base(children, arguments)
        {
        }
        public override Number Evaluate(Character character)
        {
            if (character == null || Children.Count != 1) return new Number();

            var partner = GetTeamMate(character);
            if (partner == null)
            {
                Debug.Log("Error | Partner");
                return new Number();
            }

            return Children[0].Evaluate(partner);
        }

        private static Player GetTeamMate(Character character)
        {
            if (character == null) throw new ArgumentNullException(nameof(character));

            if (character.BasePlayer == character.Team.MainPlayer)
            {
                return character.Team.TeamMate;
            }

            return character.Team.MainPlayer;
        }

        public static Node Parse(ParseState parsestate)
        {
            if (parsestate.CurrentSymbol != Symbol.Comma) return null;
            ++parsestate.TokenIndex;

            var child = parsestate.BuildNode(false);
            if (child == null) return null;
            parsestate.BaseNode.Children.Add(child);

            return parsestate.BaseNode;
        }
    }

    [StateRedirection("Enemy")]
    class Enemy : Function
    {
        public Enemy(List<IFunction> children, List<System.Object> arguments)
               : base(children, arguments)
        {
        }
        public override Number Evaluate(Character character)
        {
            if (character == null || Children.Count != 2) return new Number();

            Number nth = Children[0].Evaluate(character);
            if (nth.NumberType == NumberType.None) return new Number();

            var count = 0;
            foreach (var entity in character.Engine.Entities)
            {
                var enemy = character.FilterEntityAsCharacter(entity, AffectTeam.Enemy);
                if (enemy == null) continue;

                var enemyplayer = enemy as Player;
                if (enemyplayer == null) continue;

                if (count != nth.IntValue)
                {
                    ++count;
                    continue;
                }

                return Children[1].Evaluate(enemy);
            }

            return new Number();
        }

        /*
        public static Character RedirectState(Character character, ref bool error, bool nth)
        {
            if (character == null)
            {
                error = true;
                Debug.Log("Error | Enemy");
                return null;
            }

            foreach (var entity in character.Engine.Entities)
            {
                var enemy = character.FilterEntityAsCharacter(entity, AffectTeam.Enemy);
                if (enemy == null) continue;

                var enemyplayer = enemy as Player;
                if (enemyplayer == null) continue;

                return enemy;
            }

            error = true;
            Debug.Log("Error | Enemy");
            return null;
        }
        */

        public static Node Parse(ParseState parsestate)
        {
            var node = parsestate.BuildParenNumberNode(true);
            if (node == null)
            {
                node = parsestate.BaseNode;

#warning Hack
                node.Children.Add(Node.ZeroNode);
            }

            if (parsestate.CurrentSymbol != Symbol.Comma) return null;
            ++parsestate.TokenIndex;

            var child = parsestate.BuildNode(false);
            if (child == null) return null;

            node.Children.Add(child);

            return node;
        }
    }

#warning Not threadsafe
    [StateRedirection("EnemyNear")]
    class EnemyNear : Function
    {
        public EnemyNear(List<IFunction> children, List<System.Object> arguments)
            : base(children, arguments)
        {
        }
#warning isso tem que retornar o inimigo mais perto
        // nth = 0 o inimigo mais perto
        // nth = 1 o segundo inimigo mais perto
        // nth = 2 o terceiro inimigo mais perto
        // ... assim por diante
        public override Number Evaluate(Character character)
        {
            if (character == null || Children.Count != 2) return new Number();

            Number nth = Children[0].Evaluate(character);
            if (nth.NumberType == NumberType.None) return new Number();

            var playerlist = BuildPlayerList(character);

            playerlist = SortPlayerList(character.CurrentLocation, playerlist);

            if (nth.IntValue >= playerlist.Count) return new Number();

            return Children[1].Evaluate(playerlist[nth.IntValue]);
        }



        private static List<Player> SortPlayerList(Vector2 characterPos, List<Player> enemies)
        {
            var sortPlayerList = new List<Tuple<float, Player>>();

            for (int i = 0; i < enemies.Count; i++)
            {
                float Xmax = Math.Max(characterPos.x, enemies[i].CurrentLocation.x);
                float XMin = Math.Min(characterPos.x, enemies[i].CurrentLocation.x);
                float distance = Xmax - XMin;

                sortPlayerList.Add(new Tuple<float, Player>(distance, enemies[i]));
            }

            return sortPlayerList.OrderBy(o => o.Item1).Select(go => go.Item2).ToList();
        }

        private static List<Player> BuildPlayerList(Character character)
        {
            if (character == null) throw new ArgumentNullException(nameof(character));

            var otherteam = character.Team.OtherTeam;

            var players = new List<Player>();
            if (otherteam.MainPlayer != null) players.Add(otherteam.MainPlayer);
            if (otherteam.TeamMate != null) players.Add(otherteam.TeamMate);

            return players;
        }

        public static Node Parse(ParseState parsestate)
        {
            var node = parsestate.BuildParenNumberNode(true);
            if (node == null)
            {
                node = parsestate.BaseNode;

#warning Hack
                node.Children.Add(Node.ZeroNode);
            }

            if (parsestate.CurrentSymbol != Symbol.Comma) return null;
            ++parsestate.TokenIndex;

            var child = parsestate.BuildNode(false);
            if (child == null) return null;

            node.Children.Add(child);

            return node;
        }
    }

    [StateRedirection("PlayerID")]
    class PlayerID : Function
    {
        public PlayerID(List<IFunction> children, List<System.Object> arguments)
            : base(children, arguments)
        {
        }
    
        public override Number Evaluate(Character character)
        {
            if (character == null || Children.Count != 2) return new Number();

            Number characterId = Children[0].Evaluate(character);
            if (characterId.NumberType != NumberType.Int) return new Number();

            foreach (var entity in character.Engine.Entities)
            {
                var character2 = entity as Character;
                if (character2 == null || character2.Id != characterId.IntValue) continue;

                return Children[1].Evaluate(character2);
            }

            Debug.Log("Error | PlayerID");
            return new Number();
        }

        public static Node Parse(ParseState parsestate)
        {
            var node = parsestate.BuildParenNumberNode(true);
            if (node == null) return null;

            if (parsestate.CurrentSymbol != Symbol.Comma) return null;
            ++parsestate.TokenIndex;

            var child = parsestate.BuildNode(false);
            if (child == null) return null;

            node.Children.Add(child);

            return node;
        }
    }
}