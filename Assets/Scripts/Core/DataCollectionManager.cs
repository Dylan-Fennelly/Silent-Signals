using Codice.CM.Common;
using UnityEngine;

public class DataCollectionManager : MonoBehaviour
{
    public enum MachineType
    {
        Buoy,
        Seismic,
        WindData
    }
    public InformationGathered informationGathered;

    public void CollectData(DataCollectionManager.MachineType type)
    {
        if (informationGathered == null)
        {
            informationGathered = new InformationGathered();
        }
        switch (type)
        {
            case DataCollectionManager.MachineType.Buoy:
                informationGathered.OceanDataGathered = true;
                break;
            case DataCollectionManager.MachineType.Seismic:
                informationGathered.SeismicDataGathered = true;
                break;
            case DataCollectionManager.MachineType.WindData:
                informationGathered.WindDataGathered = true;
                break;
            default:
                Debug.LogError("Unknown machine type");
                break;
        }

    }
}
