using UnityEngine;

public class PlayerCollider : MonoBehaviour
{
    private void OnTriggerEnter2D(Collider2D other)
    {
        if (other.CompareTag("Planet"))
        {
            GameEvents.PlanetLanded(other.transform);
            GameEvents.Landing();
            GameEvents.PlanetReached();
            return;
        }

        if (other.CompareTag("Asteroid"))
        {
            GameEvents.GameOver();
        }
    }
}
