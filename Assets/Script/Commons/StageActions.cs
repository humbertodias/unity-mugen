using System;
using System.Collections.Generic;
using UnityEngine;

namespace UnityMugen.Screens
{
    [Serializable]
    public class StageActions
    {
        public List<SpriteRenderer> spritesStage = new List<SpriteRenderer>();

        [Header("Environment Shake")]
        public List<Animation> envShakeAnimations = new List<Animation>();
    }

}
