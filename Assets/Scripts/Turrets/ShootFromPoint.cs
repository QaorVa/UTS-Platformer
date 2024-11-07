using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ShootFromPoint : MonoBehaviour
{
    [SerializeField] private GameObject bulletPrefab;
    [SerializeField] private Transform shootPoint;

    private RotateToPlayer rotateToPlayer;

    private float shotTimer;
    [SerializeField] private float startShotTimer = 2.5f;
    [SerializeField] private float shotTimerReset = 1.5f;

    private AudioSource audioSource;

    // Start is called before the first frame update
    void Start()
    {
        rotateToPlayer = GetComponent<RotateToPlayer>();
        audioSource = GetComponent<AudioSource>();
    }

    // Update is called once per frame
    void Update()
    {
        if(rotateToPlayer.isPlayerInRange)
        {
            Shoot();
        }
    }

    private void Shoot()
    {
        if(shotTimer <= 0)
        {
            StartCoroutine(rotateToPlayer.DisableRotation(.5f));
            Instantiate(bulletPrefab, shootPoint.position, shootPoint.rotation);
            audioSource.Play();
            shotTimer = startShotTimer;
        }
        else
        {
            shotTimer -= Time.deltaTime;
        }
    }

    private void OnTriggerEnter2D(Collider2D other)
    {
        if(other.CompareTag("Player"))
        {
            shotTimer = shotTimerReset;
        }
    }
}
