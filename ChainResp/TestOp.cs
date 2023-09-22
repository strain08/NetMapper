namespace Chain.RealWorld
{
    public class TestOp
    {
        int number;
        double amount;
        string purpose;
        // Constructor
        public TestOp(int number, double amount, string purpose)
        {
            this.number = number;
            this.amount = amount;
            this.purpose = purpose;
        }
        // Gets or sets purchase number
        public int Number
        {
            get { return number; }
            set { number = value; }
        }
        // Gets or sets purchase amount
        public double Amount
        {
            get { return amount; }
            set { amount = value; }
        }
        // Gets or sets purchase purpose
        public string Purpose
        {
            get { return purpose; }
            set { purpose = value; }
        }
    }
}

