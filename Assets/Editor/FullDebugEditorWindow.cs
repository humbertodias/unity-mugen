using System;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;
using UnityMugen;
using UnityMugen.Animations;
using UnityMugen.Collections;
using UnityMugen.Combat;
using UnityMugen.Commands;

public class FullDebugEditorWindow : EditorWindow
{

    public static FullDebugEditorWindow s_window;

    LauncherEngine Launcher => LauncherEngine.Inst;
    FightEngine Engine => BattleActive();

    Vector2 m_scrollPos;
    bool m_players, m_helpers, m_explods, m_projectiles;
    bool m_assertSpecial, m_shake, m_match, m_pause, m_superPause;
    bool m_intVars, m_floatVars, m_sysIntVars, m_sysFloatVars;
    bool m_bind, m_animation, m_assertion, m_cmdBufferTime;

    string[] enum_Facing, enum_Flip;
    string[] enum_StateType, enum_Physics, enum_MoveType, enum_PlayerControl;

    GUILayoutOption[] m_optionsVars = new GUILayoutOption[] { GUILayout.Width(150), GUILayout.MinWidth(100) };

    Dictionary<long, bool> m_foldoutEntities = new Dictionary<long, bool>();
    Dictionary<long, bool> m_foldoutAnimations = new Dictionary<long, bool>();
    Dictionary<long, bool> m_foldoutCmd = new Dictionary<long, bool>();
    Dictionary<long, bool> m_toggleActiveCmd = new Dictionary<long, bool>();

    public string nameEntity;
    bool searchBy;
    bool pauseWhenFindId;

    public string id;
    private int? ID => IDValue();

    int? IDValue()
    {
        if (!string.IsNullOrEmpty(id))
        {
            if (int.TryParse(id, out int number))
            {
                return number;
            }
            else
            {
                id = "";
                return null;
            }
        }
        return null;
    }

    [MenuItem("UnityMugen/Full Debug")]
    static void Init()
    {
        s_window = EditorWindow.GetWindow<FullDebugEditorWindow>(false, "Full Debug", true);
        s_window.minSize = new Vector2(574, 533);
        s_window.Show();
        s_window.Inicialize();
    }

    void Inicialize()
    {
        enum_Facing = new string[] { "Left", "Right" };
        enum_Flip = new string[] { "None", "FlipHorizontally", "FlipVertically", "Both" };
        enum_StateType = new string[] { "None", "Unchanged", "Standing", "Crouching", "Airborne", "Liedown" };
        enum_Physics = new string[] { "None", "Unchanged", "Standing", "Crouching", "Airborne" };
        enum_MoveType = new string[] { "None", "Idle", "Attack", "BeingHit", "Unchanged" };
        enum_PlayerControl = new string[] { "Unchanged", "InControl", "NoControl" };
    }

    FightEngine BattleActive()
    {
        return Launcher != null &&
            Launcher.mugen != null &&
            Launcher.mugen.BattleActive ? Launcher.mugen.Engine : null;
    }

    void OnGUI()
    {
        SerializedObject serializedObject = new SerializedObject(this);

        EditorGUILayout.BeginVertical("ObjectFieldThumb");
        {
            EditorGUILayout.BeginHorizontal();
            {
                EditorGUI.BeginChangeCheck();

                EditorGUIUtility.labelWidth = 30;
                id = EditorGUILayout.TextField("ID:", id);

                GUILayout.Space(15);

                EditorGUIUtility.labelWidth = 90;
                nameEntity = EditorGUILayout.TextField("Entity Name:", nameEntity, GUILayout.Width(90 + 200));

                if (EditorGUI.EndChangeCheck())
                {
                    searchBy = (ID != null || !string.IsNullOrEmpty(nameEntity));
                }

                GUILayout.Space(15);

                EditorGUIUtility.labelWidth = 120;
                EditorGUI.BeginChangeCheck();
                pauseWhenFindId = EditorGUILayout.Toggle("Pause on finding ID:", pauseWhenFindId);
                if (EditorGUI.EndChangeCheck())
                {
                    if (!pauseWhenFindId) EditorApplication.isPaused = false;
                }
            }
            EditorGUILayout.EndHorizontal();

            EditorGUILayout.BeginHorizontal();
            {
                GUIStyle StateFieldLabelColorGreen = new GUIStyle(EditorStyles.boldLabel)
                {
                    normal = new GUIStyleState() { textColor = Color.green }
                };

                GUILayout.FlexibleSpace();
                EditorGUIUtility.labelWidth = 90;
                EditorGUILayout.LabelField(searchBy ? "Searching By Parameter" : "", StateFieldLabelColorGreen);
                GUILayout.FlexibleSpace();
            }
            EditorGUILayout.EndHorizontal();
        }
        EditorGUILayout.EndVertical();





        m_scrollPos = EditorGUILayout.BeginScrollView(m_scrollPos);



        if (Engine != null)
        {
            if (searchBy)
                SearchBy();
            else
                SearchAll();
        }
        else
        {
            GUIStyle StateFieldLabelColorRed = new GUIStyle(EditorStyles.boldLabel)
            {
                normal = new GUIStyleState() { textColor = Color.yellow, }
            };
            StateFieldLabelColorRed.fontSize = 18;

            GUILayout.FlexibleSpace();
            EditorGUILayout.BeginHorizontal();
            GUILayout.FlexibleSpace();
            EditorGUILayout.BeginVertical();
            ShowNotification(new GUIContent("Start a fight to debug."));
            EditorGUILayout.EndVertical();
            GUILayout.FlexibleSpace();
            EditorGUILayout.EndHorizontal();
            GUILayout.FlexibleSpace();
        }
        EditorGUILayout.EndScrollView();

        EditorGUILayout.BeginHorizontal();

        GUILayout.FlexibleSpace();
        EditorGUIUtility.labelWidth = 60;
        EditorGUILayout.LabelField("Version: 0.0.2", EditorStyles.boldLabel);
        EditorGUILayout.EndHorizontal();

        serializedObject.ApplyModifiedProperties();
        Repaint();
    }

