using Unisave.Facades;
using Unisave.Broadcasting;
using UnityEngine;

using ESDatabase.Classes;

public class PlayerClient : UnisaveBroadcastingClient
{
    private async void OnEnable()
    {
        Debug.Log("Player logged in");        
    }
    public async void CreateLobby(){
        string randomCode = Utilities.GenerateCode(5);
        var subscription = await OnFacet<RoomManager>
            .CallAsync<ChannelSubscription>(
                nameof(RoomManager.JoinOnlineChannel),
                randomCode,
                UnisaveManager.Instance.playerData
            );
            FromSubscription(subscription)
            .Forward<PlayerJoinedMessage>(PlayerJoined)
            .ElseLogWarning();
            Debug.Log(randomCode);
    }

    public async void JoinLobby(){
        var subscription = await OnFacet<RoomManager>
            .CallAsync<ChannelSubscription>(
                nameof(RoomManager.JoinOnlineChannel),
                UnisaveManager.Instance.lobby.text.ToUpper(),
                UnisaveManager.Instance.playerData
            );
        FromSubscription(subscription)
            .Forward<PlayerJoinedMessage>(PlayerJoined)
            .ElseLogWarning();
    }
    /*
    void MyMessageReceived(MyMessage msg)
    {
        // customize the message handling
        
        Debug.Log("MyMessage has been received");
    }
    */
    void PlayerJoined(PlayerJoinedMessage msg)
    {
        // "John joined the room"
        Debug.Log($"{msg.playerName} joined the room");
    }

}
