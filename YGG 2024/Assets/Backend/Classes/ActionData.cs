using System;
using System.Collections.Generic;
using Unisave;
using UnityEngine;

namespace ESDatabase.Classes
{
    [Serializable]
    public class ActionData
    {
        public string attackerCardID;
        public List<ActiveCards> activeDeck;
        public List<ActiveCards> cardDeck;
        public ActionType actionType;
        public int attackedSlotNo;
    }

    public class ActiveCards {
        public string uniqueID;
        public int cardHP;
    }

    public enum ActionType {
        Attack,
        None
    }
}