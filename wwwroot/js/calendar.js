document.addEventListener('DOMContentLoaded', function() {
    var calendarEl = document.getElementById('calendar');
    var calendar = new FullCalendar.Calendar(calendarEl, {
        initialView: 'dayGridMonth',
        events: '/Reservation/GetEvents',
        eventClick: function(info) {
            var reservationTime = new Date(info.event.start);
            var formattedTime = reservationTime.getHours().toString().padStart(2, '0') + ':' + reservationTime.getMinutes().toString().padStart(2, '0');
            document.getElementById('reservationTime').value = formattedTime;
            var modal = new bootstrap.Modal(document.getElementById('reservationModal'));
            modal.show();
        }
    });
    calendar.render();
});