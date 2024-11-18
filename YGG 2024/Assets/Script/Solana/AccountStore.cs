using System.Collections.Generic;
using ESDatabase.Classes;
using Solana.Unity.SDK;
using Solana.Unity.SDK.Nft;
using Solana.Unity.Wallet;
using TMPro;
using Unisave.Facets;
using UnityEngine;

public class AccountStore : MonoBehaviour
{
    [SerializeField] public PlayerClient playerClient;
    private void OnEnable(){
        Web3.OnLogin += OnLogin;
        Web3.OnLogout += OnLogout;
        //Web3.OnNFTsUpdate += OnNFTsUpdate;
    }

    private async void OnLogin(Account account){
        PlayerUIManager.Instance.OpenLoader();
        string generatedUID = Utilities.GenerateUuid();
        if(AccountManager.Instance.priceData.date != null && Utilities.CheckIfLateBy10Minutes(AccountManager.Instance.priceData.date)){
            await this.CallFacet((DatabaseService ds) => ds.InitializeLoginWithPrice(account.PublicKey, generatedUID))
            .Then(async response => {
                AccountManager.Instance.playerData = response.playerData;
                Debug.Log("Success");
                PlayerUIManager.Instance.CloseLoader();
                PlayerUIManager.Instance.CloseConnection();
                PlayerUIManager.Instance.OpenMainmenu();
                playerClient.enabled = true;
                AccountManager.Instance.EntityId = response.playerData.EntityId;
                AccountManager.Instance.uid = generatedUID;
                AccountManager.Instance.priceData.price = response.priceData.price;
                AccountManager.Instance.priceData.date = response.priceData.date;
                
            }).Catch(error => 
            {
                PlayerUIManager.Instance.CloseLoader();
            });
        }else if(AccountManager.Instance.priceData.date != null && !Utilities.CheckIfLateBy10Minutes(AccountManager.Instance.priceData.date)){
            await this.CallFacet((DatabaseService ds) => ds.CreateAccountWithoutPrice(account.PublicKey, generatedUID))
            .Then(async response => {
                AccountManager.Instance.playerData = response.playerData;
                Debug.Log("Success");
                PlayerUIManager.Instance.CloseLoader();
                PlayerUIManager.Instance.CloseConnection();
                PlayerUIManager.Instance.OpenMainmenu();
                playerClient.enabled = true;
                AccountManager.Instance.EntityId = response.playerData.EntityId;
                AccountManager.Instance.uid = generatedUID;
                
            }).Catch(error => 
            {
                PlayerUIManager.Instance.CloseLoader();
            });
        }else{
            await this.CallFacet((DatabaseService ds) => ds.InitializeLoginWithPrice(account.PublicKey, generatedUID))
            .Then(async response => {
                AccountManager.Instance.playerData = response.playerData;
                Debug.Log("Success");
                PlayerUIManager.Instance.CloseLoader();
                PlayerUIManager.Instance.CloseConnection();
                PlayerUIManager.Instance.OpenMainmenu();
                playerClient.enabled = true;
                AccountManager.Instance.EntityId = response.playerData.EntityId;
                AccountManager.Instance.uid = generatedUID;
                AccountManager.Instance.priceData.price = response.priceData.price;
                AccountManager.Instance.priceData.date = response.priceData.date;
                
            }).Catch(error => 
            {
                PlayerUIManager.Instance.CloseLoader();
            });
        }
    }
    private void OnLogout(){
        playerClient.enabled = false;
        
        PlayerUIManager.Instance.CloseMainmenu();
        PlayerUIManager.Instance.OpenConnection();
    }
    // private void OnNFTsUpdate(List<Nft> nft, int total){
    //     foreach(Nft nftData in nft){
    //         Debug.Log("NFT Data: " + nftData.metaplexData.data.offchainData.name);
    //     }

    // }
}
