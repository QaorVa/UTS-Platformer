using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DamagePlayer : MonoBehaviour
{
    [SerializeField] private bool isDestroyable = true;

    [SerializeField] private GameObject destroyEffect;

    private AudioSource audioSource;

    private bool isDestroyed = false;

    private void Start()
    {
        audioSource = GetComponent<AudioSource>();
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if(isDestroyed)
        {
            return;
        }

        if(collision.gameObject.tag == "Player")
        {
            CameraShakeTrigger.Instance.TriggerShake();
            if(!PlayerHealth.isDead)
            {
                collision.gameObject.GetComponent<PlayerHealth>().Die();
            }
            if(isDestroyable)
            {
                audioSource.Play();
                Destroy(gameObject);
                                
                if(destroyEffect != null)
                {
                    Instantiate(destroyEffect, transform.position, Quaternion.identity);
                }
                isDestroyed = true;
            }
            
        } else if (collision.gameObject.tag == "Wall" || collision.gameObject.tag == "Ground")
        {
            CameraShakeTrigger.Instance.TriggerShake();
            if (isDestroyable)
            {
                audioSource.Play();
                Destroy(gameObject);
                
                if (destroyEffect != null)
                {
                    Instantiate(destroyEffect, transform.position, Quaternion.identity);
                }
                isDestroyed = true;
            }
        } else if (collision.gameObject.tag == "Teleportable Object")
        {
            CameraShakeTrigger.Instance.TriggerShake();
            collision.gameObject.GetComponent<Destructible>().DisableObject();

            if (isDestroyable)
            {
                audioSource.Play();
                Destroy(gameObject);

                if (destroyEffect != null)
                {
                    Instantiate(destroyEffect, transform.position, Quaternion.identity);
                }
                isDestroyed = true;
            }
        }
    }
}
