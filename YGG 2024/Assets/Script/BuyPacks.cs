using System.Collections.Generic;
using ESDatabase.Classes;
using UnityEngine;
using static CardSOData;

public class BuyPacks : MonoBehaviour
{
    public async void BuyRandomItems()
    {
        GameManager.instance.itemsToTransfer.Clear();
        PlayerData playerData = AccountManager.Instance.playerData;
        // Randomly select 6 items from CardItems
        List<CardData> listCards = new List<CardData>(); 
        for (int i = 0; i < 6; i++)
        {
            // Ensure we have items to select from
            if (GameManager.instance.cardLists.CardItems.Count == 0) return;

            // Select a random index
            int randomIndex = Random.Range(0, GameManager.instance.cardLists.CardItems.Count);

            // Create a new Cards instance for the selected item
            CardSO selectedCard = GameManager.instance.cardLists.CardItems[randomIndex];
            Cards newCard = Utilities.cardtoCards(selectedCard); // Assuming quantity is 1 for each selection

            // Add the newCard to itemsToTransfer
            GameManager.instance.AddItemToTransfer(newCard);
            CardData cardData = new CardData(newCard.item.UniqueID);
            listCards.Add(cardData);
        }
        
        playerData.gameData.cardList.AddRange(listCards);
        await AccountManager.SaveData(playerData);
    }


}
