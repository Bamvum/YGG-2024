using System.Linq;
using Assets.PixelHeroes.Scripts.CharacterScrips;
using Assets.PixelHeroes.Scripts.EditorScripts;
using ESDatabase.Classes;
using Solana.Unity.Soar.Accounts;
using UnityEngine;
using UnityEngine.SceneManagement;

public class PlayerUIManager : MonoBehaviour
{
    [SerializeField] public static PlayerUIManager Instance;
    [Header("Main menu UI")]
    [SerializeField] public GameObject parentMainMenu;
    [SerializeField] public GameObject connectionMenu;
    [SerializeField] public GameObject mainMenu;
    [Header("Game UI")]
    [SerializeField] public GameObject playerUI;
    [SerializeField] public GameObject multiplayerUI;
    [SerializeField] public GameObject characterBuilder;
    [Header("Cameras")]
    [SerializeField] public GameObject mainMenuCamera;
    [Header("Scripts")]
    [SerializeField] public CharacterEditor characterEditor;
    [SerializeField] public CardsController cardsController;
    private void Awake(){
        if(Instance == null){
            Instance = this;
            connectionMenu.SetActive(true);
        }else{
            Destroy(gameObject);
        }
    }
    // Main Menu
    public void OpenMainmenu(){
        mainMenu.SetActive(true);
    }
    public void CloseMainmenu(){
        mainMenu.GetComponent<DoMove>().Close();
    }
    // Connection Menu
    public void OpenConnection(){
        connectionMenu.SetActive(true);
    }
    public void CloseConnection(){
        connectionMenu.GetComponent<DoMove>().Close();
    }
    // Play Game
    public void PlayGame(){
        PlayerData playerData = AccountManager.Instance.playerData;
        parentMainMenu.SetActive(false);
        if(playerData.gameData.isNew){
            characterBuilder.SetActive(true);
        }else{
            ProceedGame();
        }
    }

    public void ProceedGame(){
        mainMenuCamera.SetActive(false);
        SceneManager.LoadSceneAsync("TheGame", LoadSceneMode.Additive).completed += async (operation) => {
            GameManager.instance.characterBuilder = GameReference.Instance.characterBuilder;
            // characterEditor.Rebuild();
            characterBuilder.SetActive(false);
            PlayerData playerData = AccountManager.Instance.playerData;

            PlayerSkin playerSkin = playerData.gameData.playerSkin;
            if(playerData.gameData.isNew){
                playerSkin.head = characterEditor.CharacterBuilder.Head;
                playerSkin.body = characterEditor.CharacterBuilder.Body;
                playerSkin.hair = characterEditor.CharacterBuilder.Hair;
                playerSkin.armor = characterEditor.CharacterBuilder.Armor;
            }
            // GameManager.instance.characterBuilder.Head = layers["Head"];
            // GameManager.instance.characterBuilder.Body = layers["Body"];
            // GameManager.instance.characterBuilder.Hair = layers["Hair"];
            // GameManager.instance.characterBuilder.Armor = layers["Armor"];
            // GameManager.instance.characterBuilder.Helmet = layers["Helmet"];
            // GameManager.instance.characterBuilder.Weapon = layers["Weapon"];
            // GameManager.instance.characterBuilder.Shield = layers["Shield"];
            // GameManager.instance.characterBuilder.Cape = layers["Cape"];
            // GameManager.instance.characterBuilder.Back = layers["Back"];
            playerData.gameData.isNew = false;
            GameManager.instance.characterBuilder.Head = playerSkin.head;
            GameManager.instance.characterBuilder.Body = playerSkin.body;
            GameManager.instance.characterBuilder.Hair = playerSkin.hair;
            GameManager.instance.characterBuilder.Armor = playerSkin.armor;
            GameManager.instance.characterBuilder.Rebuild();
            cardsController.LoadDatabase();
            await AccountManager.SaveData(playerData);
            playerUI.SetActive(true);
            //GameManager.instance.characterBuilder.Rebuild(layer?.Name);
        };
    }
    public void OpenLink(string link){
        Application.OpenURL(link);
    }
    private void OnSceneLoaded(AsyncOperation operation)
    {

    }
}
