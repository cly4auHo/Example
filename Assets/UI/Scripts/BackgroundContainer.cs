using System;
using UnityEngine;
using UnityEngine.UI;

namespace UI
{
    public class BackgroundContainer : MonoBehaviour
    {
        public Action<int> Pressed;

        [SerializeField] private RectTransform _rectTransform;
        [SerializeField] private Button _button;
        [SerializeField] private Image _image;

        private int _index;

        public float Width => _rectTransform.sizeDelta.x;

        public void Init(Sprite sprite, in int index)
        {
            _image.sprite = sprite;
            _index = index;
            _button.onClick.AddListener(ClickHandler);
        }

        private void ClickHandler()
        {
            _button.onClick.RemoveListener(ClickHandler);
            Pressed?.Invoke(_index);
        }
    }
}