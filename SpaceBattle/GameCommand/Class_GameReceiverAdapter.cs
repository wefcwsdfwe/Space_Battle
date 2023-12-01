namespace SpaceBattle.Lib;

public class GameRecieverAdapter : IReceiver
{
    Queue<ICommand> queue;

    public GameRecieverAdapter(Queue<ICommand> queue)
    {
        this.queue = queue;
    }
    public bool isEmpty()
    {
        return queue.Count == 0;
    }
    public ICommand Receive()
    {
        return queue.Dequeue();
    }
}
