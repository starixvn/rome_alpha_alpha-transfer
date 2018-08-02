namespace Alpha.Transfer.Site.Controllers
{
    using System.Web.Mvc;
    using System.Web.SessionState;
    using MediatR;
    using Requests.TransferAllAndClearCredit;

    [SessionState(SessionStateBehavior.Default)]
    public class TransferAllAndClearCreditController : SiteBaseController
    {
        public TransferAllAndClearCreditController(IMediator mediator)
            : base(mediator)
        {
        }

        [HttpGet]
        public ActionResult Index(CalculationQuery message)
        {
            if (message == null || !message.IsValid())
            {
                return View(InvalidPageViewPath);
            }

            return View(mediator.Send(message));
        }

        [HttpGet]
        public ActionResult TransferResult(TransferResultQuery message)
        {
            if (message == null || !message.IsValid())
            {
                return View(InvalidPageViewPath);
            }

            return View(mediator.Send(message));
        }
    }
}
