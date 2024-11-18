using Unisave.Broadcasting;

public class GameLobby : BroadcastingChannel
{
    public SpecificChannel CreateRoom(string roomID)
    {
        return SpecificChannel.From<GameLobby>(roomID);
    }
    public SpecificChannel Room(string roomID)
    {
        return SpecificChannel.From<GameLobby>(roomID);
    }
}
public class OnlineChannel : BroadcastingChannel
{
    public SpecificChannel JoinRoom(string entityID)
    {
        return SpecificChannel.From<OnlineChannel>(entityID);
    }
    public SpecificChannel ForPlayer(string entityID)
    {
        return SpecificChannel.From<OnlineChannel>(entityID);
    }
}