using System.Threading.Tasks;
using UnityEngine;
using UnityEngine.AddressableAssets;
using UnityEngine.ResourceManagement.AsyncOperations;
using Zenject;

public class ContentManager : MonoBehaviour, IContentManager
{
    [SerializeField] AssetReference[] sprites;
    
    AsyncOperationHandle<Sprite>[] _spriteHandles;
    
    public async Task<Sprite> GetSprite(int index)
    {
        _spriteHandles ??= new AsyncOperationHandle<Sprite>[sprites.Length];
        var handle = sprites[index].LoadAssetAsync<Sprite>();
        
        await handle.Task;
        
        _spriteHandles[index] = handle;
        
        return handle.Status == AsyncOperationStatus.Succeeded ? handle.Result : null; 
    }
    
    public async Task<Sprite[]> GetAllSprite(int indexException = -1)
    {
        var result = new Sprite[sprites.Length];
        _spriteHandles ??= new AsyncOperationHandle<Sprite>[sprites.Length];
        
        for (int i = 0; i < sprites.Length; i++)
        {
            if (i == indexException)
                continue;
            
            var handle = sprites[i].LoadAssetAsync<Sprite>();
            await handle.Task;
            
            _spriteHandles[i] = handle;
            result[i] = handle.Status == AsyncOperationStatus.Succeeded ? handle.Result : null;
        }

        return result;
    }
    
    public void Release(in int index) => Addressables.Release(_spriteHandles[index]);
}
