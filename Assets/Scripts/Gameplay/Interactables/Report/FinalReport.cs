using System.Collections;
using UnityEngine;
using UnityEngine.UI;
using TMPro;
using UnityEngine.SceneManagement;

public class FinalReport : MonoBehaviour, IInteractable
{
    [Header("UI")]
    public CanvasGroup fadeGroup;
    public GameObject decisionPanel;        // Contains Publish and Destroy buttons
    public GameObject resultPanel;          // Contains consequence text and final buttons
    public TextMeshProUGUI consequenceText;
    public Button quitButton;
    public Button menuButton;

    [Header("Outcomes")]
    public FinalDecisionOutcome publishOutcome;
    public FinalDecisionOutcome destroyOutcome;

    [Header("Player Control")]
    public MonoBehaviour playerController;  

    public string GetInteractableName() => locked ? "Gather all data samples first!":"Final Report";
    bool locked = true;

    public void Interact()
    {
        if (locked) return;
        StartCoroutine(FadeAndShowChoices());
    }

    private IEnumerator FadeAndShowChoices()
    {
        // Disable player movement and unlock cursor
        if (playerController != null)
            playerController.enabled = false;

        Cursor.lockState = CursorLockMode.None;
        Cursor.visible = true;

        // Fade to black
        fadeGroup.gameObject.SetActive(true);
        float t = 0;
        while (t < 1f)
        {
            fadeGroup.alpha = t;
            t += Time.deltaTime / 2f;
            yield return null;
        }

        fadeGroup.alpha = 1;
        decisionPanel.SetActive(true);
    }

    public void ChoosePublish()
    {
        ShowResult(publishOutcome);
    }

    public void ChooseDestroy()
    {
        ShowResult(destroyOutcome);
    }

    private void ShowResult(FinalDecisionOutcome outcome)
    {
        decisionPanel.SetActive(false);
        consequenceText.text = outcome.consequenceMessage;
        resultPanel.SetActive(true);

        quitButton.onClick.AddListener(QuitGame);
        menuButton.onClick.AddListener(ReturnToMenu);
    }

    private void QuitGame()
    {
#if UNITY_EDITOR
        UnityEditor.EditorApplication.isPlaying = false;
#else
        Application.Quit();
#endif
    }

    private void ReturnToMenu()
    {
        SceneManager.LoadScene("Environment");
    }
    public void Unlock()
    {

        locked = false;
    }
}