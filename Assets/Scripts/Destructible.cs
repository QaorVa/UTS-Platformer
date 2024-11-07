using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Destructible : MonoBehaviour
{
    [SerializeField] private float respawnDuration = 5f;
    private Vector3 objectOriginPosition;
    private Quaternion objectOriginRotation;

    private SpriteRenderer spriteRenderer;
    private BoxCollider2D boxCollider2D;
    private Rigidbody2D rb;


    private void Start()
    {
        objectOriginPosition = transform.position;
        objectOriginRotation = transform.rotation;

        spriteRenderer = GetComponent<SpriteRenderer>();
        boxCollider2D = GetComponent<BoxCollider2D>();
        rb = GetComponent<Rigidbody2D>();
    }

    public IEnumerator RespawnObject()
    {
        yield return new WaitForSeconds(respawnDuration);
        transform.position = objectOriginPosition;
        transform.rotation = objectOriginRotation;
        spriteRenderer.enabled = true;
        boxCollider2D.enabled = true;
        rb.simulated = true;
    }

    public void DisableObject()
    {
        spriteRenderer.enabled = false;
        boxCollider2D.enabled = false;
        rb.simulated = false;

        StartCoroutine(RespawnObject());
    }

}
