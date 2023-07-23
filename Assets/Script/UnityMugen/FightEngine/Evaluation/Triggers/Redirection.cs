using System;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityMugen.Combat;

namespace UnityMugen.Evaluation.Triggers
{

    [StateRedirection("Parent")]
    internal static class Parent
    {
        public static Character RedirectState(Character character, ref bool error)
        {
            if (character == null)
            {
                error = true;
                return null;
            }

            var helper = character as UnityMugen.Combat.Helper;
            if (helper == null)
            {
                error = true;
                Debug.Log("Error | Parent");
                return null;
            }

            return helper.Creator;
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
    internal static class Root
    {
        public static Character RedirectState(Character character, ref bool error)
        {
            if (character == null)
            {
                error = true;
                return null;
            }

            var helper = character as UnityMugen.Combat.Helper;
            if (helper == null)
            {
                error = true;
                Debug.Log("Error | Root");
                return null;
            }

            return helper.BasePlayer;
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
    internal static class HelperT
    {
        public static Character RedirectState(Character character, ref bool error, int helperId)
        {
            if (character == null)
            {
                error = true;
                return null;
            }

            List<UnityMugen.Combat.Helper> helpers;
            if (character.BasePlayer.Helpers.TryGetValue(helperId, out helpers) &&
                helpers.Count > 0)
                return helpers[0];

            error = true;
            return null;
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
    internal static class Target
    {
        // Se Valor <= -1 ele Redireciona o gatilho para o primeiro alvo encontrado.
        public static Character RedirectState(Character character, ref bool error, int target_id)
        {
            if (character == null)
            {
                error = true;
                return null;
            }

            foreach (var target in character.GetTargets(target_id))
            {
                return target;
            }

            error = true;
            return null;
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
    internal static class Partner
    {
        public static Character RedirectState(Character character, ref bool error)
        {
            if (character == null)
            {
                error = true;
                return null;
            }


            var partner = GetTeamMate(character);
            if (partner == null)
            {
                error = true;
                Debug.Log("Error | Partner");
                return null;
            }

            return partner;
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
    internal static class Enemy
    {
        public static Character RedirectState(Character character, ref bool error, int nth)
        {
            if (character == null)
            {
                error = true;
                Debug.Log("Error | Enemy");
                return null;
            }

            var count = 0;
            foreach (var entity in character.Engine.Entities)
            {
                var enemy = character.FilterEntityAsCharacter(entity, AffectTeam.Enemy);
                if (enemy == null) continue;

                var enemyplayer = enemy as Player;
                if (enemyplayer == null) continue;

                if (count != nth)
                {
                    ++count;
                    continue;
                }

                return enemy;
            }

            error = true;
            Debug.Log("Error | Enemy");
            return null;
        }


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

    [StateRedirection("EnemyNear")]
    internal static class EnemyNear
    {

#warning isso tem que retornar o inimigo mais perto
        // nth = 0 o inimigo mais perto
        // nth = 1 o segundo inimigo mais perto
        // nth = 2 o terceiro inimigo mais perto
        // ... assim por diante
        public static Character RedirectState(Character character, ref bool error, int nth = 0)
        {
            if (character == null)
            {
                error = true;
                Debug.Log("Error | EnemyNear");
                return null;
            }

            var playerlist = BuildPlayerList(character);

            playerlist = SortPlayerList(character.CurrentLocation, playerlist);

            if (nth >= playerlist.Count)
            {
                error = true;
                Debug.Log("Error | EnemyNear");
                return null;
            }

            return playerlist[nth];
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
    internal static class PlayerIDT
    {
        public static Character RedirectState(Character character, ref bool error, int characterId)
        {
            if (character == null)
            {
                error = true;
                Debug.Log("Error | PlayerID");
                return null;
            }

            foreach (var entity in character.Engine.Entities)
            {
                var character2 = entity as Character;
                if (character2 == null || character2.Id != characterId) continue;

                return character2;
            }

            error = true;
            Debug.Log("Error | PlayerID");
            return null;
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