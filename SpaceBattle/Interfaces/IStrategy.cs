namespace SpaceBattle.Lib
{
    public interface IStrategy
    {
        public object StartStrategy(params object[] args);
    }
}
