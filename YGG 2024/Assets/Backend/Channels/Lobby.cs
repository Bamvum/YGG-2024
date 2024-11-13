using System;
using System.Collections;
using System.Collections.Generic;
using Unisave;
using Unisave.Entities;
using Unisave.Facades;
using Unisave.Broadcasting;

public class Lobby : BroadcastingChannel
{
    public SpecificChannel WithParameters(string roomName)
    {
        return SpecificChannel.From<Lobby>(roomName);
    }
}
