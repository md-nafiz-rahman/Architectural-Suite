
// DontDestroy.cs ensures that the GameObject it is attached to is not destroyed on loading a new scene.
// It also checks for and destroys any duplicates that might exist, making sure there's only one instance of this object throughout the game runtime.

// Adapted from JTAGames's tutorial on "How To Make a Don't Destroy on Load (One Script for Every Object)" (Unity Tutorial),
// published March 26, 2021. Accessed March 2, 2024. Available online: https://www.youtube.com/watch?v=HXaFLm3gQws

using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DontDestroy : MonoBehaviour
{
    [HideInInspector]
    public string ObjectID;
    private void Awake()
    {
        ObjectID = name + transform.position.ToString() + transform.eulerAngles.ToString();

    }

    private void Start()
    {
        for (int i = 0; i < Object.FindObjectsOfType<DontDestroy>().Length; i++)
        {

            if (Object.FindObjectsOfType<DontDestroy>()[i] != this)
            {
                if (Object.FindObjectsOfType<DontDestroy>()[i].ObjectID == ObjectID)
                {
                    Destroy(gameObject);
                }
            }


        }
        DontDestroyOnLoad(gameObject);
    }

    private void Update()
    {

    }
}
