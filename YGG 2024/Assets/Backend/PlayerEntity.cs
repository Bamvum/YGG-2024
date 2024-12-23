using System;
using System.Collections;
using System.Collections.Generic;
using Unisave;
using Unisave.Entities;
using Unisave.Facades;

/*
 * This entity represents a player of your game. To learn how to add
 * player registration and authentication, check out the documentation:
 * https://unisave.cloud/docs/authentication
 *
 * If you don't need to register players, remove this class.
 */

public class PlayerEntity : Entity
{
    // Add authentication via email:
    // https://unisave.cloud/docs/email-authentication
    //
    //      public string email;
    //      public string password;
    //      public DateTime lastLoginAt = DateTime.UtcNow;
    //

    // Add custom fields to the entity:
    //
    //      public string nickname;
    //      public int coins = 1_000;
    //      public DateTime premiumUntil = DateTime.UtcNow;
    //      public DateTime bannedUntil = DateTime.UtcNow;
    //
}
