using System.Collections.Generic;
using System.Linq;
using UnityEditor;
using UnityEngine;
using UnityMugen.EditorTools;

namespace UnityMugen.Editors
{

    //[CustomEditor(typeof(MoveList))]
    public class MoveListEditorWindow : EditorWindow
    {
        static MoveListEditorWindow s_window;

        SerializedObject m_serializedObject;

        List<WindowSection> m_sections;
        WindowSection m_topMenuSide;
        WindowSection m_topSide;
        WindowSection m_bodyLeftSide;
        WindowSection m_bodyRightSide;
        WindowSection m_bodyBottonRightSide;
        WindowSection m_bottonSide;
        WindowSection m_fullBottonSide;

        MoveList m_moveList;
        public bool editReadme;

        Vector2 m_scrollPos;
        Vector2 m_scrollPosCommonsInput;
        Vector2 m_scrollPosOthersActions;
        Vector2 m_scrollPosMoves;
        Vector2 m_scrollViewDescriotion;

        List<MoveShow> m_currentMSs;
        MoveShow m_currentMoveShow;

        Color m_colorDefault;
        int m_numberItemSelected;

        public List<Texture2D> commonsInput = new List<Texture2D>();
        public List<Texture2D> othersActions = new List<Texture2D>();
        private Texture2D missing;

        string[] m_toolbarStrings = { "Special Moves", "Super Moves" };

        int toolbar;
        int ToolBar
        {
            set
            {
                if (value == 0) m_currentMSs = m_moveList.specialMoves;
                else m_currentMSs = m_moveList.superMoves;

                if (value != toolbar)
                {
                    Repaint();
                    ChangeMoveShow(0);
                }

                toolbar = value;
            }
            get { return toolbar; }
        }

        public static void Init(MoveList moveList)
        {
            s_window = EditorWindow.GetWindow<MoveListEditorWindow>(false, "Character", true);
            s_window.titleContent = new GUIContent("Move List");
            s_window.minSize = new Vector2(854, 492);
            s_window.Show();
            s_window.Inicialize(moveList);
        }

        void Inicialize(MoveList moveList)
        {
            //UnityEngine.Object[] selection = Selection.GetFiltered(typeof(MoveList), SelectionMode.Assets);
            //if (selection.Length > 0)
            //    moveList = (MoveList)selection[0];
            m_moveList = moveList;
            if (m_moveList.specialMoves == null)
                m_moveList.specialMoves = new List<MoveShow>();

            if (m_moveList.superMoves == null)
                m_moveList.superMoves = new List<MoveShow>();

            if (m_moveList.specialMoves.Count == 0)
            {
                var moveShow = new MoveShow();
                moveShow.playerButtons = new List<Texture2D>();
                m_moveList.specialMoves.Add(moveShow);
            }

            m_currentMSs = m_moveList.specialMoves;
            m_currentMoveShow = m_currentMSs[0];

            commonsInput = Resources.LoadAll<Texture2D>("MoveList/CommonsInput").ToList().OrderBy(q => StringToInt(q.name)).ToList();
            othersActions = Resources.LoadAll<Texture2D>("MoveList/OthersActions").ToList().OrderBy(q => StringToInt(q.name)).ToList();
            missing = Resources.Load<Texture2D>("MoveList/missing");

            m_colorDefault = GUI.backgroundColor;
        }

        int StringToInt(string value)
        {
            int.TryParse(value, out int IntValue);
            return IntValue;
        }

        private void OnEnable()
        {
            m_sections = new List<WindowSection>();
            m_topMenuSide = new WindowSection(Rect.zero, new Color(0, 0, 0, 0.3f));
            m_topSide = new WindowSection(Rect.zero, Color.clear);

            m_bodyLeftSide = new WindowSection(Rect.zero, new Color(0, 0, 0, 0.3f));
            m_bodyRightSide = new WindowSection(Rect.zero, new Color(0, 0, 0, 0.3f));
            m_bodyBottonRightSide = new WindowSection(Rect.zero, Color.clear);
            m_bottonSide = new WindowSection(Rect.zero, new Color(0, 0, 0, 0.3f));
            m_fullBottonSide = new WindowSection(Rect.zero, Color.clear);

            m_sections.Add(m_topMenuSide);
            m_sections.Add(m_topSide);
            m_sections.Add(m_bodyLeftSide);
            m_sections.Add(m_bodyRightSide);
            m_sections.Add(m_bodyBottonRightSide);
            m_sections.Add(m_bottonSide);
            m_sections.Add(m_fullBottonSide);
        }

