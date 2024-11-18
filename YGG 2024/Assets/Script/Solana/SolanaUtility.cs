using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Solana.Unity.Metaplex.NFT.Library;
using Solana.Unity.Metaplex.Utilities;
using Solana.Unity.Programs;
using Solana.Unity.Rpc.Builders;
using Solana.Unity.Rpc.Models;
using Solana.Unity.Rpc.Types;
using Solana.Unity.SDK;
using Solana.Unity.SDK.Nft;
using Solana.Unity.Wallet;
using UnityEngine;

public class SolanaUtility : MonoBehaviour
{
    public static PublicKey bank = new PublicKey("DuMTi4y6t9p3Zoa4HgZB9WBTzQYas2B2hVSQbZ8veppi");
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
        await Web3.Instance.CreateAccount("NRfeXStpHdurR5bqRfb6VkA9iLiPdwGC9UvgoTGy9WwmDNq6wcmT87rVtur6AdUnamVM1XzzFAC89DeGWGaeegS", "A704Gods");
    }

    [ContextMenu("Logout")]
    public void Logout(){
        Web3.Wallet.Logout();
    }    
    public static ulong ConvertSolToLamports(decimal sol){
        decimal lamportsPerSol = 1_000_000_000m;
        return (ulong)(sol * lamportsPerSol);
    }
    public static async Task<bool> TransferSols(decimal amount){
        ulong lamports = ConvertSolToLamports(amount);
        var transaction = new Transaction
        {
            RecentBlockHash = await Web3.Instance.WalletBase.GetBlockHash(),
            FeePayer = Web3.Instance.WalletBase.Account.PublicKey,
            Instructions = new List<TransactionInstruction>
            {

                SystemProgram.Transfer(
                    Web3.Instance.WalletBase.Account.PublicKey,
                    bank,
                    lamports),
            },
            Signatures = new List<SignaturePubKeyPair>()
        };

            var result = await Web3.Instance.WalletBase.SignAndSendTransaction(transaction);
            if(result.WasSuccessful){
                
                Debug.Log("https://explorer.solana.com/tx/" + result.Result + "?cluster=" + Web3.Wallet.RpcCluster.ToString().ToLower());
                return true;
            }else{
                return false;
            }
    }
    public static async Task<string> BurnToken(Nft nft){
            try
            {
                var blockHash = await Web3.Rpc.GetLatestBlockHashAsync();
                // Create the burn instruction
                var associatedTokenAccount = AssociatedTokenAccountProgram
                .DeriveAssociatedTokenAccount(Web3.Account, new PublicKey(nft.metaplexData.data.mint));
                var transaction = new Transaction
                {
                    RecentBlockHash = await Web3.Instance.WalletBase.GetBlockHash(),
                    FeePayer = Web3.Instance.WalletBase.Account.PublicKey,
                    Instructions = new List<TransactionInstruction>
                    {

                        TokenProgram.Burn(
                            associatedTokenAccount,
                            new PublicKey(nft.metaplexData.data.mint),
                            1,
                            Web3.Account),
                    },
                    Signatures = new List<SignaturePubKeyPair>()
                };
                var result = await Web3.Instance.WalletBase.SignAndSendTransaction(transaction);

                if (!result.WasSuccessful)
                {
                    Debug.LogError($"Failed to burn NFT: {result.Reason}");
                    return "failed";
                }
                else
                {
                    return "https://explorer.solana.com/tx/" + result.Result + "?cluster=" + Web3.Wallet.RpcCluster.ToString().ToLower();
                }
            }
            catch (Exception ex)
            {
                Debug.LogError($"Error burning NFT: {ex.Message}");
                return "error";
            }
            //var sendResult = await Web3.Rpc.SendTransactionAsync(Convert.ToBase64String(transaction.Serialize()));
    }
    
    public static async Task<NFTResponse> MintNFT(CardSO cardSO, decimal price)
    {
        var mint = new Account();
        var associatedTokenAccount = AssociatedTokenAccountProgram
            .DeriveAssociatedTokenAccount(Web3.Account, mint.PublicKey);
        

        var metadata = new Metadata()
        {
            name = "Eagle's Shadow",
            symbol = "ESS",
            uri = cardSO.ardriveLink,
            sellerFeeBasisPoints = 0,
            creators = new List<Creator> { new(Web3.Account.PublicKey, 100, true) }
        };
        ulong lamports = ConvertSolToLamports(price);
        var blockHash = await Web3.Rpc.GetLatestBlockHashAsync();
        var minimumRent = await Web3.Rpc.GetMinimumBalanceForRentExemptionAsync(TokenProgram.MintAccountDataSize);
        var transaction = new TransactionBuilder()
            .SetRecentBlockHash(blockHash.Result.Value.Blockhash)
            .SetFeePayer(Web3.Account)
            .AddInstruction(
                SystemProgram.CreateAccount(
                    Web3.Account,
                    mint.PublicKey,
                    minimumRent.Result,
                    TokenProgram.MintAccountDataSize,
                    TokenProgram.ProgramIdKey))
            .AddInstruction(
                TokenProgram.InitializeMint(
                    mint.PublicKey,
                    0,
                    Web3.Account,
                    Web3.Account))
            .AddInstruction(
                AssociatedTokenAccountProgram.CreateAssociatedTokenAccount(
                    Web3.Account,
                    Web3.Account,
                    mint.PublicKey))
            .AddInstruction(
                TokenProgram.MintTo(
                    mint.PublicKey,
                    associatedTokenAccount,
                    1,
                    Web3.Account))
            .AddInstruction(SystemProgram.Transfer(
                    Web3.Instance.WalletBase.Account.PublicKey,
                    bank,
                    lamports)
            )
            .AddInstruction(MetadataProgram.CreateMetadataAccount(
                PDALookup.FindMetadataPDA(mint), 
                mint.PublicKey, 
                Web3.Account, 
                Web3.Account, 
                Web3.Account.PublicKey, 
                metadata,
                TokenStandard.NonFungible, 
                true, 
                true, 
                null,
                metadataVersion: MetadataVersion.V3))
            .AddInstruction(MetadataProgram.CreateMasterEdition(
                    maxSupply: null,
                    masterEditionKey: PDALookup.FindMasterEditionPDA(mint),
                    mintKey: mint,
                    updateAuthorityKey: Web3.Account,
                    mintAuthority: Web3.Account,
                    payer: Web3.Account,
                    metadataKey: PDALookup.FindMetadataPDA(mint),
                    version: CreateMasterEditionVersion.V3
                )
            );
        var tx = Transaction.Deserialize(transaction.Build(new List<Account> {Web3.Account, mint}));
        
        // Sign and Send the transaction
        var res = await Web3.Wallet.SignAndSendTransaction(tx);
        NFTResponse nftResponse = new NFTResponse();
        // Show Confirmation
        if (res?.Result != null){
            await Web3.Rpc.ConfirmTransaction(res.Result, Commitment.Confirmed);
            nftResponse.url = " https://explorer.solana.com/tx/" + res.Result + "?cluster=" + Web3.Wallet.RpcCluster.ToString().ToLower();
            nftResponse.response = true;
        }else{
            nftResponse.url = "Transaction Reverted/Cancelled";
            nftResponse.response = false;
        }
        return nftResponse;
    }

}
