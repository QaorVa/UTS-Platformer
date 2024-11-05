using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DamagePlayer : MonoBehaviour
{
    private void OnTriggerEnter2D(Collider2D collision)
    {
        if(collision.gameObject.tag == "Player")
        {
            CameraShakeTrigger.Instance.TriggerShake();
            if(!PlayerHealth.isDead)
            {
                collision.gameObject.GetComponent<PlayerHealth>().Die();
            }
            
            Destroy(gameObject);
        } else if (collision.gameObject.tag == "Wall" || collision.gameObject.tag == "Ground")
        {
            CameraShakeTrigger.Instance.TriggerShake();
            Destroy(gameObject);
        }
    }
}
