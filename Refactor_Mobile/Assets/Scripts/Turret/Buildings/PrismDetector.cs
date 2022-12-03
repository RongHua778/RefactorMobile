using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;

public class PrismDetector : MonoBehaviour
{
    //[SerializeField] Prism m_Prism = default;
    public float IntensifyValue;

    public void SetSize(float size)
    {
        transform.DOScaleY(1f + size, 1f);
    }
    private void OnTriggerEnter2D(Collider2D collision)
    {
        Bullet bullet = collision.GetComponent<Bullet>();
        
        if (bullet != null)
        {
            bullet.BulletDamageIntensify += 2f;
        }
    }

}
