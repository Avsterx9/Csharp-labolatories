using System;

namespace ex1___ONP_expression
{
    class Program
    {
        static void Main(string[] args)
        {
            var a = new OnpExpression("a");
            var b = new OnpExpression("b");
            var c = new OnpExpression("c");
            Console.WriteLine(a - b * c);
        }
    }
}

public class OnpExpression{

    private string expression;

    public OnpExpression(string expression){
        this.expression = expression;
    }
    
    public override string ToString(){
        return expression;
    }

    public static OnpExpression operator- (OnpExpression a, OnpExpression b){
        return new OnpExpression(a.ToString() + b.ToString() + "-");
    }

    public static OnpExpression operator* (OnpExpression a, OnpExpression b){
        return new OnpExpression(a.ToString() + b.ToString() + "*");
    }

    public static OnpExpression operator+ (OnpExpression a, OnpExpression b){
        return new OnpExpression(a.ToString() + b.ToString() + "+");
    }

    public static OnpExpression operator/ (OnpExpression a, OnpExpression b){
        return new OnpExpression(a.ToString() + b.ToString() + "/");
    }
}