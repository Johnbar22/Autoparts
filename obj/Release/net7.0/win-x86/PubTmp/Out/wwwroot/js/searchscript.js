
console.log("Search")
$(".buyButton").click(function () {
    console.log("нажали кнопку: buyButton ")
    var autoPartId = $(this).siblings(".autoPartId").val();
    var count = $(this).siblings(".count").val();
    /*var maxAutopartCount = $(this).siblings(".maxAutopartCount").text();*/
    var addToCartUrl = $("#addToCartUrl").val(); // Получаем URL из скрытого поля
    

    $.ajax({
        //url: "@Url.Action("AddToCart", "Cart")",
        //url: addToCartUrl,
        url: '/Cart/AddToCart',
        type: "POST",
        data: { autoPartId: autoPartId, count: count },
        success: function (response) {
            updateCartCount(response); // Обновляем количество товаров в корзине на основе ответа от сервера
            console.log("@!!!!Обновляем количество товаров в корзине на основе ответа от сервера " + response);
            let timerInterval;
            Swal.fire({
                html: "Товар добавлен в корзину",
                timer: 5300,
                timerProgressBar: true,
                willClose: () => {
                    clearInterval(timerInterval);
                }
            });
        },
        statusCode: {
            404: function () { // выполнить функцию если код ответа HTTP 404
                alert("страница не найдена");
            },
            500: function () { // выполнить функцию если код ответа HTTP 500
                Swal.fire({
                    icon: "error",
                    title: "Ой...",
                    text: "Вы пытаетесь купить товаров больше, чем есть на складе.",
                    footer: `В <a href="Cart">корзину</a> добавлено максимальное количество данной позиции`
                });
            }
        }
    });
});
