using System;
using System.Collections.Generic;
using Unisave;
namespace ESDatabase.Classes
{
    [Serializable]
    public class GameData
    {
        [Fillable] public string playerName;
        [Fillable] public PlayerSkin playerSkin;

        public GameData(){
            this.playerName = "Unnamed Player";
            this.playerSkin = new PlayerSkin();
        }
    }
}
