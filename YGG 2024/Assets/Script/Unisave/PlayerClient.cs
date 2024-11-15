using Unisave.Facades;
using Unisave.Broadcasting;
using UnityEngine;

using ESDatabase.Classes;
using TMPro;

public class PlayerClient : UnisaveBroadcastingClient
{
    private void OnEnable()
    {
        Debug.Log("Player logged in");        
    }
    public async void CreateLobby(){
        UnisaveManager.Instance.lobbyCode = Utilities.GenerateCode(5);
        var subscription = await OnFacet<RoomManager>
            .CallAsync<ChannelSubscription>(
                nameof(RoomManager.JoinOnlineChannel),
                UnisaveManager.Instance.lobbyCode,
                UnisaveManager.Instance.playerData
            );

            FromSubscription(subscription)
            .Forward<PlayerJoinedMessage>(PlayerJoined)
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
            .Forward<PlayerJoinedMessage>(PlayerJoined)
            .ElseLogWarning();
    }
    void ChatMessageReceived(ChatMessage msg)
    {
        // "[John]: Hello people!"
        Debug.Log($"[{msg.playerName}]: {msg.message}");
    }

    void PlayerJoined(PlayerJoinedMessage msg)
    {
        // "John joined the room"
        Debug.Log($"{msg.playerName} joined the room");
    }

}
