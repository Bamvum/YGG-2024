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
        [Fillable] public double money;
        [Fillable] public int pack;
        [Fillable] public int energy;
        [Fillable] public List<CardData> cardDeck;
        [Fillable] public List<CardData> cardList;
        public GameData(){
            this.playerName = "Unnamed Player";
            this.playerSkin = new PlayerSkin();
            this.money = 1000;
            this.pack = 1;
            this.energy = 5;
            this.cardDeck = new List<CardData>{null, null, null, null, null, null};
            this.cardList = new List<CardData>();
        }
    }
}
