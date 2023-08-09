using System;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityMugen.Collections;
using UnityMugen.Drawing.PngDecode;
using UnityMugen.IO;

namespace UnityMugen.Drawing
{
    public class SpriteInfo
    {
        SpriteUnpack asd;
    }

    public class SpriteSystem
    {
        private static LauncherEngine Launcher => LauncherEngine.Inst;

        private Sff s;

        private Dictionary<int, (Dictionary<SpriteId, SpriteData>, PaletteList, int , int)> managers =
            new Dictionary<int, (Dictionary<SpriteId, SpriteData>, PaletteList, int, int)>();

        private Dictionary<SpriteId, SpriteData> spriteDatas = new Dictionary<SpriteId, SpriteData>();

        static Texture2D m_nullpalette;
        public static Texture2D CreatePaletteTexture
        {
            get
            {
                if (m_nullpalette == null)
                {
                    m_nullpalette = new Texture2D(1, 1);
                    m_nullpalette.filterMode = FilterMode.Point;
                    m_nullpalette.SetPixel(0, 0, Color.white);
                    m_nullpalette.Apply();
                }
                return m_nullpalette;
            }
        }

        public ReadOnlyList<Texture2D> BuildPalettes(string[] palettesName)
        {
            var palettes = new List<Texture2D>(12);

            for (var i = 0; i != palettesName.Length; ++i)
            {
                var filepath = palettesName[i];
                if (string.Equals(filepath, string.Empty))
                {
                    palettes.Add(CreatePaletteTexture);
                }
                else
                {
                    var palette = LoadPaletteFile(filepath);
                    palettes.Add(palette);
                }
            }

            return new ReadOnlyList<Texture2D>(palettes);
        }

        public Texture2D LoadPaletteFile(string filepath)
        {
            if (filepath == null) throw new ArgumentNullException(nameof(filepath));

            Texture2D palette = new Texture2D(256, 1, TextureFormat.RGBA32, false, false);
            palette.filterMode = FilterMode.Point;

            //if (m_palettefilecache.TryGetValue(filepath, out palette)) return palette;

            //palette = GetSubSystem<Video.VideoSystem>().CreatePaletteTexture();
            //m_palettefilecache.Add(filepath, palette);

            using (var file = Launcher.fileSystem.OpenFile(filepath))
            {
                if (file.FileLength != 256 * 3)
                {
                    Debug.LogError(string.Format("{0} is not a character palette file", filepath));
                }
                else
                {
                    var buffer = new byte[256 * 4];
                    var filedata = file.ReadBytes(256 * 3);

                    for (var i = 0; i != 256; ++i)
                    {
                        var fileindex = i * 3;
                        Color32 color32 = new Color32(filedata[fileindex + 0], filedata[fileindex + 1], filedata[fileindex + 2], 255);
                        palette.SetPixel(255 - i, 0, color32);
                    }
                    palette.Apply();
                }
            }

            return palette;
        }

        public SpriteManager CreateManager(string nameFile, string[] palettesName)
        {
            if (managers.TryGetValue(nameFile.GetHashCode(),
                out (Dictionary<SpriteId, SpriteData>, PaletteList, int, int) value))
            {
                PaletteList pal = new PaletteList();
                pal.PalTable = value.Item2.PalTable;
                pal.PalTex = new List<Texture2D>(value.Item2.PalTex);
                pal.PalTexBackup = new List<Texture2D>(value.Item2.PalTex);
                pal.palettes = value.Item2.palettes;
                pal.numcols = value.Item2.numcols;
                pal.paletteMap = value.Item2.paletteMap;

                return new SpriteManager((value.Item1, pal, value.Item3, value.Item4));
            }

            spriteDatas = new Dictionary<SpriteId, SpriteData>();
            GetOpenFile(nameFile);


            if (palettesName != null)
            {
                var pale = BuildPalettes(palettesName);
                for (int i = 0; i < pale.Count; i++)
                {
                    s.palList.PalTex[i] = pale[i];
                }
            }

            s.palList.PalTexBackup = new List<Texture2D>(s.palList.PalTex);

            managers.Add(nameFile.GetHashCode(), (spriteDatas, s.palList, s.header.Ver0, s.header.IdSourcePal));
            return new SpriteManager((spriteDatas, s.palList, s.header.Ver0, s.header.IdSourcePal));
        }




