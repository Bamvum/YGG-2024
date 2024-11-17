using System.Collections.Generic;
using ESDatabase.Classes;
using Solana.Unity.SDK;
using Solana.Unity.Wallet;
using UnityEngine;
using UnityEngine.UI;

public class MultiplayerManager : MonoBehaviour
{
    [Header("Lobby Data")]
    public string lobbyCode = "";
    [SerializeField] public bool playerReady = false;
    [SerializeField] public bool enemyReady = false;
    [Header("Player Data")]
    [SerializeField] public PlayerData playerData;
    [SerializeField] public PlayerData enemyPlayerData;
    [Header("Player Cards")]
    [SerializeField] public Text playerPubKey;
    [SerializeField] public Text enemyPubKey;
    [SerializeField] public List<Card> playerCards = new List<Card>();
    [SerializeField] public List<Card> enemyCards = new List<Card>();
    [Header("UIs")]
    [SerializeField] public Sprite ready;
    [SerializeField] public Sprite notReady;
    [SerializeField] public GameObject multiplayerUI;
    [SerializeField] public GameObject lobbyUI;
    [SerializeField] public Text lobbyCodeText;
    [Header("Singleton")]
    public static MultiplayerManager Instance;
    public void Awake(){
        if(Instance == null){
            Instance = this;
        }else{
            Destroy(gameObject);
        }
        lobbyCodeText.text = lobbyCode;
        playerPubKey.text = "Player: " + Web3.Account.PublicKey;
        playerData = AccountManager.Instance.playerData;
        int i = 0;
        foreach(CardData cardData in playerData.gameData.cardDeck){
            Debug.Log(cardData.cardID);
        }
    }

    public void LoadEnemy(PlayerData enemyData){
        playerPubKey.text = "Opponent: " + enemyData.publicKey;
        enemyPlayerData = enemyData;
        int i = 0;
        foreach(CardData cardData in enemyPlayerData.gameData.cardDeck){
            Debug.Log(cardData.cardID);
        }
    }

}
