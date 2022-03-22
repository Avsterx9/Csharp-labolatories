using System;

namespace ex1___Geometric_figure_solver
{
    class Program
    {
        static void Main(string[] args)
        {
            EquilateralTriangle equilateralTriangle = new EquilateralTriangle(4);
            Console.WriteLine(equilateralTriangle.ToString());
            Console.WriteLine("##############");
            IsoscelesTriangle isoscelesTriangle = new IsoscelesTriangle(5,7);
            Console.WriteLine(isoscelesTriangle.ToString());
            Console.WriteLine("##############");
            RectangularTriangle rectangularTriangle = new RectangularTriangle(5,7);
            Console.WriteLine(rectangularTriangle.ToString());
        }
    }
}

public abstract class GeometricFigureSolver{

    protected double a;
    protected double b;
    protected double c;

    public double calculatePerimeter(){
        return a + b + c;
    }

    public double calculateField(){
        double p = calculatePerimeter() / 2;
        return Math.Sqrt(p * (p - a) * (p - b) * (p - c)); //wzór Herona
    }

    public abstract string ToString();
}

public class EquilateralTriangle : GeometricFigureSolver{
    public EquilateralTriangle(double edge){
        a = edge;
        b = edge;
        c = edge;
    }

    public override string ToString(){
        return  "Trójkąt równoboczny \n" + 
                "a = " + a + '\n' +
                "b = " + b + '\n' +
                "c = " + c + '\n' + 
                "Obwód = " + calculatePerimeter() + '\n' + 
                "Pole = " + calculateField() + '\n'; 
    }

}

public class IsoscelesTriangle : GeometricFigureSolver{
    public IsoscelesTriangle(double baseEdge, double edge){
        a = baseEdge;
        b = edge;
        c = edge;
    }

    public override string ToString(){
        return  "Trójkąt równoramienny \n" + 
                "a = " + a + '\n' +
                "b = " + b + '\n' +
                "c = " + c + '\n' + 
                "Obwód = " + calculatePerimeter() + '\n' + 
                "Pole = " + calculateField() + '\n'; 
    }
}

public class RectangularTriangle : GeometricFigureSolver{
    public RectangularTriangle(double firstEdge, double secondEdge){
        a = firstEdge;
        b = secondEdge;
        c = Math.Sqrt(a * a + b * b);
    }

    public override string ToString(){
        return  "Trójkąt prostokątny \n" + 
                "a = " + a + '\n' +
                "b = " + b + '\n' +
                "c = " + c + '\n' + 
                "Obwód = " + calculatePerimeter() + '\n' + 
                "Pole = " + calculateField() + '\n'; 
    }
}