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
        super.initDataTable({ responsive: false });
    }

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
        this.$fromDate.parent().on("dp.show", this.doAdjustDatePickerCalendar);
        this.$toDate.parent().on("dp.show", this.doAdjustDatePickerCalendar);
    }

    public exportExcelHandler = () => {
        this.$exportExcelForm.submit();
    }

    public doAdjustDatePickerCalendar = () => {
        if (utilsInstance.isMobileDevice() && !utilsInstance.isLandscapeMode()) {
            let width = this.$(window).width(),
                widget = this.$(".bootstrap-datetimepicker-widget.dropdown-menu");
            widget.css({ width });
        }
    }
}

$(() => {
    const transferToDownlineHistory = new TransferToDownlineHistory(
        $,
        new DateRangePickerComponent($));
    transferToDownlineHistory.init();
});