function openModal(parameters) {

    $.ajax({
        type: 'GET',
        success: function (response) {
            modal.find(".modal-body").html(response);
            modal.modal('show')

        },
        failure: function () {
            modal.modal('hide')
        }
    });
};
