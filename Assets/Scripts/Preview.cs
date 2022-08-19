using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Preview : MonoBehaviour
{
    private Director director;
    Tetromino tetromino;
    public GameObject real;

    void Start()
    {
        director = FindObjectOfType<Director>();
        tetromino = FindObjectOfType<Tetromino>();

        while (MoveCheck(0, -1))
        {
            transform.position += new Vector3(0, -1, 0); 
        }

    }

    void Update()
    {
        if (Time.timeScale != 0) //프레임이 흐르고 있다면 실행
        {
            real = GameObject.FindWithTag("Tetromino");

            if (tetromino.left)
            {
                transform.position += new Vector3(-1, 0, 0);
                PreviewCheck(-1);

                while (MoveCheck(0, -1))
                {
                    transform.position += new Vector3(0, -1, 0);
                }
                tetromino.left = false;
            }

            if (tetromino.right)
            {
                transform.position += new Vector3(1, 0, 0);
                PreviewCheck(1);

                while (MoveCheck(0, -1))
                {
                    transform.position += new Vector3(0, -1, 0);
                }
                tetromino.right = false;
            }

            if (tetromino.spin)
            {
                transform.rotation *= Quaternion.Euler(0, 0, 90); //테트리스를 90도 회전한다.
                SpinCheck(); //SpinCheck 함수 실행

                while (MoveCheck(0, -1))
                {
                    transform.position += new Vector3(0, -1, 0);
                }
                tetromino.spin = false;
            }
        }
        ///////////////////////////////
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
                    transform.position += new Vector3(1, 0, 0); //오브젝트의 위치를 x 방향으로 1 움직인다.
                    SpinCheck(); //함수를 한번 더 실행 시켜 테트리스가 제대로 회전했는지 확인한다.
                }

                if (x >= director.gameW) //x가 게임판의 넓이보다 크다면 실행
                {
                    transform.position += new Vector3(-1, 0, 0); //오브젝트의 위치를 x 방향으로 -1 움직인다.
                    SpinCheck(); //함수를 한번 더 실행 시켜 테트리스가 제대로 회전했는지 확인한다.
                }

                if (y < 0 || y >= director.gameH) //y의 값이 0보다 작거나 게임판의 높이보다 크거나 같으면 실행
                {
                    transform.position += new Vector3(0, 1, 0);
                    SpinCheck();
                }

                x = Mathf.RoundToInt(children.transform.position.x); //위의 조건문에 의해 자식 오브젝트의 x 값이 바뀌었을 수도 있으니 값을 새로 저장한다.
                y = Mathf.RoundToInt(children.transform.position.y); //위의 조건문에 의해 자식 오브젝트의 y 값이 바뀌었을 수도 있으니 값을 새로 저장한다.

                if (director.sheet[x, y] != null) //sheet 배열의 [x, y]에 해당하는 값이 비어있지 않다면 실행
                {
                    transform.position += new Vector3(0, 1, 0);
                    SpinCheck();
                }
            }
        }
        else if (MoveCheck(0, 0))
        {
            foreach (Transform children in transform) //부모 오브젝트의 자식 오브젝트 수 만큼 실행
            {
                int x = Mathf.RoundToInt(children.transform.position.x); //자식 오브젝트의 x값을 반올림하여 저장
                int y = Mathf.RoundToInt(children.transform.position.y); //자식 오브젝트의 y값을 반올림하여 저장

                if (director.sheet[x, y] == null)
                {
                    if (real.transform.position.y > gameObject.transform.position.y)
                    {
                        for (int i = 1; i < director.gameH; i++)
                        {
                            if (y + i < director.gameH)
                            {
                                if (director.sheet[x, y + i] != null)
                                {
                                    transform.position += new Vector3(0, 1, 0);
                                    SpinCheck();
                                }
                            }
                        }
                    }
                }
            }
        }
        /////////////////////////////////////
    }

    void PreviewCheck(int a)
    {
        if (!MoveCheck(0, 0)) //테트리스가 움직일 수 있는 공간인지 확인하고, 움직일 수 있는 공간이 아니라면 실행한다.
        {
            foreach (Transform children in transform) //부모 오브젝트의 자식 오브젝트 수 만큼 실행
            {
                int x = Mathf.RoundToInt(children.transform.position.x); //자식 오브젝트의 x값을 반올림하여 저장
                int y = Mathf.RoundToInt(children.transform.position.y); //자식 오브젝트의 y값을 반올림하여 저장

                if (x < 0) //x가 0보다 작으면 실행
                {
                    transform.position += new Vector3(1, 0, 0); //오브젝트의 위치를 x 방향으로 1 움직인다.
                    PreviewCheck(a); //함수를 한번 더 실행 시켜 테트리스가 제대로 회전했는지 확인한다.
                }

                if (x >= director.gameW) //x가 게임판의 넓이보다 크다면 실행
                {
                    transform.position += new Vector3(-1, 0, 0); //오브젝트의 위치를 x 방향으로 -1 움직인다.
                    PreviewCheck(a); //함수를 한번 더 실행 시켜 테트리스가 제대로 회전했는지 확인한다.
                }

                x = Mathf.RoundToInt(children.transform.position.x); //위의 조건문에 의해 자식 오브젝트의 x 값이 바뀌었을 수도 있으니 값을 새로 저장한다.

                if (director.sheet[x, y] != null) //sheet 배열의 [x, y]에 해당하는 값이 비어있지 않다면 실행
                {
                    if (real.transform.position.y > gameObject.transform.position.y)
                    {
                        transform.position += new Vector3(0, 1, 0);
                        PreviewCheck(a);
                    }
                    else
                    {
                        transform.position += new Vector3(-a, 0, 0);
                    }
                }
            }
        }
        else if (MoveCheck(0, 0))
        {
            foreach (Transform children in transform) //부모 오브젝트의 자식 오브젝트 수 만큼 실행
            {
                int x = Mathf.RoundToInt(children.transform.position.x); //자식 오브젝트의 x값을 반올림하여 저장
                int y = Mathf.RoundToInt(children.transform.position.y); //자식 오브젝트의 y값을 반올림하여 저장

                if (director.sheet[x, y] == null)
                {
                    if (real.transform.position.y > gameObject.transform.position.y)
                    {
                        for (int i = 1; i < director.gameH; i++)
                        {
                            if (y + i < director.gameH)
                            {
                                if (director.sheet[x, y + i] != null)
                                {
                                    transform.position += new Vector3(0, 1, 0);
                                    PreviewCheck(a);
                                }
                            }
                        }
                    }
                    else if (real.transform.position.y <= gameObject.transform.position.y)
                    {
                        gameObject.transform.position = real.transform.position;
                    }
                }
            }
        }
        /////////////////////////////////////
    }

}
