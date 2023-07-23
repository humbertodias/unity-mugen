using System;
using UnityEngine;
using UnityEngine.UI;
using UnityMugen;
using UnityMugen.Combat;

namespace UnityMugen.Interface
{

    public class TeamLifeBarUnity : MonoBehaviour
    {

        public Color colorDisabled;

        [Header("Character 1")]
        public Text namePlayer1;
        public Image imagePlayer1;
        public Image lifebarTeam1P1;
        public Image lifebarRedTeam1P1;

        //[Header("Character 3")]
        //public Text namePlayer3;
        //public Image imagePlayer3;
        //public Image lifebarTeam1P2;
        //public Image lifebarRedTeam1P2;


        [Header("Character 2")]
        public Text namePlayer2;
        public Image imagePlayer2;
        public Image lifebarTeam2P1;
        public Image lifebarRedTeam2P1;

        //[Header("Character 4")]
        //public Text namePlayer4;
        //public Image imagePlayer4;
        //public Image lifebarTeam2P2;
        //public Image lifebarRedTeam2P2;


        private Lifebar m_mateLifebarP1C1;
        private Lifebar m_mateLifebarP2C2;
        //private Lifebar m_mateLifebarP1C3;
        //private Lifebar m_mateLifebarP2C4;

        private LauncherEngine Launcher => LauncherEngine.Inst;
        public FightEngine Engine => Launcher.mugen.Engine;


        public void UpdateFE()
        {
            if (Engine == null) throw new ArgumentNullException(nameof(Engine));


            Player player1 = Engine.Team1.MainPlayer;
            Player player2 = Engine.Team2.MainPlayer;

            Player player3 = Engine.Team1.TeamMate;
            Player player4 = Engine.Team2.TeamMate;

            // Usado no Modo Trainner
            if (Launcher.engineInitialization.Mode == CombatMode.Training &&
                Launcher.trainnerSettings.hpRecovery != HPRecovery.Normal)
            {
                if (Launcher.trainnerSettings.hpRecovery == HPRecovery.Immediate)
                {
                    if (player1.LastTickHitable < Engine.TickCount - 150) // 2.5 Segundos
                        player1.Life = player1.playerConstants.MaximumLife;

                    if (player2.LastTickHitable < Engine.TickCount - 150) // 2.5 Segundos
                        player2.Life = player2.playerConstants.MaximumLife;

                    if (player3 != null && player4 != null)
                    {
                        if (player3.LastTickHitable < Engine.TickCount - 150) // 2.5 Segundos
                            player3.Life = player3.playerConstants.MaximumLife;

                        if (player4.LastTickHitable < Engine.TickCount - 150) // 2.5 Segundos
                            player4.Life = player4.playerConstants.MaximumLife;
                    }
                }
                else if (Launcher.trainnerSettings.hpRecovery == HPRecovery.Gradual)
                {
                    if (player1.LastTickHitable < Engine.TickCount - 90) // 1.5 Segundos
                        player1.Life += player1.playerConstants.MaximumLife * 1f / 100f;

                    if (player2.LastTickHitable < Engine.TickCount - 90) // 1.5 Segundos
                        player2.Life += player2.playerConstants.MaximumLife * 1f / 100f;

                    if (player3 != null && player4 != null)
                    {
                        if (player3.LastTickHitable < Engine.TickCount - 90) // 1.5 Segundos
                            player3.Life += player3.playerConstants.MaximumLife * 1f / 100f;

                        if (player4.LastTickHitable < Engine.TickCount - 90) // 1.5 Segundos
                            player4.Life += player4.playerConstants.MaximumLife * 1f / 100f;
                    }
                }
            }

            ComboCounter ccP1 = Engine.Team1.ComboCounter;
            ComboCounter ccP2 = Engine.Team2.ComboCounter;

            namePlayer1.text = player1.profile.displayName;
            namePlayer2.text = player2.profile.displayName;

            //if (player3 == null) {
            //    lifebarTeam1P2.color = colorDisabled;
            //    namePlayer3.color = Color.clear;
            //    imagePlayer3.color = colorDisabled;
            //}
            //else
            //{
            //    namePlayer3.text = player3.playerProfileManager.displayName;
            //    imagePlayer3.sprite = player3.playerProfileManager.smallPortrait;
            //}

            //if (player4 == null)
            //{
            //    lifebarTeam2P2.color = colorDisabled;
            //    namePlayer4.color = Color.clear;
            //    imagePlayer4.color = colorDisabled;
            //}
            //else
            //{
            //    namePlayer4.text = player4.playerProfileManager.displayName;
            //    imagePlayer4.sprite = player4.playerProfileManager.smallPortrait;
            //}


            imagePlayer1.sprite = player1.profile.smallPortrait;
            //Palette p1 = imagePlayer1.gameObject.AddComponent<Palette>();
            //p1 = player1.gameObject.GetComponent<Palette>();

            imagePlayer2.sprite = player2.profile.smallPortrait;


            if (ccP2.GetNewHitCount() == 0 || player1.Life == 0)
            {
                m_mateLifebarP1C1.Update(player1);
                //if (player3 != null)
                //    m_mateLifebarP1C3.Update(player3);
            }
            m_mateLifebarP1C1.Draw(player1);
            //if (player3 != null)
            //    m_mateLifebarP1C3.Draw(player3);

            if (ccP1.GetNewHitCount() == 0 || player2.Life == 0)
            {
                m_mateLifebarP2C2.Update(player2);
                //if (player4 != null)
                //    m_mateLifebarP2C4.Update(player4);
            }
            m_mateLifebarP2C2.Draw(player2);
            //if (player4 != null)
            //    m_mateLifebarP2C4.Draw(player4);
        }

