"use strict";

interface VisibleColumnDropdownNewOption {
    listColumn: any,
    onDropdownHidden?: Function,
    selectorId?: string,
    minWidth?: number,
    localStorageKey?: string,
    visibleColumns?: string
}

class VisibleColumnDropdownNewComponent {
    private dropdownColumn: JQuery;

    render(options: VisibleColumnDropdownNewOption) {
        let isCheckAll = false;
        const dropdown = $(`#${options.selectorId || "dropdown-column"}`);

        const visibleColumnObject = {};
        let visibleColumns = options.localStorageKey
            ? alphaTransfer.localStorage.get(options.localStorageKey)
            : options.visibleColumns;
        if (visibleColumns) {
            const visibleColumnArray = visibleColumns.split(",");
            visibleColumnArray.forEach(function (item) {
                visibleColumnObject[item] = true;
            });
            isCheckAll = options.listColumn.length === visibleColumnArray.length;
        } else {
            options.listColumn.forEach(function (item) {
                if (item.checked !== false) {
                    visibleColumnObject[item.value] = true;
                }
            });
        }

        this.dropdownColumn = dropdown.fanexMultiSelect({
            selectAllText: dropdown.data("label-selectall"),
            selectedText: "# " + dropdown.data("label-selected"),
            noneSelectedText: dropdown.data("label-noneselected"),
            customSelectAllText: dropdown.data("label-all") + " (#)",
            showFilter: true,
            filterPlaceHolder: dropdown.data("label-filterplaceholder"),
            noItemFoundText: dropdown.data("label-noitemfound"),
            isCheckAll: isCheckAll,
            minWidth: options.minWidth || 220,
            maxHeight: 250,
            showTooltip: false,
            listData: options.listColumn.map(function (item) {
                return {
                    value: item.value,
                    text: item.text,
                    checked: isCheckAll || visibleColumnObject.hasOwnProperty(item.value)
                };
            }),
            onLeave: () => {
                const column = this.getColumns();
                options.onDropdownHidden(column);

                if (options.localStorageKey) {
                    const selectedColumnsValue = [];
                    Object.entries(column).forEach(([key, value]) => value && selectedColumnsValue.push(key));
                    alphaTransfer.localStorage.set(options.localStorageKey, selectedColumnsValue.join(','))
                }
            }
        });
    }

    getColumns() {
        return this.dropdownColumn.fanexMultiSelect("getAllValues");
    }
}
