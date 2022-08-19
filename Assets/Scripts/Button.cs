using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Button : MonoBehaviour
{
    //다른 스크립트의 변수 및 함수를 사용하기 위해 선언한 변수 선언
    private Score score;
    private Director director;
    private Generator generator;
    private Sound audioPlayer;
    
    private void Start()
    {
        //다른 스크립트의 변수 및 함수를 사용하기 위해 선언한 변수 초기화
        score = FindObjectOfType<Score>();
        director = FindObjectOfType<Director>();
        generator = FindObjectOfType<Generator>();
        audioPlayer = FindObjectOfType<Sound>();
    }

    public void GamePause() //게임 일시 정지 
    {
        audioPlayer.AudioPlay("Button");
        director.GetComponent<AudioSource>().Pause();
        director.isPause = true;

        Time.timeScale = 0; //프레임 멈춤
        director.pausePanel.SetActive(true); //pausePanel 활성화
    }

    public void GameResume() //게임 이어서 실행
    {
        audioPlayer.AudioPlay("Button");
        director.GetComponent<AudioSource>().Play();
        director.isPause = false;

        director.pausePanel.SetActive(false); //pausePanel 비활성화
        Time.timeScale = 1; //프레임 시작
    }

    public void GameStart() //게임 실행(처음부터)
    {
        audioPlayer.AudioPlay("Button");
        director.GetComponent<AudioSource>().Play();
        director.isPlay = true;
        director.isPause = false;
        FindObjectOfType<Swap>().SwapReset();
        ScoreReset(); //ScoreReset 함수 실행
        director.ClearTetromino();

        director.pausePanel.SetActive(false);
        director.titlePanel.SetActive(false); //titlePanel 비활성화
        director.gamePanel.SetActive(true); //gamePanel 활성화
        director.endPanel.SetActive(false); //endPanel 비활성화

        Time.timeScale = 1; //프레임 시작

        generator.CreateTetromino(); //Generator 스크립트의 CreateTetromino 함수 실행
    }

    public void GameTitle() //준비 화면으로 가기
    {
        audioPlayer.AudioPlay("Button");
        director.GetComponent<AudioSource>().Stop();
        director.isPlay = false;

        ScoreReset(); //ScoreReset 함수 실행
        director.ClearTetromino();

        director.titlePanel.SetActive(true); //titlePanel 활성화
        director.gamePanel.SetActive(false); //gamePanel 비활성화
        director.pausePanel.SetActive(false); //pausePanel 비활성화
        director.endPanel.SetActive(false); //endPanel 비활성화

        Time.timeScale = 1; //프레임 시작
    }

    public void GameQuit() //게임 종료
    {
        Application.Quit(); //게임 종료
    }

    public void ScoreReset() //점수 초기화
    {
        score.score = 0; //score를 0으로
        score.bestScore = PlayerPrefs.GetFloat(score.key, 0); //최고 점수를 키워드 값으로 변경
        score.BestscoreText.text = "Best Score\n" + Mathf.Round(score.bestScore).ToString(); //최고 점수 Text를 변경
    }

}
