using UnityEngine;
using UnityEngine.SceneManagement; // Allows you to switch scenes

public class MenuController : MonoBehaviour {

    public void StartGame() {
        // "GameScene" must be the EXACT name of your main game scene file
        SceneManager.LoadScene("Transition"); 
    }

    public void QuitGame() {
        Application.Quit();
        Debug.Log("The player has quit the game.");
    }
}