using Assets.PixelHeroes.Scripts.CharacterScrips;
using UnityEngine;
using UnityEngine.UI;

public class GameReference : MonoBehaviour
{
    public static GameReference Instance;
    [SerializeField] public CharacterBuilder characterBuilder;
    [SerializeField] public GameObject gameCamera;
    [SerializeField] public Button createGame;
    private void Awake(){
        if(Instance == null){
            Instance = this;
        }else{
            Destroy(gameObject);
        }
    }

    public void OpenMultiplayer(){
        PlayerUIManager.Instance.playerUI.SetActive(false);
        MultiplayerManager.Instance.multiplayerUI.SetActive(true);
    }

}
