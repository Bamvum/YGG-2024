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
        Debug.Log("Player logged in");        
    }
    public async void CreateLobby(){
        MultiplayerManager.Instance.lobbyCode = Utilities.GenerateCode(5);
        var subscription = await OnFacet<RoomManager>
            .CallAsync<ChannelSubscription>(
                nameof(RoomManager.JoinOnlineChannel),
                UnisaveManager.Instance.lobbyCode,
                UnisaveManager.Instance.playerData
            );

            FromSubscription(subscription)
            .Forward<PlayerJoinedMessage>(PlayerJoin)
            .ElseLogWarning();
            Debug.Log(UnisaveManager.Instance.lobbyCode);
    }
    public async void JoinLobby(){
        var subscription = await OnFacet<RoomManager>
            .CallAsync<ChannelSubscription>(
                nameof(RoomManager.JoinOnlineChannel),
                UnisaveManager.Instance.lobby.text.ToUpper(),
                UnisaveManager.Instance.playerData
            );
        UnisaveManager.Instance.lobbyCode = UnisaveManager.Instance.lobby.text.ToUpper();
        FromSubscription(subscription)
            .Forward<PlayerJoinedMessage>(PlayerJoin)
            .ElseLogWarning();
    }

    // Receiver
    void PlayerJoin(PlayerJoinedMessage msg)
    {
        Debug.Log("Player Joined: " + msg.playerData.publicKey);
        if(!msg.playerData.gameData.Equals(Web3.Account.PublicKey)){
            MultiplayerManager.Instance.LoadEnemy(msg.playerData);
            MultiplayerManager.Instance.enemyPlayerData = msg.playerData;
        }
    }
    void ReadyReceive(ReadyMessage readyMessage){
        MultiplayerManager.Instance.enemyReady = readyMessage.isReady;
    }
}
