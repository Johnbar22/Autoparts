

//оформление заказа. заполнение конечной формы

//const { error } = require("jquery");

console.log("000")

$("#submitBtn").click(function (e) {
    e.preventDefault();
    $(".overlay").show();
    $(".spinner").show();
    $("#UserNameError").text(""); // Очистка сообщения об ошибке для фио
    $("#UserPhoneError").text("");  // Очистка сообщения об ошибке для номера телефона
    $("#UserEmailError").text(""); // Очистка сообщения об ошибке для почты
    var name = $("#UserName").val();
    var phone = $("#UserPhone").val();
    var email = $("#UserEmail").val();
    var totalPrice = $("#totalPrice").val();

    $.ajax({
        url: "Cart/ValidateData", 
        type: "POST", 
        data: { UserName: name, UserEmail: email, UserPhone: phone, TotalPrice: totalPrice },
        success: function (response) {
            $(".overlay").hide();
            $(".spinner").hide();

            // Проверяем наличие сообщений об ошибках валидации
            if (response && response.errors) {
                // Отображаем сообщения об ошибках валидации                   
                for (var key in response.errors) {
                    //console.log("вход в for (227)");
                    //console.log("Ошибка " + key + " текст ошибки: " + response.errors[key]);
                    $("#" + key + "Error").text(response.errors[key]);
                }

            }
            else {
                updateCartCount(response.cartItemCount);
                Swal.fire({
                    position: "top",
                    icon: "success",
                    title: "Заказ успешно оформлен.",
                    text: "В ближайшее время мы с Вами свяжемся для уточнения заказа",
                    showConfirmButton: false,
                    footer: "Детали заказа придут Вам на почту",
                    timer: 5500,
                    timerProgressBar: true
                }).then((result) => {
                    window.location.href = "/Cart/Index";
                });
            }
        },
        statusCode: {
            400: function (response) { // выполнить функцию если код ответа HTTP 404
                $(".overlay").hide();
                $(".spinner").hide();

                var errorMessage = response.responseText;
                console.log("response = " + response);
                console.log("responseJson = " + response.responseJSON);
                console.log("responseJson = " + response.responseJSON.responseText);
                console.log("responseJson = " + response.responseJSON.error);
                console.log("responseJson = " + response.responseJSON.errors);
                console.log("responseJson = " + response.responseJSON.errorMessage);
                console.log("responseJson = " + response.responseText);

                if (response.responseJSON) {
                    var errors = response.responseJSON;

                    // Извлечение ключа и значения из объекта errors
                    var errorMessage = '';
                    for (var key in errors) {
                        if (errors.hasOwnProperty(key)) {
                            errorMessage = errors[key];
                            break; // Предполагаем, что нужен только первый элемент
                        }
                    }

                    Swal.fire({
                        icon: "error",
                        title: "Ой...",
                        text: errorMessage
                    });
                }

                console.log(response);


                console.log("ResponseHsonText = " + errorMessage);
                Swal.fire({
                    icon: "error",
                    title: "Ой...",
                    text: errorMessage.trim()
                });
            }

        }

    });
});



// $(document).ready(function () {
//$("#submitBtn").click(function () {
//$("#UserNameError").text(""); // Очистка сообщения об ошибке для фио
//$("#UserPhoneError").text("");  // Очистка сообщения об ошибке для номера телефона
//$("#UserEmailError").text(""); // Очистка сообщения об ошибке для почты
// });
//  });


// Изменение количества товара в корзине
$(".BtUpdateCount").click(function () {
    // Выполняем AJAX-запрос для каждого товара
    $(".cartItem").each(function () {
        var itemId = $(this).find(".cartPartId").val();
        var partCount = $(this).find(".partCount").val();
        $.ajax({
            url: "Cart/UpdateCount",
            type: "POST",
            data: { cartItemId: itemId, quantity: partCount },
            success: function (response) {
                if (response.success) {
                    console.log("Изменения сохранены");
                } else {
                    console.error("Ошибка: " + response.message);
                }
            },
            error: function (xhr, status, error) {
                console.error("Ошибка: " + xhr.responseText);
            }
        });
    });
    Swal.fire({
        position: "top",
        icon: "success",
        html: "Изменения сохранены",
        showConfirmButton: false,
        timer: 2500,
        timerProgressBar: true
    }).then((result) => {
        window.location.href = "/Cart";
    });
});


// Удаление выбранной позиции из корзины
$(".removeCartItem").click(function () {
    // Получаем идентификатор элемента корзины и количество товаров
    var cartItemId = $(this).data("cartitemid");

    // Отправляем AJAX-запрос на сервер для удаления элемента из корзины
    $.ajax({
        
        /*url: "/Cart/RemoveFromCart",*/
        url: "/Cart/RemoveCartItem",
        type: "POST",
        data: { cartItemId: cartItemId },
        success: function (response) {
            // Обновляем количество позиций в корзине
            updateCartCount(response.cartItemCount);
            Swal.fire({
                position: "top",
                icon: "success",
                html: "Товар удален",
                showConfirmButton: false,
                timer: 2500,
                timerProgressBar: true
            }).then((result) => {
                window.location.href = "/Cart";
            });
        },
        error: function (xhr, status, error) {
            console.error(xhr.responseText);
        }
    });
});

