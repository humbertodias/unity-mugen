using System.Linq;
using UnityEditor;
using UnityEngine;

[CustomEditor(typeof(MeshRenderer))]
public class MeshRendererSortingEditor : Editor
{

    public override void OnInspectorGUI()
    {
        base.OnInspectorGUI();

        MeshRenderer renderer = target as MeshRenderer;


        var layers = SortingLayer.layers;

        EditorGUILayout.BeginHorizontal();
        EditorGUI.BeginChangeCheck();
        int newId = DrawSortingLayersPopup(renderer.sortingLayerID);
        if (EditorGUI.EndChangeCheck())
        {
            renderer.sortingLayerID = newId;
        }
        EditorGUILayout.EndHorizontal();

        EditorGUILayout.BeginHorizontal();
        EditorGUI.BeginChangeCheck();
        int order = EditorGUILayout.IntField("Sorting Order", renderer.sortingOrder);
        if (EditorGUI.EndChangeCheck())
        {
            renderer.sortingOrder = order;
        }
        EditorGUILayout.EndHorizontal();

    }

    int DrawSortingLayersPopup(int layerID)
    {
        var layers = SortingLayer.layers;
        var names = layers.Select(l => l.name).ToArray();
        if (!SortingLayer.IsValid(layerID))
        {
            layerID = layers[0].id;
        }
        var layerValue = SortingLayer.GetLayerValueFromID(layerID);
        var newLayerValue = EditorGUILayout.Popup("Sorting Layer", layerValue, names);
        return layers[newLayerValue].id;
    }

}