using UnityEngine;
using UnityEngine.EventSystems;

namespace UnityMugen.CustomInput
{
    public class DisableMouse : MonoBehaviour
    {

        private GameObject lastselect;

        void Awake()
        {
            Cursor.visible = false;
            Cursor.lockState = CursorLockMode.Locked;
            lastselect = new GameObject("Disable Mouse");
            lastselect.hideFlags = HideFlags.HideInInspector;
        }

        void Update()
        {
            if (EventSystem.current.currentSelectedGameObject == null)
            {
                EventSystem.current.SetSelectedGameObject(lastselect);
            }
            else
            {
                lastselect = EventSystem.current.currentSelectedGameObject;
            }
        }

    }
}