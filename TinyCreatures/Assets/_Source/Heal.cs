using _Source.PlayerSystem;
using UnityEngine;

namespace _Source
{
    public class Heal : MonoBehaviour
    {
        private void OnTriggerEnter2D(Collider2D other)
        {
            if (other.gameObject.TryGetComponent(out Player player))
            {
                player.currentHealth+=player.maxHealth/2;
            }
        }
    }
}
