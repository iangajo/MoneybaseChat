using MoneybaseChat.Domain.Entities;
using MoneybaseChat.Domain.Enums;
using MoneybaseChat.Domain.Helpers;
using MoneybaseChat.Domain.Interfaces;

namespace MoneybaseChat.Infrastructure.Services
{
    internal class AgentsService : IAgentsService
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
            new Agent()
            {
                Id = 300,
                IsOnShift = true,
                IsOverflow = true,
                SeniorityLevel = SeniorityLevel.Junior,
                CurrentChats = 0
            },
            new Agent()
            {
                Id = 400,
                IsOnShift = true,
                IsOverflow = true,
                SeniorityLevel = SeniorityLevel.Junior,
                CurrentChats = 0
            },
            new Agent()
            {
                Id = 500,
                IsOnShift = true,
                IsOverflow = true,
                SeniorityLevel = SeniorityLevel.Junior,
                CurrentChats = 0
            },
            new Agent()
            {
                Id = 600,
                IsOnShift = true,
                IsOverflow = true,
                SeniorityLevel = SeniorityLevel.Junior,
                CurrentChats = 0
            },
        };


        public async Task<Agent?> GetAgentToAssingChatSession(bool isOverflow = false)
        {
            var agentList = GetAllAgents(isOverflow);

            var agents = agentList.Where(s => s.IsOnShift).OrderBy(s => EfficiencyHelper.Efficiency[s.SeniorityLevel]).ToList();

            //Juniors
            var juniorAgents = agents.Where(s => s.CurrentChats < (int)10 * EfficiencyHelper.Efficiency[s.SeniorityLevel] && s.SeniorityLevel == SeniorityLevel.Junior && !s.IsOverflow).ToList();

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

            //Overflow agents
            if (isOverflow)
            {
                var overflowAgents = agents.Where(s => s.CurrentChats < (int)10 * EfficiencyHelper.Efficiency[s.SeniorityLevel] && s.IsOverflow).ToList();
                if (overflowAgents.Any())
                {
                    return await Task.FromResult(overflowAgents.First());
                }
            }

            return null;
        }

        public async Task<int> GetAgentsCapacityOfOnShiftAsync()
        {
            var agents = GetAllAgents(false);

            var capacity = EfficiencyHelper.CalculateTeamCapacity(agents);

            return await Task.FromResult(capacity);
        }

        public Task UpdateCurrentChat(int agentId)
        {
            _agents.First(s => s.Id == agentId).CurrentChats += 1;

            return Task.CompletedTask;
        }

        private List<Agent> GetAllAgents(bool isOverFlow =  false)
        {
            var allAgents = new List<Agent>();

            allAgents.AddRange(_agents);
            
            if (isOverFlow)
            {
                if (Common.IsOfficeHours(DateTime.Now))
                {
                    allAgents.AddRange(_overFlowAgents);
                }
            }

            return allAgents;
        }
    }
}
