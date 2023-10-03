using Chain.RealWorld;

namespace ChainResp.Approvers
{
    /// <summary>
    /// The 'ConcreteHandler' class
    /// </summary>
    public class MessageBox : Notification
    {
        public override void NotifyDriveAdded(Model m)
        {
            Console.WriteLine("Messagebox" + m.name);
            successor?.NotifyDriveAdded(m);            
        }
    }
}