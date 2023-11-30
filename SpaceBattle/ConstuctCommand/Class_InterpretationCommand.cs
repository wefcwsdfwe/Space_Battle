using Hwdtech;
namespace SpaceBattle.Lib;

public class InterpretationCommand : ICommand
{
    private IMessage mess;

    public InterpretationCommand(IMessage mess)
    {
        this.mess = mess;
    }

    public void Execute()
    {
        var command = IoC.Resolve<ICommand>("ConstructCommand", mess);
        IoC.Resolve<ICommand>("GameQueue.PushCommand", mess.GameId, command).Execute();
    }
}
