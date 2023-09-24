using Chain.RealWorld;

namespace ChainResp.Approvers
{
    /// <summary>
    /// The 'Handler' abstract class
    /// </summary>
    public abstract class Notification
    {
        protected Notification? successor;
        
        public Notification Next(Notification successor)
        {
            this.successor = successor;
            return this;
        }
        public abstract void NotifyDriveAdded(DriveModel model);
        public abstract void NotifyRemved(DriveModel model);
    }
}