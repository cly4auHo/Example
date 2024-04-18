using System.Threading.Tasks;

namespace Network
{
    public interface IServerApi
    {
        Task<GameModel> GetGameModel();
        Task<LeaderboardModel> GetLeaderboardModel();

        Task AddUserInLeaderboard(LeaderboardModel model);
    }
}