using System;
using System.Collections.Generic;
using UnityEngine;

namespace UnityMugen.Drawing
{

    public struct Sff
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
        public int palidx = -1;
        public int rle;
        public byte coldepth;
        public UInt32[] paltemp;
        public Texture2D PalTex;
    }

    [Serializable]
    public class SpriteData
    {
        public Sprite sprite;
        public int indexPalette;
        public bool isRGBA;
    }

    public struct SffHeader
    {
        public byte Ver0, Ver1, Ver2, Ver3;
        public UInt32 FirstSpriteHeaderOffset;
        public UInt32 FirstPaletteHeaderOffset;
        //public UInt32 NumberOfGroups;
        public UInt32 NumberOfSprites;
        public UInt32 NumberOfPalettes;

        public int IdSourcePal;
    }


    public class PaletteList
    {
        public List<Color[]> palettes = new List<Color[]>();
        public int[] paletteMap = new int[0];
        public Dictionary<PaletteId, int> PalTable = new Dictionary<PaletteId, int>();
        public Dictionary<PaletteId, int> numcols = new Dictionary<PaletteId, int>();
        public List<Texture2D> PalTex = new List<Texture2D>(0);
        public List<Texture2D> PalTexBackup;
    }
}