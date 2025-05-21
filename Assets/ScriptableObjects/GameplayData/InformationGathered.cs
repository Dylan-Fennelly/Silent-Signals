using UnityEngine;

[CreateAssetMenu(fileName = "InformationGathered", menuName = "Scriptable Objects/InformationGathered")]
public class InformationGathered : ScriptableObject
{
   public bool OceanDataGathered;
   public bool SeismicDataGathered;
   public bool WindDataGathered;

}
