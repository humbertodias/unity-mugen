using UnityEngine;
using UnityEngine.UI;
using UnityMugen.CustomInput;

namespace UnityMugen.Interface
{

    public class MenuCustomScrolls : MonoBehaviour
    {
        public string nameChar;
        public GameObject optionEspecialMoves;
        public GameObject optionSuperMoves;

        public GameObject contentEspecialMoves;
        public GameObject contentSuperMoves;

        public Scrollbar scrollbar;

        public Image iconChar;
        public Text nameCharText;

        public void Start()
        {
            optionEspecialMoves.GetComponent<Outline>().enabled = true;
            contentEspecialMoves.SetActive(true);

            optionSuperMoves.GetComponent<Outline>().enabled = false;
            contentSuperMoves.SetActive(false);

            scrollbar.value = 0;

            contentSuperMoves.GetComponent<CustomScroll>().ResetCS();
            contentEspecialMoves.GetComponent<CustomScroll>().ResetCS();
        }

        void Update()
        {
            if (InputCustom.PressLeftPlayerIDOne())
            {
                optionEspecialMoves.GetComponent<Outline>().enabled = true;
                contentEspecialMoves.SetActive(true);

                optionSuperMoves.GetComponent<Outline>().enabled = false;
                contentSuperMoves.SetActive(false);

                contentEspecialMoves.GetComponent<CustomScroll>().ResetCS();
            }
            else if (InputCustom.PressRightPlayerIDOne())
            {
                optionEspecialMoves.GetComponent<Outline>().enabled = false;
                contentEspecialMoves.SetActive(false);

                optionSuperMoves.GetComponent<Outline>().enabled = true;
                contentSuperMoves.SetActive(true);

                contentSuperMoves.GetComponent<CustomScroll>().ResetCS();
            }
        }
    }
}