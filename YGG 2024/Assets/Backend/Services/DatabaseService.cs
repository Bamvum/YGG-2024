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
}
