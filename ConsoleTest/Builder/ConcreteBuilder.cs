namespace ConsoleTest.Builder;

public class ConcreteBuilder : IToastBuilder
{
    private readonly Product _product = new();
    private Director _director => new(this);

    Product IToastBuilder.GetProduct()
    {
        return _product;
    }

    Director IToastBuilder.GetDirector()
    {
        return _director;
    }

    public IToastBuilder BuildMessage()
    {
        _product.Part1 = "Part 1";
        return this;
    }

    public IToastBuilder BuildButtons()
    {
        _product.Part2 = "Part 2";
        return this;
    }

    public IToastBuilder BuildTag()
    {
        _product.Part3 = "Part 3";
        return this;
    }
}