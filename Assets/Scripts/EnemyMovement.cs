using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyMovement : MonoBehaviour
{
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        gameObject.transform.Translate(Vector3.down*GameData.speed*Time.deltaTime);
        if(gameObject.transform.position.y < -6){
            Destroy(gameObject);
        }
    }
}