    void SearchBy()
    {
        foreach (Entity entity in Engine.Entities)
        {
            if ((ID.HasValue && entity.Id == ID.Value) || entity.NameSearch == nameEntity)
            {
                if (pauseWhenFindId) EditorApplication.isPaused = true;

                if (!m_foldoutEntities.ContainsKey(entity.UniqueID))
                    m_foldoutEntities.Add(entity.UniqueID, false);

                EditorGUILayout.BeginVertical("ObjectFieldThumb");
                {
                    m_foldoutEntities[entity.UniqueID] = EditorGUILayout.Foldout(m_foldoutEntities[entity.UniqueID], entity.name, true, "Foldout");
                    if (m_foldoutEntities[entity.UniqueID])
                    {
                        if (entity.typeEntity == TypeEntity.Player)
                            CharacterInfo(entity as Player);
                        else if (entity.typeEntity == TypeEntity.Helper)
                            HelperInfo(entity as Helper);
                        else if (entity.typeEntity == TypeEntity.Explod)
                            ExplodInfo(entity as Explod);
                        else if (entity.typeEntity == TypeEntity.Projectile)
                            ProjectileInfo(entity as Projectile);
                    }
                }
                EditorGUILayout.EndVertical();
            }

        }

        OtherInfo();
    }

    void SearchAll()
    {
        EditorGUILayout.BeginVertical("ObjectFieldThumb");
        {
            m_players = EditorGUILayout.Foldout(m_players, "Players", true, "Foldout");
            if (m_players)
            {
                foreach (Entity entity in Engine.Entities)
                {
                    if (entity.typeEntity == TypeEntity.Player)
                    {
                        if (!m_foldoutEntities.ContainsKey(entity.UniqueID))
                            m_foldoutEntities.Add(entity.UniqueID, false);

                        EditorGUILayout.BeginVertical("ObjectFieldThumb");
                        {
                            m_foldoutEntities[entity.UniqueID] = EditorGUILayout.Foldout(m_foldoutEntities[entity.UniqueID], entity.name, true, "Foldout");
                            if (m_foldoutEntities[entity.UniqueID])
                            {
                                CharacterInfo(entity as Player);
                            }
                        }
                        EditorGUILayout.EndVertical();

                    }
                }
            }
        }
        EditorGUILayout.EndVertical();


        EditorGUILayout.BeginVertical("ObjectFieldThumb");
        {
            m_helpers = EditorGUILayout.Foldout(m_helpers, "Helpers", true, "Foldout");
            if (m_helpers)
            {
                foreach (Entity entity in Engine.Entities)
                {
                    if (entity.typeEntity == TypeEntity.Helper)
                    {
                        if (!m_foldoutEntities.ContainsKey(entity.UniqueID))
                            m_foldoutEntities.Add(entity.UniqueID, false);

                        EditorGUILayout.BeginVertical("ObjectFieldThumb");
                        {
                            m_foldoutEntities[entity.UniqueID] = EditorGUILayout.Foldout(m_foldoutEntities[entity.UniqueID], entity.name, true, "Foldout");
                            if (m_foldoutEntities[entity.UniqueID])
                            {
                                HelperInfo(entity as Helper);
                            }
                        }
                        EditorGUILayout.EndVertical();

                    }
                }
            }
        }
        EditorGUILayout.EndVertical();


        EditorGUILayout.BeginVertical("ObjectFieldThumb");
        {
            m_explods = EditorGUILayout.Foldout(m_explods, "Explods", true, "Foldout");
            if (m_explods)
            {
                foreach (Entity entity in Engine.Entities)
                {
                    if (entity.typeEntity == TypeEntity.Explod)
                    {
                        if (!m_foldoutEntities.ContainsKey(entity.UniqueID))
                            m_foldoutEntities.Add(entity.UniqueID, false);

                        EditorGUILayout.BeginVertical("ObjectFieldThumb");
                        {
                            m_foldoutEntities[entity.UniqueID] = EditorGUILayout.Foldout(m_foldoutEntities[entity.UniqueID], entity.name, true, "Foldout");
                            if (m_foldoutEntities[entity.UniqueID])
                            {
                                ExplodInfo(entity as Explod);
                            }
                        }
                        EditorGUILayout.EndVertical();

                    }
                }
            }
        }
        EditorGUILayout.EndVertical();


        EditorGUILayout.BeginVertical("ObjectFieldThumb");
        {
            m_projectiles = EditorGUILayout.Foldout(m_projectiles, "Projectiles", true, "Foldout");
            if (m_projectiles)
            {
                foreach (Entity entity in Engine.Entities)
                {
                    if (entity.typeEntity == TypeEntity.Projectile)
                    {
                        if (!m_foldoutEntities.ContainsKey(entity.UniqueID))
                            m_foldoutEntities.Add(entity.UniqueID, false);

                        EditorGUILayout.BeginVertical("ObjectFieldThumb");
                        {
                            m_foldoutEntities[entity.UniqueID] = EditorGUILayout.Foldout(m_foldoutEntities[entity.UniqueID], entity.name, true, "Foldout");
                            if (m_foldoutEntities[entity.UniqueID])
                            {
                                ProjectileInfo(entity as Projectile);
                            }
                        }
                        EditorGUILayout.EndVertical();

                    }
                }
            }
        }
        EditorGUILayout.EndVertical();


        OtherInfo();
    }

