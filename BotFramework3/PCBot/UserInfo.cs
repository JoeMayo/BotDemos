using System;

namespace PCBot
{
    [Serializable]
    public class UserInfo
    {
        public string UserName { get; set; }
        public DateTime Joined { get; set; }
        public DateTime LastVisit { get; set; }
    }
}