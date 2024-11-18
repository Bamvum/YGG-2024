using System.Collections.Generic;
using DG.Tweening;
using ESDatabase.Classes;
using UnityEngine;
using static CardSOData;

public class BuyPacks : MonoBehaviour
{

    public GameObject itemPrefab; // Prefab that includes CardsInvItem component
    public Transform cardContainer;
    public GameObject ReceiveCard;

    //public async void BuyRandomItems()
    //{

    //    if (GameManager.instance.PlayerMoney >= 1000)
    //    {

    //        GameManager.instance.itemsToTransfer.Clear();
    //        PlayerData playerData = AccountManager.Instance.playerData;
    //        // Randomly select 6 items from CardItems
    //        List<CardData> listCards = new List<CardData>();
    //        for (int i = 0; i < 6; i++)
    //        {
    //            // Ensure we have items to select from
    //            if (GameManager.instance.cardLists.CardItems.Count == 0) return;

    //            // Select a random index
    //            int randomIndex = Random.Range(0, GameManager.instance.cardLists.CardItems.Count);

    //            // Create a new Cards instance for the selected item
    //            CardSO selectedCard = GameManager.instance.cardLists.CardItems[randomIndex].CreateCopy();
    //            Cards newCard = Utilities.cardtoCards(selectedCard); // Assuming quantity is 1 for each selection

    //            // Add the newCard to itemsToTransfer
    //            GameManager.instance.AddItemToTransfer(newCard);
    //            CardData cardData = new CardData(newCard.item.UniqueID);
    //            listCards.Add(cardData);
    //        }

    //        playerData.gameData.cardList.AddRange(listCards);
    //        await AccountManager.SaveData(playerData);

    //        GameManager.instance.DeductMoney(1000);

    //    }
    //    else
    //    {
    //        Debug.Log("Not Enough Money");
    //    }
    //}

    public async void BuyRandomItems()
    {
        if (AccountManager.Instance.playerData.gameData.money >= 1000)
        {
            GameManager.instance.itemsToTransfer.Clear();
            PlayerData playerData = AccountManager.Instance.playerData;
            List<CardData> listCards = new List<CardData>();

            // Clear the container before adding new cards
            foreach (Transform child in cardContainer)
            {
                Destroy(child.gameObject);
            }

            for (int i = 0; i < 6; i++)
            {
                if (GameManager.instance.cardLists.CardItems.Count == 0) return;

                int randomIndex = Random.Range(0, GameManager.instance.cardLists.CardItems.Count);
                CardSO selectedCard = GameManager.instance.cardLists.CardItems[randomIndex].CreateCopy();
                Cards newCard = Utilities.cardtoCards(selectedCard);

                GameManager.instance.AddItemToTransfer(newCard);
                CardData cardData = new CardData(newCard.item.UniqueID);
                listCards.Add(cardData);

            GameObject cardObject = Instantiate(itemPrefab, cardContainer);
            cardObject.transform.localScale = Vector3.zero; // Start from zero scale for animation

            // Ensure all child objects use the parent's scale by default
            foreach (Transform child in cardObject.transform)
            {
                child.localScale = Vector3.one; // Reset child scales to ensure they follow the parent's scale
            }

            // Get the CardsInvItem component and set the data
            CardsInvItem cardItemComponent = cardObject.GetComponent<CardsInvItem>();
            cardItemComponent.SetData(newCard.item.cImage, newCard.quantity, newCard.item.cName, newCard.item.cDescription, newCard.item.cType, newCard.isEquipped);

            // Use DoTween to animate the card scaling
            ReceiveCard.SetActive(true);
            cardObject.transform.DOScale(new Vector3(1.8f, 1.8f, 1.8f), 0.5f).SetDelay(i * 0.2f); // Delay animation for sequential effect
        }

            playerData.gameData.cardList.AddRange(listCards);
            await AccountManager.SaveData(playerData);
            GameManager.instance.DeductMoney(1000);
        }
        else
        {
            //Debug.Log("Not Enough Money");

            GameManager.instance.ShowFloatingText("NOT ENOUGH MONEY", Color.red);
        }
    }


}
