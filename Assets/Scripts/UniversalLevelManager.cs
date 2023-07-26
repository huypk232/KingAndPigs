using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class UniversalLevelManager : MonoBehaviour
{
    private void Start() {
        // int level = LevelSelector.selectedLevel;
        // Debug.Log(level);
    }

    public void GoBackToMainMenu(){
        SceneManager.LoadScene("Main Menu");
    }
}
