using UnityEngine;

public class UIControlBridge : MonoBehaviour
{
    [SerializeField] private GameObject pauseMenu;

    public void OnPauseClicked()
    {
        GameFlowController.Instance?.PauseGame();
        TogglePauseMenu(true);
    }

    public void OnResumeClicked()
    {
        TogglePauseMenu(false);
        GameFlowController.Instance?.ResumeGame();
    }

    public void OnQuitClicked()
    {
        GameFlowController.Instance?.QuitGame();
    }

    private void TogglePauseMenu(bool isVisible)
    {
        if (pauseMenu != null)
        {
            pauseMenu.SetActive(isVisible);
        }
    }
}
