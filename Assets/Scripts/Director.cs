using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Director : MonoBehaviour
{
    public int gameW = 10; //������ ����
    public int gameH = 20; //������ ����

    public Transform[,] sheet; //��Ʈ������ ����Ǵ� 2���� �迭
    private Score score; // ����
    private Swap swap;

    //�гε�
    public GameObject endPanel; 
    public GameObject gamePanel;
    public GameObject pausePanel;
    public GameObject titlePanel;

    public Text EndText; //���ӿ���� ��Ÿ���� �˾� �ؽ�Ʈ

    public bool isPlay = false;
    public bool isPause = false;
    void Start()
    {
        sheet = new Transform[gameW, gameH]; //������ ����
        score = FindObjectOfType<Score>(); //Score ��ũ��Ʈ ����� ���� ���� ����
        swap = FindObjectOfType<Swap>();

        transform.position = new Vector3(Mathf.RoundToInt(gameW / 2), gameH - 3, 0); //������Ʈ�� ��ġ ����
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

        CheckLines(); //�������� CheckLines()����
    }

    public void CheckLines()
    {
        for (int y = 0; y < gameH; y++) //���� �� �� ���� ���� �� �� ���� �ݺ��Ѵ�.
        {
            int count = 0; //���� �Ѿ �� ���� count�� 0���� �ʱ�ȭ

            for (int x = 0; x < gameW; x++) //���� ù ��° ĭ ���� ������ ĭ ���� �ݺ��Ѵ�.
            {
                if (sheet[x, y] != null) //ĭ�� ������� �ʴٸ� ����
                {
                    count += 1; //count�� 1����
                }

                if (count == 10) //count�� 10�� ��� ����
                {
                    for (int a = 0; a < gameW; a++) //�ش� ��(y)�� ù��° ĭ ���� ������ ĭ ���� �ݺ��Ѵ�.
                    {
                        Destroy(sheet[a, y].gameObject); //������Ʈ�� �����Ѵ�.
                        sheet[a, y] = null; //������Ʈ�� ����Ǿ� �ִ� �迭�� ����. 
                    }

                    for (int b = y; b < gameH; b++) //�ش� �� ���� ������ �� ���� �ݺ��Ѵ�.
                    {
                        for (int a = 0; a < gameW; a++) //���� ù��° ĭ ���� ������ ĭ ���� �ݺ��Ѵ�.
                        {
                            if (b + 1 < gameH) //�� ���� ������ �� ���� �Ʒ��� ��� ����
                            {
                                if (sheet[a, b + 1] != null) //�� ���� ������� ������ ����
                                {
                                    sheet[a, b] = sheet[a, b + 1]; //�� �ٿ� �ش��ϴ� �迭�� ���� ����ִ� �Ʒ��ٷ� �ű��. 
                                    sheet[a, b].gameObject.transform.position += new Vector3(0, -1, 0); //�Ű��� ���� �ش��ϴ� ������Ʈ�� ��ġ�� �Ʒ��� ������.
                                    sheet[a, b + 1] = null;  //�� ���� �迭�� ����.
                                }
                            }
                        }
                    }
                    score.AddCombo(); //������ ���Ѵ�.
                }
            }
        }
    }

    public void GameEnd() //���� ����� ����Ǵ� �Լ�
    {
        gameObject.GetComponent<AudioSource>().Stop();
        FindObjectOfType<Sound>().AudioPlay("Over");

        Time.timeScale = 0; //�������� �����.
        gamePanel.SetActive(false); //gamePanel ������Ʈ�� ��Ȱ��ȭ�Ѵ�.
        endPanel.SetActive(true); //endPanel ������Ʈ�� ��Ȱ��ȭ�Ѵ�.

        if (score.score >= score.bestScore) //Score ��ũ��Ʈ�� score ������ ���� bestScore ������ ũ�ų� ���� ��� �����Ѵ�.
        {
            PlayerPrefs.SetFloat(score.key, score.score); //Score ��ũ��Ʈ�� key���� score ������ �����Ѵ�.
        }

        //EndText�� text�� �����Ѵ�.
        EndText.text = "Best Score\n" + Mathf.Round(score.bestScore).ToString() + "\nScore\n" + Mathf.Round(score.score).ToString();

        ClearTetromino(); //ClearTetromino ����
    }

    public void ClearTetromino() //��Ʈ���� ���� �Լ�
    {
        GameObject[] SetTetromino = GameObject.FindGameObjectsWithTag("Set"); //������ ��Ʈ���� ������Ʈ�� �����ϴ� �迭
        
        GameObject tetromino = GameObject.FindWithTag("Tetromino");
        GameObject preview = GameObject.FindWithTag("Preview");
        Destroy(preview);
        Destroy(tetromino);

        FindObjectOfType<Generator>().BoxReset();

        if (tetromino != null) //�迭�� ������� ������� ����
        {
            for (int i = 0; i < SetTetromino.Length; i++) //�迭�� ó������ ������ �ݺ��Ѵ�.
            {
                Destroy(SetTetromino[i]);
            }
        }

        for (int y = 0; y < gameH; y++) //��Ʈ������ �����ϴ� 2���� �迭�� �ʱ�ȭ�ϱ� ���� �ݺ���
        {
            for (int x = 0; x < gameW; x++)
            {
                sheet[x, y] = null;
            }
        }
    }
}
