using UnityEngine;

namespace UI
{
    [CreateAssetMenu(menuName = "UILayerConfig")]
    public class UILayersConfiguration : ScriptableObject
    {
        [field:SerializeField] public UILayer[] Layers { get; private set; }
    }
}