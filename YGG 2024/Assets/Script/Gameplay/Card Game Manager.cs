using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;
using DG.Tweening;

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
    
    [SerializeField] List<Card> deck = new List<Card>();
    [SerializeField] Transform[] cardSlots;
    [SerializeField] bool[] availableCardSlots;

    [Header("HUD/UI")]
    [SerializeField] GameObject gameDoneHUD;
    [SerializeField] Text topPanelText;

    [Header("Flag")]
    [SerializeField] TMP_Text deckCountText;
    [SerializeField] bool yourTurn;

    [Space(10)]
    public Card[] selectedCard = new Card[2];

    [Header("Timer")]
    [SerializeField] TMP_Text timerText;
    [SerializeField] float timerValue;
 
    void Awake()
    {
        instance = this;
    }

    void Start()
    {
        // SET ALL AVAILABLE CARDS SLOT TO TRUE AT THE START OF THE GAME
        for(int i = 0; i < availableCardSlots.Length; i++)
        {
            availableCardSlots[i] = true;
        }

        InstantiateCardDeck();   
        DrawThreeCards();
    }

    #region - DRAW CARDS -

    public void DrawCard()
    {
        // IF (yourTurn = true)
        if(deck.Count >= 1)
        {
            Card randCard = deck[Random.Range(0, deck.Count)];
        
            for (int i = 0; i < availableCardSlots.Length; i++)
            {
                if (availableCardSlots[i] == true)
                {
                    randCard.gameObject.SetActive(true);
                    randCard.handIndex= i;

                    randCard.transform.position = cardSlots[i].position;
                    randCard.transform.SetParent(cardSlots[i]);
                    availableCardSlots[i] = false;
                    deck.Remove(randCard);
                    return;
                }
            }
        }
    }

    public void DrawThreeCards()
    {
        for (int i = 0; i < 3; i++)
        {
            DrawCard();
        }
    }

    #endregion

    #region - GENERATE CARDS IN DECK -

    void InstantiateCardDeck()
    {
        // CHANGE NA LANG DIPENDE SA NUMBER NG CARD NA NILAGAY NILA SA DECK (MAXIMUN KASI 9, RIGHT?)
        for (int i = 0; i < 9; i++)
        {
            GameObject instantiatedCardObjects = Instantiate(cardPrefab, deckParent.transform);

            instantiatedCardObjects.SetActive(false);

            Card instantiatedCard = instantiatedCardObjects.GetComponent<Card>();
            if (instantiatedCardObjects != null)
            {
                if (i < cardSOList.Length)
                {
                    instantiatedCard.cardSO = cardSOList[i]; 
                }

                deck.Add(instantiatedCard);
            }
            else
            {
                Debug.LogWarning("The instantiated object does not contain a Card component.");
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
        
        Time.timeScale = 0;
        
        gameDoneHUD.SetActive(true);
        topPanelText.text = "Nice Try";
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
        timerValue -= 1 * Time.deltaTime;

        int seconds = Mathf.FloorToInt(timerValue % 60);

        timerText.text = string.Format("{0:00}", seconds);

        if(timerValue <= 0)
        {
            timerValue = 30;

            Debug.Log("Player End Turn");
        }
    }

    #endregion

    #region  - COMBAT -

    public void CardSelect(Card cSelected)
    {
        Debug.Log("Card Select Method!!");

        // CARD SELECT
        if (cSelected.gameObject.layer == LayerMask.NameToLayer("Your Card"))
        {
            if (selectedCard[0] == null)
            {
                selectedCard[0] = cSelected;
            }
            else
            {
                Debug.LogWarning("You cannot select your own card as the target!");
            }
        }
        else if (cSelected.gameObject.layer == LayerMask.NameToLayer("Enemy Card"))
        {
            if (selectedCard[1] == null && selectedCard[1] != selectedCard[0])
            {
                selectedCard[1] = cSelected;
                Debug.Log("Second Selected Card: " + cSelected.name);
            }
        }


        // CARD COMBAT 
        if (selectedCard[0] != null && selectedCard[1] != null)
        {
            CardAttack(selectedCard[0], selectedCard[1]);
            
            selectedCard[0] = null;
            selectedCard[1] = null; 

            // ADD END TURN
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

            defender.cardSO.cHealth -= totalDamage;

            Debug.Log($"{attacker.name} dealt {totalDamage} damage to {defender.name}. Remaining health: {defender.cardSO.cHealth}");

            // DEFENDER HEALTH CHECKER 
            if (defender.cardSO.cHealth <= 0)
            {
                Destroy(defender.gameObject);
                Debug.Log($"{defender.name} has been destroyed!");
            }
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
        deckCountText.text = deck.Count.ToString();

        TimerToEndTurn();

        // DECK IS EMPTY AND CARDS SLOTS ARE EMPTY 
        if (deck.Count == 0 && AllSlotsAvailable())
        {
            Debug.Log("Game Over");

            Time.timeScale = 0;

            gameDoneHUD.SetActive(true);
            topPanelText.text = "Nice Try";
            
        }

        // DRAW CARDS IF THERE IS AVAILABLE SLOTS IN THE FIELD
        if (deck.Count > 0 && AnySlotAvailable())
        {
            DrawCard();
        }
    }
}
