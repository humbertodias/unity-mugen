using UnityEngine;

namespace UnityMugen.CustomInput
{
    public class InputCustom : MonoBehaviour
    {

        private static int maxTick = 5;

        private static bool pressUpPlayerIdOne;
        private static bool pressDownPlayerIdOne;
        private static bool pressLeftPlayerIdOne;
        private static bool pressRightPlayerIdOne;
        private static float timeNextTickVerticalPlayerIdOne = 0;
        private static int totalTickVerticalPlayerIdOne = 1;
        private static float timeNextTickHorizontallayerIdOne = 0;
        private static int totalTickHorizontalPlayerIdOne = 1;


        private static bool pressUpPlayerIdTwo;
        private static bool pressDownPlayerIdTwo;
        private static bool pressLeftPlayerIdTwo;
        private static bool pressRightPlayerIdTwo;
        private static float timeNextTickVerticalPlayerIdTwo = 0;
        private static int totalTickVerticalPlayerIdTwo = 1;
        private static float timeNextTickHorizontallayerIdTwo = 0;
        private static int totalTickHorizontalPlayerIdTwo = 1;

        public static int LastBindingAcitivPlayerIDOne = 0;
        public static int LastBindingAcitivPlayerIDTwo = 0;
        public static int LastBindingAcitivPlayerIDThree = 0;
        public static int LastBindingAcitivPlayerIDFour = 0;

        public static bool ActiveUpdate = true;

        private void Update()
        {
            if (ActiveUpdate)
            {
                PlayerIDOne();
                PlayerIDTwo();
            }
            //if (pressUp && pressDown)
            //    pressDown = false;
            //if (pressRight && pressLeft)
            //    pressLeft = false;
        }

        public static bool PressUpPlayerIDOne()
        {
            return pressUpPlayerIdOne;
        }
        public static bool PressDownPlayerIDOne()
        {
            return pressDownPlayerIdOne;
        }
        public static bool PressLeftPlayerIDOne()
        {
            return pressLeftPlayerIdOne;
        }
        public static bool PressRightPlayerIDOne()
        {
            return pressRightPlayerIdOne;
        }


        public static bool PressUpPlayerIDTwo()
        {
            return pressUpPlayerIdTwo;
        }
        public static bool PressDownPlayerIDTwo()
        {
            return pressDownPlayerIdTwo;
        }
        public static bool PressLeftPlayerIDTwo()
        {
            return pressLeftPlayerIdTwo;
        }
        public static bool PressRightPlayerIDTwo()
        {
            return pressRightPlayerIdTwo;
        }


        private void PlayerIDOne()
        {
            float vertical = InputManager.GetAxis("Vertical", PlayerID.One);
            float horizontal = InputManager.GetAxis("Horizontal", PlayerID.One);
            if (horizontal != 0)
            {
                if (horizontal > 0 && timeNextTickHorizontallayerIdOne == 0f)
                {
                    pressRightPlayerIdOne = true;
                    timeNextTickHorizontallayerIdOne += Time.deltaTime;
                }
                else if (horizontal > 0)
                {
                    pressRightPlayerIdOne = false;
                    timeNextTickHorizontallayerIdOne += (Time.deltaTime * totalTickHorizontalPlayerIdOne);

                }
                if (timeNextTickHorizontallayerIdOne >= 0.5f)
                {
                    if (totalTickHorizontalPlayerIdOne < maxTick)
                        totalTickHorizontalPlayerIdOne++;
                    timeNextTickHorizontallayerIdOne = 0f;
                }


                if (horizontal < 0 && timeNextTickHorizontallayerIdOne == 0f)
                {
                    pressLeftPlayerIdOne = true;
                    timeNextTickHorizontallayerIdOne += Time.deltaTime;
                }
                else if (horizontal < 0)
                {
                    pressLeftPlayerIdOne = false;
                    timeNextTickHorizontallayerIdOne += (Time.deltaTime * totalTickHorizontalPlayerIdOne);

                }
                if (timeNextTickHorizontallayerIdOne >= 0.5f)
                {
                    if (totalTickHorizontalPlayerIdOne < maxTick)
                        totalTickHorizontalPlayerIdOne++;
                    timeNextTickHorizontallayerIdOne = 0f;
                }
            }
            else
            {
                totalTickHorizontalPlayerIdOne = 1;
                timeNextTickHorizontallayerIdOne = 0;
                pressLeftPlayerIdOne = false;
                pressRightPlayerIdOne = false;
            }


            if (vertical != 0)
            {
                if (vertical < 0 && timeNextTickVerticalPlayerIdOne == 0f)
                {
                    pressUpPlayerIdOne = true;
                    timeNextTickVerticalPlayerIdOne += Time.deltaTime;
                }
                else if (vertical < 0)
                {
                    pressUpPlayerIdOne = false;
                    timeNextTickVerticalPlayerIdOne += (Time.deltaTime * totalTickVerticalPlayerIdOne);

                }
                if (timeNextTickVerticalPlayerIdOne >= 0.5f)
                {
                    if (totalTickVerticalPlayerIdOne < maxTick)
                        totalTickVerticalPlayerIdOne++;
                    timeNextTickVerticalPlayerIdOne = 0f;
                }


                if (vertical > 0 && timeNextTickVerticalPlayerIdOne == 0f)
                {
                    pressDownPlayerIdOne = true;
                    timeNextTickVerticalPlayerIdOne += Time.deltaTime;
                }
                else if (vertical > 0)
                {
                    pressDownPlayerIdOne = false;
                    timeNextTickVerticalPlayerIdOne += (Time.deltaTime * totalTickVerticalPlayerIdOne);

                }
                if (timeNextTickVerticalPlayerIdOne >= 0.5f)
                {
                    if (totalTickVerticalPlayerIdOne < maxTick)
                        totalTickVerticalPlayerIdOne++;
                    timeNextTickVerticalPlayerIdOne = 0f;
                }
            }
            else
            {
                totalTickVerticalPlayerIdOne = 1;
                timeNextTickVerticalPlayerIdOne = 0;
                pressDownPlayerIdOne = false;
                pressUpPlayerIdOne = false;
            }

        }





