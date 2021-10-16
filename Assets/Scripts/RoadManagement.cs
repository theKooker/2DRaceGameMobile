using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;
public class RoadManagement : MonoBehaviour
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
        playerImageIndex = PlayerPrefs.GetInt("road", 0);
        index = playerImageIndex;
        image.sprite = sprites[playerImageIndex];
    }

    public void selectPlayerRoad()
    {
        audio.Play();
        PlayerPrefs.SetInt("road", index);

    }

    public void indexUp()
    {
        audio.Play();

        index = (index + 1) % sprites.Count;
        image.sprite = sprites[index];
    }

    public void indexDown()
    {
        audio.Play();

        if (index == 0)
        {
            index = sprites.Count-1;
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
