"use strict";

interface Window {
    popupManager: {
        open: Function,
        close: Function
    };

    bootboxAlert(message: string): void;
    menu: any;
};

class UpdateSingleAutoTransfer {
    $: JQueryStatic;
    topWindow: Window;

    constructor($: JQueryStatic, topWindow: Window) {
        this.$ = $;
        this.topWindow = topWindow;
    }

    init = () => {
        this.registerEvents();
    };

    registerEvents = () => {
        var self = this;

        this.$(".btn-cancel").click(() => {
            this.cancelUpdate();
        });
        this.$(".btn-ok").click(() => {
            this.acceptUpdate();
        });

        this.$("#opt-schedule-all").click(() => {
            this.checkAllSchedule();
        });

        this.$(".opt-schedule").click(function () {
            self.scheduleChanged(this);
        });
    };

    cancelUpdate = () => {
        this.topWindow.popupManager.close();
    };

    acceptUpdate = () => {
        this.$.blockUI();

        var state = this.$("#State").val();
        var chks = this.$('.opt-schedule:checked');
        var schedule = [];
        for (var i = 0; i < chks.length; i++) {
            var c = chks[i];
            schedule.push(c.attributes['data-val'].value);
        }

        self.$.ajax({
            method: "POST",
            data: {
                CustId: this.$("#CustId").val(),
                CustName: this.$("#CustName").val(),
                State: state,
                Old_TransferOption: this.$("#Old_TransferOption").val(),
                Schedule: state === "off" ? "" : schedule.join(",")
            },
            dataType: 'json',
            url: "Update",
            success: result => {
                var msg = result.Message;
                var code = result.Code;

                if (!code || code == -1) {
                    this.topWindow.bootboxAlert(msg);
                    this.topWindow.closePopupAndReloadMainFrameAfterUpdateSuccess(3000);
                    this.topWindow.menu.location.reload();
                    return;
                }

                switch (code) {
                    case 108:
                        this.topWindow.indexManager.updateCurrentUrl("adbt=True");
                    case 105:
                        this.topWindow.indexManager.addCustomCookies('messageForReloadSite', msg);
                        this.topWindow.location.reload();
                        break;
                    default:
                        this.topWindow.bootboxAlert(msg);
                        break;
                }

            },
            error: (e) => {
                if (e.status === 200) {
                    return;
                }

                this.topWindow.bootboxAlert(e.statusText);
            },
            complete: () => { this.$.unblockUI(); }
        });
    };

    checkAllSchedule = () => {
        var isCheck = this.$("#opt-schedule-all").is(':checked');
        this.$('.opt-schedule').prop("checked", isCheck);
        this.validateAutoTransferSchedule();
    };

    validateAutoTransferSchedule = () => {
        var disabled = this.$(".opt-schedule:checked").length === 0 ? 'disabled' : null;
        this.$(".btn-ok").attr('disabled', disabled);
    };

    scheduleChanged = (elm) => {
        var chksCount = this.$('.opt-schedule').length;
        var checkedCount = this.$('.opt-schedule:checked').length;
        this.$('#opt-schedule-all').prop("checked", checkedCount == chksCount);
        this.validateAutoTransferSchedule();
    };
}

$(() => {
    const handler = new UpdateSingleAutoTransfer($, window.top);
    handler.init();
});
