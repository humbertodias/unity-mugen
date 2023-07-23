using System;
using System.Collections.Generic;
using UnityEngine;
using UnityMugen;
using UnityMugen.Combat;

public class HistoryInput : MonoBehaviour
{
    LauncherEngine Launcher => LauncherEngine.Inst;
    FightEngine Engine => Launcher.mugen.Engine;

    public int maxHistory;
    public Color inactive;
    public Transform parentP1;
    public HistoryCommands historyCommandsInstance;
    public LayoutHistoryCommands layoutHistoryCommands;

    PlayerButton playerButtonP1Last;
    HistoryCommands historyCommandsLast;
    List<HistoryCommands> allHistoryCommands = new List<HistoryCommands>();
    int frames;


    public void UpdateFE(CombatMode combatMode)
    {
        if (Launcher.trainnerSettings.showInputHistory && combatMode == UnityMugen.CombatMode.Training)
        {
            parentP1.gameObject.SetActive(true);

            if (Engine.RoundState == RoundState.Fight)
                UpdateHistory();
        }
        else
        {
            parentP1.gameObject.SetActive(false);
            if (allHistoryCommands.Count > 0)
                ClearHistory();
        }
    }

    void UpdateHistory()
    {
        if (frames < 9999)
            frames++;

        PlayerButton playerButtonP1 = Engine.Team1.MainPlayer.CurrentInput;

        if (playerButtonP1 == PlayerButton.Start)
            return;

        if (historyCommandsLast != null)
            historyCommandsLast.frames.text = frames.ToString();

        if (playerButtonP1Last != playerButtonP1)
        {
            playerButtonP1Last = playerButtonP1;
            frames = 0;

            HistoryCommands hc = Instantiate(historyCommandsInstance);
            hc.gameObject.name = "Inputs" + frames;
            historyCommandsLast = hc;
            historyCommandsLast.gameObject.SetActive(true);
            historyCommandsLast.transform.SetParent(parentP1, false);
            allHistoryCommands.Add(historyCommandsLast);

            if (playerButtonP1 == PlayerButton.None)
            {
                historyCommandsLast.direction.sprite = layoutHistoryCommands.None;
            }


            if (playerButtonP1.ToString().Contains(PlayerButton.Up.ToString()) && playerButtonP1.ToString().Contains(PlayerButton.Right.ToString()))
            {
                historyCommandsLast.direction.sprite = layoutHistoryCommands.RightUp;
            }
            else if (playerButtonP1.ToString().Contains(PlayerButton.Down.ToString()) && playerButtonP1.ToString().Contains(PlayerButton.Right.ToString()))
            {
                historyCommandsLast.direction.sprite = layoutHistoryCommands.RightDown;
            }
            else if (playerButtonP1.ToString().Contains(PlayerButton.Down.ToString()) && playerButtonP1.ToString().Contains(PlayerButton.Left.ToString()))
            {
                historyCommandsLast.direction.sprite = layoutHistoryCommands.LeftDown;
            }
            else if (playerButtonP1.ToString().Contains(PlayerButton.Up.ToString()) && playerButtonP1.ToString().Contains(PlayerButton.Left.ToString()))
            {
                historyCommandsLast.direction.sprite = layoutHistoryCommands.LeftUp;
            }
            else if (playerButtonP1.ToString().Contains(PlayerButton.Up.ToString()))
            {
                historyCommandsLast.direction.sprite = layoutHistoryCommands.Up;
            }
            else if (playerButtonP1.ToString().Contains(PlayerButton.Right.ToString()))
            {
                historyCommandsLast.direction.sprite = layoutHistoryCommands.Right;
            }
            else if (playerButtonP1.ToString().Contains(PlayerButton.Down.ToString()))
            {
                historyCommandsLast.direction.sprite = layoutHistoryCommands.Down;
            }
            else if (playerButtonP1.ToString().Contains(PlayerButton.Left.ToString()))
            {
                historyCommandsLast.direction.sprite = layoutHistoryCommands.Left;
            }

            historyCommandsLast.CommandPlayerButtons(playerButtonP1);

            if (allHistoryCommands.Count > 10)
            {
                Destroy(allHistoryCommands[0].gameObject);
                allHistoryCommands.Remove(allHistoryCommands[0]);
            }
        }
    }

    public void ClearHistory()
    {
        foreach (HistoryCommands historyCommands in allHistoryCommands)
        {
            Destroy(historyCommands.gameObject);
        }
        allHistoryCommands.Clear();
        historyCommandsLast = null;
    }
}

[Serializable]
public class LayoutHistoryCommands
{
    public Sprite None;
    public Sprite Left;
    public Sprite LeftDown;
    public Sprite Down;
    public Sprite RightDown;
    public Sprite Right;
    public Sprite RightUp;
    public Sprite Up;
    public Sprite LeftUp;

    public Sprite X;
    public Sprite Y;
    public Sprite Z;
    public Sprite A;
    public Sprite B;
    public Sprite C;

    public Sprite Taunt;
}