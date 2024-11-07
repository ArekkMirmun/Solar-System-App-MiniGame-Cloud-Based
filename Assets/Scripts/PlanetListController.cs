using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Networking;
using UnityEngine.UI;

public class PlanetListController : MonoBehaviour
{

    public List<String> planets = new List<string>();
    public Button startGameButton;

    void Start()
    {
        startGameButton.gameObject.SetActive(false);
        StartCoroutine(InitializePlanetList());
    }
    
    private IEnumerator InitializePlanetList()
    {
        using (UnityWebRequest request =
               UnityWebRequest.Get(
                   "https://drive.usercontent.google.com/u/4/uc?id=1gg5d6H70WbzyVbvxdBGQmCYqxld1YAwk&export=download"))
        {
            yield return request.SendWebRequest();
            if (request.isHttpError || request.isNetworkError)
            {
                print(request.error);
                planets = new List<string>();
            }
            else{
                print("Downloaded successfully");
                var planetsJson = request.downloadHandler.text;
                print(planetsJson);
                PlanetList planetList = JsonUtility.FromJson<PlanetList>(planetsJson);
                planets = planetList.planets;
                startGameButton.gameObject.SetActive(true);
            }
        }
        
    }
    
    private void Awake()
    {
        int numInstancias = FindObjectsByType<PlanetListController>(FindObjectsSortMode.None).Length;
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
