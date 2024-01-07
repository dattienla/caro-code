using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.SceneManagement;

public class GameManager : MonoBehaviour
{
    [SerializeField] private GameObject inGamePanel;
    [SerializeField] private GameObject player1EndGamePanel;
    [SerializeField] private GameObject player2EndGamePanel;
    [SerializeField] private GameObject pauseGameImage;
    [SerializeField] private GameObject luatChoiPanel;

    public BoardManager board;

    public string gameMode; // để biết chế độ với máy hay với người

    private void Start()
    {
        board = FindObjectOfType<BoardManager>();
        InGame(gameMode);
        Time.timeScale = 1.0f;
    }
    public void InGame(string s)
    {
        if (s == "PVP") // chế độ đánh với người 
        {
            inGamePanel.SetActive(true);
            player1EndGamePanel.SetActive(false);
            player2EndGamePanel.SetActive(false);
            pauseGameImage.SetActive(false);
            luatChoiPanel.SetActive(false);
        }
        else if (s == "PVC") // chế độ đánh với máy
        {
            inGamePanel.SetActive(true);
            luatChoiPanel.SetActive(false);
            player1EndGamePanel.SetActive(false);
            player2EndGamePanel.SetActive(false);
            pauseGameImage.SetActive(false);
            StartCoroutine(ComputerFightFirst());
        }

    }
    public void Player1EndGame()
    {
        player1EndGamePanel.SetActive(true);
        Time.timeScale = 0;
    }
    public void Player2EndGame()
    {
        player2EndGamePanel.SetActive(true);
        Time.timeScale = 0;
    }
    public void PauseGame()
    {
        pauseGameImage.SetActive(true);
        Time.timeScale = 0;
    }
    public void RePlayGame()
    {
        SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex);
    }
    public void MenuGamePanel()
    {
        SceneManager.LoadScene("MenuScene");
    }
    public void ContinueGame()
    {
        luatChoiPanel.SetActive(false);
        pauseGameImage.SetActive(false);
        Time.timeScale = 1;
    }
    public void LuatChoi()
    {
        luatChoiPanel.SetActive(true);
        Time.timeScale = 0;
    }


    #region AI
    private long[] mangDiemTanCong = new long[7] { 0, 16, 670, 11000, 200000, 200000, 200000 };
    private long[] mangDiemPhongNgu = new long[7] { 0, 5, 210, 2700, 45000, 45000, 45000 };
    private long[] mangDiemBlockTanCong = new long[7] { 0, 1, 50, 440, 200000, 200000, 200000 };
    private long[] mangDiemBlockPhongNgu = new long[7] { 0, 1, 5, 290, 45000, 45000, 45000 };

    // Máy đánh nước đầu ở giữa bàn cờ
    IEnumerator ComputerFightFirst()
    {
        yield return new WaitForSeconds(0.8f);
        board.arrayCell[10, 10].SetCell("x");
        board.mat[10, 10] = "x";
        board.DoiLuotDi();
    }

    // Máy đánh

    public void ComputerFight()
    {
        int rowMax = 1;
        int colMax = 1;
        long diemMax = 0;
        for (int i = 1; i <= 20; i++)
        {
            for (int j = 1; j <= 20; j++)
            {
                if (board.mat[i, j] == "")
                {
                    long diemTanCong = DiemTC_DuyetDoc(i, j) + DiemTC_DuyetNgang(i, j) + DiemTC_DuyetCheoPhai(i, j) + DiemTC_DuyetCheoTrai(i, j);
                    long diemPhongThu = DiemPT_DuyetDoc(i, j) + DiemPT_DuyetNgang(i, j) + DiemPT_DuyetCheoPhai(i, j) + DiemPT_DuyetCheoTrai(i, j);
                    long diemTrungGian = diemTanCong > diemPhongThu ? diemTanCong : diemPhongThu;
                    if (diemMax < diemTrungGian)
                    {
                        diemMax = diemTrungGian;
                        rowMax = i;
                        colMax = j;
                    }
                }
            }
        }
        if (board.currentTurn == "x")
        {
            board.arrayCell[rowMax, colMax].SetCell("x");
            board.mat[rowMax, colMax] = "x";
            if (board.Check(rowMax, colMax))
            {
                Player1EndGame();
            }
            board.DoiLuotDi();
        }
    }

    long DiemTC_DuyetDoc(int row, int col)
    {
        long diemTong = 0;
        int soQuanTa = 0;
        int soQuanDich = 0;
        int cnt1 = 1;
        int cnt2 = 1;
        for (int i = row - 1; i >= 1 && cnt1 <= 5; i--)
        {
            cnt1++;
            if (board.mat[i, col] == "o")
            {
                soQuanDich++;
                break;
            }
            else if (board.mat[i, col] == "x")
            {
                soQuanTa++;
            }
            else
            {
                break;
            }
        }
        for (int i = row + 1; i <= 20 && cnt2 <= 5; i++)
        {
            cnt2++;
            if (board.mat[i, col] == "o")
            {
                soQuanDich++;
                break;
            }
            else if (board.mat[i, col] == "x")
            {
                soQuanTa++;
            }
            else
            {
                break;
            }
        }
        if (soQuanDich == 2)
        {
            diemTong += mangDiemBlockTanCong[soQuanTa];
            if (soQuanTa == 3) diemTong /= 2;
        }
        else
        {
            diemTong += mangDiemTanCong[soQuanTa];
        }
        return diemTong;
    }
    long DiemTC_DuyetNgang(int row, int col)
    {
        int cnt1 = 1;
        int cnt2 = 1;
        long diemTong = 0;
        int soQuanDich = 0;
        int soQuanTa = 0;
        for (int i = col - 1; i >= 1 && cnt1 <= 5; i--)
        {
            cnt1++;
            if (board.mat[row, i] == "o")
            {
                soQuanDich++;
                break;
            }
            else if (board.mat[row, i] == "x")
            {
                soQuanTa++;
            }
            else
            {
                break;
            }
        }
        for (int i = col + 1; i <= 20 && cnt2 <= 5; i++)
        {
            cnt2++;
            if (board.mat[row, i] == "o")
            {
                soQuanDich++;
                break;
            }
            else if (board.mat[row, i] == "x")
            {
                soQuanTa++;
            }
            else
            {
                break;
            }
        }
        if (soQuanDich == 2)
        {
            diemTong += mangDiemBlockTanCong[soQuanTa];
            if (soQuanTa == 3) diemTong /= 2;
        }
        else
        {
            diemTong += mangDiemTanCong[soQuanTa];
        }
        return diemTong;
    }
    long DiemTC_DuyetCheoPhai(int row, int col)
    {
        int cnt1 = 1;
        int cnt2 = 1;
        long diemTong = 0;
        int soQuanTa = 0;
        int soQuanDich = 0;
        for (int i = row - 1; i >= 1 && cnt1 <= 5; i--)
        {
            cnt1++;

            if (board.mat[i, col + (row - i)] == "o")
            {
                soQuanDich++;
                break;
            }
            else if (board.mat[i, col + (row - i)] == "x")
            {
                soQuanTa++;
            }
            else
            {
                break;
            }
        }
        for (int i = row + 1; i <= 20 && cnt2 <= 5; i++)
        {
            cnt2++;
            if (board.mat[i, col - (i - row)] == "o")
            {
                soQuanDich++;
                break;
            }
            else if (board.mat[i, col - (i - row)] == "x")
            {
                soQuanTa++;
            }
            else
            {
                break;
            }
        }
        if (soQuanDich == 2)
        {
            diemTong += mangDiemBlockTanCong[soQuanTa];
            if (soQuanTa == 3) diemTong /= 2;
        }
        else
        {
            diemTong += mangDiemTanCong[soQuanTa];
        }
        return diemTong;
    }
    long DiemTC_DuyetCheoTrai(int row, int col)
    {
        int cnt1 = 1;
        int cnt2 = 1;
        long diemTong = 0;
        int soQuanTa = 0;
        int soQuanDich = 0;
        for (int i = col - 1; i >= 1 && cnt1 <= 5; i--)
        {
            cnt1++;
            if (board.mat[row - (col - i), i] == "o")
            {
                soQuanDich++;
                break;
            }
            else if (board.mat[row - (col - i), i] == "x")
            {
                soQuanTa++;
            }
            else
            {
                break;
            }
        }
        for (int i = col + 1; i <= 20 && cnt2 <= 5; i++)
        {
            cnt2++;
            if (board.mat[row + (i - col), i] == "o")
            {
                soQuanDich++;
                break;
            }
            else if (board.mat[row + (i - col), i] == "x")
            {
                soQuanTa++;
            }
            else
            {
                break;
            }
        }
        if (soQuanDich == 2)
        {
            diemTong += mangDiemBlockTanCong[soQuanTa];
            if (soQuanTa == 3) diemTong /= 2;
        }
        else
        {
            diemTong += mangDiemTanCong[soQuanTa];
        }
        return diemTong;
    }

    // Tính điểm phòng thủ 

    long DiemPT_DuyetDoc(int row, int col)
    {
        int cnt1 = 1;
        int cnt2 = 1;
        long diemTong = 0;
        int soQuanTa = 0;
        int soQuanDich = 0;
        for (int i = row - 1; i >= 1 && cnt1 <= 5; i--)
        {
            cnt1++;
            if (board.mat[i, col] == "o")
            {
                soQuanDich++;
            }
            else if (board.mat[i, col] == "x")
            {
                soQuanTa++;
                break;
            }
            else
            {
                break;
            }
        }
        for (int i = row + 1; i <= 20 && cnt2 <= 5; i++)
        {
            cnt2++;
            if (board.mat[i, col] == "o")
            {
                soQuanDich++;
            }
            else if (board.mat[i, col] == "x")
            {
                soQuanTa++;
                break;
            }
            else
            {
                break;
            }
        }
        if (soQuanTa == 2)
        {
            diemTong += mangDiemBlockPhongNgu[soQuanDich];
            if (soQuanDich == 3) diemTong /= 2;
        }
        else
        {
            diemTong += mangDiemPhongNgu[soQuanDich];
        }
        return diemTong;
    }
    long DiemPT_DuyetNgang(int row, int col)
    {
        int cnt1 = 1;
        int cnt2 = 1;
        int soQuanTa = 0;
        long diemTong = 0;
        int soQuanDich = 0;
        for (int i = col - 1; i >= 1 && cnt1 <= 5; i--)
        {
            cnt1++;
            if (board.mat[row, i] == "o")
            {
                soQuanDich++;
            }
            else if (board.mat[row, i] == "x")
            {
                soQuanTa++;
                break;
            }
            else
            {
                break;
            }
        }
        for (int i = col + 1; i <= 20 && cnt2 <= 5; i++)
        {
            cnt2++;
            if (board.mat[row, i] == "o")
            {
                soQuanDich++;
            }
            else if (board.mat[row, i] == "x")
            {
                soQuanTa++;
                break;
            }
            else
            {
                break;
            }
        }
        if (soQuanTa == 2)
        {
            diemTong += mangDiemBlockPhongNgu[soQuanDich];
            if (soQuanDich == 3) diemTong /= 2;
        }
        else
        {
            diemTong += mangDiemPhongNgu[soQuanDich];
        }
        return diemTong;
    }
    long DiemPT_DuyetCheoPhai(int row, int col)
    {
        int cnt1 = 1;
        int cnt2 = 1;
        long diemTong = 0;
        int soQuanDich = 0;
        int soQuanTa = 0;
        for (int i = row - 1; i >= 1 && cnt1 <= 5; i--)
        {
            cnt1++;
            if (board.mat[i, col + (row - i)] == "o")
            {
                soQuanDich++;
            }
            else if (board.mat[i, col + (row - i)] == "x")
            {
                soQuanTa++;
                break;
            }
            else
            {
                break;
            }
        }
        for (int i = row + 1; i <= 20 && cnt2 <= 5; i++)
        {
            cnt2++;
            if (board.mat[i, col - (i - row)] == "o")
            {
                soQuanDich++;
            }
            else if (board.mat[i, col - (i - row)] == "x")
            {
                soQuanTa++;
                break;
            }
            else
            {
                break;
            }
        }
        if (soQuanTa == 2)
        {
            diemTong += mangDiemBlockPhongNgu[soQuanDich];
            if (soQuanDich == 3) diemTong /= 2;
        }
        else
        {
            diemTong += mangDiemPhongNgu[soQuanDich];
        }
        return diemTong;
    }
    long DiemPT_DuyetCheoTrai(int row, int col)
    {
        int cnt1 = 1;
        int cnt2 = 1;
        long diemTong = 0;
        int soQuanDich = 0;
        int soQuanTa = 0;
        for (int i = col - 1; i >= 1 && cnt1 <= 5; i--)
        {
            cnt1++;
            if (board.mat[row - (col - i), i] == "o")
            {
                soQuanDich++;
            }
            else if (board.mat[row - (col - i), i] == "x")
            {
                soQuanTa++;
                break;
            }
            else
            {
                break;
            }
        }
        for (int i = col + 1; i <= 20 && cnt2 <= 5; i++)
        {
            cnt2++;
            if (board.mat[row + (i - col), i] == "o")
            {
                soQuanDich++;
            }
            else if (board.mat[row + (i - col), i] == "x")
            {
                soQuanTa++;
                break;
            }
            else
            {
                break;
            }
        }
        if (soQuanTa == 2)
        {
            diemTong += mangDiemBlockPhongNgu[soQuanDich];
            if (soQuanDich == 3) diemTong /= 2;
        }
        else
        {
            diemTong += mangDiemPhongNgu[soQuanDich];
        }
        return diemTong;
    }

    #endregion
}


