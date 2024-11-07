using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class WinResult : MonoBehaviour
{
    [SerializeField] private TextMeshProUGUI deathCounter;
    [SerializeField] private TextMeshProUGUI timer;


    private void OnEnable()
    {
        deathCounter.text = PlayerHealth.deathCount.ToString();

        timer.text = Timer.minutes.ToString("D2") + ":" + Timer.seconds.ToString("D2");

        
    }

    public void RestartGame()
    {
        PlayerHealth.deathCount = 0;
        Timer.seconds = 0;
        Timer.minutes = 0;

        UnityEngine.SceneManagement.SceneManager.LoadScene(0);
    }

    public void QuitGame()
    {
        Application.Quit();
    }
    
}
