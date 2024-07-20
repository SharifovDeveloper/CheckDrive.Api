using CheckDrive.Domain.Common;

namespace CheckDrive.Domain.Entities
{
    public class UndeliveredMessage : EntityBase
    {
        public SendingMessageStatus SendingMessageStatus { get; set; }
        public int ReviewId { get; set; }
        public string UserId { get; set; }
        public string Message { get; set; }
    }
}
