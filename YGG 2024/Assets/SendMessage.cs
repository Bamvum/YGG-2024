using TMPro;
using Unisave.Facets;
using UnityEngine;

public class SendMessage : MonoBehaviour
{
    [SerializeField] public TMP_InputField lobby;
    
    public void Send(){
        UnisaveManager.Instance.gameObject.GetComponent<PlayerClient>().SendMessage(lobby.text);
        this.CallFacet((RoomManager rm) => rm.SendMessage(UnisaveManager.Instance.lobbyCode, lobby.text, UnisaveManager.Instance.playerData));
    }
}
