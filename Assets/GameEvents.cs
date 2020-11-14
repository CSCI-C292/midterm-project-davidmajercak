using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

public class CollectibleEventArgs : EventArgs
{
    public float collectiblePayload;
}

public static class GameEvents
{
    public static event EventHandler LevelCompleted;
    public static event EventHandler SongStarted;
    public static event EventHandler<CollectibleEventArgs> GatheredCollectible;
    

    public static void InvokeLevelCompleted()
    {
        LevelCompleted(null, EventArgs.Empty);
    }

    public static void InvokeSongStarted()
    {
        SongStarted(null, EventArgs.Empty);
    }

    public static void InvokeGatheredCollectible(float stopTimeAmount)
    {
        GatheredCollectible(null, new CollectibleEventArgs { collectiblePayload = stopTimeAmount });
    }
}
