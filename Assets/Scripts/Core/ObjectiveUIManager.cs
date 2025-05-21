using TMPro;
using UnityEngine;

public class ObjectiveUIManager : MonoBehaviour
{
    [Header("Objective Text Elements")]
    public TextMeshProUGUI buoyObjectiveText;
    public TextMeshProUGUI seismicObjectiveText;
    public TextMeshProUGUI windObjectiveText;
    public TextMeshProUGUI finalObjectiveText; // Initially hidden

    private void Start()
    {
        InitialiseDefaultObjectiveText();
        // Ensure final objective is hidden at the start
        finalObjectiveText.gameObject.SetActive(false);
    }

    private void InitialiseDefaultObjectiveText()
    {
        buoyObjectiveText.text = "❌ Buoy Data Pending";
        seismicObjectiveText.text = "❌ Seismic Data Pending";
        windObjectiveText.text = "❌ Wind Data Pending";
        finalObjectiveText.gameObject.SetActive(false);
    }

    public void UpdateObjectives(InformationGathered gathered)
    {
        buoyObjectiveText.text = gathered.OceanDataGathered ? "Buoy Data Collected" : "Buoy Data Pending";
        seismicObjectiveText.text = gathered.SeismicDataGathered ? "Seismic Data Collected" : "Seismic Data Pending";
        windObjectiveText.text = gathered.WindDataGathered ? "Wind Data Collected" : "Wind Data Pending";

        bool allComplete = gathered.OceanDataGathered && gathered.SeismicDataGathered && gathered.WindDataGathered;

        if (allComplete)
        {
            finalObjectiveText.gameObject.SetActive(true);
            finalObjectiveText.text = "✔ All Preliminary Data Collected — Proceed to Final Analysis";
        }
        else
        {
            finalObjectiveText.gameObject.SetActive(false);
        }
    }
}
