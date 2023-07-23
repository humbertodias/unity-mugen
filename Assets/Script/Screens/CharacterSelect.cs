using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

namespace UnityMugen.Screens
{
    [ExecuteInEditMode]
    public class CharacterSelect : MonoBehaviour
    {
        public PlayerSelectType playerSelectType;
        
        [Header("Directions Select")]
        public CharacterSelect posiUp;
        public CharacterSelect posiDown;
        public CharacterSelect posiLeft;
        public CharacterSelect posiRight;

        [Header("Info Select")]
        public PlayerProfileManager profile;
        public RuntimeAnimatorController animator;

        private Sprite lastSprite;

        //private void Start()
        //{
        //    lastSprite = transform.Find("BackGroundSelect/ImageChar")?.GetComponent<Image>()?.sprite;
        //}

        private void Update()
        {
            if (profile != null && profile.largePortrait != null)
            {
                if (lastSprite != profile.largePortrait)
                {
                    lastSprite = transform.Find("BackGroundSelect/ImageChar").GetComponent<Image>().sprite = profile.largePortrait;
                    gameObject.name = profile.charName;
                }
            }
        }
    }
}