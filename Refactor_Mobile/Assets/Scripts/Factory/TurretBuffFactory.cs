using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;
using System.Linq;
using System.Reflection;

public struct TurretBuffInfo
{
    public TurretBuffName BuffName;
    public int Stacks;
    public float Duration;
    public TurretBuffInfo(TurretBuffName buffName, int stacks, float duration)
    {
        this.BuffName = buffName;
        this.Stacks = stacks;
        this.Duration = duration;
    }
}

public class TurretBuffFactory
{
    public static Dictionary<int, Type> TurretBuffDIC;
    private static bool isInitialize => TurretBuffDIC != null;

    public static void Initialize()
    {
        if (isInitialize)
            return;
        var types = Assembly.GetAssembly(typeof(TurretBuff)).GetTypes().
            Where(myType => myType.IsClass && !myType.IsAbstract && myType.IsSubclassOf(typeof(TurretBuff)));
        TurretBuffDIC = new Dictionary<int, Type>();
        foreach (var type in types)
        {
            var buff = Activator.CreateInstance(type) as TurretBuff;
            TurretBuffDIC.Add((int)buff.TBuffName, type);
        }
    }

    public static TurretBuff GetBuff(int id)
    {
        if (TurretBuffDIC.ContainsKey(id))
        {
            Type type = TurretBuffDIC[id];
            TurretBuff buff = Activator.CreateInstance(type) as TurretBuff;
            return buff;
        }
        return null;
    }


}