        private void GetOpenFile(string filepath)
        {
            if (filepath == null) throw new ArgumentNullException(nameof(filepath));

            var file = new File(filepath, new System.IO.FileStream(filepath, System.IO.FileMode.Open, System.IO.FileAccess.Read));

            s = new Sff();
            s = s.newSff();

            UInt32 lofs, tofs;
            ReadHeaderFile(file, ref s.header, out lofs, out tofs);

            // Leitura de Palette de Sff V2.x
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
                    else
                        s.palList.PalTable[new PaletteId(groupNo, itemNo)] = idx;

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

            SpriteUnpack[] spriteList = new SpriteUnpack[s.header.NumberOfSprites];
            SpriteUnpack prev = null;
            UInt32 shofs = s.header.FirstSpriteHeaderOffset;

            for (int i = 0; i < s.header.NumberOfSprites; i++)
            {
                file.SeekFromBeginning(shofs);

                UInt32 size = 0;
                UInt32 xofs = 0;
                byte copyLastPalette = 0;
                UInt16 indexOfPrevious = 0;
                spriteList[i] = new SpriteUnpack();

                if (s.header.Ver0 == 1)
                {
                    readHeader(file, ref spriteList[i], out xofs, out size, out indexOfPrevious, out copyLastPalette);
                }
                else if (s.header.Ver0 == 2)
                {
                    readHeaderV2(file, ref spriteList[i], ref xofs, ref size, lofs, tofs, ref indexOfPrevious);
                }

                if (size == 0)
                {
                    if (indexOfPrevious < i)
                    {
                        spriteList[i] = shareCopy(spriteList[i], spriteList[indexOfPrevious]);

                        var spriteId = new SpriteId(spriteList[i].Group, spriteList[i].Number);

                        if (!spriteDatas.ContainsKey(spriteId) && spriteList[i].Tex != null)
                        {
                            Rect rect = new Rect(0.0f, 0.0f, spriteList[i].Tex.width, spriteList[i].Tex.height);
                            Vector2 pivot = new Vector2(((float)spriteList[i].Offset[0] / (float)spriteList[i].Tex.width), (((float)spriteList[i].Tex.height - (float)spriteList[i].Offset[1]) / (float)spriteList[i].Tex.height));

                            Sprite sprite = Sprite.Create(spriteList[i].Tex, rect, pivot, 100.0f, 0, SpriteMeshType.FullRect);
                            sprite.name = spriteList[i].Group + "-" + spriteList[i].Number;
                            SpriteData spriteData = new SpriteData();
                            spriteData.indexPalette = spriteList[i].palidx;
                            spriteData.sprite = sprite;
                            spriteDatas.Add(spriteId, spriteData);
                        }
                    }
                    else
                    {
                        spriteList[i].palidx = 0;
                    }
                }
                else
                {
                    if (s.header.Ver0 == 1)
                    {
                        bool condicao = (prev == null || spriteList[i].Group == 0 && spriteList[i].Number == 0);
                        read(file, ref spriteList[i], ref s.header, shofs + 32, size, xofs, prev, ref s.palList, copyLastPalette, condicao);
                    }
                    else if (s.header.Ver0 == 2)
                    {
                        readV2(file, ref spriteList[i], xofs, size);
                    }
                    prev = spriteList[i];
                }

                if (!s.sprites.ContainsKey(new Int16[] { spriteList[i].Group, spriteList[i].Number }))
                    s.sprites.Add(new Int16[] { spriteList[i].Group, spriteList[i].Number }, spriteList[i]);

                if (s.header.Ver0 == 1)
                {
                    shofs = xofs;
                }
                else
                {
                    shofs += 28;
                }

            }

            //foreach(var sp in s.sprites)
            //{
            //    System.Diagnostics.Debug.WriteLine("G:" + sp.Value.Group+" N:"+ sp.Value.Number+" id:"+ sp.Value.palidx);
            //}

        }


