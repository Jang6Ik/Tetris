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
        if (Time.timeScale != 0) //�������� �帣�� �ִٸ� ����
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
                transform.rotation *= Quaternion.Euler(0, 0, 90); //��Ʈ������ 90�� ȸ���Ѵ�.
                SpinCheck(); //SpinCheck �Լ� ����

                while (MoveCheck(0, -1))
                {
                    transform.position += new Vector3(0, -1, 0);
                }
                tetromino.spin = false;
            }
        }
        ///////////////////////////////
    }

    bool MoveCheck(int x2, int y2) //��Ʈ������ ������ �� �ִ� �������� Ȯ���ϱ� ���� �Լ�
    {
        foreach (Transform children in transform) //�θ� ������Ʈ�� �ڽ� ������Ʈ �� ��ŭ �ݺ��Ѵ�.
        {
            int x = Mathf.RoundToInt(children.transform.position.x + x2); //�ڽ� ������Ʈ�� x+x2 ���� �ݿø� �Ͽ� �����Ѵ�.
            int y = Mathf.RoundToInt(children.transform.position.y + y2); //�ڽ� ������Ʈ�� y+y2 ���� �ݿø� �Ͽ� �����Ѵ�.

            //������ ����� x�� y�� ���� ���ǿ� �ϳ��� �ش��ϸ� �����Ѵ�.
            if (x < 0 || x >= director.gameW || y < 0 || y >= director.gameH || director.sheet[x, y] != null)
            {
                return false; //false�� ��ȯ�Ѵ�.
            }
        }
        return true; //true�� ��ȯ�Ѵ�.
    }

    void SpinCheck() //��Ʈ������ ����� ȸ���ߴ��� Ȯ���ϴ� �Լ�
    {
        if (!MoveCheck(0, 0)) //��Ʈ������ ������ �� �ִ� �������� Ȯ���ϰ�, ������ �� �ִ� ������ �ƴ϶�� �����Ѵ�.
        {
            foreach (Transform children in transform) //�θ� ������Ʈ�� �ڽ� ������Ʈ �� ��ŭ ����
            {
                int x = Mathf.RoundToInt(children.transform.position.x); //�ڽ� ������Ʈ�� x���� �ݿø��Ͽ� ����
                int y = Mathf.RoundToInt(children.transform.position.y); //�ڽ� ������Ʈ�� y���� �ݿø��Ͽ� ����

                if (x < 0) //x�� 0���� ������ ����
                {
                    transform.position += new Vector3(1, 0, 0); //������Ʈ�� ��ġ�� x �������� 1 �����δ�.
                    SpinCheck(); //�Լ��� �ѹ� �� ���� ���� ��Ʈ������ ����� ȸ���ߴ��� Ȯ���Ѵ�.
                }

                if (x >= director.gameW) //x�� �������� ���̺��� ũ�ٸ� ����
                {
                    transform.position += new Vector3(-1, 0, 0); //������Ʈ�� ��ġ�� x �������� -1 �����δ�.
                    SpinCheck(); //�Լ��� �ѹ� �� ���� ���� ��Ʈ������ ����� ȸ���ߴ��� Ȯ���Ѵ�.
                }

                if (y < 0 || y >= director.gameH) //y�� ���� 0���� �۰ų� �������� ���̺��� ũ�ų� ������ ����
                {
                    transform.position += new Vector3(0, 1, 0);
                    SpinCheck();
                }

                x = Mathf.RoundToInt(children.transform.position.x); //���� ���ǹ��� ���� �ڽ� ������Ʈ�� x ���� �ٲ���� ���� ������ ���� ���� �����Ѵ�.
                y = Mathf.RoundToInt(children.transform.position.y); //���� ���ǹ��� ���� �ڽ� ������Ʈ�� y ���� �ٲ���� ���� ������ ���� ���� �����Ѵ�.

                if (director.sheet[x, y] != null) //sheet �迭�� [x, y]�� �ش��ϴ� ���� ������� �ʴٸ� ����
                {
                    transform.position += new Vector3(0, 1, 0);
                    SpinCheck();
                }
            }
        }
        else if (MoveCheck(0, 0))
        {
            foreach (Transform children in transform) //�θ� ������Ʈ�� �ڽ� ������Ʈ �� ��ŭ ����
            {
                int x = Mathf.RoundToInt(children.transform.position.x); //�ڽ� ������Ʈ�� x���� �ݿø��Ͽ� ����
                int y = Mathf.RoundToInt(children.transform.position.y); //�ڽ� ������Ʈ�� y���� �ݿø��Ͽ� ����

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
        if (!MoveCheck(0, 0)) //��Ʈ������ ������ �� �ִ� �������� Ȯ���ϰ�, ������ �� �ִ� ������ �ƴ϶�� �����Ѵ�.
        {
            foreach (Transform children in transform) //�θ� ������Ʈ�� �ڽ� ������Ʈ �� ��ŭ ����
            {
                int x = Mathf.RoundToInt(children.transform.position.x); //�ڽ� ������Ʈ�� x���� �ݿø��Ͽ� ����
                int y = Mathf.RoundToInt(children.transform.position.y); //�ڽ� ������Ʈ�� y���� �ݿø��Ͽ� ����

                if (x < 0) //x�� 0���� ������ ����
                {
                    transform.position += new Vector3(1, 0, 0); //������Ʈ�� ��ġ�� x �������� 1 �����δ�.
                    PreviewCheck(a); //�Լ��� �ѹ� �� ���� ���� ��Ʈ������ ����� ȸ���ߴ��� Ȯ���Ѵ�.
                }

                if (x >= director.gameW) //x�� �������� ���̺��� ũ�ٸ� ����
                {
                    transform.position += new Vector3(-1, 0, 0); //������Ʈ�� ��ġ�� x �������� -1 �����δ�.
                    PreviewCheck(a); //�Լ��� �ѹ� �� ���� ���� ��Ʈ������ ����� ȸ���ߴ��� Ȯ���Ѵ�.
                }

                x = Mathf.RoundToInt(children.transform.position.x); //���� ���ǹ��� ���� �ڽ� ������Ʈ�� x ���� �ٲ���� ���� ������ ���� ���� �����Ѵ�.

                if (director.sheet[x, y] != null) //sheet �迭�� [x, y]�� �ش��ϴ� ���� ������� �ʴٸ� ����
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
            foreach (Transform children in transform) //�θ� ������Ʈ�� �ڽ� ������Ʈ �� ��ŭ ����
            {
                int x = Mathf.RoundToInt(children.transform.position.x); //�ڽ� ������Ʈ�� x���� �ݿø��Ͽ� ����
                int y = Mathf.RoundToInt(children.transform.position.y); //�ڽ� ������Ʈ�� y���� �ݿø��Ͽ� ����

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
