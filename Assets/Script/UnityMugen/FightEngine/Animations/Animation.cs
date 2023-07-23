using System;
using System.Collections.Generic;

namespace UnityMugen.Animations
{
    public class Animation
    {
        public Animation(int number, int loopstart, List<AnimationElement> elements)
        {
            if (number < 0) throw new ArgumentOutOfRangeException(nameof(number));
            if (loopstart < 0) throw new ArgumentOutOfRangeException(nameof(loopstart));
            if (elements == null) throw new ArgumentNullException(nameof(elements));
            if (elements.Count == 0) throw new ArgumentException("elements");
            if (loopstart >= elements.Count) throw new ArgumentOutOfRangeException(nameof(loopstart));

            Number = number;
            Loopstart = loopstart;
            Elements = elements;/*new ListIterator<AnimationElementUnpack>(elements);*/
            TotalTime = CalculateTotalTime();
        }

        private int CalculateTotalTime()
        {
            var time = 0;

            foreach (var element in this)
            {
                if (element.Gameticks == -1) return -1;
                time += element.Gameticks;
            }

            return time;
        }

        public int GetElementStartTime(int elementnumber)
        {
            if (elementnumber < 0 || elementnumber >= Elements.Count)
                throw new ArgumentOutOfRangeException(nameof(elementnumber));

            return Elements[elementnumber].StartTick;
        }

        public AnimationElement GetNextElement(int elementnumber)
        {
            if (elementnumber < 0 || elementnumber >= Elements.Count)
                throw new ArgumentOutOfRangeException(nameof(elementnumber));

            ++elementnumber;
            return elementnumber < Elements.Count ? Elements[elementnumber] : Elements[Loopstart];
        }

        public AnimationElement GetElementFromTime(int time)
        {
            if (time < 0) throw new ArgumentOutOfRangeException(nameof(time));

            for (var element = Elements[0]; element != null; element = GetNextElement(element.Id))
            {
                if (element.Gameticks == -1) return element;

                time -= element.Gameticks;
                if (time < 0) return element;
            }

            throw new InvalidOperationException();
        }

        public List<AnimationElement>.Enumerator GetEnumerator()
        {
            return Elements.GetEnumerator();
        }

        public override string ToString()
        {
            return "Animation #" + Number.ToString();
        }

        public int Number;
        public int Loopstart;
        public List<AnimationElement> Elements;
        public int TotalTime;


    }
}