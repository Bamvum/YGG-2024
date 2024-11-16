using System;
using Unisave;
namespace ESDatabase.Classes
{
    [Serializable]
    public class CardData
    {
        [Fillable] public string cardID;
        [Fillable] public int quantity;
        [Fillable] public bool isMinted = false;
        public CardData(string cID){
            this.cardID = cID;
        }
    }
}
