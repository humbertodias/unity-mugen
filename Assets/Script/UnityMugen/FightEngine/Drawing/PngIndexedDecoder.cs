using System;
using System.Collections.Generic;
using System.IO;
using System.IO.Compression;
using UnityMugen.IO;

namespace UnityMugen.Drawing
{

    public class PngIndexedDecoder
    {
        int Width;
        int Height;
        int BitDepth;

        //GrayScale = 0, TrueColor = 2, IndexedColor = 3,
        //GrayScaleWithAlpha = 4, TrueColorWithAlpha = 6
        int ColorType;

        //Deflate = 0
        int CompressionMethod;

        //None = 0, Sub = 1, Up = 2, Avg = 3, Paeth = 4
        int FilterMethod;

        //None = 0, Adam7 = 1
        int InterlaceMethod;


        int bytesPerPixel;
        int bytesPerScanline;

        int currentRow;
        int currentRowBytesRead;

        byte[] pixels;

        public byte[] Decode(byte[] bytes)
        {
            var s = new MemoryStream(bytes);
            var inputReader = new BinaryReader(s);
            var signature = inputReader.ReadBytes(8);
            if (signature[0] != 0x89 || signature[1] != 0x50 || signature[2] != 0x4E || signature[3] != 0x47
                || signature[4] != 0x0D || signature[5] != 0x0A || signature[6] != 0x1A || signature[7] != 0x0A)
            {
                throw new FormatException("Invalid signature.");
            }


            List<byte[]> arr = new List<byte[]>();

            //var ms = new MemoryStream();
            while (true)
            {
                var length = (int)inputReader.ReadBigEndianUInt32();
                var type = inputReader.ReadString(4);
                if (type == "IEND")
                {
                    break;
                }

                switch (type)
                {
                    case "IHDR":
                        {
                            if (length != 13)
                                throw new FormatException("Header not supported.");

                            Width = (int)inputReader.ReadBigEndianUInt32();
                            Height = (int)inputReader.ReadBigEndianUInt32();
                            BitDepth = inputReader.ReadByte();

                            //GrayScale = 0, TrueColor = 2, IndexedColor = 3, GrayScaleWithAlpha = 4, TrueColorWithAlpha = 6
                            ColorType = inputReader.ReadByte();

                            //Deflate = 0
                            CompressionMethod = inputReader.ReadByte();

                            //None = 0, Sub = 1, Up = 2, Avg = 3, Paeth = 4
                            FilterMethod = inputReader.ReadByte();

                            //None = 0, Adam7 = 1
                            InterlaceMethod = inputReader.ReadByte();

                            pixels = new byte[Width * Height];

                            Validation();
                            break;
                        }

                    case "PLTE":
                        {
                            var palettes = inputReader.ReadBytes(length);
                            break;
                        }

                    case "IDAT":
                        {
                            int cmf = inputReader.ReadByte(); // method
                            int flag = inputReader.ReadByte(); // check

                            if (cmf == -1 || flag == -1)
                            {
                                throw new NotImplementedException();
                            }

                            if ((cmf & 0x0F) == 8)
                            {
                                // CINFO is the base-2 logarithm of the LZ77 window size, minus eight.
                                int cinfo = (cmf & 0xF0) >> 4;

                                if (cinfo > 7)
                                {
                                    throw new NotImplementedException();
                                }
                            }

                            var chunkBytes = inputReader.ReadBytes(length - 2);

                            var inputStream = new MemoryStream(chunkBytes);
                            //var outputStream = new MemoryStream();

                            DeflateStream dataStream = new DeflateStream(inputStream, CompressionMode.Decompress, true);



                            if (InterlaceMethod == 1)//Adam7
                            {
                                throw new NotImplementedException();
                            }
                            else
                            {
                                bytesPerScanline = CalculateScanlineLength(Width) + 1;

                                var ms2 = new MemoryStream(chunkBytes);
                                var ir = new BinaryReader(ms2);

                                
                                byte[] cr = new byte[Width + 1];
                                byte[] pr = new byte[Width + 1];

                                while (currentRow < Height)
                                {
                                    while (currentRowBytesRead < bytesPerScanline)
                                    {
                                        cr = new byte[Width + 1];
                                        
                                        ir.ReadBytes(Width + 1).CopyTo(cr, 0);

                                        int bytesRead = dataStream.Read(cr, currentRowBytesRead, bytesPerScanline - currentRowBytesRead);
                                        if (bytesRead <= 0)
                                        {
#warning esta esta concluido ainda.
                                            return new byte[Width * Height];// return;
                                        }
                                        
                                        currentRowBytesRead += bytesRead;
                                    }

                                    currentRowBytesRead = 0;



                                    int FilterType = cr[0];

                                    byte[] pdat = new byte[Width];
                                    Buffer.BlockCopy(pr, 1, pdat, 0, Width);

                                    byte[] cdat = new byte[Width];
                                    Buffer.BlockCopy(cr, 1, cdat, 0, Width);

                                    Buffer.BlockCopy(cr, 0, pr, 0, Width);

                                    switch (FilterType)
                                    {
                                        case 0://None
                                            break;

                                        case 1://Sub
                                            for (int i = bytesPerPixel; i < cdat.Length; i++)
                                            {
                                                cdat[i] += cdat[i - bytesPerPixel];
                                            }
                                            break;

                                        case 2://Up
                                            for (int i = 0; i < pdat.Length; i++)
                                            {
                                                cdat[i] += pdat[i];
                                            }
                                            break;

                                        case 3://Average
                                            for (int i = 0; i < bytesPerPixel; i++)
                                            {
                                                cdat[i] += (byte)(pdat[i] / 2);
                                            }
                                            for (int i = bytesPerPixel; i < cdat.Length; i++)
                                            {
                                                cdat[i] += (byte)((cdat[i - bytesPerPixel] + pdat[i]) / 2);
                                            }
                                            break;

                                        case 4://Paeth
                                            FilterPaeth(cdat, pdat, bytesPerPixel);
                                            break;

                                        default:
                                            throw new InvalidOperationException();
                                            break;
                                    }

                                    arr.Add(cdat);

                                    currentRow++;
                                }
                            }
                            break;
                        }

                    default:
                        {
                            inputReader.ReadBytes(length);
                            break;
                        }
                }

                uint crc = inputReader.ReadBigEndianUInt32();
            }

            if ((Width * Height) != pixels.Length)
                throw new InvalidOperationException();

            MemoryStream asd = new MemoryStream();
            foreach (byte[] dd in arr)
                asd.Write(dd, 0, dd.Length);

            return asd.ToArray();/*pixels*/;
        }


