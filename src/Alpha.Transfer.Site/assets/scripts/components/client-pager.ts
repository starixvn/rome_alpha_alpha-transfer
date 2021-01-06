"use strict";

interface JQuery {
    pagination(option: any): void;
}

class ClientPagerOption {
    constructor(
        public pagerId: string = "report-pager",
        public formId: string = "filter-form",
        public pageIndexId: string = "Page",
        public pageSizeId: string = "PageSize",
        public onSelectPage: Function = null) {
    }
}

class ClientPagerComponent {
    $: JQueryStatic;
    $pageIndex: JQuery;
    $pageSize: JQuery;
    $reportForm: JQuery;

    totalRecords: number;
    pageSize: number;
    pageIndex: number;

    pageList: number[] = [10, 50, 100, 500];

    options = new ClientPagerOption();

    constructor($: JQueryStatic) {
        this.$ = $;
    }

    render = (options: ClientPagerOption = new ClientPagerOption()) => {
        let pagerOption = <ClientPagerOption>this.$.extend({}, this.options, options);
        let pager = this.$("#" + pagerOption.pagerId);

        this.$reportForm = this.$("#" + pagerOption.formId);

        this.totalRecords = parseInt(pager.data("totalrecords"), 0);
        this.pageSize = parseInt(pager.data("pagesize"), 0);
        this.pageIndex = parseInt(pager.data("pageindex"), 0);
        this.$pageIndex =
            this.$(`<input type="hidden" id="${pagerOption.pageIndexId}" name="${pagerOption.pageIndexId}" value="${this.pageIndex}" />`);
        this.$pageSize =
            this.$(`<input type="hidden" id="${pagerOption.pageSizeId}" name="${pagerOption.pageSizeId}" value="${this.pageSize}" />`);

        this.$reportForm.append(this.$pageIndex).append(this.$pageSize);

        pager.pagination({
            pageList: this.pageList,
            total: this.totalRecords,
            pageSize: this.pageSize,
            pageNumber: this.pageIndex,

            showRefresh: false,
            onSelectPage: (pageNumber: number, pageSize: number) => {
                if (pagerOption.onSelectPage) {
                    pagerOption.onSelectPage(pageNumber, pageSize);
                } else {
                    this.onSelectPage(pageNumber, pageSize);
                }
            },
            beforePageText: pager.data("lablepage"),
            afterPageText: pager.data("labelof") + " {pages}",
            displayMsg: pager.data("labeldisplayitems")
        });

        pager.removeClass("hide");

        return pager;
    };

    onSelectPage = (pageNumber: number, pageSize: number) => {
        this.$pageIndex.val(pageNumber);
        this.$pageSize.val(pageSize);

        this.$reportForm.submit();
    };
}