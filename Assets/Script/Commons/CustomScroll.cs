using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityMugen.CustomInput;

namespace UnityMugen.Interface
{

    [Serializable]
    public class ContentsData
    {
        public GameObject commands;
        public Sprite imageSample;

        [TextArea(3, 1000)]
        public string description;
    }

    public class CustomScroll : MonoBehaviour
    {
        public int totalItensShow;
        public RectTransform interno;
        public Color selected;
        public Color noSelected;
        public Scrollbar scrollbar;

        public Image imageSample;
        public Text description;

        public GameObject instanciableMove;

        public List<ContentsData> contents;

        [NonSerialized] public int currentIndex;
        int positionGrid = 1;
        float sizeHeightContent;
        float spacing;

        private void Start()
        {
            sizeHeightContent = contents[0].commands.GetComponent<RectTransform>().sizeDelta.y;
            interno.sizeDelta = new Vector2(interno.sizeDelta.x, contents.Count * sizeHeightContent);
            spacing = interno.GetComponent<VerticalLayoutGroup>().spacing;
            // UpdatePositionContents();

            imageSample.sprite = contents[0].imageSample;
            description.text = contents[0].description;
        }

        public void ResetCS()
        {
            currentIndex = 0;
            positionGrid = 1;
            scrollbar.value = 0;
            interno.anchoredPosition = new Vector2(interno.anchoredPosition.x, 0);
            imageSample.sprite = contents[0].imageSample;
            description.text = contents[0].description;

            UpdateColorSelected();
        }

        void Update()
        {

            if (InputCustom.PressUpPlayerIDOne())
            {
                if (currentIndex != 0)
                {
                    currentIndex--;
                    if (positionGrid == 1)
                        interno.anchoredPosition = new Vector2(interno.anchoredPosition.x, interno.anchoredPosition.y - sizeHeightContent - spacing);

                    if (positionGrid > 1)
                        positionGrid--;

                    imageSample.sprite = contents[currentIndex].imageSample;
                    description.text = contents[currentIndex].description;
                    scrollbar.value = (((float)currentIndex / ((float)contents.Count - 1f)) * 100f) * 0.01f;
                }
            }
            else if (InputCustom.PressDownPlayerIDOne())
            {
                if (currentIndex != contents.Count - 1)
                {
                    currentIndex++;
                    if (positionGrid == totalItensShow)
                        interno.anchoredPosition = new Vector2(interno.anchoredPosition.x, interno.anchoredPosition.y + sizeHeightContent + spacing);

                    if (positionGrid < totalItensShow)
                        positionGrid++;

                    imageSample.sprite = contents[currentIndex].imageSample;
                    description.text = contents[currentIndex].description;
                    scrollbar.value = (((float)currentIndex / ((float)contents.Count - 1f)) * 100f) * 0.01f;
                }
            }

            UpdateColorSelected();
        }


        public void UpdateColorSelected()
        {
            for (int i = 0; i < contents.Count; i++)
            {
                if (currentIndex == i)
                    contents[i].commands.GetComponent<Image>().color = selected;
                else
                    contents[i].commands.GetComponent<Image>().color = noSelected;
            }
        }

        public void UpdatePositionContents()
        {
            for (int i = 0; i < contents.Count; i++)
            {
                float position = -sizeHeightContent * i;
                float x = contents[i].commands.GetComponent<RectTransform>().anchoredPosition.x;
                contents[i].commands.GetComponent<RectTransform>().anchoredPosition = new Vector2(x, position);
            }
        }
    }
}