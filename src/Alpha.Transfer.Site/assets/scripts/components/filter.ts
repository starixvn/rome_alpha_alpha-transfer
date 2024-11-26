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
            const isHideFilter = $iconFilter.hasClass('icon-arrow-up-drop-circle');
            $('#' + (options.formId || 'transfer-form')).toggle(!isHideFilter);
            if (isHideFilter) {
                $iconFilter
                    .removeClass('icon-arrow-up-drop-circle')
                    .addClass('icon-arrow-down-drop-circle')
                    .attr('data-original-title', options.showText);
            } else {
                $iconFilter
                    .removeClass('icon-arrow-down-drop-circle')
                    .addClass('icon-arrow-up-drop-circle')
                    .attr('data-original-title', options.hideText);
            }

            $iconFilter.tooltip("show");
            options.callback && options.callback(isHideFilter);
        }).tooltip();
    }
}