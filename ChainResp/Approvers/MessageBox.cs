using Chain.RealWorld;

namespace ChainResp.Approvers
{
    /// <summary>
    /// The 'ConcreteHandler' class
    /// </summary>
    public class MessageBox : Notification
    {
        public override void NotifyDriveAdded(DriveModel model)
        {
            Console.WriteLine("Messagebox");
            successor?.NotifyDriveAdded(model);
        }

        public override void NotifyRemved(DriveModel model)
        {

            successor?.NotifyDriveAdded(model);
        }
    }
}