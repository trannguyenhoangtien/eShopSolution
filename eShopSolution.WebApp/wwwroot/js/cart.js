var CartController = function () {
    this.initialize = function () {
        loadData();
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
                var html = '';
                var total = 0;
                $.each(res, function (i, item) {
                    html += '<tr><td width="150"> <img width="150" src="' + BaseImageAddress + item.image
                        + '" alt="" /></td>' +
                        '<td>' + item.name + '</td>' + '<td>' + item.description + '</td><td>' +
                        '<div class="input-append"><input class="span1" style="max-width:34px" value="' + item.quantity + '" placeholder="1" id="appendedInputButtons" size="16" type="text"><button class="btn" type="button"><i class="icon-minus"></i></button><button class="btn" type="button"><i class="icon-plus"></i></button><button class="btn btn-danger" type="button"><i class="icon-remove icon-white"></i></button>				</div>' +
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