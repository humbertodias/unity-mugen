using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace UnityMugen.CustomInput
{
    public struct GamepadVibration//, IGamepadVibration
    {
        public float LeftMotor;
        public float RightMotor;

        public GamepadVibration(float leftMotor, float rightMotor)
        {
            LeftMotor = leftMotor;
            RightMotor = rightMotor;
        }
    }
}
