using Solana.Unity.SDK;
using UnityEngine;
using UnityEngine.UI;

public class PremiumModal : MonoBehaviour
{
    [SerializeField] public Text balance;
    private void OnEnable(){
        Web3.OnBalanceChange += OnBalanceChange;
    }
    private void OnBalanceChange(double solBalance)
    {
        balance.text = Utilities.FormatSolana(solBalance);
    }
}
