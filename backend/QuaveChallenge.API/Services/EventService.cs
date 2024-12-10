using System.Collections.Generic;
using System.Threading.Tasks;
using QuaveChallenge.API.Models;
using QuaveChallenge.API.Data;
using QuaveChallenge.API.Services;
using Microsoft.EntityFrameworkCore;

namespace QuaveChallenge.API.Services
{
    public class EventService : IEventService
    {
        private readonly ApplicationDbContext _context;

        public EventService(ApplicationDbContext context)
        {
            _context = context;
        }

        public async Task<IEnumerable<Community>> GetCommunitiesAsync()
        {
            // TODO: Implement get communities
            return await _context.Communities.ToListAsync();
            //return new List<Community>();
        }

        public async Task<IEnumerable<Person>> GetPeopleByEventAsync(int communityId)
        {
            // TODO: Implement get people by event
            //return new List<Person>();
            return await _context.People
                        .Include(p => p.Community) // Inclui a entidade relacionada Community
                        .Where(p => p.CommunityId == communityId)
                        .ToListAsync();
        }

        public async Task<Person> CheckInPersonAsync(int personId)
        {
            // TODO: Implement check-in
            var person = await _context.People.FindAsync(personId);

            if (person == null)
            {
                return null; // Ou lançar uma exceção, dependendo do comportamento esperado
            }

            // Verifica se a pessoa já tem um check-in, caso contrário, define a data de check-in
            if (person.CheckInDate == null)
            {
                person.CheckInDate = DateTime.Now;  // Grava a data e hora do check-in
            }
            else
            {
                // Caso já tenha feito check-in, você pode lançar um erro ou retornar um status apropriado
                return null;  // Ou lançar uma exceção, dependendo do comportamento esperado
            }

            // Salva as alterações no banco de dados
            await _context.SaveChangesAsync();

            return person;  // Retorna a pessoa com o campo CheckInDate atualizado
        }

        public async Task<Person> CheckOutPersonAsync(int personId)
        {
            // TODO: Implement check-out
            var person = await _context.People.FindAsync(personId);

            if (person == null)
            {
                return null; // Ou lançar uma exceção, dependendo do comportamento esperado
            }

            // Verifica se a pessoa já tem um check-in, caso contrário, define a data de check-in
            if (person.CheckOutDate == null)
            {
                person.CheckOutDate = DateTime.Now;  // Grava a data e hora do check-in
            }
            else
            {
                // Caso já tenha feito check-in, você pode lançar um erro ou retornar um status apropriado
                return null;  // Ou lançar uma exceção, dependendo do comportamento esperado
            }

            // Salva as alterações no banco de dados
            await _context.SaveChangesAsync();

            return person;  // Retorna a pessoa com o campo CheckOutDate atualizado
        }

        public async Task<EventSummary> GetEventSummaryAsync(int communityId)
        {
            // TODO: Implement get summary
            //return new EventSummary();
            var community = await _context.Communities
                                   .Include(c => c.People)
                                   .FirstOrDefaultAsync(c => c.Id == communityId);

            if (community == null)
            {
                return null; // Ou lançar uma exceção, dependendo do comportamento esperado
            }

            var summary = new EventSummary
            {
                TotalPeople = community.People.Count,
                CheckedInPeople = community.People.Count(p => p.CheckInDate.HasValue ),
                CheckedOutPeople = community.People.Count(p => p.CheckOutDate.HasValue),
                NoCheckedInPeople = community.People.Count(p => !p.CheckOutDate.HasValue)
            };

            return summary;
        }
    }
} 