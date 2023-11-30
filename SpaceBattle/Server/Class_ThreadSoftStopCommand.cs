namespace SpaceBattle.Lib;

public class ThreadSoftStopCommand : ICommand
{
    MyThread thread;

    public ThreadSoftStopCommand(MyThread thread)
    {
        this.thread = thread;
    }
    

    public void Execute()
    {
        if (Thread.CurrentThread.GetHashCode() == thread.GetThreadHash())
        {
            var cmd = new UpdateBehaviourCommand
            (
                thread,
                () =>
                {
                    if (thread.queue.isEmpty())
                    {
                        thread.HardStop();
                    }
                    else
                    {
                        thread.HandleCommand();
                    }
                }
            );
            cmd.Execute();
        }
        else
        {
            throw new Exception();
        }
    }
}
