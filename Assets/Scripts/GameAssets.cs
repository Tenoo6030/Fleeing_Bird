using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameAssets : MonoBehaviour
{
    private static GameAssets instance;

    public static GameAssets GetInstance { get { return instance; } }

    private void Awake()
    {
        instance = this;
    }

   
    public GameObject prefColumnBody;
    public GameObject prefColumnHead;
}
