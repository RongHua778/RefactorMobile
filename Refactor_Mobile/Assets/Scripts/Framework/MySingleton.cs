using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class MySingleton<T> : MonoBehaviour
    where T : MonoBehaviour
{
    private static T m_instance = null;
    protected bool alreadyExist = false;
    public static T Instance
    {
        get { return m_instance; }
        set => m_instance = value;
    }

    protected virtual void Awake()
    {
        if (Instance != null)
        {
            Destroy(this.gameObject);
            Debug.Log(this.name + "已经创建了相同singleton实例");
            alreadyExist = true;
            return;
        }
        else
        {
            m_instance = this as T;
            alreadyExist = false;
        }
    }


}
