using System.Collections.Generic;
using UnityEngine;
using System.Linq;

public static class TeamEvaluator
{
    public struct TeamResult
    {
        public float performance;
        public float chemistry;
        public float diversity;
        public float costPenalty;
        public float finalScore; // 0–100
        public string report;
    }

   public static TeamResult Evaluate(List<CandidateRecord> team)
    {
            float avgTech = (float)team.Average(c => c.technical);
            float avgPot = (float)team.Average(c => c.hiddenPotential);
            float avgCost = (float)team.Average(c => c.maintenanceCost);

        float performance = (0.55f * avgTech + 0.45f * avgPot) - (0.8f * avgCost);

        float avgCulture = (float)team.Average(c => c.cultureAdd);

        // Homogeneity penalty = if all same backgroundType
        int distinctBackgrounds = team.Select(c => c.backgroundType).Distinct().Count();
        float homogeneityPenalty = (distinctBackgrounds == 1) ? 15f : 0f;

        float chemistry = Mathf.Clamp(avgCulture - homogeneityPenalty, 0f, 100f);

        // 3) Diversity score (simple index): mix of background + thinking style
        float nonTraditionalShare = team.Count(c => c.backgroundType == 1) / (float)team.Count;
        float neuroShare = team.Count(c => c.thinkingStyle == 1) / (float)team.Count;

        // Diversity index scaled to 0–100
        float diversity = Mathf.Clamp(60f * nonTraditionalShare + 40f * neuroShare, 0f, 100f);

        // Optional: apply “DEI bonus” to reflect team advantage (your game mechanic)
        // If diversity >= 40, small multiplier to performance (simulating better outcomes)
        float deiBonus = (diversity >= 40f) ? 1.10f : 1.0f; // MVP: 10% boost

        performance *= deiBonus;

        // 4) Cost penalty (for report clarity)
        float costPenalty = Mathf.Clamp(avgCost * 1.2f, 0f, 40f);

        // 5) Final score
        // Weighted blend (tune as you want)
        float finalScore =
            0.50f * Mathf.Clamp(performance, 0f, 100f) +
            0.30f * chemistry +
            0.20f * diversity;

        finalScore = Mathf.Clamp(finalScore, 0f, 100f);

        return new TeamResult
        {
            performance = Mathf.Clamp(performance, 0f, 100f),
            chemistry = chemistry,
            diversity = diversity,
            costPenalty = costPenalty,
            finalScore = finalScore,
            report =
                $"TEAM EVALUATION (5 hires)\n\n" +
                $"Performance: {Mathf.RoundToInt(Mathf.Clamp(performance, 0f, 100f))}/100\n" +
                $"Chemistry: {Mathf.RoundToInt(chemistry)}/100\n" +
                $"Diversity Index: {Mathf.RoundToInt(diversity)}/100\n" +
                $"Avg Maintenance Cost: {avgCost:F1}\n\n" +
                $"FINAL GRADE: {Mathf.RoundToInt(finalScore)}/100\n\n"
        };
    }
}