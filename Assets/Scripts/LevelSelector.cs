using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class LevelSelector : MonoBehaviour
{
    public static int selectedLevel;
    public int level;
    public Text levelText;

    private void Start() {
        // levelText.text = level.ToString();    
    }

    public void OpenScene() {
        SceneManager.LoadScene("Level " + level.ToString());
    }
}
