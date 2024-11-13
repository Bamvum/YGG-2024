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
}