using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using TMPro;

public class GameManager : MonoBehaviour
{
    public static GameManager instance;

    // public Door door; // target

    // public GameObject levels;
    public GameObject stageCompleteCanvas;
    public GameObject pauseCanvas;
    public GameObject gameOverCanvas;
    // public GameObject healthBar;
    public GameObject diamondCounter;

    private static float diamonds = 10;
    private static float jades = 0;

    private void Awake() {
        if(instance != null)
        {
            
            Destroy(gameObject);
            return;
        }
        instance = this;
        DontDestroyOnLoad(gameObject);
    }

    // Start is called before the first frame update
    // void Start()
    // {
    //     instance = this;
    //     // diamonds = 0;
    //     // jades = 0;
    // }

    // Update is called once per frame
    void Update()
    {
        Pause();
    }

    void Pause()
    {
        if(Input.GetKeyDown(KeyCode.P))
        {
            pauseCanvas.SetActive(true);
            Time.timeScale = 0;
        }
    }    

    public void Resume()
    {
        Time.timeScale = 1;
    }

    public void Restart()
    {
        SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex);
        Time.timeScale = 1;
    }

    public void RespawnKing()
    {
        if(diamonds >= 10)
        {
            // respawn king
            diamonds -= 10;
            diamondCounter.GetComponent<TextMeshProUGUI>().SetText(diamonds.ToString());
            Time.timeScale = 1;
        } else {
            Debug.Log("Not enough diamonds");
        }

    }

    public void ColectDiamond()
    {
        diamonds += 1;
        diamondCounter.GetComponent<TextMeshProUGUI>().SetText(diamonds.ToString());
    }

    public void ColectJade()
    {
        jades += 1;
    }

    public void CompleteStage()
    {
        stageCompleteCanvas.SetActive(true);
    }

    public void NextStage()
    {
        stageCompleteCanvas.SetActive(false);
        SceneManager.LoadScene("Game");
    }

    public void BackToMainMenu()
    {
        SceneManager.LoadScene("Main Menu");
    }

    public void GameOver()
    {
        Time.timeScale = 0;
        gameOverCanvas.SetActive(true);
    }

    public void GoToRoom(GameObject currentRoom, GameObject targetRoom)
    {
        currentRoom.SetActive(false);
        targetRoom.SetActive(true);
    }



    private IEnumerator WaitGoThroughRoom()
    {
        yield return new WaitForSeconds(1f);
    }
}
