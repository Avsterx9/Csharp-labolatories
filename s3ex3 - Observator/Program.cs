using System;
using System.Collections.Generic;

/*
 * Implement the Observator class for selected collection. When value changed, display info in console what has been changed
 */

namespace s3ex3___Observator
{
    class Program
    {
        static void Main(string[] args)
        {
            Subject<string> subject = new Subject<string>();
            Observer<string> observer = new Observer<string>();
            subject.Attach(observer);
            subject.Add("one");
            subject.Add("two");
            subject.Add("three");
            Console.WriteLine("------");
            subject.Remove("four");
        }
    }
}

public class Subject<T>
{
    private List<Observer<T>> observers = new();
    private List<T> list = new();
    public T element;
    public string status = "";

    public T getElement()
    {
        return element;
    }

    public List<T> getList()
    {
        return list;
    }

    public void Attach(Observer<T> observer)
    {
        this.observers.Add(observer);
    }
    
    public void Detach(Observer<T> observer)
    {
        this.observers.Remove(observer);
    }

    public void Notify()
    {
        foreach (var observer in observers)
        {
            observer.UpdateData(this);
        }
    }

    public void Add(T element)
    {
        this.element = element;
        this.status = "Added";
        list.Add(element);
        Notify();
    }

    public void Remove(T element)
    {
        if(list.Contains(element))
        {
            this.element = element;
            this.status = "Deleted";
            list.Remove(element);
            Notify();
        }
        else
        {
            Console.WriteLine("Element does not exist!");
        }
    }
}

public class Observer<T>
{
    public void UpdateData(Subject<T> subject)
    {
        Console.WriteLine($"{subject.status} object: {subject.getElement()}");
    }
}