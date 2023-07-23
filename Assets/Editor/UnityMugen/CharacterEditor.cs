
/*
   int selected = 0;
   string[] options = new string[]
   {
"Option1", "Option2", "Option3",
   };
   selected = EditorGUILayout.Popup("Label", selected, options);
   
    /////////////////////////////////////////////

    GenericMenu previewMenu = new GenericMenu();
    previewMenu.AddDisabledItem(new GUIContent("Paste Here"));
    previewMenu.ShowAsContext();

    /////////////////////////////////////////////

    EventType eventType = Event.current.type;
    if (eventType == EventType.MouseDown)
    {
        GenericMenu fileMenu = new GenericMenu();
        Rect fileRect = EditorGUILayout.BeginHorizontal();
        //fileMenu.AddSeparator("");
        fileMenu.AddItem(new GUIContent("Exit"), false, Close);
        fileMenu.DropDown(fileRect);
        EditorGUILayout.EndHorizontal();
    }
*/

/*
public class RectsIndex
{
    public RectsIndex(ClsnType clsnType, int index)
    {
        this.clsnType = clsnType;
        this.index = index;
    }
    public ClsnType clsnType;
    public int index;
}

public enum TypeSection
{
    None,
    Definitions,
    Sprites,
    Animations,
    Commands,
    Sounds
}


public class CharacterEditor : EditorWindow
{

    static EditorWindow window;
    SerializedObject serializedObject;

    float iconWidth = 36;
    int spaceBetweenIcons = 8;

    List<WindowSection> sections;
    WindowSection topMenuSide;
    WindowSection topContentSide;

    WindowSection bodyNoneCharacterSide;

    WindowSection bodyDefinitionSide;

    WindowSection bodySpriteLeftSide;
    WindowSection bodySpriteRightSide;

    WindowSection bodyAnimationLeftSide;
    WindowSection bodyAnimationRightSide;
    WindowSection bodyAnimationBottonRightSide;

    WindowSection bodyCommandSide;

    WindowSection bodySoundLeftSide;
    WindowSection bodySoundRightSide;

    GUISkin skin;

    // Menu
    Texture2D def, sff, air, cmd, snd;

    // Contents
    Texture2D play, stop, add, remove, duplicate, open;
    Texture2D addclsn1, addclsn2, delclsns, delanimclsns, clsnprev, clsntype;
    Texture2D up, down, left, right;

    Texture2D timeSpace, markerLine;
    
    Transform transformExample;
    Transform exampleOffset;
    SpriteRenderer exampleRender;

    Transform transformExampleOnion;
    Transform exampleOffsetOnion;
    SpriteRenderer exampleRenderOnion;

    PaletteManager paletteManagerSprites;
    AudiosClipsManager audiosClipsManager;

    GameObject oldCharacter;
    GameObject character;

    Player player;
    Palette palette;
    PlayerProfileManager playerProfileManager;
    CommandList commandList;
    PlayerConstants playerConstants;

    SpriteFEManager spriteFEManager;

    AnimationFEManager animationFEManager;
    AnimationsFE currentAnimationFE;
    AnimationElementFE currentAnimationElementFE;

    //FPS set up
    public double FPS;
    public long ticks1;
    public long ticks2;
    public double interval;
    bool playAnim = false;
    //


    TypeSection currentContext = TypeSection.None;

    float saturation = 1;
    public SpriteData currentSpriteData;
    public SpriteFE currentSpriteFEOnion;
    public int scrollPosSprites, scrollPosSpritesOnion, scrollPosPalette;
    public bool onionSkin = false;

    PaletteList swatch;

    [MenuItem("UnityMugen/Character Editor")]
    public static void Init()
    {
        window = GetWindow(typeof(CharacterEditor));
        window.titleContent = new GUIContent("Unity Mugen Complex");
        window.minSize = new Vector2(440, 440);
        window.maxSize = new Vector2(440, 440);
        window.Show();
    }

    private void OnEnable()
    {
        skin = Resources.Load<GUISkin>("Skins/PIAPixelArtEditorSkin");
        def = FightEngineDatabase.Instance.GetTexture("def");
        sff = FightEngineDatabase.Instance.GetTexture("sff");
        air = FightEngineDatabase.Instance.GetTexture("air");
        cmd = FightEngineDatabase.Instance.GetTexture("cmd");
        snd = FightEngineDatabase.Instance.GetTexture("snd");

        add = FightEngineDatabase.Instance.GetTexture("add");
        remove = FightEngineDatabase.Instance.GetTexture("remove");
        duplicate = FightEngineDatabase.Instance.GetTexture("duplicate");
        play = FightEngineDatabase.Instance.GetTexture("play");
        stop = FightEngineDatabase.Instance.GetTexture("stop");
        open = FightEngineDatabase.Instance.GetTexture("open");

        addclsn1 = FightEngineDatabase.Instance.GetTexture("addclsn1");
        addclsn2 = FightEngineDatabase.Instance.GetTexture("addclsn2");
        delclsns = FightEngineDatabase.Instance.GetTexture("delclsns");
        delanimclsns = FightEngineDatabase.Instance.GetTexture("delanimclsns");
        clsnprev = FightEngineDatabase.Instance.GetTexture("clsnprev");
        clsntype = FightEngineDatabase.Instance.GetTexture("clsntype");

        up = FightEngineDatabase.Instance.GetTexture("up");
        down = FightEngineDatabase.Instance.GetTexture("down");
        left = FightEngineDatabase.Instance.GetTexture("left");
        right = FightEngineDatabase.Instance.GetTexture("right");

        timeSpace = FightEngineDatabase.Instance.GetTexture("TimeSpace.png");
        markerLine = FightEngineDatabase.Instance.GetTexture("white.png");
        
        Color color1 = new Color(0.2075472f, 0.2075472f, 0.2075472f);
        Color color2 = new Color(0.1792453f, 0.1792453f, 0.1792453f);
        Color color3 = new Color(0.1603774f, 0.1603774f, 0.1603774f);

        sections = new List<WindowSection>();
        topMenuSide = new WindowSection(new Rect(0f, 0f, position.width, 20f), color1);
        topContentSide = new WindowSection(new Rect(0f, 20, position.width, 35), color2);

        bodyNoneCharacterSide = new WindowSection(new Rect(0f, 55f, position.width, position.height - topMenuSide.GetRect().height - topContentSide.GetRect().height), Color.clear);

        bodyDefinitionSide = new WindowSection(new Rect(0f, 55f, position.width, position.height - topMenuSide.GetRect().height - topContentSide.GetRect().height), Color.clear);

        bodySpriteLeftSide = new WindowSection(new Rect(0f, 55f, position.width / 2f, position.height - topMenuSide.GetRect().height - topContentSide.GetRect().height), Color.clear);
        bodySpriteRightSide = new WindowSection(new Rect(position.width / 2f, 55f, position.width / 2f, position.height - topMenuSide.GetRect().height - topContentSide.GetRect().height), color1);

        bodyAnimationLeftSide = new WindowSection(new Rect(0f, 55f, position.width / 2f, position.height - topMenuSide.GetRect().height - topContentSide.GetRect().height), color1);
        bodyAnimationRightSide = new WindowSection(new Rect(position.width / 2f, 55f, position.width / 2f, position.height - topMenuSide.GetRect().height - topContentSide.GetRect().height - 60f), color3);
        bodyAnimationBottonRightSide = new WindowSection(new Rect(position.width / 2f, position.height - topMenuSide.GetRect().height - topContentSide.GetRect().height, position.width / 2f, 60f), Color.clear);

        bodyCommandSide = new WindowSection(new Rect(0f, 55f, position.width, position.height - topMenuSide.GetRect().height - topContentSide.GetRect().height), Color.clear);

        bodySoundLeftSide = new WindowSection(new Rect(0f, 55f, position.width / 2f, position.height - topMenuSide.GetRect().height - topContentSide.GetRect().height), Color.clear);
        bodySoundRightSide = new WindowSection(new Rect(position.width / 2f, 55f, position.width / 2f, position.height - topMenuSide.GetRect().height - topContentSide.GetRect().height), color1);


        sections.Add(topMenuSide);
        sections.Add(topContentSide);
        sections.Add(bodyNoneCharacterSide);

        //FPS set up
        FPS = 60;
        ticks1 = 0;
        ticks2 = 0;
        interval = (double)Stopwatch.Frequency / FPS;
        ////////////
    }

    protected void OnGUI()
    {
        if (window == null)
            Init();

        serializedObject = new SerializedObject(this);

        topMenuSide.SetRect(0f, 0f, position.width, 20f);
        topContentSide.SetRect(0f, 20f, position.width, 35f);

        float heightTopMenu = topMenuSide.GetRect().height;
        float heightTopContent = topContentSide.GetRect().height;
        float menuContent = heightTopMenu + heightTopContent;

        if (character == null)
        {
            bodyNoneCharacterSide.SetRect(0f, 55, position.width, position.height - topMenuSide.GetRect().height - topContentSide.GetRect().height);
        }
        else if (currentContext == TypeSection.Definitions)
        {
            window.minSize = new Vector2(440, 440);
            window.maxSize = new Vector2(440, 440);
            bodyDefinitionSide.SetRect(0f, menuContent, position.width, position.height - topMenuSide.GetRect().height - topContentSide.GetRect().height);
        }
        else if (currentContext == TypeSection.Sprites)
        {
            window.minSize = new Vector2(670, 500);
            window.maxSize = new Vector2(670, 500);
            bodySpriteLeftSide.SetRect(0f, menuContent, 250, position.height - topMenuSide.GetRect().height - topContentSide.GetRect().height);
            bodySpriteRightSide.SetRect(250, menuContent, position.width - 250, position.height - topMenuSide.GetRect().height - topContentSide.GetRect().height);
        }
        else if (currentContext == TypeSection.Animations)
        {
            window.minSize = new Vector2(900, 520);
            window.maxSize = new Vector2(1200, 520);
            bodyAnimationLeftSide.SetRect(0f, menuContent, 280, position.height - topMenuSide.GetRect().height - topContentSide.GetRect().height);
            bodyAnimationRightSide.SetRect(280, menuContent, position.width - 280, position.height - topMenuSide.GetRect().height - topContentSide.GetRect().height - 70f);
            bodyAnimationBottonRightSide.SetRect(280, menuContent + bodyAnimationRightSide.GetRect().height, position.width - 280, 70f);
        }
        else if (currentContext == TypeSection.Commands)
        {
            window.minSize = new Vector2(440, 440);
            window.maxSize = new Vector2(440, 440);
            bodyCommandSide.SetRect(0f, menuContent, position.width, position.height - topMenuSide.GetRect().height - topContentSide.GetRect().height);
        }
        else if (currentContext == TypeSection.Sounds)
        {
            window.minSize = new Vector2(600, 300);
            window.maxSize = new Vector2(900, 300);
            bodySoundLeftSide.SetRect(0f, menuContent, 250, position.height - topMenuSide.GetRect().height - topContentSide.GetRect().height);
            bodySoundRightSide.SetRect(250, menuContent, position.width - 250, position.height - topMenuSide.GetRect().height - topContentSide.GetRect().height);
        }
        
        foreach (var item in sections)
        {
            GUI.DrawTexture(item.GetRect(), item.GetTexture());
        }

        DrawMenuSection();
        DrawContentSection();

        if (character == null)
        {
            DrawBodyNoneCharacterSection();
        }
        else if (currentContext == TypeSection.Definitions)
        {
            DrawBodyDefinitionSection();
        }
        else if (currentContext == TypeSection.Sprites)
        {
            DrawBodySpriteLeftSection();
            DrawBodySpriteRightSection();
        }
        else if (currentContext == TypeSection.Animations)
        {
            Play();
            DrawLeftAnimationsSection();
            DrawBottonRightAnimationsSection();
        }
        else if (currentContext == TypeSection.Commands)
        {
            DrawBodyCommandsSection();
        }
        else if (currentContext == TypeSection.Sounds)
        {
            DrawBodySoundLeftSection();
            DrawBodySoundRightSection();
        }
        window.Repaint();
        serializedObject.ApplyModifiedProperties();
    }

    private void DrawMenuSection()
    {
        Rect topMenu = topMenuSide.GetRect();
        GUILayout.BeginArea(topMenu);
        {
            GUILayout.BeginHorizontal();
            {
                EditorGUILayout.BeginHorizontal();
                character = (GameObject)EditorGUILayout.ObjectField("", character, typeof(GameObject), true, GUILayout.Width(177));
                if (character != null && character != oldCharacter)
                {
                    oldCharacter = character;
                    player = character.GetComponent<Player>();
                    
                    spriteFEManager = player.spriteFEManager;
                    playerProfileManager = player.profile;
                    commandList = playerProfileManager.commandsList;
                    playerConstants = player.playerConstants;
                    audiosClipsManager = playerProfileManager.audiosClipsManager;

                    palette = player.paletteOverride;
                    if (palette.palettes.Length > 0)
                    {
                        swatch = palette.palettes[0];
                    }
                    currentSpriteData = spriteFEManager.spriteDatas[scrollPosSprites];

                    animationFEManager = player.animationFEManager;
                    currentAnimationFE = animationFEManager.GetAnimation(0);
                    currentAnimationElementFE = currentAnimationFE.animationElement[0];
                    SetAnimation(currentAnimationFE);
                    
                    GameObject characterTeste = new GameObject("RenderAnimations");
                    transformExample = characterTeste.transform;
                    GameObject offsetTeste = new GameObject("Offset");
                    exampleOffset = offsetTeste.transform;
                    GameObject spriteRendererTeste = new GameObject("SpriteRenderer");
                    exampleRender = spriteRendererTeste.AddComponent<SpriteRenderer>();
                    spriteRendererTeste.transform.SetParent(offsetTeste.transform);
                    exampleRender.material = new Material(Shader.Find("UnityMugen/Sprites/ColorSwap"));
                    exampleRender.sharedMaterial.SetTexture("_SwapTex", Resources.Load("swap_texture") as Texture);
                   // paletteManagerSprites = exampleRender.gameObject.AddComponent<PaletteManager>();
                    offsetTeste.transform.SetParent(characterTeste.transform);
                    transformExample.gameObject.SetActive(false);
                    
                    GameObject characterTesteOnion = new GameObject("RenderOnion");
                    transformExampleOnion = characterTesteOnion.transform;
                    GameObject offsetTesteOnion = new GameObject("Offset");
                    exampleOffsetOnion = offsetTesteOnion.transform;
                    GameObject spriteRendererTesteOnion = new GameObject("SpriteRenderer");
                    exampleRenderOnion = spriteRendererTesteOnion.AddComponent<SpriteRenderer>();
                    exampleRenderOnion.sortingOrder = -1;
                    spriteRendererTesteOnion.transform.SetParent(offsetTesteOnion.transform);
                    exampleRenderOnion.material = new Material(Shader.Find("UnityMugen/Sprites/GrayScale"));
                    exampleRenderOnion.sharedMaterial.SetFloat("_EffectAmount", 1);
                    offsetTesteOnion.transform.SetParent(characterTesteOnion.transform);
                    transformExampleOnion.gameObject.SetActive(false);

                    base.Repaint();
                }
                GUILayout.EndHorizontal();

                
                Rect characterRect = EditorGUILayout.BeginHorizontal();
                {
                    if (GUILayout.Button("Character", EditorStyles.toolbarDropDown, GUILayout.Width(100)))
                    {
                        GenericMenu characterMenu = new GenericMenu();

                        characterMenu.AddItem(new GUIContent("New Character"), false, Teste);
                        characterMenu.AddSeparator("");
                        if (true )//*teste/
{
    characterMenu.AddItem(new GUIContent("Save"), false, Teste);
                        }
                        else
                        {
                            characterMenu.AddDisabledItem(new GUIContent("Save"));
                        }

                        characterMenu.AddSeparator("");
                        characterMenu.AddItem(new GUIContent("Exit"), false, Close);
                        characterMenu.DropDown(characterRect);
                    }
                }
                GUILayout.EndHorizontal();

                Rect helpRect = EditorGUILayout.BeginHorizontal();
                if (GUILayout.Button("Help", EditorStyles.toolbarDropDown, GUILayout.Width(60)))
                {
                    GenericMenu helpMenu = new GenericMenu();
                    helpMenu.AddItem(new GUIContent("Documentation"), false, OpenURL, "https://levelalfaomega.com/documentation");
                    helpMenu.AddItem(new GUIContent("Email Support"), false, OpenURL, "mailto:levelalfaomega@gmail.com");
                    helpMenu.DropDown(helpRect);
                }
                GUILayout.EndHorizontal();
                
                GUILayout.FlexibleSpace();
                
                GUILayout.Space(20);
                GUILayout.Box("Version 0.01   ", EditorStyles.label);
            }
            GUILayout.EndHorizontal();

        }
        GUILayout.EndArea();
    }

    private void DrawContentSection()
    {
        Rect topContent = topContentSide.GetRect();
        GUILayout.BeginArea(topContent);
        {
            GUILayout.BeginHorizontal();
            {
                GUI.enabled = (character != null);
                
                if (GUILayout.Button(new GUIContent(def, "Definitions"), skin.GetStyle("toolbarbutton"), GUILayout.MaxWidth(iconWidth), GUILayout.MaxHeight(iconWidth)))
                {
                    currentContext = TypeSection.Definitions;
                    playAnim = false;
                    
                    sections.Clear();
                    sections.Add(topMenuSide);
                    sections.Add(topContentSide);
                    sections.Add(bodyDefinitionSide);

                    transformExample.gameObject.SetActive(false);
                }
                if (GUILayout.Button(new GUIContent(sff, "Sprites"), skin.GetStyle("toolbarbutton"), GUILayout.MaxWidth(iconWidth), GUILayout.MaxHeight(iconWidth)))
                {
                    currentContext = TypeSection.Sprites;
                    playAnim = false;

                    sections.Clear();
                    sections.Add(topMenuSide);
                    sections.Add(topContentSide);
                    sections.Add(bodySpriteLeftSide);
                    sections.Add(bodySpriteRightSide);

                    ResetRender();

                    exampleOffset.transform.localPosition = new Vector3(currentSpriteData.spriteFE.offset.x * Constant.Scale, currentSpriteData.spriteFE.offset.y * Constant.Scale);
                    exampleRender.sprite = currentSpriteData.spriteFE.sprite;
                    transformExample.gameObject.SetActive(true);

                    if (palette != null && palette.palettes != null && palette.palettes.Length > 0)
                    {
                        swatch = palette.palettes[scrollPosPalette];
                        paletteManagerSprites.SetExternalPalette(scrollPosPalette, palette, exampleRender);
                        base.Repaint();
                    }
                }
                if (GUILayout.Button(new GUIContent(air, "Animations"), skin.GetStyle("toolbarbutton"), GUILayout.MaxWidth(iconWidth), GUILayout.MaxHeight(iconWidth)))
                {
                    currentContext = TypeSection.Animations;

                    sections.Clear();
                    sections.Add(topMenuSide);
                    sections.Add(topContentSide);
                    sections.Add(bodyAnimationLeftSide);
                    sections.Add(bodyAnimationRightSide);
                    sections.Add(bodyAnimationBottonRightSide);

                    ResetRender();

                    transformExample.gameObject.SetActive(true);
                    DrawAnimation();
                }
                if (GUILayout.Button(new GUIContent(cmd, "Commands"), skin.GetStyle("toolbarbutton"), GUILayout.MaxWidth(iconWidth), GUILayout.MaxHeight(iconWidth)))
                {
                    currentContext = TypeSection.Commands;
                    playAnim = false;
                    
                    sections.Clear();
                    sections.Add(topMenuSide);
                    sections.Add(topContentSide);
                    sections.Add(bodyCommandSide);

                    transformExample.gameObject.SetActive(false);
                }
                if (GUILayout.Button(new GUIContent(snd, "Sounds"), skin.GetStyle("toolbarbutton"), GUILayout.MaxWidth(iconWidth), GUILayout.MaxHeight(iconWidth)))
                {
                    currentContext = TypeSection.Sounds;
                    playAnim = false;

                    sections.Clear();
                    sections.Add(topMenuSide);
                    sections.Add(topContentSide);
                    sections.Add(bodySoundLeftSide);
                    sections.Add(bodySoundRightSide);

                    transformExample.gameObject.SetActive(false);
                }
            }
            GUILayout.EndHorizontal();

        }
        GUILayout.EndArea();
    }

    void DrawBodyNoneCharacterSection()
    {
        Rect bodyContent = bodyNoneCharacterSide.GetRect();
        GUILayout.BeginArea(bodyContent);
        {
            GUILayout.BeginHorizontal();
            {
                GUILayout.Box("", GUILayout.ExpandWidth(true), GUILayout.Height(1));
                GUILayout.FlexibleSpace();
                GUILayout.Label("No Char selected");
                GUILayout.FlexibleSpace();
                GUILayout.Box("", GUILayout.ExpandWidth(true), GUILayout.Height(1));
            }
            GUILayout.EndHorizontal();
        }
        GUILayout.EndArea();
    }


    public int scrollPosSound;
    void DrawBodySoundLeftSection()
    {
        Rect bodyLeftContent = bodySoundLeftSide.GetRect();
        GUILayout.BeginArea(bodyLeftContent);
        {
            EditorGUILayout.LabelField("", GUI.skin.horizontalSlider);

            GUILayout.BeginHorizontal();
            {
                if (GUILayout.Button(new GUIContent(play, "Play sound."), GUILayout.MaxWidth(40), GUILayout.MaxHeight(30)))
                {

                }
                if (GUILayout.Button(new GUIContent(stop, "Stop sound"), GUILayout.MaxWidth(40), GUILayout.MaxHeight(30)))
                {

                }
            }
            GUILayout.EndHorizontal();

            GUILayout.BeginHorizontal();
            {
                int oldScrollPosSound = scrollPosSound;
                if (GUILayout.Button(left, skin.GetStyle("buttonSlice"), GUILayout.MaxWidth(iconWidth), GUILayout.MaxHeight(iconWidth)) && scrollPosSound > 0)
                    scrollPosSound--;
                scrollPosSound = (int)GUILayout.HorizontalSlider(scrollPosSound, 0f, (spriteFEManager.spriteDatas.Count - 1), new GUILayoutOption[]
                {
                    GUILayout.MaxWidth(110f)
                });
                if (GUILayout.Button(right, skin.GetStyle("buttonSlice"), GUILayout.MaxWidth(iconWidth), GUILayout.MaxHeight(iconWidth)) && scrollPosSound < spriteFEManager.spriteDatas.Count - 1)
                    scrollPosSound++;

                if (scrollPosSound != oldScrollPosSound)
                {
                    currentSpriteData = spriteFEManager.spriteDatas[scrollPosSound];
                    exampleOffset.transform.localPosition = new Vector3(currentSpriteData.spriteFE.offset.x * Constant.Scale, currentSpriteData.spriteFE.offset.y * Constant.Scale);
                    exampleRender.sprite = currentSpriteData.spriteFE.sprite;

                    base.Repaint();
                }


                EditorGUILayout.LabelField(scrollPosSound.ToString() + "/" + (spriteFEManager.spriteDatas.Count - 1), new GUILayoutOption[]
                {
                    GUILayout.MaxWidth(50f)
                });
            }
            GUILayout.EndHorizontal();

            GUILayout.BeginHorizontal();
            {
                EditorGUIUtility.labelWidth = 45f;
                currentSpriteData.spriteId.Group = EditorGUILayout.IntField("Group:", currentSpriteData.spriteId.Group);

                EditorGUILayout.Space();

                EditorGUIUtility.labelWidth = 45f;
                currentSpriteData.spriteId.Image = EditorGUILayout.IntField("Index:", currentSpriteData.spriteId.Image);
            }
            GUILayout.EndHorizontal();

            EditorGUILayout.Space();
            EditorGUILayout.Space();

            EditorGUILayout.LabelField("Channels: 2 (Stereo)");
            EditorGUILayout.LabelField("Sample rate: 11060 Hz");
            EditorGUILayout.LabelField("Bits per sample: 16 bits");
            EditorGUILayout.LabelField("Duration: 0.66666666 seconds");
        }
        GUILayout.EndArea();
    }

    void DrawBodySoundRightSection()
    {
        Rect bodyRightContent = bodySoundRightSide.GetRect();
        GUILayout.BeginArea(bodyRightContent);
        {
            Rect rect = new Rect(0, 0, bodyRightContent.width, bodyRightContent.height / 2);
            Rect rect2 = new Rect(0, bodyRightContent.height / 2, bodyRightContent.width, bodyRightContent.height / 2);
            AudioClip clip = audiosClipsManager.audios[0];
            AudioUtility.DrawWaveForm(clip, 0, rect);
            AudioUtility.DrawWaveForm(clip, 0, rect2);
        }
        GUILayout.EndArea();
    }


    Vector2 scrollPosRectDefinitions = new Vector2(0f, 0f);
    bool showPlayerProfile = false, showPlayerConstant = false;
    void DrawBodyDefinitionSection()
    {
        Rect bodyContent = bodyDefinitionSide.GetRect();
        GUILayout.BeginArea(bodyContent);
        {
            scrollPosRectDefinitions = GUILayout.BeginScrollView(scrollPosRectDefinitions, false, true, GUILayout.MaxHeight(bodyDefinitionSide.GetRect().height));
            {
                GUILayout.BeginVertical("GroupBox");
                {
                    GUILayout.BeginHorizontal();
                    
                    showPlayerProfile = EditorGUILayout.Foldout(showPlayerProfile, "Player Profile");

                    if (GUILayout.Button("?", GUILayout.Width(18), GUILayout.Height(18)))
                    {

                    }
                    GUILayout.EndHorizontal();

                    if (showPlayerProfile) { 
                        GUILayout.BeginVertical("GroupBox");
                        Editor playerProfileManagerScriptableObject = Editor.CreateEditor(playerProfileManager);
                        playerProfileManagerScriptableObject.OnInspectorGUI();
                        GUILayout.EndVertical();
                    }
                }
                GUILayout.EndVertical();

                GUILayout.BeginVertical("GroupBox");
                {
                    GUILayout.BeginHorizontal();
                    
                    showPlayerConstant = EditorGUILayout.Foldout(showPlayerConstant, "Player Constant");

                    if (GUILayout.Button("?", GUILayout.Width(18), GUILayout.Height(18)))
                    {

                    }
                    GUILayout.EndHorizontal();

                    if (showPlayerConstant)
                    {
                        GUILayout.BeginVertical("GroupBox");
                        Editor playerConstantsScriptableObject = Editor.CreateEditor(playerConstants);
                        playerConstantsScriptableObject.OnInspectorGUI();
                        GUILayout.EndVertical();
                    }
                }
                GUILayout.EndVertical();
                
                
            }
            GUILayout.EndScrollView();
        }
        GUILayout.EndArea();
    }

    Vector2 scrollPosRectCommands = new Vector2(0f, 0f);
    void DrawBodyCommandsSection()
    {
        Rect bodyContent = bodyCommandSide.GetRect();
        GUILayout.BeginArea(bodyContent);
        {
            scrollPosRectCommands = GUILayout.BeginScrollView(scrollPosRectCommands, false, true, GUILayout.MaxHeight(bodyCommandSide.GetRect().height));
            {
                GUILayout.BeginHorizontal("GroupBox");
                GUILayout.Label("Commands");
                GUILayout.FlexibleSpace();
                if (GUILayout.Button("?", GUILayout.Width(18), GUILayout.Height(18)))
                {

                }
                GUILayout.EndHorizontal();
                    
                GUILayout.BeginVertical("GroupBox");
                Editor commandListScriptableObject = Editor.CreateEditor(commandList);
                commandListScriptableObject.OnInspectorGUI();
                GUILayout.EndVertical();
            }
            GUILayout.EndScrollView();
        }
        GUILayout.EndArea();
    }

    void DrawBodySpriteLeftSection()
    {
        Rect bodySpriteLeftContent = bodySpriteLeftSide.GetRect();
        GUILayout.BeginArea(bodySpriteLeftContent);
        {
            GUILayout.BeginHorizontal();
            {
                int oldScrollPosSprites = scrollPosSprites;
                if (GUILayout.Button(left, skin.GetStyle("buttonSlice"), GUILayout.MaxWidth(iconWidth), GUILayout.MaxHeight(iconWidth)) && scrollPosSprites > 0)
                    scrollPosSprites--;
                scrollPosSprites = (int)GUILayout.HorizontalSlider(scrollPosSprites, 0f, (spriteFEManager.spriteDatas.Count - 1), new GUILayoutOption[]
                {
                    GUILayout.MaxWidth(110f)
                });
                if (GUILayout.Button(right, skin.GetStyle("buttonSlice"), GUILayout.MaxWidth(iconWidth), GUILayout.MaxHeight(iconWidth)) && scrollPosSprites < spriteFEManager.spriteDatas.Count - 1)
                    scrollPosSprites++;

                if (scrollPosSprites != oldScrollPosSprites)
                {
                    currentSpriteData = spriteFEManager.spriteDatas[scrollPosSprites];
                    exampleOffset.transform.localPosition = new Vector3(currentSpriteData.spriteFE.offset.x * Constant.Scale, currentSpriteData.spriteFE.offset.y * Constant.Scale);
                    exampleRender.sprite = currentSpriteData.spriteFE.sprite;
                    
                    base.Repaint();
                }


                EditorGUILayout.LabelField(scrollPosSprites.ToString() + "/" + (spriteFEManager.spriteDatas.Count - 1), new GUILayoutOption[]
                {
                    GUILayout.MaxWidth(50f)
                });
            }
            GUILayout.EndHorizontal();

            GUILayout.BeginHorizontal();
            {
                EditorGUIUtility.labelWidth = 45f;
                currentSpriteData.spriteId.Group = EditorGUILayout.IntField("Group:", currentSpriteData.spriteId.Group);

                EditorGUILayout.Space();

                EditorGUIUtility.labelWidth = 45f;
                currentSpriteData.spriteId.Image = EditorGUILayout.IntField("Index:", currentSpriteData.spriteId.Image);
            }
            GUILayout.EndHorizontal();

            EditorGUILayout.Space();
            EditorGUILayout.Space();
            EditorGUILayout.Space();
            EditorGUILayout.Space();

            Sprite inicialSprite = currentSpriteData.spriteFE.sprite;
            currentSpriteData.spriteFE.sprite =  (Sprite)EditorGUILayout.ObjectField("Sprite:", currentSpriteData.spriteFE.sprite, typeof(Sprite), false);
            if (currentSpriteData.spriteFE.sprite != inicialSprite)
            {
                //RefreshTexture();
            }

            EditorGUILayout.BeginVertical();
            {
                EditorGUIUtility.labelWidth = 150f;
                currentSpriteData.spriteFE.offset = EditorGUILayout.Vector2Field("Axis:", currentSpriteData.spriteFE.offset);
            }
            EditorGUILayout.EndVertical();

            EditorGUILayout.Space();
            EditorGUILayout.Space();
            EditorGUILayout.Space();
            EditorGUILayout.Space();

            GUILayout.BeginVertical();
            {
                onionSkin = EditorGUILayout.ToggleLeft("OnionSkin", onionSkin);

                transformExampleOnion.gameObject.SetActive(onionSkin);
            }
            GUILayout.EndVertical();

            if (onionSkin)
            {

                GUILayout.BeginHorizontal();
                {
                    int oldScrollPosSpritesOnion = scrollPosSpritesOnion;
                    if (GUILayout.Button(left, skin.GetStyle("buttonSlice"), GUILayout.MaxWidth(iconWidth), GUILayout.MaxHeight(iconWidth)) && scrollPosSpritesOnion > 0)
                        scrollPosSpritesOnion--;
                    scrollPosSpritesOnion = (int)GUILayout.HorizontalSlider(scrollPosSpritesOnion, 0f, spriteFEManager.spriteDatas.Count - 1, new GUILayoutOption[]
                    {
                        GUILayout.MaxWidth(110f)
                    });
                    if (GUILayout.Button(right, skin.GetStyle("buttonSlice"), GUILayout.MaxWidth(iconWidth), GUILayout.MaxHeight(iconWidth)) && scrollPosSpritesOnion < spriteFEManager.spriteDatas.Count - 1)
                        scrollPosSpritesOnion++;

                    if (scrollPosSpritesOnion != oldScrollPosSpritesOnion)
                    {
                        currentSpriteFEOnion = spriteFEManager.spriteDatas[scrollPosSpritesOnion].spriteFE;
                        exampleOffsetOnion.transform.localPosition = new Vector3(currentSpriteFEOnion.offset.x * Constant.Scale, currentSpriteFEOnion.offset.y * Constant.Scale);
                        exampleRenderOnion.sprite = currentSpriteFEOnion.sprite;

                        base.Repaint();
                    }

                    EditorGUILayout.LabelField(scrollPosSpritesOnion.ToString() + "/" + (spriteFEManager.spriteDatas.Count - 1), new GUILayoutOption[]
                    {
                        GUILayout.MaxWidth(50f)
                    });
                }
                GUILayout.EndHorizontal();

                GUILayout.BeginVertical();
                {
                    GUILayout.Label("Saturação");
                    float inicialValue = saturation;
                    saturation = GUILayout.HorizontalSlider(saturation, 0, 1);
                    if (inicialValue != saturation)
                    {
                        exampleRenderOnion.sharedMaterial.SetFloat("_EffectAmount", saturation);
                    }
                }
                GUILayout.EndVertical();
                
            }
        }
        GUILayout.EndArea();
    }



    private int colorRef;
    void DrawBodySpriteRightSection()
    {
        Rect bodySpriteRightContent = bodySpriteRightSide.GetRect();
        GUILayout.BeginArea(bodySpriteRightContent);
        {
            GUILayout.BeginVertical();
            {
                EditorGUILayout.LabelField("", GUI.skin.horizontalSlider);
                
                GUILayout.BeginHorizontal();
                {
                    if (GUILayout.Button(new GUIContent(add, "Add a new palette"), GUILayout.MaxWidth(40), GUILayout.MaxHeight(30)))
                    {
                        if (palette.palettes.Length == 0)
                        {
                            PaletteList paletteList = new PaletteList();
                            paletteList.paletteId = "new palette";
                            paletteList.colors = new Color[] { };
                            for (int i = 0; i < 256; i++)
                            {
                                paletteList.colors = paletteList.colors.Add(Color.black);// = palette.palettes[0].colors;
                            }
                            palette.palettes = palette.palettes.Add(paletteList);

                            swatch = palette.palettes[0];
                        }
                        else if (palette.palettes.Length > 0)
                        {
                            PaletteList paletteList = new PaletteList();
                            paletteList.paletteId = "new palette";
                            paletteList.colors = new Color[] {};
                            for (int i = 0; i < palette.palettes[0].colors.Length; i++)
                            {
                                paletteList.colors = palette.palettes[0].colors;
                            }
                            palette.palettes = palette.palettes.Add(paletteList);

                            swatch = palette.palettes[palette.palettes.Length - 1];
                            scrollPosPalette = palette.palettes.Length - 1;
                        }
                        
                        base.Repaint();

                        EditorUtility.SetDirty(palette);
                        AssetDatabase.SaveAssets();
                        AssetDatabase.Refresh();
                    }
                    
                    if (GUILayout.Button(new GUIContent(open, "Import new palette .act"), GUILayout.MaxWidth(40), GUILayout.MaxHeight(30)))
                    {
                        List<Color> colors = new List<Color>();
                        string file = EditorUtility.OpenFilePanel("Falha ao Carregar o arquivo em formato .act", "", "act");
                        if (file.Length != 0)
                        {
                            var filedata = File.ReadAllBytes(file);

                            colors = new List<Color>(256);
                            for (int i = 0; i != 256; ++i)
                            {
                                var fileindex = i * 3;
                                Color32 color = new Color32(filedata[fileindex + 0], filedata[fileindex + 1], filedata[fileindex + 2], (byte)(i == 255 ? 0 : 255));
                                colors.Add(color);
                            }

                            string[] name = file.Split(new[] { "/", "." }, StringSplitOptions.None);

                            colors.Reverse();

                            PaletteList paletteList = new PaletteList();
                            paletteList.paletteId = name[name.Length - 2];
                            paletteList.colors = colors.ToArray();
                            palette.palettes = palette.palettes.Add(paletteList);
                        }

                        swatch = palette.palettes[palette.palettes.Length - 1];
                        scrollPosPalette = palette.palettes.Length - 1;
                        base.Repaint();

                        EditorUtility.SetDirty(palette);  
                        AssetDatabase.SaveAssets(); 
                        AssetDatabase.Refresh();
                    }


                    EditorGUI.BeginDisabledGroup(swatch == null);
                    if (GUILayout.Button(new GUIContent(remove, "Remove the current palette"), GUILayout.MaxWidth(40), GUILayout.MaxHeight(30)))
                    {
                        palette.palettes = palette.palettes.Where(val => val != palette.palettes[scrollPosPalette]).ToArray();
                        if (palette.palettes.Length == 0)
                            swatch = null;

                        scrollPosPalette = 0;
                        base.Repaint();

                        EditorUtility.SetDirty(palette);
                        AssetDatabase.SaveAssets();
                        AssetDatabase.Refresh();
                    }
                    EditorGUI.EndDisabledGroup();
                }
                GUILayout.EndHorizontal();

                if (swatch != null)
                {
                    GUILayout.BeginHorizontal();
                    {
                        int oldScrollPosPalette = scrollPosPalette;
                        if (GUILayout.Button(left, skin.GetStyle("buttonSlice"), GUILayout.MaxWidth(iconWidth), GUILayout.MaxHeight(iconWidth)) && scrollPosPalette > 0)
                            scrollPosPalette--;
                        scrollPosPalette = (int)GUILayout.HorizontalSlider(scrollPosPalette, 0f, (palette.palettes.Length - 1), new GUILayoutOption[]
                        {
                            GUILayout.MaxWidth(110f)
                        });
                        if (GUILayout.Button(right, skin.GetStyle("buttonSlice"), GUILayout.MaxWidth(iconWidth), GUILayout.MaxHeight(iconWidth)) && scrollPosPalette < palette.palettes.Length - 1)
                            scrollPosPalette++;

                        if (scrollPosPalette != oldScrollPosPalette)
                        {
                            swatch = palette.palettes[scrollPosPalette];
                            paletteManagerSprites.SetExternalPalette(scrollPosPalette, palette, exampleRender);
                            base.Repaint();
                        }

                        EditorGUILayout.LabelField(scrollPosPalette.ToString() + "/" + (palette.palettes.Length - 1), new GUILayoutOption[]
                        {
                        GUILayout.MaxWidth(50f)
                        });
                    }
                    GUILayout.EndHorizontal();

                    GUILayout.BeginHorizontal();
                    {
                        EditorGUIUtility.labelWidth = 55f;
                        swatch.paletteId = EditorGUILayout.TextField("Name:", swatch.paletteId);
                    }
                    GUILayout.EndHorizontal();

                    EditorGUILayout.Space();
                    EditorGUILayout.Space();


                    EditorGUILayout.LabelField("Palette", EditorStyles.boldLabel);
                    var startingRect = GUILayoutUtility.GetLastRect();
                    if (SwatchrPaletteDrawer.DrawColorPallete(swatch, ref colorRef, true))
                    {
                        Repaint();
                    }

                    if (swatch.numColors > 0)
                    {

                        var selectedColor = swatch.GetColor(colorRef);
                        int selectedColorRow = colorRef / SwatchrPaletteDrawer.itemsPerRow;
                        float selectedColorY = selectedColorRow * EditorGUIUtility.singleLineHeight + EditorGUIUtility.singleLineHeight;
                        var changeColorRect = new Rect(startingRect.x + SwatchrPaletteDrawer.itemsPerRow * EditorGUIUtility.singleLineHeight + 30, startingRect.y + selectedColorY, 64, EditorGUIUtility.singleLineHeight);

                        EditorGUI.BeginChangeCheck();
                        var newColor = EditorGUI.ColorField(changeColorRect, selectedColor);
                        if (EditorGUI.EndChangeCheck())
                        {
                            swatch.colors[colorRef] = newColor;
                            swatch.SignalChange();
                            GameViewRepaint();
                        }
                        int x = (int)(changeColorRect.x + changeColorRect.width + 2);
                        int y = (int)(changeColorRect.y + changeColorRect.height - EditorGUIUtility.singleLineHeight);
                        if (SwatchrPaletteDrawer.DrawDeleteButton(x, y))
                        {
                            if (colorRef + 1 < swatch.colors.Length)
                            {
                                Array.Copy(swatch.colors, colorRef + 1, swatch.colors, colorRef, swatch.colors.Length - colorRef - 1);
                            }
                            Array.Resize<Color>(ref swatch.colors, swatch.colors.Length - 1);
                            if (colorRef >= swatch.colors.Length)
                            {
                                colorRef = swatch.colors.Length - 1;
                                if (colorRef < 0)
                                {
                                    colorRef = 0;
                                }
                            }
                            swatch.SignalChange();
                            GameViewRepaint();
                        }
                    }
                }
            }
            GUILayout.EndHorizontal();
        }
        GUILayout.EndArea();
    }
    static EditorWindow gameview;
    public static void GameViewRepaint()
    {
        if (gameview == null)
        {
            System.Reflection.Assembly assembly = typeof(UnityEditor.EditorWindow).Assembly;
            System.Type type = assembly.GetType("UnityEditor.GameView");
            gameview = EditorWindow.GetWindow(type);
        }
        if (gameview != null)
        {
            gameview.Repaint();
        }
    }





    void Play()
    {
        if (playAnim)
        {
            ticks2 = Stopwatch.GetTimestamp();
            if (ticks2 >= ticks1 + interval)
            {
                ticks1 = Stopwatch.GetTimestamp();
                
                if (lastindexTimeLine != sliderFrames)
                {
                    tickIndex.TryGetValue(sliderFrames, out sliderFrames);
                    lastindexTimeLine = sliderFrames;
                    currentAnimationElementFE = animationFEManager.GetElementAnimation(currentAnimationFE.number, sliderFrames);
                }
                if (currentAnimationElementFE.time == -1)
                    playAnim = false;
                else
                {
                    if (sliderFrames == 1245)
                        sliderFrames = 0;

                    if (sliderFrames == totalFramesClip)
                        sliderFrames = 0;

                    sliderFrames++;
                }
            }

            SceneView.RepaintAll();
        }
        else
        {
            if (animationFEManager != null && currentAnimationFE != null)
            {
                int outIndex;
                tickIndex.TryGetValue(sliderFrames, out outIndex);
                if (lastindexTimeLine != outIndex)
                {
                    lastindexTimeLine = outIndex;
                    currentAnimationElementFE = animationFEManager.GetElementAnimation(currentAnimationFE.number, outIndex);
                }
            }
        }
    }

    
    Rect selectedToolRect = Rect.zero;
    public int scrollPosAnimationFE, scrollPosAnimationElementFE;
    private void DrawLeftAnimationsSection()
    {
        Rect bodyAnimationLeftContent = bodyAnimationLeftSide.GetRect();
        GUILayout.BeginArea(bodyAnimationLeftContent);
        {
            EditorGUILayout.LabelField("", GUI.skin.horizontalSlider);

            GUILayout.BeginHorizontal();
            {
                EditorGUI.BeginDisabledGroup(playAnim);
                if (GUILayout.Button(new GUIContent(add, "Add a new animation"), GUILayout.MaxWidth(40), GUILayout.MaxHeight(30)))
                {
                }
                if (GUILayout.Button(new GUIContent(remove, "Remove the current animation"), GUILayout.MaxWidth(40), GUILayout.MaxHeight(30)))
                {
                }
                if (GUILayout.Button(new GUIContent(duplicate, "Duplicate the current animation"), GUILayout.MaxWidth(40), GUILayout.MaxHeight(30)))
                {
                }
                EditorGUI.EndDisabledGroup();

                EditorGUI.BeginDisabledGroup(playAnim);
                if (GUILayout.Button(new GUIContent(play, "Play the current animation."), GUILayout.MaxWidth(40), GUILayout.MaxHeight(30)))
                {
                    playAnim = true;
                    sliderFrames = 0;
                    scrollPosAnimationElementFE = 0;
                    SetAnimation(currentAnimationFE);
                }
                EditorGUI.EndDisabledGroup();

                EditorGUI.BeginDisabledGroup(playAnim == false);
                if (GUILayout.Button(new GUIContent(stop, "Stop the animation"), GUILayout.MaxWidth(40), GUILayout.MaxHeight(30)))
                {
                    playAnim = false;
                }
                EditorGUI.EndDisabledGroup();
            }
            GUILayout.EndHorizontal();
            

            GUILayout.BeginHorizontal();
            {

                int oldScrollPosAnimationFE = scrollPosAnimationFE;
                if (GUILayout.Button(left, skin.GetStyle("buttonSlice"), GUILayout.MaxWidth(iconWidth), GUILayout.MaxHeight(iconWidth)) && scrollPosAnimationFE > 0)
                    scrollPosAnimationFE--;
                scrollPosAnimationFE = (int)GUILayout.HorizontalSlider(scrollPosAnimationFE, 0f, animationFEManager.animationsFE.Count - 1, new GUILayoutOption[]
                {
                    GUILayout.MaxWidth(110f)
                });
                if (GUILayout.Button(right, skin.GetStyle("buttonSlice"), GUILayout.MaxWidth(iconWidth), GUILayout.MaxHeight(iconWidth)) && scrollPosAnimationFE < animationFEManager.animationsFE.Count - 1)
                    scrollPosAnimationFE++;

                if (scrollPosAnimationFE != oldScrollPosAnimationFE)
                {
                    playAnim = false;
                    currentAnimationFE = animationFEManager.GetAnimationIndex(scrollPosAnimationFE);
                    currentAnimationElementFE = currentAnimationFE.animationElement[0];
                    sliderFrames = 0;
                    scrollPosAnimationElementFE = 0;
                    SetAnimation(currentAnimationFE);
                    DrawAnimation();
                    base.Repaint();
                }

                EditorGUILayout.LabelField(scrollPosAnimationFE.ToString() + "/" + (animationFEManager.animationsFE.Count - 1), new GUILayoutOption[]
                {
                    GUILayout.MaxWidth(50f)
                });
            }
            GUILayout.EndHorizontal();

            GUILayout.BeginHorizontal();
            {
                EditorGUIUtility.labelWidth = 55f;
                currentAnimationFE.number = EditorGUILayout.IntField("Number:", currentAnimationFE.number);

                EditorGUILayout.Space();

                EditorGUIUtility.labelWidth = 45f;
                currentAnimationFE.loopstart = EditorGUILayout.IntField("Loop:", currentAnimationFE.loopstart);
            }
            GUILayout.EndHorizontal();

            EditorGUILayout.Space();

            GUILayout.BeginHorizontal();
            {
                EditorGUIUtility.labelWidth = 55f;
                currentAnimationFE.name = EditorGUILayout.TextField("Name:", currentAnimationFE.name);
            }
            GUILayout.EndHorizontal();

            EditorGUILayout.LabelField("", GUI.skin.horizontalSlider);

            GUILayout.BeginHorizontal();
            {
                if (GUILayout.Button(new GUIContent(add, "Add the new frame in the animation"), GUILayout.MaxWidth(40), GUILayout.MaxHeight(30)))
                {
                    AnimationsFE animationsFE = animationFEManager.GetAnimation(currentAnimationFE.number);

                    AnimationElementFE newAnimationElementFE = new AnimationElementFE();
                    newAnimationElementFE.clnsAttack = new Rect[0];
                    newAnimationElementFE.clnsNormal = new Rect[0];
                    newAnimationElementFE.time = 10;
                    newAnimationElementFE.blending = new Blending();
                    animationsFE.animationElement.Insert(scrollPosAnimationElementFE, newAnimationElementFE);
                    
                    SetAnimation(currentAnimationFE);

                    SetAnimationElement(sliderFrames);
                }
                if (GUILayout.Button(new GUIContent(remove, "Remove the current frame"), GUILayout.MaxWidth(40), GUILayout.MaxHeight(30)))
                {
                    currentAnimationFE.animationElement.Remove(currentAnimationElementFE);

                    SetAnimation(currentAnimationFE);
                }
            }
            GUILayout.EndHorizontal();

            GUILayout.BeginHorizontal();
            {
                int oldScrollPosAnimationElementFE = scrollPosAnimationElementFE;
                if (GUILayout.Button(left, skin.GetStyle("buttonSlice"), GUILayout.MaxWidth(iconWidth), GUILayout.MaxHeight(iconWidth)) && scrollPosAnimationElementFE > 0)
                    scrollPosAnimationElementFE--;
                scrollPosAnimationElementFE = (int)GUILayout.HorizontalSlider(scrollPosAnimationElementFE, 0f, currentAnimationFE.animationElement.Count - 1, new GUILayoutOption[]
                {
                    GUILayout.MaxWidth(110f)
                });
                if (GUILayout.Button(right, skin.GetStyle("buttonSlice"), GUILayout.MaxWidth(iconWidth), GUILayout.MaxHeight(iconWidth)) && scrollPosAnimationElementFE < currentAnimationFE.animationElement.Count - 1)
                    scrollPosAnimationElementFE++;

                if (scrollPosAnimationElementFE != oldScrollPosAnimationElementFE)
                {
                    currentAnimationElementFE = currentAnimationFE.animationElement[scrollPosAnimationElementFE];
                    sliderFrames = firstFrameIndex[scrollPosAnimationElementFE];
                    DrawAnimation();
                    base.Repaint();
                }

                EditorGUILayout.LabelField(scrollPosAnimationElementFE.ToString() + "/" + (currentAnimationFE.animationElement.Count - 1), new GUILayoutOption[]
                {
                    GUILayout.MaxWidth(50f)
                });
            }
            GUILayout.EndHorizontal();


            GUILayout.BeginHorizontal();
            {
                EditorGUIUtility.labelWidth = 45f;
                currentAnimationElementFE.spriteId.Group = EditorGUILayout.IntField("Group:", currentAnimationElementFE.spriteId.Group);

                EditorGUILayout.Space();

                EditorGUIUtility.labelWidth = 45f;
                currentAnimationElementFE.spriteId.Image = EditorGUILayout.IntField("Image:", currentAnimationElementFE.spriteId.Image);
            }
            GUILayout.EndHorizontal();

            EditorGUILayout.Space();

            GUILayout.BeginHorizontal();
            {
                EditorGUIUtility.labelWidth = 45f;
                currentAnimationElementFE.time = EditorGUILayout.IntField("Time:", currentAnimationElementFE.time, new GUILayoutOption[]
                {
                    GUILayout.MaxWidth(100f)
                });

                EditorGUIUtility.labelWidth = 30f;
                currentAnimationElementFE.flip = (SpriteEffects)EditorGUILayout.EnumPopup("Flip:", currentAnimationElementFE.flip);
            }
            GUILayout.EndHorizontal();

            EditorGUILayout.Space();

            EditorGUILayout.BeginVertical();
            {
                EditorGUIUtility.labelWidth = 150f;
                currentAnimationElementFE.offset = EditorGUILayout.Vector2Field("Axis:", currentAnimationElementFE.offset);
                EditorGUILayout.Space();
            }
            EditorGUILayout.EndVertical();

            EditorGUILayout.Space();

            GUILayout.BeginHorizontal();
            {
                EditorGUIUtility.labelWidth = 150f;
                currentAnimationElementFE.scale = EditorGUILayout.Vector2Field("Scale:", currentAnimationElementFE.scale);
            }
            GUILayout.EndHorizontal();

            EditorGUILayout.Space();

            GUILayout.BeginHorizontal();
            {
                EditorGUIUtility.labelWidth = 40f;
                currentAnimationElementFE.rotate = EditorGUILayout.FloatField("Angle:", currentAnimationElementFE.rotate, new GUILayoutOption[]
                {
                    GUILayout.MaxWidth(85f)
                });

                EditorGUIUtility.labelWidth = 85f;
                currentAnimationElementFE.blending.BlendType = (BlendType)EditorGUILayout.EnumPopup("Transparency:", currentAnimationElementFE.blending.BlendType, new GUILayoutOption[]
                {
                    GUILayout.MaxWidth(205f)
                });
            }
            GUILayout.EndHorizontal();

            EditorGUILayout.Space();

            GUILayout.BeginHorizontal();
            {
                EditorGUIUtility.labelWidth = 55f;
                currentAnimationElementFE.blending.SourceFactor = (byte)EditorGUILayout.IntField("Source:", currentAnimationElementFE.blending.SourceFactor);

                EditorGUILayout.Space();

                EditorGUIUtility.labelWidth = 45f;
                currentAnimationElementFE.blending.DestinationFactor = (byte)EditorGUILayout.IntField("Dest:", currentAnimationElementFE.blending.DestinationFactor);
            }
            GUILayout.EndHorizontal();

            GUILayout.Space(10);

            GUILayout.BeginHorizontal();
            {
                GUILayout.FlexibleSpace();
                if (GUILayout.Button(new GUIContent(addclsn1, "Add Attack"), GUILayout.MaxWidth(40), GUILayout.MaxHeight(26)))
                {
                    currentAnimationElementFE.clnsAttack = Misc.Add<Rect>(currentAnimationElementFE.clnsAttack, new Rect(-0.8f, 0.8f, 0.20f, 0.20f));
                    SceneView.RepaintAll();
                }
                if (GUILayout.Button(new GUIContent(addclsn2, "Add Normal"), GUILayout.MaxWidth(40), GUILayout.MaxHeight(26)))
                {
                    currentAnimationElementFE.clnsNormal = Misc.Add<Rect>(currentAnimationElementFE.clnsNormal, new Rect(-0.5f, 1, 0.20f, 0.20f));
                    SceneView.RepaintAll();
                }
                if (GUILayout.Button(new GUIContent(delclsns, "Del All Coll"), GUILayout.MaxWidth(40), GUILayout.MaxHeight(26)))
                {
                    currentAnimationElementFE.clnsAttack = new Rect[0];
                    currentAnimationElementFE.clnsNormal = new Rect[0];
                    SceneView.RepaintAll();
                }
                if (GUILayout.Button(new GUIContent(delanimclsns, "Remove all box collider of current animation"), GUILayout.MaxWidth(40), GUILayout.MaxHeight(26)))
                {
                    for (int i = 0; i < currentAnimationFE.animationElement.Count; i++)
                    {
                        currentAnimationFE.animationElement[i].clnsAttack = new Rect[0];
                        currentAnimationFE.animationElement[i].clnsNormal = new Rect[0];
                    }
                    SceneView.RepaintAll();
                }


                EditorGUI.BeginDisabledGroup(tickIndex == null || scrollPosAnimationElementFE == 0);
                if (GUILayout.Button(new GUIContent(clsnprev, "Copy all box collider of previous frame"), GUILayout.MaxWidth(40), GUILayout.MaxHeight(26)))
                {
                    currentAnimationElementFE.clnsAttack = currentAnimationFE.animationElement[scrollPosAnimationElementFE - 1].clnsAttack;
                    currentAnimationElementFE.clnsNormal = currentAnimationFE.animationElement[scrollPosAnimationElementFE - 1].clnsNormal;
                }
                EditorGUI.EndDisabledGroup();

                if (GUILayout.Button(new GUIContent(clsntype, "Switch type of all box collider"), GUILayout.MaxWidth(40), GUILayout.MaxHeight(26)))
                {
                    Rect[] newAttack = currentAnimationElementFE.clnsNormal;
                    Rect[] newNormal = currentAnimationElementFE.clnsAttack;
                    currentAnimationElementFE.clnsAttack = newAttack;
                    currentAnimationElementFE.clnsNormal = newNormal;
                    SceneView.RepaintAll();
                }
                GUILayout.FlexibleSpace();
            }
            GUILayout.EndHorizontal();
        }
        GUILayout.EndArea();
    }


    Vector2 scrollPosTimeLine = Vector2.zero;
    const int SIZE_PIXEL = 40;
    const int multiplicador = 5;
    const int afterPixelStart = 10;
    Dictionary<int, int> tickIndex;
    Dictionary<int, int> firstFrameIndex;
    int lastindexTimeLine = 0;
    int totalFramesClip = 0;
    int maxTicksCollider = 0;
    int sliderFrames = 0;
    private void DrawBottonRightAnimationsSection()
    {
        Rect bodyAnimationBottonRightContent = bodyAnimationBottonRightSide.GetRect();
        GUILayout.BeginArea(bodyAnimationBottonRightContent);
        {
            scrollPosTimeLine = GUILayout.BeginScrollView(scrollPosTimeLine, true, false, GUI.skin.horizontalScrollbar, GUIStyle.none);
            {

                GUILayout.Label("", GUILayout.Width(10000), GUILayout.Height(65));

                GUI.color = Color.black;
                for (float i = 0; i < 250; i++)
                {
                    GUI.Box(new Rect((i * SIZE_PIXEL) + afterPixelStart, 5, 30, 20), (i * multiplicador).ToString().PadLeft(1, '0'), EditorStyles.whiteMiniLabel);
                }
                

                GUI.color = Color.white;

                for (float i = 0; i < 250; i++)
                {
                    GUI.DrawTexture(new Rect((i * SIZE_PIXEL) + afterPixelStart, 20, 1, 15), markerLine);
                }

                for (float i = 0; i < 1245; i++)
                {
                    GUI.DrawTexture(new Rect((i * (SIZE_PIXEL / 5)) + afterPixelStart, 28, 1, 7), markerLine);
                }

                for (int i = 0; i < 1246; i++)
                {
                    GUI.DrawTexture(new Rect((i * (SIZE_PIXEL / 5)) + (afterPixelStart / 1.3f), 40, 5, 16), timeSpace);
                }

                if (currentAnimationFE != null)
                {

                    List<AnimationElementFE> animationElement = currentAnimationFE.animationElement;
                    for (int i = 0; i < animationElement.Count; i++)
                    {
                        GUI.color = Color.yellow;
                        GUI.DrawTexture(new Rect((i * (SIZE_PIXEL / 5)) + (afterPixelStart / 1.3f), 40, 5, 16), timeSpace);
                        int totalTicks2 = 0;
                        for (int h = 0; h < animationElement[i].time; h++)
                        {
                            totalTicks2 += animationElement[i].time;
                            GUI.color = Color.green;// new Color(0.4103774f, 1f, 0.4369332f);
                            GUI.DrawTexture(new Rect((h * (SIZE_PIXEL / 5)) + (afterPixelStart / 1.3f), 40, 5, 16), timeSpace);
                        }
                    }

                    int currentTotalTicks = 0;
                    int listIndex = 0;
                    int j = 0;

                    while (true)
                    {
                        currentTotalTicks += animationElement[listIndex].time;
                        GUI.color = Color.yellow;
                        GUI.DrawTexture(new Rect((j * (SIZE_PIXEL / 5)) + (afterPixelStart / 1.3f), 40, 5, 16), timeSpace);
                        j++;
                        for (; j < currentTotalTicks; j++)
                        {
                            GUI.color = Color.green;//new Color(1, 0.6745283f, 1);
                            GUI.DrawTexture(new Rect((j * (SIZE_PIXEL / 5)) + (afterPixelStart / 1.3f), 40, 5, 16), timeSpace);
                        }
                        if (listIndex < animationElement.Count)
                            listIndex++;
                        if (currentTotalTicks == maxTicksCollider)
                            break;
                    }
                }

                if (animationFEManager != null)
                {
                    sliderFrames = Convert.ToInt32(GUI.HorizontalSlider(new Rect(5, 10, 9970, 45), sliderFrames, 0, 1245, GUIStyle.none, GUIStyle.none));

                    GUI.color = Color.red;
                    GUI.DrawTexture(new Rect(afterPixelStart + (8 * sliderFrames), 20, 1, 40), markerLine);

                    SetAnimationElement(sliderFrames);
                }
            }
            EditorGUILayout.EndScrollView();
        }
        GUILayout.EndArea();
    }













    

    private void DrawAnimation()
    {
        Facing CurrentFacing = Facing.Right;
        int DrawingAngle = 0;
        bool AngleDraw = false;

        var drawlocation = Vector3.zero;// GetDrawLocation();
        transformExample.localPosition = new Vector3(drawlocation.x, ((drawlocation.y * -1)), transformExample.localPosition.z);

        float angle = AngleDraw ? Misc.FaceScalar(CurrentFacing, DrawingAngle) : 0f;
        transformExample.localEulerAngles = new Vector3(transformExample.localEulerAngles.x, transformExample.localEulerAngles.y, angle);

        // Flip Image. Isso esta em CurrentFacing get
        transformExample.transform.localScale = new Vector3(CurrentFacing == Facing.Right ? 1f : -1f, 1f, 1f);
        
        var spriteFE = spriteFEManager.GetSprite(currentAnimationElementFE.spriteId);
        if (spriteFE == null) return;

        if (exampleRender)
        {
            exampleOffset.localPosition = spriteFE.offset * Constant.Scale;
            exampleRender.sprite = spriteFE.sprite;
            Vector3 position = new Vector3(currentAnimationElementFE.offset.x, currentAnimationElementFE.offset.y * -1);
            exampleRender.transform.localPosition = position * Constant.Scale;

            if (currentAnimationElementFE.flip == SpriteEffects.FlipHorizontally)
            {
                exampleRender.flipX = true;
                int width = exampleRender.sprite.texture.width;
                Vector3 local = exampleRender.transform.localPosition;
                exampleRender.transform.localPosition = new Vector3(local.x + (width * Constant.Scale), local.y, local.z);
            }
            else if (currentAnimationElementFE.flip == SpriteEffects.FlipVertically)
            {
                exampleRender.flipY = true;
                int height = exampleRender.sprite.texture.height;
                Vector3 local = exampleRender.transform.localPosition;
                exampleRender.transform.localPosition = new Vector3(local.x, local.y + (height * Constant.Scale), local.z);
            }
            else if (currentAnimationElementFE.flip == SpriteEffects.None)
            {
                exampleRender.flipX = false;
                exampleRender.flipY = false;
            }
            else
            {
                exampleRender.flipX = true;
                exampleRender.flipY = true;
                int width = exampleRender.sprite.texture.width;
                int height = exampleRender.sprite.texture.height;
                Vector3 local = exampleRender.transform.localPosition;             // Constant.Scale
exampleRender.transform.localPosition = new Vector3(local.x + (width ), local.y + (height * Constant.Scale), local.z);
            }

#warning decomentar
            //exampleRender.color = currentAnimationElementFE.Blending;

        }
        
    }











    public void Teste()
    {

    }

    void OnSceneGUI(SceneView sceneView)
    {
        if (currentContext == TypeSection.Animations)
        {
            // START - Do your drawing here using Handles.
            if (currentAnimationElementFE != null)
            {
                Vector3 pos = new Vector3(0, 0, 0f);
                for (int i = 0; i < currentAnimationElementFE.clnsNormal.Length; i++)
                {
                    int controlID = GUIUtility.GetControlID("RectMoveHandles".GetHashCode(), FocusType.Passive);
                    //   AddBoxCollider(controlID);
                    DeleteBoxCollider(controlID, ClsnType.Type2Normal, i);

                    RectHandles.Do(ref currentAnimationElementFE.clnsNormal[i], ref pos, Color.blue, controlID, false);
                }
                for (int i = 0; i < currentAnimationElementFE.clnsAttack.Length; i++)
                {
                    int controlID = GUIUtility.GetControlID("RectMoveHandles".GetHashCode(), FocusType.Passive);

                    //   AddBoxCollider(controlID);
                    DeleteBoxCollider(controlID, ClsnType.Type1Attack, i);

                    RectHandles.Do(ref currentAnimationElementFE.clnsAttack[i], ref pos, Color.red, controlID, false);
                }
            }

            // END - Do your drawing here using Handles.
            // START - Do your drawing here using GUI.
            //Handles.BeginGUI();

            //Handles.EndGUI();
            // END - Do your drawing here using GUI.
        }
    }

    void DeleteBoxCollider(int controlID, ClsnType clsnType, int index)
    {
        EventType eventType2 = Event.current.GetTypeForControl(controlID);
        if (eventType2 == EventType.MouseDown)
        {
            if (Event.current.button == 1 && HandleUtility.nearestControl == controlID && !Event.current.alt)
            {
                RectsIndex rectsIndex = new RectsIndex(clsnType, index);
                GenericMenu previewMenu = new GenericMenu();
                previewMenu.AddItem(new GUIContent("Delete Box Collider"), false, DeleteBoxColliderAction, rectsIndex);
                previewMenu.ShowAsContext();
            }
        }
    }

    void DeleteBoxColliderAction(object rectsIndex)
    {
        RectsIndex newRectsIndex = ((RectsIndex)rectsIndex);

        if (newRectsIndex.clsnType == ClsnType.Type2Normal)
        {
            var lista = currentAnimationElementFE.clnsNormal.ToList<Rect>();
            lista.RemoveAt(newRectsIndex.index);
            currentAnimationElementFE.clnsNormal = lista.ToArray();
        }
        else if (newRectsIndex.clsnType == ClsnType.Type1Attack)
        {
            var lista = currentAnimationElementFE.clnsAttack.ToList<Rect>();
            lista.RemoveAt(newRectsIndex.index);
            currentAnimationElementFE.clnsAttack = lista.ToArray();
        }

        Tools.viewTool = ViewTool.None;
    }

    void AddBoxCollider(int controlID)
    {
        EventType eventType2 = Event.current.GetTypeForControl(controlID);
        if (eventType2 == EventType.MouseDown)
        {
            if (Event.current.button == 1 && HandleUtility.nearestControl != controlID && !Event.current.alt)
            {
                GenericMenu previewMenu = new GenericMenu();
                previewMenu.AddItem(new GUIContent("Add Box Collider Normal"), false, AddBoxColliderAction, ClsnType.Type2Normal);
                previewMenu.AddItem(new GUIContent("Add Box Collider Attack"), false, AddBoxColliderAction, ClsnType.Type1Attack);
                previewMenu.ShowAsContext();
            }
        }
    }

    void AddBoxColliderAction(object rectsIndex)
    {
        ClsnType clsnType = ((ClsnType)rectsIndex);
        if (clsnType == ClsnType.Type2Normal)
        {

        }
        else if (clsnType == ClsnType.Type1Attack)
        {

        }
    }

    void OnFocus()
    {
        SceneView.duringSceneGui -= this.OnSceneGUI;
        SceneView.duringSceneGui += this.OnSceneGUI;
    }

    void OnDestroy()
    {
        SceneView.duringSceneGui -= this.OnSceneGUI;

        if(transformExample != null)
            DestroyImmediate(transformExample.gameObject);
        if (transformExampleOnion != null)
            DestroyImmediate(transformExampleOnion.gameObject);

        
        //int choice = EditorUtility.DisplayDialogComplex("Save Changes", "You have made changes to the current file, do you want to save them before closing?", "Yes", "No", "Cancel");
        //if (choice == 0)
        //{
        //    //PrefabUtility.RecordPrefabInstancePropertyModifications(character);
        //    //    AssetDatabase.DeleteAsset("Assets/LIPSYNC_AUTOSAVE.asset");
        //}
        //else if (choice == 1)
        //{
        //    //    AssetDatabase.DeleteAsset("Assets/LIPSYNC_AUTOSAVE.asset");
        //}
        //else
        //{
        //    //   ShowWindow("Assets/LIPSYNC_AUTOSAVE.asset", true, oldName, oldLastLoad, markerTab, localOldSeekPosition);
        //}
    }

    void ResetRender()
    {
        transformExample.localPosition = Vector3.zero;
        transformExample.localScale = Vector3.one;
        transformExample.localEulerAngles = Vector3.zero;
        exampleOffset.localPosition = Vector3.zero;
        exampleRender.transform.localPosition = Vector3.zero;
    }

    void OpenURL(object url)
    {
        Application.OpenURL((string)url);
    }
    float PercentScreenSize(float percent, float value)
    {
        return (percent / 100) * value;
    }
    void SetAnimation(AnimationsFE animationsFE)
    {
        currentAnimationElementFE = animationFEManager.GetElementAnimation(currentAnimationFE.number, 0);
        GetTickIndex(currentAnimationFE.animationElement);

        clearTimeLineParameters();

        for (int i = 0; i < currentAnimationFE.animationElement.Count; i++)
        {
            maxTicksCollider += currentAnimationFE.animationElement[i].time;
        }
    }
    void SetAnimationElement(int TimeInAnimation)
    {
        if (lastindexTimeLine != TimeInAnimation)
        {
            tickIndex.TryGetValue(TimeInAnimation, out scrollPosAnimationElementFE);
            lastindexTimeLine = TimeInAnimation;
            currentAnimationElementFE = animationFEManager.GetElementAnimation(currentAnimationFE.number, scrollPosAnimationElementFE);

            DrawAnimation();
        }
    }
    void GetTickIndex(List<AnimationElementFE> animationElement)
    {
        List<int> times = GetOnlyTimes(animationElement);
        tickIndex = new Dictionary<int, int>();
        firstFrameIndex = new Dictionary<int, int>();
        int currentPaint = 0;
        for (int i = 0; i < times.Count; i++)
        {
            for (int j = 0; j < times[i]; j++)
            {
                tickIndex.Add(currentPaint, i);
                if (j == 0)
                {
                    firstFrameIndex.Add(i, currentPaint);
                }
                currentPaint++;
            }
            if (times[i] == -1)
            {
                firstFrameIndex.Add(i, currentPaint);
                tickIndex.Add(currentPaint, i);
            }
        }
        totalFramesClip = currentPaint;
    }
    static List<int> GetOnlyTimes(List<AnimationElementFE> animationElementFE)
    {
        var result = new List<int>();
        foreach (var element in animationElementFE)
        {
            result.Add(element.time);
        }
        return result;
    }
    void clearTimeLineParameters()
    {
        lastindexTimeLine = 0;
        //totalFramesClip = 0;
        maxTicksCollider = 0;
    }
}
*/