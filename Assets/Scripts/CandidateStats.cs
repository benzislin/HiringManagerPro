using UnityEngine;

public class CandidateStats : MonoBehaviour
{
    // Visible (show these in UI)
    public string candidateName;
    public string pronouns;
    public int experienceYears;   // 0–15
    public int educationTier;     // 0–3

    // Core attributes (1–100)
    public int technical;         // TP
    public int adaptability;      // AD
    public int cultureAdd;        // CA
    public int longevity;         // LG

    // Hidden (reveal later / used for scoring)
    public int hiddenPotential;   // HP (hidden)
    public int maintenanceCost;   // MC (hidden “cost”)

    // DEI tags (for diversity score) - keep abstract for demo
    public int backgroundType;    // 0=Traditional, 1=NonTraditional
    public int thinkingStyle;     // 0=Standard, 1=NeurodivergentTrait (abstract)

    public float ValueScore()
    {
        // Base value (weighted)
        float bv = 0.35f * technical + 0.20f * adaptability + 0.20f * cultureAdd + 0.15f * longevity + 0.10f * hiddenPotential;
        // Net value
        return bv - maintenanceCost;
    }
}