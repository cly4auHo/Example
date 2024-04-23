using System;
using DG.Tweening;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

namespace UI
{
    public class AnswerView : MonoBehaviour
    {
        public Action<int> Pressed;

        [SerializeField] private TextMeshProUGUI _text;
        [SerializeField] private Button _button;
        [SerializeField] private Image _image;

        private int _index;

        public void Init(in int index)
        {
            _index = index;
            _button.onClick.AddListener(ClickHandler);
        }

        public void Dispose() => _button.onClick.RemoveListener(ClickHandler);

        public void SetValue(in int value)
        {
            _button.interactable = true;
            _text.text = $"{value}";
            _image.color = Color.white;
        }

        public void Inactive()
        {
            SetUnInteractable();
            _text.text = string.Empty;
        }

        public void SetUnInteractable() => _button.interactable = false;

        public void HighLight(in float duration, bool isCorrect)
            => _image.DOColor(isCorrect ? Color.green : Color.red, duration);

        private void ClickHandler() => Pressed?.Invoke(_index);
    }
}