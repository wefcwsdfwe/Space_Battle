namespace SpaceBattle.Lib;

public class ActionCommand : ICommand
{
    private Action<object[]> cmd;
    private object[] args;

    public ActionCommand(Action<object[]> cmd, params object[] args)
    {
        this.cmd = cmd;
        this.args = args;
    }

    public void Execute()
    {
        cmd(args);
    }
}
