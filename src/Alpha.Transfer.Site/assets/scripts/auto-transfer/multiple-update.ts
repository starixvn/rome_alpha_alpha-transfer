"use strict";

interface Window {
    OnPopupComplete(isReload: boolean): void;
    bootboxConfirm(message: string, handleElement: object, acceptCallback: Function, cancelCallback: Function): void;
    reloadMainFrame: any;
}

class MultipleUpdateAutoTransfer {
    $: JQueryStatic;
    topWindow: Window;
    $switch: JQuery;
    confirmTurnOnAutoTransfer: string;
    confirmTurnOffAutoTransfer: string;
    autoTransferUpdateSuccess: string;

    custIds: string;
    usernames: string;
    $resultMsg: JQuery;
    lblOn: string;
    lblOff: string;
    state = 'noset';

    constructor($: JQueryStatic, topWindow: Window) {
        this.$ = $;
        this.topWindow = topWindow;
    }

    init = () => {
        this.cacheDOMElements();
        this.registerEvents();
    };
    cacheDOMElements = () => {
        this.$switch = this.$('.switch-auto-transfer');
        this.confirmTurnOnAutoTransfer = this.$('#confirmTurnOnAutoTransfer').val();
        this.confirmTurnOffAutoTransfer = this.$('#confirmTurnOffAutoTransfer').val();
        this.autoTransferUpdateSuccess = this.$('#autoTransferUpdateSuccess').val();

        this.custIds = this.$('#custIds').val();
        this.usernames = this.$('#usernames').val();
        this.$resultMsg = this.$('#result-msg');
        this.lblOn = this.$('#lblOn').val();
        this.lblOff = this.$('#lblOff').val();
    };
    registerEvents = () => {
        var self = this;

        this.resetSwitch();
    };
    resetSwitch = () => {
        var self = this;

        self.$switch.attr("data-value", null);
        this.$switch.setTheSwitch({
            onLabel: this.lblOn,
            offLabel: this.lblOff,
            onClickOn: (e) => {
                this.topWindow.bootboxConfirm(this.confirmTurnOnAutoTransfer, null, () => {
                    this.update('on');
                }, () => {
                    this.$switch.setTheSwitchValue(this.state);
                });
            },
            onClickOff: (e) => {
                this.topWindow.bootboxConfirm(this.confirmTurnOffAutoTransfer, null, () => {
                    this.update('off');
                }, () => {
                    this.$switch.setTheSwitchValue(this.state);
                });
            }
        });
    };
    update = (state) => {
        this.$.blockUI();

        self.$.ajax({
            method: "POST",
            data: {
                SearchAccountIds: this.custIds,
                State: state,
                Usernames: this.usernames
            },
            dataType: 'json',
            url: "PostUpdateMultiple",
            success: results => {
                this.$resultMsg.removeClass('msg-success msg-error msg-warning');

                if (results.Code !== 0) {
                    this.$resultMsg.addClass('msg-error');
                    this.$resultMsg.html(results.Message);
                    this.$resultMsg.removeClass('hide');
                    this.$switch.setTheSwitchValue(this.state);


                    if (results.Code === 100) {
                        this.topWindow.reloadMainFrame();
                    }

                    return;
                }

                this.state = state;
                if (results.Message !== '') {
                    this.$resultMsg.addClass('msg-warning');
                    this.$resultMsg.html(results.Message);
                    this.$resultMsg.removeClass('hide');
                } else {
                    this.$resultMsg.addClass('msg-success');
                    this.$resultMsg.html(this.autoTransferUpdateSuccess);
                    this.$resultMsg.removeClass('hide');
                }

                setTimeout(() => {
                    if (this.topWindow.reloadMainFrame) {
                        this.topWindow.reloadMainFrame();
                    }
                }, 3000);
            },
            error: (e) => {
                if (e.status !== 200) {
                    this.$resultMsg.html(e.statusText);
                    this.$resultMsg.removeClass('msg-success msg-warning');
                    this.$resultMsg.addClass('msg-error');
                    this.$resultMsg.removeClass('hide');
                    this.resetSwitch();
                }
            },
            complete: () => { this.$.unblockUI(); }
        });
    };
}

$(() => {
    const handler = new MultipleUpdateAutoTransfer($, window.top);
    handler.init();
});
