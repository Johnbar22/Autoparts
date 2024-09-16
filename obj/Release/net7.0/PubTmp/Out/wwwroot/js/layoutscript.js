
$(document).ready(function () {
    console.log("Start ");
    // Получаем сохраненное количество позиций в корзине из localStorage
    var savedCartCount = sessionStorage.getItem("cartItemCount");
    if (savedCartCount != null && savedCartCount != 0) {
        // Обновляем отображение количества позиций в корзине
        $(".cart-count").show();
        $(".cart-count").text(savedCartCount);
    }
    else {
        var itemsInCart = $(".cart-items").length;
        if (itemsInCart > 0) {
            // Если товары есть, устанавливаем cartItemCount равным количеству товаров
            savedCartCount = itemsInCart;
            sessionStorage.setItem("cartItemCount", savedCartCount);
            $(".cart-count").show();
            $(".cart-count").text(savedCartCount);
        }
    }
});

$('#loginButton').click(function (e) {
    e.preventDefault();
    $(".overlay").show();
    $(".spinner").show();
    $("#LoginError").text(""); // Очистка сообщения об ошибке логина
    $("#PasswordError").text(""); // Очистка сообщения об ошибке логина
    var token = $('input[name="__RequestVerificationToken"]').val();
    var login = $("#Login").val();
    var password = $("#Password").val();    
   
    // Отправляем запрос на сервер для выхода из учетной записи
    $.ajax({
        type: 'POST',
        url: '/Login/Login',
        data: {
            Login: login,
            Password: password
        },
        headers: {
            RequestVerificationToken: token
        },
        success: function (response) {         
            $(".overlay").hide();
            $(".spinner").hide();
             //Обработка успешного входа в учетную запись
            // Проверяем наличие сообщений об ошибках валидации
            if (response && response.errors) {
                $(".text-danger").text("");
                let errorMessage = "";
                for (var key in response.errors) {
                    if (!login || !password || key) {
                        $("#" + key + "Error").text(response.errors[key]);                        
                    }
                    else {
                        errorMessage = "\n" + response.errors[key].join("\n");
                        Swal.fire({
                            icon: "error",
                            title: "Ошибка",
                            text: errorMessage,
                        });
                    }
                    console.log("Ошибка " + key + " текст ошибки: " + response.errors[key]);
                }
            }
            else {
                Swal.fire({
                    position: "top",
                    icon: "info",
                    title: "Успешный вход.",
                    showConfirmButton: false,
                    timer: 3500
                }).then((result) => { 
                    if (response.userRoleId == 1) {
                        window.location.href = '/Order/Index';
                    }
                    else {
                        window.location.href = '/Home/Index';
                    }
                });
            }
        },
        error: function (error) {
            $(".overlay").hide();
            $(".spinner").hide();
            // Обработка ошибки
            Swal.fire({
                icon: "error",
                title: "Oops...",
                text: "Каки-то неполадки!",
                footer: 'смотри логи'
            }).then((result) => {
                console.log("Ошибка при входе в учетную запись: " + login + error)
            });
        }
    });
});

$('#linkExit').click(function (e) {
    e.preventDefault();
    var token = $('input[name="__RequestVerificationToken"]').val();
    // Отправляем запрос на сервер для выхода из учетной записи
    $.ajax({
        type: 'POST',
        url: '/Login/Logout',
        headers: {
            RequestVerificationToken: token
        },
        success: function (response) {
            // Обработка успешного выхода из учетной записи
            Swal.fire({
                position: "top",
                icon: "info",
                title: "Вы вышли из учетной записи",
                showConfirmButton: false,
                timer: 2500
            }).then((result) => {
                window.location.href = '/Home/Index';
            });
        },
        error: function (error) {
            // Обработка ошибки
            Swal.fire({
                icon: "error",
                title: "Oops...",
                text: "Каки-то неполадки!",
                footer: 'смотри логи'
            }).then((result) => {
                console.log("Ошибка при выходе из учетной записи: " + error)
            });
        }
    });
});

function updateCartCount(count) {
    console.log("Сработал updateCartCount. count = " + count);
    if (count == 0 || count == null) {
         console.log("Сработало условие if(count == 0 || count == null) в updateCartCount");
        $(".cart-count").hide();
        sessionStorage.setItem("cartItemCount", count);
    }
    else {
        console.log("Сработало else в updateCartCount. count = " + count);
        $(".cart-count").show();
        $(".cart-count").text(count);
    }
    sessionStorage.setItem("cartItemCount", count);
    console.log("Обновляем отображение количества позиций в корзине  (.cart-count).text(count) " + count);
}

$("#registration").click(function (e) {
    e.preventDefault();
    $("#UsernameError").text(""); // Очистка сообщения об ошибке для фио
    $("#TelephoneError").text("");  // Очистка сообщения об ошибке для номера телефона
    $("#EmailError").text(""); // Очистка сообщения об ошибке для почты
    $("#PasswordError").text("");  // Очистка сообщения об ошибке для пароля
    $("#ConfirmPasswordError").text(""); // Очистка сообщения об ошибке для проверки пароля
    var name = $("#Username").val();
    var phone = $("#Telephone").val();
    var email = $("#Email").val();
    var password = $("#Password").val();
    var confirmPassword = $("#ConfirmPassword").val();
    $.ajax({
        url: "Registration/Register", // Укажите адрес вашего метода действия контроллера
        type: "POST", // или "GET", в зависимости от вашего метода действия
        data: { Username: name, Email: email, Password: password, ConfirmPassword: confirmPassword, Telephone: phone },
        success: function (response) {
            // Проверяем наличие сообщений об ошибках валидации
            if (response && response.errors) {
                // Отображаем сообщения об ошибках валидации                   
                for (var key in response.errors) {
                    $("#" + key + "Error").text(response.errors[key]);
                }
            }
            else {
                Swal.fire({
                    position: "top",
                    icon: "success",
                    title: "Поздравляем с успешной регистрацией.",
                    showConfirmButton: false,
                    timer: 5500,
                    timerProgressBar: true
                }).then((result) => {
                    window.location.href = "/Home/Index";
                });
            }
        },
        statusCode: {
            400: function () { // выполнить функцию если код ответа HTTP 400
                Swal.fire({
                    icon: "error",
                    title: "Ой...",
                    text: "Адрес электронной почты не найден"
                });
            }
        }
    });
});