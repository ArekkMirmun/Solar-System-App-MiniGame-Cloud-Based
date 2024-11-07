using TMPro;
using UnityEngine;

public class PointController : MonoBehaviour
{
    [SerializeField]private int points;
    [SerializeField] private bool gameOver;
    public TextMeshProUGUI pointsText;

    public int Points {
        get => points;
        set => points = value;      
    }
    public bool GameOver
    {
        get => gameOver;
        set => gameOver = value;
    }

    private void Awake()
    {
        int numInstancias = FindObjectsByType<PointController>(FindObjectsSortMode.None).Length;
        if (numInstancias > 1)
        {
            Destroy(gameObject);
        }
        else
        {
            DontDestroyOnLoad(gameObject);
        }
    }
}
