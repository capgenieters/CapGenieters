using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LiftManager
{
    public static int PlayerCount = 1;

    public static void IncreasePlayerCount()
    {
        PlayerCount++;
    }

    public static void DecreasePlayerCount()
    {
        PlayerCount--;
    }
}
