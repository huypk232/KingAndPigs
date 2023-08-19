using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using TMPro;

public class GameManager : MonoBehaviour
{
    public static GameManager instance;

    public GameObject stageCompleteCanvas;
    public GameObject pauseCanvas;
    public GameObject gameOverCanvas;
    public GameObject onComingStageCanvas;
    public GameObject diamondCounter;

    private static int diamonds = 10;

    private void Awake() {
        if(instance == null)
        {
            instance = this;
            DontDestroyOnLoad(gameObject);
        } else if (instance != this)
        {
            Destroy(instance.gameObject);
            instance = this;
            DontDestroyOnLoad(gameObject);
        }
        diamondCounter.GetComponent<TextMeshProUGUI>().SetText(diamonds.ToString());        
    }

    void Update()
    {
        Pause(); // todo refactor
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
            FindObjectOfType<KingController>().Respawn();
            
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

    public void OnComingStageNotice()
    {
        stageCompleteCanvas.SetActive(false);
        onComingStageCanvas.SetActive(true);
    }

    private IEnumerator WaitGoThroughRoom()
    {
        yield return new WaitForSeconds(1f);
    }
}
