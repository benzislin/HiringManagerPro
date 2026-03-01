using System;
using UnityEngine;
using TMPro;
using System.Collections.Generic;
using System.Collections;
using UnityEngine.SceneManagement;

[Serializable]
public class CandidateRecord
{
    public string candidateName;
    public string pronouns;

    public int experienceYears;
    public int educationTier;

    public int technical;
    public int adaptability;
    public int cultureAdd;
    public int longevity;

    public int hiddenPotential;
    public int maintenanceCost;

    public int backgroundType;
    public int thinkingStyle;
    public GameObject characterPrefab;
}