    void CharacterInfo(Player mainPlayer)
    {
        EditorGUILayout.ObjectField(mainPlayer, typeof(Player), true);

        EditorGUILayout.BeginVertical("ObjectFieldThumb");
        {

            CommonsData(mainPlayer as Entity);

            EditorGUILayout.BeginHorizontal();
            {
                EditorGUILayout.LabelField("PaletteNo: ");
                EditorGUILayout.LabelField(mainPlayer.PaletteNumber.ToString());
                EditorGUILayout.LabelField("int");
            }
            EditorGUILayout.EndHorizontal();

            EditorGUILayout.BeginHorizontal();
            {
                EditorGUILayout.LabelField("Power: ");
                mainPlayer.Power = EditorGUILayout.FloatField(mainPlayer.Power);
                EditorGUILayout.LabelField("float");
            }
            EditorGUILayout.EndHorizontal();

            EntityData(mainPlayer as Entity);
            CharacterData(mainPlayer as Character);

        }
        EditorGUILayout.EndVertical();
    }

    void HelperInfo(Helper helper)
    {
        CommonsData(helper as Entity);
        EntityData(helper as Entity);
        CharacterData(helper as Character);
    }

    void ExplodInfo(Explod explod)
    {
        CommonsData(explod as Entity);
        EntityData(explod as Entity);
    }

    void ProjectileInfo(Projectile projectile)
    {
        CommonsData(projectile as Entity);
        EntityData(projectile as Entity);
    }

    void CommonsData(Entity entity)
    {
        EditorGUILayout.BeginHorizontal();
        {
            EditorGUILayout.LabelField("Name");
            EditorGUILayout.LabelField("Value");
            EditorGUILayout.LabelField("Type");
        }
        EditorGUILayout.EndHorizontal();

        EditorGUILayout.BeginHorizontal();
        {
            EditorGUILayout.LabelField("Id: ");
            EditorGUILayout.LabelField(entity.Id.ToString());
            EditorGUILayout.LabelField("int");
        }
        EditorGUILayout.EndHorizontal();
    }

    void EntityData(Entity entity)
    {
        EditorGUILayout.BeginHorizontal();
        {
            EditorGUILayout.LabelField("Position: ");
            entity.CurrentLocation = EditorGUILayout.Vector2Field("",entity.CurrentLocation);
            EditorGUILayout.LabelField("Vector2");
        }
        EditorGUILayout.EndHorizontal();

        EditorGUILayout.BeginHorizontal();
        {
            EditorGUILayout.LabelField("Velocity: ");
            entity.CurrentVelocity = EditorGUILayout.Vector2Field("", entity.CurrentVelocity);
            EditorGUILayout.LabelField("Vector2");
        }
        EditorGUILayout.EndHorizontal();

        EditorGUILayout.BeginHorizontal();
        {
            EditorGUILayout.LabelField("CurrentFacing: ");
            entity.CurrentFacing = (Facing)EditorGUILayout.Popup("", (int)entity.CurrentFacing, enum_Facing);
            EditorGUILayout.LabelField("Type");
        }
        EditorGUILayout.EndHorizontal();

        EditorGUILayout.BeginHorizontal();
        {
            EditorGUILayout.LabelField("CurrentFlip: ");
            entity.CurrentFlip = (SpriteEffects)EditorGUILayout.Popup("", (int)entity.CurrentFlip, enum_Flip);
            EditorGUILayout.LabelField("Type");
        }
        EditorGUILayout.EndHorizontal();

    }
    
    

