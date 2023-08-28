using System;
using UnityEngine;

namespace UnityMugen.Combat
{
    public class StateZoom
    {
        public float Scale;
        public Vector2 Pos;
        public int Time;
        public float Lag;

        public void Reset()
        {
            Scale = 1f;
            Pos = Vector2.zero;
            Time = 0;
            Lag = 0f;
        }

        public void SetStateZoom(float scale, Vector2 pos, int time, float lag)
        {
            Scale = scale;
            Pos = pos;
            Time = time;
            Lag = lag;
        }
    }
    public class CameraFE : MonoBehaviour
    {
        static FightEngine Engine => LauncherEngine.Inst.mugen.Engine;

        public StateZoom StateZoom { get; private set; }
        public Vector3 lastPosCamFollow;
        [NonSerialized] public new Camera camera;
        public Bound CameraBounds;
        public Vector2 Location;

        public Transform transUp;
        public Transform transDown;
        public Transform transLeft;
        public Transform transRight;
        public Transform transCenter;

        //public float up;
        //public float down;
        //public float left;
        //public float right;
        public Rect Corners;

        public bool isNormalSpeed = true;
        public float zoomLimiter = 0;

        private Character PosL => GetCharacterLeft();
        private Character PosR => GetCharacterRight();
        private Character PosHighest => GetCharacterHighest();
        private Character PosLowest => GetCharacterLowest();

        private delegate bool CharacterFilter(Character previous, Character current);

        public void Initialize(Stage stage)
        {
            camera = GetComponent<Camera>();
            camera.backgroundColor = Color.black;
            CameraBounds = new Bound(0f, 0f, 0f, 0f);
            Location = new Vector2(0, 0);

            StateZoom = new StateZoom();
            StateZoom.Reset();
        }


        const float normalSpeedMoveCamera = 8;
        const float maxSpeedMoveCamera = 20;

        const float fps = 0.0166666666666667f;
        float velLeft => PosL.CurrentVelocity.x;
        float velRight => PosR.CurrentVelocity.x;

        public float ScaleCamera { get; private set; }

        //Last Data before Start StateZoom
        float SaveLastZoomLimiter = -2.21f;
        public void SetStateZoom(float scale, Vector2 pos, int time, float lag)
        {
           // if (StateZoom.Time == 0)
           //     SaveLastZoomLimiter = zoomLimiter;
            
            StateZoom.SetStateZoom(scale, pos, time, lag);
        }

        public void UpdateFE()
        {
            
            float horizontal = 0, heightA = 0;
            if (StateZoom.Time > 0)
            {
                zoomLimiter = SaveLastZoomLimiter / StateZoom.Scale;
                horizontal = StateZoom.Pos.x;
                //horizontal = Misc.Center(PosL.GetLeftEdgePosition(true), PosR.GetRightEdgePosition(true));
                heightA = (StateZoom.Pos.y + Engine.stageScreen.Stage.FloorTension) * Engine.stageScreen.Stage.VerticalFollow;
                
                CameraBounds.down = Engine.stageScreen.Stage.boundLow - halfY() - (zoomLimiter / 12f);

                StateZoom.Time--;
            }
            else
            {
                UpdateFEScreen();
                UpdateCameraBound();
                Zoom();

                //if (PosHighest == null)
                //    GetCharacterHighest();
                heightA = (PosHighest.CurrentLocation.y + Engine.stageScreen.Stage.FloorTension) * Engine.stageScreen.Stage.VerticalFollow;
                horizontal = Misc.Center(PosL.GetLeftEdgePosition(true), PosR.GetRightEdgePosition(true));

            }
            ScaleCamera = zoomLimiter / Engine.stageScreen.Stage.minZoom;

            horizontal = Mathf.Lerp(camera.transform.position.x, horizontal, fps * ((isNormalSpeed ? normalSpeedMoveCamera : maxSpeedMoveCamera) + velLeft + velRight));

            Vector2 p = new Vector2(horizontal, heightA /*/ 1.5f*/);
            Location = CameraBounds.bound(p);
            Vector3 cameraLocation = new Vector3(Location.x, -Location.y, zoomLimiter);

            if (PosL.CameraFollowX && PosR.CameraFollowX && PosL.CameraFollowY && PosR.CameraFollowY)
            {
                camera.transform.position = cameraLocation;
                lastPosCamFollow = cameraLocation;
            }
            else if ((PosL.CameraFollowX && PosR.CameraFollowX) &&
                (PosL.CameraFollowY == false || PosR.CameraFollowY == false))
            {
                camera.transform.position = new Vector3(cameraLocation.x, camera.transform.position.y, cameraLocation.z);
                lastPosCamFollow = camera.transform.position;
            }

            else if ((PosL.CameraFollowX == false || PosR.CameraFollowX == false) &&
                (PosL.CameraFollowY && PosR.CameraFollowY))
            {
                camera.transform.position = new Vector3(camera.transform.position.x, cameraLocation.y, cameraLocation.z);
                lastPosCamFollow = camera.transform.position;
            }
            else
                camera.transform.position = new Vector3(lastPosCamFollow.x, lastPosCamFollow.y, cameraLocation.z);
        }

