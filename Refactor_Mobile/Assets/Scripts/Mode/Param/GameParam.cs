using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public static class GameParam
{
    public static int GroundSize = 25;
    public static int TrapCount;

    public static void ResetGameParam()//重置战斗参数
    {
        GroundSize = StaticData.GroundSize;
    }
}
