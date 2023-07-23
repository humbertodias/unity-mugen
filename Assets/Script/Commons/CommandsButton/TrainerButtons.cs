using System;
using UnityEngine;
using UnityEngine.UI;
using UnityMugen;
using UnityMugen.Combat;

public class TrainerButtons : MonoBehaviour
{
    LauncherEngine Launcher => LauncherEngine.Inst;
    FightEngine Engine => Launcher.mugen.Engine;

    private ListCommands listP1;
    private ListCommands listP2;

    public GameObject listP1GO;
    public GameObject listP2GO;


    private void Start()
    {
        listP1 = listP1GO.GetComponent<ListCommands>();
        listP2 = listP2GO.GetComponent<ListCommands>();
        listP1GO.SetActive(false);
        listP2GO.SetActive(false);
    }

    public void UpdateFE(CombatMode combatMode)
    {
        if (Launcher.trainnerSettings.showCommands && combatMode == UnityMugen.CombatMode.Training)
        {
            gameObject.SetActive(true);
            UpdateCommands();
        }
        else
        {
            gameObject.SetActive(false);
        }
    }

    private void UpdateCommands()
    {
        if (Engine == null) throw new ArgumentNullException(nameof(Engine));

        PlayerButton playerButtonP1 = Engine.Team1.MainPlayer.CurrentInput;
        PlayerButton playerButtonP2 = Engine.Team2.MainPlayer.CurrentInput;

        ResetCommands(listP1.directions, listP1.buttons);
        ResetCommands(listP2.directions, listP2.buttons);

        if (Engine.RoundState == RoundState.Fight)
        {

            EngineInitialization ini = Engine.Initialization;

            if (ini.Team1[0].mode == PlayerMode.Human)
            {
                listP1GO.SetActive(true);
                CommandPlayerDirection(listP1.directions, playerButtonP1);
                CommandPlayerButtons(listP1.buttons, playerButtonP1);
            }

            if (ini.Team2[0].mode == PlayerMode.Human)
            {
                listP2GO.SetActive(true);
                CommandPlayerDirection(listP2.directions, playerButtonP2);
                CommandPlayerButtons(listP2.buttons, playerButtonP2);
            }
        }
    }

    private void ResetCommands(GameObject[] directions, Image[] buttons)
    {
        directions[0].SetActive(false);
        directions[1].SetActive(false);
        directions[2].SetActive(false);
        directions[3].SetActive(false);
        directions[4].SetActive(false);
        directions[5].SetActive(false);
        directions[6].SetActive(false);
        directions[7].SetActive(false);
        directions[8].SetActive(true);

        buttonDesactive(buttons[0]);
        buttonDesactive(buttons[1]);
        buttonDesactive(buttons[2]);
        buttonDesactive(buttons[3]);
        buttonDesactive(buttons[4]);
        buttonDesactive(buttons[5]);
    }

    private void CommandPlayerDirection(GameObject[] directions, PlayerButton playerButton)
    {
        if (playerButton == PlayerButton.None)
            directions[8].SetActive(true);
        else
            directions[8].SetActive(false);

        if (playerButton.ToString().Contains(PlayerButton.Up.ToString()) && playerButton.ToString().Contains(PlayerButton.Right.ToString()))
        {
            directions[1].SetActive(true);
        }
        else if (playerButton.ToString().Contains(PlayerButton.Down.ToString()) && playerButton.ToString().Contains(PlayerButton.Right.ToString()))
        {
            directions[3].SetActive(true);
        }
        else if (playerButton.ToString().Contains(PlayerButton.Down.ToString()) && playerButton.ToString().Contains(PlayerButton.Left.ToString()))
        {
            directions[5].SetActive(true);
        }
        else if (playerButton.ToString().Contains(PlayerButton.Up.ToString()) && playerButton.ToString().Contains(PlayerButton.Left.ToString()))
        {
            directions[7].SetActive(true);
        }
        else if (playerButton.ToString().Contains(PlayerButton.Up.ToString()))
        {
            directions[0].SetActive(true);
        }
        else if (playerButton.ToString().Contains(PlayerButton.Right.ToString()))
        {
            directions[2].SetActive(true);
        }
        else if (playerButton.ToString().Contains(PlayerButton.Down.ToString()))
        {
            directions[4].SetActive(true);
        }
        else if (playerButton.ToString().Contains(PlayerButton.Left.ToString()))
        {
            directions[6].SetActive(true);
        }

    }



    private void CommandPlayerButtons(Image[] buttons, PlayerButton playerButton)
    {

        if (playerButton.ToString().Contains(PlayerButton.X.ToString()))
        {
            buttonActive(buttons[0]);
        }
        if (playerButton.ToString().Contains(PlayerButton.Y.ToString()))
        {
            buttonActive(buttons[1]);
        }
        if (playerButton.ToString().Contains(PlayerButton.Z.ToString()))
        {
            buttonActive(buttons[2]);
        }
        if (playerButton.ToString().Contains(PlayerButton.A.ToString()))
        {
            buttonActive(buttons[3]);
        }
        if (playerButton.ToString().Contains(PlayerButton.B.ToString()))
        {
            buttonActive(buttons[4]);
        }
        if (playerButton.ToString().Contains(PlayerButton.C.ToString()))
        {
            buttonActive(buttons[5]);
        }
    }

    private void buttonActive(Image button)
    {
        button.GetComponent<Image>().color = new Color32(255, 255, 225, 225);
    }

    private void buttonDesactive(Image button)
    {
        button.GetComponent<Image>().color = new Color32(255, 255, 225, 105);
    }
}