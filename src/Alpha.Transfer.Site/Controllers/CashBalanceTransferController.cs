namespace Alpha.Transfer.Site.Controllers
{
    using System.Threading.Tasks;
    using System.Web.Mvc;
    using System.Web.SessionState;
    using AppResources;
    using MediatR;
    using Requests.CashBalanceTransfer;
    using Sunplus.Agent.Model.Entities;
    using Transfer.Models;

    [SessionState(SessionStateBehavior.Default)]
    public class CashBalanceTransferController : SiteBaseController
    {
        private readonly IUserSession userSession;

        public CashBalanceTransferController(IMediator mediator, IUserSession userSession)
            : base(mediator)
        {
            this.userSession = userSession;
        }

        [HttpGet]
        public async Task<ActionResult> CashBalance(CashBalanceTransferQuery message)
        {
            if (message == null || !message.IsValid() || !userSession.IsAllDayBalTransfer)
            {
                return View(InvalidPageViewPath);
            }

            return View(await mediator.SendAsync(message));
        }

        [HttpPost, ValidateAntiForgeryToken]
        public async Task<ActionResult> CashBalance(CashBalanceTransferCommand command)
        {
            Result result;

            if (command == null || !command.IsValid() || !userSession.IsAllDayBalTransfer)
            {
                result = Result.CreateErrorResult(Resources.BadRequest, 400);
            }
            else if (userSession.IsInternal)
            {
                result = Result.CreateErrorResult(Resources.NotAuthorized, 403);
            }
            else
            {
                result = await mediator.SendAsync(command);
            }

            return Json(result, JsonRequestBehavior.DenyGet);
        }

        [HttpGet]
        public async Task<ActionResult> Hint(HintTransferQuery message)
        {
            if (message == null || !message.IsValid())
            {
                return View(InvalidPageViewPath);
            }

            var viewModel = await mediator.SendAsync(message);

            return View(GetHintViewNameByLanguage(message.Language), viewModel);
        }

        [NonAction]
        private string GetHintViewNameByLanguage(string language)
        {
            switch (language)
            {
                case "zh-TW":
                case "zh-CN":
                case "th-TH":
                case "vi-VN":
                    return $"{nameof(Hint)}.{language}";

                default:
                    return nameof(Hint);
            }
        }
    }
}