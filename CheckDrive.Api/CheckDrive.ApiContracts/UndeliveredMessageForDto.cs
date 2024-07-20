namespace CheckDrive.ApiContracts
{
    public class UndeliveredMessageForDto
    {
        public SendingMessageStatusForDto SendingMessageStatus { get; set; }
        public int ReviewId { get; set; }
        public string UserId { get; set; }
        public string Message { get; set; }
    }
}
