using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class levelManagement : MonoBehaviour
{
    public void goMainMenu()
    {
        GetComponent<AudioSource>().Play();
        SceneManager.LoadScene("MainMenu");
    }
    public void tryAgain()
    {
        GetComponent<AudioSource>().Play();
        GameData.score = 0;
        GameData.speed = 1;
        switch (PlayerPrefs.GetInt("road", 0))
        {
            case 0:
                SceneManager.LoadScene("HighWayCity");
                break;
            case 1: SceneManager.LoadScene("RainyHighWayCity"); break;
            case 2: SceneManager.LoadScene("SnowyHighWay"); break;
        }
    }
}
