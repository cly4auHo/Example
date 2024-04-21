using System.Collections.Generic;
using UnityEngine;

namespace UI
{
    [CreateAssetMenu(menuName = "UIConfig")]
    public class UIConfiguration : ScriptableObject
    {
        [SerializeField] private List<WidgetAssetLink> _widgets;
        
        public WidgetAssetLink GetWidgetAssetLink(WidgetName name) => _widgets.Find(w => w.Name == name);
    }
}