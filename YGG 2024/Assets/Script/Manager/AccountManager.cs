using System.Threading.Tasks;
using Solana.Unity.SDK;
using Unisave.Facets;
using UnityEngine;

public class AccountManager : MonoBehaviour
{
    [SerializeField] public static AccountManager Instance;
    [SerializeField] public PlayerData playerData;
    private void Awake(){
        if(Instance == null){
            Instance = this;
        }else{
            Destroy(gameObject);
        }
    }
    public void SaveData(){

    }

    public void Logout(){
        Web3.Instance.Logout();
        playerData = null;
    }
    public async static Task SaveData(PlayerData player)
    {

        await FacetClient.CallFacet((DatabaseService facet) => facet.SaveData(player))
        .Then(response =>
        {
            
            Debug.Log(response);
        })
        .Catch(error =>
        {
            Debug.LogError("Failed to save player data: " + error);
        });
    }
}
