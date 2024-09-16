$(document).ready(function () {
    $('#sendResetLink').click(function () {
        // Показать overlay
        $('#overlay').removeClass('d-none');
        // Переключаем видимость спиннера
        $(this).find('.spinner-border').removeClass('d-none');
        // Блокируем кнопку
        $(this).prop('disabled', true);

        var email = $('#email').val();
        $.ajax({
            url: '/Login/ForgotPassword',
            type: 'POST',
            data: { email: email },
            success: function (data) {
                // Скрыть overlay
                $('#overlay').addClass('d-none');
                $('#sendResetLink').find('.spinner-border').addClass('d-none');
                $('#sendResetLink').prop('disabled', false);
                alert("Ссылка для сброса пароля была отправлена на ваш email.");
                $('#forgotPasswordModal').modal('hide');
            },
            error: function (error) {
                $('#sendResetLink').find('.spinner-border').addClass('d-none');
                $('#sendResetLink').prop('disabled', false);
                alert("Ошибка при отправке ссылки для сброса пароля.");
            }
        });
    });
});