namespace SpaceBattle.Lib;

public class MyThread
{
    public IReceiver queue;
    private Thread thread;
    private Action behaviour;
 
    private bool stop = false;
    public void Stop()
    {
        stop = true;
    }
    public MyThread(IReceiver queue)
    {   
        this.queue = queue;
        this.behaviour = () => {
            var cmd = queue.Receive(); 
                
                try{
                    cmd.Execute();
                } catch (Exception e){
                    ICommand handleCommand = IoC.Resolve<ICommand>("ExceptionHandle", cmd, e);
                    handleCommand.Execute();
                }  
        };
        
        thread = new Thread(() =>
        {
            while (!stop)
            {   
                behaviour();                              
            }
        });

    }
    
    public void Start()
    {
        thread.Start();
    }
    public int GetThreadHash()
    {
        return thread.GetHashCode();
    }

    internal void UpdateBehaviour(Action behaviour)
    {
        this.behaviour = behaviour;
    }

    public IReceiver GetReceiver(){
        return queue;
    }

}

public class SoftStopCommand: ICommand{
    private MyThread thread;
    public SoftStopCommand(MyThread thread){
        this.thread = thread;
    }
    public void Execute(){
        
        thread.UpdateBehaviour(() => {
            
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
              
        });
    }
}

public class HardStopCommand: ICommand{
    private MyThread thread;
    public HardStopCommand(MyThread thread){
        this.thread = thread;
    }
    public void Execute(){
        thread.Stop();
    }
}