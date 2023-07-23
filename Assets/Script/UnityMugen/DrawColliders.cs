using System;
using System.Collections.Generic;
using UnityEngine;
using UnityMugen;
using UnityMugen.Animations;
using UnityMugen.Combat;


namespace UnityMugen
{

    public class DrawColliders : MonoBehaviour
    {

        List<GameObject> m_colliders = new List<GameObject>();
        Material m_material;
        Entity m_entity;
        bool m_isPlayer;

        float mult = 100;

        [NonSerialized] public Color normal, attack, hitable;

        private void Awake()
        {
            m_entity = gameObject.GetComponent<Entity>();
            m_material = new Material(Shader.Find("Sprites/Default"));

            if (m_entity is Player)
                m_isPlayer = true;
        }

        public void UpdateFE(Facing currentFacing)
        {
            if (m_entity.Engine.TickCount % 2 == 0) // Usar isso somente para economissar processo e memoria no Mobile.
                UpdateCollider(currentFacing);
        }

        public void RemoveBoxColliders()
        {
            if (m_colliders.Count > 0)
            {
                for (int i = 0; i < m_colliders.Count; i++)
                {
                    Destroy(m_colliders[i]);
                }
                m_colliders.Clear();
            }
        }

        void UpdateCollider(Facing currentFacing)
        {
            RemoveBoxColliders();

            TypeDrawCollider typeDraw = LauncherEngine.Inst.trainnerSettings.typeDrawCollider;
            if (m_entity != null && m_entity.AnimationManager != null && m_entity.AnimationManager.CurrentElement != null)
            {
                GameObject draws = new GameObject("DrawColliders");
                draws.hideFlags = HideFlags.HideAndDontSave;
                draws.transform.SetParent(m_entity.gameObject.transform);

                AnimationElement currentElement = m_entity.AnimationManager.CurrentElement;
                foreach (Rect coll in currentElement.ClsnsTipe2Normal)
                {
                    Rect newRec = new Rect(coll.x, coll.y, coll.width * mult, coll.height * mult);

                    if (m_isPlayer && ((Player)m_entity).MoveType == MoveType.BeingHit)
                    {
                        if (typeDraw == TypeDrawCollider.Paint || typeDraw == TypeDrawCollider.Both)
                        {
                            GameObject drawBeingHit = DrawCollider(newRec, hitable, currentFacing);
                            drawBeingHit.transform.SetParent(draws.transform);
                        }
                        if (typeDraw == TypeDrawCollider.Frame || typeDraw == TypeDrawCollider.Both)
                        {
                            GameObject drawBeingHit = DrawColliderBorder(newRec, hitable, currentFacing, ClsnType.Type2Normal);
                            drawBeingHit.transform.SetParent(draws.transform);
                        }
                    }
                    else
                    {
                        if (typeDraw == TypeDrawCollider.Paint || typeDraw == TypeDrawCollider.Both)
                        {
                            GameObject drawNormal = DrawCollider(newRec, normal, currentFacing);
                            drawNormal.transform.SetParent(draws.transform);
                        }
                        if (typeDraw == TypeDrawCollider.Frame || typeDraw == TypeDrawCollider.Both)
                        {
                            GameObject drawNormal = DrawColliderBorder(newRec, normal, currentFacing, ClsnType.Type2Normal);
                            drawNormal.transform.SetParent(draws.transform);
                        }
                    }
                }
                foreach (Rect coll in currentElement.ClsnsTipe1Attack)
                {
                    Rect newRec = new Rect(coll.x, coll.y, coll.width * mult, coll.height * mult);
                    if (typeDraw == TypeDrawCollider.Paint || typeDraw == TypeDrawCollider.Both)
                    {
                        GameObject drawAttack = DrawCollider(newRec, attack, currentFacing);
                        drawAttack.transform.SetParent(draws.transform);
                    }
                    if (typeDraw == TypeDrawCollider.Frame || typeDraw == TypeDrawCollider.Both)
                    {
                        GameObject drawAttack = DrawColliderBorder(newRec, attack, currentFacing, ClsnType.Type1Attack);
                        drawAttack.transform.SetParent(draws.transform);
                    }
                }

                m_colliders.Add(draws);
            }
        }

