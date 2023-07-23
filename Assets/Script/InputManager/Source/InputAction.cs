using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using UnityEngine;
using UnityMugen;

namespace UnityMugen.CustomInput
{
    [Serializable]
    public class InputAction
    {
        public const int MAX_BINDINGS = 16;

        [SerializeField]
        private string m_name;
        [SerializeField]
        private string m_description;
        [SerializeField]
        private List<InputBinding> m_bindings;

        public ReadOnlyCollection<InputBinding> Bindings
        {
            get { return m_bindings.AsReadOnly(); }
        }

        public string Name
        {
            get { return m_name; }
            set
            {
                m_name = value;
                if (Application.isPlaying)
                {
                    Debug.LogWarning("You should not change the name of an input action at runtime");
                }
            }
        }

        public string Description
        {
            get { return m_description; }
            set { m_description = value; }
        }

        public bool AnyInput
        {
            get
            {
                foreach (var binding in m_bindings)
                {
                    if (binding.AnyInput)
                        return true;
                }

                return false;
            }
        }

        public InputAction() :
            this("New Action")
        { }

        public InputAction(string name)
        {
            m_name = name;
            m_description = string.Empty;
            m_bindings = new List<InputBinding>();
        }

        public void Initialize()
        {
            foreach (var binding in m_bindings)
            {
                binding.Initialize();
            }
        }

        public void Update(float deltaTime)
        {
            foreach (var binding in m_bindings)
            {
                binding.Update(deltaTime);
            }
        }

        public float GetAxis(PlayerID playerID)
        {
            float? value = null;
            for (int i = 0; i < m_bindings.Count; i++)
            {
                value = m_bindings[i].GetAxis();
                if (value.HasValue)
                {
                    if (playerID == PlayerID.One)
                        InputCustom.LastBindingAcitivPlayerIDOne = i;
                    else if (playerID == PlayerID.Two)
                        InputCustom.LastBindingAcitivPlayerIDTwo = i;
                    else if (playerID == PlayerID.Three)
                        InputCustom.LastBindingAcitivPlayerIDThree = i;
                    else if (playerID == PlayerID.Four)
                        InputCustom.LastBindingAcitivPlayerIDFour = i;
                    break;
                }
            }

            return value ?? InputBinding.AXIS_NEUTRAL;
        }

        ///<summary>
        ///	Returns raw input with no sensitivity or smoothing applyed.
        /// </summary>
        public float GetAxisRaw(PlayerID playerID)
        {
            float? value = null;
            for (int i = 0; i < m_bindings.Count; i++)
            {
                value = m_bindings[i].GetAxisRaw();
                if (value.HasValue)
                {
                    if (playerID == PlayerID.One)
                        InputCustom.LastBindingAcitivPlayerIDOne = i;
                    else if (playerID == PlayerID.Two)
                        InputCustom.LastBindingAcitivPlayerIDTwo = i;
                    else if (playerID == PlayerID.Three)
                        InputCustom.LastBindingAcitivPlayerIDThree = i;
                    else if (playerID == PlayerID.Four)
                        InputCustom.LastBindingAcitivPlayerIDFour = i;
                    break;
                }
            }

            return value ?? InputBinding.AXIS_NEUTRAL;
        }

        public bool GetButton(PlayerID playerID)
        {
            bool? value = null;
            for (int i = 0; i < m_bindings.Count; i++)
            {
                value = m_bindings[i].GetButton();
                if (value.HasValue)
                {
                    if (playerID == PlayerID.One)
                        InputCustom.LastBindingAcitivPlayerIDOne = i;
                    else if (playerID == PlayerID.Two)
                        InputCustom.LastBindingAcitivPlayerIDTwo = i;
                    else if (playerID == PlayerID.Three)
                        InputCustom.LastBindingAcitivPlayerIDThree = i;
                    else if (playerID == PlayerID.Four)
                        InputCustom.LastBindingAcitivPlayerIDFour = i;
                    break;
                }
            }

            return value ?? false;
        }

