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
        yield return new WaitForSeconds(delay);
        string sceneToLoad = PlayerPrefs.GetString("SceneToLoad", "DefaultSceneName");
        SceneManager.LoadSceneAsync(sceneToLoad);
        SceneManager.sceneLoaded += OnSceneLoaded;
    }

    private void OnSceneLoaded(Scene scene, LoadSceneMode mode)
    {
        int slot = PlayerPrefs.GetInt("LoadSlot", -1);
        if (slot >= 0)
        {
            LoadGameData loader = FindObjectOfType<LoadGameData>(); 
            if (loader)
            {
                loader.LoadGame(slot);
            }
        }
        SceneManager.sceneLoaded -= OnSceneLoaded; 
    }
}
