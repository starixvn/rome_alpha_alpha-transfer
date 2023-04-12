"use strict";

class AutoRefreshOption {
    constructor(
        public refreshDone: () => void = null) {
    }
}

class AutoRefreshComponent {
    $: JQueryStatic;
    countDownSeconds = -1;
    isStartCountdown: boolean;

    norefresh: string;
    options: AutoRefreshOption;

    constructor($: JQueryStatic) {
        this.$ = $;
    }

    render = (options: AutoRefreshOption = new AutoRefreshOption()) => {
        this.options = options;

        var self = this;

        var refreshValues = this.$('#refresh-value a');
        var ddlRefresh = this.$('#ddlRefresh');
        this.norefresh = ddlRefresh.data('norefresh');

        refreshValues
            .off('click')
            .on('click', function () {
                var t = parseInt(self.$(this).data('refresh-value'));

                if (t === self.countDownSeconds) {
                    return;
                }

                self.countDownSeconds = parseInt(self.$(this).data('refresh-value'));
                ddlRefresh.text(t > 0 ? self.countDownSeconds : this.norefresh);

                if (self.isStartCountdown) {
                    self.stopCountdown();
                }

                if (t > 0) {
                    self.startCountdown();
                }
            });
    };

    stopCountdown = function () {
        this.isStartCountdown = false;

        var ddlRefresh = this.$('#ddlRefresh');
        var countDown = ddlRefresh.data('countdown');

        if (countDown) {
            countDown.stop();

            var t = this.countDownSeconds;
            ddlRefresh.text(t > 0 ? this.countDownSeconds : this.norefresh);
        }
    };

    startCountdown = function () {
        var self = this;
        var ddlRefresh = this.$('#ddlRefresh');
        this.isStartCountdown = false;

        ddlRefresh.countdown({
            date: +(new Date) + this.countDownSeconds * 1000,
            render: function (data) {
                var t = data.min * 60 + data.sec;

                if (t != 0) {
                    ddlRefresh.text(t);
                }
            },
            onEnd: function () {
                self.options.refreshDone();
            }
        });

        ddlRefresh.data('countdown').update(+(new Date) + this.countDownSeconds * 1000).start();
        this.isStartCountdown = true;
    };

    reset = () => {
        var ddlRefresh = this.$('#ddlRefresh');
        var t = this.countDownSeconds;
        ddlRefresh.text(t > 0 ? this.countDownSeconds : this.norefresh);

        if (t > 0) {
            this.startCountdown();
        }
    };
}