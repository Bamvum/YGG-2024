using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;
using DG.Tweening;
using ESDatabase.Classes;
using System.Linq;
using System;
using UnityEngine.SceneManagement;

/*
    TODO    - WHO WILL GO FIRST? COIN FLIP? 
            - TURN-BASED COMBAT 
*/

public class CardGameManager : MonoBehaviour
{
    public static CardGameManager instance {get; private set;}
    [SerializeField] GameObject cardPrefab;
    [SerializeField] GameObject deckParent;
    [SerializeField] CardSO[] cardSOList;
    
    [SerializeField] List<Card> hostDeck = new List<Card>();
    [SerializeField] List<Card> joinerDeck = new List<Card>();
    [SerializeField] Transform[] cardSlots;
    [SerializeField] bool[] availableCardSlots;

    [Header("HUD/UI")]
    [SerializeField] public GameObject gameHudLose;
    [SerializeField] public GameObject gameHudWin;

    [Header("Flag")]
    [SerializeField] TMP_Text deckCountText;
    [SerializeField] public bool yourTurn;

    [Space(10)]
    public Card[] selectedCard = new Card[2];

    [Header("Timer")]
    [SerializeField] TMP_Text timerText;
    [SerializeField] float timerValue;
    [SerializeField] public int ticker = 0;
    void Awake()
    {
        instance = this;
        for(int i = 0; i < availableCardSlots.Length; i++){
            availableCardSlots[i] = true;
        }
        if(MultiplayerManager.Instance.isJoiner){
            yourTurn = MultiplayerManager.Instance.lobbyData.joinerTurn;
        }else{
            yourTurn = MultiplayerManager.Instance.lobbyData.hostTurn;
        }
        if(yourTurn){
            ticker = 1;
        }
        InstantiateCardDeck();
        DrawThreeCards();
    }

    #region - DRAW CARDS -

    // public void DrawCard()
    // {
    //     // IF (yourTurn = true)
    //     if(hostDeck.Count >= 1)
    //     {
    //         Card randCard = hostDeck[Random.Range(0, hostDeck.Count)];
        
    //         for (int i = 0; i < availableCardSlots.Length; i++)
    //         {
    //             if (availableCardSlots[i] == true)
    //             {
    //                 randCard.gameObject.SetActive(true);
    //                 randCard.handIndex= i;

    //                 randCard.transform.position = cardSlots[i].position;
    //                 randCard.transform.SetParent(cardSlots[i]);
    //                 availableCardSlots[i] = false;
    //                 hostDeck.Remove(randCard);
    //                 return;
    //             }
    //         }
    //     }
    // }

    public void DrawThreeCards()
    {
        hostDeck[0].gameObject.SetActive(true);
        hostDeck[1].gameObject.SetActive(true);
        hostDeck[2].gameObject.SetActive(true);
    }

    #endregion

    #region - GENERATE CARDS IN DECK -

