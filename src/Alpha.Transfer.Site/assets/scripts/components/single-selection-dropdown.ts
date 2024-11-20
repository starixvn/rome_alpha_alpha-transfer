"use strict";
interface JQuery {
    multiselect(option: object): void;
}

class SingleSelectionDropdownOption {
    constructor(
        public dropdownId = "dropdown-single",
        public inputId = "UserSelectedId",
        public onChange: Function = null,
        public enableFiltering = false,
        public enableCaseInsensitiveFiltering = true) {
    }
}

class SingleSelectionDropdownComponent {
    $: JQueryStatic;
    $hiddenInput: JQuery;
    $dropdown: JQuery;
    $dropdownButton: JQuery;

    selectedValue: string;

    options = new SingleSelectionDropdownOption();

    constructor($: JQueryStatic) {
        this.$ = $;
    }

    render = (options: SingleSelectionDropdownOption = new SingleSelectionDropdownOption()) => {
        this.options = this.$.extend({}, this.options, options) as SingleSelectionDropdownOption;

        const $dropdown = this.$("#" + this.options.dropdownId);
        this.$dropdown = $dropdown;

        const $hiddenInput = this.$("#" + this.options.inputId);
        this.$hiddenInput = $hiddenInput;

        this.selectedValue = this.getSelectedItem();

        this.$dropdown.multiselect({
            multiple: false,
            maxHeight: 300,
            numberDisplayed: 1,
            includeSelectAllOption: false,
            onChange: this.onchange,
            enableFiltering: this.options.enableFiltering,
            enableCaseInsensitiveFiltering: this.options.enableFiltering && this.options.enableCaseInsensitiveFiltering,
            filterPlaceholder: $dropdown.data("label-search-text"),
        });
    };
    getSelectedItem = () => {
        const selected = [];
        const selectedOptions = this.$dropdown.find("option:selected");
        const this$ = this.$;

        selectedOptions.each(function () {
            selected.push([this$(this).val()]);
        });

        return selected.length ? selected[0].toString() : "";
    };
    updateSelectedValue = () => {
        this.$hiddenInput.val(this.selectedValue);
    };

    onchange = () => {
        this.selectedValue = this.getSelectedItem();
        if (this.options.onChange) {
            this.options.onChange();
        }
    };
}
