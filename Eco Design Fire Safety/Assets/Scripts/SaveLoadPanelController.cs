using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SaveLoadPanelController : MonoBehaviour
{
    public GameObject saveCanvas; // Assign in inspector
    public GameObject loadCanvas; // Assign in inspector
    public PauseMenu pausemenu;

    // Activate the Save Canvas and freeze the game
    public void ActivateSaveCanvas()
    {
        saveCanvas.SetActive(true);
        pausemenu.pauseMenuUI.SetActive(false);
        loadCanvas.SetActive(false); // Ensure only one canvas is active at a time
        FreezeGame(true);
    }

    // Deactivate the Save Canvas and unfreeze the game if no other UI needs it
    public void DeactivateSaveCanvas()
    {
        saveCanvas.SetActive(false);
        CheckAndUnfreezeGame();
    }

    // Activate the Load Canvas and freeze the game
    public void ActivateLoadCanvas()
    {
        pausemenu.pauseMenuUI.SetActive(false);
        loadCanvas.SetActive(true);
        saveCanvas.SetActive(false); // Ensure only one canvas is active at a time
        FreezeGame(true);
    }

    // Deactivate the Load Canvas and unfreeze the game if no other UI needs it
    public void DeactivateLoadCanvas()
    {
        loadCanvas.SetActive(false);
        CheckAndUnfreezeGame();
    }

    // Check if any UI elements are active and unfreeze the game if none are active
    private void CheckAndUnfreezeGame()
    {
        if (!saveCanvas.activeSelf && !loadCanvas.activeSelf)
        {
            FreezeGame(false);
        }
    }

    // Handle freezing and unfreezing the game
    private void FreezeGame(bool freeze)
    {
        Time.timeScale = freeze ? 0 : 1;
        Cursor.lockState = freeze ? CursorLockMode.None : CursorLockMode.Locked;
        Cursor.visible = freeze;
    }
}