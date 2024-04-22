using System.Collections.Generic;
using UnityEngine;
using Zenject;

namespace UI
{
    public class UIManager : MonoBehaviour, IUIManager
    {
        private Dictionary<int, Transform> _containers;
        
        [Inject] private UIConfiguration _uiConfiguration;
        [Inject] private UILayersConfiguration _layersConfiguration;
        [Inject] private DiContainer _container;
        
        public void Init()
        {
            _containers = new Dictionary<int, Transform>();

            foreach (var layer in _layersConfiguration.Layers)
            {
                var uiLayer = Instantiate(layer);
                _containers.Add(uiLayer.Layer, uiLayer.Container);
            }
        }

        public void Present(WidgetName widgetName)
        {
            var widget = _uiConfiguration.GetWidgetAssetLink(widgetName).Widget;
            var container = _containers[widget.gameObject.layer];
            var instance = _container.InstantiatePrefab(widget, container).GetComponent<BaseWidget>();
            
            instance.OnCreated();
            instance.Closed += OnWidgetClose;
        }
        
        public void PresentWithData<T>(WidgetName widgetName, T data)
        {
            var widget = _uiConfiguration.GetWidgetAssetLink(widgetName).Widget;
            var container = _containers[widget.gameObject.layer];
            var instance = _container.InstantiatePrefab(widget, container).GetComponent<BaseWidgetWithData<T>>();

            instance.Data = data;
            instance.OnCreated();
            instance.Closed += OnWidgetClose;
        }

        private void OnWidgetClose(BaseWidget widget)
        {
            widget.Closed -= OnWidgetClose;
            widget.OnClosed();
            Destroy(widget.gameObject);
        }
    }
}