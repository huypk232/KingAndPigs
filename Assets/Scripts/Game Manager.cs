using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class GameManager : MonoBehaviour
{
    public static GameManager instance;

    // public Door door; // target

    // public GameObject levels;
    public GameObject stageCompleteCanvas;
    public GameObject pauseCanvas;

    private static float diamonds;
    private static float jades;
    // private int currentLevel;

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

    public void ColectDiamond()
    {
        diamonds += 1;
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
