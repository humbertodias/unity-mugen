#if UNITY_EDITOR

using System;
using System.IO;
using System.Reflection;
using UnityEditor;
using UnityEngine;

public static class MiscTools
{

    // string do atual caminho aberto no Projeto Unity
    public static string GetCurrentPathOpen()
    {
        Type projectWindowUtilType = typeof(ProjectWindowUtil);
        MethodInfo getActiveFolderPath = projectWindowUtilType.GetMethod("GetActiveFolderPath", BindingFlags.Static | BindingFlags.NonPublic);
        object obj = getActiveFolderPath.Invoke(null, new object[0]);
        return obj.ToString();
    }

    public static void UpdateTexture2DSettings(string dirPath, Vector2 pivot)
    {
        AssetImporter assetImporter = AssetImporter.GetAtPath(dirPath);
        TextureImporter importer = assetImporter as TextureImporter;
        importer.anisoLevel = 0;
        importer.filterMode = FilterMode.Point;
        importer.spritePixelsPerUnit = 100;
        importer.spritePivot = Vector2.zero;


        TextureImporterSettings settings = new TextureImporterSettings();
        importer.ReadTextureSettings(settings);
        settings.spriteAlignment = (int)SpriteAlignment.Custom;
        settings.spritePivot = pivot;
        importer.SetTextureSettings(settings);

        TextureImporterPlatformSettings platformSettings = new TextureImporterPlatformSettings
        {
            textureCompression = TextureImporterCompression.Uncompressed,
            format = TextureImporterFormat.Alpha8,
            resizeAlgorithm = TextureResizeAlgorithm.Mitchell,
            maxTextureSize = 2048,
            compressionQuality = 0
        };
        importer.SetPlatformTextureSettings(platformSettings);
        assetImporter.SaveAndReimport();
    }

    public static void Texture2DToPng(Texture2D texture2D, string nameFile, string pathSave)
    {
        if (!Directory.Exists(pathSave))
            Debug.LogError("Directory not found: " + pathSave);

        byte[] bytes = texture2D.EncodeToPNG();
        System.IO.File.WriteAllBytes(pathSave + nameFile, bytes);
    }

}

#endif