        protected void OnGUI()
        {
            if (s_window == null || m_moveList == null)
                Close();//Init();

            m_serializedObject = new SerializedObject(this);

            m_topMenuSide.SetRect(0f, 0f, position.width, 20f);

            if (!editReadme)
            {
                m_topSide.SetRect(0f, 20f, position.width, 70);

                float heightTopMenu = m_topMenuSide.GetRect().height;
                float heightTopContent = m_topSide.GetRect().height;
                float menuContent = heightTopMenu + heightTopContent;
                float heightRightSide = menuContent + m_bodyRightSide.GetRect().height;

                s_window.minSize = new Vector2(854, 492);

                float Left = 0;
                float margenLeft = 325;
                float heightBottonRight = 58;
                float heightBotton = 68 - 10 - 5;
                float heightFullBotton = 68 - 10 - 5;

                m_bodyLeftSide.SetRect(Left, menuContent, margenLeft, position.height - m_topMenuSide.GetRect().height - m_topSide.GetRect().height - heightBotton - heightFullBotton);
                m_bodyRightSide.SetRect(margenLeft, menuContent, position.width - margenLeft, position.height - menuContent - heightBottonRight - heightBotton - heightFullBotton);

                m_bodyBottonRightSide.SetRect(margenLeft, heightRightSide, position.width - margenLeft, heightBottonRight);
                m_bottonSide.SetRect(Left, position.height - heightBotton - heightFullBotton, position.width, heightBotton);
                m_fullBottonSide.SetRect(Left, position.height - heightFullBotton, position.width, heightBotton);

                foreach (var item in m_sections)
                {
                    GUI.DrawTexture(item.GetRect(), item.GetTexture());
                }

                DrawMenuSection();
                DrawContentSection();

                DrawLeftSection();
                DrawRightSection();
                DrawBottonRightSection();
                DrawBottonSection();
                DrawFullBottonSection();
            }
            else
            {
                EditorGUILayout.PropertyField(m_serializedObject.FindProperty("moveList"), true);
                EditorGUILayout.PropertyField(m_serializedObject.FindProperty("editReadme"), true);
            }

            s_window.Repaint();
            m_serializedObject.ApplyModifiedProperties();

            if (GUI.changed)
            {
                Undo.RecordObject(m_moveList, "Editor Modify");
                EditorUtility.SetDirty(m_moveList);
            }
        }

