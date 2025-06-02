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
}
