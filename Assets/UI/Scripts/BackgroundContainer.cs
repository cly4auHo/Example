using System;
using UnityEngine;
using UnityEngine.UI;

public class BackgroundContainer : MonoBehaviour
{
    public Action<int> Pressed;

    [SerializeField] RectTransform _rectTransform;
    [SerializeField] Button _button;
    [SerializeField] Image _image;
    
    int _index;

    public float Width => _rectTransform.sizeDelta.x;
    
    public void Init(Sprite sprite, in int index)
    {
        _image.sprite = sprite;
        _index = index;
        _button.onClick.AddListener(ClickHandler);
    }

    void ClickHandler()
    {
        _button.onClick.RemoveListener(ClickHandler);
        Pressed?.Invoke(_index);
    }
}
