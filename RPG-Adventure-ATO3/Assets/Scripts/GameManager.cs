using System.Collections;
using UnityEngine;

public class GameManager : MonoBehaviour
{
    public static GameManager Instance;
    private GameObject PlayerRef;

    public GameObject PlayerPrefab;
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

    private void Start()
    {
        PlayerRef = GameObject.FindGameObjectWithTag("Player");
    }

    public void KillPlayer()
    {
        PlayerRef.GetComponentInChildren<CameraController>().CanLook = false;
        Destroy(PlayerRef.GetComponentInChildren<MeshRenderer>());
        Destroy(PlayerRef.GetComponent<Movement>());

        UIManager.Instance.DeathScreen.SetActive(true);

        StartCoroutine(RespawnPlayer());
    }

    public void SpawnPlayer()
    {
        UIManager.Instance.DeathScreen.SetActive(false);

        Destroy(PlayerRef);
        PlayerRef = Instantiate(PlayerPrefab, LastCheckpoint.position, Quaternion.identity);
    }

    private IEnumerator RespawnPlayer()
    {
        yield return new WaitForSeconds(3f);

        SpawnPlayer();
    }
}
