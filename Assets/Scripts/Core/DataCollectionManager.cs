using Codice.CM.Common;
using UnityEngine;
using UnityEngine.Events;

public class DataCollectionManager : MonoBehaviour
{
    public static DataCollectionManager Instance { get; private set; }

    public enum MachineType
    {
        Buoy,
        Seismic,
        WindData
    }

    public InformationGathered informationGathered;
    public ObjectiveUIManager uiManager;
    public UnityEvent onAllInformationGathered;

    private void Awake()
    {
        // Singleton enforcement
        if (Instance != null && Instance != this)
        {
            Debug.LogWarning("Multiple DataCollectionManager instances found, destroying extra.");
            Destroy(gameObject);
            return;
        }

        Instance = this;
        DontDestroyOnLoad(gameObject); // Optional: persist between scenes
        informationGathered.OceanDataGathered = false;
        informationGathered.SeismicDataGathered = false;
        informationGathered.WindDataGathered = false;
    }

    public void CollectData(MachineType type)
    {
        if (informationGathered == null)
        {
            informationGathered = new InformationGathered();
        }

        switch (type)
        {
            case MachineType.Buoy:
                informationGathered.OceanDataGathered = true;
                break;
            case MachineType.Seismic:
                informationGathered.SeismicDataGathered = true;
                break;
            case MachineType.WindData:
                informationGathered.WindDataGathered = true;
                break;
            default:
                Debug.LogError("Unknown machine type");
                break;
        }

        // Update UI
        if (uiManager != null)
        {
            uiManager.UpdateObjectives(informationGathered);
        }

        // Check if all data has been collected
        if (informationGathered.OceanDataGathered && informationGathered.SeismicDataGathered && informationGathered.WindDataGathered)
        {
            onAllInformationGathered?.Invoke();
        }
    }
}