    void CharacterData(Character character)
    {
        EditorGUILayout.BeginHorizontal();
        {
            EditorGUILayout.LabelField("Life: ");
            character.Life = EditorGUILayout.FloatField(character.Life);
            EditorGUILayout.LabelField("float");
        }
        EditorGUILayout.EndHorizontal();

        EditorGUILayout.BeginHorizontal();
        {
            EditorGUILayout.LabelField("JugglePoints: ");
            EditorGUILayout.LabelField(character.JugglePoints.ToString());
            EditorGUILayout.LabelField("int");
        }
        EditorGUILayout.EndHorizontal();

        EditorGUILayout.BeginHorizontal();
        {
            EditorGUILayout.LabelField("StateType: ");
            character.StateType = (StateType)EditorGUILayout.Popup("", (int)character.StateType, enum_StateType);
            EditorGUILayout.LabelField("Type");
        }
        EditorGUILayout.EndHorizontal();

        EditorGUILayout.BeginHorizontal();
        {
            EditorGUILayout.LabelField("Physics: ");
            character.Physics = (Physic)EditorGUILayout.Popup((int)character.Physics, enum_Physics);
            EditorGUILayout.LabelField("Type");
        }
        EditorGUILayout.EndHorizontal();

        EditorGUILayout.BeginHorizontal();
        {
            EditorGUILayout.LabelField("MoveType: ");
            character.MoveType = (MoveType)EditorGUILayout.Popup((int)character.MoveType, enum_MoveType);
            EditorGUILayout.LabelField("Type");
        }
        EditorGUILayout.EndHorizontal();

        EditorGUILayout.BeginHorizontal();
        {
            EditorGUILayout.LabelField("PlayerControl: ");
            character.PlayerControl = (PlayerControl)EditorGUILayout.Popup((int)character.PlayerControl, enum_PlayerControl);
            EditorGUILayout.LabelField("Type");
        }
        EditorGUILayout.EndHorizontal();

        EditorGUILayout.BeginHorizontal();
        {
            EditorGUILayout.LabelField("hit.pause.time: ");
            EditorGUILayout.LabelField(character.OffensiveInfo.HitPauseTime.ToString());
            EditorGUILayout.LabelField("int");
        }
        EditorGUILayout.EndHorizontal();

        EditorGUILayout.BeginHorizontal();
        {
            EditorGUILayout.LabelField("camera.follow.x: ");
            character.CameraFollowX = EditorGUILayout.Toggle(character.CameraFollowX);
            EditorGUILayout.LabelField("    bool");
        }
        EditorGUILayout.EndHorizontal();

        EditorGUILayout.BeginHorizontal();
        {
            EditorGUILayout.LabelField("camera.follow.y: ");
            character.CameraFollowY = EditorGUILayout.Toggle(character.CameraFollowY);
            EditorGUILayout.LabelField("    bool");
        }
        EditorGUILayout.EndHorizontal();

        EditorGUILayout.BeginHorizontal();
        {
            EditorGUILayout.LabelField("push.checking: ");
            character.PushFlag = EditorGUILayout.Toggle(character.PushFlag);
            EditorGUILayout.LabelField("    bool");
        }
        EditorGUILayout.EndHorizontal();


        EditorGUILayout.BeginHorizontal();
        {
            EditorGUILayout.LabelField("StateNo: ");
            character.StateManager.CurrentState.number = EditorGUILayout.IntField(character.StateManager.CurrentState.number);
            EditorGUILayout.LabelField("int");
        }
        EditorGUILayout.EndHorizontal();

        EditorGUILayout.BeginHorizontal();
        {
            EditorGUILayout.LabelField("PrevStateNo: ");
            EditorGUILayout.LabelField(character.StateManager.PreviousState != null ? character.StateManager.PreviousState.number.ToString() : "None");
            EditorGUILayout.LabelField("int");
        }
        EditorGUILayout.EndHorizontal();


        EditorGUILayout.BeginHorizontal();
        {
            EditorGUILayout.LabelField("StateTime: ");
            character.StateManager.StateTime = EditorGUILayout.IntField(character.StateManager.StateTime);
            EditorGUILayout.LabelField("int");
        }
        EditorGUILayout.EndHorizontal();

        AnimationManager(character.AnimationManager);

        EditorGUILayout.BeginVertical("ObjectFieldThumb");
        {
            m_bind = EditorGUILayout.Foldout(m_bind, "Bind:", true, "Foldout");
            if (m_bind)
            {
                if (character.Bind != null && character.Bind.BindTo != null)
                {
                    EditorGUILayout.BeginHorizontal();
                    {
                        EditorGUILayout.LabelField("Name: ");
                        EditorGUILayout.LabelField(character.Bind.BindTo.name.ToString());
                        EditorGUILayout.LabelField("string");
                    }
                    EditorGUILayout.EndHorizontal();

                    EditorGUILayout.BeginHorizontal();
                    {
                        EditorGUILayout.LabelField("Time: ");
                        EditorGUILayout.LabelField(character.Bind.Time.ToString());
                        EditorGUILayout.LabelField("int");
                    }
                    EditorGUILayout.EndHorizontal();

                    EditorGUILayout.BeginHorizontal();
                    {
                        EditorGUILayout.LabelField("Facing: ");
                        EditorGUILayout.LabelField(character.Bind.FacingFlag.ToString());
                        EditorGUILayout.LabelField("int");
                    }
                    EditorGUILayout.EndHorizontal();

                    EditorGUILayout.BeginHorizontal();
                    {
                        EditorGUILayout.LabelField("Position: ");
                        EditorGUILayout.LabelField(character.Bind.Offset.ToString());
                        EditorGUILayout.LabelField("vector2");
                    }
                    EditorGUILayout.EndHorizontal();
                }
                else
                {
                    EditorGUILayout.LabelField("No Bind ");
                }
            }
        }
        EditorGUILayout.EndVertical();



        EditorGUILayout.BeginVertical("ObjectFieldThumb");
        {
            m_assertion = EditorGUILayout.Foldout(m_assertion, "Assertions:", true, "Foldout");
            if (m_assertion)
            {
                EditorGUILayout.BeginHorizontal();
                EditorGUILayout.LabelField("NoStandingGuard:");
                EditorGUILayout.LabelField(character.Assertions.NoStandingGuard.ToString());
                EditorGUILayout.LabelField("bool");
                EditorGUILayout.EndHorizontal();

                EditorGUILayout.BeginHorizontal();
                EditorGUILayout.LabelField("NoCrouchingGuard:");
                EditorGUILayout.LabelField(character.Assertions.NoCrouchingGuard.ToString());
                EditorGUILayout.LabelField("bool");
                EditorGUILayout.EndHorizontal();

                EditorGUILayout.BeginHorizontal();
                EditorGUILayout.LabelField("NoAirGuard:");
                EditorGUILayout.LabelField(character.Assertions.NoAirGuard.ToString());
                EditorGUILayout.LabelField("bool");
                EditorGUILayout.EndHorizontal();

                EditorGUILayout.BeginHorizontal();
                EditorGUILayout.LabelField("NoAutoTurn:");
                EditorGUILayout.LabelField(character.Assertions.NoAutoTurn.ToString());
                EditorGUILayout.LabelField("bool");
                EditorGUILayout.EndHorizontal();

                EditorGUILayout.BeginHorizontal();
                EditorGUILayout.LabelField("NoShadow:");
                EditorGUILayout.LabelField(character.Assertions.NoShadow.ToString());
                EditorGUILayout.LabelField("bool");
                EditorGUILayout.EndHorizontal();

                EditorGUILayout.BeginHorizontal();
                EditorGUILayout.LabelField("NoJuggleCheck:");
                EditorGUILayout.LabelField(character.Assertions.NoJuggleCheck.ToString());
                EditorGUILayout.LabelField("bool");
                EditorGUILayout.EndHorizontal();

                EditorGUILayout.BeginHorizontal();
                EditorGUILayout.LabelField("NoWalk:");
                EditorGUILayout.LabelField(character.Assertions.NoWalk.ToString());
                EditorGUILayout.LabelField("bool");
                EditorGUILayout.EndHorizontal();

                EditorGUILayout.BeginHorizontal();
                EditorGUILayout.LabelField("UnGuardable:");
                EditorGUILayout.LabelField(character.Assertions.UnGuardable.ToString());
                EditorGUILayout.LabelField("bool");
                EditorGUILayout.EndHorizontal();

                EditorGUILayout.BeginHorizontal();
                EditorGUILayout.LabelField("Invisible:");
                EditorGUILayout.LabelField(character.Assertions.Invisible.ToString());
                EditorGUILayout.LabelField("bool");
                EditorGUILayout.EndHorizontal();
            }
        }
        EditorGUILayout.EndVertical();

        Cmd(character.CommandManager);

        ShowVarsInt("Vars:", character.Variables.IntegerVariables, ref m_intVars);
        ShowVarsFloat("FloatVars:", character.Variables.FloatVariables, ref m_floatVars);
        ShowVarsInt("SysVars:", character.Variables.SystemIntegerVariables, ref m_sysIntVars);
        ShowVarsFloat("SysFloatVars:", character.Variables.SystemFloatVariables, ref m_sysFloatVars);
    }

