using Unisave.Facades;
using Unisave.Broadcasting;
using UnityEngine;

using ESDatabase.Classes;
using TMPro;
using Solana.Unity.SDK;
using System.Linq;
using UnityEngine.SceneManagement;
using System;

public class PlayerClient : UnisaveBroadcastingClient
{
    private void OnEnable()
    {
        Debug.Log("Connected to Game Server");
    }
    private void OnDisable(){
        Debug.Log("Disconnected to Game Server");
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
            .Forward<ActionMessage>(ReceiveAction)
            .Forward<SurrenderMessage>(ReceiveSurrender)
            .ElseLogWarning();
            MultiplayerManager.Instance.multiplayerUI.SetActive(false);
            MultiplayerManager.Instance.lobbyUI.SetActive(true);
            MultiplayerManager.Instance.StartLobby();
            PlayerUIManager.Instance.CloseLoader();
    }
    public async void JoinLobby(){
        var subscription = await OnFacet<RoomManager>
            .CallAsync<ChannelSubscription>(
                nameof(RoomManager.JoinOnlineChannel),
                MultiplayerManager.Instance.lobbyCodeInput.text.ToUpper(),
                AccountManager.Instance.playerData
            );
        MultiplayerManager.Instance.lobbyCode = MultiplayerManager.Instance.lobbyCodeInput.text.ToUpper();
        MultiplayerManager.Instance.isJoiner = true;
        FromSubscription(subscription)
            .Forward<PlayerJoinedMessage>(PlayerJoin)
            .Forward<ReadyMessage>(ReadyReceive)
            .Forward<SendData>(ReceiveEnemy)
            .Forward<GameStart>(ReceiveStartGame)
            .Forward<InGameMessage>(ReceiveInGame)
            .Forward<ActionMessage>(ReceiveAction)
            .Forward<SurrenderMessage>(ReceiveSurrender)
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
            MultiplayerManager.Instance.lobbyData = new LobbyData(MultiplayerManager.Instance.playerDeck, MultiplayerManager.Instance.enemyDeck);
            // Debug.Log("Host Current Active Cards:");
            // foreach(ActiveCards activeCards in MultiplayerManager.Instance.lobbyData.hostActiveCards){
            //     CardSO selectedCard = GameManager.instance.cardLists.CardItems.FirstOrDefault(card => card.UniqueID.Equals(activeCards.uniqueID));
            //     Debug.Log(selectedCard.cName);
            // }
            // Debug.Log("Host Current Deck:");
            // foreach(ActiveCards activeCards in MultiplayerManager.Instance.lobbyData.hostCurrentDeck){
            //     CardSO selectedCard = GameManager.instance.cardLists.CardItems.FirstOrDefault(card => card.UniqueID.Equals(activeCards.uniqueID));
            //     Debug.Log(selectedCard.cName);
            // }
            Debug.Log("Joiner Current Active Cards:");
            foreach(ActiveCards activeCards in MultiplayerManager.Instance.lobbyData.joinerActiveCards){
                CardSO selectedCard = GameManager.instance.cardLists.CardItems.FirstOrDefault(card => card.UniqueID.Equals(activeCards.uniqueID)).CreateCopy();
                Debug.Log(activeCards.cardHP);
            }
            Debug.Log("Joiner Current Deck:");
            foreach(ActiveCards activeCards in MultiplayerManager.Instance.lobbyData.joinerCurrentDeck){
                CardSO selectedCard = GameManager.instance.cardLists.CardItems.FirstOrDefault(card => card.UniqueID.Equals(activeCards.uniqueID)).CreateCopy();
                Debug.Log(activeCards.cardHP);
            }
            
            MultiplayerManager.Instance.enemyPlayerData = msg.playerData;
            MultiplayerManager.Instance.SendPlayerData();
        }
    }
    // NANDITO YUNG START GAME NA ISA
    void ReadyReceive(ReadyMessage readyMessage){
        if(!readyMessage.playerData.publicKey.Equals(AccountManager.Instance.playerData.publicKey.ToString())){
            MultiplayerManager.Instance.SetEnemyReady(readyMessage.isReady);
            if(MultiplayerManager.Instance.playerReady && MultiplayerManager.Instance.enemyReady){
                MultiplayerManager.Instance.lobbyUI.SetActive(false);
                PlayerUIManager.Instance.gameCamera.SetActive(false);
                PlayerUIManager.Instance.OpenLoader();
                MultiplayerManager.Instance.StartGame();
                MultiplayerManager.Instance.gameStarted = true;
                ProceedGame();
            }
        }
    }
    void ReceiveInGame(InGameMessage inGameMessage){
        if(!inGameMessage.playerData.publicKey.Equals(AccountManager.Instance.playerData.publicKey.ToString())){
            MultiplayerManager.Instance.SetEnemyInGame(inGameMessage.inGame);
            if(MultiplayerManager.Instance.playerInGame && MultiplayerManager.Instance.enemyInGame){
                PlayerUIManager.Instance.CloseLoader();
            }
        }
    }
    // NANDITO YUNG START GAME RECEIVE
    void ReceiveStartGame(GameStart game){
        if(!game.playerData.publicKey.Equals(AccountManager.Instance.playerData.publicKey.ToString())){
            MultiplayerManager.Instance.lobbyUI.SetActive(false);
            PlayerUIManager.Instance.gameCamera.SetActive(false);
            PlayerUIManager.Instance.OpenLoader();
            MultiplayerManager.Instance.gameStarted = game.gameStarted;
            if(game.gameStarted){
                ProceedGame();
            }
        }
    }
    void ReceiveEnemy(SendData data){
        if(!data.playerData.publicKey.Equals(AccountManager.Instance.playerData.publicKey.ToString())){
            MultiplayerManager.Instance.LoadEnemy(data.playerData);
            MultiplayerManager.Instance.lobbyData = data.lobbyData;
            // Debug.Log("Host Current Active Cards:");
            // foreach(ActiveCards activeCards in data.lobbyData.hostActiveCards){
            //     CardSO selectedCard = GameManager.instance.cardLists.CardItems.FirstOrDefault(card => card.UniqueID.Equals(activeCards.uniqueID));
            //     Debug.Log(selectedCard.cName);
            // }
            // Debug.Log("Host Current Deck:");
            // foreach(ActiveCards activeCards in data.lobbyData.hostCurrentDeck){
            //     CardSO selectedCard = GameManager.instance.cardLists.CardItems.FirstOrDefault(card => card.UniqueID.Equals(activeCards.uniqueID));
            //     Debug.Log(selectedCard.cName);
            // }
            // Debug.Log("Joiner Current Active Cards:");
            // foreach(ActiveCards activeCards in MultiplayerManager.Instance.lobbyData.joinerActiveCards){
            //     CardSO selectedCard = GameManager.instance.cardLists.CardItems.FirstOrDefault(card => card.UniqueID.Equals(activeCards.uniqueID)).CreateCopy();
            //     Debug.Log(activeCards.cardHP);
            // }
            // Debug.Log("Joiner Current Deck:");
            // foreach(ActiveCards activeCards in MultiplayerManager.Instance.lobbyData.joinerCurrentDeck){
            //     CardSO selectedCard = GameManager.instance.cardLists.CardItems.FirstOrDefault(card => card.UniqueID.Equals(activeCards.uniqueID)).CreateCopy();
            //     Debug.Log(activeCards.cardHP);
            // }
        }   
    }
    void ReceiveAction(ActionMessage actionMessage){
        MultiplayerManager.Instance.lobbyData = actionMessage.lobbyData;

        if(!actionMessage.playerData.publicKey.Equals(AccountManager.Instance.playerData.publicKey.ToString())){
            CardGameManager.instance.ToggleTurn();
        }
    }
    void ReceiveSurrender(SurrenderMessage surrenderMessage){
        if(!surrenderMessage.playerData.publicKey.Equals(AccountManager.Instance.playerData.publicKey.ToString())){
            CardGameManager.instance.gameHudWin.SetActive(true);
            PlayerData playerData = AccountManager.Instance.playerData;
            if(surrenderMessage.throughWinComplete){
                GameManager.instance.AddMoney(2000);
            }else if(surrenderMessage.throughSurrenderButton){
                GameManager.instance.AddMoney(500);
            }
        }else{
            if(AccountManager.Instance.playerData.gameData.money < 500){
                GameManager.instance.DeductMoney(AccountManager.Instance.playerData.gameData.money);
            }else{
                GameManager.instance.DeductMoney(500);
            }
        }
    }
    public void ProceedGame(){
        SceneManager.LoadSceneAsync("Testing Gameplay", LoadSceneMode.Additive).completed += async (operation) => {
            MultiplayerManager.Instance.playerInGame = true;
            PlayerUIManager.Instance.createLobby.gameObject.SetActive(false);
            MultiplayerManager.Instance.SendInGame();
            if(MultiplayerManager.Instance.playerInGame && MultiplayerManager.Instance.enemyInGame){
                PlayerUIManager.Instance.CloseLoader();
            }
        };
    }
}
