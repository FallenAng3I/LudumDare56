using UnityEngine;
using UnityEngine.SceneManagement;

namespace _Source.MenuManager
{
    public class PauseMenu : MonoBehaviour
    {
        public static bool PauseGame;
        public GameObject pauseMenu;
        public GameObject panelSettings;
        
        void Update()
        {
            if (Input.GetKeyDown(KeyCode.Escape))
            {
                
                if (PauseGame)
                {
                    panelSettings.SetActive(false);
                    Resume();
                }
                else
                {
                    panelSettings.SetActive(false);
                    Pause();
                }
            }
        }
        public void Resume()
        {
            pauseMenu.SetActive(false);
            Time.timeScale = 1f;
            PauseGame = false;
        }
        public void Pause()
        {
            pauseMenu.SetActive(true);
            Time.timeScale = 0f;
            PauseGame = true;
        }
        public void LoadMenu()
        {
            Time.timeScale = 1f;
            SceneManager.LoadScene("MainMenu");
        }
    }
}