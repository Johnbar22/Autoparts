
$(document).ready(function () {
    /*checkAdminStatus();*/
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



// Проверяем статус администратора при загрузке страницы
//checkAdminStatusOnStart();

// Обработчик нажатия кнопки "Для администратора"
//$('#admin').click(function () {
//    // Проверяем статус администратора перед показом модального окна
//    checkAdminStatus();
//});



// Обработчик отправки формы авторизации администратора
//$('#adminLoginForm').submit(function (e) {
//    e.preventDefault(); // Предотвращаем отправку формы по умолчанию

//    // Отправляем данные на сервер для входа администратора
//    var token = $('input[name="__RequestVerificationToken"]').val();
//    $.ajax({
//        type: 'POST',
//        url: '/Admin/Login',
//        data: {
//            Login: $('#adminEmail').val(),
//            Password: $('#adminPassword').val()
//        },
//        headers: {
//            RequestVerificationToken: token
//        },
//        success: function (response) {
//            Swal.fire({
//                position: "top",
//                icon: "success",
//                title: "Вы вошли в учетную запись администратора",
//                showConfirmButton: false,
//                timer: 2500
//            }).then((result) => {
//                window.location.href = response.redirectUrl;
//            });
//                        // Обработка успешного ответа от сервера
//                        /*window.location.href = response.redirectUrl*/;
//        },
//        error: function (error) {
//            // Обработка ошибки
//            Swal.fire({
//                icon: "error",
//                title: "Oops...",
//                text: "Ошибка входа!",
//            });
//            //alert('Ошибка при аутентификации: ' + error);
//        }
//    });
//});


// Функция для проверки статуса администратора
//function checkAdminStatus() {
//    $.ajax({
//        type: 'GET',
//        url: '/Admin/CheckRole',
//        success: function (response) {
//            if (response) {
//                console.log("CheckRole: Role = admin")
//                $('#loginModal').modal('hide');
//                $('#linkExit').show();
//                $('#linkAdmin').show();
//                $('#admin').hide();

//            } else {
//                console.log("CheckRole: Role = Guest");
//                $('#admin').show();
//                $('#linkExit').hide();
//                $('#linkAdmin').hide();
//            }
//        },
//        error: function (error) {
//            // Обработка ошибки
//            alert('Ошибка при проверке статуса администратора: ' + error);
//        }
//    });
//}


//loginButton
//выход из учетной записи админа

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
                console.log("вход в if (136)");
                //if (!login || !password) {
                //    for (var key in response.errors) {
                //        $("#" + key + "Error").text(response.errors[key]);
                //        console.log(key);
                //    }
                //}
                //else {
                //    console.log("response.errors = " + response.errors)
                //    //errorMessage += "\n" + response.errors[Login].join("\n");
                //    Swal.fire({
                //        icon: "error",
                //        title: "Ошибка",
                //        text: errorMessage,
                //    });
                //}




                for (var key in response.errors) {
                    console.log("вход в for (157)");
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
                    /*if (login == "admin@admin.ru") {*/
                    if (response.userRoleId == 1) {
                        window.location.href = '/Order/Index';
                    }
                    else {
                        window.location.href = '/Home/Index';//response.redirectUrl;
                    }
                });
            }

            //window.location.href = response.redirectUrl;
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




//if (response && response.errors) {
//    let errorMessage = "Произошли ошибки:";
//    // Отображаем сообщения об ошибках валидации                   
//    for (var key in response.errors) {
//        console.log("вход в for (134)");
//        //console.log("Ошибка " + key + " текст ошибки: " + response.errors[key]);
//        $("#" + key + "Error").text(response.errors[key]);
//        //if (response.errors.hasOwnProperty(key)) {
//        //    errorMessage += "\n" + response.errors[key].join("\n");
//        //}
//    }
//    Swal.fire({
//        icon: "error",
//        title: "Ошибка",
//        text: errorMessage,
//    });

//}
//else {
//    Swal.fire({
//        position: "top",
//        icon: "info",
//        title: "Успешный вход.",
//        showConfirmButton: false,
//        timer: 2500
//    }).then((result) => {
//        window.location.href = '/Home/Index';//response.redirectUrl;                 
//    });
//}


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
                window.location.href = '/Home/Index';//response.redirectUrl;
            });
            //window.location.href = response.redirectUrl;
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
    console.log("вход в TEST");
    $.ajax({
        url: "Registration/Register", // Укажите адрес вашего метода действия контроллера
        type: "POST", // или "GET", в зависимости от вашего метода действия
        data: { Username: name, Email: email, Password: password, ConfirmPassword: confirmPassword, Telephone: phone },
        success: function (response) {
            console.log("TEST - success");

            // Проверяем наличие сообщений об ошибках валидации
            if (response && response.errors) {
                // Отображаем сообщения об ошибках валидации                   
                for (var key in response.errors) {
                    console.log("вход в for (227)");
                    //console.log("Ошибка " + key + " текст ошибки: " + response.errors[key]);
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
            400: function () { // выполнить функцию если код ответа HTTP 404
                Swal.fire({
                    icon: "error",
                    title: "Ой...",
                    text: "Адрес электронной почты не найден"
                });
            }

        }

    });
});