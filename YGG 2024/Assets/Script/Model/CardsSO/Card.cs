using Solana.Unity.Soar.Accounts;
using TMPro;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class Card : MonoBehaviour
{
    [Header("Script")]
    [SerializeField] CardGameManager cardGameManager;

    [Header("Scriptable Objects")]
    public CardSO cardSO;

    [Header("HUD")]
    [SerializeField] TMP_Text cardName;
    [SerializeField] TMP_Text cardDescription;
    [SerializeField] TMP_Text cardTypes;
    [SerializeField] Image cardImage;
    [SerializeField] GameObject selected;
    [SerializeField] public bool isSelected;

    [Space(10)]
    [SerializeField] Button cardButton;

    [Header("Flags")]
    [SerializeField] int cardHealth;
    public bool hasBeenPlayed;
    public int handIndex;

    public void OnClickEvent()
    {
        if(CardGameManager.instance != null){
            if(!CardGameManager.instance.yourTurn){
                Debug.Log("Not Your Turn!");
                return;
            }
            if(isSelected){
                Deselect();
            }else{
                Select();
            }
            CardGameManager.instance.CardSelect(this);
        }
    }
    public void Deselect(){
        selected.SetActive(false);
        isSelected = false;
    }
    public void Select(){
        selected.SetActive(true);
        isSelected = true;
    }
    public void DisplayCard()
    {
        cardHealth = cardSO.cHealth;
        cardName.text = cardSO.cName;
        cardDescription.text = cardSO.cDescription;
        cardImage.sprite = cardSO.cImage;
        
        cardTypes.text = cardSO.cType;

        if (cardSO.cType == "Inferno")
        {
            cardTypes.color = Color.red; // Red for Inferno
        }
        else if (cardSO.cType == "Nature")
        {
            cardTypes.color = Color.green; // Green for Nature
        }
        else if (cardSO.cType == "Hydro")
        {
            cardTypes.color = Color.blue; // Blue for Hydro
        }
        else
        {
            cardTypes.color = Color.black; // Default color if no match
        }
    }
}