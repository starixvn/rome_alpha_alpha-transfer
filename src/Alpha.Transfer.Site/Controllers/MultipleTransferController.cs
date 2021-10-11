namespace Alpha.Transfer.Site.Controllers
{
    using System.Web.Mvc;
    using MediatR;
    using Requests.MultipleTransfer;

    public class MultipleTransferController : SiteBaseController
    {
        public MultipleTransferController(IMediator mediator)
            : base(mediator)
        {
        }

        [HttpGet]
        public ActionResult CustomerList(CustomerListQuery message)
            => View(mediator.Send(message));
    }
}