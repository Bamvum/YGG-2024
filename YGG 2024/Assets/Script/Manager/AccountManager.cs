using System.Threading.Tasks;
using ESDatabase.Classes;
using Solana.Unity.SDK;
using Unisave.Facets;
using UnityEngine;
using UnityEngine.SceneManagement;

public class AccountManager : MonoBehaviour
{
    [SerializeField] public static AccountManager Instance;
    [SerializeField] public PlayerData playerData;
    [SerializeField] public PriceData priceData;
    [SerializeField] public string EntityId;
    [SerializeField] public string uid;
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
        uid = "";
        EntityId = "";
        playerData = null;
        Web3.Instance.Logout();

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
    public void ForceLogout(){
        if(GameReference.Instance != null && !MultiplayerManager.Instance.playerInGame){
            SceneManager.UnloadSceneAsync("TheGame").completed += async (operation) => {
                PlayerUIManager.Instance.playerUI.SetActive(false);
                PlayerUIManager.Instance.mainMenuCamera.SetActive(true);
                PlayerUIManager.Instance.parentMainMenu.SetActive(true);
                Logout();
            };
        }else if(GameReference.Instance != null && MultiplayerManager.Instance != null){
                MultiplayerManager.Instance.SendSurrender(false, true);
                SceneManager.UnloadSceneAsync("Testing Gameplay").completed += async (operation) => {
                    MultiplayerManager.Instance.ClearMultiplayer();
                    SceneManager.UnloadSceneAsync("TheGame").completed += async (operation) => {
                    PlayerUIManager.Instance.playerUI.SetActive(false);
                    PlayerUIManager.Instance.mainMenuCamera.SetActive(true);
                    PlayerUIManager.Instance.parentMainMenu.SetActive(true);
                    Logout();
                };
            };
        }else{
            PlayerUIManager.Instance.playerUI.SetActive(false);
            PlayerUIManager.Instance.mainMenuCamera.SetActive(true);
            PlayerUIManager.Instance.parentMainMenu.SetActive(true);
            Logout();
        }
    }
    public async void CheckSession(string message){
        PlayerData newPlayer = await GetPlayer();
        PlayerData oldPlayer = playerData;

        if(oldPlayer.token.Equals(newPlayer.token)){
            Debug.Log("New login but you are safe");
        }else{
            ForceLogout();
            Debug.Log(message);
        }
    }
    public async Task<PlayerData> GetPlayer()
    {
        PlayerData player = null;
        await FacetClient.CallFacet((DatabaseService facet) => facet.GetPlayerById(EntityId))
        .Then(response => 
        {
            player = response;
        })
        .Catch(error => 
        {
            Debug.LogError("Failed to fetch player data: " + error);
        });
        return player;
    }
    public void GetPrice(){
        FacetClient.CallFacet((DatabaseService facet) => facet.GetPrice())
        .Then(response => 
        {
            priceData.price = response.price;
            priceData.date = response.date;
        })
        .Catch(error => 
        {
            Debug.LogError("Failed to fetch Price: " + error);
        });
    }
}
