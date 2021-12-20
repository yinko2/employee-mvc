var calendarEl = document.getElementById('calendar');
let calendar = new FullCalendar.Calendar(calendarEl, {
    initialView: 'dayGridMonth',
    headerToolbar: {
        left: 'prev,next today',
        center: 'title',
        right: ''
    },
    events: '/Home/GetHolidays',
    selectable: true,
    select: showLeaves,
    eventColor: 'yellow',
    firstDay: 1
});

calendar.render();

function showLeaves() {

    $('#eventModalLabel').html('Leaves');

    $('#eventModal').modal('show');
}

