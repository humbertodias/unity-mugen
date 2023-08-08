using System;
using System.Reflection;
using UnityEditor;
using UnityEngine;
using UnityMugen;
using UnityMugen.Combat;
using UnityMugen.Evaluation;
using UnityMugen.IO;

public class ConverterPlayerConstantEditorWindow : EditorWindow
{
    public string nameChar;
 
    private bool m_currentPathProject = true;
    private TextFile m_textFile;
    private string m_outputChars = "Assets/";

    [MenuItem("UnityMugen/Converter Player Constant File")]
    static void Init()
    {
        EditorWindow.GetWindow(typeof(ConverterPlayerConstantEditorWindow)).Show();
    }

    void OnGUI()
    {
        SerializedObject serializedObject = new SerializedObject(this);

        EditorGUILayout.PropertyField(serializedObject.FindProperty("nameChar"), true);

        if (GUILayout.Button("Converter Constant File"))
        {
            string file = EditorUtility.OpenFilePanel("Load .cns file", "", "cns");
            if (file.Length != 0)
            {
                m_textFile = new FileSystem().OpenTextFile(file);
                Converter();

                this.ShowNotification(new GUIContent("Finish."));
            }
        }

        m_currentPathProject = GUILayout.Toggle(m_currentPathProject, "Current Path Project");

        serializedObject.ApplyModifiedProperties();
    }

    public PlayerConstants BuildExternal(string path, string _nameChar, string _OutputChars)
    {
        nameChar = _nameChar;
        m_outputChars = _OutputChars;
        m_currentPathProject = false;
        m_textFile = new FileSystem().OpenTextFile(path);
        return Converter();
    }

