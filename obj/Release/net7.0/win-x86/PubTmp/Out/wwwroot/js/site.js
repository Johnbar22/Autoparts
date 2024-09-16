// Please see documentation at https://docs.microsoft.com/aspnet/core/client-side/bundling-and-minification
// for details on configuring this project to bundle and minify static web assets.

// Write your JavaScript code.


function openModal(parameters){

    $.ajax({
        type: 'GET',
       success: function(response){
           modal.find(".modal-body").html(response);
           modal.modal('show')

        },
     failure:function(){
         modal.modal('hide')
       }
});


};
