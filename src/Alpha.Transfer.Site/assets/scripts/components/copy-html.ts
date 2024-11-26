'use strict';
declare var Clipboard: any;

interface CopyHtmlOption {
    copyText: string
    copiedText: string,
    iconCopySelectorId?: string,
    tableSelectorId?: string
}

class CopyHtmlComponent {
    private clipboard: any;

    constructor(clipboard: any) {
        this.clipboard = clipboard;
    }

    init(options: CopyHtmlOption) {
        options = $.extend({
            iconCopySelectorId: 'icon-copy',
            tableSelectorId: 'tbl-transfer'
        }, options);

        $('#' + options.iconCopySelectorId).tooltip();
        const clipboard = new this.clipboard('#' + options.iconCopySelectorId, {
            text: () => this._copyTableHtml($('#' + options.tableSelectorId))
        });

        if (window.document.queryCommandSupported && window.document.queryCommandSupported('copy')) {
            clipboard.on('success', function (e) {
                const $this = $(e.trigger);
                $this.attr('data-original-title', options.copiedText);
                $this.tooltip('show');
                setTimeout(() => {
                    $this.tooltip('hide');
                    $this.attr('data-original-title', options.copyText);
                }, 3000);
            }).on('error', function (e) {
                $(e.trigger).remove();
                console.error('Action:', e.action);
                console.error('Trigger:', e.trigger);
            });
        }
    }

    _copyTableHtml(tableReport: JQuery) {
        const html = ['<table>'];
        html.push(this._copyTableHeader(tableReport));
        html.push(this._copyTableBody(tableReport));
        html.push(this._copyTableFooter(tableReport));
        html.push('</table>');

        return html.join('');
    }

    _copyTableHeader(tableReport: JQuery) {
        const html = ['<thead><tr>'];
        tableReport.find('thead tr:first th:visible').each(function () {
            const $this = $(this);
            html.push('<th');

            const rowspan = $this.attr('rowspan');
            if (rowspan) {
                html.push(' rowspan="', rowspan, '"');
            }

            const colspan = $this.attr('colspan');
            if (colspan) {
                html.push(' colspan="', colspan, '"');
            }

            html.push('>', this.innerText, '</th>');
        });
        html.push('</tr>');

        const theadRow2s = tableReport.find('thead tr.thead-row2');
        if (theadRow2s.length) {
            html.push('<tr>');
            theadRow2s.find('th:visible').each(function () {
                html.push('<th>', this.innerText, '</th>');
            });
            html.push('</tr>');
        }

        html.push('</thead>');
        return html.join('');
    }

    _copyTableBody(tableReport: JQuery) {
        const html = ['<tbody>'];

        tableReport.find('tbody tr').each(function () {
            html.push('<tr>');
            $(this).find('td:visible').each(function () {
                html.push('<td');

                const colspan = $(this).attr('colspan');
                if (colspan) {
                    html.push(' colspan="', colspan, '"');
                }

                html.push('>', this.innerText, '</td>');
            });
            html.push('</tr>');
        });

        html.push('</tbody>');
        return html.join('');
    }

    _copyTableFooter(tableReport: JQuery) {
        if (tableReport.find('tfoot').length) {
            const html = ['<tfoot><tr>'];
            tableReport.find('tfoot tr td:visible').each(function () {
                html.push('<td>', this.innerText, '</td>');
            });

            html.push('</tr></tfoot>');
            return html.join('');
        }

        return '';
    }
}