        GameObject DrawColliderBorder(Rect rect, Color color, Facing CurrentFacing, ClsnType clsnType)
        {
            GameObject collider = new GameObject();
            collider.name = "Collider";
            Vector2 pos = m_entity.CurrentLocationYTransform();
            float posX = CurrentFacing == Facing.Right ? (rect.xMin + pos.x) : -(rect.xMin - pos.x);
            float posY = rect.yMin + pos.y;
            collider.transform.position = new Vector3(posX, posY);
            collider.transform.localScale = new Vector3(1, 1, 1);
            SpriteRenderer spriteRenderer = collider.AddComponent<SpriteRenderer>();
            spriteRenderer.sprite = m_entity.Launcher.trainnerSettings.boxCollider;
            spriteRenderer.drawMode = SpriteDrawMode.Sliced;
            spriteRenderer.size = new Vector2(rect.width * 0.01f, rect.height * 0.01f);
            spriteRenderer.color = new Color(color.r, color.g, color.b, 1);
            spriteRenderer.sortingOrder = 50 + (clsnType == ClsnType.Type1Attack ? 1 : 0);
            spriteRenderer.sortingLayerName = "Entity";

            return collider;
        }


        GameObject DrawColliderBorderBotton(Rect rect, Color color, Facing CurrentFacing)
        {
            GameObject collider = new GameObject();
            collider.name = "Collider";
            Vector2 pos = m_entity.CurrentLocationYTransform();
            float posX = CurrentFacing == Facing.Right ? (rect.xMin + pos.x) : -(rect.xMin - pos.x);
            float posY = rect.yMin + pos.y;
            collider.transform.position = new Vector3(posX, posY - ((rect.height / 2) * 0.01f));
            collider.transform.localScale = new Vector3(rect.width, 1, 1);
            SpriteRenderer spriteRenderer = collider.AddComponent<SpriteRenderer>();
            spriteRenderer.material = m_material;
            spriteRenderer.material.color = color;
            spriteRenderer.sortingOrder = 50;
            spriteRenderer.sortingLayerName = "Entity";
            spriteRenderer.sprite = Sprite;
            return collider;
        }



        GameObject DrawCollider(Rect rect, Color color, Facing CurrentFacing)
        {
            GameObject collider = new GameObject();
            collider.name = "Collider";
            Vector2 pos = m_entity.CurrentLocationYTransform();// gameObject.transform.position;
            float posX = CurrentFacing == Facing.Right ? (rect.xMin + pos.x) : -(rect.xMin - pos.x);
            float posY = rect.yMin + pos.y;
            collider.transform.position = new Vector3(posX, posY);
            collider.transform.localScale = new Vector3(rect.width, rect.height, 1);
            SpriteRenderer spriteRenderer = collider.AddComponent<SpriteRenderer>();
            spriteRenderer.material = m_material;
            spriteRenderer.material.color = color;
            spriteRenderer.sortingOrder = 50; //5
            spriteRenderer.sortingLayerName = "Entity";
            spriteRenderer.sprite = Sprite;
            return collider;
        }

        static Sprite s_sprite;
        public static Sprite Sprite
        {
            get
            {
                if (s_sprite == null)
                {
                    Rect rect = new Rect(0.0f, 0.0f, WhiteTexture.width, WhiteTexture.height);
                    Vector2 pivot = new Vector2(0.5f, 0.5f);
                    s_sprite = Sprite.Create(WhiteTexture, rect, pivot, 100.0f);
                }
                return s_sprite;
            }
        }

        static Sprite s_spriteDimensionFront;
        public static Sprite SpriteDimensionFront
        {
            get
            {
                if (s_spriteDimensionFront == null)
                {
                    Rect rect = new Rect(0.0f, 0.0f, WhiteTexture.width, WhiteTexture.height);
                    Vector2 pivot = new Vector2(0, 0.5f);
                    s_spriteDimensionFront = Sprite.Create(WhiteTexture, rect, pivot, 100.0f);
                }
                return s_spriteDimensionFront;
            }
        }

        static Sprite s_spriteDimensionBack;
        public static Sprite SpriteDimensionBack
        {
            get
            {
                if (s_spriteDimensionBack == null)
                {
                    Rect rect = new Rect(0.0f, 0.0f, WhiteTexture.width, WhiteTexture.height);
                    Vector2 pivot = new Vector2(1, 0.5f);
                    s_spriteDimensionBack = Sprite.Create(WhiteTexture, rect, pivot, 100.0f);
                }
                return s_spriteDimensionBack;
            }
        }

        static Texture2D s_whiteTexture;
        public static Texture2D WhiteTexture
        {
            get
            {
                if (s_whiteTexture == null)
                {
                    s_whiteTexture = new Texture2D(1, 1);
                    s_whiteTexture.SetPixel(0, 0, Color.white);
                    s_whiteTexture.Apply();
                }
                return s_whiteTexture;
            }
        }

    }
}