using Assets.PixelHeroes.Scripts.CharacterScrips;
using UnityEngine;

public class GameReference : MonoBehaviour
{
    public static GameReference Instance;
    [SerializeField] public CharacterBuilder characterBuilder;
    private void Awake(){
        if(Instance == null){
            Instance = this;
        }else{
            Destroy(gameObject);
        }
    }
}
