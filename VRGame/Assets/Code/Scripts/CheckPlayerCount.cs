using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CheckPlayerCount
{
    public static int PlayerCount;

    public static void IncreasePlayerCount()
    {
        PlayerCount++;
    }

    public static void DecreasePlayerCount()
    {
        PlayerCount--;
    }
}
