using System.Linq;
using ESDatabase.Classes;
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
    [SerializeField] public int slotNo = 0;
    [SerializeField] public int hp = 0;
    [SerializeField] public bool isPlayercard = false;

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
            CardGameManager.instance.CardSelect(this);
        }
    }
    public void Update(){
        if(CardGameManager.instance != null){
            LobbyData lobby = MultiplayerManager.Instance.lobbyData;
            if (MultiplayerManager.Instance.isJoiner) {
                // Check joiner's active cards
                if (lobby.joinerActiveCards[slotNo].cardHP < 1 && lobby.joinerCurrentDeck.Count > 0 && isPlayercard) {
                    Debug.Log($"Replacing joiner card in slot {slotNo}");
                    CardSO selectedCard = GameManager.instance.cardLists.CardItems.FirstOrDefault(card => card.UniqueID == lobby.joinerCurrentDeck[0].uniqueID);
                    cardSO = selectedCard;
                    hp = 20;
                    isPlayercard = true;
                    lobby.joinerActiveCards[slotNo].uniqueID = selectedCard.UniqueID;
                    lobby.joinerActiveCards[slotNo].cardHP = 20;
                    DisplayCard();
                    lobby.joinerCurrentDeck.RemoveAt(0);
                } else if (lobby.joinerActiveCards[slotNo].cardHP < 1 && lobby.joinerCurrentDeck.Count == 0 && isPlayercard) {
                    Debug.Log($"Destroying joiner card in slot {slotNo}");
                    Destroy(gameObject);
                }

                // Check host's active cards
                if (lobby.hostActiveCards[slotNo].cardHP < 1 && lobby.hostCurrentDeck.Count > 0 && !isPlayercard) {
                    Debug.Log($"Replacing host card in slot {slotNo} by joiner");
                    CardSO selectedCard = GameManager.instance.cardLists.CardItems.FirstOrDefault(card => card.UniqueID == lobby.hostCurrentDeck[0].uniqueID);
                    cardSO = selectedCard;
                    hp = 20;
                    isPlayercard = false;
                    lobby.hostActiveCards[slotNo].uniqueID = selectedCard.UniqueID;
                    lobby.hostActiveCards[slotNo].cardHP = 20;
                    DisplayCard();
                    lobby.hostCurrentDeck.RemoveAt(0);
                } else if (lobby.hostActiveCards[slotNo].cardHP < 1 && lobby.hostCurrentDeck.Count == 0 && !isPlayercard) {
                    Debug.Log($"Destroying host card in slot {slotNo}");
                    Destroy(gameObject);
                }
            } else {
                // Host's logic
                if (lobby.hostActiveCards[slotNo].cardHP < 1 && lobby.hostCurrentDeck.Count > 0 && isPlayercard) {
                    Debug.Log($"Replacing host card in slot {slotNo}");
                    CardSO selectedCard = GameManager.instance.cardLists.CardItems.FirstOrDefault(card => card.UniqueID == lobby.hostCurrentDeck[0].uniqueID);
                    cardSO = selectedCard;
                    hp = 20;
                    isPlayercard = true;
                    lobby.hostActiveCards[slotNo].uniqueID = selectedCard.UniqueID;
                    lobby.hostActiveCards[slotNo].cardHP = 20;
                    DisplayCard();
                    lobby.hostCurrentDeck.RemoveAt(0);
                } else if (lobby.hostActiveCards[slotNo].cardHP < 1 && lobby.hostCurrentDeck.Count == 0 && isPlayercard) {
                    Debug.Log($"Destroying host card in slot {slotNo}");
                    Destroy(gameObject);
                }

                // Check joiner's active cards
                if (lobby.joinerActiveCards[slotNo].cardHP < 1 && lobby.joinerCurrentDeck.Count > 0 && !isPlayercard) {
                    Debug.Log($"Replacing joiner card in slot {slotNo} by host");
                    CardSO selectedCard = GameManager.instance.cardLists.CardItems.FirstOrDefault(card => card.UniqueID == lobby.joinerCurrentDeck[0].uniqueID);
                    cardSO = selectedCard;
                    hp = 20;
                    isPlayercard = false;
                    lobby.joinerActiveCards[slotNo].uniqueID = selectedCard.UniqueID;
                    lobby.joinerActiveCards[slotNo].cardHP = 20;
                    DisplayCard();
                    lobby.joinerCurrentDeck.RemoveAt(0);
                } else if (lobby.joinerActiveCards[slotNo].cardHP < 1 && lobby.joinerCurrentDeck.Count == 0 && !isPlayercard) {
                    Debug.Log($"Destroying joiner card in slot {slotNo}");
                    Destroy(gameObject);
                }
            }
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