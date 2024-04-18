using System.Collections.Generic;
using System.IO;
using System.Linq;
using Network;
using UnityEngine;
using Zenject;

public class LeaderboardSystem : ILeaderboardSystem
{
    int _maxAmount;

    [Inject] IServerApi _serverApi;
    
    public bool Initialized { get; private set; }
    public List<LeaderboardUserModel> AllUsers { get; private set; }

    public async void Init()
    {
        var leaderboardModel = await _serverApi.GetLeaderboardModel();

        _maxAmount = leaderboardModel.MaxAmount;
        AllUsers = leaderboardModel.Users;
        AllUsers = AllUsers.OrderByDescending(user => user.score).Take(_maxAmount).ToList();
        Initialized = true;
    }

    public async void AddUser(LeaderboardUserModel model)
    {
        var existUser = AllUsers.FirstOrDefault(user => user.name == model.name);

        if (existUser == null)
            AllUsers.Add(model);
        else
            existUser.score = Mathf.Max(existUser.score, model.score);
        
        AllUsers = AllUsers.OrderByDescending(user => user.score).Take(_maxAmount).ToList();

        await _serverApi.AddUserInLeaderboard(new LeaderboardModel {Users = AllUsers, MaxAmount = _maxAmount});
    }

    public bool IsNewRecord(in int score) => !AllUsers.Any() || AllUsers.Last().score < score;
}
