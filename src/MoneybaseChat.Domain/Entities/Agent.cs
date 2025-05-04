using MoneybaseChat.Domain.Enums;

namespace MoneybaseChat.Domain.Entities
{
    public class Agent
    {
        public int Id { get; set; }
        public SeniorityLevel SeniorityLevel { get; set; }
        public bool IsOnShift { get; set; } = false;
        public bool IsOverflow { get; set; } = false;
        public int CurrentChats { get; set; } = 0;
    }
}
