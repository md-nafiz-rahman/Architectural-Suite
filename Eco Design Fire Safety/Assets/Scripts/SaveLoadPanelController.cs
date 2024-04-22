using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SaveLoadPanelController : MonoBehaviour
{
    public GameObject saveCanvas; 
    public GameObject loadCanvas; 
    public PauseMenu pausemenu;


    public void ActivateSaveCanvas()
    {
        saveCanvas.SetActive(true);
        pausemenu.pauseMenuUI.SetActive(false);
        loadCanvas.SetActive(false); 
        FreezeGame(true);
    }

    public void DeactivateSaveCanvas()
    {
        saveCanvas.SetActive(false);
        CheckAndUnfreezeGame();
    }

    public void ActivateLoadCanvas()
    {
        pausemenu.pauseMenuUI.SetActive(false);
        loadCanvas.SetActive(true);
        saveCanvas.SetActive(false); 
        FreezeGame(true);
    }

    public void DeactivateLoadCanvas()
    {
        loadCanvas.SetActive(false);
        CheckAndUnfreezeGame();
    }

    private void CheckAndUnfreezeGame()
    {
        if (!saveCanvas.activeSelf && !loadCanvas.activeSelf)
        {
            FreezeGame(false);
        }
    }

    private void FreezeGame(bool freeze)
    {
        Time.timeScale = freeze ? 0 : 1;
        Cursor.lockState = freeze ? CursorLockMode.None : CursorLockMode.Locked;
        Cursor.visible = freeze;
    }
}