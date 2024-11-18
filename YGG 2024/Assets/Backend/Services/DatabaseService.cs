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
using Unisave.Broadcasting;

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
    public LoginResponse CreateAccountWithoutPrice(string pubkey, string uid)
    {
        LoginResponse loginResponse = new LoginResponse();
        PlayerData player = DB.TakeAll<PlayerData>().Get().FirstOrDefault(data => data.publicKey == pubkey);
        string isExisting = DBHelper.IsPlayerExisting(pubkey, player);
        PlayerData copiedPlayer = DB.TakeAll<PlayerData>().Get().FirstOrDefault(data => data.EntityId == isExisting);
        copiedPlayer.token = uid;
        copiedPlayer.Save();
        loginResponse.playerData = copiedPlayer;
        loginResponse.priceData = GetPrice();
        Auth.Login(copiedPlayer);
        return loginResponse;
    }
    public LoginResponse InitializeLoginWithPrice(string pubkey, string UID)
    {
        LoginResponse loginResponse = new LoginResponse();
        PlayerData player = DB.TakeAll<PlayerData>().Get().FirstOrDefault(data => data.publicKey == pubkey);
        string isExisting = DBHelper.IsPlayerExisting(pubkey, player);
        if(CheckSession(pubkey)){
            BroadcastExistingPlayerSession(player.EntityId);
        }        
        PlayerData copiedPlayer = DB.TakeAll<PlayerData>().Get().FirstOrDefault(data => data.EntityId == isExisting);
        copiedPlayer.token = UID;
        copiedPlayer.Save();
        Auth.Login(copiedPlayer);
        
        loginResponse.playerData = copiedPlayer;
        loginResponse.priceData = GetPrice();
        return loginResponse;
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
    public ChannelSubscription JoinOnlineChannel()
    {
        PlayerData p = Auth.GetPlayer<PlayerData>();
        var subscription = Broadcast
            .Channel<OnlineChannel>()
            .JoinRoom(p.EntityId)
            .CreateSubscription();
        return subscription;
    }
    private void BroadcastExistingPlayerSession(string playerId)
    {
        Broadcast.Channel<OnlineChannel>()
            .ForPlayer(playerId)
            .Send(new NewExistingSession {
                message = "New client logged into your account."
            });
    }
    public bool CheckSession(string pubkey)
    {
        List<PlayerData> playerList = DB.TakeAll<PlayerData>().Get();
        PlayerData player = playerList.FirstOrDefault(d => d.publicKey == pubkey);

        string query = $"FOR s IN u_sessions FILTER s.sessionData.authenticatedPlayerId == '{player.EntityId}' RETURN s.sessionData.authenticatedPlayerId";
        List<string> data = DB.Query(query).GetAs<string>();
        if(data.Count > 0){
            if(data[0].Equals(player.EntityId)){
                return true;
            }else{
                return false;
            }
        }else{
             return false;
        }
    }
    public PlayerData GetPlayerById(string entityID)
    {
        PlayerData player = DB.Find<PlayerData>(entityID);

        return player;
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
