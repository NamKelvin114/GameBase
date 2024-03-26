using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Test : MonoBehaviour
{
    struct MyStruct
    {
        public int ncodon;
        public int dodo;
    }
    public delegate void SetDelea(string name);

    int[] daily = { 1, 2, 3, 4 };
    List<int> coding = new();
    private int m = 10;
    public event SetDelea DealEvent;
    private Test1 _test1;
    private string context;
    private int[,] aaa =
    {
        { 1, 2, 3, 4, 5, 5, 5 }
    };
    private void Start()
    {
        context = "lehah";
        String(context);
        Debug.Log(context);
        // int tt = 90;
        // if (tt>100)
        // {
        //     Debug.Log("morethan 100");
        // }
        // else if (tt<=91)
        // {
        //     Debug.Log("behon");
        // }
        // else if (tt<=95)
        // {
        //     Debug.Log("hbehonexa");
        // }
        // if (tt<=95)
        // {
        //     Debug.Log("hbehonexa");
        // }
        // Debug.Log(aaa[0,0]);
        // int? i = null;
        // Observer.TestPredicate += ATest;
        // DealEvent +=num;
        // Run( element => {
        //     coding.Add(element);
        //     if (element == 2) return;
        // });
        // (string message, string asda) testTupes = new("Namhake", "Lehanam");
        // var a = testTupe(6, "Nam");
        // Debug.Log(testTupes);
        // Debug.Log(a.Item1);

    }
    void String(string h)
    {
        h = "dsdas";
    }
    public Tuple<int, string> testTupe(int y, string s)
    {

        return new Tuple<int, string>(y, s);
    }
    public void num(string a)
    {

    }
    void DoRun(int element)
    {
        coding.Add(element);
        if (element == 2) return;
    }
    bool ATest(string alo)
    {
        if (alo == null)
        {
            return true;
        }
        return false;
    }
    private void Run(Action<int> action)
    {
        foreach (int i in daily)
        {
            action(i);
        }
        DealEvent?.Invoke("a");
    }

}
