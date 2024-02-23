using UnityEngine;
using UnityEngine.SceneManagement;
using System.Collections;

public class LoadingScreenManager : MonoBehaviour
{
    public float delay = 3f;

    void Start()
    {
        StartCoroutine(LoadSceneAfterDelay());
    }

    private IEnumerator LoadSceneAfterDelay()
    {
        string sceneToLoad = PlayerPrefs.GetString("SceneToLoad");

        yield return new WaitForSeconds(delay);

        SceneManager.LoadSceneAsync(sceneToLoad);
    }
}