        void UpdateCameraBound()
        {
            CameraBounds.left = Engine.stageScreen.Stage.boundLeft + halfX();
            CameraBounds.right = Engine.stageScreen.Stage.boundRight - halfX();
            CameraBounds.up = Engine.stageScreen.Stage.boundHigh + halfY();
            CameraBounds.down = Engine.stageScreen.Stage.boundLow - halfY();
        }
        void Zoom()
        {
            float posLeft = PosL.GetLeftEdgePosition(true);
            float posRight = PosR.GetRightEdgePosition(true);

            if (posLeft - (23 * Constant.Scale) < ScreenBounds().xMin &&
                posLeft + (23 * Constant.Scale) > ScreenBounds().xMin &&
                posRight - (23 * Constant.Scale) < ScreenBounds().xMax &&
                posRight + (23 * Constant.Scale) > ScreenBounds().xMax)
            {
                if (velLeft == 0 && velRight == 0)
                    return;

                isNormalSpeed = false;
                if (posLeft - (5 * Constant.Scale) < Engine.stageScreen.Stage.boundLeft &&
                    posLeft + (5 * Constant.Scale) > Engine.stageScreen.Stage.boundLeft &&
                    velLeft != 0 && velRight == 0)
                    return;
                if (posRight - (5 * Constant.Scale) < Engine.stageScreen.Stage.boundRight &&
                    posRight + (5 * Constant.Scale) > Engine.stageScreen.Stage.boundRight &&
                    velRight != 0 && velLeft == 0)
                    return;

                if (zoomLimiter >= Engine.stageScreen.Stage.maxZoom)
                {
                    zoomLimiter -= fps * ((velLeft + velRight + 50) * Constant.Scale);
                    //Engine.stageScreen.stage.zoomLimiter = Mathf.Clamp(Engine.stageScreen.stage.zoomLimiter, Engine.Stage.maxZoom, Engine.Stage.minZoom);
                }
            }
            else
            {
                isNormalSpeed = true;
                if (zoomLimiter <= Engine.stageScreen.Stage.minZoom)
                {
                    zoomLimiter += fps * (50 * Constant.Scale);
                }
            }
            zoomLimiter = Mathf.Clamp(zoomLimiter, Engine.stageScreen.Stage.maxZoom, Engine.stageScreen.Stage.minZoom);
        }

        public void ResetFE()
        {
            Location = new Vector2(0, 0);
            ResetScreenCamera(Engine.stageScreen.Stage.StartPositionCamera);
        }

        private Character GetCharacter(CharacterFilter filter)
        {
            if (filter == null) throw new ArgumentNullException(nameof(filter));

            Character found = null;
            foreach (var entity in Engine.Entities)
            {
                var character = entity as Character;
                if (character == null || Misc.HelperCheck(character))
                    continue;

                if (filter(found, character))
                    found = character;
            }

            return found;
        }





        void UpdateFEScreen()
        {
            var C = CornerCamera();
            transLeft.localPosition = new Vector3(C.xMin, 0, -transform.position.z);
            transUp.localPosition = new Vector3(0, C.yMax, -transform.position.z);
            transRight.localPosition = new Vector3(C.xMax, 0, -transform.position.z);
            transDown.localPosition = new Vector3(0, C.yMin, -transform.position.z);
            transCenter.localPosition = new Vector3((C.xMin + C.xMax) / 2, (C.yMin + C.yMax) / 2, -transform.position.z);
            transCenter.localPosition = new Vector3((C.xMin + C.xMax) / 2, (C.yMin + C.yMax) / 2, -transform.position.z);

            Corners.yMax = transUp.position.y;
            Corners.yMin = transDown.position.y;
            Corners.xMin = transLeft.position.x;
            Corners.xMax = transRight.position.x;
        }

        public Rect CornerCamera()
        {
            Rect rect = new Rect();
            Vector3[] frustumCorners = new Vector3[4];
            camera.CalculateFrustumCorners(new Rect(0, 0, 1, 1), -transform.position.z, Camera.MonoOrStereoscopicEye.Mono, frustumCorners);
            rect.xMin = camera.transform.TransformVector(frustumCorners[0]).x;
            rect.yMax = camera.transform.TransformVector(frustumCorners[1]).y;
            rect.xMax = camera.transform.TransformVector(frustumCorners[2]).x;
            rect.yMin = camera.transform.TransformVector(frustumCorners[3]).y;

            //for (int i = 0; i < 4; i++)
            //{
            //    var worldSpaceCorner = camera.transform.TransformVector(frustumCorners[i]);
            //    Vector3 v = camera.transform.position;
            //    UnityEngine.Debug.DrawRay(new Vector3(v.x, v.y, v.z), worldSpaceCorner, Color.blue);
            //}

            return rect;
        }


