using System.Threading.Tasks;
using UnityEngine;
using Zenject;

public class BackgroundSystem : IBackgroundSystem
{
    const string KEY = "background";
    
    [Inject] IContentManager _contentManager;

    Sprite[] _all;
    
    public bool Initialized { get; private set; }
    public Sprite CurrentBackground { get; private set; }
    public int CurrentIndex { get; private set; } = -1;
    
    public async void Init()
    {
        CurrentIndex = PlayerPrefs.GetInt(KEY, 0);
        
        CurrentBackground = await _contentManager.GetSprite(CurrentIndex);

        Initialized = true;
    }

    public void SetNewBackground(int index)
    {
        PlayerPrefs.SetInt(KEY, index);
        CurrentBackground = _all[index];
        CurrentIndex = index;
        
        for (int i = 0; i < _all.Length; i++)
        {
            if (i != index)
                _contentManager.Release(i);   
        }
    }

    public async Task<Sprite[]> GetAll()
    {
        _all = await _contentManager.GetAllSprite(CurrentIndex);

        if (CurrentIndex != -1)
            _all[CurrentIndex] = CurrentBackground;
        
        return _all;
    }
}
