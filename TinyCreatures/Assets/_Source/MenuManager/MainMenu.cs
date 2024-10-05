using UnityEngine;
using UnityEngine.SceneManagement;

namespace _Source.MenuManager
{
    public class MainMenu : MonoBehaviour
    {
        public void StartGame()
        {
            SceneManager.LoadScene("SampleScene");
        }
    }
}