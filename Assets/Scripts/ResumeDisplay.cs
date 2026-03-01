using UnityEngine;
using TMPro;
using UnityEngine.SceneManagement;

public class ResumeDisplay : MonoBehaviour
{
    public TextMeshProUGUI resultsText;

    void Start()
    {
        if (GlobalData.FinalTeam == null || GlobalData.FinalTeam.Count == 0)
        {
            resultsText.text = "No team data found!";
            return;
        }

        // 1. Run your TeamEvaluator logic
        TeamEvaluator.TeamResult result = TeamEvaluator.Evaluate(GlobalData.FinalTeam);

        // 2. Display the overall report
        string fullDisplay = result.report;


        resultsText.text = fullDisplay;
    }
    
    public void RestartGame()
    {
        GlobalData.FinalTeam.Clear();
        SceneManager.LoadScene("Main Scene");
    }
}