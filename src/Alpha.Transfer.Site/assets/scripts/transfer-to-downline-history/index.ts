"use strict";

declare var initAutoComplete: Function;

class TransferToDownlineHistory extends TransferReport {
    $: JQueryStatic;
    $username: JQuery;
    $usernameAutoSuggestionPath: JQuery;
    $output: JQuery;
    $exportExcel: JQuery;
    $exportExcelForm: JQuery;
    $fromDate: JQuery;
    $toDate: JQuery;

    dateRangePickerComponent: DateRangePickerComponent;

    constructor(
        $: JQueryStatic,
        dateRangePickerComponent: DateRangePickerComponent
    ) {
        super($);

        this.$ = $;
        this.dateRangePickerComponent = dateRangePickerComponent;
    }

    public init(): void {
        super.init();

        this.initUsernameSuggestion();
        this.dateRangePickerComponent.render();
    }

    public initDataTable(): void {
        super.initDataTable({
            serverSide: true,
            ajax: (data, callback) => {
                const params = {
                    UserName: this.$("#UserName").val(),
                    FromDate: this.$("#FromDate").val(),
                    ToDate: this.$("#ToDate").val(),
                    UserSelectedDateRangeOption: this.$("#UserSelectedDateRangeOption").val(),
                    PageIndex: (data.start / data.length) + 1,
                    PageSize: data.length,
                };

                this.getTransferDownlineHistoryData(params, callback);
            },
            columns: this.buildDataTableColumns(),
            columnDefs: [],
            responsive: false,
            isSubmit: false
        });
    }

    private getTransferDownlineHistoryData(params, callback): void {
        this.$.ajax({
            type: "POST",
            url: this.$("#GetTransferToDownlineHistoryDataUrl").val(),
            data: params,
            dataType: "json",
            beforeSend: this.$.blockUI,
            success: function (data) {
                if (data.IsFailed && data.Code === 6001) {
                    window.top["bootboxAlert"](data.Message, null, window.top["headerNav"].reloadPage);
                    window.location.href = window.top["alpha"].urls.mainRootPath + "Dashboard/Index";
                } else {
                    callback({
                        data: data.TransferToDownlines,
                        recordsTotal: data.TotalRecords,
                        recordsFiltered: data.TotalRecords
                    });
                }
            },
            complete: this.$.unblockUI
        });
    }

    private buildDataTableColumns = () => {
        return [
            { className: "col-date", data: "FormattedActionDate" },
            { className: "col-text", data: "DownlineName" },
            { className: "col-text", data: "TransferTypeName" },
            { className: "col-number", data: "FormattedTransferAmount" },
            { className: "col-number", data: "FormattedOldBalance" },
            { className: "col-number", data: "FormattedNewBalance" },
            { data: "Description" },
        ];
    };

    public initUsernameSuggestion(): void {
        const queryUsernameURL = `${this.$usernameAutoSuggestionPath.val()}?${this.$.param({ isDir: 1, issub: 0, ismember: 0 })}`;
        initAutoComplete(this.$username[0], queryUsernameURL);

        let left = 1,
            top = this.$username.offset().top + this.$username.outerHeight(true) + 1,
            width = utilsInstance.isMobileDevice()
                ? utilsInstance.isLandscapeMode() ? `calc(50% - ${left}px)` : `calc(100% - ${left}px)`
                : this.$username.outerWidth() - left;

        this.$output.css({ left, top, width });
    }

    public cacheDOMElements(): void {
        super.cacheDOMElements();

        this.$username = this.$("#txt-username");
        this.$usernameAutoSuggestionPath = this.$("#usernameAutoSuggestionPath");
        this.$output = this.$("#output");
        this.$exportExcel = this.$("#icon-excel");
        this.$exportExcelForm = this.$("#export-excel-form");
        this.$fromDate = this.$("#from-date");
        this.$toDate = this.$("#to-date");
    }

    public registerEvents(): void {
        super.registerEvents();

        this.$username.keyup(() => {
            !this.$username.val().trim()
                ? this.$username.addClass("text-italic")
                : this.$username.removeClass("text-italic");
        });

        this.$exportExcel.click(this.exportExcelHandler);
    }

    public exportExcelHandler = () => {
        this.$exportExcelForm.submit();
    }
}

$(() => {
    const transferToDownlineHistory = new TransferToDownlineHistory(
        $,
        new DateRangePickerComponent($));
    transferToDownlineHistory.init();
});