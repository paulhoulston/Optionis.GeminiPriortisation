﻿var Dashboard = {
    formatString: function (str, args) {
        args = typeof args === 'object' ? args : Array.prototype.slice.call(arguments, 1);
        return str.replace(/\{([^}]+)\}/gm, function () {
            return args[arguments[1]];
        });
    },

    bindToTemplate: function (opts) {
        var source = $(opts.templateSelector).html(),
                template = Handlebars.compile(source);
        return $(opts.destinationSelector).empty().html(template(opts.data));
    },

    bindToTemplateWithUrl: function (opts) {
        $.get(Dashboard.formatString(opts.uri, opts.sitePath), function (data) {
            //console.log(JSON.stringify(data));

            Dashboard.bindToTemplate({ data: data, templateSelector: opts.template, destinationSelector: opts.selector });
        });
        if (opts.onComplete) {
            opts.onComplete();
        }
    },

    init: function () {

        var sitePath;

        function getWorkHistoryForUser() {
            var tabIndex = $('#timesheetsTabs').tabs('option', 'active'),
                userId = $($('#timesheetsTabs').tabs().find('li')[tabIndex]).attr('data-gemini-user-id'),
                selectedDate = $('#availableDates').val();

            if (userId) {
                Dashboard.bindToTemplateWithUrl({
                    uri: Dashboard.formatString('{0}/loggedtimes/{1}/{2}', '{0}', userId, selectedDate),
                    selector: '#history',
                    template: '#history-template',
                    sitePath: sitePath,
                    onComplete: function () {
                        Dashboard.bindToTemplateWithUrl({ uri: Dashboard.formatString('{0}/issues/{1}', '{0}', userId), selector: '#assignedTasks div', template: '#issues-template', sitePath: sitePath });
                    }
                });
            }
        }

        function getMetrics() {
            var el = $('#metricsTabs'),
                tabIndex = el.tabs('option', 'active'),
                templateId = $(el.tabs().find('li a')[tabIndex]).attr('data-template');

            Dashboard.bindToTemplateWithUrl({
                uri: Dashboard.formatString('{0}/issues/reopened/{1}', '{0}', '2016-02-10'),
                selector: '#metric',
                template: templateId,
                sitePath: sitePath
            });
        }

        function createTabs(people) {

            function onPermissableDatesBound() {
                $('#dates').on('change', getWorkHistoryForUser);
            }

            Dashboard.bindToTemplate({
                templateSelector: '#tabs-template',
                data: people,
                destinationSelector: '#tabs'
            }).tabs({ activate: getWorkHistoryForUser });

            $('#timesheetsTabs').tabs({ activate: getWorkHistoryForUser });

            Dashboard.bindToTemplateWithUrl({ uri: '{0}/priorities', selector: '#backlog', template: '#issues-template', sitePath: sitePath });
            Dashboard.bindToTemplateWithUrl({ uri: '{0}/bautasks', selector: '#bau', template: '#enhancements-template', sitePath: sitePath });
            Dashboard.bindToTemplateWithUrl({ uri: '{0}/applicationenhancements', selector: '#enhancements', template: '#enhancements-template', sitePath: sitePath });
            Dashboard.bindToTemplateWithUrl({ uri: '{0}/permissibledates', selector: '#dates', template: '#availabledates-template', sitePath: sitePath, onComplete: onPermissableDatesBound });
            Dashboard.bindToTemplateWithUrl({ uri: '{0}/metrics', selector: '#metrics', template: '#metrics-template', sitePath: sitePath });
        }

        function getPath() {
            var index = window.location.pathname.lastIndexOf('/');
            return index >= 0 ?
                window.location.pathname.substring(0, index) :
                window.location.pathname;
        }

        sitePath = getPath();
        $.getJSON(Dashboard.formatString('{0}/people', sitePath), createTabs);
        Dashboard.bindToTemplateWithUrl({ uri: '{0}/siteversion', selector: 'footer', template: '#footer-template', sitePath: sitePath });
    }
};

// Format the date
Handlebars.registerHelper("formatDate", function (datetime) {
    var parsedDate = new Date(datetime);
    return Dashboard.formatString('{0}/{1}/{2}', parsedDate.getDay(), parsedDate.getMonth(), parsedDate.getFullYear());
});