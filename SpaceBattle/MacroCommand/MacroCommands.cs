namespace SpaceBattle.Lib
{
    public class MacroCommands : ICommand
    {
        IEnumerable<ICommand> commands;
        public MacroCommands(IEnumerable<ICommand> commands)
        {
            this.commands = commands;
        }
        public void Execute()
        {
            foreach (ICommand command in commands)
            {
                command.Execute();
            }
        }
    }
}
