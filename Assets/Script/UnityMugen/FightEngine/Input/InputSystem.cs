using System;
using UnityMugen.CustomInput;

/// <summary>
/// Interfaces between keyboard input and game code.
/// </summary>
/// 

namespace UnityMugen.Input
{

    [Serializable]
    public class CommandSetInput
    {
        public bool PressUp = false;
        public bool PressDown = false;
        public bool PressLeft = false;
        public bool PressRight = false;
        public bool PressX = false;
        public bool PressY = false;
        public bool PressZ = false;
        public bool PressA = false;
        public bool PressB = false;
        public bool PressC = false;
        public bool PressStart = false;
        public bool PressSelect = false;
        public bool PressTaunt = false;

        public void ClearAll()
        {
            PressUp = false;
            PressDown = false;
            PressLeft = false;
            PressRight = false;
            PressX = false;
            PressY = false;
            PressZ = false;
            PressA = false;
            PressB = false;
            PressC = false;
        }
    }


    [Serializable]
    public class InputSystem
    {

        private InitializationSettings initialization;
        private static LauncherEngine Launcher => LauncherEngine.Inst;

        public PlayerButton playerButton1Newtwork;
        public PlayerButton playerButton2Newtwork;

        public int? player1SelectedOptions;
        public int? player2SelectedOptions;

        public CommandSetInput commandSetInputP1 = new CommandSetInput();
        public CommandSetInput commandSetInputP2 = new CommandSetInput();

        public bool isControllersActives { get; set; }

        public InputSystem Inicialize()
        {
            initialization = Launcher.initializationSettings;
            isControllersActives = true;
            return this;
        }

        public PlayerButton KeyboardStateP1(bool onlyOneButton = false)
        {
            PlayerButton b = KeyboardState(initialization.controller1, onlyOneButton);
            return b |= CommandSetInput(commandSetInputP1);
        }

        public PlayerButton KeyboardStateP2(bool onlyOneButton = false)
        {
            PlayerButton b = KeyboardState(initialization.controller2, onlyOneButton);

            if (Launcher.mugen.Engine.Initialization.Mode == CombatMode.Training &&
                Launcher.trainnerSettings.stanceType != StanceType.Controller)
                b = PlayerButton.None;

            return b |= CommandSetInput(commandSetInputP2);
        }



        public PlayerButton KeyboardState(PlayerID controller, bool onlyOneButton)
        {
            PlayerButton ks = PlayerButton.None;

            if (isControllersActives && InputManager.GetAxis("Horizontal", controller) != 0)
            {
                float value = InputManager.GetAxis("Horizontal", controller);
                if (value == 1)
                    ks |= PlayerButton.Right;
                else if (value == -1)
                    ks |= PlayerButton.Left;
            }
            if (isControllersActives && InputManager.GetAxis("Vertical", controller) != 0)
            {
                float value = InputManager.GetAxis("Vertical", controller);
                if (value == -1)
                    ks |= PlayerButton.Up;
                else if (value == 1)
                    ks |= PlayerButton.Down;
            }
            if (isControllersActives && InputManager.GetButton("X", controller))
            {
                ks |= PlayerButton.X;
            }
            if (isControllersActives && InputManager.GetButton("Y", controller))
            {
                ks |= PlayerButton.Y;
            }
            if (isControllersActives && InputManager.GetButton("Z", controller))
            {
                ks |= PlayerButton.Z;
            }
            if (isControllersActives && InputManager.GetButton("A", controller))
            {
                ks |= PlayerButton.A;
            }
            if (isControllersActives && InputManager.GetButton("B", controller))
            {
                ks |= PlayerButton.B;
            }
            if (isControllersActives && InputManager.GetButton("C", controller))
            {
                ks |= PlayerButton.C;
            }
            if (isControllersActives && InputManager.GetButton("Taunt", controller))
            {
                ks |= PlayerButton.Taunt;
            }
            if (isControllersActives && InputManager.GetButton("Start", controller))
            {
                ks |= PlayerButton.Start;
            }
            if (isControllersActives && InputManager.GetButton("Select", controller))
            {
                ks |= PlayerButton.Select;
            }


            if (isControllersActives && (InputManager.GetButton("LP_LK", controller)))
            {
                ks |= PlayerButton.X;
                ks |= PlayerButton.A;
            }
            if (isControllersActives && (InputManager.GetButton("MP_MK", controller)))
            {
                ks |= PlayerButton.Y;
                ks |= PlayerButton.B;
            }
            if (isControllersActives && (InputManager.GetButton("SP_SK", controller)))
            {
                ks |= PlayerButton.Z;
                ks |= PlayerButton.C;
            }
            if (isControllersActives && (InputManager.GetButton("LP_MP_SP", controller)))
            {
                ks |= PlayerButton.X;
                ks |= PlayerButton.Y;
                ks |= PlayerButton.Z;
            }
            if (isControllersActives && (InputManager.GetButton("LK_MK_SK", controller)))
            {
                ks |= PlayerButton.A;
                ks |= PlayerButton.B;
                ks |= PlayerButton.C;
            }

            return ks;
        }


        public PlayerButton CommandSetInput(CommandSetInput input)
        {
            PlayerButton ks = PlayerButton.None;

            if (isControllersActives && (input.PressLeft || input.PressRight))
            {
                if (input.PressRight)
                    ks |= PlayerButton.Right;
                else if (input.PressLeft)
                    ks |= PlayerButton.Left;
            }
            if (input.PressUp || input.PressDown)
            {
                if (input.PressUp)
                    ks |= PlayerButton.Up;
                else if (input.PressDown)
                    ks |= PlayerButton.Down;
            }
            if (isControllersActives && input.PressX)
            {
                ks |= PlayerButton.X;
            }
            if (isControllersActives && input.PressY)
            {
                ks |= PlayerButton.Y;
            }
            if (isControllersActives && input.PressZ)
            {
                ks |= PlayerButton.Z;
            }
            if (isControllersActives && input.PressA)
            {
                ks |= PlayerButton.A;
            }
            if (isControllersActives && input.PressB)
            {
                ks |= PlayerButton.B;
            }
            if (isControllersActives && input.PressC)
            {
                ks |= PlayerButton.C;
            }
            if (isControllersActives && input.PressTaunt)
            {
                ks |= PlayerButton.Taunt;
            }
            if (isControllersActives && input.PressStart)
            {
                ks |= PlayerButton.Start;
            }
            if (isControllersActives && input.PressSelect)
            {
                ks |= PlayerButton.Select;
            }

            input.ClearAll();

            return ks;
        }

    }


    public class NetworkControl
    {
        public int playerNumber;
        public PlayerButton buttonPress;

        LauncherEngine Launcher => LauncherEngine.Inst;

        public void DoUpdate()
        {
            buttonPress = Launcher.inputSystem.KeyboardStateP1();
        }

        public void DoFixedUpdate()
        {
            buttonPress = Launcher.inputSystem.KeyboardStateP1();
        }
    }
}