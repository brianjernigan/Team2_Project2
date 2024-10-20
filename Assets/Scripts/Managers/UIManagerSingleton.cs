using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UIManagerSingleton : MonoBehaviour
{
    public static UIManagerSingleton Instance { get; private set; }

    private void Awake()
    {
        if (Instance is null)
        {
            Instance = this;
            DontDestroyOnLoad(gameObject);
        }
        else
        {
            Destroy(gameObject);
        }
    }
}
