using System;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;




[CreateAssetMenu]
public class CardSOData : ScriptableObject
{

    [SerializeField]
    public List<Cards> CardItems;

    public event Action<Dictionary<int, Cards>> OnInventoryUpdated;
    [field: SerializeField]

    public int Size { get; private set; }//this will add a size field in unity//sets to 999


    public void Initialize()
    {

        CardItems = new List<Cards>();
        for (int i = 0; i < Size; i++)
        {
            CardItems.Add(Cards.GetEmptyItem());
        }
    }

    public void AddItem(Cards item)
    {
        AddItem(item.item, item.quantity, item.isEquipped);
    }

    public int AddItem(CardSO item, int quantity, bool equip)
    {
        if (item.IsStackable == false)
        {
            for (int i = 0; i < CardItems.Count;)//i++
            {
                while (quantity > 0)
                {
                    quantity -= AddItemToFirstFreeSlot(item, quantity);
                }
                InformAboutChange();
                return quantity;
            }
        }
        InformAboutChange();
        return quantity = AddStackebleItem(item, quantity, equip);
    }


    private int AddItemToFirstFreeSlot(CardSO item, int quantity)
    {
        Cards newItem = new Cards
        {
            item = item,
            quantity = quantity
        };

        for (int i = 0; i < CardItems.Count; i++)
        {
            if (CardItems[i].isEmpty)
            {
                CardItems[i] = newItem;
                return quantity;
            }
        }
        return 0;
    }


    private bool IsInventoryFull() => CardItems.Where(item => item.isEmpty).Any() == false;

    public int AddStackebleItem(CardSO item, int quantity, bool equip)
    {

        for (int i = 0; i < CardItems.Count; i++)
        {
            if (CardItems[i].isEmpty) { continue; }
            if (CardItems[i].item.ID == item.ID)
            {

                int amountPossibleToTake = CardItems[i].item.MaxStackableSize - CardItems[i].quantity;

                if (quantity > amountPossibleToTake)
                {
                    CardItems[i] = CardItems[i].ChangeQuantity(CardItems[i].item.MaxStackableSize, equip);
                    quantity -= amountPossibleToTake;
                }
                else
                {
                    CardItems[i] = CardItems[i].ChangeQuantity(CardItems[i].quantity + quantity, equip);
                    InformAboutChange();
                    return 0;
                }
            }

        }
        while (quantity > 0 && IsInventoryFull() == false)
        {
            int newQuantity = Mathf.Clamp(quantity, 0, item.MaxStackableSize);
            quantity -= newQuantity;
            AddItemToFirstFreeSlot(item, newQuantity);


        }
        return quantity;
    }


    internal void RemoveItem(int itemIndex, int amount, bool equip)
    {
        try
        {
            if (CardItems.Count > itemIndex)
            {
                if (CardItems[itemIndex].isEmpty)
                {
                    return;
                }

                int currentQuantity = CardItems[itemIndex].quantity;

                if (currentQuantity <= amount)
                {
                    // Remove the entire item.
                    CardItems.RemoveAt(itemIndex);
                    //GameManager.instance.itemsToTransfer.RemoveAt(itemIndex);
                    InformAboutChange();

                }
                else
                {
                    // Decrease the item quantity.
                    CardItems[itemIndex] = CardItems[itemIndex].ChangeQuantity(currentQuantity - amount, equip);
                    //GameManager.instance.itemsToTransfer[itemIndex] = GameManager.instance.itemsToTransfer[itemIndex].ChangeQuantity(currentQuantity - amount);
                    InformAboutChange();


                }

            }
        }
        catch (Exception) { }

    }
    internal void RemoveItem(string name, int amount, bool equip)
    {
        try
        {
            // Find the index of the itemToRemove in the CardItems list
            int indexToRemove = CardItems.FindIndex(item => item.item.name.Equals(name));
            // Debug.LogError(indexToRemove);

            if (CardItems.Count > indexToRemove)
            {
                if (CardItems[indexToRemove].isEmpty)
                {
                    return;
                }

                int currentQuantity = CardItems[indexToRemove].quantity;

                if (currentQuantity <= amount)
                {
                    // Remove the entire item.
                    CardItems.RemoveAt(indexToRemove);
                    //GameManager.instance.itemsToTransfer.RemoveAt(indexToRemove);
                    InformAboutChange();

                }
                else
                {
                    // Decrease the item quantity.
                    CardItems[indexToRemove] = CardItems[indexToRemove].ChangeQuantity(currentQuantity - amount, equip);
                   // GameManager.instance.itemsToTransfer[indexToRemove] = GameManager.instance.itemsToTransfer[indexToRemove].ChangeQuantity(currentQuantity - amount);
                    InformAboutChange();
                    // Debug.LogError("Item has been removed");
                }

            }

        }
        catch (Exception)
        {
            // Handle any exceptions here
        }
    }

    public Dictionary<int, Cards> GetCurrentInventoryState()
    {
        Dictionary<int, Cards> returnValue = new Dictionary<int, Cards>();
        for (int i = 0; i < CardItems.Count; i++)
        {
            if (CardItems[i].isEmpty)
            {
                continue;
            }
            returnValue[i] = CardItems[i];
        }
        return returnValue;
    }
   

    public Cards GetItemAt(int itemIndex)
    {
        return CardItems[itemIndex];
    }

    public void SwapItems(int itemIndex1, int itemIndex2)
    {
        try
        {
            Cards item1 = CardItems[itemIndex1];
            CardItems[itemIndex1] = CardItems[itemIndex2];
            CardItems[itemIndex2] = item1;
            InformAboutChange();
        }
        catch (Exception ex)
        {
            Console.WriteLine("Error:" + ex.Message);
        }

    }

    public void UpdateCardAt(int index, Cards updatedCard)
    {
        if (index >= 0 && index < CardItems.Count)
        {
            CardItems[index] = updatedCard;
            InformAboutChange();
        }
    }

    public void InformAboutChange()
    {

        OnInventoryUpdated?.Invoke(GetCurrentInventoryState());
    }

    [Serializable]
    public struct Cards
    {
        public int quantity;
        public bool isEquipped;
        public CardSO item;
        public bool isEmpty => item == null;

        public Cards ChangeQuantity(int newQuantity, bool equip)
        {
            return new Cards
            {
                item = this.item,
                quantity = newQuantity,
                isEquipped = equip
            };
        }

        public static Cards GetEmptyItem() => new Cards
        {
            item = null,
            quantity = 0,
            isEquipped = false
        };
    }
}
