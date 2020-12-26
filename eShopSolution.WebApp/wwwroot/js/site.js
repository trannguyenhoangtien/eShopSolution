var SiteController = function () {
    this.initialize = function () {
        loadCart();
        RegisterEvents();
    }
    function loadCart() {
        var url = '/' + $('#hidCulture').val() + '/cart/GetListCart';
        $.ajax({
            type: 'GET',
            url: url,
            data: {
                languageId: $('#hidCulture').val()
            },
            success: function (res) {
                $("#lbTotalItemCommon").html(res.length);
            },
            error: function (err) {
                
            }
        });
    }

    function RegisterEvents() {
        $('body').on('click', '.btn-add-cart', function (e) {
            e.preventDefault();
            const id = $(this).data('id');
            var url = '/' + $('#hidCulture').val() + '/cart/AddToCart';
            var redirect = '/' + $('#hidCulture').val() + '/cart';
            $.ajax({
                type: 'POST',
                url: url,
                dataType: 'json',
                data: {
                    id: id
                },
                success: function (res) {
                    window.location.href = redirect;
                },
                error: function (e) {
                    alert(e);
                }
            });
        });
    }
}