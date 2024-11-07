using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Cinemachine;

public class CameraSizeTrigger : MonoBehaviour
{
    [SerializeField] private float cameraSize = 14f;
    [SerializeField] private float transitionDuration = 2f;
    [SerializeField] private bool isOneUse = true;

    private bool isUsed = false;

    private CinemachineVirtualCamera _virtualCamera;
    private float currentSize;

    // Start is called before the first frame update
    void Start()
    {
        _virtualCamera = FindObjectOfType<CinemachineVirtualCamera>();
        currentSize = _virtualCamera.m_Lens.OrthographicSize;
        Debug.Log("current camera size = " + currentSize);
        Debug.Log(isUsed);
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if(isOneUse && isUsed)
        {
            Debug.Log(isUsed);
            return;
        }

        if (_virtualCamera != null && collision.gameObject.tag == "Player")
        {
            StartCoroutine(ChangeCameraSize(cameraSize, transitionDuration));

            currentSize = cameraSize;
            Debug.Log("current camera size = " + currentSize);
            isUsed = true;
        }
        
    }

    private IEnumerator ChangeCameraSize(float targetSize, float duration)
    {
        float startSize = _virtualCamera.m_Lens.OrthographicSize;
        float elapsedTime = 0f;

        while (elapsedTime < duration)
        {
            elapsedTime += Time.deltaTime;
            _virtualCamera.m_Lens.OrthographicSize = Mathf.Lerp(startSize, targetSize, elapsedTime / duration);
            yield return null;
        }

        _virtualCamera.m_Lens.OrthographicSize = targetSize;
    }
}
