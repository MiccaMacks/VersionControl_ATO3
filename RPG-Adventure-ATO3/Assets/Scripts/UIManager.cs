using UnityEngine;

public class UIManager : MonoBehaviour
{
    public GameObject DeathScreen;

    public static UIManager Instance;

    private void Awake()
    {
        if (Instance != null)
        {
            Destroy(this);
        }
        else
        {
            Instance = this;
        }
    }

    private void Start()
    {
        DeathScreen.SetActive(false);
    }

    public void ToggleDeathScreen(bool toggle)
    {
        DeathScreen.SetActive(toggle);
        Cursor.visible = toggle;
        if (toggle) { Cursor.lockState = CursorLockMode.Confined; }
        else { Cursor.lockState = CursorLockMode.Locked; }
    }

    public void OnRespawnButtonPressed()
    {
        StartCoroutine(GameManager.Instance.RespawnPlayer(0.8f));
    }
}
