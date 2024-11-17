using Unisave.Facades;
using Unisave.Broadcasting;
using UnityEngine;

using ESDatabase.Classes;
using TMPro;
using Solana.Unity.SDK;

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
            MultiplayerManager.Instance.enemyPlayerData = msg.playerData;
            // MultiplayerManager.Instance.SendPlayerData();
        }
    }
    void ReadyReceive(ReadyMessage readyMessage){
        MultiplayerManager.Instance.enemyReady = readyMessage.isReady;
    }
}
