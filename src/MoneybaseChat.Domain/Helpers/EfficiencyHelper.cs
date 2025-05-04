using MoneybaseChat.Domain.Entities;
using MoneybaseChat.Domain.Enums;

namespace MoneybaseChat.Domain.Helpers
{
    public static class EfficiencyHelper
    {
        public static int CalculateTeamCapacity(List<Agent> agents)
        {
            double total = agents.Sum(a => 10 * Efficiency[a.SeniorityLevel]) * 1.5;
            return (int)Math.Floor(total);
        }

        public static Dictionary<SeniorityLevel, double> Efficiency = new()
        {
            { SeniorityLevel.Junior, 0.4 },
            { SeniorityLevel.MidLevel, 0.6 },
            { SeniorityLevel.Senior, 0.8 },
            { SeniorityLevel.TeamLead, 0.5 }
        };
    }
}