    void OtherInfo()
    {

        EditorGUILayout.BeginVertical("ObjectFieldThumb");
        {
            m_assertSpecial = EditorGUILayout.Foldout(m_assertSpecial, "Assert Special:", true, "Foldout");
            if (m_assertSpecial)
            {
                AssertSpecial(Engine.Assertions);
            }
        }
        EditorGUILayout.EndVertical();

        EditorGUILayout.BeginVertical("ObjectFieldThumb");
        {
            m_shake = EditorGUILayout.Foldout(m_shake, "Environment Shake:", true, "Foldout");
            if (m_shake)
            {
                EnvShake(Engine.EnvironmentShake);
            }
        }
        EditorGUILayout.EndVertical();

        EditorGUILayout.BeginVertical("ObjectFieldThumb");
        {
            m_match = EditorGUILayout.Foldout(m_match, "Match:", true, "Foldout");
            if (m_match)
            {
                Match(Engine);
            }
        }
        EditorGUILayout.EndVertical();

        EditorGUILayout.BeginVertical("ObjectFieldThumb");
        {
            m_pause = EditorGUILayout.Foldout(m_pause, "Pause:", true, "Foldout");
            if (m_pause)
            {
                Pause(Engine.Pause);
            }
        }
        EditorGUILayout.EndVertical();

        EditorGUILayout.BeginVertical("ObjectFieldThumb");
        {
            m_superPause = EditorGUILayout.Foldout(m_superPause, "Super Pause:", true, "Foldout");
            if (m_superPause)
            {
                Pause(Engine.SuperPause);
            }
        }
        EditorGUILayout.EndVertical();

    }

