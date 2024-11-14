using Unisave.Facades;
using Unisave.Facets;
using Unisave.Broadcasting;
using ESDatabase.Classes;
using System.Diagnostics;

public class RoomManager : Facet
{
    public ChannelSubscription CreateChannel(string roomID, PlayerData playerData){
        var subscription = Broadcast
            .Channel<GameLobby>()
            .CreateRoom(roomID)
            .CreateSubscription();
        return subscription;
    }
    public ChannelSubscription JoinOnlineChannel(string roomID, PlayerData playerData)
    {

        var subscription = Broadcast
            .Channel<GameLobby>()
            .JoinRoom(roomID).CreateSubscription();
        
        // new player in the room broadcast
        Broadcast.Channel<GameLobby>()
            .JoinRoom(roomID)
            .Send(new PlayerJoinedMessage {
                playerName = playerData.gameData.playerName
            });

        return subscription;
    }
    public void SendMessage(string room, string msg , PlayerData playerData)
    {
        // get the authenticated player
        var player = Auth.GetPlayer<PlayerEntity>();

        // send the message into the channel
        Broadcast.Channel<GameLobby>()
            .JoinRoom(room)
            .Send(new ChatMessage {
                playerName = playerData.gameData.playerName,
                message = msg
            });
    }

}
