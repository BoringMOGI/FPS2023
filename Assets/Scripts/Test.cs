using System;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using UnityEngine;

public class Test : MonoBehaviour
{
    class Temp
    {
        public void Func()
        {
            throw new Exception("��� ���ܰ� �Ͼ��.");
        }
    }

    void Start()
    {
        int[] array = null;
        Temp temp = new Temp();

        StreamReader sr = null;

        try
        {
            //sr = new StreamReader("C/Text.txt");
            //string text = sr.ReadToEnd();
            //Debug.Log(text);
            //sr.Close();
        }
        catch (Exception e)
        {
            Debug.Log(e.Message);
        }
        // try���� ������ �Ͼ�� �ʾҰų�
        // ���ܰ� �Ͼ�� catch���� ���ų�
        // �������� ������ ���̴�.
        finally
        {
            Debug.Log("Finally");
        }

    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
