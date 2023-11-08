"use strict";

interface Window {
    popupManager: {
        open: Function,
        close: Function
    };
};

class UpdateAutoTransferSchedule {
    $: JQueryStatic;
    topWindow: Window;
    $resultMsg: JQuery;

    constructor($: JQueryStatic, topWindow: Window) {
        this.$ = $;
        this.topWindow = topWindow;
    }

    init = () => {
        this.cacheDOMElements();
        this.registerEvents();
    };
    cacheDOMElements = () => {
        this.$resultMsg = this.$('#result-msg');
    };
    registerEvents = () => {
        var self = this;

        this.$("#opt-schedule-all").click(function () {
            self.checkAllSchedule(this);
        });

        this.$(".opt-schedule").click(function () {
            self.scheduleChanged(this);
        });

        this.$(".btn-cancel").click(function () {
            self.cancelUpdate();
        });
        this.$(".btn-ok").click(function () {
            self.acceptUpdate();
        });
    };

    checkAllSchedule = (elm) => {
        var $this = this.$(elm);
        var isCheck = $this.is(":checked");

        var chks = this.$('.opt-schedule');
        chks.each((_, e) => {
            this.$(e).prop("checked", isCheck);
        });

        this.validateSchedule();
    };

    scheduleChanged = (elm) => {
        var $this = this.$(elm);
        var checkedCount = this.$(".opt-schedule:checked").length;
        this.$('#opt-schedule-all').prop("checked", checkedCount === this.$(".opt-schedule").length);
        this.validateSchedule();
    };

    cancelUpdate = () => {
        this.topWindow.popupManager.close();
    };

    acceptUpdate = () => {
        this.$.blockUI();

        var scheduleDates = [];
        var checkeds = this.$(".opt-schedule:checked");

        checkeds.each((_, e) => {
            scheduleDates.push(this.$(e).attr("data-val"));
        });

        self.$.ajax({
            method: "POST",
            data: {
                CustId: this.$("#CustId").val(),
                Schedule: scheduleDates.join(','),
                CustName: this.$("#CustName").val(),
                Old_TransferOption: this.$("#Old_TransferOption").val(),
            },
            dataType: 'json',
            url: "PostUpdateSchedule",
            success: results => {
                this.$resultMsg.removeClass('msg-success msg-error msg-warning');
                if (results.Code === 0 || results.Code === -1) {
                    this.$resultMsg.addClass(results.Code === 0 ? 'msg-success' : 'msg-warning');
                    this.$resultMsg.html(results.Message);
                    this.$resultMsg.removeClass('hide');
                    this.topWindow.closePopupAndReloadMainFrameAfterUpdateSuccess(3000);

                    return;
                }

                this.$resultMsg.addClass('msg-error');
                this.$resultMsg.html(results.Message);
                this.$resultMsg.removeClass('hide');

                if (results.Code !== 108 || results.Code !== 105) {
                    return;
                }

                if (results.Code === 108) {
                    // ADBT changed
                    this.topWindow.indexManager.updateCurrentUrl("adbt=False");
                }

                this.topWindow.indexManager.addCustomCookies('messageForReloadSite', results.Message);
                this.topWindow.location.reload();
            },
            error: (e) => {
                if (e.status !== 200) {
                    this.$resultMsg.html(e.statusText);
                    this.$resultMsg.removeClass('msg-success msg-warning');
                    this.$resultMsg.addClass('msg-error');
                    this.$resultMsg.removeClass('hide');
                }
            },
            complete: () => { this.$.unblockUI(); }
        });
    };

    validateSchedule = () => {
        if (this.$(".opt-schedule:checked").length === 0) {
            this.$('.btn-ok').attr('disabled', 'disabled');
            return;
        }

        this.$('.btn-ok').attr('disabled', null);
    }
}

$(() => {
    const handler = new UpdateAutoTransferSchedule($, window.top);
    handler.init();
});
