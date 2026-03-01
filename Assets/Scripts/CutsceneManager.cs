using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI; // Required for the Slider
using System.Collections;

public class CutsceneManager : MonoBehaviour
{
    [Header("Settings")]
    public string nextSceneName = "Main Scene"; 
    public float waitTime = 5.0f;

    [Header("UI")]
    // Drag your UI Slider into this slot in the Inspector
    public Slider loadingBar; 

    [Header("Character Animations")]
    public Animator[] characterAnimators; 
    public string cutsceneTriggerName = "isWalking"; 

    void Start()
    {
        // 1. Reset the loading bar to 0 at the start
        if (loadingBar != null)
        {
            loadingBar.value = 0f;
        }

        // 2. Trigger animations
        foreach (Animator anim in characterAnimators)
        {
            if (anim != null)
            {
                anim.SetTrigger(cutsceneTriggerName);
            }
        }

        // 3. Start the timer and loading bar
        StartCoroutine(WaitAndLoad());
    }

    private IEnumerator WaitAndLoad()
    {
        float elapsedTime = 0f;

        // Gradually increase elapsedTime until it reaches the waitTime
        while (elapsedTime < waitTime)
        {
            elapsedTime += Time.deltaTime;
            
            // Update the slider's value (calculated as a percentage from 0 to 1)
            if (loadingBar != null)
            {
                loadingBar.value = elapsedTime / waitTime * 100; 
            }

            // Wait until the next frame before looping again
            yield return null; 
        }

        // Ensure the bar hits exactly 100% before transitioning
        if (loadingBar != null)
        {
            loadingBar.value = 1f;
        }

        SceneManager.LoadScene(nextSceneName);
    }
}