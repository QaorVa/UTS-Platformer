using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Cinemachine;

public class PlayerHealth : MonoBehaviour
{
    [SerializeField] private int maxHealth = 1;

    private int currentHealth;

    public static bool isDead;
    public static bool isInvincible;

    private PlayerMovement playerMovement;

    private Animator anim;

    public Vector3 respawnPoint;

    private float defaultCameraSize;
    private CinemachineVirtualCamera _virtualCamera;

    public static int deathCount = 0;

    // Start is called before the first frame update
    void Start()
    {
        currentHealth = maxHealth;
        playerMovement = GetComponent<PlayerMovement>();
        anim = GetComponent<Animator>();

        respawnPoint = transform.position;

        _virtualCamera = FindObjectOfType<CinemachineVirtualCamera>();
        defaultCameraSize = _virtualCamera.m_Lens.OrthographicSize;
    }

    // Update is called once per frame
    void Update()
    {
        if(currentHealth <= 0 && !isDead)
        {
            Die();
        }
    }

    public void Die()
    {
        if(isInvincible || isDead)
        {
            return;
        }

        isDead = true;
        anim.Play("Player_Death"); 
        playerMovement.rb.velocity = Vector2.zero;
        playerMovement.enabled = false;
        _virtualCamera.m_Lens.OrthographicSize = defaultCameraSize;
        deathCount++;

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
