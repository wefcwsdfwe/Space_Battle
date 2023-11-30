namespace SpaceBattle.Lib;
using System.Collections.Concurrent;
using Hwdtech;

public class SendCommandStrategy : IStrategy
{
    public object StartStrategy(params object[] args)
    {
        string thread_id = (string)args[0];
        ICommand cmd = (ICommand)args[1];
        return new ActionCommand((arg) =>
        {
            ISender sender = IoC.Resolve<ISender>("Thread.GetSender", thread_id);
            sender.Send(cmd);
        });
    }
}
