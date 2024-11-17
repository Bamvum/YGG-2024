using Unisave.Broadcasting;
using UnityEngine;
namespace ESDatabase.Classes
{
    public class ChatMessage : BroadcastingMessage
    {
        public string playerName;
        public string message;
    }

    public class PlayerJoinedMessage : BroadcastingMessage
    {
        public string playerName;
        public PlayerData playerData;
    }
    public class GameStart : BroadcastingMessage
    {
        public PlayerData playerData;
        public bool gameStarted = false;
    }

    public class ReadyMessage : BroadcastingMessage{
        public PlayerData playerData;
        public bool isReady;
    }
    public class SwapTurn : BroadcastingMessage{
        public PlayerData playerData;
        public bool turn;
    }
    public class InGameMessage : BroadcastingMessage{
        public PlayerData playerData;
        public bool inGame;
    }
    public class SendData : BroadcastingMessage{
        public PlayerData playerData;
        public LobbyData lobbyData;
    }
    public class CoinflipMessage : BroadcastingMessage
    {
        public bool isHead;
    }
    public class SurrenderMessage : BroadcastingMessage
    {
        public string message;
        bool isSelf;
    }
    public class WinMessage : BroadcastingMessage
    {
        public string playerName;
        bool isSelf;
    }

    public class ActionMessage : BroadcastingMessage{
        public PlayerData playerData;
        public LobbyData lobbyData;
        public ActionData actionData;
        
        public int target = 0;
    }
}