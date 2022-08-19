using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Swap : MonoBehaviour
{
    public GameObject GameTetro;
    Director director;
    Generator generator;

    public int HoldNum = -1;
    public int GameNum = -1;

    public bool isSwap = false;

    public Transform HoldTransform;

    private void Start()
    {
        director = FindObjectOfType<Director>();
        generator = FindObjectOfType<Generator>();
    }

    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.X))
        {
            GameTetro = GameObject.FindWithTag("Tetromino");

            if (!isSwap)
            {
                if (HoldNum == -1)
                {
                    GameCheck(GameTetro);
                    HoldCheck();

                    TetroReset();
                    CreateHold();

                    generator.CreateTetromino();
                    isSwap = true;
                }
                else if(HoldNum != -1)
                {
                    GameCheck(GameTetro);
                    HoldCheck();

                    TetroReset();
                    CreateHold();

                    generator.SwapTetro(GameNum);
                    isSwap = true;
                }
                FindObjectOfType<Sound>().AudioPlay("Turn");
            }
        }
    }

    public void CreateHold()
    {
        GameObject HoldTetro = Instantiate(generator.Tetrominos[HoldNum], HoldTransform.transform.position,
            Quaternion.identity);
        HoldTetro.tag = "Hold";
        HoldTetro.transform.parent = HoldTransform.transform;
        HoldTetro.transform.localScale = new Vector3(0.2f, 0.2f, 0.2f);
        generator.NextPosition(HoldTetro);
        HoldTetro.GetComponent<Tetromino>().enabled = false;
    }

    public void GameCheck(GameObject tetro)
    {
        switch (tetro.name)
        {
            case "Shape1(Clone)":
                GameNum = 0;
                break;
            case "Shape2(Clone)":
                GameNum = 1;
                break;
            case "Shape3(Clone)":
                GameNum = 2;
                break;
            case "Shape4(Clone)":
                GameNum = 3;
                break;
            case "Shape5(Clone)":
                GameNum = 4;
                break;
            case "Shape6(Clone)":
                GameNum = 5;
                break;
            case "Shape7(Clone)":
                GameNum = 6;
                break;
        }
    }

    public void HoldCheck()
    {
        if (HoldNum == -1)
        {
            HoldNum = GameNum;
            GameNum = -1;
        }
        else
        {
            int empty;
            empty = GameNum;
            GameNum = HoldNum;
            HoldNum = empty;
        }
    }

    public void TetroReset()
    {
        GameObject gameT = GameObject.FindWithTag("Tetromino");
        GameObject gameP = GameObject.FindWithTag("Preview");
        GameObject holdT = GameObject.FindWithTag("Hold");
        Destroy(gameT);
        Destroy(gameP);
        Destroy(holdT);
    }

    public void SwapReset()
    {
        HoldNum = -1;
        GameNum = -1; 
        isSwap = false;
    }

}
