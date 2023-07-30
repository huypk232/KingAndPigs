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
        try
        {
            SceneManager.LoadScene("Level " + level.ToString());
        }
        catch (Exception e)
        {
            Debug.Log(e);
        }
        finally
        {
            GameManager.instance.OnComingStageNotice();
        }
    }
}
