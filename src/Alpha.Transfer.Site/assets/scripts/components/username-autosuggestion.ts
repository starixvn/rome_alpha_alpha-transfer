'use strict';
interface UsernameAutoSuggestionOption {
    url: string,
    directDownlines?: boolean,
    isAccInfo?: boolean,
    onBlur?: Function,
    selectorId?: string
}

class UsernameAutoSuggestionComponent {
    private usernameInput: JQuery;

    init(options: UsernameAutoSuggestionOption) {
        this.usernameInput = $('#' + (options.selectorId || 'txt-username'));
        if (!this.usernameInput.length) {
            return;
        }

        const queryString = [];
        if (options.directDownlines) {
            queryString.push('directdownlines=' + options.directDownlines);
        }
        if (typeof options.isAccInfo === 'boolean' && !options.isAccInfo) {
            queryString.push('isaccinfo=false');
        }
        const urlAndQueryString = `${options.url}?${queryString.join('&')}${queryString.length ? '&' : ''}query=`;

        const typeaheadOptions = {
            source: function (query: string, process) {
                return $.get(`${urlAndQueryString}${query}`, process);
            },
            items: 10,
            minLength: 2,
            highlighter: function (displayText: string) {
                const parts = displayText.split('#');
                return `${parts[0]} <br/> ${parts[1]}`;
            },
            displayText: function (item) {
                const nickname = item.Username2 ? ` (${item.Username2})` : '';
                return `${item.Username}${nickname}#${item.FirstName} ${item.LastName}`;
            },
            autoSelect: false,
            afterSelect: (item) => this.usernameInput.val(item.Username),
            delay: 200
        };

        let hitEnterTextboxUsername = false;
        this.usernameInput.typeahead(typeaheadOptions);
        this.usernameInput.keypress((e) => {
            const key = e.which || e.keyCode;
            if (key === 13) {
                hitEnterTextboxUsername = true;
                options.onBlur && options.onBlur();
            }
        }).blur(() => {
            if (hitEnterTextboxUsername) {
                hitEnterTextboxUsername = false;
            } else {
                const visible = this.usernameInput.next('.typeahead').is(':visible');
                !visible && options.onBlur && options.onBlur();
            }
        });
    }

    getUsername() {
        if (this.usernameInput.length) {
            return $.trim(this.usernameInput.val());
        }
    }

    resetUsername() {
        this.usernameInput.val('');
    }
}
