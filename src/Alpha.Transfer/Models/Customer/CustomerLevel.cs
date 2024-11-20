namespace Alpha.Transfer.Models.Customer
{
    public class CustomerLevel
    {
        public int CustId { get; set; }

        public byte RoleId { get; set; }

        public string Username { get; set; }

        public string FirstName { get; set; }

        public string LastName { get; set; }

        public string Username2 { get; set; }

        public int? AgentId { get; set; }

        public int? MasterId { get; set; }
    }
}
