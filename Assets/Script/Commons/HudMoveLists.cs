using System;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.UI;
using UnityMugen.CustomInput;

namespace UnityMugen.Interface
{

    public class HudMoveLists : MonoBehaviour
    {

        LauncherEngine Launcher => LauncherEngine.Inst;

        public GameObject instance;

        public GameObject notification;
        List<MenuCustomScrolls> moveLists = new List<MenuCustomScrolls>();

        [NonSerialized] public List<string> namesChar = new List<string>();
        int currentMoveListActive;

        private void Awake()
        {
            ImportCommands();

            foreach (PlayerProfileManager ppm in Launcher.profileLoader.profiles)
            {
                if (ppm.moveList == null) continue;

                var menuCustomScroll = Instantiate<GameObject>(instance).GetComponent<MenuCustomScrolls>();
                menuCustomScroll.name = "Canvas" + ppm.charName;
                menuCustomScroll.nameChar = ppm.charName;
                menuCustomScroll.nameCharText.text = ppm.moveList.nameChar;
                menuCustomScroll.iconChar.sprite = TextureToSprite(ppm.moveList.iconChar);
                menuCustomScroll.gameObject.transform.SetParent(instance.transform.parent, false);
                moveLists.Add(menuCustomScroll);

                CustomScroll customScroll = menuCustomScroll.contentEspecialMoves.GetComponent<CustomScroll>();
                CustomScroll customScrollSuper = menuCustomScroll.contentSuperMoves.GetComponent<CustomScroll>();
                for (int i = 0; i < ppm.moveList.specialMoves.Count; i++)
                {
                    var instanciableMove = Instantiate<GameObject>(customScroll.instanciableMove).GetComponent<InstaciableMove>();
                    instanciableMove.transform.SetParent(customScroll.interno.transform, false);
                    instanciableMove.gameObject.SetActive(true);
                    instanciableMove.nameMove.text = "	" + ppm.moveList.specialMoves[i].nameMove;

                    int totalParent = 0;
                    TotalParent(ppm.moveList.specialMoves, i, ref totalParent);

                    Vector2 size = instanciableMove.GetComponent<RectTransform>().sizeDelta;
                    instanciableMove.GetComponent<RectTransform>().sizeDelta = new Vector2(size.x - (totalParent * 60), size.y);
                    instanciableMove.parent.SetActive(totalParent > 0);

                    foreach (Texture2D tex in ppm.moveList.specialMoves[i].playerButtons)
                        CreateCommands(instanciableMove.moveLine1.transform, tex, instanciableMove.moveLine1.sizeDelta.y);
                    
                    ContentsData data = new ContentsData();
                    data.commands = instanciableMove.gameObject;
                    data.imageSample = TextureToSprite(ppm.moveList.specialMoves[i].imageMove);
                    data.description = ppm.moveList.specialMoves[i].description;

                    customScroll.contents.Add(data);
                }

                for (int i = 0; i < ppm.moveList.superMoves.Count; i++)
                {
                    var instanciableMove = Instantiate<GameObject>(customScrollSuper.instanciableMove).GetComponent<InstaciableMove>();
                    instanciableMove.transform.SetParent(customScrollSuper.interno.transform, false);
                    instanciableMove.gameObject.SetActive(true);
                    instanciableMove.nameMove.text = ppm.moveList.superMoves[i].nameMove;

                    int totalParent = 0;
                    TotalParent(ppm.moveList.superMoves, i, ref totalParent);

                    Vector2 size = instanciableMove.GetComponent<RectTransform>().sizeDelta;
                    instanciableMove.GetComponent<RectTransform>().sizeDelta = new Vector2(size.x - (totalParent * 60), size.y);
                    instanciableMove.parent.SetActive(totalParent > 0);

                    foreach (Texture2D tex in ppm.moveList.superMoves[i].playerButtons)
                        CreateCommands(instanciableMove.moveLine1.transform, tex, instanciableMove.moveLine1.sizeDelta.y);
                    
                    ContentsData data = new ContentsData();
                    data.commands = instanciableMove.gameObject;
                    data.imageSample = TextureToSprite(ppm.moveList.superMoves[i].imageMove);
                    data.description = ppm.moveList.superMoves[i].description;

                    customScrollSuper.contents.Add(data);
                }
            }
        }

