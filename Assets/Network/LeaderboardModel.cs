using System;
using System.Collections.Generic;

namespace Network
{
    [Serializable]
    public class LeaderboardModel
    {
        public int MaxAmount;
        public List<LeaderboardUserModel> Users;
    }
}