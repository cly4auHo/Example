using System.IO;
using UnityEngine;
using Zenject;

public class GameManager : MonoBehaviour
{
    [Inject] IBackgroundSystem _backgroundSystem;
    [Inject] ILeaderboardSystem _leaderboardSystem;
    [Inject] Widget _widget;
    
    void Start()
    {
        _backgroundSystem.Init();
        _leaderboardSystem.Init();

        var model = LoadGameModel();
        
        _widget.Init(model);

        ExampleGenerator.AmountOfAnswers = model.AmountOfAnswers;
    }

    GameModel LoadGameModel()
    {
        var filePath = $"{Application.dataPath}/Game/JSON/Data.json";
        
        if (File.Exists(filePath))
            return JsonUtility.FromJson<GameModel>(File.ReadAllText(filePath)) ?? GameModel.DEFAULT;
       
        File.WriteAllText(filePath, JsonUtility.ToJson(GameModel.DEFAULT));
        
        return GameModel.DEFAULT;
    }
}
