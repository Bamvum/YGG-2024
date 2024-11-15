using UnityEngine;

public class PlayerUIManager : MonoBehaviour
{
    [SerializeField] public static PlayerUIManager Instance;
    [Header("Main menu UI")]
    [SerializeField] public GameObject connectionMenu;
    [SerializeField] public GameObject mainMenu;
    [Header("Game UI")]
    [SerializeField] public GameObject playerUI;
    [SerializeField] public GameObject multiplayerUI;
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

    public void OpenLink(string link){
        Application.OpenURL(link);
    }
}
