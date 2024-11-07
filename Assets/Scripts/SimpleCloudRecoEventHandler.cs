using System.Collections;
using Classes;
using UnityEngine;
using UnityEngine.Networking;
using Vuforia;

public class SimpleCloudRecoEventHandler : MonoBehaviour
{
    CloudRecoBehaviour mCloudRecoBehaviour;
    bool mIsScanning = false;
    public string mTargetMetadata = "";
    public GameObject sphere; // Reference to the sphere object in the scene
    public GameController mGameController;

    public ImageTargetBehaviour ImageTargetTemplate;

    // Register cloud reco callbacks
    void Awake()
    {
        mCloudRecoBehaviour = GetComponent<CloudRecoBehaviour>();
        mCloudRecoBehaviour.RegisterOnInitializedEventHandler(OnInitialized);
        mCloudRecoBehaviour.RegisterOnInitErrorEventHandler(OnInitError);
        mCloudRecoBehaviour.RegisterOnUpdateErrorEventHandler(OnUpdateError);
        mCloudRecoBehaviour.RegisterOnStateChangedEventHandler(OnStateChanged);
        mCloudRecoBehaviour.RegisterOnNewSearchResultEventHandler(OnNewSearchResult);
    }

    //Unregister cloud reco callbacks when the handler is destroyed
    void OnDestroy()
    {
        mCloudRecoBehaviour.UnregisterOnInitializedEventHandler(OnInitialized);
        mCloudRecoBehaviour.UnregisterOnInitErrorEventHandler(OnInitError);
        mCloudRecoBehaviour.UnregisterOnUpdateErrorEventHandler(OnUpdateError);
        mCloudRecoBehaviour.UnregisterOnStateChangedEventHandler(OnStateChanged);
        mCloudRecoBehaviour.UnregisterOnNewSearchResultEventHandler(OnNewSearchResult);
    }

    public void OnInitialized(CloudRecoBehaviour cloudRecoBehaviour)
    {
        Debug.Log("Cloud Reco initialized");
    }

    public void OnInitError(CloudRecoBehaviour.InitError initError)
    {
        Debug.Log("Cloud Reco init error " + initError.ToString());
    }

    public void OnUpdateError(CloudRecoBehaviour.QueryError updateError)
    {
        Debug.Log("Cloud Reco update error " + updateError.ToString());

    }

    public void OnStateChanged(bool scanning)
    {
        mIsScanning = scanning;

        if (scanning)
        {
            // Clear all known targets
        }
    }

    // Here we handle a cloud target recognition event
    public void OnNewSearchResult(CloudRecoBehaviour.CloudRecoSearchResult cloudRecoSearchResult)
    {
        // Store the target metadata
        mTargetMetadata = cloudRecoSearchResult.MetaData;
        
        // Access the metadata from the cloud target
        string planetJson = mTargetMetadata;

        Planet planet = JsonUtility.FromJson<Planet>(planetJson);
        
        // Start downloading the image from the URL
        StartCoroutine(SetPlanetScanned(planet));

        // Build augmentation based on target 
        if (ImageTargetTemplate)
        {
            /* Enable the new result with the same ImageTargetBehaviour: */
            mCloudRecoBehaviour.EnableObservers(cloudRecoSearchResult, ImageTargetTemplate.gameObject);
        }
    }
    
    void OnGUI() {
        // Display current 'scanning' status
        GUI.Box (new Rect(100,100,200,50), mIsScanning ? "Scanning" : "Not scanning");
        // Display metadata of latest detected cloud-target
        GUI.Box (new Rect(100,200,200,50), "Metadata: " + mTargetMetadata);
    }
    public void TestPlanetEarth(string json)
    {
        Planet planet = JsonUtility.FromJson<Planet>(json);
        print(planet.Name);
        print(planet.URL);
        StartCoroutine(SetPlanetScanned(planet));
    }
    private IEnumerator SetPlanetScanned(Planet planet)
    {
        string url = planet.URL;

        sphere.SetActive(false);
        using (UnityWebRequest webRequest = UnityWebRequestTexture.GetTexture(url))
        {
            yield return webRequest.SendWebRequest();

            if (webRequest.result is UnityWebRequest.Result.ConnectionError or UnityWebRequest.Result.ProtocolError)
            {
                Debug.LogError("Error downloading image: " + webRequest.error);
                yield break;
            }

            // Get the downloaded texture
            Texture2D texture = DownloadHandlerTexture.GetContent(webRequest);

            // Create a new material with the specified shader
            // Use URP/Lit shader for URP compatibility
            Material material = new Material(Shader.Find("Universal Render Pipeline/Lit"));
            material.mainTexture = texture;
            
            mGameController.PlanetScanned(planet);

            // Assign the new material to the sphere
            if (sphere != null)
            {
                sphere.GetComponent<Renderer>().material = material;
                sphere.SetActive(true);
            }
        }
    }
}