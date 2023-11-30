namespace SpaceBattle.Lib;

public class ThreadHardStopCommand : ICommand
{
    MyThread thread;

    public ThreadHardStopCommand(MyThread thread)
    {
        this.thread = thread;
    }

    public void Execute()
    {
        if (Thread.CurrentThread.GetHashCode() == thread.GetThreadHash())
        {
            thread.HardStop();
        }
        else
        {
            throw new Exception();
        }
    }
}
