using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Score : MonoBehaviour
{
    public Text ScoreText; //현재 점수가 표시되는 Text
    public Text BestscoreText; //최고 점수가 표시되는 Text
    public Text ComboText;

    [HideInInspector] //public으로 선언된 변수를 inspector 창에서 감추기 위해서 사용
    public float score; //현재 점수
    [HideInInspector]
    public float bestScore; //최고 점수
    [HideInInspector]
    public string key = "BestScore"; //최고 점수 저장을 위한 키워드

    //점수 콤보 관련 변수들
    int comboCount = 0;
    bool IsCombo = false;
    bool IsScore = false;
    float comboLimit = 10;
    float comboTime = 0;

    private Sound audioPlayer;

    void Start()
    {
        audioPlayer = FindObjectOfType<Sound>();
        bestScore = PlayerPrefs.GetFloat(key, 0); //최고 점수를 키워드 값으로 변경한다. 없을경우 0으로 설정한다.
        BestscoreText.text = "Best Score\n" + Mathf.Round(bestScore).ToString(); //최고 점수가 표시되는 Text를 변경한다.
    }

    void Update()
    {
        ScoreText.text = "Score\n" + Mathf.Round(score).ToString(); //현재 점수 text를 변경한다.

        if (score >= bestScore) //현재 점수가 최고 점수와 같거나 크면 실행한다.
        {
            bestScore = score; //최고 점수를 현재 점수의 값으로 변경한다.
            BestscoreText.text = "Best Score\n" + Mathf.Round(bestScore).ToString(); //최고 점수가 표시되는 Text를 변경한다.
        }

        if (IsCombo) //IsCombo가 true면 실행
        {
            comboTime += Time.deltaTime; //comboTime에 매 프레임 시간을 더한다.

            if (comboTime < comboLimit) //comboTime이 comboLimit보다 작으면 실행
            {
                ComboText.text = comboCount + "\nCOMBO"; //ComboText의 text 변경
                if (IsScore) //IsScore가 true면 실행
                {
                    AddScore(comboCount); //AddScore 함수를 comboCount 값을 넣어서 실행
                    IsScore = false; //IsScore를 false로 변경
                }
            }

            if (comboTime > comboLimit) //comboTime이 comboLimit보다 크면 실행
            {
                IsCombo = false;  //IsCombo를 false로 변경
            }
        }
        else //IsCombo가 false면 실행
        {
            //comboCount, comboTime 0으로 초기화
            comboCount = 0;
            comboTime = 0;
            ComboText.text = comboCount + "\nCOMBO"; //ComboText의 text 변경
        }
    }

    public void AddCombo() //콤보 증가 함수
    {
        comboCount += 1; //comboCount 값을 1 추가
        IsCombo = true;  //IsCombo를 true로 변경
        IsScore = true;  //IsScore를 true로 변경
    }

    public void AddScore(int comboCount) //점수 증가 함수
    {
        comboTime = 0; //comboTime 값을 0으로 변경
        int stdScore = 100; //기본점수
        float comboBonus = comboCount * 10; //콤보 보너스점수

        if (comboCount == 1) //함수가 받은 값이 1이면 실행
        {
            score += stdScore; //score에 기본점수 값을 더한다.
        }
        else //함수가 받은 값이 1이 아니면 실행
        {
            score += stdScore + comboBonus; //score에 기본점수+콤보 보너스점수 값을 더한다. 
        }
        audioPlayer.AudioPlay("Clear");
    }
}