    void AssertSpecial(EngineAssertions assertions)
    {
        EditorGUILayout.BeginHorizontal();
        EditorGUILayout.LabelField("NoKOSound:");
        assertions.NoKOSound = EditorGUILayout.Toggle(assertions.NoKOSound);
        EditorGUILayout.LabelField("    bool");
        EditorGUILayout.EndHorizontal();

        EditorGUILayout.BeginHorizontal();
        EditorGUILayout.LabelField("NoKOSlow:");
        assertions.NoKOSlow = EditorGUILayout.Toggle(assertions.NoKOSlow);
        EditorGUILayout.LabelField("    bool");
        EditorGUILayout.EndHorizontal();

        EditorGUILayout.BeginHorizontal();
        EditorGUILayout.LabelField("GlobalNoShadow:");
        assertions.GlobalNoShadow = EditorGUILayout.Toggle(assertions.GlobalNoShadow);
        EditorGUILayout.LabelField("    bool");
        EditorGUILayout.EndHorizontal();

        EditorGUILayout.BeginHorizontal();
        EditorGUILayout.LabelField("NoMusic:");
        assertions.NoMusic = EditorGUILayout.Toggle(assertions.NoMusic);
        EditorGUILayout.LabelField("    bool");
        EditorGUILayout.EndHorizontal();

        EditorGUILayout.BeginHorizontal();
        EditorGUILayout.LabelField("TimerFreeze:");
        assertions.TimerFreeze = EditorGUILayout.Toggle(assertions.TimerFreeze);
        EditorGUILayout.LabelField("    bool");
        EditorGUILayout.EndHorizontal();

        EditorGUILayout.BeginHorizontal();
        EditorGUILayout.LabelField("Intro:");
        assertions.Intro = EditorGUILayout.Toggle(assertions.Intro);
        EditorGUILayout.LabelField("    bool");
        EditorGUILayout.EndHorizontal();

        EditorGUILayout.BeginHorizontal();
        EditorGUILayout.LabelField("NoBarDisplay:");
        assertions.NoBarDisplay = EditorGUILayout.Toggle(assertions.NoBarDisplay);
        EditorGUILayout.LabelField("    bool");
        EditorGUILayout.EndHorizontal();

        EditorGUILayout.BeginHorizontal();
        EditorGUILayout.LabelField("WinPose:");
        assertions.WinPose = EditorGUILayout.Toggle(assertions.WinPose);
        EditorGUILayout.LabelField("    bool");
        EditorGUILayout.EndHorizontal();

        EditorGUILayout.BeginHorizontal();
        EditorGUILayout.LabelField("NoFrontLayer:");
        assertions.NoFrontLayer = EditorGUILayout.Toggle(assertions.NoFrontLayer);
        EditorGUILayout.LabelField("    bool");
        EditorGUILayout.EndHorizontal();

        EditorGUILayout.BeginHorizontal();
        EditorGUILayout.LabelField("Invisible:");
        assertions.NoBackLayer = EditorGUILayout.Toggle(assertions.NoBackLayer);
        EditorGUILayout.LabelField("    bool");
        EditorGUILayout.EndHorizontal();
    }

