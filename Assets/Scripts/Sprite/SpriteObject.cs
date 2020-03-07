using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace SpaceOrigin.MahjongConnect
{
    // spriteobject(model) links to spriteview(view)
    public class SpriteObject: CellObject
    {
        private bool _selected;
        private bool _hintEnabled;
        private Cell _parentCell;
        private SpriteTypes _spriteType;

        public delegate void SpriteObjectDelegate();
        public delegate void SpriteObjectSelectDelegate(Color color);
        public SpriteObjectSelectDelegate onSelectedObject;
        public SpriteObjectSelectDelegate onUnselectObject;
        public SpriteObjectSelectDelegate onDestroyObject;
        public SpriteObjectDelegate onEnableHint;
        public SpriteObjectDelegate onDesableHint;

        public Cell ParentCell { get => _parentCell; set => _parentCell = value; }
        public SpriteTypes SpriteType { get => _spriteType; set => _spriteType = value; }

        public SpriteObject(SpriteTypes spriteType)
        {
            _spriteType = spriteType;
        }

        public void SelectObject (Color selectedColor)
        {
            _selected = true;
            if (onSelectedObject != null) onSelectedObject.Invoke(selectedColor);
        }

        public void UnSelect(Color unselectedColor)
        {
            _selected = false;
            if (onUnselectObject != null) onSelectedObject.Invoke(unselectedColor);
        }

        public void EnableHint()
        {
            _hintEnabled = true;
            if (onEnableHint != null) onEnableHint.Invoke();
        }

        public void DesableHint()
        {
            _hintEnabled = false;
            if (onDesableHint != null) onDesableHint.Invoke();
        }

        public void Destroy(Color unselectedColor)
        {
            _parentCell.CellObj = null; // life of this object is determined by its presents on parent cell
            if (onDestroyObject != null) onDestroyObject.Invoke(unselectedColor);
        }
    }
}
