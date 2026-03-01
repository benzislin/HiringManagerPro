using UnityEngine;
using TMPro; // NEW: Required for editing TextMeshPro UI
using System.Collections.Generic;

public class EndScreenManager : MonoBehaviour
{
    [Header("Your 5 End Scene Characters")]
    public GameObject[] placeholders; 

    [Header("Their Name Tags")]
    // NEW: Array to hold your 5 Text objects
    public TextMeshProUGUI[] nameLabels; 

    void Start()
    {
        if (GlobalData.FinalTeam == null || GlobalData.FinalTeam.Count == 0)
        {
            Debug.LogWarning("No team data found!");
            return;
        }

        for (int i = 0; i < GlobalData.FinalTeam.Count; i++)
        {
            if (i >= placeholders.Length) break;

            // Grab the full candidate record so we can access BOTH the prefab and the name
            CandidateRecord candidate = GlobalData.FinalTeam[i];
            GameObject hiredPrefab = candidate.characterPrefab;

            if (hiredPrefab != null)
            {
                Transform spot = placeholders[i].transform;

                GameObject clone = Instantiate(hiredPrefab);
                clone.transform.SetParent(spot.parent, false);

                clone.transform.localPosition = spot.localPosition;
                clone.transform.localRotation = spot.localRotation;
                clone.transform.localScale = spot.localScale;

                placeholders[i].SetActive(false);
            }

            // NEW: Apply the candidate's name to the correct UI Text element
            if (i < nameLabels.Length && nameLabels[i] != null)
            {
                // You can customize this! E.g., $"{candidate.candidateName}\n({candidate.pronouns})"
                nameLabels[i].text = candidate.candidateName; 
            }
        }
    }
}