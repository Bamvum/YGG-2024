using Solana.Unity.Soar.Accounts;
using TMPro;
using Unity.VisualScripting;
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

    [Space(10)]
    [SerializeField] Button cardButton;

    [Header("Flags")]
    [SerializeField] int cardHealth;
    public bool hasBeenPlayed;
    public int handIndex;

    private void Start()
    {
        cardButton.onClick.AddListener(() => CardGameManager.instance.CardSelect(this));

        DisplayCard();
    }

    void DisplayCard()
    {
        cardHealth = cardSO.cHealth;
        cardName.text = cardSO.cName;
        cardDescription.text = cardSO.cDescription;
        cardImage.sprite = cardSO.cImage;
        // cardTypes.text = cardSO.cType;
    }
}