using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Text.RegularExpressions;
using UnityEngine;
using UnityMugen.Collections;
using UnityMugen.Drawing;
using UnityMugen.IO;
using UnityMugen.Video;

namespace UnityMugen.Animations
{
    public class AnimationLoader
    {

        public AnimationLoader()
        {
            m_animationtitleregex = new Regex(@"^\s*Begin action\s+(-?\d+)(,.+)?\s*$", RegexOptions.IgnoreCase);
            m_clsnregex = new Regex(@"Clsn([12])(Default)?:\s*(\d+)", RegexOptions.IgnoreCase);
            m_clsnlineregex = new Regex(@"Clsn([12])?\[(-?\d+)\]\s*=\s*(-?\d+)\s*,\s*(-?\d+)\s*,\s*(-?\d+)\s*,\s*(-?\d+)", RegexOptions.IgnoreCase);
            m_elementregex = new Regex(@"\s*,\s*", RegexOptions.IgnoreCase);
        }

        public KeyedCollection<int, Animation> LoadAnimations(TextFile textfile, Vector2 scale)
        {
            if (textfile == null) throw new ArgumentNullException(nameof(textfile));

            var animations = new KeyedCollection<int, Animation>(x => x.Number);

            foreach (var section in textfile)
            {
                var animation = CreateAnimation(section, scale);
                if (animation != null)
                {
                    if (animations.Contains(animation.Number) == false)
                    {
                        animations.Add(animation);
                    }
                    else
                    {
                        UnityEngine.Debug.LogWarningFormat("Duplicate animation #{0}. Discarding duplicate.", animation.Number);
                    }
                }
            }

            return animations;
        }

        public bool IsClsnLine(string line)
        {
            if (line == null) throw new ArgumentNullException(nameof(line));

            return m_clsnlineregex.IsMatch(line);
        }

        private Animation CreateAnimation(TextSection section, Vector2 scale)
        {
            if (section == null) throw new ArgumentNullException(nameof(section));

            var titlematch = m_animationtitleregex.Match(section.Title);
            if (titlematch.Success == false) return null;

            var sc = StringComparer.OrdinalIgnoreCase;

            var animationnumber = int.Parse(titlematch.Groups[1].Value);

            var loopstart = 0;
            var starttick = 0;
            var elements = new List<AnimationElement>();

            var loading_type1 = new List<Rect>();
            var loading_type2 = new List<Rect>();
            var default_type1 = new List<Rect>();
            var default_type2 = new List<Rect>();

            var loaddefault = false;
            var loadtype = ClsnType.None;
            var loadcount = 0;

            foreach (var line in section.Lines)
            {
                if (loadcount > 0)
                {
                    var clsn = CreateClsn(line/*, loadtype*/);
                    if (clsn != null)
                    {
                        if (loaddefault)
                        {
                            if (loadtype == ClsnType.Type1Attack) default_type1.Add(clsn.Value);
                            if (loadtype == ClsnType.Type2Normal) default_type2.Add(clsn.Value);
                        }
                        else
                        {
                            if (loadtype == ClsnType.Type1Attack) loading_type1.Add(clsn.Value);
                            if (loadtype == ClsnType.Type2Normal) loading_type2.Add(clsn.Value);
                        }
                    }
                    else
                    {
                        UnityEngine.Debug.LogWarningFormat("Could not create Clsn from line: {0}", line);
                    }

                    --loadcount;
                    continue;
                }

                var clsnmatch = m_clsnregex.Match(line);
                if (clsnmatch.Success)
                {
                    var clsntype = ClsnType.None;
                    if (sc.Equals(clsnmatch.Groups[1].Value, "1")) clsntype = ClsnType.Type1Attack;
                    if (sc.Equals(clsnmatch.Groups[1].Value, "2")) clsntype = ClsnType.Type2Normal;

                    var isdefault = sc.Equals(clsnmatch.Groups[2].Value, "default");
                    if (isdefault)
                    {
                        if (clsntype == ClsnType.Type1Attack) default_type1.Clear();
                        if (clsntype == ClsnType.Type2Normal) default_type2.Clear();
                    }

                    loadcount = int.Parse(clsnmatch.Groups[3].Value);
                    loaddefault = isdefault;
                    loadtype = clsntype;
                    continue;
                }

                if (sc.Equals(line, "Loopstart"))
                {
                    loopstart = elements.Count;
                    continue;
                }

                var element = CreateElement(line, scale, elements.Count, starttick, default_type1, default_type2, loading_type1, loading_type2);
                if (element != null)
                {
                    if (element.Gameticks == -1)
                    {
                        starttick = -1;
                    }
                    else
                    {
                        starttick += element.Gameticks;
                    }

                    elements.Add(element);

                    loading_type1.Clear();
                    loading_type2.Clear();
                }
                else
                {
                    UnityEngine.Debug.LogWarningFormat("Error parsing element for Animation #{0} - {1}", animationnumber, line);
                }
            }

            if (elements.Count == 0)
            {
                UnityEngine.Debug.LogWarningFormat("No elements defined for Animation #{0}", animationnumber);
                return null;
            }

            if (loopstart == elements.Count) loopstart = 0;

            return new Animation(animationnumber, loopstart, elements);
        }

