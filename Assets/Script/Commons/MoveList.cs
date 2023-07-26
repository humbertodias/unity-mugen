using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenuAttribute(fileName = "CharMoveList", menuName = "Unity Mugen/Character/Move List")]
public class MoveList : ScriptableObject
{
    public bool editReadme;
    public Texture2D iconChar;
    public string nameChar;

    public List<MoveShow> specialMoves;
    public List<MoveShow> superMoves;
}

[Serializable]
public class MoveShow
{
    public string nameMove;
    public Texture2D imageMove;
    public int parent;

    [TextArea(4, 1000)]
    public string description;
    public List<Texture2D> playerButtons;
    [NonSerialized] public Vector2 scrollAction;

    public MoveShow() { }

    public MoveShow(MoveShow moveShow)
    {
        nameMove = moveShow.nameMove;
        imageMove = moveShow.imageMove;
        parent = moveShow.parent;
        description = moveShow.description;
        playerButtons = new List<Texture2D>(moveShow.playerButtons);
    }

}
