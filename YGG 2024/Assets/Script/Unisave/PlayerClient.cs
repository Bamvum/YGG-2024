using Unisave.Facades;
using Unisave.Broadcasting;
using UnityEngine;

using ESDatabase.Classes;
using TMPro;
using Solana.Unity.SDK;
using System.Linq;

public class PlayerClient : UnisaveBroadcastingClient
{
    private void OnEnable()
    {
        Debug.Log("Player Logged in");
    }
    private void OnDisable(){
        Debug.Log("Player Logged out");
    }
    public async void CreateLobby(){
        MultiplayerManager.Instance.lobbyCode = Utilities.GenerateCode(5);
        var subscription = await OnFacet<RoomManager>
            .CallAsync<ChannelSubscription>(
                nameof(RoomManager.JoinOnlineChannel),
                MultiplayerManager.Instance.lobbyCode,
                AccountManager.Instance.playerData
            );

            FromSubscription(subscription)
            .Forward<PlayerJoinedMessage>(PlayerJoin)
            .Forward<ReadyMessage>(ReadyReceive)
            .Forward<SendData>(ReceiveEnemy)
            .Forward<GameStart>(ReceiveStartGame)
            .Forward<InGameMessage>(ReceiveInGame)
            .ElseLogWarning();
            MultiplayerManager.Instance.multiplayerUI.SetActive(false);
            MultiplayerManager.Instance.lobbyUI.SetActive(true);
            MultiplayerManager.Instance.StartLobby();
    }
    public async void JoinLobby(){
        var subscription = await OnFacet<RoomManager>
            .CallAsync<ChannelSubscription>(
                nameof(RoomManager.JoinOnlineChannel),
                MultiplayerManager.Instance.lobbyCodeInput.text.ToUpper(),
                AccountManager.Instance.playerData
            );
        MultiplayerManager.Instance.lobbyCode = MultiplayerManager.Instance.lobbyCodeInput.text.ToUpper();
        FromSubscription(subscription)
            .Forward<PlayerJoinedMessage>(PlayerJoin)
            .Forward<ReadyMessage>(ReadyReceive)
            .Forward<SendData>(ReceiveEnemy)
            .Forward<GameStart>(ReceiveStartGame)
            .Forward<InGameMessage>(ReceiveInGame)
            .ElseLogWarning();
        MultiplayerManager.Instance.multiplayerUI.SetActive(false);
        MultiplayerManager.Instance.lobbyUI.SetActive(true);
        MultiplayerManager.Instance.StartLobby();
    }

    // Receiver
    void PlayerJoin(PlayerJoinedMessage msg)
    {
        Debug.Log("Player Joined: " + msg.playerData.publicKey);
        if(!msg.playerData.publicKey.Equals(AccountManager.Instance.playerData.publicKey.ToString())){
            MultiplayerManager.Instance.LoadEnemy(msg.playerData);
            MultiplayerManager.Instance.lobbyData = new LobbyData();
            Debug.Log("Host Current Active Cards:");
            foreach(ActiveCards activeCards in MultiplayerManager.Instance.lobbyData.hostActiveCards){
                CardSO selectedCard = GameManager.instance.cardLists.CardItems.FirstOrDefault(card => card.UniqueID.Equals(activeCards.uniqueID));
                Debug.Log(selectedCard.cName);
            }
            Debug.Log("Host Current Deck:");
            foreach(ActiveCards activeCards in MultiplayerManager.Instance.lobbyData.hostCurrentDeck){
                CardSO selectedCard = GameManager.instance.cardLists.CardItems.FirstOrDefault(card => card.UniqueID.Equals(activeCards.uniqueID));
                Debug.Log(selectedCard.cName);
            }
            Debug.Log("Joiner Current Active Cards:");
            foreach(ActiveCards activeCards in MultiplayerManager.Instance.lobbyData.joinerActiveCards){
                CardSO selectedCard = GameManager.instance.cardLists.CardItems.FirstOrDefault(card => card.UniqueID.Equals(activeCards.uniqueID));
                Debug.Log(selectedCard.cName);
            }
            Debug.Log("Joiner Current Deck:");
            foreach(ActiveCards activeCards in MultiplayerManager.Instance.lobbyData.joinerCurrentDeck){
                CardSO selectedCard = GameManager.instance.cardLists.CardItems.FirstOrDefault(card => card.UniqueID.Equals(activeCards.uniqueID));
                Debug.Log(selectedCard.cName);
            }
            MultiplayerManager.Instance.enemyPlayerData = msg.playerData;
            MultiplayerManager.Instance.SendPlayerData();
        }
    }
    void ReadyReceive(ReadyMessage readyMessage){
        if(!readyMessage.playerData.publicKey.Equals(AccountManager.Instance.playerData.publicKey.ToString())){
            MultiplayerManager.Instance.SetEnemyReady(readyMessage.isReady);
            if(MultiplayerManager.Instance.playerReady && MultiplayerManager.Instance.enemyReady){
                MultiplayerManager.Instance.StartGame();
                MultiplayerManager.Instance.gameStarted = true;
            }
        }
    }
    void ReceiveInGame(InGameMessage inGameMessage){
        if(!inGameMessage.playerData.publicKey.Equals(AccountManager.Instance.playerData.publicKey.ToString())){
            MultiplayerManager.Instance.SetEnemyInGame(inGameMessage.inGame);
        }
    }
    void ReceiveStartGame(GameStart game){
        MultiplayerManager.Instance.gameStarted = game.gameStarted;
        if(game.gameStarted){
            Debug.Log("Game Started");
        }
    }
    void ReceiveEnemy(SendData data){
        MultiplayerManager.Instance.LoadEnemy(data.playerData);
        MultiplayerManager.Instance.lobbyData = data.lobbyData;
        Debug.Log("Host Current Active Cards:");
        foreach(ActiveCards activeCards in data.lobbyData.hostActiveCards){
            CardSO selectedCard = GameManager.instance.cardLists.CardItems.FirstOrDefault(card => card.UniqueID.Equals(activeCards.uniqueID));
            Debug.Log(selectedCard.cName);
        }
        Debug.Log("Host Current Deck:");
        foreach(ActiveCards activeCards in data.lobbyData.hostCurrentDeck){
            CardSO selectedCard = GameManager.instance.cardLists.CardItems.FirstOrDefault(card => card.UniqueID.Equals(activeCards.uniqueID));
            Debug.Log(selectedCard.cName);
        }
        Debug.Log("Joiner Current Active Cards:");
        foreach(ActiveCards activeCards in data.lobbyData.joinerActiveCards){
            CardSO selectedCard = GameManager.instance.cardLists.CardItems.FirstOrDefault(card => card.UniqueID.Equals(activeCards.uniqueID));
            Debug.Log(selectedCard.cName);
        }
        Debug.Log("Joiner Current Deck:");
        foreach(ActiveCards activeCards in data.lobbyData.joinerCurrentDeck){
            CardSO selectedCard = GameManager.instance.cardLists.CardItems.FirstOrDefault(card => card.UniqueID.Equals(activeCards.uniqueID));
            Debug.Log(selectedCard.cName);
        }
    }
}
