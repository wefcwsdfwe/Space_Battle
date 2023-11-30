namespace SpaceBattle.Lib;

public interface IMessage{
    string CommandName{get;}
    string GameId{get;}
    string GameItemId{get;}
    IDictionary<string,object> CommandParams{get;}
}
