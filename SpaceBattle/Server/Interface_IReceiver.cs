namespace SpaceBattle.Lib;
public interface IReceiver{
    ICommand Receive();

    bool IsEmpty();
}