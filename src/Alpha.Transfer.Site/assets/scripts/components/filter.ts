'use strict';

interface FilterOption {
    hideText: string,
    showText: string,
    iconFilterId?: string,
    formId?: string,
    callback?: Function
}

class FilterComponent {
    initShowHide(options: FilterOption) {
        $('#' + (options.iconFilterId || 'icon-filter')).click(function (e) {
            const $iconFilter = $(e.target);
            const isHideFilter = $iconFilter.hasClass('ic-arrow-up-circle');
            $('#' + (options.formId || 'transfer-form')).toggle(!isHideFilter);
            if (isHideFilter) {
                $iconFilter
                    .removeClass('ic-arrow-up-circle')
                    .addClass('ic-arrow-down-circle')
                    .attr('data-original-title', options.showText);
            } else {
                $iconFilter
                    .removeClass('ic-arrow-down-circle')
                    .addClass('ic-arrow-up-circle')
                    .attr('data-original-title', options.hideText);
            }

            $iconFilter.tooltip("show");
            options.callback && options.callback(isHideFilter);
        }).tooltip();
    }
}