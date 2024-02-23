using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace SojaExiles
{
    public class Quitgame : MonoBehaviour
    {
        void Start()
        {
        
        }
        void Update()
        {
            if (Input.GetKeyDown(KeyCode.Escape))
            {
                Application.Quit();
            }
        }
    }
}