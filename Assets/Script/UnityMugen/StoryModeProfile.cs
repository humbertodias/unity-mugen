using System;
using System.Collections.Generic;
using UnityEngine;
using UnityMugen;

[CreateAssetMenu(fileName = "StoryModeProfile", menuName = "Unity Mugen/Settings/Story Mode Profile")]
public class StoryModeProfile : ScriptableObject
{
    public StoryModeType storyMode;
    public TeamMode teamMode;
    public bool randomBattle;

    public List<StoryModeData> storiesMode;

}

[Serializable]
public class StoryModeData
{
    public UnityEngine.Object sceneStoryOpen;

    /// <summary>
    /// Se este campo estiver vazio, o sistema escolhera automaticamente um personagem
    /// para lutar contra voce.
    /// </summary>
    public PlayerProfileManager playerProfile;

    public int StateID;

    public UnityEngine.Object sceneStoryEnd;
}

public enum StoryModeType
{
    Fixed,// A historia começa e termina com personagens pre determinado, estilo Mortal Kombat
    ByCharacter,//Cada personagem tem sua sequencia de personagens para enfrentar, estilo Skullgirls
}