    PlayerConstants Converter()
    {
        var datasection = m_textFile.GetSection("Data");
        var sizesection = m_textFile.GetSection("Size");
        var velocitysection = m_textFile.GetSection("Velocity");
        var movementsection = m_textFile.GetSection("Movement");

        if (datasection == null) throw new ArgumentException("Constants file '" + m_textFile.Filepath + "' does not have a 'Data' section");
        if (sizesection == null) throw new ArgumentException("Constants file '" + m_textFile.Filepath + "' does not have a 'Size' section");
        if (velocitysection == null) throw new ArgumentException("Constants file '" + m_textFile.Filepath + "' does not have a 'Velocity' section");
        if (movementsection == null) throw new ArgumentException("Constants file '" + m_textFile.Filepath + "' does not have a 'Movement' section");

        PlayerConstants playerConstants = ScriptableObject.CreateInstance<PlayerConstants>();

        playerConstants.MaximumLife = datasection.GetAttribute("life", 1000);
        playerConstants.MaximumPower = datasection.GetAttribute("power", 3000);
        playerConstants.AttackPower = datasection.GetAttribute("attack", 100);
        playerConstants.DefensivePower = datasection.GetAttribute("defence", 100);
        playerConstants.FallDefenseUp = datasection.GetAttribute("fall.defence_up", 50);
        playerConstants.LieDownTime = datasection.GetAttribute("liedown.time", 60);
        playerConstants.AirJuggle = datasection.GetAttribute("airjuggle", 15);

        PrefixedExpression sparkno = datasection.GetAttribute<PrefixedExpression>("sparkno", null);
        playerConstants.DefaultSparkNumber = EvaluationHelper.AsInt32(null, sparkno, -1);
        playerConstants.DefaultSparkNumberIsCommon = !EvaluationHelper.IsCommon(sparkno, false);// if equal "" ou "s" is common

        PrefixedExpression defaultguardspark = datasection.GetAttribute<PrefixedExpression>("guard.sparkno", null);
        playerConstants.DefaultGuardSparkNumber = EvaluationHelper.AsInt32(null, defaultguardspark, -1);
        playerConstants.DefaultGuardSparkNumberIsCommon = !EvaluationHelper.IsCommon(defaultguardspark, false);// if equal "" ou "s" is common

        playerConstants.KOEcho = datasection.GetAttribute("KO.echo", false);
        playerConstants.VolumeOffset = datasection.GetAttribute("volume", 0);
        playerConstants.PersistanceIntIndex = datasection.GetAttribute("IntPersistIndex", 60);
        playerConstants.PersistanceFloatIndex = datasection.GetAttribute("FloatPersistIndex", 40);


        // [Size]
        float x = sizesection.GetAttribute("xscale", 1.0f);
        float y = sizesection.GetAttribute("yscale", 1.0f);
        playerConstants.Scale = new Vector2(x, y);

        playerConstants.GroundBack = sizesection.GetAttribute("ground.back", 15);
        playerConstants.GroundFront = sizesection.GetAttribute("ground.front", 16);
        playerConstants.Airback = sizesection.GetAttribute("air.back", 12);
        playerConstants.Airfront = sizesection.GetAttribute("air.front", 12);
        playerConstants.Height = sizesection.GetAttribute("height", 60);
        playerConstants.Attackdistance = sizesection.GetAttribute("attack.dist", 160);
        playerConstants.Projectileattackdist = sizesection.GetAttribute("proj.attack.dist", 90);
        playerConstants.ProjectileScaling = sizesection.GetAttribute("proj.doscale", false);
        playerConstants.Headposition = sizesection.GetAttribute("head.pos", Vector2.zero);
        playerConstants.Midposition = sizesection.GetAttribute("mid.pos", Vector2.zero);
        playerConstants.Shadowoffset = sizesection.GetAttribute("shadowoffset", 0);
        playerConstants.Drawoffset = sizesection.GetAttribute("draw.offset", new Vector2(0, 0));


        // [Velocity]
        playerConstants.Walk_forward = velocitysection.GetAttribute<float>("walk.fwd");
        playerConstants.Walk_back = velocitysection.GetAttribute<float>("walk.back");
        playerConstants.Run_fwd = velocitysection.GetAttribute<Vector2>("run.fwd");
        playerConstants.Run_back = velocitysection.GetAttribute<Vector2>("run.back");
        playerConstants.Jump_neutral = velocitysection.GetAttribute<Vector2>("jump.neu");
        playerConstants.Jump_back = velocitysection.GetAttribute<Vector2>("jump.back");
        playerConstants.Jump_forward = velocitysection.GetAttribute<Vector2>("jump.fwd");
        playerConstants.Runjump_back = velocitysection.GetAttribute("runjump.back", playerConstants.Run_back);
        playerConstants.Runjump_fwd = velocitysection.GetAttribute("runjump.fwd", playerConstants.Run_fwd);
        playerConstants.Airjump_neutral = velocitysection.GetAttribute("airjump.neu", playerConstants.Jump_neutral);
        playerConstants.Airjump_back = velocitysection.GetAttribute("airjump.back", playerConstants.Jump_back);
        playerConstants.Airjump_forward = velocitysection.GetAttribute("airjump.fwd", playerConstants.Jump_forward);
        // NOVOS TESTE
        playerConstants.AirGethitGroundrecover = velocitysection.GetAttribute("air.gethit.groundrecover", new Vector2(-.15f, -3.5f));
        playerConstants.AirGethitAirrecoverMul = velocitysection.GetAttribute("air.gethit.airrecover.mul", new Vector2(.5f, .2f));
        playerConstants.AirGethitAirrecoverAdd = velocitysection.GetAttribute("air.gethit.airrecover.add", new Vector2(0f, -4.5f));
        playerConstants.AirGethitAirrecoverBack = velocitysection.GetAttribute("air.gethit.airrecover.back", -1);
        playerConstants.AirGethitAirrecoverFwd = velocitysection.GetAttribute("air.gethit.airrecover.fwd", 0);
        playerConstants.AirGethitAirrecoverUp = velocitysection.GetAttribute("air.gethit.airrecover.up", -2);
        playerConstants.AirGethitAirrecoverDown = velocitysection.GetAttribute("air.gethit.airrecover.down", 1.5f);


        // [Movement]
        playerConstants.Airjumps = movementsection.GetAttribute("airjump.num", 0);
        playerConstants.Airjumpheight = movementsection.GetAttribute("airjump.height", 0);
        playerConstants.Vert_acceleration = movementsection.GetAttribute<float>("yaccel");
        playerConstants.Standfriction = movementsection.GetAttribute<float>("stand.friction");
        playerConstants.Crouchfriction = movementsection.GetAttribute<float>("crouch.friction");
        // NOVOS TESTE
        playerConstants.StandFrictionThreshold = movementsection.GetAttribute<float>("stand.friction.threshold", 2);
        playerConstants.CrouchFrictionThreshold = movementsection.GetAttribute<float>("crouch.friction.threshold", .05f);
        playerConstants.JumpChangeanimThreshold = movementsection.GetAttribute<float>("jump.changeanim.threshold", 0);
        playerConstants.AirGethitGroundlevel = movementsection.GetAttribute<float>("air.gethit.groundlevel", 25);
        playerConstants.AirGethitGroundrecoverGroundThreshold = movementsection.GetAttribute<float>("air.gethit.groundrecover.ground.threshold", -20);
        playerConstants.AirGethitGroundrecoverGroundlevel = movementsection.GetAttribute<float>("air.gethit.groundrecover.groundlevel", 10);
        playerConstants.AirGethitAirrecoverThreshold = movementsection.GetAttribute<float>("air.gethit.airrecover.threshold", -1);
        playerConstants.AirGethitAirrecoverYaccel = movementsection.GetAttribute<float>("air.gethit.airrecover.yaccel", .35f);
        playerConstants.AirGethitTripGroundlevel = movementsection.GetAttribute<float>("air.gethit.trip.groundlevel", 15);
        playerConstants.DownBounceOffset = movementsection.GetAttribute("down.bounce.offset", new Vector2(0, 20));
        playerConstants.DownBounceYaccel = movementsection.GetAttribute<float>("down.bounce.yaccel", .4f);
        playerConstants.DownBounceGroundlevel = movementsection.GetAttribute<float>("down.bounce.groundlevel", 12);
        playerConstants.DownFrictionThreshold = movementsection.GetAttribute<float>("down.friction.threshold", .05f);



        if (m_currentPathProject)
        {
            Type projectWindowUtilType = typeof(ProjectWindowUtil);
            MethodInfo getActiveFolderPath = projectWindowUtilType.GetMethod("GetActiveFolderPath", BindingFlags.Static | BindingFlags.NonPublic);
            object obj = getActiveFolderPath.Invoke(null, new object[0]);
            string path = obj.ToString() + "/" + nameChar + "PlayerConstants.asset";
            AssetDatabase.CreateAsset(playerConstants, path);
        }
        else
        {
            AssetDatabase.CreateAsset(playerConstants, m_outputChars + nameChar + "PlayerConstants.asset");
        }

        AssetDatabase.SaveAssets();
        Selection.activeObject = playerConstants;
        EditorUtility.FocusProjectWindow();

        return playerConstants;
    }
}
