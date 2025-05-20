using UnityEngine;

[CreateAssetMenu(fileName = "InformationGathered", menuName = "Scriptable Objects/InformationGathered")]
public class InformationGathered : ScriptableObject
{
    bool OceanDataGathered;
    bool SeismicDataGathered;
    bool WindDataGathered;
}
