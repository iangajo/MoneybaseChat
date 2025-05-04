using MoneybaseChat.Domain.Entities;
using MoneybaseChat.Domain.Enums;
using MoneybaseChat.Domain.Helpers;
using MoneybaseChat.Domain.Interfaces;

namespace MoneybaseChat.Infrastructure.Services
{
    public class AgentsService : IAgentsService
    {

        private List<Agent> _agents = new List<Agent>()
        {
            new Agent()
            {
                Id = 1,
                IsOnShift = true,
                IsOverflow = false,
                SeniorityLevel = SeniorityLevel.MidLevel,
                CurrentChats = 0
            },
            new Agent()
            {
                Id = 2,
                IsOnShift = true,
                IsOverflow = false,
                SeniorityLevel = SeniorityLevel.Junior,
                CurrentChats = 0
            },
            new Agent()
            {
                Id = 3,
                IsOnShift = true,
                IsOverflow = false,
                SeniorityLevel = SeniorityLevel.Junior,
                CurrentChats = 0
            }
        };

        private List<Agent> _overFlowAgents = new List<Agent>()
        {
            new Agent()
            {
                Id = 100,
                IsOnShift = true,
                IsOverflow = true,
                SeniorityLevel = SeniorityLevel.Junior,
                CurrentChats = 0
            },
            new Agent()
            {
                Id = 200,
                IsOnShift = true,
                IsOverflow = true,
                SeniorityLevel = SeniorityLevel.Junior,
                CurrentChats = 0
            },
        };


        public async Task<Agent?> GetAgentToAssingChatSession()
        {
            var agentList = GetAllAgents();

            var agents = agentList.Where(s => s.IsOnShift).OrderBy(s => EfficiencyHelper.Efficiency[s.SeniorityLevel]).ToList();

            //Juniors
            var juniorAgents = agents.Where(s => s.CurrentChats < (int)10 * EfficiencyHelper.Efficiency[s.SeniorityLevel] && s.SeniorityLevel == SeniorityLevel.Junior).ToList();

            if (juniorAgents.Any())
            {
                return await Task.FromResult(juniorAgents.OrderBy(s => s.CurrentChats).First());
            }

            //Mid
            var midAgents = agents.Where(s => s.CurrentChats < (int)10 * EfficiencyHelper.Efficiency[s.SeniorityLevel] && s.SeniorityLevel == SeniorityLevel.MidLevel).ToList();

            if (midAgents.Any())
            {
                return await Task.FromResult(midAgents.First());
            }
            
            //Seniors
            var seniorAgents = agents.Where(s => s.CurrentChats < (int)10 * EfficiencyHelper.Efficiency[s.SeniorityLevel] && s.SeniorityLevel == SeniorityLevel.Senior).ToList();

            if (seniorAgents.Any())
            {
                return await Task.FromResult(seniorAgents.First());
            }

            //Team leads
            var teamLeads = agents.Where(s => s.CurrentChats < (int)10 * EfficiencyHelper.Efficiency[s.SeniorityLevel] && s.SeniorityLevel == SeniorityLevel.TeamLead).ToList();

            if (teamLeads.Any())
            {
                return await Task.FromResult(teamLeads.First());
            }

            return null;
        }

        public async Task<int> GetAgentsCapacityOfOnShiftAsync()
        {
            var agents = GetAllAgents();

            var capacity = EfficiencyHelper.CalculateTeamCapacity(agents);

            return await Task.FromResult(capacity);
        }

        public Task UpdateCurrentChat(int agentId)
        {
            _agents.First(s => s.Id == agentId).CurrentChats += 1;

            return Task.CompletedTask;
        }

        private List<Agent> GetAllAgents()
        {
            var allAgents = new List<Agent>();

            allAgents.AddRange(_agents);

            if (Common.IsOfficeHours(DateTime.Now))
            {
                allAgents.AddRange(_overFlowAgents);
            }

            return allAgents;
        }
    }
}