        /*
        void UpdateFEScreen()
        {
            var C = CornerCamera();
            transLeft.position = new Vector3(C.xMin, C.center.y, 0);
            transUp.position = new Vector3(C.center.x, C.yMax, 0);
            transRight.position = new Vector3(C.xMax, C.center.y, 0);
            transDown.position = new Vector3(C.center.x, C.yMin, 0);

            Corners.yMax = transUp.position.y;
            Corners.yMin = transDown.position.y;
            Corners.xMin = transLeft.position.x;
            Corners.xMax = transRight.position.x;
        }

        public Rect CornerCamera()
        {
            Rect rect = new Rect();
            Vector3[] frustumCorners = new Vector3[4];
            camera.CalculateFrustumCorners(new Rect(0, 0, 1, 1), -transform.position.z, Camera.MonoOrStereoscopicEye.Mono, frustumCorners);
            //rect.xMin = camera.transform.TransformVector(frustumCorners[0]).x;
            //rect.yMax = camera.transform.TransformVector(frustumCorners[1]).y;
            //rect.xMax = camera.transform.TransformVector(frustumCorners[2]).x;
            //rect.yMin = camera.transform.TransformVector(frustumCorners[3]).y;

            rect.xMin = camera.transform.TransformPoint(frustumCorners[0]).x;
            rect.yMax = camera.transform.TransformPoint(frustumCorners[1]).y;
            rect.xMax = camera.transform.TransformPoint(frustumCorners[2]).x;
            rect.yMin = camera.transform.TransformPoint(frustumCorners[3]).y;

            //for (int i = 0; i < 4; i++)
            //{
            //    var worldSpaceCorner = camera.transform.TransformVector(frustumCorners[i]);
            //    Vector3 v = camera.transform.position;
            //    UnityEngine.Debug.DrawRay(new Vector3(v.x, v.y, v.z), worldSpaceCorner, Color.blue);
            //}

            return rect;
        }
        */

        public void ResetScreenCamera(Vector3 StartPositionCamera)
        {
            camera.transform.position = StartPositionCamera;
            UpdateFEScreen();
        }

        public Rect ConverterBound()
        {
            //Rect rect = new Rect();
            //rect.xMax = this.right;
            //rect.xMin = this.left;
            //rect.yMax = this.up;
            //rect.yMin = this.down;
            return Corners;
        }

        public Rect ScreenBounds()
        {
            Rect rect = new Rect();
            rect.xMax = Corners.xMax;
            rect.xMin = Corners.xMin;
            rect.yMax = Corners.yMin;
            rect.yMin = Location.y;// sc.transDown.position.y;
            return rect;
        }

        float halfX()
        {
            return ConverterBound().width / 2;
        }

        float halfY()
        {
            return ConverterBound().height / 2;
        }


        private Character GetCharacterLeft()
        {
            Character found = null;
            foreach (var entity in Engine.Entities)
            {
                var character = entity as Character;
                if (character == null || Misc.HelperCheck(character))
                    continue;

                if (found == null)
                    found = character;

                if (found != null && character != found)
                {
                    if (((character.CurrentFacing == Facing.Left && found.CurrentFacing == Facing.Left) ||
                    (character.CurrentFacing == Facing.Right && found.CurrentFacing == Facing.Right)) &&
                    (character.GetLeftEdgePosition(true) > found.GetRightEdgePosition(true)))
                    {
                        found = character;
                    }
                    else if (character.GetLeftEdgePosition(true) < found.GetLeftEdgePosition(true))
                    {
                        found = character;
                    }
                }
            }
            return found;
        }

        private Character GetCharacterRight()
        {
            Character found = null;
            foreach (var entity in Engine.Entities)
            {
                var character = entity as Character;
                if (character == null || Misc.HelperCheck(character))
                    continue;

                if (found == null)
                    found = character;

                if (found != null && character != found)
                {
                    if (((character.CurrentFacing == Facing.Left && found.CurrentFacing == Facing.Left) ||
                    (character.CurrentFacing == Facing.Right && found.CurrentFacing == Facing.Right)) &&
                    (character.GetLeftEdgePosition(true) < found.GetRightEdgePosition(true)))
                    {
                        found = character;
                    }
                    else if (character.GetRightEdgePosition(true) > found.GetRightEdgePosition(true))
                    {
                        found = character;
                    }
                }
            }
            return found;
        }


        private Character GetCharacterHighest()
        {
            Character found = null;
            foreach (var entity in Engine.Entities)
            {
                var character = entity as Character;
                if (character == null || Misc.HelperCheck(character))
                    continue;

                if (/*character.CameraFollowY && */(found == null || character.CurrentLocation.y < found.CurrentLocation.y))
                    found = character;
            }
            return found;
        }

        private Character GetCharacterLowest()
        {
            Character found = null;
            foreach (var entity in Engine.Entities)
            {
                var character = entity as Character;
                if (character == null || Misc.HelperCheck(character))
                    continue;

                if (character.CameraFollowY && (found == null || character.CurrentLocation.y > found.CurrentLocation.y))
                    found = character;
            }
            return found;
        }

    }
}