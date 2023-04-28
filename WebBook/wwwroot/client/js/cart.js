$(document).ready(function () {
    $('body').on('click', '.btnAddToCart', function (e) {
        e.preventDefault();

        var id = $(this).data('id');
        var quantity = 1;
        var quantityText = $('#quantity_value').text();
        if (quantityText != '') {
            quantity = parseInt(quantityText);
        }

        $.ajax({
            url: '/cart/addtocart',
            type: 'POST',
            data: { id: id, quantity: quantity },
            success: function (rs) {
                if (rs.success) {
                    $('#checkout_items').html(rs.count);
                }
            }
        });
    });

    function GetTotalPrice() {
        var str = "";
        var listCheckbox = $('#table-cart').find('tr td input:checked');
        var i = 0;
        listCheckbox.each(function () {
            var _id = $(this).data('id');
            if (i == 0) {
                str += _id;
            }
            else {
                str += "," + _id;
            }
            i++;
        });

        if (str.length > 0) {
            $.ajax({
                url: 'cart/totalprice',
                type: 'POST',
                data: { ids: str },
                success: function (rs) {
                    if (rs.success) {
                        $('#totalPrice').html(rs.t);
                    }
                }
            });
        }
        else {
            $('#totalPrice').html('0');
        }
    }

    $('body').on('click', '#btnDeleteCartItem', function () {
        var id = $(this).data('id');
        $.ajax({
            url: 'cart/delete',
            type: 'POST',
            data: { id: id },
            success: function (rs) {
                if (rs.success) {
                    $('#trCartItem-' + id).remove();
                    GetTotalPrice();
                }
            }
        });
    });

   

    $('body').on('click', '.updateQuantity', function (e) {
        e.preventDefault();
        var id = $(this).data('id');
        var quantity = $('.quantity_value-' + id).text();
        if (quantity != '') {
            quantity = parseInt(quantity);
        }

        $.ajax({
            url: 'cart/update',
            type: 'POST',
            data: { id: id, quantity: quantity },
            success: function (rs) {
                if (rs.success) {
                    $('.quantity_value-' + id).html(rs.quantity);
                    $('#totalPrice-' + id).html(rs.totalPrice);
                    GetTotalPrice();
                }
            }
        });
    });

    $('body').on('change', '#checkboxAll', function () {
        $('input:checkbox').not(this).prop('checked', this.checked);
        GetTotalPrice();
    });

    $('body').on('change', '#checkboxItem', function (e) {
        e.preventDefault();
        GetTotalPrice();
    });

    //$('body').on('click', '#btnThanhToan', function (e) {
    //    //e.preventDefault();
    //    var str = "";
    //    var listCheckbox = $('#table-cart').find('tr td input:checked');
    //    var i = 0;
    //    listCheckbox.each(function () {
    //        var _id = $(this).data('id');
    //        if (i == 0) {
    //            str += _id;
    //        }
    //        else {
    //            str += "," + _id;
    //        }
    //        i++;
    //    });

    //    if (str.length > 0) {
    //        $.ajax({
    //            url: '/cart/checkout',
    //            contentType: 'application/html; charset=utf-8',
    //            type: 'GET',
    //            dataType: 'html',
    //            async: false,
    //            cache: false,
    //            data: { ids: str },
    //            success: function (carts) {
    //                $(document).html(carts); 
    //                //console.log(carts);
    //            }
    //        });
    //    }
    //    else {
    //        alert('Bạn chưa chọn sản phẩm nào!');
    //    }
    //});
    ``

    $('body').on('click', '#btnThanhToan', function () {
        var str = '';
        var listCheckbox = $('#table-cart').find('tr td input:checked');
        var i = 0;
        listCheckbox.each(function () {
            var _id = $(this).data('id');
            if (i == 0) {
                str += _id;
            }
            else {
                str += "," + _id;
            }
            i++;
        });
        $('#list_IdCart').val(str);
        if (str != '') {
            //$('#formpay').submit();
            $('#formpay').unbind('submit').submit();
        }
        else {
            alert('Bạn chưa chọn sản phẩm nào!');
            $('#formpay').submit(function (e) {
               e.preventDefault();
                //return false;
            });
        }
    });
});
