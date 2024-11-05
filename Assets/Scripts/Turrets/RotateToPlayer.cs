using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RotateToPlayer : MonoBehaviour
{
    [SerializeField] private float rotationSpeed = 10f;

    private Transform target;
    [HideInInspector]public bool isPlayerInRange;

    [SerializeField] private GameObject pivotPoint;

    public bool isAbleToRotate = true;

    // Start is called before the first frame update
    void Start()
    {
        target = GameObject.FindGameObjectWithTag("Target").transform;
    }

    // Update is called once per frame
    void Update()
    {
        if(isPlayerInRange)
        {
            RotateToTarget();
        }
    }

    private void OnTriggerEnter2D(Collider2D other)
    {
        if(other.CompareTag("Player"))
        {
            isPlayerInRange = true;
        }
    }

    private void OnTriggerExit2D(Collider2D other)
    {
        if(other.CompareTag("Player"))
        {
            isPlayerInRange = false;
        }
    }

    private void RotateToTarget()
    {
        if(target != null && isAbleToRotate)
        {
            var direction = (Vector2)(target.position - pivotPoint.transform.position);
            float angle = Mathf.Atan2(direction.y, direction.x) * Mathf.Rad2Deg + 90;

            var q = Quaternion.AngleAxis(angle, Vector3.forward);
            pivotPoint.transform.rotation = Quaternion.Slerp(pivotPoint.transform.rotation, q, Time.deltaTime * rotationSpeed);
        }
        
    }

    public IEnumerator DisableRotation(float duration = .4f)
    {
        isAbleToRotate = false;
        yield return new WaitForSeconds(duration);
        isAbleToRotate = true;
    }
}
