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
            .Room(roomID)
            .CreateSubscription();
        return subscription;
    }
    public ChannelSubscription JoinOnlineChannel(string roomID, PlayerData playerData)
    {

        var subscription = Broadcast
            .Channel<GameLobby>()
            .Room(roomID).CreateSubscription();
        
        // new player in the room broadcast
        Broadcast.Channel<GameLobby>()
            .Room(roomID)
            .Send(new PlayerJoinedMessage {
                playerName = playerData.gameData.playerName,
                playerData = playerData
            });

        return subscription;
    }
    public void SendReady(string roomID, PlayerData playerData, bool isReady)
    {        
        // new player in the room broadcast
        Broadcast.Channel<GameLobby>()
            .Room(roomID)
            .Send(new ReadyMessage {
                playerData = playerData,
                isReady = isReady
            });
    }

    public void SendSwapTurn(string roomID, PlayerData playerData, bool turn)
    {        
        // new player in the room broadcast
        Broadcast.Channel<GameLobby>()
            .Room(roomID)
            .Send(new SwapTurn {
                playerData = playerData,
                turn = turn
            });
    }
    public void SendSurrender(string roomID, PlayerData playerData, bool throughWinComplete, bool throughSurrenderButton)
    {        
        // new player in the room broadcast
        Broadcast.Channel<GameLobby>()
            .Room(roomID)
            .Send(new SurrenderMessage {
                playerData = playerData,
                throughWinComplete = throughWinComplete,
                throughSurrenderButton = throughSurrenderButton
            });
    }
    public void SendAction(string roomID, PlayerData playerData, LobbyData lobbyData, ActionData actionData)
    {        
        // new player in the room broadcast
        Broadcast.Channel<GameLobby>()
            .Room(roomID)
            .Send(new ActionMessage {
                playerData = playerData,
                lobbyData = lobbyData,
                actionData = actionData
            });
    }

    public void SendInGame(string roomID, PlayerData playerData, bool inGame){
        Broadcast.Channel<GameLobby>()
            .Room(roomID)
            .Send(new InGameMessage {
                playerData = playerData,
                inGame = inGame
            });
    }
    public void SendGameStart(string roomID, PlayerData playerData, bool isStarted)
    {        
        // new player in the room broadcast
        Broadcast.Channel<GameLobby>()
            .Room(roomID)
            .Send(new GameStart {
                playerData = playerData,
                gameStarted = isStarted
            });
    }
    public void SendPlayerData(string roomID, PlayerData playerData, LobbyData lobbyData)
    {        
        // new player in the room broadcast
        Broadcast.Channel<GameLobby>()
            .Room(roomID)
            .Send(new SendData {
                playerData = playerData,
                lobbyData = lobbyData
            });
    }
    public void SendMessage(string room, string msg , PlayerData playerData)
    {
        // get the authenticated player
        var player = Auth.GetPlayer<PlayerEntity>();

        // send the message into the channel
        Broadcast.Channel<GameLobby>()
            .Room(room)
            .Send(new ChatMessage {
                playerName = playerData.gameData.playerName,
                message = msg
            });
    }

}
