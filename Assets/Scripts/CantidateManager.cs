using UnityEngine;
using TMPro;
using System.Collections.Generic;
using System.Collections;
using UnityEngine.SceneManagement;

public class CandidateManager : MonoBehaviour
{
    public StarRatingSwapFade starUI;
    public GameObject[] cityPeoplePrefabs;
    public TextMeshProUGUI statsText;
    public TextMeshProUGUI teamText;
    public AudioSource audioSource;
    public AudioClip hireSound;
    public AudioClip rejectSound;

    public Transform spawnPoint;
    public Transform interviewSeat;

    [Header("Settings")]
    public float animationDelay = 1.5f; 
    public int teamSize = 5;

    private GameObject currentCharacter;
    private GameObject currentPrefab; 
    
    private List<CandidateRecord> hiredTeam = new List<CandidateRecord>();

    private string[] maleNames = {"Nick", "James", "Ethan", "Jack", "Noah", "Kai", "Sam","Liam", "Lucas", "Mateo", "Leo", "Elias", "Sebastian", "Jackson","Aiden", "Owen", "Wyatt", "David", "Carlos", "Amir", "Julian","Caleb", "Isaac", "Marcus", "Zane", "Tariq", "Diego", "Finn","Omar", "Arthur", "Hassan", "Malik", "Beau", "Jude", "Victor"};

    private string[] femaleNames = {"Priya", "Emily", "Olivia", "Sophia", "Rose", "Hazel","Emma", "Charlotte", "Amelia", "Mia", "Isabella", "Amina", "Luna","Layla", "Chloe", "Zoe", "Nora", "Lily", "Eleanor", "Hannah","Maya", "Naomi", "Elena", "Fatima", "Kira", "Jasmine", "Ruby","Aisha", "Carmen", "Stella", "Iris", "Jade", "Nia", "Clara"
    };
    private string[] pronouns = { "she/her", "he/him", "they/them" };
    bool male;

    void Start()
    {
        if (cityPeoplePrefabs == null || cityPeoplePrefabs.Length == 0)
        {
            Debug.LogError("cityPeoplePrefabs is EMPTY.");
            return;
        }
        if (interviewSeat == null)
        {
            Debug.LogError("interviewSeat is NOT assigned.");
            return;
        }
        if (statsText == null)
        {
            Debug.LogError("statsText is NOT assigned.");
            return;
        }

        SpawnNewCandidate();
        UpdateTeamUI();
        UpdateStarsFromTeam();
    }

    public void NextCandidate(bool hired)
    {
        if (currentCharacter == null) return;

        CandidateStats stats = currentCharacter.GetComponent<CandidateStats>();
        Animator anim = currentCharacter.GetComponent<Animator>();

        if (hired && stats != null && hiredTeam.Count < teamSize)
        {
            CandidateRecord rec = new CandidateRecord
            {
                candidateName = stats.candidateName,
                pronouns = stats.pronouns,
                experienceYears = stats.experienceYears,
                educationTier = stats.educationTier,
                technical = stats.technical,
                adaptability = stats.adaptability,
                cultureAdd = stats.cultureAdd,
                longevity = stats.longevity,
                hiddenPotential = stats.hiddenPotential,
                maintenanceCost = stats.maintenanceCost,
                backgroundType = stats.backgroundType,
                thinkingStyle = stats.thinkingStyle,
                // NEW: Save the prefab into the record!
                characterPrefab = currentPrefab 
            };

            hiredTeam.Add(rec);
            UpdateTeamUI();
            UpdateStarsFromTeam();

            if (anim != null) anim.SetTrigger("hired");
            if (audioSource != null && hireSound != null) audioSource.PlayOneShot(hireSound);
        }
        else
        {
            if (anim != null) anim.SetTrigger("nothired");
            if (audioSource != null && rejectSound != null) audioSource.PlayOneShot(rejectSound);
        }

        CandidateMovement mover = currentCharacter.GetComponent<CandidateMovement>();
        if (mover != null) mover.StopAllCoroutines();

        StartCoroutine(ProcessNextCandidate());
    }

    private IEnumerator ProcessNextCandidate()
    {
        GameObject personToRemove = currentCharacter;
        currentCharacter = null; 
        currentPrefab = null; // Clear the prefab reference too

        yield return new WaitForSeconds(animationDelay);
        if (personToRemove != null) Destroy(personToRemove);

        if (hiredTeam.Count >= teamSize)
        {
            GlobalData.FinalTeam = new List<CandidateRecord>(hiredTeam);
            SceneManager.LoadScene("End Scene");
        }
        else
        {
            SpawnNewCandidate();
        }
    }
    
    int lastIndex = -1;
    
