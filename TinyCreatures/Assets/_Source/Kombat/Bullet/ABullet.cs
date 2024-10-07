using UnityEngine;

public class ABullet : MonoBehaviour
{
    private void OnTriggerEnter2D(Collider2D other)
    {
        if (other.gameObject.layer != LayerMask.NameToLayer("Edge"))
        {
            Destroy(gameObject);
        }
    }
}

