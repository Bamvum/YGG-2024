using System;
using ESDatabase.Classes;


namespace ESDatabase.Utilities
{
    [Serializable]
    public class DBHelper
    {
        public static string IsPlayerExisting(string pubkey, PlayerData existingPlayer)
        {

            if (existingPlayer != null)
            {
                existingPlayer.lastLoginAt = DateTime.UtcNow;
                existingPlayer.Save();
                return existingPlayer.EntityId;
            }
            else
            {
                var player = new PlayerData(pubkey, DateTime.UtcNow);
                player.Save();
                return player.EntityId;
            }
        }
    }
}
