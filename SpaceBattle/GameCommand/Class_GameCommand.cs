using Hwdtech;
namespace SpaceBattle.Lib;

public class GameCommand : ICommand
{
    IReceiver queue;
    object scope;
    string gameId;
    System.Diagnostics.Stopwatch time = new System.Diagnostics.Stopwatch();

    public GameCommand(string gameId, object scope, IReceiver queue)
    {
        this.gameId = gameId;
        this.scope = scope;
        this.queue = queue;
    }

    public void Execute()
    {
        IoC.Resolve<Hwdtech.ICommand>("Scopes.Current.Set",scope).Execute();
        time.Reset();
        while(time.ElapsedMilliseconds <  IoC.Resolve<int>("GetGameTimeLimit")){
            time.Start();
            var cmd = queue.Receive();
            try {cmd.Execute();}
            catch (Exception ex)
            {
                 ex.Data["CmdType"] = cmd.GetType();
                var handle = IoC.Resolve<IExceptionHandler>("GetExceptionHandler", cmd, ex);
                handle.Handle();
            }
            time.Stop();
        }
    }
}