        public static void ReadHeaderFile(UnityMugen.IO.File file, ref SffHeader sh, out UInt32 lofs, out UInt32 tofs)
        {
            if (file == null) throw new ArgumentNullException(nameof(file));

            lofs = tofs = 0;

            System.IO.BinaryReader binaryReader = new System.IO.BinaryReader(file.Stream);
            var data = binaryReader.ReadBytes(65);
            file.Stream.Seek(0, System.IO.SeekOrigin.Begin);

            sh.Ver3 = data[12];
            sh.Ver2 = data[13];
            sh.Ver1 = data[14];
            sh.Ver0 = data[15];

            //UInt32 dummy = data[16];

            switch (sh.Ver0)
            {
                case 1:
                    sh.FirstPaletteHeaderOffset = 0;
                    sh.NumberOfPalettes = 0;
                    //sh.NumberOfGroups = BitConverter.ToUInt32(data, 16); // Number of groups [04] -- 352
                    sh.NumberOfSprites = BitConverter.ToUInt32(data, 20); // Number of images [04] -- 2689
                    sh.FirstSpriteHeaderOffset = BitConverter.ToUInt32(data, 24); // File offset where first subfile is located [04] -- 512
                                                                                  //sh.SubheaderSizeA = BitConverter.ToUInt32(data, 28); // Size of subheader in bytes [04] -- 32
                    break;
                case 2:
                    sh.FirstSpriteHeaderOffset = BitConverter.ToUInt32(data, 36); // offset where first sprite node header data is located
                    sh.NumberOfSprites = BitConverter.ToUInt32(data, 40);// OK Total number of sprites
                    sh.FirstPaletteHeaderOffset = BitConverter.ToUInt32(data, 44); // offset where first palette node header data is located
                    sh.NumberOfPalettes = BitConverter.ToUInt32(data, 48); // Total number of palettes

                    lofs = BitConverter.ToUInt32(data, 52); /*ldata offset*/ // (LdataOffset == primeira posicao do index de palette) continuar apartir desta descricao
                                                                             //LdataLenght = BitConverter.ToInt32(data, 56); // ldata length
                    tofs = BitConverter.ToUInt32(data, 60); // tdata offset
                                                            //TdataLenght = BitConverter.ToInt32(data, 64); // tdata length
                    break;
                default:
                    throw new ArgumentNullException("Unrecognized SFF version");
            }

        }



        public SpriteUnpack shareCopy(SpriteUnpack s, SpriteUnpack src)
        {
            s.Pal = src.Pal;
            s.Tex = src.Tex;
            s.Size = src.Size;
            if (s.palidx < 0)
                s.palidx = src.palidx;

            s.rle = src.rle;
            return s;
        }

        public void readV2(File f, ref SpriteUnpack s, UInt32 offset, UInt32 datasize)
        {
            f.SeekFromBeginning(offset + 4);

            if (s.rle < 0)
            {
                int format = -s.rle;
                byte[] px = null;
                bool isRaw = false;

                if (2 <= format && format <= 4)
                {
                    if (datasize < 4)
                    {
                        datasize = 4;
                    }
                    
                }

                px = f.ReadBytes((int)datasize - 4);

                switch (format)
                {
                    case 2:
                        px = DecodeSFF.Rle8Decode(ref s, px);
                        break;
                    case 3:
                        px = DecodeSFF.Rle5Decode(ref s, px);
                        break;
                    case 4:
                        px = DecodeSFF.Lz5Decode(ref s, px);
                        break;
                    case 10:
                        {
                            px = Png.IDataFiltered(px);
                            MiscSprites.Flip(s.Size, ref px);
                            break;
                        }
                    case 11:
                    case 12:
                        {
                            Texture2D tex = new Texture2D(2, 2);
                            ImageConversion.LoadImage(tex, px);

                            Rect rect2 = new Rect(0.0f, 0.0f, tex.width, tex.height);
                            Vector2 pivot = new Vector2((float)s.Offset[0] / (float)tex.width, (((float)s.Size[1] - (float)s.Offset[1]) / (float)tex.height));
                            Sprite sprite = Sprite.Create(tex, rect2, pivot, 100.0f, 0, SpriteMeshType.FullRect);
                            sprite.name = s.Group + "-" + s.Number;

                            SpriteData spriteData = new SpriteData();
                            spriteData.indexPalette = s.palidx;
                            spriteData.sprite = sprite;
                            spriteData.isRGBA = true;

                            var id = new SpriteId(s.Group, s.Number);
                            if (!spriteDatas.ContainsKey(id))
                                spriteDatas.Add(id, spriteData);

                            s.Tex = tex;

                            return;
                        }

                    default:
                        throw new ArgumentNullException("Unknown format");

                }

                if (!isRaw)
                {
                    s = SetPxl(ref s, px);
                }
            }
        }

