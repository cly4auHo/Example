using System.Collections.Generic;
using Network;

namespace Leaderboard
{
    
    public interface ILeaderboardSystem
    {
        bool Initialized { get; }
        List<LeaderboardUserModel> AllUsers { get; }

        void Init();
        void AddUser(LeaderboardUserModel model);
        bool IsNewRecord(in int score);
    }
}