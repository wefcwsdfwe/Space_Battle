namespace SpaceBattle.Lib;

public class DefaultHandler : IExceptionHandler
{
    private Exception ex;
    public DefaultHandler(Exception ex){
        this.ex = ex;
    }
    public void Handle()
    {
        throw this.ex;
    }
}