        void Start()
        {
            m_mateLifebarP1C1 = new Lifebar(lifebarTeam1P1, lifebarRedTeam1P1);
            m_mateLifebarP2C2 = new Lifebar(lifebarTeam2P1, lifebarRedTeam2P1);

            //m_mateLifebarP1C3 = new Lifebar(lifebarTeam1P2, lifebarRedTeam1P2);
            //m_mateLifebarP2C4 = new Lifebar(lifebarTeam2P2, lifebarRedTeam2P2);
        }


        public void ResetDamage()
        {
            Engine.Team1.MainPlayer.Life = Engine.Team1.MainPlayer.playerConstants.MaximumLife;
            Engine.Team2.MainPlayer.Life = Engine.Team2.MainPlayer.playerConstants.MaximumLife;


            if (Engine.Team1.TeamMate)
                Engine.Team1.TeamMate.Life = Engine.Team1.TeamMate.playerConstants.MaximumLife;
            if (Engine.Team2.TeamMate)
                Engine.Team2.TeamMate.Life = Engine.Team2.TeamMate.playerConstants.MaximumLife;


            m_mateLifebarP1C1.m_currentLife = Engine.Team1.MainPlayer.Life;
            m_mateLifebarP1C1.m_damage = Engine.Team1.MainPlayer.Life;

            m_mateLifebarP2C2.m_currentLife = Engine.Team2.MainPlayer.Life;
            m_mateLifebarP2C2.m_damage = Engine.Team2.MainPlayer.Life;

            //if (Engine.Team1.TeamMate)
            //{
            //    m_mateLifebarP1C3.m_currentLife = Engine.Team1.TeamMate.Life;
            //    m_mateLifebarP1C3.m_damage = Engine.Team1.TeamMate.Life;
            //}

            //if (Engine.Team2.TeamMate)
            //{
            //    m_mateLifebarP2C4.m_currentLife = Engine.Team2.TeamMate.Life;
            //    m_mateLifebarP2C4.m_damage = Engine.Team2.TeamMate.Life;
            //}
        }

        public void LifeZero()
        {
            Engine.Team1.MainPlayer.Life = 1f;
            Engine.Team2.MainPlayer.Life = 1f;


            if (Engine.Team1.TeamMate)
                Engine.Team1.TeamMate.Life = 1f;
            if (Engine.Team2.TeamMate)
                Engine.Team2.TeamMate.Life = 1f;


            m_mateLifebarP1C1.m_currentLife = Engine.Team1.MainPlayer.Life;
            m_mateLifebarP1C1.m_damage = Engine.Team1.MainPlayer.Life;

            m_mateLifebarP2C2.m_currentLife = Engine.Team2.MainPlayer.Life;
            m_mateLifebarP2C2.m_damage = Engine.Team2.MainPlayer.Life;

            //if (Engine.Team1.TeamMate)
            //{
            //    m_mateLifebarP1C3.m_currentLife = Engine.Team1.TeamMate.Life;
            //    m_mateLifebarP1C3.m_damage = Engine.Team1.TeamMate.Life;
            //}

            //if (Engine.Team2.TeamMate)
            //{
            //    m_mateLifebarP2C4.m_currentLife = Engine.Team2.TeamMate.Life;
            //    m_mateLifebarP2C4.m_damage = Engine.Team2.TeamMate.Life;
            //}
        }


    }
}