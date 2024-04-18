using UnityEngine;
using UnityEngine.UI;

public class BackgroundWidget : BaseWidget
{
    [SerializeField] BackgroundContainer _backgroundContainerPrefab;
    [SerializeField] RectTransform _container;
    [SerializeField] HorizontalLayoutGroup _layoutGroup;
    [SerializeField] float _spacing;
    
    IBackgroundSystem _backgroundSystem;

    BackgroundContainer[] _backgroundContainers;
    
    public async void Init(IBackgroundSystem backgroundSystem)
    {
        _backgroundSystem = backgroundSystem;

        var allBackgrounds = await _backgroundSystem.GetAll();

        _backgroundContainers = new BackgroundContainer[allBackgrounds.Length];
        _layoutGroup.spacing = _spacing;
        _container.sizeDelta = new Vector2((allBackgrounds.Length - 1) * (_spacing + _backgroundContainerPrefab.Width), _container.sizeDelta.y);
        
        for (int i = 0; i < allBackgrounds.Length; i++)
        {
            var background = Instantiate(_backgroundContainerPrefab, _container);

            _backgroundContainers[i] = background;
            
            background.Init(allBackgrounds[i], i);
            background.Pressed += ChooseBackgroundClickHandler;
        }
    }

    void ChooseBackgroundClickHandler(int index)
    {
        foreach (var background in _backgroundContainers)
            background.Pressed -= ChooseBackgroundClickHandler;

        _backgroundSystem.SetNewBackground(index);
        
        Closed?.Invoke(this);
        
        Destroy(gameObject); //lifecycle of widget must control UI system 
    }
}
