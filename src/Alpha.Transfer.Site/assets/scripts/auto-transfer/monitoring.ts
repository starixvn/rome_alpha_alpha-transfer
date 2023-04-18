"use strict";
declare var numeral: any;

interface Window {
    bootboxAlert(message: string, handleElement: any, callbackFunction: any): void;
}

interface Object {
    entries: any;
}

class AutoTransferMonitoring {
    $: JQueryStatic;
    topWindow: Window;
    numeral: any;

    $tabHistory: JQuery;
    virtualDirectory: string;
    noInformationIsAvailable: string;
    ViewerId: string;
    SubAccId: string;
    confirmCancelJob: string;

    $username: JQuery;
    $status: JQuery;
    $tblMonitoring: JQuery;
    $sorters: JQuery;
    $pager: JQuery;
    pagerComponent: PagerComponent;
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
        visibleColumnDropdownComponent: VisibleColumnDropdownComponent,
        autoRefreshComponent: AutoRefreshComponent,
        statusComponent: MultipleSelectionDropdownComponent) {
        this.$ = $;
        this.topWindow = topWindow;
        this.numeral = numeral;
        this.pagerComponent = pageComponent;
        this.visibleColumnDropdownComponent = visibleColumnDropdownComponent;
        this.autoRefreshComponent = autoRefreshComponent;
        this.$statusComponent = statusComponent;
    }

    init = () => {
        this.cacheDOMElements();
        this.registerEvents();
    };
    cacheDOMElements = () => {
        this.$tabHistory = this.$("#tab-history");
        this.virtualDirectory = this.$("#VirtualDirectory").val();
        this.noInformationIsAvailable = this.$("#NoInformationIsAvailable").val();

        this.$username = this.$("#txt-username");
        this.$status = this.$("#ddl-status");
        this.$tblMonitoring = this.$("#tbl-monitoring");
        this.$sorters = this.$(".sorting");
        this.$pager = this.$("#report-pager");
        this.ViewerId = this.$("#ViewerId").val();
        this.SubAccId = this.$("#SubAccId").val();
        this.confirmCancelJob = this.$("#confirmCancelJob").val();
    };
    registerEvents = () => {
        this.$tabHistory.click(() => this.viewHistory());
        this.$('.icon-information-outline').click(() => this.viewInfo());
        this.$('.btn-submit').click(() => this.viewTransfers());

        var self = this;
        this.$('.sorting').click(function () {
            self.sort(this);
        });

        var visibleColumnDropdownOption = new VisibleColumnDropdownOption(
            `Transfer_${this.ViewerId}_${this.SubAccId}:Columns`,
            (col, isShow) => this.showHideColumn(col, isShow));
        this.visibleColumnDropdownComponent.render(visibleColumnDropdownOption);

        this.$statusComponent.render(
            new MultipleSelectionDropdownOption("ddl-status", "Status", this.changeStatus));

        this.autoRefreshComponent.render(new AutoRefreshOption(
            () => {
                this.refreshTransfers();
            }));
    };

    viewHistory = () => {
        location.href = this.$tabHistory.attr("data-tablink");
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
            Direction: this.sortDirection.asc
        };
        this.updatePager(alphaTransfer.pager.pageIndex, alphaTransfer.pager.pageSize);

        self.$.ajax({
            method: "POST",
            data: this.reportParam,
            dataType: 'json',
            url: "GetTodayTransfers",
            success: results => { this.showTransfers(results); },
            error: (e) => { },
            complete: () => {
                this.$.unblockUI();
            }
        });
    };

    showTransfers = (results) => {
        var html = '';

        if (results.Transfers.length === 0) {
            html = '<tr><td colspan="8" class="text-center">' + this.noInformationIsAvailable + '</td></tr>';
        } else {
            for (var i = 0; i < results.Transfers.length; i++) {
                var t = results.Transfers[i];
                var cancellable = t.IsAllowCancel;
                var amount = t.TransferAmount ? this.numeral(t.TransferAmount).format(this.amountFormat) : '';

                html += '<tr>' +
                    '<td col-value="1" class="text-center">' + (i + 1) + '</td>' +
                    '<td col-value="2" class="text-left">' + t.Username + '</td>' +
                    '<td col-value="3" class="text-left">' + t.FirstLastName + '</td>' +
                    '<td col-value="4" class="text-right">' + amount + '</td>' +
                    '<td col-value="5" class="text-left col-status ' + t.Status.toLowerCase() + '">' + this.$('#' + t.Status).val() + '</td>' +
                    '<td col-value="6" class="text-left">' + t.Reason + '</td>' +
                    '<td col-value="7" class="text-center">' + (t.CompletedTime || '') + '</td>' +
                    '<td col-value="8" class="text-center">' +
                    (cancellable ? '<div class="icon icon-cancel" data-custid="' + t.CustId + '" data-status="' + t.Status + '"></div>' : '') +
                    '</td>' +
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

        var self = this;
        this.$('.icon-cancel').click(function () {
            self.topWindow.bootboxConfirm(self.confirmCancelJob, null, () => {
                var elm = self.$(this);
                self.cancelJob(elm);
            }, () => { });
        });

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
            url: "GetTodayTransfers",
            success: results => { this.showTransfers(results); },
            error: (e) => { },
            complete: () => { this.$.unblockUI(); }
        });
    };

    pagingTransfers = (pageIndex, pageSize) => {
        this.$.blockUI();

        console.log(pageIndex + '|' + pageSize);
        this.updatePager(pageIndex, pageSize);

        self.$.ajax({
            method: "POST",
            data: this.reportParam,
            dataType: 'json',
            url: "GetTodayTransfers",
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
            url: "GetTodayTransfers",
            success: results => { this.showTransfers(results); },
            error: (e) => { },
            complete: () => {
                this.autoRefreshComponent.reset();
                this.$.unblockUI();
            }
        });
    };

    cancelJob = (elm) => {
        this.$.blockUI();

        var param = {
            CustId: elm.data('custid'),
            ViewStatus: elm.data('status'),
        };

        self.$.ajax({
            method: "POST",
            data: param,
            dataType: 'json',
            url: "CancelJob",
            success: results => {
                if (results.Code) {
                    this.topWindow.bootboxAlert(results.Message, null, () => this.refreshTransfers());
                } else {
                    this.refreshTransfers();
                }
            },
            error: (e) => { },
            complete: () => { this.$.unblockUI(); }
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
    const handler = new AutoTransferMonitoring(
        $,
        window.top,
        numeral,
        new PagerComponent($),
        new VisibleColumnDropdownComponent($),
        new AutoRefreshComponent($),
        new MultipleSelectionDropdownComponent($));
    handler.init();
});
