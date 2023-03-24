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
    custIds: string;
    $resultMsg: JQuery;
    lblOn: string;
    lblOff: string;

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

        this.custIds = this.$('#custIds').val();
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
                    self.resetSwitch();
                });
            },
            onClickOff: (e) => {
                this.topWindow.bootboxConfirm(this.confirmTurnOffAutoTransfer, null, () => {
                    this.update('off');
                }, () => {
                    self.resetSwitch();
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
                State: state
            },
            dataType: 'json',
            url: "PostUpdateMultiple",
            success: results => {
                this.$resultMsg.html(results.Message);
                if (results.Code !== 0) {
                    this.$resultMsg.addClass('msg-error');
                    this.$resultMsg.removeClass('msg-success hide');
                    return;
                }

                this.$resultMsg.addClass('msg-success');
                this.$resultMsg.removeClass('msg-error hide');

                setTimeout(() => {
                    if (this.topWindow.reloadMainFrame) {
                        this.topWindow.reloadMainFrame();
                    }
                }, 3000);
            },
            error: (e) => {
                if (e.status !== 200) {
                    this.$resultMsg.html(e.statusText);
                    this.$resultMsg.addClass('msg-error');
                    this.$resultMsg.removeClass('msg-success hide');
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
