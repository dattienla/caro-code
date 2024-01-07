using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class CellManager : MonoBehaviour
{
    public Sprite xSprite;
    public Sprite oSprite;
    public int row = 0;
    public int col = 0;

    private Image img;
    private Button btn;
    private GameManager game;
    private void Awake()
    {
        img = GetComponent<Image>();
        btn = GetComponent<Button>();
    }
    // Start is called before the first frame update
    void Start()
    {
        game = FindObjectOfType<GameManager>();
        if (game.gameMode == "PVP")
        {
            btn.onClick.AddListener(OnClickVsHuman);
        }
        else if (game.gameMode == "PVC")
        {
            btn.onClick.AddListener(OnClickVsCom);
        }
    }
    // Thay đổi ảnh khi ấn thành X O
    public void SetCell(string s)
    {
        if (s == "x")
        {
            img.sprite = xSprite;
            btn.interactable = false;
        }
        if (s == "o")
        {
            img.sprite = oSprite;
            btn.interactable = false;
        }
    }

    // hàm delay cho máy
    IEnumerator Delay()
    {
        yield return new WaitForSeconds(0.8f);
        game.ComputerFight();
    }
    
    // Hàm click cho button Cell
    public void OnClickVsHuman()
    {
        SetCell(game.board.currentTurn);
        game.board.mat[row, col] = game.board.currentTurn;
        // check end game
        if (game.board.Check(row, col) && game.board.currentTurn == "x")
        {
            game.Player1EndGame();
        }
        else if (game.board.Check(row, col) && game.board.currentTurn == "o")
        {
            game.Player2EndGame();
        }
        // đổi lượt đi
        game.board.DoiLuotDi();
    }
    public void OnClickVsCom()
    {
        if (game.board.currentTurn == "o")
        {
            SetCell(game.board.currentTurn);
            game.board.mat[row, col] = game.board.currentTurn;
            if (game.board.Check(row, col) && game.board.currentTurn == "x")
            {
                game.Player1EndGame();
            }
            else if (game.board.Check(row, col) && game.board.currentTurn == "o")
            {
                game.Player2EndGame();
            }
            game.board.DoiLuotDi();
            StartCoroutine(Delay());

        }
    }
}
