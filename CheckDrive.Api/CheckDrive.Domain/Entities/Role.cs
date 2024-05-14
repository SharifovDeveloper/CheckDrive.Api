using CheckDrive.Domain.Common;

namespace CheckDrive.Domain.Entities
{
    public class Role : EntityBase
    {
        public string Name { get; set; }

        public virtual ICollection<Account> Accounts { get; set; }
    }
}
