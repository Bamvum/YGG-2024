using Unisave.Broadcasting;

public class GameLobby : BroadcastingChannel
{
    public SpecificChannel CreateRoom(string roomID)
    {
        return SpecificChannel.From<GameLobby>(roomID);
    }
    public SpecificChannel JoinRoom(string roomID)
    {
        return SpecificChannel.From<GameLobby>(roomID);
    }
}