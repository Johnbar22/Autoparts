// Реализованный ПОИСК по кнопке
$(".searchButton").click(function (e) {
    e.preventDefault();
    //console.log("homeIndexScript")
    var searchText = $(".searchString").val();

    if (!searchText.trim()) {
        console.log("Поле поиска не заполнено");
        return;  // Прекращаем выполнение, если searchText пусто
    }

    $("#loader").show();

    $.ajax({
        url: "/Home/Search?searchString=" + searchText,
        type: 'POST',
        success: function (result) {
            console.log("success: function,")
            $("#searchResults").html(result);
        },
        complete: function () {
            console.log("complete: function, #loader.hide")
            // Скрыть спиннер после завершения AJAX-запроса
            $("#loader").hide();
        }
    });
});



// Рализованный поиск по вводу букв // начало

// Обработчик события keydown для поля ввода
//function preventEnterSubmit(event) {
//    if (event.keyCode === 13) {
//        event.preventDefault(); // Предотвращаем стандартное действие по нажатию Enter
//        return false;
//    }
//}

//// Получаем все поля ввода и добавляем обработчик события для каждого из них
//var inputFields = document.querySelectorAll('input');
//inputFields.forEach(function (inputField) {
//    inputField.addEventListener('keydown', preventEnterSubmit);
//});




//$('#searchString').keyup(function () {
//    console.log("homeIndexScript")
//    $("#loader").show();
//    var searchText = $(this).val();
//    if (searchText.trim() === "") {
//        $('#searchResults').html("<p></p>");
//        $("#loader").hide();
//        return;
//    }

//    $.ajax({
//        url: "/Home/Search?searchString=" + searchText,
//        type: 'GET',
//        success: function (result) {
//            $('#searchResults').html(result);
//        },
//        complete: function () {
//            console.log("complete: function, #loader.hide")
//            // Скрыть спиннер после завершения AJAX-запроса
//            $("#loader").hide();
//        }
//    });
//});

// Рализованный поиск по вводу букв // конец