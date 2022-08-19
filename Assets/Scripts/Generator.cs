using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Generator : MonoBehaviour
{
    public GameObject[] Tetrominos; //테트리스의 프리팹을 저장하기 위한 배열
    public GameObject[] Previews;
    
    public List<int> box = new List<int>();
    public List<int> number = new List<int>();
        
    public Transform next1;
    public Transform next2;
    public Transform next3;

    void Start()
    {
        BoxReset();
    }

    private void Update()
    { 
        if (number.Count == 3)
        {
            for (int i = 0; i < 7; i++)
            {
                box.Add(i);
            }
            NumberArrangement();
        }

    }

    public void CreateTetromino()
    {
        ResetNext();
        NextShow();
        //배열에 포함된 테트리스 프리팹 중에서 랜덤으로 하나를 스크립트를 가진 오브젝트의 위치에 생성한다. 
        Instantiate(Tetrominos[number[0]], transform.position,
            Quaternion.identity);
        Instantiate(Previews[number[0]], transform.position,
            Quaternion.identity);
        FindObjectOfType<Swap>().isSwap = false;
        number.RemoveAt(0);
        
    }

    public void SwapTetro(int a)
    {
        //배열에 포함된 테트리스 프리팹 중에서 랜덤으로 하나를 스크립트를 가진 오브젝트의 위치에 생성한다. 
        Instantiate(Tetrominos[a], transform.position,
            Quaternion.identity);
        Instantiate(Previews[a], transform.position,
            Quaternion.identity);
        FindObjectOfType<Swap>().isSwap = false;
    }

    public void BoxReset()
    {
        number.Clear();
        box.Clear();

        for (int i = 0; i < 7; i++)
        {
            box.Add(i);
        }
        NumberArrangement();
    }

    public void NumberArrangement()
    {
        while (box.Count != 0)
        {
            int rands = Random.Range(0, box.Count);
            number.Add(box[rands]);
            box.RemoveAt(rands);
        }
    }

    public void NextShow()
    {
        GameObject nextTetro1 = Instantiate(Tetrominos[number[1]], next1.transform.position,
            Quaternion.identity);
        nextTetro1.tag = "Next";
        nextTetro1.transform.parent = next1.transform;
        nextTetro1.transform.localScale = new Vector3(0.2f, 0.2f, 0.2f);
        NextPosition(nextTetro1);
        nextTetro1.GetComponent<Tetromino>().enabled = false;

        GameObject nextTetro2 = Instantiate(Tetrominos[number[2]], next2.transform.position,
            Quaternion.identity);
        nextTetro2.tag = "Next";
        nextTetro2.transform.parent = next2.transform;
        nextTetro2.transform.localScale = new Vector3(0.2f, 0.2f, 0.2f);
        NextPosition(nextTetro2);
        nextTetro2.GetComponent<Tetromino>().enabled = false;

        GameObject nextTetro3 = Instantiate(Tetrominos[number[3]], next3.transform.position,
            Quaternion.identity);
        nextTetro3.tag = "Next";
        nextTetro3.transform.parent = next3.transform;
        NextPosition(nextTetro3);
        nextTetro3.transform.localScale = new Vector3(0.2f, 0.2f, 0.2f);
        nextTetro3.GetComponent<Tetromino>().enabled = false;
    }

    public void NextPosition(GameObject next)
    {
        switch (next.name)
        {
            case  "Shape1(Clone)" :
                next.transform.position += new Vector3(-0.4f, 0, 0);
                break;
            case "Shape2(Clone)":
                next.transform.position += new Vector3(0, 0.5f, 0);
                break;
            case "Shape3(Clone)":
                next.transform.position += new Vector3(-0.5f, -0.5f, 0);
                break;
            case "Shape4(Clone)":
                next.transform.position += new Vector3(0, -0.5f, 0);
                break;
            case "Shape5(Clone)":
                next.transform.position += new Vector3(0, -0.5f, 0);
                break;
            case "Shape6(Clone)":
                next.transform.position += new Vector3(0, -0.5f, 0);
                break;
            case "Shape7(Clone)":
                next.transform.position += new Vector3(-0.4f, 0.4f, 0);
                break;
        }
    }

    public void ResetNext()
    {
        GameObject[] nextTetros = GameObject.FindGameObjectsWithTag("Next");
        for (int i = 0; i < nextTetros.Length; i++)
        {
            Destroy(nextTetros[i]);
        }
    }
}
