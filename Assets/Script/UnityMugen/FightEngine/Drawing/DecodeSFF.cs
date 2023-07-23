
namespace UnityMugen.Drawing
{

    public static class DecodeSFF
    {
        // V 1.0
        public static byte[] RlePcxDecode(ref SpriteUnpack s, byte[] rle)
        {
            if (rle.Length == 0 || s.rle <= 0)
            {
                return rle;
            }

            byte[] p = new byte[(int)s.Size[0] * (int)s.Size[1]]; //25, 25
            int i = 0, j = 0, k = 0, w = (int)s.Size[0];

            for (; j < p.Length;)
            {
                int n = 1;
                byte d = rle[i];
                if (i < rle.Length - 1)
                {
                    i++;
                }
                if (d >= 0xc0)
                {
                    n = (int)(d & 0x3f);
                    d = rle[i];
                    if (i < rle.Length - 1)
                    {
                        i++;
                    }
                }
                for (; n > 0; n--)
                {
                    if (k < w && j < p.Length)
                    {
                        p[j] = d;
                        j++;
                    }
                    k++;
                    if (k == s.rle)
                    {
                        k = 0;
                        n = 1;
                    }
                }
            }

            s.rle = 0;
            MiscSprites.Flip(s.Size, ref p);
            return p;
        }



        // V 2.0
        public static byte[] Rle8Decode(ref SpriteUnpack s, byte[] rle)
        {
            if (rle.Length == 0)
            {
                return rle;
            }

            byte[] p = new byte[(int)s.Size[0] * (int)s.Size[1]];
            int i = 0, j = 0;

            while (j < p.Length)
            {
                int n = 1;
                byte d = rle[i];
                if (i < rle.Length - 1)
                {
                    i++;
                }
                if ((d & 0xc0) == 0x40)
                {
                    n = (int)(d & 0x3f);
                    d = rle[i];
                    if (i < rle.Length - 1)
                    {
                        i++;
                    }
                }
                for (; n > 0; n--)
                {
                    if (j < p.Length)
                    {
                        p[j] = d;
                        j++;
                    }
                }
            }
            MiscSprites.Flip(s.Size, ref p);
            return p;
        }

        public static byte[] Rle5Decode(ref SpriteUnpack s, byte[] rle)
        {
            if (rle.Length == 0)
            {
                return rle;
            }

            byte[] p = new byte[(int)s.Size[0] * (int)s.Size[1]];
            int i = 0, j = 0;

            for (; j < p.Length;)
            {
                int rl = rle.Length;
                if (i < rle.Length - 1)
                {
                    i++;
                }
                int dl = rle[i] & 0x7f;
                byte c = 0;

                if ((rle[i] >> 7) != 0)
                {
                    if (i < rle.Length - 1)
                    {
                        i++;
                    }
                    c = rle[i];
                }
                if (i < rle.Length - 1)
                {
                    i++;
                }
                while (true)
                {
                    if (j < p.Length)
                    {
                        p[j] = c;
                        j++;
                    }
                    rl--;
                    if (rl < 0)
                    {
                        dl--;
                        if (dl < 0)
                            break;

                        c = (byte)(rle[i] & 0x1f);
                        rl = rle[i] >> 5;
                        if (i < rle.Length - 1)
                        {
                            i++;
                        }
                    }
                }
            }
            MiscSprites.Flip(s.Size, ref p);
            return p;
        }

        public static byte[] Lz5Decode(ref SpriteUnpack s, byte[] rle)
        {
            if (rle.Length == 0)
            {
                return rle;
            }

            byte[] p = new byte[(int)s.Size[0] * (int)s.Size[1]];
            int i = 0, j = 0, n = 0;
            byte ct = rle[i];
            uint cts = 0;
            byte rb = 0;
            uint rbc = 0;

            if (i < rle.Length - 1)
            {
                i++;
            }

            while (j < p.Length)
            {
                int d = rle[i];
                if (i < rle.Length - 1)
                {
                    i++;
                }

                if ((ct & (byte)(1 << (int)cts)) != 0)
                {
                    if ((d & 0x3f) == 0)
                    {
                        d = (d << 2 | rle[i]) + 1;
                        if (i < rle.Length - 1)
                        {
                            i++;
                        }

                        n = rle[i] + 2;
                        if (i < rle.Length - 1)
                        {
                            i++;
                        }
                    }
                    else
                    {
                        rb |= (byte)((d & 0xc0) >> (int)rbc);
                        rbc += 2;
                        n = d & 0x3f;
                        if (rbc < 8)
                        {
                            d = rle[i] + 1;
                            if (i < rle.Length - 1)
                            {
                                i++;
                            }
                        }
                        else
                        {
                            d = rb + 1;
                            rb = 0;
                            rbc = 0;
                        }
                    }

                    while (true)
                    {
                        if (j < p.Length)
                        {
                            p[j] = p[j - d];
                            j++;
                        }
                        n--;
                        if (n < 0)
                        {
                            break;
                        }
                    }
                }
                else
                {
                    if ((d & 0xe0) == 0)
                    {
                        n = rle[i] + 8;
                        if (i < rle.Length - 1)
                        {
                            i++;
                        }
                    }
                    else
                    {
                        n = d >> 5;
                        d &= 0x1f;
                    }

                    for (; n > 0; n--)
                    {
                        if (j < p.Length)
                        {
                            p[j] = (byte)d;
                            j++;
                        }
                    }
                }
                cts++;
                if (cts >= 8)
                {
                    ct = rle[i];
                    cts = 0;
                    if (i < rle.Length - 1)
                    {
                        i++;
                    }
                }

            }
            MiscSprites.Flip(s.Size, ref p);
            return p;
        }

    }
}