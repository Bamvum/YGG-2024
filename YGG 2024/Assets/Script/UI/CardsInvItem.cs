using System;
using TMPro;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class CardsInvItem : MonoBehaviour, IPointerClickHandler
{

    [SerializeField]
    private Image itemImage;
    
    [SerializeField]
    private TMP_Text Name;
    [SerializeField]
    private TMP_Text Description;
    [SerializeField]
    private TMP_Text Type;

    [SerializeField]
    private Image borderImage;

    [SerializeField]
    private Image EquippedImage;

    public event Action<CardsInvItem> OnItemClicked;

    public void OnPointerClick(PointerEventData eventData)
    {
        if (eventData.button == PointerEventData.InputButton.Right)
        {

            OnItemClicked?.Invoke(this);
        }
        else
        {
            OnItemClicked?.Invoke(this);
           
        }
    }

    public void select()
    {
        borderImage.enabled = true;
    }

    public void DeSelect()
    {
        borderImage.enabled = false;
    }

    public int temporaryIndex = 0;
    public void SetTemporaryIndex(int index)
    {
        GameManager.instance.tempindex = index;
        temporaryIndex = index;
    }

    public void SetData(Sprite sprite, int quantity, string name, string desc, string type, bool Equipped)
    {
        if (itemImage != null)
        {

            Name.text = name;
            Description.text = desc;
            Type.text = type;

            itemImage.gameObject.SetActive(true);
            itemImage.sprite = sprite;

            if(Equipped == true)
            {
                EquippedImage.gameObject.SetActive(true);
            }
            else
            {
                EquippedImage.gameObject.SetActive(false);
            }

            //itemQuantity.text = quantity.ToString();
            
        }
        else
        {
            Debug.LogWarning("itemImage is null. This may be due to the scene being reloaded.");
        }
    }

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
