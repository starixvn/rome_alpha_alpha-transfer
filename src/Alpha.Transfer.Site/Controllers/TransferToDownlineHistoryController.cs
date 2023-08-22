namespace Alpha.Transfer.Site.Controllers
{
    using System.Web.Mvc;
    using Alpha.Transfer.AppResources;
    using Alpha.Transfer.Models;
    using Alpha.Transfer.Services;
    using MediatR;
    using Requests.TransferToDownlineHistory;

    public class TransferToDownlineHistoryController : SiteBaseController
    {
        private const string InvalidTransferStatusViewPath = "~/Views/Shared/InvalidTransferStatus.cshtml";
        private readonly TransferService transferService;

        public TransferToDownlineHistoryController(
            IMediator mediator,
            TransferService transferService) : base(mediator)
        {
            this.transferService = transferService;
        }

        public ActionResult Index(TransferToDownlineHistoryQuery message)
        {
            if (message != null && transferService.IsAllDayBalanceTransfer(message.ViewerId))
            {
                return View(message);
            }

            return View(InvalidTransferStatusViewPath);
        }

        public ActionResult Excel(TransferToDownlineHistoryExcelQuery message)
        {
            if (message != null && transferService.IsAllDayBalanceTransfer(message.ViewerId))
            {
                return ExcelView(message);
            }

            return View(InvalidTransferStatusViewPath);
        }

        public JsonResult GetTransferToDownlineHistoryData(TransferToDownlineHistoryPagingQuery message)
        {
            if (transferService.IsAllDayBalanceTransfer(message.ViewerId))
            {
                return Json(mediator.Send(message), JsonRequestBehavior.DenyGet);
            }

            return Json(Result.CreateErrorResult(Resources.DisallowedAllDayBalanceTransferError, 6001));
        }
    }
}