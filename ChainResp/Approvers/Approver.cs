using Chain.RealWorld;

namespace ChainResp.Approvers
{
    /// <summary>
    /// The 'Handler' abstract class
    /// </summary>
    public abstract class Approver
    {
        protected Approver? successor;
        public void SetSuccessor(Approver successor)
        {
            this.successor = successor;
        }
        public abstract void ProcessRequest(Purchase purchase);
       // public abstract void ProcessRequest(TestOp purchase);
    }
}