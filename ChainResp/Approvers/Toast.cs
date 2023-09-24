using Chain.RealWorld;

namespace ChainResp.Approvers
{
    /// <summary>
    /// The 'ConcreteHandler' class
    /// </summary>
    public class Toast : Notification
    {       

        public override void NotifyDriveAdded(DriveModel m)
        {
            Console.WriteLine("Toast");

            successor?.NotifyDriveAdded(m);
        }

        public override void NotifyRemved(DriveModel model)
        {
            throw new NotImplementedException();
        }
    }
}