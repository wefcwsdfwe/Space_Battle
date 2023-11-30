using Hwdtech;
namespace SpaceBattle.Lib;

public class ConstructCommandStrategy : IStrategy
{
    public object StartStrategy(params object[] args)
    {
        IMessage mess = (IMessage)args[0];
        var gameObj = IoC.Resolve<IUObject>("GetGameObject", mess.GameItemId);
        var parameters = mess.CommandParams;
        foreach (KeyValuePair<string, object> par in parameters)
        {
            IoC.Resolve<ICommand>("UObjectsetProperty", gameObj, par.Key, par.Value).Execute();
        }
        string dep_name = mess.CommandName+"Command";
        return IoC.Resolve<ICommand>(dep_name, gameObj);
    }
}
