using System.Threading.Tasks;
using UnityEngine;

namespace Content
{
    public interface IContentManager
    {
        Task<Sprite> GetSprite(int index);
        Task<Sprite[]> GetAllSprite(int indexException = -1);

        void Release(in int index);
    }
}