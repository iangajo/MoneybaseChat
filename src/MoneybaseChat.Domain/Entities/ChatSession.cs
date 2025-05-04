namespace MoneybaseChat.Domain.Entities
{
    public class ChatSession
    {
        public Guid SessionId { get; set; }
        public bool IsActivve { get; set; } = true;
        public int AssignedAgentId { get; set; }

    }
}
