namespace SpaceBattle.Lib;
using Hwdtech;

public class ThreadCreateAndStartCommandStrategy : IStrategy
{
    public object StartStrategy(params object[] args)
    {
        string thread_id = (string)args[0];
        Action? acmd = null;
        if (args.Length > 1)
        {
            acmd = (Action)args[1];
        }
        return new ActionCommand((arg) =>
        {
            try{
            var cmd = new ThreadCreateAndStartCommand(thread_id);
            cmd.Execute();}
            finally{
            if (acmd != null)
            {
                acmd();
            }}
        });
    }
}
