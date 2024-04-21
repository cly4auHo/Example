using System;
using System.Threading.Tasks;
using UnityEngine;

namespace Background
{
    public interface IBackgroundSystem
    {
        event Action<Sprite> NewBackground; 
        
        bool Initialized { get; }
        Sprite CurrentBackground { get; }

        void Init();
        void SetNewBackground(int index);

        Task<Sprite[]> GetAll();
    }
}