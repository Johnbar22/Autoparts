

$('#getOrdersBut').click(function () {
    // Переключаем видимость спиннера
    $(this).find('.spinner-border').removeClass('d-none');

    // Блокируем кнопку
    $(this).prop('disabled', true);

    var selectedDate = $('#dateInput').val();

    $.ajax({
        url: '/Order/GetOrders', // Укажите URL вашего метода контроллера для получения списка заказов
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


$('#updateButton').click(function () {
    // Показать overlay
    $('#overlay').removeClass('d-none');
    // Переключаем видимость спиннера
    $(this).find('.spinner-border').removeClass('d-none');

    // Блокируем кнопку
    $(this).prop('disabled', true);
    
    // Выполняем обновление списка товаров
    $.ajax({
        url: '/DBUpdate/Index', 
        type: 'GET',
        success: function (data) {
            // Скрыть overlay
            $('#overlay').addClass('d-none');
            // Обновление списка товаров успешно выполнено
            // Восстанавливаем видимость спиннера и активность кнопки
            $('#updateButton').find('.spinner-border').addClass('d-none');
            $('#updateButton').prop('disabled', false);
            Swal.fire({
                text: "База успешно обновлена",
                imageUrl: "https://images.postnews.ru/unsafe/rs:auto:590:394/gravity:ce/q:90/plain/s3://postnews-new-prod/upload/623c8e211be90800127a385f.gif",
                imageWidth: 350,
                imageHeight: 400,
                imageAlt: "err"
            });
            console.log("updateDB success");

            // Ваш код для обновления отображаемых данных
        },
        error: function () {
            // Произошла ошибка при обновлении списка товаров
            // Восстанавливаем видимость спиннера и активность кнопки
            $('#updateButton').find('.spinner-border').addClass('d-none');
            $('#updateButton').prop('disabled', false);
            alert('Произошла ошибка при обновлении списка товаров.');
        }
    });
});


