namespace SpaceBattle.Lib;

public class GameQueuePushCommand : ICommand
{
    private IQueue<ICommand> queue;
    private ICommand cmd;
    public GameQueuePushCommand(IQueue<ICommand> gameQueue, ICommand cmd)
    {
        queue = gameQueue;
        this.cmd = cmd;
    }

    public void Execute() => queue.Push(cmd);
}
