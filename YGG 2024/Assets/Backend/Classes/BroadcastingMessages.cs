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
        public bool gameStarted = false;
    }

    public class ReadyMessage : BroadcastingMessage{
        public PlayerData playerData;
        public bool isReady;
    }
    public class SendData : BroadcastingMessage{
        public PlayerData playerData;
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
}