using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Rendering.Universal;

public class ActivateVisual : MonoBehaviour
{

    [SerializeField] private GameObject visual;
    [SerializeField] private Color onColor;
    [SerializeField] private Color offColor;
    [SerializeField] private Light2D light2d;
    // Start is called before the first frame update
    void Start()
    {
        offColor = visual.GetComponent<SpriteRenderer>().color;
        onColor = light2d.color;
        light2d.enabled = false;
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.gameObject.tag == "Player")
        {
            visual.GetComponent<SpriteRenderer>().color = onColor;
            light2d.enabled = true;
        }
    }

    private void OnTriggerExit2D(Collider2D collision)
    {
        if (collision.gameObject.tag == "Player")
        {
            visual.GetComponent<SpriteRenderer>().color = offColor;
            light2d.enabled = false;
        }
    }
}
