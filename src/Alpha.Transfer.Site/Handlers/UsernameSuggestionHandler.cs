namespace Alpha.Transfer.Site.Handlers
{
    using System.Collections.Generic;
    using System.Threading.Tasks;
    using Alpha.Transfer.Models.Customer;
    using Alpha.Transfer.Services;
    using Alpha.Transfer.Site.Requests;
    using MediatR;

    public class UsernameSuggestionHandler : IAsyncRequestHandler<UsernameSuggestionQuery, IEnumerable<UsernameSuggestion>>
    {
        private readonly CustomerService customerService;

        public UsernameSuggestionHandler(CustomerService customerService)
        {
            this.customerService = customerService;
        }

        public Task<IEnumerable<UsernameSuggestion>> Handle(UsernameSuggestionQuery message)
            => customerService.GetUsernamesSuggestion(
                message.ViewerId,
                message.Query,
                message.IsInternal,
                message.DirectDownlines,
                message.IsAccInfo);
    }
}