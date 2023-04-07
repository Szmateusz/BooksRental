
function remind(id) {
    $.ajax({
        url: '/Admin/Remind',
        type: 'POST',
        data: { rentalId: id},
        success: function (data) {
            if (data.success) {

                showSuccessMessage(data.result, "success");
            } else {
                showSuccessMessage(data.result, "failed");

            }
        }
    });
}
 

function showSuccessMessage(text,type) {
    // Pokaż diva z komunikatem

    $('#message').addClass(type);
    $('#message').html(text);
    $('#message').css('display', 'block').fadeIn();


    // Ukryj diva po czasie
    setTimeout(function () {
        $('#message').fadeOut();
    $('#message').removeClass(type);

    }, 3000);
}