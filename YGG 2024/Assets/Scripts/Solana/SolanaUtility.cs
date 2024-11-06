using Solana.Unity.SDK;
using UnityEngine;

public class SolanaUtility : MonoBehaviour
{
    


    // Login Accounts
    [ContextMenu("MedinaAccount")]
    public async void MedinaAccount(){
        await Web3.Instance.CreateAccount("47Kj963sjZo3UwRauVXXnCpnTFwuJ5x7h6SbdAgpQWW2sD9BKFaztqnJoutJxAhhcYwThVKpwwtV4ufYAPa5PGoL", "A704Gods");
    }
    [ContextMenu("VumAccount")]
    public async void VumAccount(){
        await Web3.Instance.CreateAccount("5zk85zcrvWRNLqTX1svhXq2RKH4sAoWZy1vSdx75kj3WkdYm9dJEShaDJbALavtYbeYgzwLMFLEMgiLWrS3CF4Xo", "A704Gods");
    }
    [ContextMenu("KyleAccount")]
    public async void KyleAccount(){
        await Web3.Instance.CreateAccount("4e6Sk14LQfb9xzUjGTjxNYLzN4x1SQdojyAVu92cLEUNsfYK4mXeJjLYz9W5PcK2hHhmjgJNmWBcKZNoDDUPC2wk", "A704Gods");
    }
    [ContextMenu("JVAccount")]
    public async void JVAccount(){
        await Web3.Instance.CreateAccount("5nD46oP3g54DmxcU3SFtnprVYKzn2j6dhuL2FaRkymWbA6vvxY2tas173H6Z37qaezKazR523PMMGh6MwDybaZFn", "A704Gods");
    }
    [ContextMenu("AJAccount")]
    public async void AJAccount(){
        await Web3.Instance.CreateAccount("5nD46oP3g54DmxcU3SFtnprVYKzn2j6dhuL2FaRkymWbA6vvxY2tas173H6Z37qaezKazR523PMMGh6MwDybaZFn", "A704Gods");
    }

    [ContextMenu("Logout")]
    public void Logout(){
        Web3.Wallet.Logout();
    }
}
