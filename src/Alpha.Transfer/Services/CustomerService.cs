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
    }

    public class CustomerServiceImpl : CustomerService
    {
        private readonly IDynamicRepository dynamicRepository;

        public CustomerServiceImpl(IDynamicRepository dynamicRepository)
            => this.dynamicRepository = dynamicRepository;

        public async Task<IEnumerable<UsernameSuggestion>> GetUsernamesSuggestion(
            int actorId,
            string query,
            bool isInternal,
            bool directDownline,
            bool isAccInfo = false)
            => (await dynamicRepository.FetchAsync<UsernameSuggestion>(new GetUsernameSuggestionCriteria(
                actorId,
                query,
                isInternal,
                directDownline,
                isAccInfo))).OrderBy(item => item.Username);
    }
}
