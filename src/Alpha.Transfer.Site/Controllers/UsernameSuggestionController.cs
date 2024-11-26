namespace Alpha.Transfer.Site.Controllers
{
    using System.Threading.Tasks;
    using System.Web.Mvc;
    using Alpha.Transfer.Site.Requests;
    using MediatR;

    public class UsernameSuggestionController : SiteBaseController
    {
        public UsernameSuggestionController(IMediator mediator) : base(mediator)
        {
        }

        public Task<ActionResult> Get(UsernameSuggestionQuery message) => JsonAsync(message);
    }
}