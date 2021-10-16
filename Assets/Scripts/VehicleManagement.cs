using System.Net.Mime;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;
public class VehicleManagement : MonoBehaviour
{
    public List<Sprite> sprites;
    public Image image;
    private int playerImageIndex;
    private int index;
    private AudioSource audio;
    // Start is called before the first frame update
    void Start()
    {
        audio = GetComponent<AudioSource>();
        playerImageIndex = PlayerPrefs.GetInt("vehicle", 0);
        index = playerImageIndex;
        image.sprite = sprites[playerImageIndex];
    }

    public void selectPlayerVehicle()
    {
        audio.Play();
        PlayerPrefs.SetInt("vehicle", index);

    }

    public void indexUp()
    {
        audio.Play();

        index = (index + 1) % 4;
        image.sprite = sprites[index];
    }

    public void indexDown()
    {
        audio.Play();

        if (index == 0)
        {
            index = 3;
        }
        else
        {
            index--;
        }
        image.sprite = sprites[index];
    }

    public void getBack()
    {
        audio.Play();

        SceneManager.LoadScene("MainMenu");
    }
    // Update is called once per frame
}
