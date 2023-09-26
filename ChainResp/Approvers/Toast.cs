using Chain.RealWorld;

namespace ChainResp.Approvers
{
    /// <summary>
    /// The 'ConcreteHandler' class
    /// </summary>
    public class Toast : Notification
    {
        public override void NotifyDriveAdded(Model m)
        {
            Console.WriteLine("Toast"+ m.name);
            m.name = "sss";
            successor?.NotifyDriveAdded(m);

        }


    }
}