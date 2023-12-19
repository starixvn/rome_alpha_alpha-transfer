"use strict";
interface JQuery {
    pagination(option: any): void;
}

class PagerOption {
    constructor(
        public onSelectPage: () => void = null,
        public pagerId: string = "report-pager",
        public formId: string = "report-form",
        public pageIndexId: string = "PageIndex",
        public pageSizeId: string = "PageSize") {
    }
}

class PagerComponent {
    $: JQueryStatic;
    $pageIndex: JQuery;
    $pageSize: JQuery;
    $reportForm: JQuery;
    $pager: JQuery;

    totalRecords: number;
    pageSize: number;
    pageIndex: number;

    pageList: number[] = [100, 200, 500, 1000];
    // pageList: number[] = [2, 5, 10, 20, 500];

    constructor($: JQueryStatic) {
        this.$ = $;
    }

    render = (options: PagerOption = new PagerOption()) => {
        let self = this;
        let pagerOption = <PagerOption>this.$.extend({}, this.options, options);
        let pager = this.$("#" + pagerOption.pagerId);

        this.$reportForm = this.$("#" + this.options.formId);
        this.totalRecords = pager.data("totalrecords");
        this.pageSize = pager.data("pagesize");
        this.pageIndex = pager.data("pageindex");
        this.$pageIndex =
            this.$(`<input type="hidden" id="${pagerOption.pageIndexId}" name="${pagerOption.pageIndexId}" value="${this.pageIndex}" />`);
        this.$pageSize =
            this.$(`<input type="hidden" id="${pagerOption.pageSizeId}" name="${pagerOption.pageSizeId}" value="${this.pageSize}" />`);

        this.$reportForm.append(this.$pageIndex).append(this.$pageSize);

        if (this.isShow()) {
            pager.pagination({
                pageList: this.pageList,
                total: this.totalRecords,
                pageSize: this.pageSize,
                pageNumber: this.pageIndex,

                showRefresh: false,
                onSelectPage: function (pageNumber: number, pageSize: number) {
                    self.updateHiddenFields(pageNumber, pageSize);

                    if (options.onSelectPage) {
                        options.onSelectPage();
                    } else {
                        self.onChange();
                    }
                },
                beforePageText: pager.data("lablepage"),
                afterPageText: pager.data("labelof") + " {pages}",
                displayMsg: pager.data("labeldisplayitems")
            });

            pager.removeClass("hide");
        } else {
            pager.addClass("hide");
        }

        this.$pager = pager;

        return pager;
    };

    onChange = () => {
        this.$reportForm.trigger("submit");
    };

    updateHiddenFields = (pageNumber: number, pageSize: number) => {
        this.$pageIndex.val(pageNumber);
        this.$pageSize.val(pageSize);
    };

    isShow = () => {
        // total records less than min page size => do not need to show pager
        return this.pageList[0] <= this.totalRecords;
    };

    hasAllDataOnClient = () => {
        var result = this.totalRecords <= this.pageSize;
        return result;
    };

    options = new PagerOption(this.onChange);

    reset = () => {
        if (this.$pageIndex) {
            this.$pageIndex.remove();
            this.$pageSize.remove();
        }
    };
}