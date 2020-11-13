using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

public static class GameEvents
{
    public static event EventHandler LevelCompleted;
    public static event EventHandler SongStarted;

    public static void InvokeLevelCompleted()
    {
        LevelCompleted(null, EventArgs.Empty);
    }

    public static void InvokeSongStarted()
    {
        SongStarted(null, EventArgs.Empty);
    }
}
