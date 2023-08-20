using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class LevelSelector : MonoBehaviour
{
    public static int selectedLevel;
    public int level;

    public void OpenScene() {
        if (level == -1) {
            Application.Quit();
        } else if (SceneUtility.GetBuildIndexByScenePath("Assets/Scenes/Level " + level.ToString() + ".unity") >= 0) {
            SceneManager.LoadScene("Level " + level.ToString());
        } else {
            GameManager.instance.OnComingStageNotice();
        }
    }
}
