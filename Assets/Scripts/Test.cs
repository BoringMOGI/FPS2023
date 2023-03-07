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
            throw new Exception("어떠한 예외가 일어났다.");
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
        // try에서 에러가 일어나지 않았거나
        // 예외가 일어나서 catch문을 들어갔거나
        // 마지막에 들어오는 곳이다.
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
