using System;
using System.Reflection;
using UnityEditor;
using UnityEngine;

namespace Anima2D
{
    public class HandlesExtra
    {
        public delegate void CapFunction(int controlID, Vector3 position, Quaternion rotation, float size, EventType eventType);

        public class Styles
        {
            public readonly GUIStyle dragDot = "U2D.dragDot";
            public readonly GUIStyle dragDotActive = "U2D.dragDotActive";
            public readonly GUIStyle pivotDot = "U2D.pivotDot";
            public readonly GUIStyle pivotDotActive = "U2D.pivotDotActive";
        }

        static Styles s_Styles;
        public static Styles styles
        {
            get
            {
                if (s_Styles == null)
                {
                    s_Styles = new Styles();
                }

                return s_Styles;
            }
        }

        static Material s_HandleWireMaterial;
        static Material s_HandleWireMaterial2D;
        static MethodInfo s_ApplyWireMaterialMethodInfo;

        private static Vector2 s_CurrentMousePosition;
        private static Vector2 s_DragStartScreenPosition;
        private static Vector2 s_DragScreenOffset;

        static Vector3[] s_circleArray;

        public static Vector3 GUIToWorld(Vector3 guiPosition)
        {
            return GUIToWorld(guiPosition, Vector3.forward, Vector3.zero);
        }

        public static Vector3 GUIToWorld(Vector3 guiPosition, Vector3 planeNormal, Vector3 planePos)
        {
            Vector3 worldPos = Handles.inverseMatrix.MultiplyPoint(guiPosition);

            if (Camera.current)
            {
                Ray ray = HandleUtility.GUIPointToWorldRay(guiPosition);

                planeNormal = Handles.matrix.MultiplyVector(planeNormal);

                planePos = Handles.matrix.MultiplyPoint(planePos);

                Plane plane = new Plane(planeNormal, planePos);

                float distance = 0f;

                //	if(plane.Raycast(ray, out distance))
                //	{
                worldPos = Handles.inverseMatrix.MultiplyPoint(ray.GetPoint(distance));
                //	}
            }

            return worldPos;
        }

        public static Vector2 Slider2D(int id, Vector2 position, CapFunction drawCapFunction)
        {
            return Slider2D(id, position, Vector3.zero, Vector3.zero, drawCapFunction);
        }

        public static Vector2 Slider2D(int id, Vector2 position, Vector3 planeNormal, Vector3 planePosition, CapFunction drawCapFunction)
        {
            EventType eventType = Event.current.GetTypeForControl(id);

            switch (eventType)
            {
                case EventType.MouseDown:
                    if (Event.current.button == 0 && HandleUtility.nearestControl == id && !Event.current.alt)
                    {
                        GUIUtility.keyboardControl = id;
                        GUIUtility.hotControl = id;
                        s_CurrentMousePosition = Event.current.mousePosition;
                        s_DragStartScreenPosition = Event.current.mousePosition;
                        Vector2 b = HandleUtility.WorldToGUIPoint(position);
                        s_DragScreenOffset = s_CurrentMousePosition - b;
                        EditorGUIUtility.SetWantsMouseJumping(1);

                        Event.current.Use();
                    }
                    break;
                case EventType.MouseUp:
                    if (GUIUtility.hotControl == id && (Event.current.button == 0 || Event.current.button == 2))
                    {
                        GUIUtility.hotControl = 0;
                        Event.current.Use();
                        EditorGUIUtility.SetWantsMouseJumping(0);
                    }
                    break;
                case EventType.MouseDrag:
                    if (GUIUtility.hotControl == id)
                    {
                        s_CurrentMousePosition = Event.current.mousePosition;
                        Vector2 center = position;
                        position = GUIToWorld(s_CurrentMousePosition - s_DragScreenOffset, planeNormal, planePosition);
                        if (!Mathf.Approximately((center - position).magnitude, 0f))
                        {
                            GUI.changed = true;
                        }
                        Event.current.Use();
                    }
                    break;
                case EventType.KeyDown:
                    if (GUIUtility.hotControl == id && Event.current.keyCode == KeyCode.Escape)
                    {
                        position = GUIToWorld(s_DragStartScreenPosition - s_DragScreenOffset);
                        GUIUtility.hotControl = 0;
                        GUI.changed = true;
                        Event.current.Use();
                    }
                    break;
            }

            if (drawCapFunction != null)
            {
                drawCapFunction(id, position, Quaternion.identity, 1f, eventType);
            }

            return position;
        }


        public static void PivotCap(int controlID, Vector3 position, Quaternion rotation, float size, EventType eventType)
        {
            DrawImageBasedCap(controlID, position, rotation, size, styles.pivotDot, styles.pivotDotActive);
        }

        public static void DotCap(int controlID, Vector3 position, Quaternion rotation, float size, EventType eventType)
        {
            DrawImageBasedCap(controlID, position, rotation, size, styles.dragDot, styles.dragDotActive);
        }

        static Rect GetGUIStyleRect(GUIStyle style, Vector3 position)
        {
            Vector3 vector = HandleUtility.WorldToGUIPoint(position);

            float fixedWidth = style.fixedWidth;
            float fixedHeight = style.fixedHeight;

            return new Rect(vector.x - fixedWidth / 2f, vector.y - fixedHeight / 2f, fixedWidth, fixedHeight);
        }

        static void DrawImageBasedCap(int controlID, Vector3 position, Quaternion rotation, float size, GUIStyle normal, GUIStyle active)
        {
            if (Event.current.type != EventType.Repaint)
                return;

            if (Camera.current && Vector3.Dot(position - Camera.current.transform.position, Camera.current.transform.forward) < 0f)
                return;

            Handles.BeginGUI();
            if (GUIUtility.hotControl == controlID)
            {
                active.Draw(GetGUIStyleRect(normal, position), GUIContent.none, controlID);
            }
            else
            {
                normal.Draw(GetGUIStyleRect(active, position), GUIContent.none, controlID);
            }
            Handles.EndGUI();
        }



        public static void ApplyWireMaterial()
        {
            if (s_ApplyWireMaterialMethodInfo == null)
            {
                s_ApplyWireMaterialMethodInfo = typeof(HandleUtility).GetMethod("ApplyWireMaterial", BindingFlags.Static | BindingFlags.NonPublic, null, new Type[] { }, null);
            }

            if (s_ApplyWireMaterialMethodInfo != null)
            {
                s_ApplyWireMaterialMethodInfo.Invoke(null, null);
            }
        }

    }
}
