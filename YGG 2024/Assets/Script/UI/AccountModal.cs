using System.Collections.Generic;
using System.Linq;
using Solana.Unity.SDK;
using Solana.Unity.SDK.Nft;
using UnityEngine;
using UnityEngine.UI;

public class AccountModal : MonoBehaviour
{
    [SerializeField] public Text publicKey;
    [SerializeField] public Text balance;
    [SerializeField] public CardsInvItem prefab;
    [SerializeField] public RectTransform population;
    private void OnEnable(){
        publicKey.text = Web3.Account.PublicKey.ToString();
        Web3.OnBalanceChange += OnBalanceChange;
        Web3.OnNFTsUpdate += OnNFTsUpdate;
    }
    private void OnDisable(){
        ClearContent(population);
    }
    private void OnBalanceChange(double solBalance)
    {
        balance.text = Utilities.FormatSolana(solBalance);
    }
    private void OnNFTsUpdate(List<Nft> nfts, int total)
    {
        ClearContent(population);
        if(nfts.Count > 0){
            foreach(Nft n in nfts){
                if(n.metaplexData.data.offchainData.name.Equals("Card Rush")){
                    CardSO selectedCard = GameManager.instance.cardLists.CardItems.FirstOrDefault(card => card.cName == n.metaplexData.data.offchainData.attributes[0].value).CreateCopy(); 
                    CardsInvItem uiItem = Instantiate(prefab, Vector3.zero, Quaternion.identity);
                    uiItem.transform.SetParent(population);
                    uiItem.SetData(selectedCard.cImage, 1, selectedCard.cName, selectedCard.cDescription, selectedCard.cType, false);
                }
            }
        }
    }
    public static void ClearContent(RectTransform cPanel)
        {
                foreach (Transform child in cPanel)
                {
                        Destroy(child.gameObject);
                }
        }
}
