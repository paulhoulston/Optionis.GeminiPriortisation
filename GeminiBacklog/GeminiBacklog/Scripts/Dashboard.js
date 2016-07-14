Dashboard = {
    init: function () {

        var sitePath;

        function formatString(str, args) {
            args = typeof args === 'object' ? args : Array.prototype.slice.call(arguments, 1);
            return str.replace(/\{([^}]+)\}/gm, function () {
                return args[arguments[1]];
            });
        }

        function bindToTemplate(opts) {
            var source = $(opts.templateSelector).html(),
                    template = Handlebars.compile(source);
            return $(opts.destinationSelector).html(template(opts.data));
        }

        function getIssues(uri, selector) {
            $.get(formatString(uri, sitePath), function (issues) {
                bindToTemplate({ data: issues, templateSelector: '#issues-template', destinationSelector: selector });
            });
        }
        
        function getWorkHistoryForUser() {
            var tabIndex = $('#timesheetsTabs').tabs('option', 'active'),
                userId = $($('#timesheetsTabs').tabs().find('li')[tabIndex]).attr('data-gemini-user-id'),
                selectedDate = $('#availableDates').val(),
                url = formatString('{0}/loggedtimes/{1}/{2}', sitePath, userId, selectedDate);

            if (userId) {
                $.get(url, function (data) {
                    bindToTemplate({
                        data: data,
                        templateSelector: '#history-template',
                        destinationSelector: '#history'
                    });

                    $.get(formatString('{0}/issues/{1}', sitePath, userId), function (data) {
                        bindToTemplate({
                            data: data,
                            templateSelector: '#issues-template',
                            destinationSelector: '#assignedTasks div'
                        });
                    });
                });
            }
        }

        function bindAvailableDates() {
            $.get(formatString('{0}/permissibledates', sitePath), function (dates) {
                bindToTemplate({
                    data: dates,
                    templateSelector: '#availabledates-template',
                    destinationSelector: '#dates'
                }).on('change', getWorkHistoryForUser);
            });
        }

        function createTabs(people) {
            bindToTemplate({
                templateSelector: '#tabs-template',
                data: people,
                destinationSelector: '#tabs'
            }).tabs({ activate: getWorkHistoryForUser });

            $('#timesheetsTabs').tabs({ activate: getWorkHistoryForUser });

            getIssues('{0}/priorities', '#backlog');
            getIssues('{0}/bautasks', '#bau');
            getIssues('{0}/applicationenhancements', '#enhancements');
            bindAvailableDates();
        }

        function getPath() {
            var index = window.location.pathname.lastIndexOf('/');
            return index >= 0 ?
                window.location.pathname.substring(0, index) :
                window.location.pathname;
        }

        sitePath = getPath();
        $.getJSON(formatString('{0}/people', sitePath), createTabs);
    }
};