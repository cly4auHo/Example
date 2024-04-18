using System.Threading.Tasks;
using UnityEngine;

public interface IBackgroundSystem
{
    bool Initialized { get; }
    Sprite CurrentBackground { get; } 
    int CurrentIndex { get; }
    
    void Init();
    void SetNewBackground(int index);

    Task<Sprite[]> GetAll();
}
