namespace Sales.Common.Models
{
    using System.Collections.Generic;

    public class MyUserASP
    {
        public string Email { get; set; }

        public bool EmailConfirmed { get; set; }

        public string PasswordHash { get; set; }

        public string SecurityStamp { get; set; }

        public string PhoneNumber { get; set; }

        public bool PhoneNumberConfirmed { get; set; }

        public bool TwoFactorEnabled { get; set; }

        public object LockoutEndDateUtc { get; set; }

        public bool LockoutEnabled { get; set; }

        public int AccessFailedCount { get; set; }

        public List<Claim> Claims { get; set; }

        public string Id { get; set; }

        public string UserName { get; set; }
    }
}