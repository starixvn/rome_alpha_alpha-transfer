namespace Alpha.Transfer.Site.Controllers
{
    using System.Web.Mvc;
    using MediatR;
    using Requests.TransferToDownlineHistory;

    public class TransferToDownlineHistoryController : SiteBaseController
    {
        public TransferToDownlineHistoryController(IMediator mediator) : base(mediator)
        {
        }

        public ActionResult Index(TransferToDownlineHistoryQuery message) => View(message);

        public ActionResult Excel(TransferToDownlineHistoryExcelQuery message) => ExcelView(message);

        public JsonResult GetTransferToDownlineHistoryData(TransferToDownlineHistoryPagingQuery message)
            => Json(mediator.Send(message), JsonRequestBehavior.DenyGet);
    }
}