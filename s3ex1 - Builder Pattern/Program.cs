using System;

namespace s3ex1___Builder_Pattern
{
    class Program
    {
        static void Main(string[] args)
        {
            Cook mazlum = new Cook();
            KebabBuilder americanKebab = new AmericanKebabBuilder();
        
            mazlum.MakeKebab(americanKebab);
            Kebab kebab = americanKebab.getKebab();
            Console.WriteLine(kebab.ToString());
        }
    }
}

//Builder
public interface  KebabBuilder
{
    void BuildDough(); 
    void BuildSauce();
    void BuildMeat();
    void BuildAdditions();
    Kebab getKebab();
}

//Director
class Cook
{
    public void MakeKebab(KebabBuilder kebabBuilder)
    {
        kebabBuilder.BuildDough();
        kebabBuilder.BuildSauce();
        kebabBuilder.BuildMeat();
        kebabBuilder.BuildAdditions();
    }
}

class AmericanKebabBuilder : KebabBuilder
{
    private Kebab kebab = new Kebab();
    
    public void BuildDough()
    {
        kebab.doughType = "pita";
    }

    public void BuildSauce()
    {
        kebab.sauce = "mild";
    }

    public void BuildMeat()
    {
        kebab.meatType = "mixed";
    }

    public void BuildAdditions()
    {
        kebab.additions = "fries";
    }

    public Kebab getKebab()
    {
        return kebab;
    }
}

public class Kebab
{
    public string doughType { get; set; }
    public string sauce { get; set; }
    public string meatType { get; set; }
    public string additions { get; set; }
    
    public override string ToString()
    {
        return $"Kebab = {doughType} {sauce} {meatType} {additions}";
    }
}