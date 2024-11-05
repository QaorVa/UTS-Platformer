using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerHealth : MonoBehaviour
{
    [SerializeField] private int maxHealth = 1;

    private int currentHealth;

    public static bool isDead;
    public static bool isInvincible;

    private PlayerMovement playerMovement;

    private Animator anim;

    public Vector3 respawnPoint;

    // Start is called before the first frame update
    void Start()
    {
        currentHealth = maxHealth;
        playerMovement = GetComponent<PlayerMovement>();
        anim = GetComponent<Animator>();

        respawnPoint = transform.position;
    }

    // Update is called once per frame
    void Update()
    {
        if(currentHealth <= 0)
        {
            Die();
        }
    }

    public void Die()
    {
        if(isInvincible)
        {
            return;
        }

        isDead = true;
        anim.Play("Player_Death"); 
        playerMovement.rb.velocity = Vector2.zero;
        playerMovement.enabled = false;

        if(respawnPoint != null)
        {
            StartCoroutine(Respawn());
        }
    }

    public IEnumerator Respawn()
    {
        yield return new WaitForSeconds(1f);
        isDead = false;
        anim.Play("Player_Idle");
        playerMovement.enabled = true;
        currentHealth = maxHealth;
        transform.position = respawnPoint;
    }
}
