interface SortOption {
    tableElement: JQuery,
    mapSortColumnIndexHandler?: Function,
    sortEndCallback?: Function
}

class SortComponent {
    public sortColumnName: string;
    public sortColumnIndex: number;
    public sortIndicator = 0; // asc: 0, desc: 1

    public tableSorter(options: SortOption) {
        const canSort = options.tableElement.data('cansort').toLowerCase() === 'true';
        const sortColumns = options.tableElement.find('.sorting');

        if (canSort) {
            const headers = {};
            sortColumns.each(function () {
                // sorttype is text or digit
                const $this = $(this);
                headers[$this.data('colindex')] = { sorter: $this.data('sorttype') || 'text' };
            });
            options.tableElement.find('.disabledsort').each(function () {
                headers[$(this).data('colindex')] = { sorter: false };
            });

            if (!options.mapSortColumnIndexHandler) {
                options.mapSortColumnIndexHandler = () => this.sortColumnIndex;
            }
            const sortList = typeof this.sortColumnIndex === 'undefined'
                ? []
                : [[options.mapSortColumnIndexHandler(), this.sortIndicator]];

            options.tableElement.tablesorter({
                headers: headers,
                sortList: sortList,
                textExtraction: this.textExtraction
            }).bind('sortEnd', () => {
                const sortedColumns = options.tableElement.find('.sorting-asc, .sorting-desc');
                if (sortedColumns.length) {
                    const firstSortedColumn = sortedColumns.eq(0);
                    this.sortColumnName = firstSortedColumn.data('colname');
                    this.sortColumnIndex = firstSortedColumn.data('colindex');
                    this.sortIndicator = firstSortedColumn.hasClass('sorting-desc') ? 1 : 0; // asc: 0, desc: 1
                }

                options.sortEndCallback && options.sortEndCallback();
            });
        } else {
            sortColumns.removeClass('sorting');
        }
    }

    private textExtraction(node: any) {
        const $node = $(node);
        const sortedValue = $node.data('sortedvalue');

        return sortedValue ? sortedValue : $node.html();
    }
}