        void FilterPaeth(byte[] cdat, byte[] pdat, int bytesPerPixel)
        {
            int a, b, c, pa, pb, pc;

            for (int i = 0; i < bytesPerPixel; i++)
            {
                a = c = 0;

                for (int j = i; j < cdat.Length; j += bytesPerPixel)
                {
                    b = pdat[j];
                    pa = b - c;
                    pb = a - c;
                    pc = Math.Abs(pa + pb);
                    pa = Math.Abs(pa);
                    pb = Math.Abs(pb);

                    if (pa <= pb && pa <= pc)
                    {// No-op.
                    }
                    else if (pb <= pc)
                        a = b;
                    else
                        a = c;

                    a += cdat[j];
                    a &= 0xff;
                    cdat[j] = (byte)a;
                    c = b;
                }
            }
        }



        public void Validation()
        {
            if (BitDepth != 8)
                throw new NotImplementedException();

            if (ColorType != 3)
                throw new NotImplementedException();

            if (CompressionMethod != 0)
                throw new NotImplementedException();

            if (FilterMethod != 0)
                throw new NotImplementedException();

            if (InterlaceMethod != 0)
                throw new NotImplementedException();

            //if (InterlaceMethod == 1)//Adam7
            //    throw new NotImplementedException();

            bytesPerPixel = (BitDepth + 7) / 8;

            int rowSize = (BitDepth * Width + 7) / 8;
            if (rowSize != Width)
                throw new NotImplementedException();
        }

        private byte[] Deflate(byte[] bytes)
        {
            var inputStream = new MemoryStream(bytes);
            var deflateStream = new DeflateStream(inputStream, CompressionMode.Decompress);

            bytesPerScanline = CalculateScanlineLength(Width) + 1;

            var buffer = new byte[4096];
            var outputStream = new MemoryStream();
            while (true)
            {
                int l;
                try
                {
                    l = deflateStream.Read(buffer, 0, buffer.Length);
                }
                catch
                {
                    return new byte[0];
                }
                outputStream.Write(buffer, 0, l);

                if (l < buffer.Length)
                {
                    break;
                }
            }

            return outputStream.ToArray();
        }

        private int CalculateScanlineLength(int width)
        {
            int mod = BitDepth == 16 ? 16 : 8;
            int scanlineLength = width * BitDepth * bytesPerPixel;

            int amount = scanlineLength % mod;
            if (amount != 0)
            {
                scanlineLength += mod - amount;
            }

            return scanlineLength / mod;
        }
    }
}
