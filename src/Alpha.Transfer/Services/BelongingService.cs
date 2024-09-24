using System.Threading.Tasks;
using Alpha.Transfer.Criteria;
using Fanex.Data.Repository;

namespace Alpha.Transfer.Services
{
    public interface BelongingService
    {
        Task<bool> CheckMultipleCustomersBelonging(int actorId, int subAccId, string custIds);
    }

    public class BelongingServiceImpl : BelongingService
    {
        private readonly IDynamicRepository repository;

        public BelongingServiceImpl(IDynamicRepository repository)
            => this.repository = repository;

        public async Task<bool> CheckMultipleCustomersBelonging(int actorId, int subAccId, string custIds)
        {
            var criteria = new CheckMultipleCustomesBelongingCriteria(actorId, subAccId, custIds);
            await repository.ExecuteAsync(criteria);

            return criteria.RoleId > 0 && criteria.IsActive;
        }
    }
}
