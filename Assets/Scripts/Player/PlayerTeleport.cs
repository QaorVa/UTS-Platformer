using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Rendering.Universal;

public class PlayerTeleport : MonoBehaviour
{
    [SerializeField] private Material teleportMaterialOn;
    [SerializeField] private Material teleportMaterialOff;
    [SerializeField] private LineRenderer lineRenderer;
    [SerializeField] private float slowMotionFactor = 0.15f;
    [SerializeField] private float delayDuration = 0.3f;

    private GameObject teleportTarget;

    private Vector3 tempPlayerVector3;

    private void Start()
    {
        
        lineRenderer.enabled = false;
    }

    private void Update()
    {
        
        if (teleportTarget != null)
        {
            lineRenderer.enabled = true;
            lineRenderer.SetPosition(0, transform.parent.position); 
            lineRenderer.SetPosition(1, teleportTarget.transform.position); 
        }
        else
        {
            lineRenderer.enabled = false; 
        }

        if(PlayerHealth.isDead)
        {
            lineRenderer.enabled = false;
        }
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if(collision.gameObject.tag == "Teleportable Object")
        {
            teleportTarget = collision.gameObject;
            collision.gameObject.GetComponent<SpriteRenderer>().material = teleportMaterialOn;
            collision.gameObject.GetComponent<Light2D>().enabled = true;

        }
    }

    private void OnTriggerExit2D(Collider2D collision)
    {
        if(collision.gameObject.tag == "Teleportable Object")
        {
            teleportTarget = null;
            collision.gameObject.GetComponent<SpriteRenderer>().material = teleportMaterialOff;
            collision.gameObject.GetComponent<Light2D>().enabled = false;
        }
    }

    public void Teleport()
    {
        if(teleportTarget == null || PlayerHealth.isDead)
        {
            lineRenderer.enabled = false;
            return;
        }

        StartCoroutine(TeleportWithDelay());
    }

    private IEnumerator TeleportWithDelay()
    {
        PlayerHealth.isInvincible = true;
        Time.timeScale = slowMotionFactor;

        yield return new WaitForSecondsRealtime(delayDuration);

        tempPlayerVector3 = transform.parent.position;
        transform.parent.position = teleportTarget.transform.position;
        teleportTarget.transform.position = tempPlayerVector3;

        yield return new WaitForSecondsRealtime(delayDuration);

        Time.timeScale = 1f;

        yield return new WaitForSecondsRealtime(delayDuration);

        PlayerHealth.isInvincible = false;
    }
}