        public void read(File f, ref SpriteUnpack s, ref SffHeader sh, UInt32 offset, UInt32 datasize,
            UInt32 nextSubheader, SpriteUnpack prev, ref PaletteList pl, byte copyLastPalette, bool c00)
        {
            //byte ps;
            bool paletteSame;
            UInt32 palSize;
            byte[] px;

            if (nextSubheader > offset)
            {
                datasize = nextSubheader - offset;
            }

            paletteSame = copyLastPalette != 0 && prev != null;

            readPcxHeader(f, offset, ref s);

            f.SeekFromBeginning(offset + 128);
            if (c00 || paletteSame)
            {
                palSize = 0;
            }
            else
            {
                palSize = 768;
            }

            if (datasize < 128 + palSize)
            {
                datasize = (128 + palSize);
            }

            px = f.ReadBytes((int)(datasize - (128 + palSize)));

            if (paletteSame)
            {
                if (prev != null)
                    s.palidx = prev.palidx;

                if (s.palidx < 0)
                {
                    var (palidx, _, paletteList) = pl.NewPal();
                    pl = paletteList;
                    s.palidx = palidx;
                }
            }
            else
            {
                var (palidx, colors, paletteList) = pl.NewPal();
                pl = paletteList;
                s.palidx = palidx;

                if (c00)
                {
                    f.SeekFromBeginning(offset + datasize - 768);
                }
                for (int i = 0; i < colors.Length; i++)
                {
                    var data3 = f.ReadBytes(3);
                    Color32 color = new Color32(data3[0], data3[1], data3[2], (byte)(i == 0 ? 0 : 255));
                    colors[i] = color;
                }

                pl.PalTex[palidx] = (GenericClass.BuildPalettes(colors));
            }

            var imgSprite = DecodeSFF.RlePcxDecode(ref s, px);

            if (s.Group == 0 && s.Number == 0)
                sh.IdSourcePal = s.palidx;

            s = SetPxl(ref s, imgSprite);
        }


        public void readHeader(File file, ref SpriteUnpack s, out UInt32 ofs, out UInt32 size, out UInt16 link, out byte copyLastPalette)
        {
            if (file == null) throw new ArgumentNullException(nameof(file));

            var data = file.ReadBytes(19);
            if (data.Length != 19) throw new ArgumentException("File is not long enough", nameof(file));

            ofs = BitConverter.ToUInt32(data, 0);
            size = BitConverter.ToUInt32(data, 4);

            s.Offset = new Int16[2];
            s.Offset[0] = BitConverter.ToInt16(data, 8);
            s.Offset[1] = BitConverter.ToInt16(data, 10);
            s.Group = BitConverter.ToInt16(data, 12);
            s.Number = BitConverter.ToInt16(data, 14);
            link = BitConverter.ToUInt16(data, 16);
            copyLastPalette = data[18];
        }

        public void readHeaderV2(File file, ref SpriteUnpack s, ref UInt32 ofs, ref UInt32 size, UInt32 lofs, UInt32 tofs, ref UInt16 link)
        {
            if (file == null) throw new ArgumentNullException(nameof(file));

            var data = file.ReadBytes(28);
            if (data.Length != 28) throw new ArgumentException("File is not long enough", nameof(file));

            s.Group = BitConverter.ToInt16(data, 0);
            s.Number = BitConverter.ToInt16(data, 2);
            s.Size = new UInt16[2];
            s.Size[0] = BitConverter.ToUInt16(data, 4);
            s.Size[1] = BitConverter.ToUInt16(data, 6);
            s.Offset = new Int16[2];
            s.Offset[0] = BitConverter.ToInt16(data, 8);
            s.Offset[1] = BitConverter.ToInt16(data, 10);

            link = BitConverter.ToUInt16(data, 12);
            s.rle = -data[14]; // fmt = Format(0 raw, 1 invalid(no use), 2 RLE8, 3 RLE5, 4 LZ5)
            s.coldepth = data[15];
            ofs = BitConverter.ToUInt32(data, 16);
            size = BitConverter.ToUInt32(data, 20);
            s.palidx = BitConverter.ToInt16(data, 24);
            var flags = BitConverter.ToInt16(data, 26);
            if ((flags & 1) == 0)
                ofs += lofs;
            else
                ofs += tofs;
        }


