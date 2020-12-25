namespace Alpha.Transfer.Site.Controllers
{
    using MediatR;
    using Requests.TransferToDownlineHistory;
    using System.Web.Mvc;
    using System.Web.SessionState;

    [SessionState(SessionStateBehavior.Default)]
    public class TransferToDownlineHistoryController : SiteBaseController
    {
        public TransferToDownlineHistoryController(IMediator mediator) : base(mediator)
        {
        }

        public ActionResult Index(TransferToDownlineHistoryQuery message)
        {
            if (message == null || !message.IsValid())
            {
                return View(InvalidPageViewPath);
            }

            return View(message);
        }

        public ActionResult Excel(TransferToDownlineHistoryQuery message) => Index(message);
    }
}