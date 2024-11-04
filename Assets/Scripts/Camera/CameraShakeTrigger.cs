using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Cinemachine;

public class CameraShakeTrigger : MonoBehaviour
{
    public static CameraShakeTrigger Instance { get; private set; }

    public CinemachineImpulseSource impulseSource;

    private void Awake()
    {
        
        if (Instance == null)
        {
            Instance = this;
        }
        else
        {
            Destroy(gameObject);
        }
    }

    // Method to trigger the camera shake
    public void TriggerShake()
    {
        impulseSource.GenerateImpulse();
    }
}
