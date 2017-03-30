using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Manager : Singleton<Manager>
{
    public int score = -1;
    public int[] pStats = new int[4];
}