        private void DrawMenuSection()
        {
            Rect topMenu = m_topMenuSide.GetRect();
            GUILayout.BeginArea(topMenu);
            {
                GUILayout.BeginHorizontal();
                {
                    Rect helpRect = EditorGUILayout.BeginHorizontal();
                    if (GUILayout.Button("Help", EditorStyles.toolbarDropDown, GUILayout.Width(60)))
                    {
                        GenericMenu helpMenu = new GenericMenu();
                        helpMenu.AddItem(new GUIContent("Documentation"), false, OpenURL, "https://levelalfaomega.gitbook.io/unity-mugen/editors/move-list");
                        helpMenu.AddItem(new GUIContent("Email Support"), false, OpenURL, "mailto:levelalfaomega@gmail.com");
                        helpMenu.AddItem(new GUIContent("Inspect Values"), false, InspectValues);
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
            Rect topContent = m_topSide.GetRect();
            GUILayout.BeginArea(topContent);
            {
                EditorGUILayout.BeginHorizontal();
                {
                    m_moveList.iconChar = (Texture2D)EditorGUILayout.ObjectField(m_moveList.iconChar, typeof(Texture2D), false, GUILayout.Width(70), GUILayout.Height(70));

                    EditorGUILayout.BeginVertical();
                    {
                        EditorGUIUtility.labelWidth = 90;
                        m_moveList.nameChar = EditorGUILayout.TextField("Name:", m_moveList.nameChar);
                    }
                    EditorGUILayout.EndVertical();

                }
                EditorGUILayout.EndHorizontal();

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


        int TotalParent(int index, ref int totalParent)
        {
            if (m_currentMSs != null && m_currentMSs.ElementAt(index).parent != 0 &&
                m_currentMSs.ElementAt(index).parent - 1 != index)
            {
                totalParent++;
                TotalParent(m_currentMSs.ElementAt(index).parent - 1, ref totalParent);
            }
            return totalParent;
        }
        private void DrawLeftSection()
        {
            Rect bodyAnimationLeftContent = m_bodyLeftSide.GetRect();
            GUILayout.BeginArea(bodyAnimationLeftContent);
            {
                GUILayout.BeginVertical("ObjectFieldThumb");
                {
                    ToolBar = GUILayout.Toolbar(ToolBar, m_toolbarStrings);

                    m_scrollPosMoves = EditorGUILayout.BeginScrollView(m_scrollPosMoves);
                    for (int i = 0; i < m_currentMSs.Count; i++)
                    {
                        GUILayout.BeginHorizontal();
                        {
                            if (i == m_numberItemSelected)
                                GUI.color = new Color32(255, 255, 0, 156);
                            else
                                GUI.color = m_colorDefault;

                            GUILayout.FlexibleSpace();
                            int totalParent = 0;
                            TotalParent(i, ref totalParent);

                            GUILayoutOption gui = GUILayout.Width(255 - (totalParent * 25));
                            if (GUILayout.Button((totalParent > 0 ? "↪" : "") + m_currentMSs[i].nameMove, gui))
                            {
                                ChangeMoveShow(i);
                            }
                            GUI.color = m_colorDefault;

                            EditorGUI.BeginDisabledGroup(i == 0);
                            if (GUILayout.Button("↑", GUILayout.Width(20), GUILayout.Height(18)))
                            {
                                MoveShow item = m_currentMSs[i];
                                m_currentMSs.RemoveAt(i);
                                m_currentMSs.Insert(i - 1, item);
                                ChangeMoveShow(i - 1);
                            }
                            EditorGUI.EndDisabledGroup();

                            EditorGUI.BeginDisabledGroup(i == m_currentMSs.Count - 1);
                            if (GUILayout.Button("↓", GUILayout.Width(20), GUILayout.Height(18)))
                            {
                                MoveShow item = m_currentMSs[i];
                                m_currentMSs.RemoveAt(i);
                                m_currentMSs.Insert(i + 1, item);
                                ChangeMoveShow(i + 1);
                            }
                            EditorGUI.EndDisabledGroup();
                        }
                        GUILayout.EndHorizontal();
                    }
                    EditorGUILayout.EndScrollView();

                    GUILayout.FlexibleSpace();

                    //GUI.color = colorDefault;

                    if (GUILayout.Button("Add Move"))
                    {
                        var moveShow = new MoveShow();
                        moveShow.playerButtons = new List<Texture2D>();
                        m_currentMSs.Add(moveShow);

                        ChangeMoveShow(m_currentMSs.Count - 1);
                    }
                    if (GUILayout.Button("Clone Selected Move"))
                    {
                        m_currentMSs.Add(new MoveShow(m_currentMSs[m_numberItemSelected]));
                        ChangeMoveShow(m_currentMSs.Count - 1);
                    }
                }
                GUILayout.EndVertical();
            }
            GUILayout.EndArea();
        }


        private List<string> allMoves => AllMoves();
        List<string> AllMoves()
        {
            List<string> moves = new List<string>();
            moves.Add("None");
            foreach (MoveShow ddd in m_currentMSs)
            {
                moves.Add(ddd.nameMove);
            }
            return moves;
        }
        private void DrawRightSection()
        {
            Rect bodyRightContent = m_bodyRightSide.GetRect();
            GUILayout.BeginArea(bodyRightContent);
            {
                EditorGUILayout.BeginHorizontal();
                {
                    EditorGUILayout.BeginVertical();
                    {
                        m_scrollPos = EditorGUILayout.BeginScrollView(m_scrollPos);
                        try
                        {
                            float heightPanel = bodyRightContent.height - 45;
                            EditorGUILayout.BeginVertical("ObjectFieldThumb", GUILayout.Height(heightPanel));
                            {
                                EditorGUILayout.BeginHorizontal();
                                {
                                    EditorGUILayout.BeginHorizontal();
                                    {
                                        EditorGUILayout.BeginVertical("ObjectFieldThumb", GUILayout.Width(190), GUILayout.Height(heightPanel));
                                        {
                                            m_currentMoveShow.parent = EditorGUILayout.Popup("Parent:", m_currentMoveShow.parent, allMoves.ToArray());
                                            if (m_currentMoveShow.parent > (m_numberItemSelected /*+ 1*/) ||
                                                m_currentMoveShow.parent > (m_numberItemSelected + 2))
                                            {
                                                m_currentMoveShow.parent = 0;
                                                ShowNotification(new GUIContent("A command cannot reference itself or anything below it."), 3);
                                            }

                                            EditorGUI.BeginChangeCheck();
                                            m_currentMoveShow.imageMove = (Texture2D)EditorGUILayout.ObjectField(m_currentMoveShow.imageMove, typeof(Texture2D), false);
                                            if (EditorGUI.EndChangeCheck() && string.IsNullOrEmpty(m_currentMoveShow.nameMove))
                                                m_currentMoveShow.nameMove = m_currentMoveShow.imageMove.name;

                                            GUILayout.FlexibleSpace();
                                            EditorGUILayout.BeginHorizontal();
                                            GUILayout.FlexibleSpace();
                                            GUILayout.Label(m_currentMoveShow.imageMove/*, GUILayout.Width(100)*/, GUILayout.Height(heightPanel - 15 - 2));
                                            GUILayout.FlexibleSpace();
                                            EditorGUILayout.EndHorizontal();
                                            GUILayout.FlexibleSpace();
                                        }
                                        EditorGUILayout.EndVertical();

                                        EditorGUILayout.BeginVertical();
                                        {
                                            EditorGUILayout.BeginHorizontal();
                                            EditorGUIUtility.labelWidth = 90;
                                            m_currentMoveShow.nameMove = EditorGUILayout.TextField("Name Move:", m_currentMoveShow.nameMove);

                                            GUILayout.Space(30);

                                            if (GUILayout.Button("Remove Move", GUILayout.Width(100)))
                                            {
                                                m_currentMSs.RemoveAt(m_numberItemSelected);
                                                ChangeMoveShow(m_numberItemSelected);
                                            }
                                            EditorGUILayout.EndHorizontal();

                                            Rect rect = GUILayoutUtility.GetRect(50, heightPanel + 10);
                                            EditorStyles.textField.wordWrap = true;
                                            m_currentMoveShow.description = EditorGUI.TextArea(rect, m_currentMoveShow.description);
                                        }
                                        EditorGUILayout.EndVertical();
                                    }
                                    EditorGUILayout.EndHorizontal();

                                }
                                EditorGUILayout.EndHorizontal();
                            }
                            EditorGUILayout.EndVertical();
                        }
                        catch { }
                        EditorGUILayout.EndScrollView();
                    }
                    EditorGUILayout.EndVertical();
                }
                EditorGUILayout.EndHorizontal();
            }
            GUILayout.EndArea();
        }

        List<Texture2D> copyCommand;
        private void DrawBottonRightSection()
        {
            Rect bodyRightContent = m_bodyBottonRightSide.GetRect();
            GUILayout.BeginArea(bodyRightContent);
            {
                EditorGUILayout.BeginHorizontal();

                EditorGUILayout.BeginVertical();
                {
                    GUILayout.FlexibleSpace();
                    if (m_currentMoveShow.playerButtons.Count > 0)
                    {
                        if (GUILayout.Button("COPY", GUILayout.Width(50), GUILayout.Height(18)))
                            copyCommand = new List<Texture2D>(m_currentMoveShow.playerButtons);
                    }

                    if (GUILayout.Button("PASTE", GUILayout.Width(50), GUILayout.Height(18)))
                        m_currentMSs[m_numberItemSelected].playerButtons.AddRange(copyCommand);

                    if (m_currentMoveShow.playerButtons.Count > 0)
                    {
                        if (GUILayout.Button("CLEAR", GUILayout.Width(50), GUILayout.Height(18)))
                            m_currentMSs[m_numberItemSelected].playerButtons = new List<Texture2D>();
                    }

                    GUILayout.FlexibleSpace();
                }
                EditorGUILayout.EndVertical();


                m_currentMoveShow.scrollAction = EditorGUILayout.BeginScrollView(m_currentMoveShow.scrollAction, GUI.skin.horizontalScrollbar, GUIStyle.none);
                {
                    EditorGUILayout.BeginHorizontal();
                    {
                        try
                        {
                            if (m_currentMoveShow.playerButtons.Count == 0)
                            {
                                EditorGUILayout.BeginVertical();
                                GUILayout.FlexibleSpace();
                                EditorGUIUtility.labelWidth = 35;
                                EditorGUILayout.LabelField("NO COMMANDS");
                                GUILayout.FlexibleSpace();
                                EditorGUILayout.EndVertical();
                            }
                            else
                            {
                                foreach (Texture2D texture2D in m_currentMoveShow.playerButtons)
                                {
                                    EditorGUILayout.BeginVertical();
                                    GUILayout.FlexibleSpace();

                                    Texture tex = texture2D;
                                    if (tex == null)
                                        tex = missing;

                                    float proportion = tex.width * 35 / tex.height;
                                    if (GUILayout.Button(tex, GUILayout.Width(proportion), GUILayout.Height(35)))
                                        m_currentMoveShow.playerButtons.Remove(texture2D);
                                    GUILayout.FlexibleSpace();
                                    EditorGUILayout.EndVertical();
                                }
                            }
                        }
                        catch { }
                        GUILayout.FlexibleSpace();
                    }
                    EditorGUILayout.EndHorizontal();
                }
                EditorGUILayout.EndScrollView();
                EditorGUILayout.EndHorizontal();
            }
            GUILayout.EndArea();
        }

        void DrawBottonSection()
        {
            Rect bodyBottonContent = m_bottonSide.GetRect();
            GUILayout.BeginArea(bodyBottonContent);
            {
                m_scrollPosCommonsInput = EditorGUILayout.BeginScrollView(m_scrollPosCommonsInput);
                {
                    EditorGUILayout.BeginHorizontal();
                    {
                        GUILayout.FlexibleSpace();
                        foreach (var texture in commonsInput)
                        {
                            EditorGUILayout.BeginVertical();
                            GUILayout.FlexibleSpace();
                            var tex = texture as Texture2D;
                            float proportion = tex.width * 35 / tex.height;
                            if (GUILayout.Button(tex, GUILayout.Width(proportion), GUILayout.Height(35)))
                            {
                                m_currentMSs[m_numberItemSelected].playerButtons.Add(tex);
                            }
                            GUILayout.FlexibleSpace();
                            EditorGUILayout.EndVertical();
                        }
                        GUILayout.FlexibleSpace();
                    }
                    EditorGUILayout.EndHorizontal();
                }
                EditorGUILayout.EndScrollView();
            }
            GUILayout.EndArea();
        }


        void DrawFullBottonSection()
        {
            Rect bodyBottonContent = m_fullBottonSide.GetRect();
            GUILayout.BeginArea(bodyBottonContent);
            {
                m_scrollPosOthersActions = EditorGUILayout.BeginScrollView(m_scrollPosOthersActions);
                {
                    EditorGUILayout.BeginHorizontal();
                    {
                        GUILayout.FlexibleSpace();
                        foreach (var texture in othersActions)
                        {
                            EditorGUILayout.BeginVertical();
                            GUILayout.FlexibleSpace();
                            var tex = texture as Texture2D;
                            float proportion = tex.width * 35 / tex.height;
                            if (GUILayout.Button(tex, GUILayout.Width(proportion), GUILayout.Height(35)))
                            {
                                m_currentMSs[m_numberItemSelected].playerButtons.Add(tex);
                            }
                            GUILayout.FlexibleSpace();
                            EditorGUILayout.EndVertical();
                        }
                        GUILayout.FlexibleSpace();
                    }
                    EditorGUILayout.EndHorizontal();
                }
                EditorGUILayout.EndScrollView();
            }
            GUILayout.EndArea();
        }


        public void InspectValues()
        {
            editReadme = !editReadme;
        }


        void OnDestroy()
        {
            AssetDatabase.Refresh();
            AssetDatabase.SaveAssets();
        }

        void ChangeMoveShow(int index)
        {
            if (index == m_currentMSs.Count && index > 0)
                index--;

            if (m_currentMSs.Count > 0)
            {
                m_currentMoveShow = m_currentMSs[index];
                m_numberItemSelected = index;
            }
            GUIUtility.keyboardControl = 0;
            GUIUtility.hotControl = 0;

        }

        void OpenURL(object url)
        {
            Application.OpenURL((string)url);
        }
    }
}