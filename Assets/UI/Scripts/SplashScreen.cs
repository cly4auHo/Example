using GameManagment;
using UnityEngine;
using Zenject;

public class SplashScreen : MonoBehaviour
{
    [Inject] private IGameManager _gameManager;
    
    public void Start()
    {
        _gameManager.RestartGame += StartGame;
    }

    private void StartGame()
    {
        _gameManager.RestartGame -= StartGame;
        Destroy(gameObject);
    }
}