    void EnvShake(EnvironmentShake shake)
    {
        EditorGUILayout.BeginHorizontal();
        EditorGUILayout.LabelField("TimeElasped:");
        shake.TimeElasped = EditorGUILayout.IntField(shake.TimeElasped);
        EditorGUILayout.LabelField("int");
        EditorGUILayout.EndHorizontal();

        EditorGUILayout.BeginHorizontal();
        EditorGUILayout.LabelField("Time:");
        shake.Time = EditorGUILayout.IntField(shake.Time);
        EditorGUILayout.LabelField("int");
        EditorGUILayout.EndHorizontal();

        EditorGUILayout.BeginHorizontal();
        EditorGUILayout.LabelField("Frequency:");
        shake.Frequency = EditorGUILayout.FloatField(shake.Frequency);
        EditorGUILayout.LabelField("float");
        EditorGUILayout.EndHorizontal();

        EditorGUILayout.BeginHorizontal();
        EditorGUILayout.LabelField("Amplitude:");
        shake.Amplitude = EditorGUILayout.FloatField(shake.Amplitude);
        EditorGUILayout.LabelField("float");
        EditorGUILayout.EndHorizontal();

        EditorGUILayout.BeginHorizontal();
        EditorGUILayout.LabelField("Phase:");
        shake.Phase = EditorGUILayout.FloatField(shake.Phase);
        EditorGUILayout.LabelField("float");
        EditorGUILayout.EndHorizontal();
    }

    void Match(FightEngine engine)
    {
        EditorGUILayout.BeginHorizontal();
        EditorGUILayout.LabelField("Round.Time:");
        engine.TickCount = EditorGUILayout.IntField(engine.TickCount);
        EditorGUILayout.LabelField("int");
        EditorGUILayout.EndHorizontal();

        EditorGUILayout.BeginHorizontal();
        EditorGUILayout.LabelField("RoundNumber:");
        engine.RoundNumber = EditorGUILayout.IntField(engine.RoundNumber);
        EditorGUILayout.LabelField("int");
        EditorGUILayout.EndHorizontal();

        EditorGUILayout.BeginHorizontal();
        EditorGUILayout.LabelField("RoundState:");
        EditorGUILayout.LabelField((int)engine.RoundState + " - " + engine.RoundState.ToString());
        EditorGUILayout.LabelField("int");
        EditorGUILayout.EndHorizontal();

        EditorGUILayout.BeginHorizontal();
        EditorGUILayout.LabelField("MatchNumber:");
        engine.MatchNumber = EditorGUILayout.IntField(engine.MatchNumber);
        EditorGUILayout.LabelField("int");
        EditorGUILayout.EndHorizontal();

        EditorGUILayout.BeginHorizontal();
        EditorGUILayout.LabelField("P1 Win:");
        EditorGUILayout.LabelField(engine.Team1.Wins.Count.ToString());
        EditorGUILayout.LabelField("int");
        EditorGUILayout.EndHorizontal();

        EditorGUILayout.BeginHorizontal();
        EditorGUILayout.LabelField("P2 Win:");
        EditorGUILayout.LabelField(engine.Team1.Wins.Count.ToString());
        EditorGUILayout.LabelField("int");
        EditorGUILayout.EndHorizontal();

        EditorGUILayout.BeginHorizontal();
        EditorGUILayout.LabelField("DrawGames:");
        engine.DrawGames = EditorGUILayout.IntField(engine.DrawGames);
        EditorGUILayout.LabelField("int");
        EditorGUILayout.EndHorizontal();
    }

    void Pause(Pause pause)
    {
        EditorGUILayout.BeginHorizontal();
        EditorGUILayout.LabelField("Hitpause:");
        pause.Hitpause = EditorGUILayout.Toggle(pause.Hitpause);
        EditorGUILayout.LabelField("    bool");
        EditorGUILayout.EndHorizontal();

        EditorGUILayout.BeginHorizontal();
        EditorGUILayout.LabelField("Pausebackgrounds:");
        pause.Pausebackgrounds = EditorGUILayout.Toggle(pause.Pausebackgrounds);
        EditorGUILayout.LabelField("    bool");
        EditorGUILayout.EndHorizontal();

        EditorGUILayout.BeginHorizontal();
        EditorGUILayout.LabelField("Time:");
        EditorGUILayout.LabelField((pause.Totaltime - pause.ElapsedTime).ToString());
        EditorGUILayout.LabelField("int");
        EditorGUILayout.EndHorizontal();

        EditorGUILayout.BeginHorizontal();
        EditorGUILayout.LabelField("Commandbuffertime:");
        pause.Commandbuffertime = EditorGUILayout.IntField(pause.Commandbuffertime);
        EditorGUILayout.LabelField("int");
        EditorGUILayout.EndHorizontal();
    }


