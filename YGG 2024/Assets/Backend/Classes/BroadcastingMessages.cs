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
    }

    public class ReadyMessage : BroadcastingMessage{
        public string playerName;
        public bool isReady = false;
    }
    public class CoinflipMessage : BroadcastingMessage
    {
        public bool isHead;
    }
    public class SurrenderMessage : BroadcastingMessage
    {
        public string message;
        bool isSelf = false;
    }
    public class WinMessage : BroadcastingMessage
    {
        public string playerName;
        bool isSelf = false;
    }
}