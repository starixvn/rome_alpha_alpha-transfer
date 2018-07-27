namespace Alpha.Transfer.Site.Controllers
{
    using System.Web.Mvc;
    using MediatR;
    using Web;

    public abstract class SiteBaseController : BaseController
    {
        protected readonly IMediator mediator;
        protected const string InvalidPageViewPath = "~/Views/Shared/InvalidPage.cshtml";

        protected SiteBaseController(IMediator mediator)
        {
            this.mediator = mediator;
        }

        protected ActionResult View<TResponse>(IRequest<TResponse> message)
        {
            return View(mediator.Send(message));
        }
    }
}
