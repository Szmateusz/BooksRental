function newestArrow(direction) {

    var div = document.getElementById("newestBooks");

    if (direction == 'left') {
        div.scrollLeft += 400;

    } else {
        div.scrollLeft -= 400;

    }
}

function recommendArrow(direction) {

    var div = document.getElementById("recommendBooks");

    if (direction == 'left') {
        div.scrollLeft += 400;

    } else {
        div.scrollLeft -= 400;

    }
}

