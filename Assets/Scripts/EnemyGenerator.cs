using System.Linq.Expressions;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyGenerator : MonoBehaviour
{
    public GameObject enemy;
    public List<Sprite> sprites;
    private int counter = 0;
    private int max = 1000;
    Random rnd = new Random();
    // Start is called before the first frame update

    // Update is called once per frame
    void Update()
    {
        

    
        if(counter>max){
        GameObject e = enemy;
        int index = Random.Range(0,sprites.Count);
        float[] arr = {-1.5f,0.0f,1.5f};
        float x = arr[Random.Range(0,3)];
        Debug.Log("index = "+index.ToString()+" count = "+sprites.Count.ToString());
        e.GetComponent<SpriteRenderer>().sprite = sprites[index];
        GameObject.Instantiate(e,new Vector3(x,6.2f,-7),Quaternion.identity);
        counter = 0;
        if(max > 10)
        max -= 5;
        }
        
        counter+= 5;
    }
}
