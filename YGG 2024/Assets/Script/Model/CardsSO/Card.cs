using TMPro;
using UnityEngine;
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
    
    [Header("Flags")]
    public bool hasBeenPlayed;
    public int handIndex;

    private void Start()
    {
        // cardGameManager = FindObjectOfType<CardGameManager>();

        //DisplayCard();
    }

    void DisplayCard()
    {
        cardName.text = cardSO.cName;
        cardDescription.text = cardSO.cDescription;
        cardImage.sprite = cardSO.cImage;
        // cardTypes.text = cardSO.cType;
    }

    public void ForTesting()
    {
        Debug.Log("Card Pressed");
    }
}
