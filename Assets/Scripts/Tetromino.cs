using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Tetromino : MonoBehaviour
{
    private float downTime; //������ �ð� ��
    public float downLimit = 1; //��Ʈ������ �ڵ����� �������� ���� �ʿ��� �ð� ��

    private Director director; //Director ��ũ��Ʈ�� ����ϱ� ���� ����
    private Generator generator; //Generator ��ũ��Ʈ�� ����ϱ� ���� ����
    private Sound audioPlayer;

    public bool left = false;
    public bool right = false;
    public bool spin = false;
    void Start()
    {
        director = FindObjectOfType<Director>(); //Director ��ũ��Ʈ�� ����ϱ� ���� ����
        generator = FindObjectOfType<Generator>(); //Generator ��ũ��Ʈ�� ����ϱ� ���� ����
        audioPlayer = FindObjectOfType<Sound>();
    }

    void Update()
    {
        downTime += Time.deltaTime; //�� ������ ���� ���Ѵ�.
        
        if (Time.timeScale != 0) //�������� �帣�� �ִٸ� ����
        {
            if (!MoveCheck(0, 0)) //��Ʈ������ ������ �� �ִ� ������ �ƴ϶�� ����
            { 
                transform.position += new Vector3(0, 1, 0); //��Ʈ������ y�� 1 �����δ�. 
                audioPlayer.AudioPlay("Turn");

                if (transform.position.y >= director.gameH) //��Ʈ������ y���� �������� ���̿� ���ų� ũ�ٸ� ����
                {
                    director.GameEnd(); //������ �����Ѵ�.
                }
            }

            if (downTime > downLimit) //downTime���� downLimit���� ũ�ٸ� ����
            {
                if (MoveCheck(0, -1)) //��Ʈ������ y�� -1 �̵����� �� ������ �� �ִٸ� ����
                {
                    transform.position += new Vector3(0, -1, 0); //��Ʈ������ y�� -1 �̵��Ѵ�.
                    audioPlayer.AudioPlay("Turn");
                }
                else //��Ʈ������ y�� -1 �̵����� �� ������ �� ���ٸ� ����
                {
                    SetTetromino(); //SetTetromino �Լ� ����
                    generator.CreateTetromino(); //Generator ��ũ��Ʈ�� CreateTetromino�Լ� ����
                }
                downTime = 0; //downTime ���� �ʱ�ȭ �Ѵ�.
            }

            if (Input.GetKeyDown(KeyCode.LeftArrow)) //���� ȭ��Ű�� ������ ����
            {
                if (MoveCheck(-1,0)) //��Ʈ������ x�� -1 �̵����� �� ������ �� �ִٸ� ����
                {
                    left = true;
                    transform.position += new Vector3(-1, 0, 0); //��Ʈ������ x�� -1 �����δ�. 
                    audioPlayer.AudioPlay("Turn");
                }
            }
            
            if (Input.GetKeyDown(KeyCode.RightArrow)) //������ ȭ��Ű�� ������ ����
            {
                if (MoveCheck(1, 0)) //��Ʈ������ x�� 1 �̵����� �� ������ �� �ִٸ� ����
                {
                    right = true;
                    transform.position += new Vector3(1, 0, 0); //��Ʈ������ x�� 1 �����δ�. 
                    audioPlayer.AudioPlay("Turn");
                }
            }

            if (Input.GetKeyDown(KeyCode.DownArrow)) //�Ʒ��� ȭ��Ű�� ������ ����
            {
                if (MoveCheck(0, -1)) //��Ʈ������ y�� -1 �̵����� �� ������ �� �ִٸ� ����
                {
                    transform.position += new Vector3(0, -1, 0); //��Ʈ������ y�� -1 �����δ�. 
                    audioPlayer.AudioPlay("Turn");
                }
            }

            if (Input.GetKeyDown(KeyCode.UpArrow)) //���� ȭ��Ű�� ������ ����
            {
                transform.rotation *= Quaternion.Euler(0, 0, 90); //��Ʈ������ 90�� ȸ���Ѵ�.
                SpinCheck(); //SpinCheck �Լ� ����
            }

            if (Input.GetKeyDown(KeyCode.Space)) //���� ȭ��Ű�� ������ ����
            {
                while (MoveCheck(0, -1)) //��Ʈ������ y�� -1 �̵����� �� ������ �� �ִٸ� �ݺ��Ѵ�.
                {
                    transform.position += new Vector3(0, -1, 0); //��Ʈ������ y�� -1 �����δ�. 
                }
                audioPlayer.AudioPlay("Turn");
                SetTetromino(); //SetTetromino �Լ� ����
                generator.CreateTetromino(); //Generator ��ũ��Ʈ�� CreateTetromino�Լ� ����

            }
        }
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
                    spin = true;
                    transform.position += new Vector3(1, 0, 0); //������Ʈ�� ��ġ�� x �������� 1 �����δ�.
                    SpinCheck(); //�Լ��� �ѹ� �� ���� ���� ��Ʈ������ ����� ȸ���ߴ��� Ȯ���Ѵ�.
                }

                if (x >= director.gameW) //x�� �������� ���̺��� ũ�ٸ� ����
                {
                    spin = true;
                    transform.position += new Vector3(-1, 0, 0); //������Ʈ�� ��ġ�� x �������� -1 �����δ�.
                    SpinCheck(); //�Լ��� �ѹ� �� ���� ���� ��Ʈ������ ����� ȸ���ߴ��� Ȯ���Ѵ�.
                }

                if (y < 0 || y >= director.gameH) //y�� ���� 0���� �۰ų� �������� ���̺��� ũ�ų� ������ ����
                {
                    transform.rotation *= Quaternion.Euler(0, 0, -90); //������Ʈ�� -90�� �ٽ� ȸ������ ȸ���� ���� ������ ó�� ���̰� �Ѵ�.
                }

                x = Mathf.RoundToInt(children.transform.position.x); //���� ���ǹ��� ���� �ڽ� ������Ʈ�� x ���� �ٲ���� ���� ������ ���� ���� �����Ѵ�.
                y = Mathf.RoundToInt(children.transform.position.y); //���� ���ǹ��� ���� �ڽ� ������Ʈ�� y ���� �ٲ���� ���� ������ ���� ���� �����Ѵ�.

                if (director.sheet[x, y] != null) //sheet �迭�� [x, y]�� �ش��ϴ� ���� ������� �ʴٸ� ����
                {
                    transform.rotation *= Quaternion.Euler(0, 0, -90); //������Ʈ�� -90�� �ٽ� ȸ������ ȸ���� ���� ������ ó�� ���̰� �Ѵ�.
                }
            }
        }
        else
        {
            spin = true;
            audioPlayer.AudioPlay("Turn");
        }
    }

    void SetTetromino() //��Ʈ������ �迭�� �����ϴ� �Լ�
    {
        foreach (Transform children in transform) //�ڽ� ������Ʈ �� ��ŭ �ݺ�
        {
            int x = Mathf.RoundToInt(children.transform.position.x); //�ڽ� ������Ʈ�� x���� �ݿø��Ͽ� ����
            int y = Mathf.RoundToInt(children.transform.position.y); //�ڽ� ������Ʈ�� y���� �ݿø��Ͽ� ����

            director.sheet[x, y] = children; //sheet �迭 [x, y]�� �ش��ϴ� ���� �ڽ� ������Ʈ�� ����
        }
        gameObject.tag = "Set";

        GameObject preview = GameObject.FindWithTag("Preview");
        Destroy(preview);

        enabled = false; //������Ʈ�� ������ �� ���� �Ѵ�.
    }
}
