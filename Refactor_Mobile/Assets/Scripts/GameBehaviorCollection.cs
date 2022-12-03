using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class GameBehaviorCollection
{
    public List<IGameBehavior> behaviors = new List<IGameBehavior>();

    public void Add(IGameBehavior behavior)
    {
        behaviors.Add(behavior);
    }

    public void Remove(IGameBehavior behavior)
    {
        behaviors.Remove(behavior);
    }

    public void RemoveAll()
    {
        foreach (var item in behaviors)
        {
            ObjectPool.Instance.UnSpawn(item as ReusableObject);
        }
        behaviors.Clear();
    }

    public void GameUpdate()
    {
        for(int i = 0; i < behaviors.Count; i++)
        {
            if (!behaviors[i].GameUpdate())
            {
                int lastIndex = behaviors.Count - 1;
                behaviors[i] = behaviors[lastIndex];
                behaviors.RemoveAt(lastIndex);
                i -= 1;
            }
        }
    }
}
