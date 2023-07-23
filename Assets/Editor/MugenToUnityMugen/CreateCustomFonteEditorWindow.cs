using System;
using System.Collections.Generic;
using System.IO;
using UnityEditor;
using UnityEngine;


public class FontParameter
{
    public string index;
    public int x, y;
    public int width, height;

    public FontParameter(string index, int x, int y, int width, int height)
    {
        this.index = index;
        this.x = x;
        this.y = y;
        this.width = width;
        this.height = height;
    }
}

public class CreateCustomFonteEditorWindow : EditorWindow
{

    const int SPACE = 32;// Valor ascii Unicode

    int m_widthMax;
    int m_heightMax;
    int m_maiorWidth;
    string m_fonteName;

    string m_pathAssetsFont;
    string m_path;


    List<FontParameter> customSeilaOques;

    public Sprite[] sprites;

    void Diretorio(string fonteName)
    {
        m_pathAssetsFont = "Assets/Fonts/FonteMugen/" + fonteName + "/";
        m_path = Application.dataPath + "/Fonts/FonteMugen/" + fonteName + "/";

        if (!Directory.Exists("Assets/Fonts/FonteMugen/"))
        {
            Directory.CreateDirectory(Application.dataPath + "/Fonts/FonteMugen/");
        }

        if (!Directory.Exists(m_path))
        {
            Directory.CreateDirectory(m_path);
        }
    }

    [MenuItem("UnityMugen/Create Custom Fonte")]
    static void Init()
    {
        EditorWindow.GetWindow(typeof(CreateCustomFonteEditorWindow)).Show();
    }

    void OnGUI()
    {
        SerializedObject serializedObject = new SerializedObject(this);

        if (GUILayout.Button("Create Font"))
        {
            Processo();
        }

        EditorGUILayout.PropertyField(serializedObject.FindProperty("sprites"), true);
        serializedObject.ApplyModifiedProperties();
    }

    void Processo()
    {
        customSeilaOques = new List<FontParameter>();
        m_widthMax = 0;
        m_heightMax = 0;
        m_maiorWidth = 0;
        m_fonteName = null;
        int incrementalWidth = 0;

        if (sprites.Length == 0)
            this.ShowNotification(new GUIContent("Não existe nenhuma imagem adicionada."));
        else
        {
            m_fonteName = sprites[0].name.Split(new[] { "_", "-", "." }, StringSplitOptions.None)[0];
            m_heightMax = sprites[0].texture.height;

            foreach (Sprite sprite in sprites)
            {
                if (sprite.texture.width > m_maiorWidth)
                    m_maiorWidth = sprite.texture.width;
            }

            // O (+ maiorWidth) referece ao spaco em branco. do barra espaco
            m_widthMax += sprites.Length * m_maiorWidth + m_maiorWidth;

            Texture2D colorSwapTex = new Texture2D(m_widthMax, m_heightMax, TextureFormat.RGBA32, false, false);
            colorSwapTex.filterMode = FilterMode.Point;

            // Adicionando espaco em branco
            FontParameter Space = new FontParameter(SPACE.ToString(), incrementalWidth, 0, m_maiorWidth, m_heightMax);
            customSeilaOques.Add(Space);
            for (int i = 0; i < m_maiorWidth; i++)
            {
                for (int j = 0; j < m_heightMax; j++)
                {
                    colorSwapTex.SetPixel(incrementalWidth, j, Color.clear);
                }
                incrementalWidth += 1;
            }
            ////////////////////////////////

            foreach (Sprite sprite in sprites)
            {
                string[] split = sprite.name.Split(new[] { "_", "-", "." }, StringSplitOptions.None);
                FontParameter fonteParameter = new FontParameter(split[2], incrementalWidth, 0, sprite.texture.width, sprite.texture.height);
                customSeilaOques.Add(fonteParameter);

                for (int i = 0; i < sprite.texture.width; i++)
                {
                    for (int j = 0; j < sprite.texture.height; j++)
                    {
                        Color color = sprite.texture.GetPixel(i, j);
                        colorSwapTex.SetPixel(incrementalWidth, j, color);
                    }
                    incrementalWidth += 1;
                }
                for (int i = 0; i < m_maiorWidth - sprite.texture.width; i++)
                {
                    for (int j = 0; j < sprite.texture.height; j++)
                    {
                        Color color = Color.clear;
                        colorSwapTex.SetPixel(incrementalWidth, j, color);
                    }
                    incrementalWidth += 1;
                }

            }

            Diretorio(m_fonteName);

            // Encode texture into PNG
            byte[] bytes = colorSwapTex.EncodeToPNG();
            File.WriteAllBytes(m_path + m_fonteName + ".png", bytes);

            CreatFont(customSeilaOques);

            this.ShowNotification(new GUIContent("Created."));
        }
    }

    void CreatFont(List<FontParameter> customSeilaOques)
    {
        Font font = new Font();


        Material mat = new Material(Shader.Find("GUI/Text Shader"));
        AssetDatabase.CreateAsset(mat, m_pathAssetsFont + m_fonteName + ".mat");

        font.material = mat;

        ImportFont(customSeilaOques, font, new Vector2(m_widthMax, m_heightMax));
        AssetDatabase.SaveAssets();
    }


    public void SetAsciiStartOffset(Font font, int asciiStartOffset)
    {
        Editor editor = Editor.CreateEditor(font);

        SerializedProperty startOffsetProperty = editor.serializedObject.FindProperty("m_AsciiStartOffset");
        startOffsetProperty.intValue = asciiStartOffset;

        editor.serializedObject.ApplyModifiedProperties();

    }


    public void ImportFont(List<FontParameter> customSeilaOques, Font font, Vector2 size)
    {
        int asciiOffset = 0;
        List<CharacterInfo> infoList = new List<CharacterInfo>();
        for (int i = 0; i < customSeilaOques.Count; i++)
        {
            int width = customSeilaOques[i].width;
            int height = customSeilaOques[i].height;
            int x = customSeilaOques[i].x;
            int y = customSeilaOques[i].y;



            CharacterInfo info = new CharacterInfo();

            float widthFloat = (float)1 / (float)customSeilaOques.Count;
            info.uv.x = widthFloat * (float)i;
            info.uv.y = y;

            info.uv.width = widthFloat;
            info.uv.height = 1;
            info.vert.x = 0;
            info.vert.y = 0;
            info.vert.width = m_maiorWidth;
            info.vert.height = -(height);
            info.width = (float)width;
            info.index = Convert.ToInt32(customSeilaOques[i].index);

            infoList.Add(info);
        }

        this.SetAsciiStartOffset(font, asciiOffset);
        font.characterInfo = infoList.ToArray();

        AssetDatabase.CreateAsset(font, m_pathAssetsFont + m_fonteName + ".fontsettings");
    }


}
