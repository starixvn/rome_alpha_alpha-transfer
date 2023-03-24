"use strict";
declare var numeral: any;

interface Object {
    entries: any;
}

class AutoTransferHistory {
    $: JQueryStatic;
    topWindow: Window;
    numeral: any;

    $tabMonitoring: JQuery;
    virtualDirectory: string;
    noInformationIsAvailable: string;
    ViewerId: string;
    SubAccId: string;

    $username: JQuery;
    $status: JQuery;
    $tblMonitoring: JQuery;
    $sorters: JQuery;
    $pager: JQuery;
    pagerComponent: PagerComponent;
    dateRangePickerComponent: DateRangePickerComponent;
    visibleColumnDropdownComponent: VisibleColumnDropdownComponent;
    autoRefreshComponent: AutoRefreshComponent;
    $statusComponent: MultipleSelectionDropdownComponent;
    amountFormat = "0,0.[00]";

    sortDirection = {
        asc: 0,
        desc: 1
    };

    reportParam: any;

    constructor(
        $: JQueryStatic,
        topWindow: Window,
        numeral: any,
        pageComponent: PagerComponent,
        dateRangePickerComponent: DateRangePickerComponent,
        visibleColumnDropdownComponent: VisibleColumnDropdownComponent,
        autoRefreshComponent: AutoRefreshComponent,
        statusComponent: MultipleSelectionDropdownComponent) {
        this.$ = $;
        this.topWindow = topWindow;
        this.numeral = numeral;
        this.pagerComponent = pageComponent;
        this.dateRangePickerComponent = dateRangePickerComponent;
        this.visibleColumnDropdownComponent = visibleColumnDropdownComponent;
        this.autoRefreshComponent = autoRefreshComponent;
        this.$statusComponent = statusComponent;
    }

    init = () => {
        this.cacheDOMElements();
        this.registerEvents();
    };
    cacheDOMElements = () => {
        this.$tabMonitoring = this.$("#tab-monitoring");
        this.virtualDirectory = this.$("#VirtualDirectory").val();
        this.noInformationIsAvailable = this.$("#NoInformationIsAvailable").val();

        this.$username = this.$("#txt-username");
        this.$status = this.$("#ddl-status");
        this.$tblMonitoring = this.$("#tbl-monitoring");
        this.$sorters = this.$(".sorting");
        this.$pager = this.$("#report-pager");
        this.ViewerId = this.$("#ViewerId").val();
        this.SubAccId = this.$("#SubAccId").val();
    };
    registerEvents = () => {
        this.$tabMonitoring.click(() => this.viewMonitoring());
        this.$('.icon-information-outline').click(() => this.viewInfo());
        this.$('.btn-submit:not(.dropdown-toggle)').click(() => {
            this.viewTransfers();
            return false;
        });

        var self = this;
        this.$('.sorting').click(function () {
            self.sort(this);
        });

        this.dateRangePickerComponent.render();
        this.$('#report-form').submit(() => { return false; });

        var visibleColumnDropdownOption = new VisibleColumnDropdownOption(
            `TransferHistory_${this.ViewerId}_${this.SubAccId}:Columns`,
            (col, isShow) => this.showHideColumn(col, isShow));
        this.visibleColumnDropdownComponent.render(visibleColumnDropdownOption);

        this.$statusComponent.render(
            new MultipleSelectionDropdownOption("ddl-status", "Status", this.changeStatus));

        this.autoRefreshComponent.render(new AutoRefreshOption(
            () => {
                this.refreshTransfers();
            }));
    };

    viewMonitoring = () => {
        location.href = this.$tabMonitoring.attr("data-tablink");
    };

    viewInfo = () => {
        var url = "/" + this.virtualDirectory + "autotransfer/hint";
        this.topWindow.popupManager.open(url, "", 500, 520);
    };

    viewTransfers = () => {
        this.$.blockUI();

        this.reportParam = {
            Username: this.$username.val(),
            Status: this.$statusComponent.getSelectedItem(),
            PageIndex: alphaTransfer.pager.pageIndex,
            PageSize: alphaTransfer.pager.pageSize,
            SortedBy: 1,
            Direction: this.sortDirection.asc,
            FromDate: this.$("#from-date").val(),
            ToDate: this.$("#to-date").val(),
        };

        this.updatePager(alphaTransfer.pager.pageIndex, alphaTransfer.pager.pageSize);
        self.$.ajax({
            method: "POST",
            data: this.reportParam,
            dataType: 'json',
            url: "GetHistoryTransfers",
            success: results => { this.showTransfers(results); },
            error: (e) => { },
            complete: () => { this.$.unblockUI(); }
        });
    };

