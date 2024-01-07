using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.UI;

public class BoardManager : MonoBehaviour
{
    
    public string currentTurn = "x"; 
    public Transform cellpre;
    public Transform boardpre;
    public string[,] mat = new string[100,100]; // mảng lưu kí tự xem ô này thuộc sở hữu của X hay O
    public CellManager[,] arrayCell = new CellManager[25, 25]; // mảng lưu tọa độ ô cờ

    // Start is called before the first frame update
    private void Start()
    {
        CreateBoard();
    }
    // Tạo bàn cờ
    public void CreateBoard()
    {
        for (int i = 1; i <= 20; i++)
        {
            for (int j = 1; j <= 20; j++)
            {
                Transform celltrans = Instantiate(cellpre, boardpre);
                CellManager cell = celltrans.GetComponent<CellManager>();
                arrayCell[i, j] = cell;
                cell.row = i;
                cell.col = j;
                mat[i, j] = "";
            }
        }
    }
    
    // Đổi lượt đi 
    public void DoiLuotDi()
    {
        if (currentTurn == "x")
        {
            currentTurn = "o";
        }
        else if (currentTurn == "o")
        {
            currentTurn = "x";
        }
    }

    // Check xem đủ điều kiện thắng chưa?
    public bool Check(int row, int col)
    {
        
        int count1 = 0;
        int count2 = 0;
        int count3 = 0;
        int count4 = 0;
        // check hang doc
        for (int i = row - 1; i >= 1; i--)
        {
            if (mat[i, col] == currentTurn)
            {
                count1++;
            }
            else
            {
                break;
            }
        }
        for (int i = row + 1; i <= 20; i++)
        {
            if (mat[i, col] == currentTurn)
            {
                count1++;
            }
            else
            {
                break;
            }
        }
        // check hang ngang
        for (int i = col - 1; i >= 1; i--)
        {
            if (mat[row, i] == currentTurn)
            {
                count2++;
            }
            else
            {
                break;
            }
        }
        for (int i = col + 1; i <= 20; i++)
        {
            if (mat[row, i] == currentTurn)
            {
                count2++;
            }
            else
            {
                break;
            }
        }

        // check hang cheo phai 
        for (int i = row - 1; i >= 1; i--)
        {
            if (mat[i, col + (row - i)] == currentTurn)
            {
                count3++;
            }
            else
            {
                break;
            }
        }
        for (int i = row + 1; i <= 20; i++)
        {
            if (mat[i, col - (i - row)] == currentTurn)
            {
                count3++;
            }
            else
            {
                break;
            }
        }
        // check hang cheo trai
        for (int i = col - 1; i >= 1; i--)
        {
            if (mat[row - (col - i), i] == currentTurn)
            {
                count4++;
            }
            else
            {
                break;
            }
        }
        for (int i = col + 1; i <= 20; i++)
        {
            if (mat[row + (i - col), i] == currentTurn)
            {
                count4++;
            }
            else
            {
                break;
            }
        }
        if (count1 >= 4 || count2 >= 4 || count3 >= 4 || count4 >= 4)
        {
            return true;
        };
        return false;
    }
 

}
