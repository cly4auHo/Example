using UnityEngine;
using UnityEngine.AddressableAssets;

namespace Content
{
    [CreateAssetMenu(menuName = "ContentConfig")]
    public class ContentCollection : ScriptableObject
    {
        [field: SerializeField] public AssetReference[] Sprites { get; private set; }
    }
}