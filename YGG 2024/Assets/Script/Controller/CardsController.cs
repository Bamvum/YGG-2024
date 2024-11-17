using ESDatabase.Classes;
using Org.BouncyCastle.Asn1.X509;
using System;
using System.Collections.Generic;
using System.Linq;
using TMPro;
using UnityEngine;
using UnityEngine.UI;
using static CardSOData;
using static UnityEditor.Progress;

public class CardsController : MonoBehaviour
{
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    [SerializeField]
    private CardsInvPage inventoryUI;

    [SerializeField]
    private CardsInvDesc inventoryDesc;

    [SerializeField]
    private CardSOData inventoryData;

    public List<Cards> initialItems = new List<Cards>();

    public List<CardsInvItem> ListofUsedItems = new List<CardsInvItem>();
    private Dictionary<int, int> usedItemsIndexMap = new Dictionary<int, int>();


    public Button UseButton;
    public Button SellButton;
    public Button MintButton;
    public Button UnequippedButton;


    public  Image Card1;
    public  Image Card2;
    public  Image Card3;
    public  Image Card4;
    public  Image Card5;
    public  Image Card6;

    private Transform[] cardParents;

    public TMP_Text UseCardname;
    public TMP_Text UseCardType;
    public TMP_Text UseCarddesc;




    private int CardIndex = 0;

    void Awake() // or use Start() if you prefer
    {
        // Initialize the array in Awake or Start
        cardParents = new Transform[]
        {
        Card1.transform,
        Card2.transform,
        Card3.transform,
        Card4.transform,
        Card5.transform,
        Card6.transform
        };
    }

    void Start()
    {
        GameManager.instance.OnItemsToTransferUpdated += UpdateInventory;
        PrepareInventoryData();
        PrepareUI();
        inventoryUI.ResetSelection();

        UseButton.onClick.AddListener(() => UseCards(CardIndex, cardParents));
        UnequippedButton.onClick.AddListener(() => UnequipCard(CardIndex));
        SellButton.onClick.AddListener(() => SellCard(CardIndex));
    }

    // Update is called once per frame
    void Update()
    {

    }
    public void UpdateInventory(Cards updatedItems)
    {
        
        inventoryData.OnInventoryUpdated += UpdateInventoryUI;
        inventoryData.AddItem(updatedItems);
       
        
    }

    public void PrepareInventoryData()
    {
        initialItems.AddRange(GameManager.instance.itemsToTransfer);
        inventoryData.Initialize();
        inventoryData.OnInventoryUpdated += UpdateInventoryUI;
        foreach (Cards item in initialItems)
        {
            if (item.isEmpty) { continue; }
            inventoryData.AddItem(item);
        }
    }

