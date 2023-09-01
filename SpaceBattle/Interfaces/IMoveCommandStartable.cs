namespace SpaceBattle.Lib
{
    public interface IMoveCommandStartable
    {
        IUObject Uobj{ get; }
        IDictionary<string, object> action{ get; }
    }
}
