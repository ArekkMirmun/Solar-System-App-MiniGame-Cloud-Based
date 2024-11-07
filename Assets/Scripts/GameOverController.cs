using TMPro;
using UnityEngine;

public class GameOverController : MonoBehaviour
{
    private PointController _pointController;
    public TextMeshProUGUI pointText;
    
    
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        _pointController = GameObject.Find("PointsController").GetComponent<PointController>();
        pointText.text = _pointController.Points.ToString();
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
