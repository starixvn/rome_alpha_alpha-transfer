namespace Alpha.Transfer.Site.Controllers
{
    using MediatR;
    using Requests;
    using System.Web.Mvc;
    using ViewModels;
    using Web;

    public abstract class SiteBaseController : BaseController
    {
        protected readonly IMediator mediator;

        protected const string InvalidPageViewPath = "~/Views/Shared/InvalidPage.cshtml";
        protected const string InvalidExcelFileName = "InvalidExcelFile";

        protected SiteBaseController(IMediator mediator)
        {
            this.mediator = mediator;
        }

        protected ActionResult View<TResponse>(IRequest<TResponse> message)
        {
            var query = message as TransferBaseQuery;
            if (query == null || !query.IsValid())
            {
                return View(InvalidPageViewPath);
            }

            return View(mediator.Send(message));
        }

        protected FileContentResult ExcelView(IRequest<ExcelFileViewModel> message)
        {
            var query = message as TransferBaseQuery;
            if (query == null || !query.IsValid())
            {
                return GetExcelContent(new ExcelFileViewModel(InvalidExcelFileName, new byte[] { }));
            }

            return GetExcelContent(mediator.Send(message));
        }

        private FileContentResult GetExcelContent(ExcelFileViewModel viewModel)
            => File(viewModel.Content, viewModel.ContentType, viewModel.Name);
    }
}