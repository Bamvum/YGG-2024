using System;
using System.Collections;
using System.Collections.Generic;
using Unisave;
using Unisave.Facets;
using Unisave.Facades;
using Unisave.Authentication.Middleware;
using ESDatabase.Classes;
using ESDatabase.Utilities;
using System.Linq;

public class DatabaseService : Facet
{
    /// <summary>
    /// Client can call this facet method and receive a greeting
    /// Replace this with your own server-side code
    /// </summary>
    public string GreetClient()
    {
        return "Hello client! I'm the server!";
    }
    public PlayerData CreateAccount(string pubkey)
    {
        PlayerData player = DB.TakeAll<PlayerData>().Get().FirstOrDefault(data => data.publicKey == pubkey);
        string isExisting = DBHelper.IsPlayerExisting(pubkey, player);
        PlayerData copiedPlayer = DB.TakeAll<PlayerData>().Get().FirstOrDefault(data => data.EntityId == isExisting);
        //copiedPlayer.token = UID;
        copiedPlayer.Save();
        Auth.Login(copiedPlayer);
        return copiedPlayer;
    }
    public string SaveData(PlayerData givenPlayer)
    {
        List<PlayerData> playerList = DB.TakeAll<PlayerData>().Get();
        PlayerData player = playerList.FirstOrDefault(data => data.publicKey == givenPlayer.publicKey);
        try{
            player.FillWith(givenPlayer);
            player.Save();
            return "Saving Success";
        }catch(Exception err){
            Log.Error("Error", err);
            return "Failed to save";
        }
    }

    public PriceData GetPrice(){
        string usdToSolUrl = "http://23.88.54.33:3444/nft-price-card"; // Use a crypto price API
        decimal solPriceInUSD = decimal.Parse(Http.Get(usdToSolUrl)["data"].AsString);
        DateTime fetchedDate = DateTime.Parse(Http.Get(usdToSolUrl)["lastUpdate"].AsString);
        PriceData priceData = new PriceData(){
            price = solPriceInUSD,
            date = fetchedDate
        };
        return priceData;
    }
}