        int TotalParent(List<MoveShow> ms, int index, ref int totalParent)
        {
            if (ms.ElementAt(index).parent != 0)
            {
                totalParent++;
                TotalParent(ms, ms.ElementAt(index).parent - 1, ref totalParent);
            }
            return totalParent;
        }

        Dictionary<string, Sprite> values = new Dictionary<string, Sprite>();
        Sprite missing;
        void ImportCommands()
        {
            var commonsInput = Resources.LoadAll<Texture2D>("MoveList/CommonsInput").ToList();
            var othersActions = Resources.LoadAll<Texture2D>("MoveList/OthersActions").ToList();
            var miss = Resources.Load<Texture2D>("MoveList/missing");
            Rect rect2 = new Rect(0, 0, miss.width, miss.height);
            this.missing = Sprite.Create(miss, rect2, new Vector2(0, 0), 100);
            this.missing.name = miss.name;

            commonsInput.AddRange(othersActions);

            foreach (Texture2D tex in commonsInput)
            {
                Rect rect = new Rect(0, 0, tex.width, tex.height);
                var spri = Sprite.Create(tex, rect, new Vector2(0, 0), 100);
                spri.name = tex.name;
                values.Add(tex.name, spri);
            }
        }

        void CreateCommands(Transform parent ,Texture2D tex, float heightMoveLine)
        {
            GameObject go = new GameObject();
            var rect = go.AddComponent<RectTransform>();
            var im = go.AddComponent<Image>();
            im.preserveAspect = true;
            im.raycastTarget = false;

            if (tex == null)
            {
                Debug.LogError("A texture is missing for the Command List");
                im.sprite = missing;
                go.transform.SetParent(parent, false);
                go.name = missing.name;

                float proportionMiss = missing.texture.width * heightMoveLine / missing.texture.height;
                rect.sizeDelta = new Vector2(proportionMiss, heightMoveLine);
                return;
            }
            
            go.name = tex.name;
            float proportion = tex.width * heightMoveLine / tex.height;
            rect.sizeDelta = new Vector2(proportion, heightMoveLine);
            
            values.TryGetValue(tex.name, out Sprite sprite);
            im.sprite = sprite;
            go.transform.SetParent(parent, false);
        }

        Sprite TextureToSprite(Texture2D tex)
        {
            if (tex == null)
            {
                Debug.LogWarning("A list of commands has no reference image.");
                return null;
            }
            Rect rect = new Rect(0, 0, tex.width, tex.height);
            var spri = Sprite.Create(tex, rect, new Vector2(0, 0), 100);
            spri.name = tex.name;
            return spri;
        }

        public void Active()
        {
            enabled = true;
            foreach (MenuCustomScrolls item in moveLists)
            {
                if (item.nameChar == namesChar[currentMoveListActive])
                    item.gameObject.SetActive(true);
                else
                    item.gameObject.SetActive(false);
            }

            notification.SetActive(true);
        }

        private void Disable()
        {
            enabled = false;
            foreach (MenuCustomScrolls item in moveLists)
            {
                item.gameObject.SetActive(false);
            }

            notification.SetActive(false);
        }

        // Update is called once per frame
        void Update()
        {
            if (UnityEngine.Input.GetKeyDown(KeyCode.T))
            {
                if (currentMoveListActive == 0)
                    currentMoveListActive = 1;
                else
                    currentMoveListActive = 0;

                if (namesChar.Count > 0)
                {
                    foreach (MenuCustomScrolls item in moveLists)
                    {
                        if (item.nameChar == namesChar[currentMoveListActive])
                        {
                            item.Start();
                            item.gameObject.SetActive(true);
                        }
                        else
                            item.gameObject.SetActive(false);
                    }
                }
            }

            if (InputManager.GetButtonDown("Y") || InputManager.GetButtonDown("Start"))
            {
                Launcher.mugen.Engine.stageScreen.PauseFight.Back();
                Launcher.mugen.Engine.stageScreen.PauseFight.currentPanelPauseFight = PanelPauseFight.Pause;
                Disable();
            }
        }
    }
}