using System.Threading.Tasks;
using UnityEngine;
using UnityEngine.AddressableAssets;
using UnityEngine.ResourceManagement.AsyncOperations;
using Zenject;

namespace Content
{
    public class ContentManager : IContentManager
    {
        [Inject] private ContentCollection _content;
        
        private AsyncOperationHandle<Sprite>[] _spriteHandles;

        public async Task<Sprite> GetSprite(int index)
        {
            _spriteHandles ??= new AsyncOperationHandle<Sprite>[_content.Sprites.Length];
            var handle = _content.Sprites[index].LoadAssetAsync<Sprite>();

            await handle.Task;

            _spriteHandles[index] = handle;

            return handle.Status == AsyncOperationStatus.Succeeded ? handle.Result : null;
        }

        public async Task<Sprite[]> GetAllSprite(int indexException = -1)
        {
            var result = new Sprite[_content.Sprites.Length];
            _spriteHandles ??= new AsyncOperationHandle<Sprite>[_content.Sprites.Length];

            for (int i = 0; i < result.Length; i++)
            {
                if (i == indexException)
                    continue;

                var handle = _content.Sprites[i].LoadAssetAsync<Sprite>();
                await handle.Task;

                _spriteHandles[i] = handle;
                result[i] = handle.Status == AsyncOperationStatus.Succeeded ? handle.Result : null;
            }

            return result;
        }

        public void Release(in int index) => Addressables.Release(_spriteHandles[index]);
    }
}