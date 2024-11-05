using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Rendering.Universal;

public class Checkpoint : MonoBehaviour
{
    private bool isActivated = false;
    [SerializeField] Material checkpointMaterialOn;
    [SerializeField] Light2D checkpointLight;

    // Start is called before the first frame update
    void Start()
    {
        checkpointLight.enabled = false;
    }


    private void OnTriggerEnter2D(Collider2D collision)
    {
        if(collision.gameObject.tag == "Player" && !isActivated)
        {
            isActivated = true;
            collision.GetComponent<PlayerHealth>().respawnPoint = transform.position;
            gameObject.GetComponentInChildren<SpriteRenderer>().material = checkpointMaterialOn;
            checkpointLight.enabled = true;
        }
    }
}
