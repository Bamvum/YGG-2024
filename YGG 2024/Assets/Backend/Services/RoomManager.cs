using Unisave.Facades;
using Unisave.Facets;
using Unisave.Broadcasting;
using ESDatabase.Classes;
using System.Diagnostics;

public class RoomManager : Facet
{

    public ChannelSubscription JoinOnlineChannel(string roomID, PlayerData playerData)
    {

        var subscription = Broadcast
            .Channel<GameLobby>()
            .JoinRoom(roomID)
            .CreateSubscription();
        
        // new player in the room broadcast
        Broadcast.Channel<GameLobby>()
            .JoinRoom(roomID)
            .Send(new PlayerJoinedMessage {
                playerName = playerData.gameData.playerName
            });

        return subscription;
    }
}
