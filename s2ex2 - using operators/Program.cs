using System;
using System.Collections.Generic;

namespace s2ex2___using_operators
{
    class Program
    {
        static void Main(string[] args)
        {
            var a = new SpecialList<Int32>(new List<Int32>{1,2,3});
            var b = new SpecialList<Int32>(new List<Int32>{3,7,5});
            Console.WriteLine(a > b);
            Console.WriteLine(a < b);
        }
    }
}

public class SpecialList<T>{

    private List<T> innerList;

    public SpecialList(List<T> innerList){
        this.innerList = innerList;
    }

    public static bool operator > (SpecialList<T> a, SpecialList<T> b){
        return a.SumElements(a.innerList) > b.SumElements(b.innerList) ? true : false;
    }

    public static bool operator < (SpecialList<T> a, SpecialList<T> b){
        return a.SumElements(a.innerList) > b.SumElements(b.innerList) ? false : true;
    }

    private double SumElements(List<T> list){
        double sum = 0;
        foreach(var x in list){
            sum += System.Convert.ToDouble(x);  
        }
        return sum;
    }
}