namespace ConsoleTest.Builder;

public class Director
{
    private readonly IToastBuilder builder;

    public Director(IToastBuilder builder)
    {
        this.builder = builder;
    }

    public Product ToastA()
    {
        builder.BuildMessage()
            .BuildButtons()
            .BuildTag();
        return builder.GetProduct();
    }
}