    showTransfers = (results) => {
        var html = '';

        if (results.Transfers.length === 0) {
            html = '<tr><td colspan="7" class="text-center">' + this.noInformationIsAvailable + '</td></tr>';
        } else {
            for (var i = 0; i < results.Transfers.length; i++) {
                var t = results.Transfers[i];

                html += '<tr>' +
                    '<td col-value="1" class="text-center">' + (i + 1) + '</td>' +
                    '<td col-value="2" class="text-left">' + t.Username + '</td>' +
                    '<td col-value="3" class="text-left">' + t.FirstLastName + '</td>' +
                    '<td col-value="4" class="text-right">' + this.numeral(t.TransferAmount).format(this.amountFormat) + '</td>' +
                    '<td col-value="5" class="text-left col-status ' + t.Status.toLowerCase() + '">' + this.$('#' + t.Status).val() + '</td>' +
                    '<td col-value="6" class="text-left">' + t.Reason + '</td>' +
                    '<td col-value="7" class="text-center">' + (t.CompletedTime || '') + '</td>' +
                    '</tr>';
            }
        }

        this.$tblMonitoring.find('tbody').empty().html(html);
        this.$sorters.removeClass("sorting_asc sorting_desc");

        var selectedSort = this.$tblMonitoring.find('.sorting[data-sort="' + results.SortedBy + '"]');
        selectedSort.addClass('sorting_' + (results.Direction === this.sortDirection.asc ? 'asc' : 'desc'));

        this.$pager.attr('data-pageindex', results.PageIndex);
        this.$pager.attr('data-pagesize', results.PageSize);
        this.$pager.attr('data-totalrecords', results.TotalItems);

        this.pagerComponent.reset();
        this.pagerComponent.render(new PagerOption(
            () => {
                var pageIndex = parseInt(this.$('#PageIndex').val(), 0);
                var pageSize = parseInt(this.$('#PageSize').val(), 0);

                this.pagingTransfers(pageIndex, pageSize);
            }));

        var visibleStatus = this.visibleColumnDropdownComponent.getVisibleStatus();

        for (const [key, value] of Object.entries(visibleStatus)) {
            this.showHideColumn(key, value);
        }

        this.$tblMonitoring.removeClass('hide');
    };
    sort = (elm) => {
        var $elm = this.$(elm);

        var sortedBy = $elm.data('sort');
        var direction = $elm.hasClass('sorting_asc')
            ? this.sortDirection.desc : this.sortDirection.asc;
        this.$.blockUI();

        this.updatePager(alphaTransfer.pager.pageIndex, alphaTransfer.pager.pageSize);
        this.reportParam.SortedBy = sortedBy;
        this.reportParam.Direction = direction;

        self.$.ajax({
            method: "POST",
            data: this.reportParam,
            dataType: 'json',
            url: "GetHistoryTransfers",
            success: results => { this.showTransfers(results); },
            error: (e) => { },
            complete: () => { this.$.unblockUI(); }
        });
    };

    pagingTransfers = (pageIndex, pageSize) => {
        this.$.blockUI();
        this.updatePager(pageIndex, pageSize);

        self.$.ajax({
            method: "POST",
            data: this.reportParam,
            dataType: 'json',
            url: "GetHistoryTransfers",
            success: results => { this.showTransfers(results); },
            error: (e) => { },
            complete: () => { this.$.unblockUI(); }
        });
    };

    showHideColumn = (col, isShow) => {
        if (isShow) {
            this.$tblMonitoring.find('th[col-value="' + col + '"]').removeClass('hide');
            this.$tblMonitoring.find('td[col-value="' + col + '"]').removeClass('hide');

        } else {
            this.$tblMonitoring.find('th[col-value="' + col + '"]').addClass('hide');
            this.$tblMonitoring.find('td[col-value="' + col + '"]').addClass('hide');
        }
    };

    refreshTransfers = () => {
        if (!this.reportParam) {
            this.autoRefreshComponent.reset();
            return;
        }

        this.$.blockUI();

        self.$.ajax({
            method: "POST",
            data: this.reportParam,
            dataType: 'json',
            url: "GetHistoryTransfers",
            success: results => { this.showTransfers(results); },
            error: (e) => { },
            complete: () => {
                this.autoRefreshComponent.reset();
                this.$.unblockUI();
            }
        });
    };
    changeStatus = () => {
        this.$statusComponent.updateSelectedValue();
    };
    updatePager = (pageIndex, pageSize) => {
        this.$pager.data('pageindex', pageIndex);
        this.$pager.data('pagesize', pageSize);

        this.reportParam.PageIndex = pageIndex;
        this.reportParam.PageSize = pageSize;
    };
}

$(() => {
    const handler = new AutoTransferHistory(
        $,
        window.top,
        numeral,
        new PagerComponent($),
        new DateRangePickerComponent($),
        new VisibleColumnDropdownComponent($),
        new AutoRefreshComponent($),
        new MultipleSelectionDropdownComponent($));
    handler.init();
});
