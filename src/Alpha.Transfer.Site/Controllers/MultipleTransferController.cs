namespace Alpha.Transfer.Site.Controllers
{
    using System.Web.Mvc;
    using MediatR;
    using Requests.MultipleTransfer;
    using Sunplus.Agent.Model.Entities;

    public class MultipleTransferController : SiteBaseController
    {
        public MultipleTransferController(IMediator mediator, IUserSession userSession)
            : base(mediator, userSession)
        {
        }

        [HttpGet]
        public ActionResult CustomerList(CustomerListQuery message)
            => View(mediator.Send(message));
    }
}