using System.Threading.Tasks;
using Content;
using UnityEngine;
using Zenject;

namespace Background
{
    public class BackgroundSystem : IBackgroundSystem
    {
        private const string KEY = "background";

        [Inject] private IContentManager _contentManager;

        private Sprite[] _all;
        private int _currentIndex = -1;
        
        public bool Initialized { get; private set; }
        public Sprite CurrentBackground { get; private set; }
        
        public async void Init()
        {
            _currentIndex = PlayerPrefs.GetInt(KEY, 0);

            CurrentBackground = await _contentManager.GetSprite(_currentIndex);

            Initialized = true;
        }

        public void SetNewBackground(int index)
        {
            PlayerPrefs.SetInt(KEY, index);
            CurrentBackground = _all[index];
            _currentIndex = index;

            for (int i = 0; i < _all.Length; i++)
            {
                if (i != index)
                    _contentManager.Release(i);
            }
        }

        public async Task<Sprite[]> GetAll()
        {
            _all = await _contentManager.GetAllSprite(_currentIndex);

            if (_currentIndex != -1)
                _all[_currentIndex] = CurrentBackground;

            return _all;
        }
    }
}