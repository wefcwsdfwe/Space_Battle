using Hwdtech;
using Hwdtech.Ioc;

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
    public void Start()
    {
        thread.Start();
    }
    public MyThread(IReceiver queue)
    {   this.queue = queue;
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
