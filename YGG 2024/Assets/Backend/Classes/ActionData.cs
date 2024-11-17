using System;
using System.Collections.Generic;
using System.Linq;
using Unisave;
using UnityEngine;

namespace ESDatabase.Classes
{
    [Serializable]
    public class ActionData
    {
        public string attackerCardID;
        public List<ActiveCards> activeDeck;
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
    
    public class LobbyData {
        public List<ActiveCards> hostActiveCards;
        public List<ActiveCards> hostCurrentDeck;
        public List<ActiveCards> hostDeck;
        public List<ActiveCards> joinerActiveCards;
        public List<ActiveCards> joinerCurrentDeck;
        public List<ActiveCards> joinerDeck;
        
        public bool hostTurn = false;
        public bool joinerTurn = false;

        public LobbyData(List<ActiveCards> hd, List<ActiveCards> ed){
            hostDeck = hd;
            joinerDeck = ed;
            hostActiveCards = DrawCards(hostDeck, 3);
            joinerActiveCards = DrawCards(joinerDeck, 3);
            hostCurrentDeck = new List<ActiveCards>(hostDeck);
            joinerCurrentDeck = new List<ActiveCards>(joinerDeck);
        }
        public List<ActiveCards> DrawCards(List<ActiveCards> deck, int numberOfCards)
        {
            if (numberOfCards > deck.Count)
            {
                throw new InvalidOperationException("Not enough cards in the deck.");
            }

            List<ActiveCards> drawnCards = deck.Take(numberOfCards).ToList();
            deck.RemoveRange(0, numberOfCards);
            return drawnCards;
        } 
    }
}