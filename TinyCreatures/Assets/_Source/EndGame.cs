using _Source.PlayerSystem;

using UnityEngine;

public class EndGame : MonoBehaviour
{
    [SerializeField]private GameObject WinMenu;
    private void OnTriggerEnter2D(Collider2D other)
    {
        if (other.gameObject.TryGetComponent(out Player player))
        {
            Destroy(gameObject);
            WinMenu.gameObject.SetActive(true);
        }
    }
}
