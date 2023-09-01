using Hwdtech;
namespace SpaceBattle.Lib
{
    public class MacroCommandFactoryStrategy : IStrategy
    {
        public object StartStrategy(params object[] args)
        {
            var name = (string)args[0];
            var uObj = (IUObject)args[1];
            IEnumerable<string> names = IoC.Resolve<IEnumerable<string>>("Config.MacroCommand." + name);
            IEnumerable<ICommand> commands = new List<ICommand>();
            foreach (string command in names)
            {
                commands.Append(IoC.Resolve<ICommand>(command, uObj));
            }
            return IoC.Resolve<ICommand>("SimpleMacroCommand", commands);
        }
    }
}
