using System;
using System.Collections;
using System.Collections.Generic;
using Unisave;
using Unisave.Facets;
using Unisave.Facades;
using Unisave.Authentication.Middleware;
using Unisave.Broadcasting;
using ESDatabase.Classes;

public class RoomManager : Facet
{
    public ChannelSubscription JoinRoom(string room)
    {
        // get the authenticated player
        var player = Auth.GetPlayer<PlayerData>();
        // subscribe the client into the channel
        var subscription = Broadcast
            .Channel<Lobby>()
            .WithParameters(room)
            .CreateSubscription();
        
        // new player in the room broadcast
        Broadcast.Channel<Lobby>()
            .WithParameters(room)
            .Send(new PlayerJoinedMessage {
                playerName = player.gameData.playerName
            });

        return subscription;
    }

}
