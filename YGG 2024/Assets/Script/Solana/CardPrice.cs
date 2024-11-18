using System;
using UnityEngine;
using UnityEngine.UI;

public class CardPrice : MonoBehaviour
{
    [SerializeField] public double pesoPrice;
    [SerializeField] public double solPrice;
    [SerializeField] public double goldAmount;
    [SerializeField] public Text buttonText;
    private void OnEnable(){
        solPrice = Convert.ToDouble(AccountManager.Instance.priceData.price) * ((double)pesoPrice / 50);
        buttonText.text = solPrice.ToString("#.###") + " SOL\nPHP " + pesoPrice;
    }
    private void Update(){
        if(AccountManager.Instance != null){
            solPrice = Convert.ToDouble(AccountManager.Instance.priceData.price) * ((double)pesoPrice / 50);
            buttonText.text = solPrice.ToString("#.###") + "SOL\nPHP " + pesoPrice;
        }
    }
    public async void Buy(){
        bool response = await SolanaUtility.TransferSols((decimal)solPrice);
        if(response){
            GameManager.instance.AddMoney(goldAmount);
            GameManager.instance.ShowFloatingText("Transaction Successful", Color.green);
        }else{
            GameManager.instance.ShowFloatingText("Transaction Reverted", Color.red);
        }
    }
}
