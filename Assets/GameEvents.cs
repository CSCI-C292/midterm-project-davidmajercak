using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

public static class GameEvents
{
    public static event EventHandler LevelCompleted;

    public static void InvokeLevelCompleted()
    {
        LevelCompleted(null, EventArgs.Empty);
    }
}
