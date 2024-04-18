using System.Collections.Generic;
using System.IO;
using System.Threading.Tasks;
using UnityEngine;

namespace Network
{
    public class ServerApiStub : IServerApi
    {
        const int DEFAULT_MAX_AMOUNT_LEADERBOARD = 10;
        
        public async Task<GameModel> GetGameModel()
        {
            if (File.Exists(ServerApiUrls.GAME_MODEL))
                return JsonUtility.FromJson<GameModel>(await File.ReadAllTextAsync(ServerApiUrls.GAME_MODEL)) ?? GameModel.DEFAULT;
       
            await File.WriteAllTextAsync(ServerApiUrls.GAME_MODEL, JsonUtility.ToJson(GameModel.DEFAULT));
        
            return GameModel.DEFAULT;
        }

        public async Task<LeaderboardModel> GetLeaderboardModel()
        {
            if (File.Exists(ServerApiUrls.LEADERBOARD_DATA))
                return JsonUtility.FromJson<LeaderboardModel>(await File.ReadAllTextAsync(ServerApiUrls.LEADERBOARD_DATA));
            
            var model = new LeaderboardModel {MaxAmount = DEFAULT_MAX_AMOUNT_LEADERBOARD, Users = new List<LeaderboardUserModel>()};
            
            await File.WriteAllTextAsync(ServerApiUrls.LEADERBOARD_DATA, JsonUtility.ToJson(model));

            return model;
        }

        public async Task AddUserInLeaderboard(LeaderboardModel model)
        {
            await File.WriteAllTextAsync(ServerApiUrls.LEADERBOARD_DATA, JsonUtility.ToJson(model));
        }
    }
}