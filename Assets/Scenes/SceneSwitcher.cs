using UnityEngine;
using UnityEngine.SceneManagement;

public class SceneSwitcher : MonoBehaviour
{
    public void GoToMainScene()
    {
        
        GameStats.HiredNames.Clear(); 
        
        SceneManager.LoadScene("MainMenu"); 
    }
}