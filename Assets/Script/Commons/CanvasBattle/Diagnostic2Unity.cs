using System;
using System.Text;
using UnityEngine;
using UnityEngine.UI;
using UnityMugen.Combat;

namespace UnityMugen.Interface
{

    public class Diagnostic2Unity : MonoBehaviour
    {

        private Text text;
        private StringBuilder m_stringbuilder;

        FightEngine Engine => LauncherEngine.Inst.mugen.Engine;

        private void Awake()
        {
            text = GetComponent<Text>();
            m_stringbuilder = new StringBuilder();
        }


        public void UpdateFE(UnityMugen.CombatMode combatMode)
        {
            if (m_stringbuilder == null)
                return;

            if (Engine == null) throw new ArgumentNullException(nameof(Engine));

            m_stringbuilder.Length = 0;

            foreach (var entity in Engine.Entities)
            {
                if (entity is UnityMugen.Combat.Helper)
                    BuildTextHelpers(entity as Helper);
                if (entity is UnityMugen.Combat.Explod)
                    BuildTextExplod(entity as Explod);
                if (entity is UnityMugen.Combat.Projectile)
                    BuildTextProjectile(entity as Projectile);
            }

            text.text = m_stringbuilder.ToString();
        }

        private void BuildTextHelpers(Helper entity)
        {
            m_stringbuilder.AppendFormat("Helpers P:{0} {1}", entity.BasePlayer.m_PlayerNumber, Environment.NewLine);
            m_stringbuilder.AppendFormat("Anim: {0}   Spr: {1}   Elem: {2} / {3}   Time: {4} / {5}\r\n", entity.AnimationManager.CurrentAnimation.Number, entity.AnimationManager.CurrentElement.SpriteId, entity.AnimationManager.CurrentElement.Id + 1, entity.AnimationManager.CurrentAnimation.Elements.Count, entity.AnimationManager.TimeInAnimation, entity.AnimationManager.CurrentAnimation.TotalTime);
            m_stringbuilder.AppendFormat("CurrentLocation: {0} \r\n", entity.CurrentLocation);
            m_stringbuilder.AppendFormat("ESTATE: {0} \r\n", entity.StateManager.CurrentState.number);
            m_stringbuilder.AppendFormat("-------------{0}", Environment.NewLine);
        }

        private void BuildTextExplod(Explod entity)
        {
            m_stringbuilder.AppendFormat("Explod P:{0} {1}", entity.BasePlayer.m_PlayerNumber, Environment.NewLine);

            if (entity.AnimationManager.CurrentElement != null)
                m_stringbuilder.AppendFormat("Anim: {0}   Spr: {1}   Elem: {2} / {3}   Time: {4} / {5}\r\n", entity.AnimationManager.CurrentAnimation.Number, entity.AnimationManager.CurrentElement.SpriteId, entity.AnimationManager.CurrentElement.Id + 1, entity.AnimationManager.CurrentAnimation.Elements.Count, entity.AnimationManager.TimeInAnimation, entity.AnimationManager.CurrentAnimation.TotalTime);
            m_stringbuilder.AppendFormat("CurrentLocation: {0} \r\n", entity.CurrentLocation);
            m_stringbuilder.AppendFormat("-------------{0}", Environment.NewLine);
        }

        private void BuildTextProjectile(Projectile entity)
        {
            m_stringbuilder.AppendFormat("Projectile P:{0} {1}", entity.BasePlayer.m_PlayerNumber, Environment.NewLine);
            m_stringbuilder.AppendFormat("Anim: {0}   Spr: {1}   Elem: {2} / {3}   Time: {4} / {5}\r\n", entity.AnimationManager.CurrentAnimation.Number, entity.AnimationManager.CurrentElement.SpriteId, entity.AnimationManager.CurrentElement.Id + 1, entity.AnimationManager.CurrentAnimation.Elements.Count, entity.AnimationManager.TimeInAnimation, entity.AnimationManager.CurrentAnimation.TotalTime);
            m_stringbuilder.AppendFormat("CurrentLocation: {0} \r\n", entity.CurrentLocation);
            m_stringbuilder.AppendFormat("-------------{0}", Environment.NewLine);
        }

    }
}