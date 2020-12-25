// Please see documentation at https://docs.microsoft.com/aspnet/core/client-side/bundling-and-minification
// for details on configuring this project to bundle and minify static web assets.

// Write your JavaScript code.

$('body').on('click', '.btn-add-cart', function (e) {
    e.preventDefault();
    const id = $(this).data('id');
    var url = '/' + $('#hidCulture').val() + '/cart/AddToCart';
    
    $.ajax({
        type: 'POST',
        url: url,
        dataType: 'json',
        data: {
            id: id,
            languageId: $('#hidCulture').val()
        },
        success: function (res) {
            console.log(res);
        },
        error: function (e) {
            alert(e);
        }
    });
});