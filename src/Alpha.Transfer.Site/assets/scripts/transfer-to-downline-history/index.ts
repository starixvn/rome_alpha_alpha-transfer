"use strict";

declare var initAutoComplete: Function;

class TransferToDownlineHistory {
    $: JQueryStatic;
    $filterForm: JQuery;
    $username: JQuery;
    $usernameAutoSuggestionPath: JQuery;
    $output: JQuery;
    $exportExcel: JQuery;
    $exportExcelForm: JQuery;
    $pageSize: JQuery;
    $selPageSize: JQuery;

    dateRangePickerComponent: DateRangePickerComponent;

    constructor(
        $: JQueryStatic,
        dateRangePickerComponent: DateRangePickerComponent
    ) {
        this.$ = $;
        this.dateRangePickerComponent = dateRangePickerComponent;
    }

    public init(): void {
        this.cacheDOMElements();
        this.registerEvents();

        this.dateRangePickerComponent.render();
    }

    public cacheDOMElements(): void {
        this.$filterForm = this.$("#filter-form");
        this.$username = this.$("#txt-username");
        this.$usernameAutoSuggestionPath = this.$("#usernameAutoSuggestionPath");
        this.$output = this.$("#output");
        this.$exportExcel = this.$("#icon-excel");
        this.$exportExcelForm = this.$("#export-excel-form");
        this.$pageSize = this.$("#txt-pagesize");
        this.$selPageSize = this.$("#sel-pagesize");
    }

    public registerEvents(): void {
        const queryUsernameURL = `${this.$usernameAutoSuggestionPath.val()}?${this.$.param({ isDir: 1, issub: 0, ismember: 0 })}`;
        initAutoComplete(this.$username[0], queryUsernameURL);

        const positionTop = 30;
        this.$output.css("top", this.$username.offset().top + positionTop);

        this.$username.keyup(() => {
            if (this.$username.val().trim() === "") {
                this.$username.addClass("text-italic");
            } else {
                this.$username.removeClass("text-italic");
            }
        });

        this.$exportExcel.click(this.exportExcelHandler);

        this.$selPageSize.on("change", this.changePageSizeHandler);
    }

    public exportExcelHandler = () => {
        this.$exportExcelForm.submit();
    }

    public changePageSizeHandler = (event) => {
        this.$pageSize.val(this.$(event.currentTarget).find(":selected").text());
        this.$filterForm.submit();
    };
}

$(() => {
    const transferToDownlineHistory = new TransferToDownlineHistory(
        $,
        new DateRangePickerComponent($));
    transferToDownlineHistory.init();
});