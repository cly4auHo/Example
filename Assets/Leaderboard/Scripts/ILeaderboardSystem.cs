using System.Collections.Generic;
using Network;

public interface ILeaderboardSystem
{
    bool Initialized { get; }
    List<LeaderboardUserModel> AllUsers { get; }

    void Init();
    void AddUser(LeaderboardUserModel model);
    bool IsNewRecord(in int score);
}
