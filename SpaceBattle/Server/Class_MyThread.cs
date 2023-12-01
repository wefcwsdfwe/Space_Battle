namespace SpaceBattle.Lib;
using Hwdtech;

public class MyThread
{
    private bool stop = false;
    public IReceiver queue;
    private Thread thread;
    private Action strategy;
    public void HardStop()
    {
        stop = true;
    }

    internal void HandleCommand()
    {
        var cmd = queue.Receive();
        try { cmd.Execute(); }
        catch (Exception ex)
        {
            var list = new List<Type> { cmd.GetType(), ex.GetType() };
            var handle = IoC.Resolve<ICommand>("GetExceptionHandler", list);
            handle.Execute();
        }
    }

    public MyThread(IReceiver queue)
    {
        this.queue = queue;
        strategy = () =>
        {
            HandleCommand();
        };
        thread = new Thread(() =>
        {
            while (!stop)
            {
                strategy();
            }
        });
    }
    internal void UpdateBehaviour(Action newBehaviour)
    {
        strategy = newBehaviour;
    }
    public void Execute()
    {
        thread.Start();
    }
    public int GetThreadHash()
    {
        return thread.GetHashCode();
    }

}
