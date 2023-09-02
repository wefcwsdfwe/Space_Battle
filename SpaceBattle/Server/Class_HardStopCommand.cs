namespace SpaceBattle.Lib;

public class HardStopCommand: ICommand{
    private MyThread thread;
    public HardStopCommand(MyThread thread){
        this.thread = thread;
    }
    public void StartHS(){
        thread.Stop();
    }
}