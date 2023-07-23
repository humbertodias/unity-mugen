using System;
using System.Diagnostics;
using UnityMugen.Collections;

namespace UnityMugen.Animations
{
    /// <summary>
    /// Controls the playing of Animations.
    /// </summary>
    public class AnimationManager
    {
        /// <summary>
        /// Initializes a new instance of this class.
        /// </summary>
        /// <param name="filepath">Path to the AIR file that the Animations were parsed out of.</param>
        /// <param name="animations">Collection of Animations that were parsed of the AIR file.</param>
        public AnimationManager(KeyedCollection<int, Animation> animations)
        {
            if (animations == null) throw new ArgumentNullException(nameof(animations));

            Animations = animations;
            IsForeignAnimation = false;
            CurrentAnimation = null;
            IsAnimationFinished = false;
            Animationinloop = false;
            TimeInAnimation = 0;
            TimeLoop = 0;
            Elementswitchtime = 0;
        }

        /// <summary>
        /// Creates a new AnimationManager contains the same Animations.
        /// </summary>
        /// <returns></returns>
        public AnimationManager Clone()
        {
            return new AnimationManager(Animations);
        }

        /// <summary>
        /// Determines whether or not an Animation is part of the collection.
        /// </summary>
        /// <param name="number">The Animation number that is looked for.</param>
        /// <returns>true if the requested Animation exists; false otherwise.</returns>
        public bool HasAnimation(int number)
        {
            return Animations.Contains(number);
        }

        /// <summary>
        /// Changes the active Animation.
        /// </summary>
        /// <param name="animationnumber">The number of the new Animation.</param>
        /// <param name="elementnumber">The index of the starting element of the new Animation.</param>
        /// <returns>true if the requested Animation is set; false otherwise.</returns>
        public bool SetLocalAnimation(int animationnumber, int elementnumber)
        {
            if (HasAnimation(animationnumber) == false) return false;

            var animation = Animations[animationnumber];
            if (elementnumber < 0 || elementnumber >= animation.Elements.Count) return false;

            IsForeignAnimation = false;
            SetAnimation(animation, animation.Elements[elementnumber]);
            return true;
        }

        /// <summary>
        /// Changes the active Animation to one in a different AnimationManager.
        /// </summary>
        /// <param name="animations">The AnimationManager to get the new Animation out of.</param>
        /// <param name="animationnumber">The number of the new Animation.</param>
        /// <param name="elementnumber">The index of the starting element of the new Animation.</param>
        /// <returns>true if the requested Animation is set; false otherwise.</returns>
        public bool SetForeignAnimation(AnimationManager animations, int animationnumber, int elementnumber)
        {
            if (animations == null) throw new ArgumentNullException(nameof(animations));

            if (animations.HasAnimation(animationnumber) == false) return false;

            var animation = animations.Animations[animationnumber];
            if (elementnumber < 0 || elementnumber >= animation.Elements.Count) return false;

            IsForeignAnimation = true;
            SetAnimation(animation, animation.Elements[elementnumber]);
            return true;
        }

        /// <summary>
        /// Moves the currently active Animation one ticks further in its sequence.
        /// </summary>
        public void UpdateFE()
        {
            if (CurrentAnimation == null || CurrentElement == null)
                return;

            IsAnimationFinished = false;
            ++TimeInAnimation; // Se esta Linha estiver em baixo, dara erro na pesquisa com TE.AnimElemTime
            ++TimeLoop;

            if (TimeLoop > CurrentAnimation.TotalTime)
                TimeLoop = CurrentAnimation.GetElementStartTime(CurrentAnimation.Loopstart);

            if (Elementswitchtime == -1)
                return;

            // ++TimeInAnimation;

            if (Elementswitchtime > 1)
            {
                --Elementswitchtime;
            }
            else
            {
                var newlement = CurrentAnimation.GetNextElement(CurrentElement.Id);

                if (newlement.Id <= CurrentElement.Id)
                {
                    Animationinloop = true;
                    IsAnimationFinished = true;
                }
                CurrentElement = newlement;
                Elementswitchtime = CurrentElement.Gameticks;
            }
        }

        /// <summary>
        /// Changes the active Animation.
        /// </summary>
        /// <param name="animation">The new Animation.</param>
        /// <param name="element">The new AnimationElement of the given Animation.</param>
        private void SetAnimation(Animation animation, AnimationElement element)
        {
            if (animation == null) throw new ArgumentNullException(nameof(animation));
            if (element == null) throw new ArgumentNullException(nameof(element));

            CurrentAnimation = animation;
            CurrentElement = element;
            IsAnimationFinished = false;
            Animationinloop = false;
            TimeInAnimation = CurrentAnimation.GetElementStartTime(CurrentElement.Id);
            TimeLoop = TimeInAnimation;
            Elementswitchtime = CurrentElement.Gameticks;
        }

        /// <summary>
        /// Returns whether the active Animation is from another AnimationManager.
        /// </summary>
        /// <returns>true if the active Animation is from another AnimationManager; false otherwise.</returns>
        public bool IsForeignAnimation;

        /// <summary>
        /// Returns the currently active Animation.
        /// </summary>
        /// <returns>The currently active Animation.</returns>
        public Animation CurrentAnimation;

        /// <summary>
        /// Returns the current element of the active Animation.
        /// </summary>
        /// <returns>The current element of the active Animation.</returns>
        public AnimationElement CurrentElement;

        /// <summary>
        /// Returns whether the last element of the current Animation has been passed.
        /// </summary>
        /// <returns>true if the last element of the current Animation has been passed; false otherwise.</returns>
        public bool IsAnimationFinished;

        /// <summary>
        /// Returns the time, in gameticks, spent in the current Animation.
        /// </summary>
        /// <returns>The time, in gameticks, spent in the current Animation.</returns>
        public int TimeInAnimation;
        public int TimeLoop;

        public int Elementswitchtime;
        public bool Animationinloop;

        public KeyedCollection<int, Animation> Animations;



        #region Fields
        [DebuggerBrowsable(DebuggerBrowsableState.Never)]
        private readonly string m_filepath;
        #endregion
    }
}