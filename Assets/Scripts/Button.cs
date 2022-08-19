using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Button : MonoBehaviour
{
    //�ٸ� ��ũ��Ʈ�� ���� �� �Լ��� ����ϱ� ���� ������ ���� ����
    private Score score;
    private Director director;
    private Generator generator;
    private Sound audioPlayer;
    
    private void Start()
    {
        //�ٸ� ��ũ��Ʈ�� ���� �� �Լ��� ����ϱ� ���� ������ ���� �ʱ�ȭ
        score = FindObjectOfType<Score>();
        director = FindObjectOfType<Director>();
        generator = FindObjectOfType<Generator>();
        audioPlayer = FindObjectOfType<Sound>();
    }

    public void GamePause() //���� �Ͻ� ���� 
    {
        audioPlayer.AudioPlay("Button");
        director.GetComponent<AudioSource>().Pause();
        director.isPause = true;

        Time.timeScale = 0; //������ ����
        director.pausePanel.SetActive(true); //pausePanel Ȱ��ȭ
    }

    public void GameResume() //���� �̾ ����
    {
        audioPlayer.AudioPlay("Button");
        director.GetComponent<AudioSource>().Play();
        director.isPause = false;

        director.pausePanel.SetActive(false); //pausePanel ��Ȱ��ȭ
        Time.timeScale = 1; //������ ����
    }

    public void GameStart() //���� ����(ó������)
    {
        audioPlayer.AudioPlay("Button");
        director.GetComponent<AudioSource>().Play();
        director.isPlay = true;
        director.isPause = false;
        FindObjectOfType<Swap>().SwapReset();
        ScoreReset(); //ScoreReset �Լ� ����
        director.ClearTetromino();

        director.pausePanel.SetActive(false);
        director.titlePanel.SetActive(false); //titlePanel ��Ȱ��ȭ
        director.gamePanel.SetActive(true); //gamePanel Ȱ��ȭ
        director.endPanel.SetActive(false); //endPanel ��Ȱ��ȭ

        Time.timeScale = 1; //������ ����

        generator.CreateTetromino(); //Generator ��ũ��Ʈ�� CreateTetromino �Լ� ����
    }

    public void GameTitle() //�غ� ȭ������ ����
    {
        audioPlayer.AudioPlay("Button");
        director.GetComponent<AudioSource>().Stop();
        director.isPlay = false;

        ScoreReset(); //ScoreReset �Լ� ����
        director.ClearTetromino();

        director.titlePanel.SetActive(true); //titlePanel Ȱ��ȭ
        director.gamePanel.SetActive(false); //gamePanel ��Ȱ��ȭ
        director.pausePanel.SetActive(false); //pausePanel ��Ȱ��ȭ
        director.endPanel.SetActive(false); //endPanel ��Ȱ��ȭ

        Time.timeScale = 1; //������ ����
    }

    public void GameQuit() //���� ����
    {
        Application.Quit(); //���� ����
    }

    public void ScoreReset() //���� �ʱ�ȭ
    {
        score.score = 0; //score�� 0����
        score.bestScore = PlayerPrefs.GetFloat(score.key, 0); //�ְ� ������ Ű���� ������ ����
        score.BestscoreText.text = "Best Score\n" + Mathf.Round(score.bestScore).ToString(); //�ְ� ���� Text�� ����
    }

}
