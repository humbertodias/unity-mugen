using System;
using System.Collections.Generic;
using System.IO;
using UnityEngine;
using UnityMugen.IO;

namespace UnityMugen.Drawing
{

    public class PaletteSystem
    {

        private Dictionary<int, PaletteList> managers = new Dictionary<int, PaletteList>();

        public List<Texture2D> BuildPalettes(PaletteList palette)
        {
            List<Texture2D> palettesTex2D = new List<Texture2D>();
            foreach (Color[] Palette in palette.palettes)
            {
                Texture2D colorSwapTex = new Texture2D(256, 1, TextureFormat.RGBA32, false, false);
                colorSwapTex.filterMode = FilterMode.Point;

                for (int i = 0; i < Palette.Length; i++)
                {
                    Color32 color32 = Palette[i];
                    //colorSwapTex.SetPixel((int)(color32.r), 0, Palette.colors[i]);
                    colorSwapTex.SetPixel(i, 0, color32);
                }
                colorSwapTex.Apply();
                palettesTex2D.Add(colorSwapTex);


                //byte[] bytes = colorSwapTex.EncodeToPNG();
                //var dirPath = Application.dataPath + "/../SaveImages/";
                //if (!Directory.Exists(dirPath))
                //{
                //    Directory.CreateDirectory(dirPath);
                //}
                //System.IO.File.WriteAllBytes(dirPath + Palette.namePalette + "Image" + ".png", bytes);
                //throw new ArgumentNullException("");
            }

            return new List<Texture2D>(palettesTex2D);
        }

        static Texture2D m_nullpalette;
        public static Texture2D CreatePaletteTexture
        {
            get
            {
                if (m_nullpalette == null)
                {
                    m_nullpalette = new Texture2D(1, 1);
                    m_nullpalette.SetPixel(0, 0, Color.white);
                    m_nullpalette.Apply();
                }
                return m_nullpalette;
            }
        }


        public PaletteList LoadPalette(string filepath)
        {
            if (managers.TryGetValue(filepath.GetHashCode(), out PaletteList value))
            {
                PaletteList pal = new PaletteList();
                pal.PalTable = value.PalTable;
                pal.PalTex = new List<Texture2D>(value.PalTex);
                pal.PalTexBackup = new List<Texture2D>(value.PalTex);
                pal.palettes = value.palettes;
                pal.numcols = value.numcols;
                pal.paletteMap = value.paletteMap;
                return pal;
            }
            else
            {
                SffUnpack sffUnpack = LoadPaletteFile(filepath);
                //   Palette pal = SpriteSystem.CreatePalettes();
                managers.Add(filepath.GetHashCode(), sffUnpack.palList);
                sffUnpack.palList.PalTexBackup = new List<Texture2D>(sffUnpack.palList.PalTex);
                return sffUnpack.palList;
            }
        }

        public SffUnpack LoadPaletteFile(string filepath)
        {
            if (filepath == null) throw new ArgumentNullException(nameof(filepath));

            var file = new FileUnpack(filepath, new FileStream(filepath, FileMode.Open, FileAccess.Read));

            SffUnpack s = new SffUnpack();
            s = s.newSff();

            UInt32 lofs, tofs;
            SpriteSystem.ReadHeaderFile(file, out s.header, out lofs, out tofs);

            // Leitura de Palette de SffV2
            if (s.header.Ver0 != 1)
            {
                Dictionary<PaletteId, int> uniquePals = new Dictionary<PaletteId, int>();
                for (int i = 0; i < s.header.NumberOfPalettes; i++)
                {
                    file.SeekFromBeginning(s.header.FirstPaletteHeaderOffset + (i * 16));
                    var data = file.ReadBytes(16);

                    //Int16[] gn_ = new Int16[3];
                    Int16 groupNo = BitConverter.ToInt16(data, 0);
                    Int16 itemNo = BitConverter.ToInt16(data, 2);
                    Int16 numCols = BitConverter.ToInt16(data, 4);
                    UInt16 link = BitConverter.ToUInt16(data, 6);
                    UInt32 ofs = BitConverter.ToUInt32(data, 8);
                    UInt32 siz = BitConverter.ToUInt32(data, 12);

                    Color[] pal;
                    int idx;
                    if (uniquePals.TryGetValue(new PaletteId(groupNo, itemNo), out int old))
                    {
                        idx = old;
                        pal = s.palList.Get(old);
                        Debug.LogWarning("duplicated palette");
                    }
                    else if (siz == 0)
                    {
                        idx = link;
                        pal = s.palList.Get(idx);
                    }
                    else
                    {
                        file.SeekFromBeginning(lofs + ofs);
                        pal = new Color[256];

                        for (int j = 0; j < siz / 4 && j < pal.Length; j++)
                        {
                            var data3 = file.ReadBytes(4);
                            Color32 color;
                            if (s.header.Ver2 == 0)
                                color = new Color32(data3[0], data3[1], data3[2], (byte)(j == 0 ? 0 : 255));
                            else
                                color = new Color32(data3[0], data3[1], data3[2], (byte)(j == 0 ? 0 : data3[3]));

                            pal[j] = color;
                        }
                        idx = i;
                    }

                    if (!uniquePals.ContainsKey(new PaletteId(groupNo, itemNo)))
                        uniquePals.Add(new PaletteId(groupNo, itemNo), idx);

                    s.palList = s.palList.SetSource(i, pal);

                    if (!s.palList.PalTable.ContainsKey(new PaletteId(groupNo, itemNo)))
                        s.palList.PalTable.Add(new PaletteId(groupNo, itemNo), idx);

                    if (!s.palList.numcols.ContainsKey(new PaletteId(groupNo, itemNo)))
                        s.palList.numcols.Add(new PaletteId(groupNo, itemNo), numCols);

                    bool exis1 = s.palList.PalTable.TryGetValue(new PaletteId(1, i + 1), out int value1);
                    bool exis2 = s.palList.PalTable.TryGetValue(new PaletteId(groupNo, itemNo), out int value2);
                    if ((exis1 && exis2) && i <= 12 && value1 == value2 && groupNo != 1 && itemNo != (i + 1))
                    {
                        if (!s.palList.PalTable.ContainsKey(new PaletteId(1, i + 1)))
                            s.palList.PalTable.Add(new PaletteId(1, i + 1), -1);
                    }

                    if (i <= 12 && i + 1 == s.header.NumberOfPalettes)
                    {
                        for (int j = i + 1; j < 12; j++)
                        {
                            s.palList.PalTable.Remove(new PaletteId(1, (j + 1)));
                        }
                    }
                }
            }

            return s;
        }


    }
}