using UnityEngine;

namespace UnityMugen.CustomInput
{
    [CreateAssetMenu(fileName = "New Input Binding Reference", menuName = "Unity Mugen/Input Manager/Input Binding Reference")]
    public class BindingReference : ScriptableObject
    {
        [SerializeField]
        private string m_schemeName;
        [SerializeField]
        private string m_actionName;
        [SerializeField]
        private int m_bindingIndex;

        [System.NonSerialized]
        private InputBinding m_cachedInputBinding = null;

        public InputBinding Get()
        {
            if (m_cachedInputBinding == null && InputManager.Exists)
            {
                var action = InputManager.GetAction(m_schemeName, m_actionName);
                if (action != null)
                {
                    m_cachedInputBinding = action.GetBinding(m_bindingIndex);
                }
            }

            return m_cachedInputBinding;
        }

        private void OnValidate()
        {
            if (InputManager.Exists)
            {
                var action = InputManager.GetAction(m_schemeName, m_actionName);
                if (action != null)
                {
                    m_cachedInputBinding = action.GetBinding(m_bindingIndex);
                }
                else
                {
                    m_cachedInputBinding = null;
                }
            }
        }
    }
}