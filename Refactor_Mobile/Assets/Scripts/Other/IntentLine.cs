using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class IntentLine : MonoBehaviour
{
    public float IntensifyValue;

    private void OnTriggerEnter2D(Collider2D collision)
    {
        Bullet bullet = collision.GetComponent<Bullet>();

        if (bullet != null)
        {
            bullet.BulletDamageIntensify += IntensifyValue;
                
        }
    }
}