    private void SpawnNewCandidate()
    {
        int randomIndex = Random.Range(0, cityPeoplePrefabs.Length);
        while(randomIndex != -1 && randomIndex == lastIndex)
        {
            randomIndex = Random.Range(0, cityPeoplePrefabs.Length);
        }
        lastIndex = randomIndex;
        
        GameObject prefab = cityPeoplePrefabs[randomIndex];
        // NEW: Store this prefab so NextCandidate can access it later
        currentPrefab = prefab; 
        
        if(randomIndex < 4)
            male = false;
        else   
            male = true;

        currentCharacter = Instantiate(prefab, spawnPoint.position, spawnPoint.rotation);

        CandidateStats cs = currentCharacter.AddComponent<CandidateStats>();
        GenerateStats(cs);

        statsText.text =
            $"Name: {cs.candidateName} ({cs.pronouns})\n" +
            $"Experience: {cs.experienceYears} years\n" +
            $"Education Tier: {cs.educationTier}\n\n" +
            $"Technical: {cs.technical}\n" +
            $"Adaptability: {cs.adaptability}\n" +
            $"Culture Add: {cs.cultureAdd}\n" +
            $"Longevity: {cs.longevity}";

        CandidateMovement mover = currentCharacter.GetComponent<CandidateMovement>();
        if (mover == null) mover = currentCharacter.AddComponent<CandidateMovement>();
        StartCoroutine(mover.WalkToDesk(interviewSeat.position));
    }

    private void GenerateStats(CandidateStats c)
    {
        if(male)
        {
            c.candidateName = maleNames[Random.Range(0, maleNames.Length)];
            c.pronouns = pronouns[1];
        }
        else
        {
            c.candidateName = femaleNames[Random.Range(0, femaleNames.Length)];
            c.pronouns = pronouns[0];
        }
        
        if(Random.Range(0, 5) == 1) 
            c.pronouns = pronouns[2];
            
        c.experienceYears = Random.Range(0, 16);
        c.educationTier = Random.Range(0, 4);

        bool traditional = Random.value < 0.5f;
        c.backgroundType = traditional ? 0 : 1;
        c.thinkingStyle = (Random.value < 0.2f) ? 1 : 0;

        if (traditional)
        {
            c.technical = Random.Range(80, 101);       
            c.adaptability = Random.Range(55, 86);     
            c.cultureAdd = Random.Range(50, 86);       
            c.longevity = Random.Range(50, 86);        
            c.hiddenPotential = Random.Range(55, 86);  
            c.maintenanceCost = Random.Range(15, 36);  
        }
        else
        {
            c.technical = Random.Range(65, 91);        
            c.adaptability = Random.Range(80, 101);    
            c.cultureAdd = Random.Range(75, 101);      
            c.longevity = Random.Range(75, 101);       
            c.hiddenPotential = Random.Range(85, 101); 
            c.maintenanceCost = Random.Range(5, 16);   
        }

        if (c.thinkingStyle == 1)
        {
            c.hiddenPotential = Mathf.Clamp(c.hiddenPotential + 10, 1, 100);
            c.technical = Mathf.Clamp(c.technical + 8, 1, 100);
        }
    }

    private void UpdateTeamUI()
    {
        if (teamText == null) return;

        teamText.text = $"Team ({hiredTeam.Count}/{teamSize})\n";
        for (int i = 0; i < hiredTeam.Count; i++)
        {
            CandidateRecord c = hiredTeam[i]; 
            teamText.text += $"{i + 1}. {c.candidateName} ({c.pronouns}) | TP {c.technical} | CA {c.cultureAdd}\n";
        }
    }

    private void UpdateStarsFromTeam()
    {
        if (starUI == null) return;

        float score0to100 = 0f;
        if (hiredTeam.Count > 0)
        {
            float sum = 0f;
            float highestCandidateScore = 0f;

            foreach (var c in hiredTeam)
            {
                float candidateScore = 0.4f * c.technical + 0.25f * c.adaptability + 0.2f * c.cultureAdd + 0.15f * c.longevity;
                
                sum += candidateScore;
                
                if (candidateScore > highestCandidateScore) 
                    highestCandidateScore = candidateScore;
            }

            float rawAverage = sum / hiredTeam.Count;
            score0to100 = Mathf.Lerp(rawAverage, highestCandidateScore, 0.2f);
        }

        float starPercentage = Mathf.InverseLerp(55f, 85f, score0to100);
        int rating0to4 = Mathf.RoundToInt(starPercentage * 4f);
        rating0to4 = Mathf.Clamp(rating0to4, 0, 4);

        starUI.SetRating(rating0to4);
    }
}