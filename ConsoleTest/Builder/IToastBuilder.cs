namespace ConsoleTest.Builder;

public interface IToastBuilder
{
    Director GetDirector();
    Product GetProduct();
    IToastBuilder BuildMessage();
    IToastBuilder BuildButtons();
    IToastBuilder BuildTag();
}