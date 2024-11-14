using UnityEngine;
using static CardSOData;

public class BuyPacks : MonoBehaviour
{
    public void BuyRandomItems()
    {
        // Clear the previous items if needed
        GameManager.instance.itemsToTransfer.Clear();

        // Randomly select 6 items from CardItems
        for (int i = 0; i < 6; i++)
        {
            // Ensure we have items to select from
            if (GameManager.instance.cardLists.CardItems.Count == 0) return;

            // Select a random index
            int randomIndex = Random.Range(0, GameManager.instance.cardLists.CardItems.Count);

            // Create a new Cards instance for the selected item
            CardSO selectedCard = GameManager.instance.cardLists.CardItems[randomIndex];
            Cards newCard = cardtoCards(selectedCard); // Assuming quantity is 1 for each selection

            // Add the newCard to itemsToTransfer
            GameManager.instance.AddItemToTransfer(newCard);
        }
    }

    public Cards cardtoCards(CardSO selectedCard)
    {
        Cards newCard = new Cards();

        newCard.item = selectedCard;
        newCard.quantity = 1;

        return newCard;
    }
}
