"use strict";

interface Window {
    alpha: any
}

class AlphaTransfer {
    $: JQueryStatic;
    store: StoreJSStatic;

    numberDisplayedSelectedOnFilter = 1;
    pager = {
        pageSize: 500,
        pageIndex: 1
    };

    constructor($: JQueryStatic, store: StoreJSStatic) {
        this.$ = $;
        this.store = store;
    }

    localStorage = {
        get: (key: string) => {
            return this.store.enabled ? this.store.get(key) : this.$.cookie(key);
        },

        set: (key: string, value: any) => {
            this.store.enabled ? this.store.set(key, value) : this.$.cookie(key, value, this.localStorage.getCookieExpireOptions());
        },

        getCookieExpireOptions: () => {
            var cookieExpire: any = { expires: 365, path: window.location.pathname };

            if (this.isIe()) {
                cookieExpire = { expires: 365 };
            }

            return cookieExpire;
        }
    };

    isIe = () => {
        var ua = window.navigator.userAgent;
        var msie = ua.indexOf("MSIE ");

        return msie > 0 || !!navigator.userAgent.match(/Trident.*rv\:11\./);
    };

    updateCurrentUrl = (queryString: string) => {
        try {
            window.top.indexManager.updateCurrentUrl(queryString);
        } catch (e) {
        }
    };

    showFeedback = (id: number) => {
        setTimeout(function () {
            window.top.indexManager && window.top.indexManager.initFeedbackFunction(id);
        }, 1000);
    };
}

const alphaTransfer = new AlphaTransfer($, store);
