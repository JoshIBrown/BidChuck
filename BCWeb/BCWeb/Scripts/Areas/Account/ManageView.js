$(function () {
    $('.close').click(function () {
        $(this).parent().remove();
    });

    AddAntiForgeryToken = function (data) {
        data.__RequestVerificationToken = $('input[name=__RequestVerificationToken]').val();
        return data;
    };

    $('.resendInviteLink').click(function () {
        var email = $(this).parent().parent().find('.email').text();
        var token = $('input[name=__RequestVerificationToken]').val();
        $.post('/Account/Users/ResendInvitation', { email: email, __RequestVerificationToken: token })
            .success(function (data) {
                var alertbox = $('#alerts');
                alertbox.html('<div class="alert-box success">Invitation succefully resent to ' + email + '<a href="#" class="close">&times;</a></div>');
                $('.close').click(function () {
                    $(this).parent().remove();
                });
            });;
    });
});