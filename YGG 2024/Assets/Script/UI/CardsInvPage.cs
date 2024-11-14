using System.Collections.Generic;
using System;
using UnityEngine;

public class CardsInvPage : MonoBehaviour
{
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    [SerializeField]
    public CardsInvItem itemPrefab;
    [SerializeField]
    private RectTransform contentPanel;
   

    public List<CardsInvItem> ListofItems = new List<CardsInvItem>();
    

    public event Action<int> OnItemActionRequested;
   

    void Start()
    {
        
    }
    public void InitializeInventoryUI(int inventSize)
    {

        for (int i = 0; i < inventSize; i++)
        {
            CardsInvItem uiItem = Instantiate(itemPrefab, Vector3.zero, Quaternion.identity);
            uiItem.transform.SetParent(contentPanel);
            //uiItem.transform.localScale = new Vector3(100, 100, 100);
            ListofItems.Add(uiItem);
            uiItem.OnItemClicked += HandleItemSelection;
            
        }

    }

    public void UpdateData(int itemIndex, Sprite ItemImage, int ItemQuantity, string name, string desc, string type, bool equipped)
    {
        if (ListofItems.Count > itemIndex)
        {
            ListofItems[itemIndex].SetData(ItemImage, ItemQuantity, name, desc, type, equipped);
        }
    }

    public void ResetSelection()
    {
        DeselectAllItems();
    }
    private void DeselectAllItems()
    {
        foreach (CardsInvItem item in ListofItems)
        {
            item.DeSelect();
        }
    }

    public void SelectItemAtIndex(int selectedIndex)
    {
        if (selectedIndex >= 0 && selectedIndex < ListofItems.Count)
        {
            // Deselect all items first
            ResetSelection();
            DeselectAllItems();

            // Select the item at the specified index
            ListofItems[selectedIndex].select();
            //itemDesc.ResetDescription();
            //OnDescriptionRequested?.Invoke(selectedIndex);

        }
        else
        {
            Debug.LogError("Invalid index provided for selection.");
        }

    }

    private void HandleItemSelection(CardsInvItem InventoryItemUI)
    {
        int index = ListofItems.IndexOf(InventoryItemUI);
        if (index == -1)
        {
            return;
        }
        InventoryItemUI.SetTemporaryIndex(index);
        //OnDescriptionRequested?.Invoke(index);
        OnItemActionRequested?.Invoke(index);

    }

    public void ClearItems()
    {
        foreach (var item in ListofItems)
        {
            item.gameObject.SetActive(false);// Assuming ListOfShopItem2s contains the GameObjects of shop items
        }
        ListofItems.Clear();
    }


    // Update is called once per frame
    void Update()
    {
        
    }
}
