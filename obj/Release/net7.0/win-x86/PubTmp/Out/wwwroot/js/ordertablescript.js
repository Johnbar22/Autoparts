﻿
$('.getOrderDet').click(function () {
    console.log("!getOrderDet button.");
    var orderId = $(this).data('order-id');
    $.ajax({
        url: '/Order/GetOrderDetails',
        type: 'GET',
        data: { orderId: orderId },
        success: function (data) {
            // Здесь вы можете отобразить полученную информацию о заказе, например, в модальном окне
            console.log("!GetOrderDetails Success.");
            // Обновляем таблицу в модальном окне с полученными данными о деталях заказа
            //$('#orderDetailsModal').modal('show');
            $('#orderDetailsModal').html(data);
            // Показываем модальное окно

            $('#orderDetailsModal').modal('show');
        },
        error: function () {
            alert('Произошла ошибка при загрузке дополнительной информации о заказе.');
        }
    });
});
