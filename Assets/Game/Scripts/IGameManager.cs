using System;

namespace GameManagment
{
    public interface IGameManager
    {
        event Action RestartGame;
        
        void StartGame();
        void OpenLeaderBoard();
        void OpenBackground();
        void EndGame(in int score);
        void Restart();
        void Submit(string name);
    }
}