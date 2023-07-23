using UnityEngine;
using UnityEngine.UI;
using UnityMugen;

public class HistoryCommands : MonoBehaviour
{

    public Image direction;
    public Image X;
    public Image Y;
    public Image Z;
    public Image A;
    public Image B;
    public Image C;
    public Image Taunt;
    public Text frames;


    public void CommandPlayerButtons(PlayerButton playerButton)
    {
        if (playerButton.ToString().Contains(PlayerButton.X.ToString()))
        {
            X.color = Color.white;
        }
        if (playerButton.ToString().Contains(PlayerButton.Y.ToString()))
        {
            Y.color = Color.white;
        }
        if (playerButton.ToString().Contains(PlayerButton.Z.ToString()))
        {
            Z.color = Color.white;
        }
        if (playerButton.ToString().Contains(PlayerButton.A.ToString()))
        {
            A.color = Color.white;
        }
        if (playerButton.ToString().Contains(PlayerButton.B.ToString()))
        {
            B.color = Color.white;
        }
        if (playerButton.ToString().Contains(PlayerButton.C.ToString()))
        {
            C.color = Color.white;
        }
        if (playerButton.ToString().Contains(PlayerButton.Taunt.ToString()))
        {
            Taunt.enabled = true;
        }
    }
}
