namespace MoneybaseChat.Domain.Entities
{
    public class ChatSession
    {
        public Guid SessionId { get; set; }
        public bool IsActive { get; set; } = true;
        public int AssignedAgentId { get; set; }

    }
}