        private void PlayerIDTwo()
        {
            float vertical = InputManager.GetAxis("Vertical", PlayerID.Two);
            float horizontal = InputManager.GetAxis("Horizontal", PlayerID.Two);
            if (horizontal != 0)
            {
                if (horizontal > 0 && timeNextTickHorizontallayerIdTwo == 0f)
                {
                    pressRightPlayerIdTwo = true;
                    timeNextTickHorizontallayerIdTwo += Time.deltaTime;
                }
                else if (horizontal > 0)
                {
                    pressRightPlayerIdTwo = false;
                    timeNextTickHorizontallayerIdTwo += (Time.deltaTime * totalTickHorizontalPlayerIdTwo);

                }
                if (timeNextTickHorizontallayerIdTwo >= 0.5f)
                {
                    if (totalTickHorizontalPlayerIdTwo < maxTick)
                        totalTickHorizontalPlayerIdTwo++;
                    timeNextTickHorizontallayerIdTwo = 0f;
                }


                if (horizontal < 0 && timeNextTickHorizontallayerIdTwo == 0f)
                {
                    pressLeftPlayerIdTwo = true;
                    timeNextTickHorizontallayerIdTwo += Time.deltaTime;
                }
                else if (horizontal < 0)
                {
                    pressLeftPlayerIdTwo = false;
                    timeNextTickHorizontallayerIdTwo += (Time.deltaTime * totalTickHorizontalPlayerIdTwo);

                }
                if (timeNextTickHorizontallayerIdTwo >= 0.5f)
                {
                    if (totalTickHorizontalPlayerIdTwo < maxTick)
                        totalTickHorizontalPlayerIdTwo++;
                    timeNextTickHorizontallayerIdOne = 0f;
                }
            }
            else
            {
                totalTickHorizontalPlayerIdTwo = 1;
                timeNextTickHorizontallayerIdTwo = 0;
                pressLeftPlayerIdTwo = false;
                pressRightPlayerIdTwo = false;
            }


            if (vertical != 0)
            {
                if (vertical < 0 && timeNextTickVerticalPlayerIdTwo == 0f)
                {
                    pressUpPlayerIdTwo = true;
                    timeNextTickVerticalPlayerIdTwo += Time.deltaTime;
                }
                else if (vertical < 0)
                {
                    pressUpPlayerIdTwo = false;
                    timeNextTickVerticalPlayerIdTwo += (Time.deltaTime * totalTickVerticalPlayerIdTwo);

                }
                if (timeNextTickVerticalPlayerIdTwo >= 0.5f)
                {
                    if (totalTickVerticalPlayerIdTwo < maxTick)
                        totalTickVerticalPlayerIdTwo++;
                    timeNextTickVerticalPlayerIdTwo = 0f;
                }


                if (vertical > 0 && timeNextTickVerticalPlayerIdTwo == 0f)
                {
                    pressDownPlayerIdTwo = true;
                    timeNextTickVerticalPlayerIdTwo += Time.deltaTime;
                }
                else if (vertical > 0)
                {
                    pressDownPlayerIdTwo = false;
                    timeNextTickVerticalPlayerIdTwo += (Time.deltaTime * totalTickVerticalPlayerIdTwo);

                }
                if (timeNextTickVerticalPlayerIdTwo >= 0.5f)
                {
                    if (totalTickVerticalPlayerIdTwo < maxTick)
                        totalTickVerticalPlayerIdTwo++;
                    timeNextTickVerticalPlayerIdTwo = 0f;
                }
            }
            else
            {
                totalTickVerticalPlayerIdTwo = 1;
                timeNextTickVerticalPlayerIdTwo = 0;
                pressDownPlayerIdTwo = false;
                pressUpPlayerIdTwo = false;
            }

        }


    }
}