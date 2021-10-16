using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class SceneMangement : MonoBehaviour
{

    public void play()
    {
        GetComponent<AudioSource>().Play();
        switch (PlayerPrefs.GetInt("road", 0))
        {
            case 0:
                SceneManager.LoadScene("HighWayCity");
                break;
            case 1: SceneManager.LoadScene("RainyHighWayCity"); break;
            case 2: SceneManager.LoadScene("SnowyHighWay"); break;
        }


    }

    public void selectVehicle()
    {
        GetComponent<AudioSource>().Play();
        SceneManager.LoadScene("Vehicle");
    }

    public void selectRoad() {
        GetComponent<AudioSource>().Play();
        SceneManager.LoadScene("Road");
    }


    public Text highScore;

    public void Start()
    {
        highScore.text = PlayerPrefs.GetInt("high", 0).ToString();
    }
    public void Update()
    {
        Debug.Log(PlayerPrefs.GetInt("high", 0));
        highScore.text = PlayerPrefs.GetInt("high", 0).ToString();
    }
}
