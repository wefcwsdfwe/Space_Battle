using System.Collections.Concurrent;

namespace SpaceBattle.Lib;

public class Sender : ISender
{
    BlockingCollection<ICommand> queue;

    public Sender(BlockingCollection<ICommand> queue)
    {
        this.queue = queue;
    }
    public void Send(ICommand cmd)
    {
        queue.Add(cmd);
    }
}