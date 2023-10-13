"use strict";

interface Window {
    OnPopupComplete(isReload: boolean): void;
    bootboxConfirm(message: string, handleElement: object, acceptCallback: Function, cancelCallback: Function): void;
    closePopupAndReloadMainFrameAfterUpdateSuccess: any;
}

class UpdateAutoTransferMultiple {
    $: JQueryStatic;
    topWindow: Window;

    $switchOn: JQuery;
    $switchOff: JQuery;
    $resultMsg: JQuery;

    constructor($: JQueryStatic, topWindow: Window) {
        this.$ = $;
        this.topWindow = topWindow;
    }

    init = () => {
        this.cacheDOMElements();
        this.initDOMElements();
        this.registerEvents();
    };
    cacheDOMElements = () => {
        this.$switchOn = this.$('#switch-auto-transfer-on');
        this.$switchOff = this.$('#switch-auto-transfer-off');
        this.$resultMsg = this.$('#result-msg');
    };
    initDOMElements = () => {
        var isMixStatus = this.$('#IsMixStatus').val();
        if (isMixStatus == "1") {
            this.$('.div-separator').removeClass('hide');
        }
    };
    registerEvents = () => {
        var self = this;

        this.setSwitchOn();
        this.setSwitchOff();

        this.$(".div-on-users .opt-schedule-all").click(function () {
            self.checkAllScheduleOn(this);
        });
        this.$(".div-on-users .opt-schedule").click(function () {
            self.scheduleChangedOn(this);
        });

        this.$(".div-off-users .opt-schedule-all").click(function () {
            self.checkAllScheduleOff(this);
        });
        this.$(".div-off-users .opt-schedule").click(function () {
            self.scheduleChangedOff(this);
        });

        this.$(".btn-cancel").click(function () {
            self.cancelUpdate();
        });
        this.$(".btn-ok").click(function () {
            self.confirmUpdate();
        });
    };
    setSwitchOn = () => {
        var self = this;

        self.$switchOn.attr("data-value", null);

        var chks = this.$(".div-on-users .opt-schedule,.div-on-users .opt-schedule-all");
        this.$switchOn.setTheSwitch({
            steps: 2,
            action: 'on',
            onLabel: this.$("#lblOn").val(),
            offLabel: this.$("#lblOff").val(),
            onClickOn: (e) => {
                chks.attr("disabled", null);
                chks.prop('checked', true);
            },
            onClickOff: (e) => {
                chks.prop('checked', false);
                chks.attr("disabled", "disabled");
            }
        });
    };
    setSwitchOff = () => {
        var self = this;

        self.$switchOff.attr("data-value", null);
        var chks = this.$(".div-off-users .opt-schedule,.div-off-users .opt-schedule-all");
        this.$switchOff.setTheSwitch({
            steps: 2,
            action: 'off',
            onLabel: this.$("#lblOn").val(),
            offLabel: this.$("#lblOff").val(),
            onClickOn: (e) => {
                chks.attr("disabled", null);
                chks.prop('checked', true);
            },
            onClickOff: (e) => {
                chks.prop('checked', false);
                chks.attr("disabled", "disabled");
            }
        });
    };

    checkAllScheduleOn = (elm) => {
        var $this = this.$(elm);
        var isCheck = $this.is(":checked");
        var chks = this.$('.div-on-users .opt-schedule');
        chks.each((_, e) => {
            this.$(e).prop("checked", isCheck);
        });

        this.validateSchedule();
    };
    checkAllScheduleOff = (elm) => {
        var $this = this.$(elm);
        var isCheck = $this.is(":checked");
        var chks = this.$('.div-off-users .opt-schedule');
        chks.each((_, e) => {
            this.$(e).prop("checked", isCheck);
        });

        this.validateSchedule();
    };
    scheduleChangedOn = (elm) => {
        var $this = this.$(elm);
        var checkedCount = this.$(".div-on-users .opt-schedule:checked").length;
        this.$('.div-on-users .opt-schedule-all').prop("checked", checkedCount === this.$(".div-on-users .opt-schedule").length);

        this.validateSchedule();
    };
    scheduleChangedOff = (elm) => {
        var $this = this.$(elm);
        var checkedCount = this.$(".div-off-users .opt-schedule:checked").length;
        this.$('.div-off-users .opt-schedule-all').prop("checked", checkedCount === this.$(".div-off-users .opt-schedule").length);

        this.validateSchedule();
    };

    cancelUpdate = () => {
        this.topWindow.popupManager.close();
    };
    confirmUpdate = () => {
        var msg = this.$("#confirmUpdateAutoTransfer").val();
        var onNames = this.$("#OnNames").val();
        var offNames = this.$switchOff.attr('data-value') == 'off' ? '' : this.$("#OffNames").val();

        if (onNames && offNames) {
            msg += " " + onNames + ", " + offNames;
        } else if (onNames) {
            msg += " " + onNames;
        } else {
            msg += " " + offNames;
        }

        this.topWindow.bootboxConfirm(msg, null, () => this.acceptUpdate());
    };
    getSchedule = (container) => {
        var scheduleDates = [];
        var checkeds = this.$(container + " .opt-schedule:checked");

        checkeds.each((_, e) => {
            scheduleDates.push(this.$(e).attr("data-val"));
        });

        return scheduleDates.join(',');
    };
    acceptUpdate = () => {
        this.$.blockUI();

        self.$.ajax({
            method: "POST",
            data: {
                OnIds: this.$("#OnIds").val(),
                OffIds: this.$("#OffIds").val(),
                OnStatus: this.$switchOn.attr('data-value'),
                OffStatus: this.$switchOff.attr('data-value'),
                OnSchedule: this.getSchedule(".div-on-users"),
                OffSchedule: this.getSchedule(".div-off-users"),
            },
            dataType: 'json',
            url: "UpdateMultipleAutoTransfer",
            success: results => {
                this.$resultMsg.removeClass('msg-success msg-error msg-warning');
                if (results.Code === 0) {
                    this.$resultMsg.addClass('msg-success');
                    this.$resultMsg.html(results.Message);
                    this.$resultMsg.removeClass('hide');
                    this.topWindow.closePopupAndReloadMainFrameAfterUpdateSuccess(3000);
                    return;
                }

                this.$resultMsg.addClass('msg-error');
                this.$resultMsg.html(results.Message);
                this.$resultMsg.removeClass('hide');
                if (results.Code === 100) {
                    this.topWindow.bootboxAlert(results.Message, null, () => { });
                    this.topWindow.main.location.reload(true);
                }
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
        if (this.$('.div-off-users .opt-schedule-all').attr('disabled') != 'disabled'
            && this.$(".div-off-users .opt-schedule:checked").length === 0) {
            this.$('.btn-ok').attr('disabled', 'disabled');
            return;
        }

        if (this.$('.div-on-users .opt-schedule-all').attr('disabled') != 'disabled'
            && this.$(".div-on-users .opt-schedule:checked").length === 0) {
            this.$('.btn-ok').attr('disabled', 'disabled');
            return;
        }

        this.$('.btn-ok').attr('disabled', null);
    }
}

$(() => {
    const handler = new UpdateAutoTransferMultiple($, window.top);
    handler.init();
});
