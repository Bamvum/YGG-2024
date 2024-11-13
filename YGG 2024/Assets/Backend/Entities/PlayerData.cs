using System;
using System.Collections;
using System.Collections.Generic;
using ESDatabase.Classes;
using Unisave;
using Unisave.Entities;
using Unisave.Facades;

public class PlayerData : Entity
{
    public string publicKey;
    public DateTime lastLoginAt = DateTime.UtcNow;
    public string token {get;set;}
    [Fillable] public GameData gameData {get; set;}
    public PlayerData() { }
    public PlayerData(string pubKey, DateTime loginDate){
        this.publicKey = pubKey;
        this.lastLoginAt = loginDate;
        this.gameData = new GameData();
    }
    
}
