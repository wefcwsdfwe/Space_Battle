namespace SpaceBattle.Lib;

public class UpdateBehaviourCommand : ICommand
{
    Action newBehaviour;
    MyThread thread;

    public UpdateBehaviourCommand(MyThread thread, Action newBehaviour)
    {
        this.thread = thread;
        this.newBehaviour = newBehaviour;
    }

    public void Execute()
    {
        thread.UpdateBehaviour(newBehaviour);
    }
}
