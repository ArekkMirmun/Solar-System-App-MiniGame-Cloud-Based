using System;
using System.Collections;
using System.Collections.Generic;
using Newtonsoft.Json;
using TMPro;
using UnityEngine;
using UnityEngine.Networking;
using Button = UnityEngine.UI.Button;
using Random = UnityEngine.Random;

public class GameController : MonoBehaviour
{
    public float countDownStartTime;
    private float _countDownTime;
    
    public TextMeshProUGUI countDownText;
    public TextMeshProUGUI planetText;
    public TextMeshProUGUI pointText;
    public TextMeshProUGUI correctPlanetScannedText;
    public TextMeshProUGUI incorrectPlanetScannedText;
    public TextMeshProUGUI loadingPlanetText;
    public Button loadGameOverButton;
    
    private PointController _pointController;
    
    public AudioSource correctPlanetSound;
    public AudioSource incorrectPlanetSound;

    public bool scannable;
    public bool loadingPlanet;

    public List<string> planets;

    // Start is called once before the first execution of Update after the MonoBehaviour is created

    void Start()
    {
        _pointController = GameObject.Find("PointsController").GetComponent<PointController>();
        ResetCountDown();
        InitializePlanetList();
        NextPlanet();
        SetLoadingPlanet(false);
        scannable = true;
        loadingPlanet = false;
        _pointController.Points = 0;
        _pointController.GameOver = false;
        correctPlanetScannedText.gameObject.SetActive(false);
        incorrectPlanetScannedText.gameObject.SetActive(false);
        loadGameOverButton.gameObject.SetActive(false);
    }

    private void InitializePlanetList()
    {
        planets = FindObjectsByType<PlanetListController>(FindObjectsSortMode.None)[0].planets;
    }

    // Update is called once per frame
    void Update()
    {
        if (scannable && !loadingPlanet)
        {
            if (_countDownTime > 0)
            {
                _countDownTime -= Time.deltaTime;
                countDownText.text = _countDownTime.ToString("0");
            }
            else
            {
                if (!_pointController.GameOver)
                {
                    NextPlanet();
                }
            }
        }
    }

    private void NextPlanet()
    {
        if (planets.Count > 0)
        {
            string selectedPlanet = planets[Random.Range(0, planets.Count)];
            planets.Remove(selectedPlanet);
            planetText.text = selectedPlanet;
            ResetCountDown();
        }
        else
        {
            print("No more planets");
            planetText.text = "No more planets";
            _countDownTime = 0;
            countDownText.text = "";
            _pointController.GameOver = true;
            loadGameOverButton.gameObject.SetActive(true);
        }
    }

    void ResetCountDown()
    {
        _countDownTime = countDownStartTime;
    }

    public void PlanetScanned(Planet planet)
    {
        SetLoadingPlanet(false);
        string planetName = planet.Name;
        
        print("Plannet scanned: " + planetName);
        if (scannable && !_pointController.GameOver)
        {
            StartCoroutine(CheckPlanetScanned(planetName));
        }
    }

    public void SetLoadingPlanet(bool loadingPlanet)
    {
        loadingPlanetText.gameObject.SetActive(loadingPlanet);
        this.loadingPlanet = loadingPlanet;
    }

    private IEnumerator CheckPlanetScanned(string planetName)
    {
        scannable = false;
        countDownText.text = "";

        bool correctPlanet = planetText.text == planetName || planetName == "cheatGood";

        if (correctPlanet)
        {
            _pointController.Points++;
            correctPlanetSound.Play();
            pointText.text = _pointController.Points.ToString();
            correctPlanetScannedText.gameObject.SetActive(true);
        }
        else
        {
            incorrectPlanetSound.Play();
            incorrectPlanetScannedText.gameObject.SetActive(true);
        }

        yield return new WaitForSeconds(2f);
        correctPlanetScannedText.gameObject.SetActive(false);
        incorrectPlanetScannedText.gameObject.SetActive(false);
        NextPlanet();
        scannable = true;
    }
    
    
    
}