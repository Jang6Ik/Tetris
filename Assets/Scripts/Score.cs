using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Score : MonoBehaviour
{
    public Text ScoreText; //���� ������ ǥ�õǴ� Text
    public Text BestscoreText; //�ְ� ������ ǥ�õǴ� Text
    public Text ComboText;

    [HideInInspector] //public���� ����� ������ inspector â���� ���߱� ���ؼ� ���
    public float score; //���� ����
    [HideInInspector]
    public float bestScore; //�ְ� ����
    [HideInInspector]
    public string key = "BestScore"; //�ְ� ���� ������ ���� Ű����

    //���� �޺� ���� ������
    int comboCount = 0;
    bool IsCombo = false;
    bool IsScore = false;
    float comboLimit = 10;
    float comboTime = 0;

    private Sound audioPlayer;

    void Start()
    {
        audioPlayer = FindObjectOfType<Sound>();
        bestScore = PlayerPrefs.GetFloat(key, 0); //�ְ� ������ Ű���� ������ �����Ѵ�. ������� 0���� �����Ѵ�.
        BestscoreText.text = "Best Score\n" + Mathf.Round(bestScore).ToString(); //�ְ� ������ ǥ�õǴ� Text�� �����Ѵ�.
    }

    void Update()
    {
        ScoreText.text = "Score\n" + Mathf.Round(score).ToString(); //���� ���� text�� �����Ѵ�.

        if (score >= bestScore) //���� ������ �ְ� ������ ���ų� ũ�� �����Ѵ�.
        {
            bestScore = score; //�ְ� ������ ���� ������ ������ �����Ѵ�.
            BestscoreText.text = "Best Score\n" + Mathf.Round(bestScore).ToString(); //�ְ� ������ ǥ�õǴ� Text�� �����Ѵ�.
        }

        if (IsCombo) //IsCombo�� true�� ����
        {
            comboTime += Time.deltaTime; //comboTime�� �� ������ �ð��� ���Ѵ�.

            if (comboTime < comboLimit) //comboTime�� comboLimit���� ������ ����
            {
                ComboText.text = comboCount + "\nCOMBO"; //ComboText�� text ����
                if (IsScore) //IsScore�� true�� ����
                {
                    AddScore(comboCount); //AddScore �Լ��� comboCount ���� �־ ����
                    IsScore = false; //IsScore�� false�� ����
                }
            }

            if (comboTime > comboLimit) //comboTime�� comboLimit���� ũ�� ����
            {
                IsCombo = false;  //IsCombo�� false�� ����
            }
        }
        else //IsCombo�� false�� ����
        {
            //comboCount, comboTime 0���� �ʱ�ȭ
            comboCount = 0;
            comboTime = 0;
            ComboText.text = comboCount + "\nCOMBO"; //ComboText�� text ����
        }
    }

    public void AddCombo() //�޺� ���� �Լ�
    {
        comboCount += 1; //comboCount ���� 1 �߰�
        IsCombo = true;  //IsCombo�� true�� ����
        IsScore = true;  //IsScore�� true�� ����
    }

    public void AddScore(int comboCount) //���� ���� �Լ�
    {
        comboTime = 0; //comboTime ���� 0���� ����
        int stdScore = 100; //�⺻����
        float comboBonus = comboCount * 10; //�޺� ���ʽ�����

        if (comboCount == 1) //�Լ��� ���� ���� 1�̸� ����
        {
            score += stdScore; //score�� �⺻���� ���� ���Ѵ�.
        }
        else //�Լ��� ���� ���� 1�� �ƴϸ� ����
        {
            score += stdScore + comboBonus; //score�� �⺻����+�޺� ���ʽ����� ���� ���Ѵ�. 
        }
        audioPlayer.AudioPlay("Clear");
    }
}