    void InstantiateCardDeck()
    {
        if(MultiplayerManager.Instance.isJoiner){
            int j = 0;
            foreach(ActiveCards activeCards in MultiplayerManager.Instance.lobbyData.joinerActiveCards){
                    CardSO selectedCard = GameManager.instance.cardLists.CardItems.FirstOrDefault(card => card.UniqueID == activeCards.uniqueID).CreateCopy();
                    GameObject instantiatedCardObjects = Instantiate(cardPrefab, deckParent.transform);
            
                    instantiatedCardObjects.SetActive(false);
                    Card instantiatedCard = instantiatedCardObjects.GetComponent<Card>();
                    instantiatedCard.transform.position = cardSlots[j].position;
                    instantiatedCard.transform.SetParent(cardSlots[j]);
                    availableCardSlots[j] = false;
                    instantiatedCard.cardSO = selectedCard;
                    instantiatedCard.slotNo = j;
                    instantiatedCard.isPlayercard = true;
                    instantiatedCard.DisplayCard();
                    hostDeck.Add(instantiatedCard);
                    j++;
            }
            int i = 0;
            foreach(ActiveCards activeCards in MultiplayerManager.Instance.lobbyData.hostActiveCards){
                    CardSO selectedCard = GameManager.instance.cardLists.CardItems.FirstOrDefault(card => card.UniqueID == activeCards.uniqueID).CreateCopy();
                    joinerDeck[i].cardSO = selectedCard;
                    joinerDeck[i].slotNo = i;
                    joinerDeck[i].isPlayercard = false;
                    joinerDeck[i].DisplayCard();
                    i++;
            }
        }else{
            int j = 0;
            foreach(ActiveCards activeCards in MultiplayerManager.Instance.lobbyData.hostActiveCards){
                    CardSO selectedCard = GameManager.instance.cardLists.CardItems.FirstOrDefault(card => card.UniqueID == activeCards.uniqueID).CreateCopy();
                    GameObject instantiatedCardObjects = Instantiate(cardPrefab, deckParent.transform);

                    instantiatedCardObjects.SetActive(false);
                    Card instantiatedCard = instantiatedCardObjects.GetComponent<Card>();
                    instantiatedCard.transform.position = cardSlots[j].position;
                    instantiatedCard.transform.SetParent(cardSlots[j]);
                    availableCardSlots[j] = false;
                    instantiatedCard.cardSO = selectedCard;
                    instantiatedCard.slotNo = j;
                    instantiatedCard.isPlayercard = true;
                    instantiatedCard.DisplayCard();
                    hostDeck.Add(instantiatedCard);
                    j++;
            }
            int i = 0;
            foreach(ActiveCards activeCards in MultiplayerManager.Instance.lobbyData.joinerActiveCards){
                    CardSO selectedCard = GameManager.instance.cardLists.CardItems.FirstOrDefault(card => card.UniqueID == activeCards.uniqueID).CreateCopy();
                    joinerDeck[i].cardSO = selectedCard;
                    joinerDeck[i].slotNo = i;
                    joinerDeck[i].isPlayercard = false;
                    joinerDeck[i].DisplayCard();
                    i++;
            }
        }
    }

    #endregion

    #region - AVAILABLE CARD SLOT CHECKER -

    bool AllSlotsAvailable()
    {
        foreach (bool slotsAvailable in availableCardSlots)
        {
            if (!slotsAvailable)
            {
                return false;
            }
        }

        return true;
    }

    bool AnySlotAvailable()
    {
        foreach (bool slotsAvailable in availableCardSlots)
        {
            if (slotsAvailable)
            {
                return true;
            }
        }

        return false;
    }

    #endregion 

    #region - BUTTONS (SURRENDER, RETURN, BATTLE) -
    
    public void Surrender()
    {
        Debug.Log("You Surrendered!");
        
        ticker = 0;
        MultiplayerManager.Instance.SendSurrender(false, true);
        gameHudLose.SetActive(true);
    }

    public void Return()
    {
        Debug.Log("Return!");

    }

    public void Battle()
    {
        Debug.Log("Battle!");

    }

    #endregion

    #region - TIMER -

    void TimerToEndTurn()
    {
        timerValue -= ticker * Time.deltaTime;

        int seconds = Mathf.FloorToInt(timerValue % 60);

        timerText.text = string.Format("{0:00}", seconds);

        if(timerValue <= 0)
        {
            timerValue = 30;
            ticker = 0;
            ActionData actionData = new ActionData(){
                attackerCardID = "",
                actionType = ActionType.None,
                attackedSlotNo = 0
            };
            MultiplayerManager.Instance.SendAction(MultiplayerManager.Instance.lobbyData, actionData);
            Debug.Log("Player End Turn");
        }
    }

    #endregion

    #region  - COMBAT -

