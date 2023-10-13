namespace Alpha.Transfer.Site.Controllers
{
    using System.Threading.Tasks;
    using System.Web.Mvc;
    using System.Web.Security;
    using System.Web.SessionState;
    using Alpha.Transfer.AppResources;
    using Alpha.Transfer.Configurations;
    using Alpha.Transfer.Models;
    using Alpha.Transfer.Services;
    using Alpha.Transfer.Site.Requests.AutoTransfer;
    using Alpha.Transfer.Site.ViewModels.AutoTransfer;
    using AlphaPlus.User.Models;
    using MediatR;

    [SessionState(SessionStateBehavior.Default)]
    public class AutoTransferController : SiteBaseController
    {
        private readonly IUserSession userSession;
        private readonly TransferService transferService;
        private const string InvalidTransferStatusViewPath = "~/Views/AutoTransfer/ADBTChange.cshtml";

        public AutoTransferController(
            IMediator mediator,
            IUserSession userSession,
            TransferService transferService)
            : base(mediator)
        {
            this.userSession = userSession;
            this.transferService = transferService;
        }

        [HttpPost]
        public async Task<ActionResult> Update(UpdateQuery message)
        {
            Result result;

            if (transferService.IsAllDayBalanceTransfer(userSession.CustId))
            {
                result = Result.CreateErrorResult(Resources.ADBTChangeError, 100);
            }
            else if (message == null || !message.IsValid())
            {
                result = Result.CreateErrorResult(Resources.BadRequest, 400);
            }
            else if (userSession.IsInternal)
            {
                result = Result.CreateErrorResult(Resources.NotAuthorized, 403);
            }
            else
            {
                result = await mediator.SendAsync(message);
            }

            return Json(result, JsonRequestBehavior.DenyGet);
        }

        [HttpGet]
        public ViewResult Monitoring(MonitoringQuery message)
        {
            if (transferService.IsAllDayBalanceTransfer(userSession.CustId))
            {
                return View(InvalidTransferStatusViewPath);
            }

            return View(new MonitoringViewModel(userSession, message));
        }

        [HttpGet]
        public ViewResult History()
        {
            if (transferService.IsAllDayBalanceTransfer(userSession.CustId))
            {
                return View(InvalidTransferStatusViewPath);
            }

            return View(new HistoryViewModel(userSession));
        }

        [HttpGet]
        public ViewResult Hint()
        {
            if (transferService.IsAllDayBalanceTransfer(userSession.CustId))
            {
                return View(InvalidTransferStatusViewPath);
            }

            return View();
        }

        [HttpPost]
        public async Task<ActionResult> GetTodayTransfers(GetTodayTransfersQuery message)
        {
            if (transferService.IsAllDayBalanceTransfer(userSession.CustId))
            {
                return Json(Result.CreateErrorResult(Resources.ADBTChangeError, 100), JsonRequestBehavior.DenyGet);
            }

            var result = await mediator.SendAsync(message);

            return Json(result, JsonRequestBehavior.DenyGet);
        }

        [HttpPost]
        public async Task<ActionResult> GetHistoryTransfers(GetHistoryTransfersQuery message)
        {
            if (transferService.IsAllDayBalanceTransfer(userSession.CustId))
            {
                return Json(Result.CreateErrorResult(Resources.ADBTChangeError, 100), JsonRequestBehavior.DenyGet);
            }

            var result = await mediator.SendAsync(message);

            return Json(result, JsonRequestBehavior.DenyGet);
        }

        [HttpPost]
        public async Task<ActionResult> CancelJob(CancelJobQuery message)
        {
            Result result;

            if (transferService.IsAllDayBalanceTransfer(userSession.CustId))
            {
                result = Result.CreateErrorResult(Resources.ADBTChangeError, 100);
            }
            else if (message == null || !message.IsValid())
            {
                result = Result.CreateErrorResult(Resources.BadRequest, 400);
            }
            else if (userSession.IsInternal)
            {
                result = Result.CreateErrorResult(Resources.NotAuthorized, 403);
            }
            else
            {
                result = await mediator.SendAsync(message);
            }

            return Json(result, JsonRequestBehavior.DenyGet);
        }

        [HttpPost]
        public async Task<ActionResult> CancelAllJobs(CancelAllJobsQuery message)
        {
            Result result;

            if (transferService.IsAllDayBalanceTransfer(userSession.CustId))
            {
                result = Result.CreateErrorResult(Resources.ADBTChangeError, 100);
            }
            else if (message == null || !message.IsValid())
            {
                result = Result.CreateErrorResult(Resources.BadRequest, 400);
            }
            else if (userSession.IsInternal)
            {
                result = Result.CreateErrorResult(Resources.NotAuthorized, 403);
            }
            else
            {
                result = await mediator.SendAsync(message);
            }

            return Json(result, JsonRequestBehavior.DenyGet);
        }

        [HttpGet]
        public ViewResult UpdateSchedule(UpdateScheduleQuery message)
        {
            if (transferService.IsAllDayBalanceTransfer(userSession.CustId))
            {
                return View(InvalidTransferStatusViewPath);
            }

            return View(message);
        }

        [HttpPost]
        public async Task<ActionResult> PostUpdateSchedule(PostUpdateScheduleQuery message)
        {
            Result result;

            if (transferService.IsAllDayBalanceTransfer(userSession.CustId))
            {
                result = Result.CreateErrorResult(Resources.ADBTChangeError, 100);
            }
            else if (userSession.IsInternal)
            {
                result = Result.CreateErrorResult(Resources.NotAuthorized, 403);
            }
            else if (message == null || !message.IsValid())
            {
                result = Result.CreateErrorResult(Resources.BadRequest, 400);
            }
            else
            {
                result = await mediator.SendAsync(message);
            }

            return Json(result, JsonRequestBehavior.DenyGet);
        }

        [HttpGet]
        public ViewResult UpdateMultipleAutoTransfer(UpdateMultipleAutoTransferQuery message)
        {
            if (transferService.IsAllDayBalanceTransfer(userSession.CustId))
            {
                return View(InvalidTransferStatusViewPath);
            }

            return View(message);
        }

        [HttpPost]
        public async Task<ActionResult> UpdateMultipleAutoTransfer(PostUpdateMultipleAutoTransferQuery message)
        {
            Result result;

            if (transferService.IsAllDayBalanceTransfer(userSession.CustId))
            {
                result = Result.CreateErrorResult(Resources.ADBTChangeError, 100);
            }
            else if (userSession.IsInternal)
            {
                result = Result.CreateErrorResult(Resources.NotAuthorized, 403);
            }
            else if (message == null)
            {
                result = Result.CreateErrorResult(Resources.BadRequest, 400);
            }
            else
            {
                result = await mediator.SendAsync(message);
            }

            return Json(result, JsonRequestBehavior.DenyGet);
        }

        protected override void OnAuthorization(AuthorizationContext filterContext)
        {
            base.OnAuthorization(filterContext);

            var userSession = UserSession.Current;
            var method = filterContext.HttpContext?.Request?.HttpMethod;

            var enabled = transferService.IsEnableAutoTransfer(userSession.CustId, userSession.Currency, userSession.IsSubAcc, userSession.SubPageAccess, AppSettings.GetConfigValue("OfferAutoTransfer.Currencies"));

            if (enabled)
            {
                return;
            }

            filterContext.Result = new HttpUnauthorizedResult("You are not authorized on this function!");

            var session = filterContext.HttpContext.Session;
            session.Clear();
            session.Abandon();
            if (!session.IsCookieless)
            {
                FormsAuthentication.SignOut();
            }
        }
    }
}