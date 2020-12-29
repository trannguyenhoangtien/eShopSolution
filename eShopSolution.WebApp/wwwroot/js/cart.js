var CartController = function () {
    this.initialize = function () {
        loadData();
        registerEvents();
    }

    function registerEvents() {
        $('body').on('click', '.btn-plus', function (e) {
            e.preventDefault();
            const id = $(this).data('id');
            var qty = parseInt($('#txt_quantity_' + id).val()) + 1;
            updateCart(id, (qty));
        });

        $('body').on('click', '.btn-minus', function (e) {
            e.preventDefault();
            const id = $(this).data('id');
            var qty = parseInt($('#txt_quantity_' + id).val()) - 1;
            updateCart(id, qty);
        });

        $('body').on('click', '.btn-remove', function (e) {
            e.preventDefault();
            const id = $(this).data('id');
            updateCart(id, 0);
        });

        $('body').on('change', '.txt-qty', function (e) {
            e.preventDefault();
            const id = $(this).data('id');
            var qty = parseInt($('#txt_quantity_' + id).val())
            updateCart(id, qty);
        });
        
    }

    function updateCart(id, quantity) {
        var url = '/' + $('#hidCulture').val() + '/cart/UpdateCart';
        $.ajax({
            type: 'POST',
            url: url,
            data: {
                id: id,
                quantity: quantity
            },
            success: function (res) {
                loadData();
            },
            error: function (err) {
                console.log(err);
            }
        });
    }

    function loadData() {
        var BaseImageAddress = $("#BaseImageAddress").val();
        var url = '/' + $('#hidCulture').val() + '/cart/GetListCart';
        $.ajax({
            type: 'GET',
            url: url,
            data: {
                languageId: $('#hidCulture').val()
            },
            success: function (res) {
                if (res.length === 0) {
                    $("#tbCart").hide();
                    return;
                }
                var html = '';
                var total = 0;
                $.each(res, function (i, item) {
                    html += '<tr><td width="150"> <img width="150" src="' + BaseImageAddress + item.image
                        + '" alt="" /></td>' +
                        '<td>' + item.name + '</td>' + '<td>' + item.description + '</td><td>' +
                        '<div class="input-append"><input  data-id="' + item.productId + '" class="span1 txt-qty" style="max-width:34px" value="' + item.quantity + '" placeholder="1" id="txt_quantity_' + item.productId + '" size="16" type="text"><button class="btn btn-minus" data-id="' + item.productId + '" type="button"><i class="icon-minus"></i></button><button data-id="' + item.productId + '" class="btn btn-plus" type="button"><i class="icon-plus"></i></button><button class="btn btn-danger btn-remove" type="button" data-id="' + item.productId + '"><i class="icon-remove icon-white"></i></button>				</div>' +
                        '</td><td>' + item.price + '</td><td>' + item.price * item.quantity + '</td></tr>';
                    total += item.price * item.quantity;
                });
                $("#cart_body").html(html);
                $("#lbTotal").html(total);
                $("#lbTotalItem").html(res.length);
                $("#lbTotalItemCommon").html(res.length);
            },
            error: function (err) {
                console.log(err);
            }
        });
    }
}