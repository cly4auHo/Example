using UnityEngine;
using UnityEngine.AddressableAssets;

[CreateAssetMenu(menuName = "ContentConfig")]
public class ContentCollection : ScriptableObject
{
    [field: SerializeField] public AssetReference[] Sprites { get; private set; }
}
