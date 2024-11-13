using System.Collections.Generic;
using TMPro;
using UnityEngine;
using DG.Tweening;
using Unity.VisualScripting;

public class CardGameManager : MonoBehaviour
{
    [SerializeField] GameObject cardPrefab;
    [SerializeField] GameObject deckParent;
    [SerializeField] CardSO[] cardSOList;
    
    public static CardGameManager instance {get; private set;}

    [SerializeField] List<Card> deck = new List<Card>();
    [SerializeField] Transform[] cardSlots;
    [SerializeField] bool[] availableCardSlots;

    
    [SerializeField] TMP_Text deckCountText;
    [SerializeField] bool yourTurn;

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

    #region  - GENERATE CARDS IN DECK -

    void InstantiateCardDeck()
    {
        // CHANGE NA LANG DIPENDE SA NUMBER NG CARD NA NILAGAY NILA SA DECK (MAXIMUN KASI 9)
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

    void Update()
    {
        deckCountText.text = deck.Count.ToString();
    }

}
