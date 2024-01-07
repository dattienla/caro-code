using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class TimeRush : MonoBehaviour
{
    [SerializeField] private float timeLeft = 30f; // thời gian cho mỗi nước đi 
    [SerializeField] private TMP_Text txt;
    [SerializeField] private string stringOfPlayer; // để biết thời gian của ai phải chạy

    private BoardManager board;
    private GameManager game;
    // Start is called before the first frame update
    void Start()
    {
        board = FindObjectOfType<BoardManager>();
        game = FindObjectOfType<GameManager>();
    }

    // Update is called once per frame
    void Update()
    {
        if (board.currentTurn == stringOfPlayer)
        {
            timeLeft -= Time.deltaTime;
            if (timeLeft > 0)
            {
                ShowTime(timeLeft);
            }
            else
            {
                timeLeft = 0;
                if (board.currentTurn == "o")
                {
                    game.Player1EndGame();

                }
                else if (board.currentTurn == "x")
                {
                    game.Player2EndGame();

                }
            }
        }
        else
        {
            timeLeft = 30f;
            ShowTime(timeLeft);
        }
    }

    void ShowTime(float t) // đổi từ giây ra phút, giay và in ra text
    {
        float m = Mathf.FloorToInt(t / 60);
        float s = Mathf.FloorToInt(t % 60);
        txt.text = string.Format("{0:00} : {1:00}", m, s);
    }
}
