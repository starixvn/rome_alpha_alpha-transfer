namespace Alpha.Transfer.Site.Controllers
{
    using System.Threading.Tasks;
    using System.Web.Mvc;
    using System.Web.Security;
    using System.Web.SessionState;
    using Alpha.Transfer.AppResources;
    using Alpha.Transfer.Configurations;
    using Alpha.Transfer.Models;
    using Alpha.Transfer.Site.Requests.AutoTransfer;
    using Alpha.Transfer.Site.ViewModels.AutoTransfer;
    using MediatR;
    using Sunplus.Agent.Model.Entities;

    [SessionState(SessionStateBehavior.Default)]
    public class AutoTransferController : SiteBaseController
    {
        private readonly IUserSession userSession;

        public AutoTransferController(IMediator mediator, IUserSession userSession)
            : base(mediator)
        {
            this.userSession = userSession;
        }

        [HttpPost]
        public async Task<ActionResult> Update(UpdateQuery message)
        {
            Result result;

            if (message == null || !message.IsValid())
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
        public async Task<ViewResult> UpdateMultiple(UpdateMultipleQuery message)
        {
            var viewModel = await mediator.SendAsync(new GetTransferConditionQuery(message));

            return View(viewModel);
        }

        [HttpPost]
        public async Task<ActionResult> PostUpdateMultiple(UpdateMultipleQuery message)
        {
            Result result;

            if (message == null || !message.IsValid())
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
        public ViewResult Monitoring()
            => View(new MonitoringViewModel(userSession) { IsProtectedMode = userSession.IsProtectedMode });

        [HttpGet]
        public ViewResult History()
            => View(new HistoryViewModel(userSession));

        [HttpGet]
        public ViewResult Hint()
            => View();

        [HttpPost]
        public async Task<ActionResult> GetTodayTransfers(GetTodayTransfersQuery message)
        {
            var result = await mediator.SendAsync(message);

            return Json(result, JsonRequestBehavior.DenyGet);
        }

        [HttpPost]
        public async Task<ActionResult> GetHistoryTransfers(GetHistoryTransfersQuery message)
        {
            var result = await mediator.SendAsync(message);

            return Json(result, JsonRequestBehavior.DenyGet);
        }

        [HttpPost]
        public async Task<ActionResult> CancelJob(CancelJobQuery message)
        {
            Result result;

            if (message == null || !message.IsValid())
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

        protected override void OnAuthorization(AuthorizationContext filterContext)
        {
            base.OnAuthorization(filterContext);

            var userSession = UserSession.Current;
            var enabled = AppSettings.IsEnableAutoTransfer(userSession.Currency, userSession.IsSubAcc, userSession.SubPageAccess);

            if ((userSession.IsSubAcc && !userSession.SubPageAccess.Transfer) || !enabled)
            {
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
}