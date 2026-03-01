using UnityEngine;
using TMPro;

public class ResumeUI : MonoBehaviour
{
    [Header("Panel")]
    public GameObject resumePanel;

    [Header("Fields")]
    public TextMeshProUGUI nameText;
    public TextMeshProUGUI pronounsText;
    public TextMeshProUGUI expText;
    public TextMeshProUGUI educationText;
    public TextMeshProUGUI skillsText;
    public TextMeshProUGUI summaryText;

    private CandidateStats current;

    public void SetCandidate(CandidateStats c)
    {
        current = c;
    }

    public void OpenResume()
    {
        if (resumePanel == null) return;
        if (current == null) return;

        // Prefill like a resume (VISIBLE stats only)
        nameText.text = current.candidateName;
        pronounsText.text = current.pronouns;
        expText.text = $"{current.experienceYears} years";
        educationText.text = EducationTierToString(current.educationTier);

        skillsText.text =
            $"Technical: {current.technical}\n" +
            $"Adaptability: {current.adaptability}\n" +
            $"Culture Add: {current.cultureAdd}\n" +
            $"Longevity: {current.longevity}";

        summaryText.text = MakeSummary(current);

        resumePanel.SetActive(true);
    }

    public void CloseResume()
    {
        if (resumePanel == null) return;
        resumePanel.SetActive(false);
    }

    private string EducationTierToString(int tier)
    {
        switch (tier)
        {
            case 0: return "High School / GED";
            case 1: return "Community College / Associate";
            case 2: return "Bachelor's Degree";
            case 3: return "Elite / Top Program";
            default: return "Unknown";
        }
    }

    private string MakeSummary(CandidateStats c)
    {
        // Simple “resume blurb” generated from stats
        if (c.technical >= 85) return "Strong technical performer with proven execution.";
        if (c.adaptability >= 85) return "Highly adaptable learner with strong growth trajectory.";
        if (c.cultureAdd >= 85) return "Team-oriented contributor who elevates collaboration.";
        return "Well-rounded candidate with balanced strengths across core competencies.";
    }
}