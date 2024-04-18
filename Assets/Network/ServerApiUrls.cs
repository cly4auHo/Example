using UnityEngine;

namespace Network
{
    public static class ServerApiUrls
    {
        public static readonly string GAME_MODEL = $"{Application.dataPath}/Game/JSON/Data.json";
        public static readonly string LEADERBOARD_DATA = $"{Application.dataPath}/Leaderboard/JSON/PlayerData.json";
    }
}