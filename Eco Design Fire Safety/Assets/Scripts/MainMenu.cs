using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement; 

public class MainMenu : MonoBehaviour
{

    public void StartNewDesign()
    {
        SceneManager.LoadScene("ModernHomes");
    }


    public void LoadDesign()
    {
        Debug.Log("Load Design functionality not implemented yet.");
    }

    public void Options()
    {
        Debug.Log("Options functionality not implemented yet.");
    }

    public void ExitGame()
    {
#if UNITY_EDITOR
        Debug.Log("ExitGame method called - Exiting game in editor.");
#else
        Application.Quit();
#endif
    }
}
