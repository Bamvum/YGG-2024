using System;
using System.Collections;
using System.Collections.Generic;
using Unisave;
using Unisave.Entities;
using Unisave.Facades;
using Unisave.Broadcasting;
using UnityEngine;
using UnityEngine.UI;
using ESDatabase.Classes;

public class PlayerClient : UnisaveBroadcastingClient
{
    private async void OnEnable()
    {
        Debug.Log("Player logged in");        
    }
    public async void CreateLobby(){
        var subscription = await OnFacet<RoomManager>
        .CallAsync<ChannelSubscription>(
                nameof(RoomManager.JoinRoom),
                Utilities.GenerateCode(5)
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
