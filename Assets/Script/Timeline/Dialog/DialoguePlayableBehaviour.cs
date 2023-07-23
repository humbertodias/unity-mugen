using System;
using UnityEngine;
using UnityEngine.Playables;

namespace UnityMugen.Timeline
{

    // The Serializable attribute is required to be animated by timeline, and used as a template.
    [Serializable]
    public class DialoguePlayableBehaviour : PlayableBehaviour
    {

        public TypeSideName typeSideName;

        public string nameText = "";

        [Tooltip("The text to display")]
        [TextAreaAttribute]
        public string dialogText = "";

        [Range(0f, .05f)]
        // O buttom [Hide Curves View] só aparece se tiver uma variavel float
        // em uma class que erda PlayableBehaviour.
        public float fadeDialogText;

        public TextAnchor textAnchor;

    }

}
