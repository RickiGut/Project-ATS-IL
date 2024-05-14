using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
public class PlayerManager : MonoBehaviour
{

    public static bool isGameOver;
    public GameObject gameOverScreen;    

    private void Awake(){
        isGameOver = false;
    }

    // Update is called once per frame
    void Update()
    {
        if(isGameOver){
            gameOverScreen.SetActive(true);
        }
    }


    public void QuitGame(){
        SceneManager.LoadScene("MainMenu");
    }

    public void ReplayLevel(){
        SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex);
    }
}
