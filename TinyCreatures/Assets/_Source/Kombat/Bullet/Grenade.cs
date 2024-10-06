using UnityEngine;

public class Grenade : ABullet
{
    [SerializeField] private Launcher launcher;
    
    private void Start()
    {
        launcher = FindObjectOfType<Launcher>();
    }

    private void OnCollisionEnter2D(Collision2D other)
    {
        launcher.DetonateGrenade();
        Destroy(gameObject);
    }
}
