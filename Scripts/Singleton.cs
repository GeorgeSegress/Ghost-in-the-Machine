using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Singleton : MonoBehaviour
{
    public static bool instaned;

    void Start()
    {
        if (!Singleton.instaned)
            Singleton.instaned = true;
        else
            Destroy(gameObject);
        DontDestroyOnLoad(this);
    }

    void Update()
    {
        
    }
}
