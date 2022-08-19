using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Tetromino : MonoBehaviour
{
    private float downTime; //지나간 시간 값
    public float downLimit = 1; //테트리스가 자동으로 떨어지기 위해 필요한 시간 값

    private Director director; //Director 스크립트를 사용하기 위한 변수
    private Generator generator; //Generator 스크립트를 사용하기 위한 변수
    private Sound audioPlayer;

    public bool left = false;
    public bool right = false;
    public bool spin = false;
    void Start()
    {
        director = FindObjectOfType<Director>(); //Director 스크립트를 사용하기 위한 변수
        generator = FindObjectOfType<Generator>(); //Generator 스크립트를 사용하기 위한 변수
        audioPlayer = FindObjectOfType<Sound>();
    }

    void Update()
    {
        downTime += Time.deltaTime; //매 프레임 값을 더한다.
        
        if (Time.timeScale != 0) //프레임이 흐르고 있다면 실행
        {
            if (!MoveCheck(0, 0)) //테트리스가 움직일 수 있는 공간이 아니라면 실행
            { 
                transform.position += new Vector3(0, 1, 0); //테트리스를 y로 1 움직인다. 
                audioPlayer.AudioPlay("Turn");

                if (transform.position.y >= director.gameH) //테트리스의 y값이 게임판의 높이와 같거나 크다면 실행
                {
                    director.GameEnd(); //게임을 종료한다.
                }
            }

            if (downTime > downLimit) //downTime값이 downLimit보다 크다면 실행
            {
                if (MoveCheck(0, -1)) //테트리스가 y로 -1 이동했을 때 움직일 수 있다면 실행
                {
                    transform.position += new Vector3(0, -1, 0); //테트리스를 y로 -1 이동한다.
                    audioPlayer.AudioPlay("Turn");
                }
                else //테트리스가 y로 -1 이동했을 때 움직일 수 없다면 실행
                {
                    SetTetromino(); //SetTetromino 함수 실행
                    generator.CreateTetromino(); //Generator 스크립트의 CreateTetromino함수 실행
                }
                downTime = 0; //downTime 값을 초기화 한다.
            }

            if (Input.GetKeyDown(KeyCode.LeftArrow)) //왼쪽 화살키가 눌리면 실행
            {
                if (MoveCheck(-1,0)) //테트리스가 x로 -1 이동했을 때 움직일 수 있다면 실행
                {
                    left = true;
                    transform.position += new Vector3(-1, 0, 0); //테트리스를 x로 -1 움직인다. 
                    audioPlayer.AudioPlay("Turn");
                }
            }
            
            if (Input.GetKeyDown(KeyCode.RightArrow)) //오른쪽 화살키가 눌리면 실행
            {
                if (MoveCheck(1, 0)) //테트리스가 x로 1 이동했을 때 움직일 수 있다면 실행
                {
                    right = true;
                    transform.position += new Vector3(1, 0, 0); //테트리스를 x로 1 움직인다. 
                    audioPlayer.AudioPlay("Turn");
                }
            }

            if (Input.GetKeyDown(KeyCode.DownArrow)) //아래쪽 화살키가 눌리면 실행
            {
                if (MoveCheck(0, -1)) //테트리스가 y로 -1 이동했을 때 움직일 수 있다면 실행
                {
                    transform.position += new Vector3(0, -1, 0); //테트리스를 y로 -1 움직인다. 
                    audioPlayer.AudioPlay("Turn");
                }
            }

            if (Input.GetKeyDown(KeyCode.UpArrow)) //위쪽 화살키가 눌리면 실행
            {
                transform.rotation *= Quaternion.Euler(0, 0, 90); //테트리스를 90도 회전한다.
                SpinCheck(); //SpinCheck 함수 실행
            }

            if (Input.GetKeyDown(KeyCode.Space)) //왼쪽 화살키가 눌리면 실행
            {
                while (MoveCheck(0, -1)) //테트리스가 y로 -1 이동했을 때 움직일 수 있다면 반복한다.
                {
                    transform.position += new Vector3(0, -1, 0); //테트리스를 y로 -1 움직인다. 
                }
                audioPlayer.AudioPlay("Turn");
                SetTetromino(); //SetTetromino 함수 실행
                generator.CreateTetromino(); //Generator 스크립트의 CreateTetromino함수 실행

            }
        }
    }

    bool MoveCheck(int x2, int y2) //테트리스가 움직일 수 있는 공간인지 확인하기 위한 함수
    {
        foreach (Transform children in transform) //부모 오브젝트의 자식 오브젝트 수 만큼 반복한다.
        {
            int x = Mathf.RoundToInt(children.transform.position.x + x2); //자식 오브젝트의 x+x2 값을 반올림 하여 저장한다.
            int y = Mathf.RoundToInt(children.transform.position.y + y2); //자식 오브젝트의 y+y2 값을 반올림 하여 저장한다.

            //위에서 저장된 x와 y의 값이 조건에 하나라도 해당하면 실행한다.
            if (x < 0 || x >= director.gameW || y < 0 || y >= director.gameH || director.sheet[x, y] != null)
            {
                return false; //false를 반환한다.
            }
        }
        return true; //true를 반환한다.
    }

    void SpinCheck() //테트리스가 제대로 회전했는지 확인하는 함수
    {
        if (!MoveCheck(0, 0)) //테트리스가 움직일 수 있는 공간인지 확인하고, 움직일 수 있는 공간이 아니라면 실행한다.
        {
            foreach (Transform children in transform) //부모 오브젝트의 자식 오브젝트 수 만큼 실행
            {
                int x = Mathf.RoundToInt(children.transform.position.x); //자식 오브젝트의 x값을 반올림하여 저장
                int y = Mathf.RoundToInt(children.transform.position.y); //자식 오브젝트의 y값을 반올림하여 저장

                if (x < 0) //x가 0보다 작으면 실행
                {
                    spin = true;
                    transform.position += new Vector3(1, 0, 0); //오브젝트의 위치를 x 방향으로 1 움직인다.
                    SpinCheck(); //함수를 한번 더 실행 시켜 테트리스가 제대로 회전했는지 확인한다.
                }

                if (x >= director.gameW) //x가 게임판의 넓이보다 크다면 실행
                {
                    spin = true;
                    transform.position += new Vector3(-1, 0, 0); //오브젝트의 위치를 x 방향으로 -1 움직인다.
                    SpinCheck(); //함수를 한번 더 실행 시켜 테트리스가 제대로 회전했는지 확인한다.
                }

                if (y < 0 || y >= director.gameH) //y의 값이 0보다 작거나 게임판의 높이보다 크거나 같으면 실행
                {
                    transform.rotation *= Quaternion.Euler(0, 0, -90); //오브젝트를 -90도 다시 회전시켜 회전을 하지 않은것 처럼 보이게 한다.
                }

                x = Mathf.RoundToInt(children.transform.position.x); //위의 조건문에 의해 자식 오브젝트의 x 값이 바뀌었을 수도 있으니 값을 새로 저장한다.
                y = Mathf.RoundToInt(children.transform.position.y); //위의 조건문에 의해 자식 오브젝트의 y 값이 바뀌었을 수도 있으니 값을 새로 저장한다.

                if (director.sheet[x, y] != null) //sheet 배열의 [x, y]에 해당하는 값이 비어있지 않다면 실행
                {
                    transform.rotation *= Quaternion.Euler(0, 0, -90); //오브젝트를 -90도 다시 회전시켜 회전을 하지 않은것 처럼 보이게 한다.
                }
            }
        }
        else
        {
            spin = true;
            audioPlayer.AudioPlay("Turn");
        }
    }

    void SetTetromino() //테트리스를 배열에 저장하는 함수
    {
        foreach (Transform children in transform) //자식 오브젝트 수 만큼 반복
        {
            int x = Mathf.RoundToInt(children.transform.position.x); //자식 오브젝트의 x값을 반올림하여 저장
            int y = Mathf.RoundToInt(children.transform.position.y); //자식 오브젝트의 y값을 반올림하여 저장

            director.sheet[x, y] = children; //sheet 배열 [x, y]에 해당하는 곳에 자식 오브젝트를 저장
        }
        gameObject.tag = "Set";

        GameObject preview = GameObject.FindWithTag("Preview");
        Destroy(preview);

        enabled = false; //오브젝트를 조작할 수 없게 한다.
    }
}
