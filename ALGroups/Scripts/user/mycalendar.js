$(document).ready(function () {
    $('#calendar').fullCalendar("destroy");

    $('#calendar').fullCalendar({
        defaultDate: new Date(),
        header: {
            left: 'prev,next today',
            center: 'title',
            right: 'month,basicWeek,basicDay,agenda'
        },
        buttonText: {
            today: 'today',
            month: 'month',
            week: 'week',
            day: 'day'
        },
        eventColor: "#378006",
        events: function (start, end, tz, callback) {
            var pathname = window.location.pathname;
            var groupId = pathname.match(/\d+/)[0];
            fetchEvents(groupId, callback);
        }
    })
})

var fetchEvents = function (groupId, callback) {
    $.ajax({
        type: "GET",
        url: "/Activity/List?groupId=" + groupId,
        dataType: "JSON",
        success: function (response) {
            var events = [];
            $.each(response, function (i, val) {
                events.push(toEvent(val))
            })
            callback(events);
        }
    })
}

var toEvent = function (val) {
    return {
        title: val.Subject,
        description: val.Description,
        start: moment(val.Start),
        end: val.End == null ? null : moment(val.End),
        allDay: val.IsFullDay
    }
}