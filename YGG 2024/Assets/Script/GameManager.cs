using Assets.PixelHeroes.Scripts.CharacterScrips;
using System.Collections.Generic;
using System;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;
using static CardSOData;
using Solana.Unity.Wallet;

public class GameManager : MonoBehaviour
{
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    public static GameManager instance;
    public CharacterBuilder characterBuilder;
    public Button Editor;


    public event Action<Cards> OnItemsToTransferUpdated;
    public List<Cards> itemsToTransfer = new List<Cards>();
    public int tempindex;

    public CardList cardLists;

    public double PlayerMoney = 0;
    public Text PlayerMoneyText;

    public GameObject floatingTextPrefab; // Reference to the floating text prefab
    public Transform cANVAS;


    void Start()
    {
        
        
        // Editor.onClick.AddListener(OpenEditor);
    }

    public void ShowFloatingText(string text, Color color)
    {
        if (floatingTextPrefab != null && cANVAS != null)
        {
            GameObject floatingText = Instantiate(floatingTextPrefab, transform.position, Quaternion.identity, cANVAS);

            // Set the local position based on the image transform values
            floatingText.transform.localPosition = new Vector3(0, -300, 0); // Use Pos X and Pos Y from the image

            // Get the NotifText component and set the text and color
            NotifText floatingTextComponent = floatingText.GetComponent<NotifText>();
            floatingTextComponent.SetText(text, color);
        }
    }

    public void AddItemToTransfer(Cards item)
    {
        itemsToTransfer.Add(item);
        OnItemsToTransferUpdated?.Invoke(item);
    }
    public async void AddMoney(double amount){
        AccountManager.Instance.playerData.gameData.money += amount;
        await AccountManager.SaveData(AccountManager.Instance.playerData);
    }
    public async void DeductMoney(double amount){
        AccountManager.Instance.playerData.gameData.money -= amount;
        await AccountManager.SaveData(AccountManager.Instance.playerData);
    }
    // Update is called once per frame
    void Update()
    {
        if(AccountManager.Instance.playerData != null){
            PlayerMoneyText.text = AccountManager.Instance.playerData.gameData.money.ToString("#.##");
        }
    }

    public void OpenEditor()
    {
        SceneManager.LoadSceneAsync("YggCharacterEditor", LoadSceneMode.Additive);
    }

    private void Awake()
    {
       
        if (instance == null)
        {

            instance = this;
            DontDestroyOnLoad(gameObject);

        }
        else
        {
            // If another instance already exists, destroy this one
            Destroy(gameObject);
        }
    }

    
}
