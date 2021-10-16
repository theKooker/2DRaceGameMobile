using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameData : MonoBehaviour
{
    private static int _score = 0;
    public static int score
    {
        get { return _score; }
        set
        {
            _score = value;
        }
    }
    private static float _speed = 1;
    public static float speed
    {
        get { return _speed; }
        set
        {
            _speed = value;
        }
    }
}
