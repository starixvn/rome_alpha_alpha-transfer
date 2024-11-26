namespace Alpha.Transfer.Services
{
    using System.Collections.Generic;
    using System.Linq;
    using System.Threading.Tasks;
    using Alpha.Transfer.Criteria.Customer;
    using Alpha.Transfer.Models.Customer;
    using Fanex.Data.Repository;

    public interface CustomerService
    {
        Task<IEnumerable<UsernameSuggestion>> GetUsernamesSuggestion(
            int actorId,
            string query,
            bool isInternal,
            bool directDownline,
            bool isAccInfo = false);

        Task UpdateUserSearch(int actorId, string query, string firstName, string lastName, string nickname);

        Task<CustomerLevel> GetCustomerLevel(string username, int actorId);
    }

    public class CustomerServiceImpl : CustomerService
    {
        private readonly IDynamicRepository repository;

        public CustomerServiceImpl(IDynamicRepository repository)
            => this.repository = repository;

        public async Task<IEnumerable<UsernameSuggestion>> GetUsernamesSuggestion(
            int actorId,
            string query,
            bool isInternal,
            bool directDownline,
            bool isAccInfo = false)
            => (await repository.FetchAsync<UsernameSuggestion>(new GetUsernameSuggestionCriteria(
                actorId,
                query,
                isInternal,
                directDownline,
                isAccInfo))).OrderBy(item => item.Username);

        public Task UpdateUserSearch(int actorId, string query, string firstName, string lastName, string nickname)
            => repository.ExecuteAsync(new UpdateUserSearchCriteria(actorId, query, firstName, lastName, nickname));

        public Task<CustomerLevel> GetCustomerLevel(string username, int actorId)
            => repository.GetAsync<CustomerLevel>(new GetCustomerLevelCriteria(username, actorId));
    }
}
