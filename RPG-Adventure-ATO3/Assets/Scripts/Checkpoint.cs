using UnityEngine;

public class Checkpoint : MonoBehaviour
{
    [SerializeField] private Transform respawnPoint;


    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            Debug.Log("New Respawn Point");
            GameManager.Instance.LastCheckpoint = respawnPoint;
        }
    }
}
