using UnityEngine;

[CreateAssetMenu(fileName = "FinalDecisionOutcome", menuName = "Game/Final Decision Outcome", order = 1)]
public class FinalDecisionOutcome : ScriptableObject
{
    [Header("Display Settings")]
    public string decisionName = "Publish Reports";
    public string consequenceMessage = "Publishing has reduced corporate funding, but the data lives on.";

    [Header("Fade Settings")]
    public float fadeDuration = 2f;

    [Header("Game State Flags")]
    public bool isEthicalChoice = true;
    public bool increaseFunding = false;
    public bool damageEnvironment = false;
}
