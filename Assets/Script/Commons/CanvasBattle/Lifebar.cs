using System;
using UnityEngine.UI;
using UnityMugen.Combat;

namespace UnityMugen.Interface
{

    public class Lifebar
    {
        public float m_currentLife;
        public float m_damage;

        public Image m_lifebar, m_lifebarRed;

        public Lifebar(Image lifebar, Image lifebarRed)
        {
            m_currentLife = 1000;
            m_damage = 1000;
            m_lifebar = lifebar;
            m_lifebarRed = lifebarRed;
        }

        public void Draw(Player player)
        {
            var lifePercentage = Math.Max(0.0f, player.Life / (float)player.playerConstants.MaximumLife);
            m_lifebar.fillAmount = lifePercentage;

            m_lifebarRed.fillAmount = m_damage / (float)player.playerConstants.MaximumLife;
        }

        public void Update(Player player)
        {
            m_damage -= ((m_damage - player.Life) / 6.0f + 0.5f);
        }
    }
}