    public void ToggleALLButton()
    {
        inventoryUI.ClearItems();
        inventoryUI.InitializeInventoryUI(GetUsedSlotsCount());

        foreach (var item in inventoryData.GetCurrentInventoryState())
        {
            inventoryUI.UpdateData(item.Key, item.Value.item.cImage, item.Value.quantity, item.Value.item.cName, item.Value.item.cDescription, item.Value.item.cType, item.Value.isEquipped);
        }

        inventoryUI.ResetSelection();


    }
    public void LoadDatabase(){
        PlayerData playerData = AccountManager.Instance.playerData;

        for(int i = 0; i < 6; i++){
            
            CardData cardData =  playerData.gameData.cardDeck[i];
            if(cardData != null){ 
                
                CardSO selectedCard = GameManager.instance.cardLists.CardItems.FirstOrDefault(card => card.UniqueID.Equals(cardData.cardID));
                Cards newCard = Utilities.cardtoCards(selectedCard);

                CardsInvItem item = Instantiate(inventoryUI.itemPrefab, cardParents[i].GetChild(0));
                item.DeSelect();
                    // Set the card image
                item.SetData(selectedCard.cImage, newCard.quantity, selectedCard.cName, selectedCard.cDescription, selectedCard.cType, newCard.isEquipped);

                newCard.isEquipped = true;
                inventoryData.UpdateCardAt(i, newCard);
                ListofUsedItems.Add(item);

                item.OnItemClicked += HandleItemSelection;
                usedItemsIndexMap[ListofUsedItems.Count - 1] = i;


                    // Set the item's position based on the available slot
                item.transform.localPosition = cardParents[i].GetChild(0).localPosition;
                item.transform.localScale = new Vector3(1.2f, 1.2f, 1.2f);
            }
        }

        foreach(CardData cardData in playerData.gameData.cardList){
            CardSO selectedCard = GameManager.instance.cardLists.CardItems.FirstOrDefault(card => card.UniqueID.Equals(cardData.cardID));
            Cards newCard = Utilities.cardtoCards(selectedCard);

            GameManager.instance.AddItemToTransfer(newCard);
        }
    }

    
    public async void UseCards(int cardIndex, Transform[] parents)
    {
        Cards cards = inventoryData.GetItemAt(cardIndex);
        CardSO card = cards.item;

        // Check if the card is valid
        if (card == null)
        {
            Debug.Log("Invalid card.");
            return;
        }

        // Check if the card is already equipped
        if (cards.isEquipped)
        {
            Debug.Log("The card is already equipped.");
            return;
        }

        PlayerData playerData = AccountManager.Instance.playerData;
        // Iterate over each parent to find an available slot
        foreach (Transform parent in parents)
        {
            Transform availableSlot = FindAvailableSlot(parent);

            if (availableSlot != null)
            {
                // Initialize the itemPrefab
                CardsInvItem item = Instantiate(inventoryUI.itemPrefab, availableSlot);
                item.DeSelect();
                // Set the card image
                item.SetData(card.cImage, cards.quantity, card.cName, card.cDescription, card.cType, cards.isEquipped);

                cards.isEquipped = true;
                inventoryData.UpdateCardAt(cardIndex, cards);
                ListofUsedItems.Add(item);
                CardData cardData = new CardData(cards.item.UniqueID);
                playerData.gameData.cardDeck[ListofUsedItems.Count - 1] = cardData;

                item.OnItemClicked += HandleItemSelection;
                usedItemsIndexMap[ListofUsedItems.Count - 1] = cardIndex;


                // Set the item's position based on the available slot
                item.transform.localPosition = availableSlot.localPosition;
                item.transform.localScale = new Vector3(1.2f, 1.2f, 1.2f);
                await AccountManager.SaveData(playerData);
                // Stop after placing the card in the first available slot
                return;
            }
        }

        // If no available slots were found
        Debug.Log("There's no slot available.");
    }

    public async void UnequipCard(int cardIndex)
    {
        // Retrieve the card data from the inventory
        Cards cards = inventoryData.GetItemAt(cardIndex);
        CardSO card = cards.item;

        // Check if the card is valid
        if (card == null)
        {
            Debug.Log("Invalid card.");
            return;
        }

        // Check if the card is currently equipped
        if (!cards.isEquipped)
        {
            Debug.Log("The card is not equipped.");
            return;
        }

        // Find the corresponding UI item for the equipped card
        CardsInvItem itemToRemove = null;
        int indexToRemove = -1;
        for (int i = 0; i < ListofUsedItems.Count; i++)
        {
            if (usedItemsIndexMap[i] == cardIndex)
            {
                itemToRemove = ListofUsedItems[i];
                indexToRemove = i;
                break;
            }
        }

        // If the item is found, remove it from the UI
        if (itemToRemove != null)
        {
            // Set the previously selected item to the one being unequipped
            previouslySelectedItem = itemToRemove;
            PlayerData playerData = AccountManager.Instance.playerData;
            playerData.gameData.cardDeck[indexToRemove] = null;
            ListofUsedItems.RemoveAt(indexToRemove);
            Destroy(itemToRemove.gameObject); // Remove the item from the UI

            // Update the usedItemsIndexMap
            foreach (var kvp in usedItemsIndexMap.ToList())
            {
                if (kvp.Key > indexToRemove)
                {
                    usedItemsIndexMap[kvp.Key - 1] = kvp.Value;
                    usedItemsIndexMap.Remove(kvp.Key);
                }
            }
            await AccountManager.SaveData(playerData);
        }
        else
        {
            Debug.Log("No UI item found for the equipped card.");
            return;
        }

        // Update the card data to mark it as unequipped
        cards.isEquipped = false;
        inventoryData.UpdateCardAt(cardIndex, cards);

        Debug.Log($"Card '{card.cName}' has been unequipped.");
    }

