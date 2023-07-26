using UnityEngine;

namespace UnityMugen.CustomInput
{
    [CreateAssetMenu(fileName = "New Input Action Reference", menuName = "Unity Mugen/Input Manager/Input Action Reference")]
    public class ActionReference : ScriptableObject
    {
        [SerializeField]
        private string m_schemeName;
        [SerializeField]
        private string m_actionName;

        [System.NonSerialized]
        private InputAction m_cachedInputAction = null;

        public InputAction Get()
        {
            if (m_cachedInputAction == null && InputManager.Exists)
            {
                m_cachedInputAction = InputManager.GetAction(m_schemeName, m_actionName);
            }

            return m_cachedInputAction;
        }

        private void OnValidate()
        {
            if (InputManager.Exists)
            {
                m_cachedInputAction = InputManager.GetAction(m_schemeName, m_actionName);
            }
        }
    }
}