$(document).ready(function () {   
    $("#showUserDetailsButton").click(function () {
        $("#userDetailsSection").toggle(); // Показать или скрыть секцию при нажатии
    });
});

$('#getOrdersBut').click(function () {
    // Переключаем видимость спиннера
    $(this).find('.spinner-border').removeClass('d-none');

    // Блокируем кнопку
    $(this).prop('disabled', true);

    var selectedDate = $('#dateInput').val();

    $.ajax({
        url: '/User/GetOrders', // Укажите URL вашего метода контроллера для получения списка заказов
        type: 'GET',
        data: { date: selectedDate },
        success: function (data) {
            $('#getOrdersBut').find('.spinner-border').addClass('d-none');
            $('#getOrdersBut').prop('disabled', false);
            console.log("getOrdersBut success");
            // После успешного получения данных от сервера, обновите таблицу заказов
            $('#ordersTable').html(data);
        },
        error: function () {
            $('#getOrdersBut').find('.spinner-border').addClass('d-none');
            $('#getOrdersBut').prop('disabled', false);
            alert('Произошла ошибка при загрузке заказов.');
        }
    });
});

$("#editInfoForm").submit(function (e) {
    e.preventDefault();

    $("#UsernameError").text("");
    $("#EmailError").text("");
    $("#TelephoneError").text("");

    var name = $("#nameInputLK").val();
    var email = $("#emailInputLK").val();
    var phone = $("#phoneInputLK").val();

    $.ajax({
        url: "/User/EditInfo",
        type: "POST",
        data: { Name: name, Email: email, Phone: phone },
        success: function (response) {
            if (response && response.errors) {
                // Отображаем сообщения об ошибках валидации
                for (var key in response.errors) {
                    $("#" + key + "Error").text(response.errors[key][0]).show();
                }
            } else {
                Swal.fire({
                    position: "top",
                    icon: "success",
                    title: "Информация успешно обновлена",
                    showConfirmButton: false,
                    timer: 3000,
                    timerProgressBar: true
                }).then((result) => {
                    window.location.href = "/User/Index";
                });
            }
        },
        error: function () {
            Swal.fire({
                icon: "error",
                title: "Ошибка",
                text: "Произошла ошибка при обновлении информации. Пожалуйста, попробуйте снова."
            });
        }
    });
});