    public void SellCard(int cardIndex)
    {
        // Retrieve the card data from the inventory
        Cards cards = inventoryData.GetItemAt(cardIndex);
        CardSO card = cards.item;

        // Check if the card is valid
        if (card == null)
        {
            Debug.Log("Invalid card.");
            return;
        }

        // Check if the card is currently equipped
        if (cards.isEquipped)
        {
            Debug.Log("You cannot sell an equipped card. Please unequip it first.");
            return;
        }
       
        // Remove the card from the inventory
        inventoryData.RemoveItem(cardIndex,1, false); // 'false' indicates that we are not equipping the item

        Debug.Log($"Card '{card.cName}' has been sold.");
        ToggleALLButton();
    }

    private CardsInvItem previouslySelectedItem;
    private void HandleItemSelection(CardsInvItem InventoryItemUI)
    {
        int index = ListofUsedItems.IndexOf(InventoryItemUI);
        if (index == -1)
        {
            return;
        }

        if (previouslySelectedItem != null && previouslySelectedItem != InventoryItemUI)
        {
            previouslySelectedItem.DeSelect();
        }


        InventoryItemUI.select();
        previouslySelectedItem = InventoryItemUI;

        if (usedItemsIndexMap.TryGetValue(index, out int originalCardIndex))
        {
            // Retrieve the item from the inventoryData using the original index
            Cards inventoryItem = inventoryData.GetItemAt(originalCardIndex);
            //Debug.Log($"Selected item from original index {originalCardIndex}: {inventoryItem.item.cName}");

            UseCardname.text = inventoryItem.item.cName;
            UseCardType.text = inventoryItem.item.cType;
            UseCarddesc.text = inventoryItem.item.cDescription;
            CardIndex = originalCardIndex;

            // Proceed with any additional logic for the selected item
        }
        else
        {
            Debug.Log("Original index not found in the dictionary.");
        }

    }

    public void AutoSelectFirstItem()
    {
        if (ListofUsedItems.Count > 0)
        {
            // Get the first item (index 0) in the ListofUsedItems
            CardsInvItem firstItem = ListofUsedItems[0];

            
            HandleItemSelection(firstItem);
        }
        else
        {
            Debug.Log("No items in the list to auto-select.");
        }
    }

    private Transform FindAvailableSlot(Transform parent)
    {
        for (int i = 0; i < parent.childCount; i++)
        {
            Transform child = parent.GetChild(i);

            if (child.childCount == 0 || child.GetChild(0).childCount == 0)
            {
                return child;
            }
        }

        return null;
    }



    private void PrepareUI()
    {
        inventoryUI.InitializeInventoryUI(GetUsedSlotsCount());

        foreach (var item in inventoryData.GetCurrentInventoryState())
        {
            inventoryUI.UpdateData(item.Key, item.Value.item.cImage, item.Value.quantity, item.Value.item.cName, item.Value.item.cDescription, item.Value.item.cType, item.Value.isEquipped);
        }

        inventoryUI.OnItemActionRequested += HandleItemActionRequest;

    }

    private void UpdateInventoryUI(Dictionary<int, Cards> inventoryState)
    {
        
        foreach (var item in inventoryState)
        {
            inventoryUI.UpdateData(item.Key, item.Value.item.cImage, item.Value.quantity, item.Value.item.cName, item.Value.item.cDescription, item.Value.item.cType, item.Value.isEquipped);
        }
    }
    private int GetUsedSlotsCount()//this will only used the slots with items
    {
        int usedSlots = 0;
        foreach (var item in inventoryData.CardItems)
        {
            if (!item.isEmpty)
            {
                usedSlots++;
            }
        }
        return usedSlots;
    }

    
    private void HandleItemActionRequest(int itemIndex)
    {
        Cards inventoryItem = inventoryData.GetItemAt(itemIndex);
        if (inventoryItem.isEmpty)
        {

            inventoryUI.ResetSelection();
            return;
        }

        CardIndex = itemIndex;
        inventoryUI.SelectItemAtIndex(itemIndex);
        inventoryDesc.Show();
        inventoryDesc.UpdateData(inventoryItem.item.cImage, inventoryItem.item.cName, inventoryItem.item.cDescription, inventoryItem.item.cType);
    }

    private void OnApplicationQuit()
    {
        
    }
}
