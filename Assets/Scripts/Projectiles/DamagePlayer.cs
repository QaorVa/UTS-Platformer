using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DamagePlayer : MonoBehaviour
{
    [SerializeField] private bool isDestroyable = true;

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if(collision.gameObject.tag == "Player")
        {
            CameraShakeTrigger.Instance.TriggerShake();
            if(!PlayerHealth.isDead)
            {
                collision.gameObject.GetComponent<PlayerHealth>().Die();
            }
            if(isDestroyable)
            {
                Destroy(gameObject);
            }
            
        } else if (collision.gameObject.tag == "Wall" || collision.gameObject.tag == "Ground")
        {
            CameraShakeTrigger.Instance.TriggerShake();
            if (isDestroyable)
            {
                Destroy(gameObject);
            }
        } else if (collision.gameObject.tag == "Teleportable Object")
        {
            CameraShakeTrigger.Instance.TriggerShake();
            collision.gameObject.GetComponent<Destructible>().DisableObject();

            if (isDestroyable)
            {
                Destroy(gameObject);
            }
        }
    }
}
