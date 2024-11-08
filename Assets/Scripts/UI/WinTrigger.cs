using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WinTrigger : MonoBehaviour
{
    [SerializeField] private GameObject winResult;
    private PlayerMovement playerMovement;

    private bool isWon = false;

    private void Start()
    {
        playerMovement = FindObjectOfType<PlayerMovement>();
    }

    private void OnTriggerEnter2D(Collider2D other)
    {
        if (other.gameObject.tag == "Player" && !isWon)
        {
            Debug.Log("Player has won!");
            isWon = true;
            StartCoroutine(ShowUI());
            PlayerHealth.isInvincible = true;
            playerMovement.rb.velocity = Vector2.zero;
            playerMovement.anim.Play("Player_Win");
            playerMovement.enabled = false;
        }
    }

    private IEnumerator ShowUI()
    {
        yield return new WaitForSeconds(1.5f);

        winResult.SetActive(true);
    }

    
}
