using UnityEngine;

[CreateAssetMenu(fileName = "NewCandidate", menuName = "Interview/Candidate")]
public class InterviewData : ScriptableObject {
    public string candidateName;
    public string pronouns;
    public GameObject characterPrefab; // The 3D model for this person
    
    [Header("Resume Details")]
    public int experienceYears;
    public string education;
}