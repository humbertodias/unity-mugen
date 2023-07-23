using UnityEngine;
using UnityMugen.Combat;
using UnityMugen.Interface;

namespace UnityMugen.Screens
{

    public interface IStageScreen
    {
        Stage Stage { get; }
        CanvasFull CanvasFull { get; }
        HudCanvasBattleUnity HCBU { get; }
        HudMoveLists MoveLists { get; }
        HudPauseFight PauseFight { get; }
        GameObject Foreground { get; }
        GameObject Background { get; }
        StageActions StageActions { get; }

        Transform Entities { get; }
        Transform Shadowns { get; }
        Transform Reflections { get; }
        Transform AfterImages { get; }

    }

}