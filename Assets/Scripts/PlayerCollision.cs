using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class PlayerCollision : MonoBehaviour
{
    // Start is called before the first frame update
    public AudioClip crash;
    public Text lose;
    public Button tryAgain;
    public Button goToMainMenu;
    public GameObject road;
    public GameObject enemyGenerator;
    public GameObject gameSound;

    public AudioClip siren;


    public List<Sprite> sprites;

    private bool crashed = false;

    void Start()
    {
        GetComponent<SpriteRenderer>().sprite = sprites[PlayerPrefs.GetInt("vehicle", 0)];
    }

    // Update is called once per frame
    void Update()
    {

    }
    public bool isCrashed() {
        return crashed;
    }
    void OnCollisionEnter2D(Collision2D other)
    {
        crashed = true;
        GetComponent<AudioSource>().PlayOneShot(crash);
        lose.gameObject.SetActive(true);
        tryAgain.gameObject.SetActive(true);
        goToMainMenu.gameObject.SetActive(true);
        int highScore = PlayerPrefs.GetInt("high", 0);
        if (highScore < GameData.score)
        {
            PlayerPrefs.SetInt("high", GameData.score);
        }
        road.GetComponent<RoadMovement>().enabled = false;
        enemyGenerator.GetComponent<EnemyGenerator>().enabled = false;
        if (other.gameObject.GetComponent<SpriteRenderer>().sprite.name == "water")
        {
            other.gameObject.transform.position = new Vector3(0, 0, other.gameObject.transform.position.z);
            other.gameObject.transform.localScale = new Vector3(8, 8, 1);
        }
        else if(other.gameObject.GetComponent<SpriteRenderer>().sprite.name == "Police") {
            gameSound.GetComponent<AudioSource>().clip = siren;
            gameSound.GetComponent<AudioSource>().Play();
        }
        other.gameObject.GetComponent<EnemyMovement>().enabled = false;

    }
}
