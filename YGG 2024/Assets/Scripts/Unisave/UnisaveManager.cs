using ESDatabase.Classes;
using TMPro;
using Unisave.Facets;
using UnityEngine;

public class UnisaveManager : MonoBehaviour
{
    public static UnisaveManager Instance;
    [SerializeField] public PlayerData playerData;
    [SerializeField] string uuid;
    [SerializeField] public TMP_InputField lobby;
    private void Awake(){
        if(Instance == null){
            Instance = this;
        }else{
            Destroy(gameObject);
        }
    }

    public void CreateAccount(string pubkey){
        this.CallFacet((DatabaseService ds) => ds.CreateAccount(pubkey))
        .Then(response => {
            playerData = response;
            Debug.Log(playerData.publicKey);
        });
    }

    public void CreateLobby(){
        Debug.Log(Utilities.GenerateCode(5));
    }

    public void JoinLobby(){
        Debug.Log(lobby.text);
    }
}
