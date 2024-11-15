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

    public void Login(){

    }
}
