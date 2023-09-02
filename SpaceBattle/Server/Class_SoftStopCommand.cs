namespace SpaceBattle.Lib;

public class SoftStopCommand: ICommand{
    private MyThread thread;
    private Action evHandlerSS;
    public SoftStopCommand(MyThread thread){
        this.thread = thread;

        this.evHandlerSS = () => {
            if(thread.GetReceiver().IsEmpty()){
                thread.Stop();
            } else {
                var cmd = queue.Receive(); 
                
                try{
                    cmd.Execute();
                } catch (Exception e){
                    ICommand handleCommand = IoC.Resolve<ICommand>("ExceptionHandle", cmd, e);
                    handleCommand.Execute();
                }
            };
        };
    }
    public void StartSS(){
        thread.UpdateBehaviour(() => {
            evHandlerSS();
        });
    }
}