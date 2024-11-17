using System.Collections.Generic;
using System.Linq;
using ESDatabase.Classes;
using Solana.Unity.SDK;
using Solana.Unity.Wallet;
using Unisave.Facets;
using UnityEngine;
using UnityEngine.UI;
using static CardSOData;

public class MultiplayerManager : MonoBehaviour
{
    [Header("Lobby Data")]
    public string lobbyCode = "";
    [SerializeField] public LobbyData lobbyData;
    [SerializeField] public bool playerReady = false;
    [SerializeField] public bool enemyReady = false;
    [SerializeField] public bool playerInGame = false;
    [SerializeField] public bool enemyInGame = false;
    [SerializeField] public bool gameStarted = false;
    [SerializeField] public bool isJoiner = false;
    [Header("Player Data")]
    [SerializeField] public PlayerData playerData;
    [SerializeField] public PlayerData enemyPlayerData;
    [Header("Player Cards")]
    [SerializeField] public Text playerPubKey;
    [SerializeField] public Text enemyPubKey;
    [SerializeField] public List<Card> playerCards = new List<Card>();
    [SerializeField] public List<Card> enemyCards = new List<Card>();
    [SerializeField] public List<ActiveCards> playerDeck = new List<ActiveCards>{null, null, null, null, null, null};
    [SerializeField] public List<ActiveCards> enemyDeck = new List<ActiveCards>{null, null, null, null, null, null};
    [Header("UIs")]
    [SerializeField] public Image readyImagePlayer;
    [SerializeField] public Image readyImageEnemy;
    [SerializeField] public Sprite ready;
    [SerializeField] public Sprite notReady;
    [SerializeField] public GameObject multiplayerUI;
    [SerializeField] public GameObject lobbyUI;
    [SerializeField] public Text lobbyCodeText;
    [SerializeField] public InputField lobbyCodeInput;
    [Header("Singleton")]
    public static MultiplayerManager Instance;
    public void Awake(){
        if(Instance == null){
            Instance = this;
        }else{
            Destroy(gameObject);
        }
    }
    public void StartLobby(){
        lobbyCodeText.text = "Lobby Code: " + lobbyCode;
        playerPubKey.text = "Player: " + Web3.Account.PublicKey;
        playerData = AccountManager.Instance.playerData;
        int i = 0;
        foreach(CardData cardData in playerData.gameData.cardDeck){
            if(cardData != null){
                CardSO selectedCard = GameManager.instance.cardLists.CardItems.FirstOrDefault(card => card.UniqueID.Equals(cardData.cardID)).CreateCopy();
                ActiveCards actCard = new ActiveCards();
                actCard.uniqueID = selectedCard.UniqueID;
                actCard.cardHP = selectedCard.cHealth;
                playerDeck[i] = actCard;
                playerCards[i].cardSO = selectedCard;
                playerCards[i].DisplayCard();
                playerCards[i].gameObject.SetActive(true);
            }
            i++;
        }
    }

    public void LoadEnemy(PlayerData enemyData){
        enemyPubKey.text = "Opponent: " + enemyData.publicKey;
        enemyPlayerData = enemyData;

        int i = 0;
        foreach(CardData cardData in enemyPlayerData.gameData.cardDeck){
            if(cardData != null){
                CardSO selectedCard = GameManager.instance.cardLists.CardItems.FirstOrDefault(card => card.UniqueID.Equals(cardData.cardID)).CreateCopy();
                ActiveCards actCard = new ActiveCards();
                actCard.uniqueID = selectedCard.UniqueID;
                actCard.cardHP = selectedCard.cHealth;
                enemyDeck[i] = actCard;
                enemyCards[i].cardSO = selectedCard;
                enemyCards[i].DisplayCard();
                enemyCards[i].gameObject.SetActive(true);
            }
            i++;
        }
    }
    public void SetPlayerReady(){
        if(enemyPlayerData == null) {
            Debug.Log("Wait for opponent to set ready");
            return;
        }
        if(gameStarted) {
            Debug.Log("Game is Starting you can't unready!");
            return;
        }
        playerReady = !playerReady;
        if(playerReady){
            readyImagePlayer.sprite = ready;
        }else{
            readyImagePlayer.sprite = notReady;
        }
        SendReady();
    }
    public void SetEnemyReady(bool isReady){
        enemyReady = isReady;
        if(enemyReady){
            readyImageEnemy.sprite = ready;
        }else{
            readyImageEnemy.sprite = notReady;
        }
    }
    public void SetEnemyInGame(bool inGame){
        enemyInGame = inGame;
    }
    public void StartGame(){
        this.CallFacet((RoomManager rm) => rm.SendGameStart(lobbyCode, AccountManager.Instance.playerData, true));
    }
    public void SendReady(){
        this.CallFacet((RoomManager rm) => rm.SendReady(lobbyCode, AccountManager.Instance.playerData, playerReady));
    }
    public void SendAction(LobbyData lobbyData, ActionData actionData){
        this.CallFacet((RoomManager rm) => rm.SendAction(lobbyCode, AccountManager.Instance.playerData, lobbyData, actionData));
    }
    public void SendInGame(){
        this.CallFacet((RoomManager rm) => rm.SendInGame(lobbyCode, AccountManager.Instance.playerData, true));
    }
    public void SendPlayerData(){
        this.CallFacet((RoomManager rm) => rm.SendPlayerData(lobbyCode, AccountManager.Instance.playerData, lobbyData));
    }
}
