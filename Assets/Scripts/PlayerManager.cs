using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class PlayerManager : MonoBehaviour
{
    public static bool gameOverFlag;
    public GameObject gameOverPanel;
    public static int numberOfCoins;
    public static bool isGameStarted;
    public GameObject startingText;
    public Text coinsText;
    void Start()
    {

        Time.timeScale = 1;
        gameOverFlag = false;
        isGameStarted = false;
        numberOfCoins = 0;
    }

    // Update is called once per frame
    void Update()
    {
        if (gameOverFlag)
        {
            Time.timeScale = 0;
            gameOverPanel.SetActive(true);
        }
        coinsText.text = "Coins: " + numberOfCoins;
        if (SwipeManager.tap && isGameStarted == false)
        { 
            isGameStarted = true;
            numberOfCoins = 0;
            Destroy(startingText);
        }
    }
}