    void AnimationManager(AnimationManager animationManager)
    {
        if (!m_foldoutAnimations.ContainsKey(animationManager.GetHashCode()))
            m_foldoutAnimations.Add(animationManager.GetHashCode(), false);

        EditorGUILayout.BeginVertical("ObjectFieldThumb");
        {
            m_foldoutAnimations[animationManager.GetHashCode()] = EditorGUILayout.Foldout(m_foldoutAnimations[animationManager.GetHashCode()], "animation:", true, "Foldout");
            if (m_foldoutAnimations[animationManager.GetHashCode()])
            {
                EditorGUILayout.BeginHorizontal();
                {
                    EditorGUILayout.LabelField("action.no: ");
                    EditorGUILayout.LabelField(animationManager.CurrentAnimation.Number.ToString());
                    EditorGUILayout.LabelField("int");
                }
                EditorGUILayout.EndHorizontal();

                EditorGUILayout.BeginHorizontal();
                {
                    EditorGUILayout.LabelField("duration: ");
                    EditorGUILayout.LabelField(animationManager.CurrentAnimation.TotalTime.ToString());
                    EditorGUILayout.LabelField("int");
                }
                EditorGUILayout.EndHorizontal();

                if (animationManager.CurrentElement != null)
                {
                    EditorGUILayout.BeginHorizontal();
                    {
                        EditorGUILayout.LabelField("elem.no: ");
                        EditorGUILayout.LabelField(animationManager.CurrentElement.Id.ToString());
                        EditorGUILayout.LabelField("int");
                    }
                    EditorGUILayout.EndHorizontal();

                    EditorGUILayout.BeginHorizontal();
                    {
                        EditorGUILayout.LabelField("Group/Index: ");
                        EditorGUILayout.LabelField(animationManager.CurrentElement.SpriteId.ToString());
                        EditorGUILayout.LabelField("sprite id");
                    }
                    EditorGUILayout.EndHorizontal();

                    EditorGUILayout.BeginHorizontal();
                    {
                        EditorGUILayout.LabelField("Axis: ");
                        EditorGUILayout.LabelField(animationManager.CurrentElement.Offset.ToString());
                        EditorGUILayout.LabelField("vector2");
                    }
                    EditorGUILayout.EndHorizontal();
                }
            }
        }
        EditorGUILayout.EndVertical();
    }

    void Cmd(ICommandManager commandManager)
    {
        if (!m_foldoutCmd.ContainsKey(commandManager.GetHashCode()))
            m_foldoutCmd.Add(commandManager.GetHashCode(), false);

        EditorGUILayout.BeginVertical("ObjectFieldThumb");
        {
            m_foldoutCmd[commandManager.GetHashCode()] = EditorGUILayout.Foldout(m_foldoutCmd[commandManager.GetHashCode()], "cmd.buffer.time:", true, "Foldout");
            if (m_foldoutCmd[commandManager.GetHashCode()])
            {
                foreach (UnityMugen.Commands.Command command in commandManager.Commands)
                {
                    int hasCodeCmd = commandManager.GetHashCode() + command.Name.GetHashCode();
                    int active = commandManager.CommandCount[command.Name].Value; /* .IsActive(command.Name) ? 1 : 0*/;

                    if (!m_toggleActiveCmd.ContainsKey(hasCodeCmd))
                        m_toggleActiveCmd.Add(hasCodeCmd, false);

                    EditorGUILayout.BeginHorizontal();
                    EditorGUIUtility.labelWidth = 100;
                    m_toggleActiveCmd[hasCodeCmd] = EditorGUILayout.Toggle(command.Name, m_toggleActiveCmd[hasCodeCmd]);
                    if (active > 0 && m_toggleActiveCmd[hasCodeCmd] == true)
                        EditorApplication.isPaused = true;

                    EditorGUILayout.LabelField(active.ToString());
                    EditorGUILayout.LabelField("bool");

                    EditorGUILayout.EndHorizontal();
                }
            }
        }
        EditorGUILayout.EndVertical();
    }

    void ShowVarsInt(string label, ListIterator<int> variables, ref bool foldout)
    {
        EditorGUILayout.BeginVertical("ObjectFieldThumb");
        {
            foldout = EditorGUILayout.Foldout(foldout, label, true, "Foldout");
            if (foldout)
            {
                for (int i = 0; i < variables.Count; i++)
                {
                    EditorGUILayout.BeginHorizontal();
                    EditorGUILayout.LabelField(string.Format("ID: {0}", i), m_optionsVars);
                    EditorGUIUtility.labelWidth = 5;
                    EditorGUILayout.LabelField("VALUE:");
                    variables[i] = EditorGUILayout.IntField(variables[i]);
                    GUILayout.FlexibleSpace();
                    EditorGUILayout.EndHorizontal();
                }
            }
        }
        EditorGUILayout.EndVertical();
    }

    void ShowVarsFloat(string label, ListIterator<float> variables, ref bool foldout)
    {
        EditorGUILayout.BeginVertical("ObjectFieldThumb");
        {
            foldout = EditorGUILayout.Foldout(foldout, label, true, "Foldout");
            if (foldout)
            {
                for (int i = 0; i < variables.Count; i++)
                {
                    EditorGUILayout.BeginHorizontal();
                    EditorGUILayout.LabelField(string.Format("ID: {0}", i), m_optionsVars);
                    EditorGUIUtility.labelWidth = 5;
                    EditorGUILayout.LabelField("VALUE:");
                    variables[i] = EditorGUILayout.FloatField(variables[i]);
                    GUILayout.FlexibleSpace();
                    EditorGUILayout.EndHorizontal();
                }
            }
        }
        EditorGUILayout.EndVertical();
    }
}
