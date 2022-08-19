using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Director : MonoBehaviour
{
    public int gameW = 10; //게임판 넓이
    public int gameH = 20; //게임판 높이

    public Transform[,] sheet; //테트리스가 저장되는 2차원 배열
    private Score score; // 점수
    private Swap swap;

    //패널들
    public GameObject endPanel; 
    public GameObject gamePanel;
    public GameObject pausePanel;
    public GameObject titlePanel;

    public Text EndText; //게임엔드시 나타나는 팝업 텍스트

    public bool isPlay = false;
    public bool isPause = false;
    void Start()
    {
        sheet = new Transform[gameW, gameH]; //게임판 생성
        score = FindObjectOfType<Score>(); //Score 스크립트 사용을 위한 변수 지정
        swap = FindObjectOfType<Swap>();

        transform.position = new Vector3(Mathf.RoundToInt(gameW / 2), gameH - 3, 0); //오브젝트의 위치 조정
    }

    void Update()
    {
        if (Input.GetKeyDown(KeyCode.Escape))
        {
            if (isPlay == true)
            {
                if (isPause == false)
                {
                    FindObjectOfType<Button>().GamePause();
                    isPause = true;
                }
                else if (isPause == true)
                {
                    FindObjectOfType<Button>().GameResume();
                    isPause = false;
                }
            }
        }

        if (Input.GetKeyDown(KeyCode.R))
        {
            if (isPlay == true)
            {
                if (isPause == false)
                {
                    FindObjectOfType<Button>().GameStart();
                }
            }
        }

        CheckLines(); //매프레임 CheckLines()실행
    }

    public void CheckLines()
    {
        for (int y = 0; y < gameH; y++) //제일 밑 줄 부터 제일 위 줄 까지 반복한다.
        {
            int count = 0; //줄이 넘어갈 때 마다 count를 0으로 초기화

            for (int x = 0; x < gameW; x++) //줄의 첫 번째 칸 부터 마지막 칸 까지 반복한다.
            {
                if (sheet[x, y] != null) //칸이 비어있지 않다면 실행
                {
                    count += 1; //count를 1증가
                }

                if (count == 10) //count가 10일 경우 실행
                {
                    for (int a = 0; a < gameW; a++) //해당 줄(y)의 첫번째 칸 부터 마지막 칸 까지 반복한다.
                    {
                        Destroy(sheet[a, y].gameObject); //오브젝트를 제거한다.
                        sheet[a, y] = null; //오브젝트가 저장되어 있던 배열을 비운다. 
                    }

                    for (int b = y; b < gameH; b++) //해당 줄 부터 마지막 줄 까지 반복한다.
                    {
                        for (int a = 0; a < gameW; a++) //줄의 첫번째 칸 부터 마지막 칸 까지 반복한다.
                        {
                            if (b + 1 < gameH) //윗 줄이 마지막 줄 보다 아래일 경우 실행
                            {
                                if (sheet[a, b + 1] != null) //윗 줄이 비어있지 않으면 실행
                                {
                                    sheet[a, b] = sheet[a, b + 1]; //윗 줄에 해당하는 배열의 값을 비어있는 아랫줄로 옮긴다. 
                                    sheet[a, b].gameObject.transform.position += new Vector3(0, -1, 0); //옮겨진 값에 해당하는 오브젝트의 위치를 아래로 내린다.
                                    sheet[a, b + 1] = null;  //윗 줄의 배열을 비운다.
                                }
                            }
                        }
                    }
                    score.AddCombo(); //점수를 더한다.
                }
            }
        }
    }

    public void GameEnd() //게임 종료시 실행되는 함수
    {
        gameObject.GetComponent<AudioSource>().Stop();
        FindObjectOfType<Sound>().AudioPlay("Over");

        Time.timeScale = 0; //프레임을 멈춘다.
        gamePanel.SetActive(false); //gamePanel 오브젝트를 비활성화한다.
        endPanel.SetActive(true); //endPanel 오브젝트를 비활성화한다.

        if (score.score >= score.bestScore) //Score 스크립트의 score 변수의 값이 bestScore 값보다 크거나 같을 경우 실행한다.
        {
            PlayerPrefs.SetFloat(score.key, score.score); //Score 스크립트의 key값을 score 값으로 저장한다.
        }

        //EndText의 text를 변경한다.
        EndText.text = "Best Score\n" + Mathf.Round(score.bestScore).ToString() + "\nScore\n" + Mathf.Round(score.score).ToString();

        ClearTetromino(); //ClearTetromino 실행
    }

    public void ClearTetromino() //테트리스 삭제 함수
    {
        GameObject[] SetTetromino = GameObject.FindGameObjectsWithTag("Set"); //생성된 테트리스 오브젝트를 저장하는 배열
        
        GameObject tetromino = GameObject.FindWithTag("Tetromino");
        GameObject preview = GameObject.FindWithTag("Preview");
        Destroy(preview);
        Destroy(tetromino);

        FindObjectOfType<Generator>().BoxReset();

        if (tetromino != null) //배열이 비어있지 않을경우 실행
        {
            for (int i = 0; i < SetTetromino.Length; i++) //배열의 처음부터 끝까지 반복한다.
            {
                Destroy(SetTetromino[i]);
            }
        }

        for (int y = 0; y < gameH; y++) //테트리스를 저장하는 2차원 배열을 초기화하기 위한 반복문
        {
            for (int x = 0; x < gameW; x++)
            {
                sheet[x, y] = null;
            }
        }
    }
}
