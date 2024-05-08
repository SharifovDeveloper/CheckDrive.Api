using CheckDriver.Domain.Common;

namespace CheckDriver.Domain.Entities
{
    public class Role : EntityBase
    {
        public string Name { get; set; }

        public virtual ICollection<Account> Accounts { get; set; }
    }
}
