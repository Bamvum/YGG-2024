using Unisave.Broadcasting;

public class GameLobby : BroadcastingChannel
{
    public SpecificChannel JoinRoom(string roomID)
    {
        return SpecificChannel.From<GameLobby>(roomID);
    }
}