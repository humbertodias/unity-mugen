using System;
using System.Collections.Generic;
using UnityEngine;

namespace UnityMugen.Drawing
{

    public struct SffUnpack
    {
        public SffHeader header;
        public Dictionary<Int16[], SpriteUnpack> sprites;
        public PaletteList palList;
    }

    public class SpriteUnpack
    {
        public int[] Pal;
        public Texture2D Tex;
        public Int16 Group, Number;
        public UInt16[] Size;
        public Int16[] Offset;
        public int palidx;
        public int rle;
        public byte coldepth;
        public UInt32[] paltemp;
        public Texture2D PalTex;
    }

    public struct SffHeader
    {
        public byte Ver0, Ver1, Ver2, Ver3;
        public UInt32 FirstSpriteHeaderOffset;
        public UInt32 FirstPaletteHeaderOffset;
        //public UInt32 NumberOfGroups;
        public UInt32 NumberOfSprites;
        public UInt32 NumberOfPalettes;
    }


    public struct PaletteList
    {
        public List<Color[]> palettes;
        public int[] paletteMap;
        public Dictionary<PaletteId, int> PalTable;
        public Dictionary<PaletteId, int> numcols;
        public List<Texture2D> PalTex;
        public List<Texture2D> PalTexBackup;

        public void init()
        {
            palettes = new List<Color[]>();
            paletteMap = new int[0];
            PalTable = new Dictionary<PaletteId, int>();
            numcols = new Dictionary<PaletteId, int>();
            PalTex = new List<Texture2D>(0);
        }
    }
}