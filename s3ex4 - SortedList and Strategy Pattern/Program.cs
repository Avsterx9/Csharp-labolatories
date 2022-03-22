using System;
using System.Collections.Generic;
using System.Linq;

namespace s3ex4___SortedList_and_Strategy_Pattern
{
    class Program
    {
        static void Main(string[] args)
        {
            SortingClass sortingClass = new SortingClass();
            
            sortingClass.AddElement(2);
            sortingClass.AddElement(8);
            sortingClass.AddElement(12);
            sortingClass.AddElement(1);
            sortingClass.AddElement(28);
            sortingClass.AddElement(14);
            sortingClass.AddElement(9);
            
            Console.WriteLine("List before sorting");
            sortingClass.displayList();
            
            //sortingClass.SetStrategy(new QuickSort());
            Console.WriteLine("List after sorting");
            sortingClass.SetStrategy(new BubbleSort());
            sortingClass.SortList();
            sortingClass.displayList();
        }
    }
}

public abstract class SortStrategy
{
    public abstract void Sort(List<int> list);
}

public class QuickSort : SortStrategy
{
    public override void Sort(List<int> list)
    {
        list.Sort();
    }
}

public class BubbleSort : SortStrategy
{
    public override void Sort(List<int> list)
    {
        //Bubble Sort
        bool wasMoved = false;
        do
        {
            wasMoved = false;
            for (int i = 0; i < list.Count() - 1; i++)
            {
                if (list[i] > list[i + 1])
                {
                    var lower = list[i + 1];
                    list[i + 1] = list[i];
                    list[i] = lower;
                    wasMoved = true;
                }
            }
        } while (wasMoved);
    }
}

public class SortingClass
{
    private List<int> list;
    private SortStrategy strategy;
    
    public SortingClass()
    {
        list = new List<int>();
    }

    public void SetStrategy(SortStrategy strategy)
    {
        this.strategy = strategy;
    }

    public void AddElement(int value)
    {
        list.Add(value);
    }

    public void SortList()
    {
        strategy.Sort(list);
    }

    public void displayList()
    {
        foreach (var num in list)
        {
            Console.Write($"{num} ");
        }
        Console.WriteLine();
    }
}