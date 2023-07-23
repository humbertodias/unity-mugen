using System.Collections.Generic;

namespace UnityMugen.CustomInput
{
    public class SaveData
    {
        public List<ControlScheme> ControlSchemes;
        public string ButtonSystemScheme;
        public string PlayerOneScheme;
        public string PlayerTwoScheme;
        public string PlayerThreeScheme;
        public string PlayerFourScheme;

        public SaveData()
        {
            ControlSchemes = new List<ControlScheme>();
            ButtonSystemScheme = null;
            PlayerOneScheme = null;
            PlayerTwoScheme = null;
            PlayerThreeScheme = null;
            PlayerFourScheme = null;
        }
    }
}
