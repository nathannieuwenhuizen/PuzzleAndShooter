using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public static class GameSettings
{
    public static List<MyCharacterInfo> matchCharacters;
    public static int goalScore = 5;
    public static bool paused = false;
}
public class MyCharacterInfo
{
    public int goal;
    public int controlledBy;
    public bool AI;
}