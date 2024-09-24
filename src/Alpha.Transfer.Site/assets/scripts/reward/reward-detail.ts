'use strict';
interface Window {
    columnList: any,
}

class RewardDetailView {
    private filter: FilterComponent;
    private dateRangePicker: DateRangePickerJQueryComponent;
    private programDropdown: SingleSelectionDropdownComponent;
    private paymentStatusDropdown: SingleSelectionDropdownComponent;
    private usernameAutoSuggestion: UsernameAutoSuggestionComponent;
    private visibleColumn: VisibleColumnDropdownNewComponent;
    private sorter: SortComponent;
    private pager: PagerComponent;
    private pageSize: number;
    private copyHtml: CopyHtmlComponent;

    constructor(
        filter: FilterComponent,
        dateRangePicker: DateRangePickerJQueryComponent,
        programDropdown: SingleSelectionDropdownComponent,
        paymentStatusDropdown: SingleSelectionDropdownComponent,
        usernameAutoSuggestion: UsernameAutoSuggestionComponent,
        visibleColumn: VisibleColumnDropdownNewComponent,
        sorter: SortComponent,
        pager: PagerComponent,
        copyHtml: CopyHtmlComponent) {
        this.filter = filter;
        this.dateRangePicker = dateRangePicker;
        this.programDropdown = programDropdown;
        this.paymentStatusDropdown = paymentStatusDropdown;
        this.usernameAutoSuggestion = usernameAutoSuggestion;
        this.visibleColumn = visibleColumn;
        this.sorter = sorter;
        this.pager = pager;
        this.copyHtml = copyHtml;
    }

    init() {
        this.initComponents();
        this.bindTableEvents();

        this.pageSize = +$('#PageSize').val();
        $('#rewardDetail').removeAttr('style');
        alphaTransfer.showFeedback(11);
    }

    private initComponents() {
        this.filter.initShowHide({
            hideText: $('#hiddenHideFilterText').val(),
            showText: $('#hiddenShowFilterText').val()
        });

        this.dateRangePicker.render({
            onClosed: () => this.getRewardReport(),
            language: top.$('#language').data('currentlanguage')
        });

        this.programDropdown.render(new SingleSelectionDropdownOption('ddl-program', undefined, () => this.getRewardReport(), true));
        this.paymentStatusDropdown.render(new SingleSelectionDropdownOption('ddl-paymentStatus', undefined, () => this.getRewardReport()));

        this.usernameAutoSuggestion.init({
            url: $("#hiddenUsernameAutoSuggestionUrl").val(),
            onBlur: () => this.getRewardReport()
        });

        this.visibleColumn.render({
            listColumn: window.columnList,
            onDropdownHidden: (visibleColumn) => this.showHideColumns($('#tbl-transfer'), visibleColumn),
            localStorageKey: $('#hiddenVisibleColumnKey').val(),
            minWidth: 250
        });

        this.copyHtml.init({
            copyText: $('#hiddenCopyText').val(),
            copiedText: $('#hiddenCopiedText').val()
        });

        const $iconExcel = $('#icon-excel').tooltip();
        const excelUrl = $iconExcel.attr('href');
        $iconExcel.click(() => {
            $iconExcel.attr('href', `${excelUrl}?${$.param(this.getSearchData())}`);
            setTimeout(function () {
                $iconExcel.attr('href', 'javascript:;');
            }, 1000);
        }).attr('href', 'javascript:;');
    }

    private getRewardReport(pageIndex: number = undefined, pageSize: number = undefined) {
        $.blockUI();
        const queryString = $.param(this.getSearchData(pageIndex, pageSize || this.pageSize));
        $.get(`GetDetail?${queryString}`, (html) => {
            $('#tbl-transfer-container').html(html);
            this.bindTableEvents();
            alphaTransfer.updateCurrentUrl(queryString);
        }).always($.unblockUI);
    }

    private getSearchData(pageIndex: number = undefined, pageSize: number = undefined) {
        let searchData: any = this.dateRangePicker.getDateRangeValues();

        const username = this.usernameAutoSuggestion.getUsername();
        if (username) {
            searchData.username = username;
        }

        const programId = this.programDropdown.getSelectedItem();
        if (programId !== '0') {
            searchData.programId = programId;
        }

        const paymentStatus = this.paymentStatusDropdown.getSelectedItem();
        if (paymentStatus !== '0') {
            searchData.paymentStatus = paymentStatus;
        }

        if (pageIndex) {
            searchData.pageIndex = pageIndex;
        }

        if (pageSize) {
            searchData.pageSize = pageSize;
        }

        return searchData;
    }

    private bindTableEvents() {
        this.pager.reset();
        this.pager.render(new PagerOption(
            () => {
                let currentPageIndex = +$('#PageIndex').val();
                const currentPageSize = +$('#PageSize').val();
                if (this.pageSize !== currentPageSize) {
                    currentPageIndex = 1;
                    this.pageSize = currentPageSize;
                }

                this.getRewardReport(currentPageIndex, currentPageSize);
            }, 'transfer-pager', 'transfer-form'));

        const $tblTransfer = $('#tbl-transfer');
        this.sorter.tableSorter({ tableElement: $tblTransfer });

        this.showHideColumns($tblTransfer);

        $tblTransfer.stickyTableHeaders('destroy');
        $tblTransfer.stickyTableHeaders();
    }

    private showHideColumns($tblTransfer: JQuery, visibleColumn = undefined) {
        visibleColumn = visibleColumn || this.visibleColumn.getColumns();
        this.showHideSpecificColumn($tblTransfer, visibleColumn, '0');
        this.showHideSpecificColumn($tblTransfer, visibleColumn, '1');
        this.showHideGroupColumn($tblTransfer, visibleColumn, $tblTransfer.find('#groupMember'), ['2', '3', '4', '5', '6']);
        this.showHideGroupColumn($tblTransfer, visibleColumn, $tblTransfer.find('#groupAgent'), ['7', '8', '9']);
        this.showHideGroupColumn($tblTransfer, visibleColumn, $tblTransfer.find('#groupMaster'), ['10', '11', '12']);
    }

    private showHideGroupColumn($tblTransfer: JQuery, visibleColumn, $groupColumn: JQuery, columnIndexes: string[]) {
        let colspan = 0;
        columnIndexes.forEach((item) => {
            if (this.showHideSpecificColumn($tblTransfer, visibleColumn, item)) {
                colspan++;
            }
        });

        if ($groupColumn.length) {
            colspan ? $groupColumn.attr('colspan', colspan).show() : $groupColumn.hide();
        }
    }

    private showHideSpecificColumn($tblTransfer: JQuery, visibleColumn, key: string) {
        const visible = visibleColumn[key];
        $tblTransfer.find(`[data-colindex=${key}]`).toggle(visible);

        return visible;
    }
}

$(function () {
    new RewardDetailView(
        new FilterComponent(),
        new DateRangePickerJQueryComponent(),
        new SingleSelectionDropdownComponent($),
        new SingleSelectionDropdownComponent($),
        new UsernameAutoSuggestionComponent(),
        new VisibleColumnDropdownNewComponent(),
        new SortComponent(),
        new PagerComponent($),
        new CopyHtmlComponent(Clipboard)).init();
});
