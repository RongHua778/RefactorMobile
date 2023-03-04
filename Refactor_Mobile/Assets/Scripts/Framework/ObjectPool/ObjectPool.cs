﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ObjectPool : MySingleton<ObjectPool>
{
    public string ResourceDir = "";
    Dictionary<string, SubPool> m_pools = new Dictionary<string, SubPool>();
    Dictionary<SubPool, Transform> m_pools_parent = new Dictionary<SubPool, Transform>();

    public void ClearPools()
    {
        m_pools.Clear();
    }

    //取对象
    //public ReusableObject Spawn(string name)
    //{
    //    if (!m_pools.ContainsKey(name))
    //        RegisterNew(name);
    //    SubPool pool = m_pools[name];

    //    return pool.Spawn(m_pools_parent[pool]);

    //}

    public ReusableObject Spawn(ReusableObject gameObj)
    {
        if (!m_pools.ContainsKey(gameObj.name))
            RegisterNew(gameObj);
        SubPool pool = m_pools[gameObj.name];

        return pool.Spawn(m_pools_parent[pool]);
    }
    //回收对象
    public void UnSpawn(ReusableObject go)
    {
        SubPool pool = null;
        foreach (SubPool p in m_pools.Values)
        {
            if (p.Contains(go))
            {
                pool = p;
                break;
            }
        }
        pool.UnSpawn(go);
    }
    //回收所有对象
    public void UnSpawnAll()
    {
        foreach (SubPool p in m_pools.Values)
        {
            p.UnSpawnAll();
        }
    }
    //创建新子池子
    //void RegisterNew(string name)
    //{
    //    //预设路径
    //    string path = "";
    //    if (string.IsNullOrEmpty(ResourceDir))
    //        path = name;
    //    else
    //        path = ResourceDir + "/" + name;

    //    //加载预设
    //    ReusableObject prefab = Resources.Load<ReusableObject>(path);

    //    //创建子对象池
    //    SubPool pool = new SubPool(name, prefab);
    //    m_pools.Add(pool.Name, pool);

    //    GameObject container = new GameObject($"Pool-{name}");
    //    m_pools_parent.Add(pool, container.transform);
    //}

    void RegisterNew(ReusableObject obj)
    {
        ReusableObject prefab = obj;

        SubPool pool = new SubPool(obj.name, prefab);
        m_pools.Add(pool.Name, pool);

        GameObject container = new GameObject($"Pool-{obj.name}");
        m_pools_parent.Add(pool, container.transform);
    }
}
