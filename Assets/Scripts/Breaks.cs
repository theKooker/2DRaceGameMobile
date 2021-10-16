using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Breaks : MonoBehaviour
{
    // Start is called before the first frame update
    public AudioClip breaks;
    public AudioClip engine;
    public void decreaseSpeed()
    {
        if(GameData.speed>2){
        GameData.speed = GameData.speed - 1;
        GetComponent<AudioSource>().PlayOneShot(breaks);
        }
    }

    public void increaseSpeed() {
        if(GameData.speed < 30){
        GameData.speed = GameData.speed + 1;
        GetComponent<AudioSource>().PlayOneShot(engine);

        }
    }
}
