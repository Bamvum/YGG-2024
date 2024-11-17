using System.Collections.Generic;
using Solana.Unity.SDK;
using Solana.Unity.SDK.Nft;
using Solana.Unity.Wallet;
using TMPro;
using Unisave.Facets;
using UnityEngine;

public class AccountStore : MonoBehaviour
{
    [SerializeField] TextMeshProUGUI publicKey;
    [SerializeField] TextMeshProUGUI balance;
    private void OnEnable(){
        Web3.OnLogin += OnLogin;
        Web3.OnLogout += OnLogout;
        Web3.OnBalanceChange += OnBalanceChange;
        Web3.OnNFTsUpdate += OnNFTsUpdate;
    }

    private async void OnLogin(Account account){
        PlayerUIManager.Instance.OpenLoader();
        publicKey.SetText("Public Key: " + account.PublicKey);
        await this.CallFacet((DatabaseService ds) => ds.CreateAccount(account.PublicKey))
        .Then(response => {
            AccountManager.Instance.playerData = response;
            Debug.Log("Success");
            PlayerUIManager.Instance.CloseLoader();
            PlayerUIManager.Instance.CloseConnection();
            PlayerUIManager.Instance.OpenMainmenu();
        }).Catch(error => 
        {
            PlayerUIManager.Instance.CloseLoader();
        });
        
    }
    private void OnLogout(){
        publicKey.SetText("");
        balance.SetText("");
        PlayerUIManager.Instance.CloseMainmenu();
        PlayerUIManager.Instance.OpenConnection();
    }
    private void OnBalanceChange(double sol){
        balance.SetText("Balance: " + sol.ToString("#.#########"));
    }
    private void OnNFTsUpdate(List<Nft> nft, int total){
        foreach(Nft nftData in nft){
            Debug.Log("NFT Data: " + nftData.metaplexData.data.offchainData.name);
        }

    }
}
