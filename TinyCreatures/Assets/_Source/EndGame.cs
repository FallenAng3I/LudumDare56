using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;
using System.Collections;

public class EndGame : MonoBehaviour
{
    [SerializeField] private Image fadePanel;
    [SerializeField] private float fadeDuration = 1.0f;

    private void OnTriggerEnter2D(Collider2D other)
    {
        if (other.gameObject.layer == LayerMask.NameToLayer("Player"))
        {
            StartCoroutine(FadeOutAndLoadScene());
        }
    }

    private IEnumerator FadeOutAndLoadScene()
    {
        float elapsedTime = 0f;
        Color panelColor = fadePanel.color;
        
        while (elapsedTime < fadeDuration)
        {
            elapsedTime += Time.deltaTime;
            panelColor.a = Mathf.Clamp01(elapsedTime / fadeDuration);
            fadePanel.color = panelColor;
            yield return null;
        }

        SceneManager.LoadScene("MainMenu");
    }
}