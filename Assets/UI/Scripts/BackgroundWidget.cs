using Background;
using UnityEngine;
using UnityEngine.UI;
using Zenject;

namespace UI
{
    public class BackgroundWidget : BaseWidget
    {
        [SerializeField] private BackgroundContainer _backgroundContainerPrefab;
        [SerializeField] private RectTransform _container;
        [SerializeField] private HorizontalLayoutGroup _layoutGroup;
        [SerializeField] private float _spacing;

        [Inject] private IBackgroundSystem _backgroundSystem;
        
        private BackgroundContainer[] _backgroundContainers;

        public override async void OnCreated()
        {
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

        public override void OnClosed()
        {
            foreach (var background in _backgroundContainers)
                background.Pressed -= ChooseBackgroundClickHandler;
        }

        private void ChooseBackgroundClickHandler(int index)
        {
            _backgroundSystem.SetNewBackground(index);
            Closed?.Invoke(this);
        }
    }
}