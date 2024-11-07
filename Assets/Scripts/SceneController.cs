using TMPro;
using UnityEngine;
using UnityEngine.SceneManagement;

public class SceneController : MonoBehaviour
{
    
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
    }

    // Update is called once per frame
    void Update()
    {
        
    }
    
    public void LoadGame(){
        SceneManager.LoadScene("Game");
    }
    
    public void LoadStartMenu(){
        SceneManager.LoadScene("StartMenu");
    }

    public void LoadCredits()
    {
        SceneManager.LoadScene("Credits");
    }

    public void CloseGame()
    {
        Application.Quit();
    }

    public void LoadGameOver()
    {
        SceneManager.LoadScene("GameOver");
    }
    
}