        private Rect? CreateClsn(string line/*, ClsnType overridetype*/)
        {
            if (string.Compare(line, 0, "Clsn", 0, 4, StringComparison.OrdinalIgnoreCase) != 0) return null;

            var match = m_clsnlineregex.Match(line);
            if (match.Success == false) return null;

            var x1 = int.Parse(match.Groups[3].Value);
            var y1 = int.Parse(match.Groups[4].Value);
            var x2 = int.Parse(match.Groups[5].Value);
            var y2 = int.Parse(match.Groups[6].Value);

            if (x1 > x2) Misc.Swap(ref x1, ref x2);
            if (y1 > y2) Misc.Swap(ref y1, ref y2);

            Clsn clsn = new Clsn(/*overridetype,*/ new Rect(x1, y1, x2 - x1, y2 - y1));

            Rect rect = MudaScala(RectAirToCollider(clsn));

            return rect;
        }

        private Rect MudaScala(Rect rects)
        {
            return new Rect(rects.x * Constant.Scale, rects.y * Constant.Scale, rects.width * Constant.Scale, rects.height * Constant.Scale);
        }

        private Rect RectAirToCollider(Clsn clsn)
        {
            Rect newRect = new Rect();
            Rect rect = clsn.Rect;
            float xMin = rect.xMin;
            float xMax = rect.xMax;
            float yMin = rect.yMin;
            float yMax = rect.yMax;

            float sizeX = rect.width;
            float sizeY = rect.height;
            float offSetX = xMin + (sizeX / 2);
            float offSetY = yMin + (sizeY / 2);

            newRect.center = new Vector2(offSetX, ((yMin >= 0 && yMax >= 0 && offSetY < 0) ? offSetY : offSetY * -1));
            newRect.size = new Vector2(sizeX, sizeY);
            return newRect;
        }

        private AnimationElement CreateElement(string line, Vector2 scale, int elementid, int starttick, List<Rect> default_type1, List<Rect> default_type2, List<Rect> loading_type1, List<Rect> loading_type2)
        {

            if (line == null) throw new ArgumentNullException(nameof(line));
            if (elementid < 0) throw new ArgumentOutOfRangeException(nameof(elementid));
            if (starttick < 0)
            {
#warning hack Tiago teste para fazer o kid goku funcionar
                starttick = 0; // Teste
                               //            throw new ArgumentOutOfRangeException(nameof(starttick)); 
            }
            if (default_type1 == null) throw new ArgumentNullException(nameof(default_type1));
            if (default_type2 == null) throw new ArgumentNullException(nameof(default_type2));
            if (loading_type1 == null) throw new ArgumentNullException(nameof(loading_type1));
            if (loading_type2 == null) throw new ArgumentNullException(nameof(loading_type2));

            var elements = m_elementregex.Split(line);
            if (elements == null) return null;

            int groupnumber;
            if (int.TryParse(elements[0], out groupnumber) == false) return null;

            int imagenumber;
            if (int.TryParse(elements[1], out imagenumber) == false) return null;

            int offset_x;
            if (int.TryParse(elements[2], out offset_x) == false) return null;

            int offset_y;
            if (int.TryParse(elements[3], out offset_y) == false) return null;

            int gameticks;
            if (int.TryParse(elements[4], out gameticks) == false) return null;

            var flip = SpriteEffects.None;
            if (elements.Length >= 6)
            {
                if (elements[5].IndexOf('H') != -1 || elements[5].IndexOf('h') != -1) flip |= SpriteEffects.FlipHorizontally;
                if (elements[5].IndexOf('V') != -1 || elements[5].IndexOf('v') != -1) flip |= SpriteEffects.FlipVertically;
            }

            var blending = new Blending();
            if (elements.Length >= 7)
            {
                blending = new StringConverter().Convert<Blending>(elements[6]);
            }

            float scale_x = 1, scale_y = 1;
            if (elements.Length >= 9)
            {
                float.TryParse(elements[7], out scale_x);
                float.TryParse(elements[8], out scale_y);
            }

            float angle = 0;
            if (elements.Length >= 10)
            {
                float.TryParse(elements[9], out angle);
            }

            var clsnAttack = new List<Rect>();
            var clsnNormal = new List<Rect>();
            clsnAttack.AddRange(loading_type1.Count != 0 ? loading_type1 : default_type1);
            clsnNormal.AddRange(loading_type2.Count != 0 ? loading_type2 : default_type2);

            var element = new AnimationElement(elementid, clsnAttack, clsnNormal, gameticks, starttick, new SpriteId(groupnumber, imagenumber), new Vector2(offset_x, offset_y), flip, blending, new Vector2(scale_x, scale_y), angle, scale);
            return element;
        }

        [DebuggerBrowsable(DebuggerBrowsableState.Never)]
        private readonly Regex m_animationtitleregex;

        [DebuggerBrowsable(DebuggerBrowsableState.Never)]
        private readonly Regex m_clsnregex;

        [DebuggerBrowsable(DebuggerBrowsableState.Never)]
        private readonly Regex m_clsnlineregex;

        [DebuggerBrowsable(DebuggerBrowsableState.Never)]
        private readonly Regex m_elementregex;

    }
}