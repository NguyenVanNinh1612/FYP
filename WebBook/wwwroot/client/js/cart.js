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
});