    public async void CardSelect(Card cSelected)
    {
        
        Debug.Log("Card Select Method!!");
        
        // CARD SELECT
        if (cSelected.gameObject.layer == LayerMask.NameToLayer("Your Card"))
        {
            if (selectedCard[0] == null)
            {
                selectedCard[0] = cSelected;
                await selectedCard[0].Select();
            }else{
                await selectedCard[0].Deselect();
                selectedCard[0] = cSelected;    
                await selectedCard[0].Select();
            }
        }
        else if (cSelected.gameObject.layer == LayerMask.NameToLayer("Enemy Card"))
        {
            if(selectedCard[0] != null && selectedCard[1] == null){
                selectedCard[1] = cSelected;
                await selectedCard[1].Select();
            }else{
                Debug.Log("Select a card first");
            }
        }


        // CARD COMBAT 
        if (selectedCard[0] != null && selectedCard[1] != null)
        {
            CardAttack(selectedCard[0], selectedCard[1]);
            await selectedCard[0].Deselect();
            await selectedCard[1].Deselect();
            selectedCard[0] = null;
            selectedCard[1] = null; 

            // ADD END TURN
        }
    }
    public void ToggleTurn(){
        yourTurn = !yourTurn;
        if(yourTurn){
            ticker = 1;
        }else{
            ticker = 0;
        }
    }
    void CardAttack(Card attacker, Card defender)
    {
        if (attacker.cardSO != null && defender.cardSO != null)
        {
            int attackDamage =  attacker.cardSO.cAttack;
            int damageModifier = GetTypeDamageModifier(attacker.cardSO.cType, defender.cardSO.cType);

            int totalDamage = attackDamage + damageModifier;
            totalDamage = Mathf.Max(0, totalDamage);

            timerValue = 30;
            ticker = 0;
            LobbyData lobby = MultiplayerManager.Instance.lobbyData;
            ActionData actionData = new ActionData(){
                attackerCardID = selectedCard[0].cardSO.UniqueID,
                actionType = ActionType.Attack,
                damage = totalDamage,
                attackedSlotNo = selectedCard[1].slotNo
            };
            Debug.Log($"Attacking card in slot {selectedCard[1].slotNo} with {totalDamage} damage");
        
            if (!MultiplayerManager.Instance.isJoiner) {
                lobby.joinerActiveCards[selectedCard[1].slotNo].cardHP = Mathf.Max(0, lobby.joinerActiveCards[selectedCard[1].slotNo].cardHP - totalDamage);
                Debug.Log($"Updated joiner card HP: {lobby.joinerActiveCards[selectedCard[1].slotNo].cardHP}");
            } else {
                lobby.hostActiveCards[selectedCard[1].slotNo].cardHP = Mathf.Max(0, lobby.hostActiveCards[selectedCard[1].slotNo].cardHP - totalDamage);
                Debug.Log($"Updated host card HP: {lobby.hostActiveCards[selectedCard[1].slotNo].cardHP}");
            }
            MultiplayerManager.Instance.SendAction(MultiplayerManager.Instance.lobbyData, actionData);
            yourTurn = !yourTurn;
            Debug.Log("Player End Turn");
            // DEFENDER HEALTH CHECKER 
            // if (defender.cardSO.cHealth <= 0)
            // {
            //     Destroy(defender.gameObject);
            //     Debug.Log($"{defender.name} has been destroyed!");
            // }
        }
        else
        {
            Debug.LogWarning("One or both cards are missing CardSO data.");
        }  
    }

    int GetTypeDamageModifier(string attackerType, string defenderType)
    {
        if (attackerType == "Inferno")
        {
            if (defenderType == "Nature") return 1;    // Strong against Nature
            if (defenderType == "Hydro") return -1;   // Weak against Hydro
        }
        else if (attackerType == "Nature")
        {
            if (defenderType == "Hydro") return 1;    // Strong against Hydro
            if (defenderType == "Inferno") return -1; // Weak against Inferno
        }
        else if (attackerType == "Hydro")
        {
            if (defenderType == "Inferno") return 1;  // Strong against Inferno
            if (defenderType == "Nature") return -1;  // Weak against Nature
        }

        // Neutral interaction (no bonus/penalty)
        return 0;
    }

    #endregion

    void Update()
    {
        if (MultiplayerManager.Instance.isJoiner) {
            LobbyData lobby = MultiplayerManager.Instance.lobbyData;
            deckCountText.text = lobby.joinerCurrentDeck.Count.ToString();
            if(lobby.joinerCurrentDeck.Count == 0 && AreAllCardsBelowHpThreshold(lobby.joinerActiveCards)){
                MultiplayerManager.Instance.SendSurrender(true, false);
                gameHudLose.SetActive(true);
            }
        } else {
            LobbyData lobby = MultiplayerManager.Instance.lobbyData;
            deckCountText.text = lobby.hostCurrentDeck.Count.ToString();
            if(lobby.hostCurrentDeck.Count == 0 && AreAllCardsBelowHpThreshold(lobby.hostActiveCards)){
                MultiplayerManager.Instance.SendSurrender(true, false);
                gameHudLose.SetActive(true);
            }
        }
        TimerToEndTurn();


        // DRAW CARDS IF THERE IS AVAILABLE SLOTS IN THE FIELD
        // if (hostDeck.Count > 0 && AnySlotAvailable())
        // {
        //     DrawCard();
        // }
    }
    public void Win(){
        Return();
    }
    public bool AreAllCardsBelowHpThreshold(List<ActiveCards> activeCards)
    {
        foreach (ActiveCards card in activeCards)
        {
            if (card.cardHP >= 1)
            {
                return false;
            }
        }
        return true;
    }
}
