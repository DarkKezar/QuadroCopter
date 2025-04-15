using UnityEngine;
using UnityEngine.SceneManagement;
using DroneController;


public class EventFunctions : MonoBehaviour, IPauseHandler
{
    private void Start()
    {
        PauseManager.Instance.Register(this);
    }

    public void SetPaused(bool isPaused)
    {
        if (isPaused)
            PauseGame();
        else
            ResumeGame();
    }

    public void PauseGame()
    {
        Time.timeScale = 0f;
        InputManager.Instance.enabled = false;
        AudioListener.pause = true;

    }

    public void ResumeGame()
    {
        Time.timeScale = 1f;
        InputManager.Instance.enabled = true;
        AudioListener.pause = false;

    }

    public void RestartLevel()
    {
        SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex);
    }

    public void GoToScene(string sceneName)
    {
        SceneManager.LoadScene(sceneName);
    }

    public void LoadMainMenu()
    {
        SceneManager.LoadScene("SelectLevel");
    }

    public void QuitGame()
    {
    #if UNITY_STANDALONE
        Application.Quit();
    #endif
    #if UNITY_EDITOR
        UnityEditor.EditorApplication.isPlaying = false;
    #endif
    }

    public void toggleGameObject(GameObject gameObject)
    {
        if (gameObject.activeSelf)
        {
            gameObject.SetActive(false);
        }
        else
        {
            gameObject.SetActive(true);
        }
    }

    public void toggleGameObjects(GameObject[] gameObjects)
    {
        foreach (GameObject g in gameObjects)
        {
            if (!g.activeSelf)
            {
                g.SetActive(true);
            }
            else
            {
                g.SetActive(false);
            }
        }
    }
}
