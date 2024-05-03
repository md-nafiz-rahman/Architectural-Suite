
// LoadingScreenManager.cs manages the loading of scenes with a delay.
 
using UnityEngine.SceneManagement;
using UnityEngine;
using System.Collections;

public class LoadingScreenManager : MonoBehaviour
{
    public float delay = 3f;

    private void Start()
    {
        StartCoroutine(LoadSceneAfterDelay());
    }

    private IEnumerator LoadSceneAfterDelay()
    {
        yield return new WaitForSeconds(delay); // Pause execution and wait for the specified delay
        string sceneToLoad = PlayerPrefs.GetString("SceneToLoad", "DefaultSceneName"); // Retrieve the scene name to load from PlayerPrefs
        SceneManager.LoadSceneAsync(sceneToLoad); // Start loading the specified scene asynchronously
        SceneManager.sceneLoaded += OnSceneLoaded; // Register the OnSceneLoaded method to be called when the scene has finished loading
    }

    private void OnSceneLoaded(Scene scene, LoadSceneMode mode)
    {
        int slot = PlayerPrefs.GetInt("LoadSlot", -1); // Retrieve the saved game slot from PlayerPrefs
        if (slot >= 0) // Check if the slot index is valid
        {
            LoadGameData loader = FindObjectOfType<LoadGameData>(); // Find an instance of LoadGameData in the scene
            if (loader)
            {
                loader.LoadGame(slot); // Load the game data for the specified slot
            }
        }
        SceneManager.sceneLoaded -= OnSceneLoaded; // Unregister the OnSceneLoaded method to prevent it from being called again
    }
}
