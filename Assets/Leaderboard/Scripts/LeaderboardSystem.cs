using System.Collections.Generic;
using System.IO;
using System.Linq;
using UnityEngine;

public class LeaderboardSystem : ILeaderboardSystem
{
    const int DEFAULT_MAX_AMOUNT_LEADERBOARD = 10;
    
    int _maxAmount;
    string _saveFilePath;

    public bool Initialized { get; private set; }
    public List<LeaderboardUserModel> AllUsers { get; private set; }

    public void Init()
    {
        _saveFilePath = $"{Application.dataPath}/Leaderboard/JSON/PlayerData.json"; 

        if (File.Exists(_saveFilePath))
        {
            var model = JsonUtility.FromJson<LeaderboardModel>(File.ReadAllText(_saveFilePath));

            _maxAmount = model.MaxAmount;
            AllUsers = model.model;
            AllUsers = AllUsers.OrderByDescending(user => user.score).Take(_maxAmount).ToList();
        }
        else
        {
            AllUsers = new List<LeaderboardUserModel>();
            var model = new LeaderboardModel {MaxAmount = DEFAULT_MAX_AMOUNT_LEADERBOARD, model = AllUsers};
            File.WriteAllText(_saveFilePath, JsonUtility.ToJson(model));
        }

        Initialized = true;
    }

    public void AddUser(LeaderboardUserModel model)
    {
        var existUser = AllUsers.FirstOrDefault(user => user.name == model.name);

        if (existUser == null)
            AllUsers.Add(model);
        else
            existUser.score = Mathf.Max(existUser.score, model.score);
        
        AllUsers = AllUsers.OrderByDescending(user => user.score).Take(_maxAmount).ToList();

        File.WriteAllText(_saveFilePath, JsonUtility.ToJson(new LeaderboardModel {model = AllUsers, MaxAmount = _maxAmount}));
    }

    public bool IsNewRecord(in int score) => !AllUsers.Any() || AllUsers.Last().score < score;
}
