using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class RoadMovement : MonoBehaviour
{
    public Transform road1;
    public Transform road2;
    public float setting = 12;

    public Text scoreText;

    private Vector3 road1InitialPos;
    private Vector3 road2InitialPos;

    private int counter = 0;
    // Start is called before the first frame update
    void Start()
    {
        road1InitialPos = road1.position;
        road2InitialPos = road2.position;
    }
    // Update is called once per frame
    void Update()
    {
        road1.Translate(Vector3.up * Time.deltaTime * GameData.speed);
        road2.Translate(Vector3.up * Time.deltaTime * GameData.speed);
        if (road1.position.y >= setting)
        {
            road1.position = new Vector3(road1.position.x, -setting, road1.position.z);
        }
        if (road2.position.y >= setting)
        {
            road2.position = new Vector3(road2.position.x, -setting, road2.position.z);
        }
        GameData.score += (int)(GameData.speed * 0.25f);
        if (counter > 100 && GameData.speed<30)
        {
            GameData.speed += 0.5f;
            counter = 0;
        }
        counter ++;

        scoreText.text = "Score: " + GameData.score.ToString();
    }
}
