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
    cancelAllAutoTransferJobsConfirm: string;

    $username: JQuery;
    $status: JQuery;
    $tblMonitoring: JQuery;
    $sorters: JQuery;
    $pager: JQuery;
    $navTabs: JQuery;
    $cancelAllCount: JQuery;
    $divCancelAll: JQuery;
    $divStatusAll: JQuery;

    pagerComponent: PagerComponent;
    visibleColumnDropdownComponent: VisibleColumnDropdownComponent;
    autoRefreshComponent: AutoRefreshComponent;
    $statusComponent: MultipleSelectionDropdownComponent;
    levelComponent: SingleSelectionDropdownComponent
    directMemberText: string;
    masterAgentText: string;
    isShowLevelColumn: boolean;
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
        statusComponent: MultipleSelectionDropdownComponent,
        levelComponent: SingleSelectionDropdownComponent) {
        this.$ = $;
        this.topWindow = topWindow;
        this.numeral = numeral;
        this.pagerComponent = pageComponent;
        this.visibleColumnDropdownComponent = visibleColumnDropdownComponent;
        this.autoRefreshComponent = autoRefreshComponent;
        this.$statusComponent = statusComponent;
        this.levelComponent = levelComponent;
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
        this.$navTabs = this.$("#nav-tabs");
        this.cancelAllAutoTransferJobsConfirm = this.$("#CancelAllAutoTransferJobsConfirm").val();

        this.$divStatusAll = this.$(".div-status-all");
        this.$divCancelAll = this.$(".div-cancel-all");
        this.$cancelAllCount = this.$(".cancel-all-count");

        this.directMemberText = this.$("#DirectMemberText").val();
        this.masterAgentText = this.$("#MasterAgentText").val();
        this.isShowLevelColumn = typeof this.directMemberText !== "undefined" && typeof this.masterAgentText !== "undefined";
    };

    registerEvents = () => {
        this.$tabHistory.click(() => this.viewHistory());
        this.$('.icon-information-outline').click(() => this.viewInfo());
        this.$('#btn-submit').click(() => this.viewTransfers());
        this.$('.div-cancel-all').click(() => this.cancelAllJobs());
        this.$('#report-form').submit(() => { return false; });

        const self = this;
        this.$('.sorting').click(function () {
            self.sort(this);
        });

        const visibleColumnDropdownOption = new VisibleColumnDropdownOption(
            `Transfer_${this.ViewerId}_${this.SubAccId}:Columns`,
            (visibleStatus) => {
                for (const [key, value] of Object.entries(visibleStatus)) {
                    this.showHideColumn(key, value);
                }
            });
        this.visibleColumnDropdownComponent.render(visibleColumnDropdownOption);

        this.$statusComponent.render(
            new MultipleSelectionDropdownOption("ddl-status", "Status", this.changeStatus));
        this.isShowLevelColumn && this.levelComponent.render(
            new SingleSelectionDropdownOption("ddl-levelType", "levelType"));

        this.autoRefreshComponent.render(new AutoRefreshOption(() => this.refreshTransfers()));

        if (this.$('#status').val() === "Failed") {
            this.viewTransfers();
        }
    };

    viewHistory = () => {
        location.href = this.$tabHistory.attr("data-tablink");
    };

    viewInfo = () => {
        const url = "/" + this.virtualDirectory + "autotransfer/hint";
        this.topWindow.popupManager.open(url, "", 500, 250);
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
        if (this.isShowLevelColumn) {
            this.reportParam.LevelType = this.levelComponent.getSelectedItem();
        }

        self.$.ajax({
            method: "POST",
            data: this.reportParam,
            dataType: 'json',
            url: "GetTodayTransfers",
            success: results => this.showTransfers(results),
            complete: this.$.unblockUI
        });
    };

    showTransfers = (results) => {
        if (results.Code == 100) {
            location.reload();
            return;
        }

        const html = [];

        if (results.Transfers.length === 0) {
            html.push('<tr><td colspan="', this.isShowLevelColumn ? 8 : 7, '" class="text-center">', this.noInformationIsAvailable, '</td></tr>');
        } else {
            for (let i = 0; i < results.Transfers.length; i++) {
                const t = results.Transfers[i];
                const cancellable = t.IsAllowCancel;
                const amount = t.TransferAmount ? this.numeral(t.TransferAmount).format(this.amountFormat) : '';
                const ino = i + 1 + results.PageSize * (results.PageIndex - 1);
                const amountClass = (t.TransferAmount || 0) < 0 ? " format-negative" : "";
                const status = t.Status.toLowerCase() === 'waiting' ? 'Processing' : t.Status;

                html.push('<tr>',
                    '<td col-value="1" class="text-center">', ino + '</td>',
                    '<td col-value="2" class="text-left">', t.Username, '</td>',
                    this.isShowLevelColumn ? `<td col-value="8" class="text-center">${t.IsDirectMember ? this.directMemberText : this.masterAgentText}</td>` : '',
                    '<td col-value="3" class="text-left">', t.FirstLastName, '</td>',
                    '<td col-value="4" class="text-right', amountClass, '">', amount + '</td>',
                    '<td col-value="5" class="text-left col-status ', status.toLowerCase(), '">',
                    '<div class="flex">',
                    this.$('#' + status).val(),
                    (cancellable ? `<div class="icon-close-circle" data-custid="${t.CustId}" data-status="${t.Status}" data-custname="${t.Username}"></div>` : ''),
                    '</div>',
                    '</td>',
                    '<td col-value="6" class="text-left">', (t.Reason || ''), '</td>',
                    '<td col-value="7" class="text-center">', (t.CompletedTime || ''), '</td>',
                    '</tr>');
            }
        }

        this.$tblMonitoring.find('tbody').empty().html(html.join(""));
        this.$sorters.removeClass("sorting_asc sorting_desc");

        const selectedSort = this.$tblMonitoring.find('.sorting[data-sort="' + results.SortedBy + '"]');
        selectedSort.addClass('sorting_' + (results.Direction === this.sortDirection.asc ? 'asc' : 'desc'));

        this.$pager.data('pageindex', results.PageIndex);
        this.$pager.data('pagesize', results.PageSize);
        this.$pager.data('totalrecords', results.TotalItems);

        this.pagerComponent.reset();
        this.pagerComponent.render(new PagerOption(
            () => {
                const pageIndex = parseInt(this.$('#PageIndex').val(), 0);
                const pageSize = parseInt(this.$('#PageSize').val(), 0);

                this.pagingTransfers(pageIndex, pageSize);
            }));

        const self = this;
        this.$('.icon-close-circle').click(function () {
            self.topWindow.bootboxConfirm(self.confirmCancelJob, null, () => {
                const elm = self.$(this);
                self.cancelJob(elm);
            }, () => { });
        });

        const visibleStatus = this.visibleColumnDropdownComponent.getVisibleStatus();

        for (const [key, value] of Object.entries(visibleStatus)) {
            this.showHideColumn(key, value);
        }

        this.$cancelAllCount.html(results.TotalCancelAll);
        this.$(".spn-pending-count").html(results.TotalPending);
        this.$(".spn-processing-count").html(results.TotalProcessing);
        this.$(".spn-failed-count").html(results.TotalFailed);
        this.$divStatusAll.removeClass('hide');

        this.$tblMonitoring.removeClass('hide');
        this.$tblMonitoring.stickyTableHeaders({
            fixedOffset: this.$navTabs
        });
    };

    sort = (elm) => {
        const $elm = this.$(elm);

        const sortedBy = $elm.data('sort');
        const direction = $elm.hasClass('sorting_asc')
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
            success: results => this.showTransfers(results),
            complete: this.$.unblockUI
        });
    };

    pagingTransfers = (pageIndex, pageSize) => {
        this.$.blockUI();

        this.updatePager(pageIndex, pageSize);

        self.$.ajax({
            method: "POST",
            data: this.reportParam,
            dataType: 'json',
            url: "GetTodayTransfers",
            success: results => this.showTransfers(results),
            complete: this.$.unblockUI
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
            success: results => this.showTransfers(results),
            complete: () => {
                this.autoRefreshComponent.reset();
                this.$.unblockUI();
            }
        });
    };

    cancelJob = (elm) => {
        this.$.blockUI();

        const param = {
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
            complete: this.$.unblockUI
        });
    };

    changeStatus = () => this.$statusComponent.updateSelectedValue()

    updatePager = (pageIndex, pageSize) => {
        this.$pager.data('pageindex', pageIndex);
        this.$pager.data('pagesize', pageSize);

        this.reportParam.PageIndex = pageIndex;
        this.reportParam.PageSize = pageSize;
    };

    cancelAllJobs = () => {
        const count = parseInt(this.$cancelAllCount.html());
        let message;

        if (count === 0) {
            message = this.$("#AutoTransfer_NoCancellable").val();
            this.topWindow.bootboxAlert(message, null, () => { });
            return;
        }

        message = this.cancelAllAutoTransferJobsConfirm.replace('{0}', count.toString());
        this.topWindow.bootboxConfirm(message, null, () => { this.doCancelAllJobs(); });
    };

    doCancelAllJobs = () => {
        this.$.blockUI();

        self.$.ajax({
            method: "POST",
            data: this.reportParam,
            dataType: 'json',
            url: "CancelAllJobs",
            success: results => {
                if (results.Code) {
                    this.topWindow.bootboxAlert(results.Message, null, () => this.refreshTransfers());
                } else {
                    this.refreshTransfers();
                    this.topWindow.bootboxAlert(results.Message, null, () => { });
                }
            },
            complete: this.$.unblockUI
        });
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
        new MultipleSelectionDropdownComponent($),
        new SingleSelectionDropdownComponent($));
    handler.init();
});
