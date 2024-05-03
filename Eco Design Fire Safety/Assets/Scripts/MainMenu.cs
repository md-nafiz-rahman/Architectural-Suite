
// MainMenu.cs handles user interactions in the main menu, including starting new designs, loading saved designs, and exiting the game.

using UnityEngine;
using UnityEngine.SceneManagement; 

public class MainMenu : MonoBehaviour
{
    public GameObject mainMenu;
    public GameObject loadMenu;


    // Starts a new design session.
    public void StartNewDesign()
    {
        PlayerPrefs.SetString("SceneToLoad", "ModernHomes");
        PlayerPrefs.SetInt("LoadSlot", -1); 
        SceneManager.LoadScene("LoadingScreen");
    }

    public void OpenLoadPanel()
    {
        mainMenu.SetActive(false);
        loadMenu.SetActive(true);
    }

    public void BackMainMenu()
    {
        mainMenu.SetActive(true);
        loadMenu.SetActive(false);
    }


    public void LoadDesign(int slot)
    {
        loadMenu.SetActive(false);
        PlayerPrefs.SetInt("LoadSlot", slot);
        PlayerPrefs.SetString("SceneToLoad", "ModernHomes");
        SceneManager.LoadScene("LoadingScreen");
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
