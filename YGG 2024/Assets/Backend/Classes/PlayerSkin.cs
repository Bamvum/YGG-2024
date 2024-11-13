using System;
using System.Collections.Generic;
using Unisave;
namespace ESDatabase.Classes
{
    [Serializable]
    public class PlayerSkin
    {
        
        [Fillable] public string armor {get;set;}
        [Fillable] public string back {get;set;}
        [Fillable] public string body {get;set;}
        [Fillable] public string hair {get;set;}
        [Fillable] public string head {get;set;}
        [Fillable] public string helmet {get;set;}
        [Fillable] public string shield {get;set;}
        [Fillable] public string weapon {get;set;}

        public PlayerSkin(){
            armor = "";
            back = "";
            body = "";
            hair = "";
            head = "";
            helmet = "";
            shield = "";
            weapon = "";
        }
    }
}
