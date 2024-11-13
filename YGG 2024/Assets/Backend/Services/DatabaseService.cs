using System;
using System.Collections;
using System.Collections.Generic;
using Unisave;
using Unisave.Facets;
using Unisave.Facades;
using Unisave.Authentication.Middleware;
using ESDatabase.Classes;

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
        GameData data = new GameData();
        PlayerData player = new PlayerData(pubkey, DateTime.UtcNow);
        player.Save();
        
        return player;
    }

}
