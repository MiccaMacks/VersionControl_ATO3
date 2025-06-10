using System.Collections;
using UnityEngine;

public class GameManager : MonoBehaviour
{
    public static GameManager Instance;
    private GameObject PlayerRef;

    [SerializeField] private GameObject PlayerPrefab;
    public Transform LastCheckpoint;

    private void Awake()
    {
        if (Instance == null)
        {
            DontDestroyOnLoad(this);
            Instance = this;
        }
        else
        {
            Destroy(this);
        }
    }

    //spawns player at default spawn pos on start.
    private void Start()
    {
        SpawnPlayer(LastCheckpoint.position);
    }

    //removes players ability to look around and move, and deletes mesh renderer to make them dissapear, then turns on death screen.
    public void KillPlayer()
    {
        PlayerRef.GetComponentInChildren<CameraController>().CanLook = false;
        Destroy(PlayerRef.GetComponentInChildren<MeshRenderer>());
        Destroy(PlayerRef.GetComponent<Movement>());

        UIManager.Instance.ToggleDeathScreen(true);

        //used before respawn button was added
        //StartCoroutine(RespawnPlayer(0.6f));
    }

    //Spawns player at given position. Deletes any previous versions of the player and turns of Death Screen.
    public void SpawnPlayer(Vector3 spawnPos)
    {
        UIManager.Instance.ToggleDeathScreen(false);

        if (PlayerRef != null) { Destroy(PlayerRef); }

        PlayerRef = Instantiate(PlayerPrefab, spawnPos, Quaternion.identity);
    }

    //Begins a timer to respawn the player.
    public IEnumerator RespawnPlayer(float time)
    {
        yield return new WaitForSeconds(time);

        SpawnPlayer(LastCheckpoint.position);
    }
}
