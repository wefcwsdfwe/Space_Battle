using Hwdtech;


namespace SpaceBattle.Lib;

public class StartMoveCommand : ICommand
{
    IMoveCommandStartable order { get; }

    public StartMoveCommand(IMoveCommandStartable UnicObj)
    {
        order = UnicObj;
    }

    public void Execute()
    {
        order.action.ToList().ForEach(o => IoC.Resolve<ICommand>("Сomprehensive.SetProperty", order.Uobj, o.Key, o.Value).Execute());
        ICommand MCommand = IoC.Resolve<ICommand>("Operation.Move", order.Uobj);
        IoC.Resolve<ICommand>("Сomprehensive.SetProperty", order.Uobj, "Commands.Movement", MCommand).Execute();
        IoC.Resolve<ICommand>("Queue.Push", MCommand).Execute();
    }
}
