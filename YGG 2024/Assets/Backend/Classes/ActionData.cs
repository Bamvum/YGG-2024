using System;
using System.Collections.Generic;
using System.Linq;

namespace ESDatabase.Classes
{
    [Serializable]
    public class ActionData
    {
        public string attackerCardID;
        public ActionType actionType;
        public int damage = 0;
        public int attackerSlotNo;
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
            bool hostFirst = CoinFlip();
            hostTurn = hostFirst;
            joinerTurn = !hostTurn;
            hostDeck = hd;
            joinerDeck = ed;
            ShuffleDeck(hostDeck);
            ShuffleDeck(joinerDeck);
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
        public void ShuffleDeck(List<ActiveCards> deck)
        {
            Random rng = new Random();
            for (int i = deck.Count - 1; i > 0; i--)
            {
                int swapIndex = rng.Next(i + 1);
                ActiveCards temp = deck[i];
                deck[i] = deck[swapIndex];
                deck[swapIndex] = temp;
            }
        }
        public bool CoinFlip()
        {
            Random random = new Random();
            return random.Next(0, 2) == 0;
        }
    }
}