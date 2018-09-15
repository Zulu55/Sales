namespace Sales.Common.Models
{
    public class UserRequest
    {
        public string FirstName { get; set; }

        public string LastName { get; set; }

        public string EMail { get; set; }

        public string Phone { get; set; }

        public string Address { get; set; }

        public string Password { get; set; }

        public string ImagePath { get; set; }

        public byte[] ImageArray { get; set; }
    }
}