        public void readPcxHeader(File file, Int64 offset, ref SpriteUnpack s)
        {
            UInt16 dummy;
            byte bpp;
            int encoding;
            UInt16[] rect = new UInt16[4];

            UInt16 bpl;

            file.SeekFromBeginning(offset);
            var data = file.ReadBytes(27);
            if (data.Length != 27) throw new ArgumentException("File is not long enough", nameof(file));

            dummy = BitConverter.ToUInt16(data, 0);
            encoding = data[2];
            bpp = data[3];

            if (bpp != 8)
            {
                throw new ArgumentNullException(string.Format("Invalid PCX color depth: expected 8-bit, got %v", bpp));
            }

            rect[0] = BitConverter.ToUInt16(data, 4);
            rect[1] = BitConverter.ToUInt16(data, 6);
            rect[2] = BitConverter.ToUInt16(data, 8);
            rect[3] = BitConverter.ToUInt16(data, 10);

            file.SeekFromBeginning(offset + 66);
            data = file.ReadBytes(2);
            bpl = BitConverter.ToUInt16(data, 0);

            s.Size = new UInt16[2];
            s.Size[0] = (ushort)(rect[2] - rect[0] + 1);
            s.Size[1] = (ushort)(rect[3] - rect[1] + 1);

            if (encoding == 1)
            {
                s.rle = (int)bpl;
            }
            else
            {
                s.rle = 0;
            }
        }


        public SpriteUnpack SetPxl(ref SpriteUnpack s, byte[] px)
        {
            Vector2Int size = new Vector2Int(s.Size[0], s.Size[1]);
            var texture = MiscSprites.CreatePixelTexture(size);

            texture.SetPixelData(px, 0, 0);
            texture.Apply();

            Rect rect = new Rect(0.0f, 0.0f, texture.width, texture.height);
            Vector2 pivot = new Vector2((float)s.Offset[0] / (float)texture.width, (((float)s.Size[1] - (float)s.Offset[1]) / (float)texture.height));
            Sprite sprite = Sprite.Create(texture, rect, pivot, 100.0f, 0, SpriteMeshType.FullRect);
            sprite.name = s.Group + "-" + s.Number;


            SpriteData spriteData = new SpriteData();
            spriteData.indexPalette = s.palidx;
            spriteData.sprite = sprite;

            var id = new SpriteId(s.Group, s.Number);
            if (!spriteDatas.ContainsKey(id))
                spriteDatas.Add(id, spriteData);

            s.Tex = texture;
            return s;
        }
    }









    public static class GenericClass
    {

        public static Sff newSff(this Sff s)
        {
            s.sprites = new Dictionary<Int16[], SpriteUnpack>();
            s.palList = new PaletteList();
            for (int i = 1; i <= /*MaxPalNo*/12; i++)
            {
                (int index, _, PaletteList palList) = s.palList.NewPal();
                s.palList = palList;
                s.palList.PalTable.Add(new PaletteId(1, i), index);
            }
            return s;
        }

        public static (int, Color[], PaletteList) NewPal(this PaletteList pl)
        {
            int i = pl.palettes.Count;
            Color[] p = new Color[256];
            pl = pl.SetSource(i, p);
            return (i, p, pl);
        }

        public static Color[] Get(this PaletteList pl, int i)
        {
            return pl.palettes[pl.paletteMap[i]];
        }

        public static PaletteList SetSource(this PaletteList pl, int i, Color[] p)
        {
            if (pl.paletteMap != null && i < pl.paletteMap.Length)
            {
                pl.paletteMap[i] = i;
            }
            else
            {
                for (; i > pl.paletteMap.Length;)
                {
                    pl.paletteMap = pl.paletteMap.Concat(new int[] { pl.paletteMap.Length }).ToArray();
                }
                pl.paletteMap = pl.paletteMap.Concat(new int[] { i }).ToArray();
            }

            if (i < pl.palettes.Count)
            {
                pl.palettes[i] = p;
                pl.PalTex[i] = (BuildPalettes(p));
            }
            else
            {
                for (; i > pl.palettes.Count;)
                {
                    pl.palettes.Add(null);
                }

                pl.palettes.Add(p);
                pl.PalTex.Add(BuildPalettes(p));
            }

            return pl;
        }

        public static Texture2D BuildPalettes(Color[] Palette)
        {
            Texture2D colorSwapTex = new Texture2D(256, 1, TextureFormat.RGBA32, false, false);
            colorSwapTex.filterMode = FilterMode.Point;

            for (int i = 0; i < Palette.Length; i++)
            {
                Color32 color32 = Palette[i];
                colorSwapTex.SetPixel(i, 0, color32);
            }

            colorSwapTex.Apply();
            return colorSwapTex;
        }
    }
}