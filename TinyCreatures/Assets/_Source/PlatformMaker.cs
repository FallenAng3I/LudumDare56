using _Source.PlayerSystem;
using UnityEngine;

public class PlatformMaker : MonoBehaviour
{
    [SerializeField]private GameObject platform;
    private Platform platformComponent; // Ссылка на компонент Platform

    private void Start()
    {
        platformComponent = platform.GetComponent<Platform>(); // Получаем компонент Platform
        platformComponent.enabled = false; // По умолчанию отключаем компонент
    }

    public void OnCollisionEnter2D(Collision2D other)
    {
        if (other.gameObject.GetComponent<Player>())
        {
            platformComponent.enabled = true; // Включаем компонент
        }
    }
}
