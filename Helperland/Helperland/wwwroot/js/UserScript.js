$(document).ready(function () {
    // this will get the full URL at the address bar
    var url = window.location.href;
    // hightlight active link
    $(".side-bar-menu li a").each(function () {
        // checks if its the same on the address bar
        if (url == (this.href)) {
            $(this).addClass("active");
        }
    });
});

function ServiceStartDate(inputDate) {
    const date = new Date(inputDate);
    return AppendZero(date.getDate().toString()) + "/" + AppendZero((date.getMonth() + 1).toString()) + "/" + date.getFullYear().toString();
}

function ServiceTime(inputDate, totalHour) {
    const date = new Date(inputDate);
    date.setMinutes(date.getMinutes() + (totalHour * 60));
    return AppendZero(date.getHours().toString()) + ":" + AppendZero(date.getMinutes().toString());
}

//appent 0 to single digit number for month and date
function AppendZero(input) {
    if (input.length == 1) {
        return '0' + input;
    }
    return input;
}


//check valid date from seperate day, month & year
function isValidDate(year, month, day) {
    var d = new Date(year + "-" + AppendZero(month) + "-" + AppendZero(day));
    if (d.getFullYear().toString() == year && (d.getMonth() + 1).toString() == month && d.getDate().toString() == day) {
        return true;
    }
    return false;
}