        public bool GetButtonDown(PlayerID playerID)
        {
            bool? value = null;
            for (int i = 0; i < m_bindings.Count; i++)
            {
                value = m_bindings[i].GetButtonDown();
                if (value.HasValue)
                {
                    if (playerID == PlayerID.One)
                        InputCustom.LastBindingAcitivPlayerIDOne = i;
                    else if (playerID == PlayerID.Two)
                        InputCustom.LastBindingAcitivPlayerIDTwo = i;
                    else if (playerID == PlayerID.Three)
                        InputCustom.LastBindingAcitivPlayerIDThree = i;
                    else if (playerID == PlayerID.Four)
                        InputCustom.LastBindingAcitivPlayerIDFour = i;
                    break;
                }
            }

            return value ?? false;
        }

        public bool GetButtonUp(PlayerID playerID)
        {
            bool? value = null;
            for (int i = 0; i < m_bindings.Count; i++)
            {
                value = m_bindings[i].GetButtonUp();
                if (value.HasValue)
                {
                    if (playerID == PlayerID.One)
                        InputCustom.LastBindingAcitivPlayerIDOne = i;
                    else if (playerID == PlayerID.Two)
                        InputCustom.LastBindingAcitivPlayerIDTwo = i;
                    else if (playerID == PlayerID.Three)
                        InputCustom.LastBindingAcitivPlayerIDThree = i;
                    else if (playerID == PlayerID.Four)
                        InputCustom.LastBindingAcitivPlayerIDFour = i;
                    break;
                }
            }

            return value ?? false;
        }

        public InputBinding GetBinding(int index)
        {
            if (index >= 0 && index < m_bindings.Count)
                return m_bindings[index];

            return null;
        }

        public InputBinding CreateNewBinding()
        {
            if (m_bindings.Count < MAX_BINDINGS)
            {
                InputBinding binding = new InputBinding();
                m_bindings.Add(binding);

                return binding;
            }

            return null;
        }

        public InputBinding CreateNewBinding(InputBinding source)
        {
            if (m_bindings.Count < MAX_BINDINGS)
            {
                InputBinding binding = InputBinding.Duplicate(source);
                m_bindings.Add(binding);

                return binding;
            }

            return null;
        }

        public InputBinding InsertNewBinding(int index)
        {
            if (m_bindings.Count < MAX_BINDINGS)
            {
                InputBinding binding = new InputBinding();
                m_bindings.Insert(index, binding);

                return binding;
            }

            return null;
        }

        public InputBinding InsertNewBinding(int index, InputBinding source)
        {
            if (m_bindings.Count < MAX_BINDINGS)
            {
                InputBinding binding = InputBinding.Duplicate(source);
                m_bindings.Insert(index, binding);

                return binding;
            }

            return null;
        }

        public void DeleteBinding(int index)
        {
            if (index >= 0 && index < m_bindings.Count)
                m_bindings.RemoveAt(index);
        }

        public void SwapBindings(int fromIndex, int toIndex)
        {
            if (fromIndex >= 0 && fromIndex < m_bindings.Count && toIndex >= 0 && toIndex < m_bindings.Count)
            {
                var temp = m_bindings[toIndex];
                m_bindings[toIndex] = m_bindings[fromIndex];
                m_bindings[fromIndex] = temp;
            }
        }

        public void Copy(InputAction source)
        {
            m_name = source.m_name;
            m_description = source.m_description;

            m_bindings.Clear();
            foreach (var binding in source.m_bindings)
            {
                m_bindings.Add(InputBinding.Duplicate(binding));
            }
        }

        public void CopyBinding(InputAction source, int numberBinding)
        {
            m_name = source.m_name;
            m_description = source.m_description;

            //    m_bindings.Clear();
            //    m_bindings.Add(InputBinding.Duplicate(source.m_bindings[numberBinding]));
            m_bindings[numberBinding] = InputBinding.Duplicate(source.m_bindings[numberBinding]);
        }

        public void Reset()
        {
            foreach (var binding in m_bindings)
            {
                binding.Reset();
            }
        }

        public static InputAction Duplicate(InputAction source)
        {
            return Duplicate(source.m_name, source);
        }

        public static InputAction Duplicate(string name, InputAction source)
        {
            InputAction duplicate = new InputAction();
            duplicate.m_name = name;
            duplicate.m_description = source.m_description;
            duplicate.m_bindings = new List<InputBinding>();
            foreach (var binding in source.m_bindings)
            {
                duplicate.m_bindings.Add(InputBinding.Duplicate(binding));
            }

            return duplicate;
        }
    }
}