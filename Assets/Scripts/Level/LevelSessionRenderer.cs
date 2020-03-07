using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using SpaceOrigin.ObjectPool;

namespace SpaceOrigin.MahjongConnect
{
    // reders level session on the screen
    public class LevelSessionRenderer : MonoBehaviour
    {
        public Sprite[] sprites;
        private Dictionary<SpriteTypes, Sprite> spriteMap; // cahe map for 0(1) retrivel

        void Awake()
        {
            spriteMap = new Dictionary<SpriteTypes, Sprite>();
            for (int i = 0; i < sprites.Length; i++)
            {
                string spriteName = sprites[i].name;
                SpriteTypes spriteType = (SpriteTypes)Enum.Parse(typeof(SpriteTypes), spriteName);
                if (!spriteMap.ContainsKey(spriteType))
                {
                    spriteMap.Add(spriteType, sprites[i]);
                }
                else
                {
                    Debug.Log("some files got similar name");
                }
            }
        }

        public void RenderSessionOnScreen(LevelSession levelSession)
        {
            Cell[,] cells = levelSession.Grid.Cells;
            int rows = levelSession.Grid.Rows;
            int columns = levelSession.Grid.Columns;

            for (int i = 0; i < rows; i++)
            {
                for (int j = 0; j < columns; j++)
                {
                    SpriteObject spriteObj = ((SpriteObject)cells[i,j].CellObj);
                    if (spriteObj != null)
                    {
                        GameObject spriteViewGO = PoolManager.Instance.GetObjectFromPool("SpriteView");
                        SpriteRenderer rendere = spriteViewGO.GetComponent<SpriteRenderer>();
                        rendere.sprite = spriteMap[spriteObj.SpriteType];
                        SpriteView spriteView = spriteViewGO.GetComponent<SpriteView>();
                        spriteView.LinkView(spriteObj);
                        spriteViewGO.SetActive(true);
                    }
                }
            }
        }
    }
}
