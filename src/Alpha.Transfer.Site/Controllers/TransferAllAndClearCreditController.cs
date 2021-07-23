namespace Alpha.Transfer.Site.Controllers
{
    using System.Web.Mvc;
    using System.Web.SessionState;
    using MediatR;
    using Requests.TransferAllAndClearCredit;
    using Sunplus.Agent.Model.Entities;

    [SessionState(SessionStateBehavior.Default)]
    public class TransferAllAndClearCreditController : TransferAllClearCreditControllerBase
    {
        public TransferAllAndClearCreditController(IMediator mediator, IUserSession userSession)
            : base(mediator, userSession)
        {
        }

        [HttpGet]
        public ActionResult Index(CalculationQuery message)
        {
            AssignDarkModeViewData();
            if (message == null || !message.IsValid())
            {
                return View(InvalidPageViewPath);
            }

            return View(message);
        }

        [HttpGet]
        public ActionResult TransferResult(CalculationQuery message)
        {
            AssignDarkModeViewData();
            if (message == null || !message.IsValid())
            {
                return View(InvalidPageViewPath);
            }

            return View("~/Views/TransferAllAndClearCredit/Index.cshtml", mediator.Send(message));
        }

        [HttpPost, ValidateAntiForgeryToken]
        public ActionResult TransferResult(TransferResultQuery message)
        {
            AssignDarkModeViewData();
            if (message == null || !message.IsValid())
            {
                return View(InvalidPageViewPath);
            }

            return View(mediator.Send(message));
        }
    }
}
