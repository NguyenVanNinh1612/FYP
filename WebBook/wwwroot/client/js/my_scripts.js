$(document).ready(function () {
    $('#newsletter_submit').on('click', function (e) {
        e.preventDefault();
        var email = $('#newsletter_email').val();
        if (email == '') {
            $('.validEmail').html('Email is required');
            return false;
        }
        if (IsEmail(email) == false) {
            $('.validEmail').html('Email is invalid');
            return false;
        }
        $('.validEmail').html('');
        if (email != '') {
            $.ajax({
                url: '/home/subscribe',
                type: 'POST',
                data: { email: email },
                success: function (rs) {

                }
            });
        }
    });

    function IsEmail(email) {
        var regex = /^([a-zA-Z0-9_\.\-\+])+\@(([a-zA-Z0-9\-])+\.)+([a-zA-Z0-9]{2,4})+$/;
        if (!regex.test(email)) {
            return false;
        } else {
            return true;
        }
    }


    $('body').on('click', '#review_submit', function (e) {
        e.preventDefault();
        var content = $('#review_message').val();
        var rating = parseInt($('#rating_value').val());
        var userId = $('#user_id').val();
        var productId = parseInt($('#product_id').val());
        if (userId == '') {
            $('#review_error').html('Bạn phải đăng nhập để thực hiện chức năng này!');
            return false;
        }
        if (content == '') {
            $('#review_error').html('Bạn chưa nhập nội dung!');
            return false;
        }



        $.ajax({
            url: '/review/submit',
            type: 'POST',
            data: {
                userId: userId,
                productId: productId,
                content: content,
                rating: rating
            },
            success: function (rs) {
                if (rs.success) {
                    var str = "";
                    for (var i = 0; i <= rating; i++) {
                        str += '<li><i class="fa fa-star" aria-hidden="true"></i></li>';
                    }
                    for (var i = 0; i < 4 - rating; i++) {
                        str += '<li><i class="fa fa-star-o" aria-hidden="true"></i></li>';
                    }
                    var html = '<div class="user_review_container d-flex flex-column flex-sm-row"><div class="user"><div class="user_pic"><img width="100%" id="user_pic"/></div><div class="user_rating"><ul class="star_rating">' + str + '</ul></div> </div><div class="review"><div class="review_date">' + rs.createdDate + '</div> <div class="user_name">' + rs.fullName + '</div><p>' + content + '</p></div></div>';
                    $('#prepend_review').prepend(html);
                    $('#user_pic').attr('src', $('#get_user_pic').attr('src'));
                    var count = parseInt($('#count_review').text());
                    $('#count_review').text(count + 1);
                }
            }
        });
    });



    //Acive linke menu
    var pathMenuCurrent = window.location.pathname;
    var menuCurrentName = pathMenuCurrent.split('/');
    $('.navbar_menu li').each(function () {
        if ($(this).data('id') == menuCurrentName[1].toLowerCase()) {
            $(this).css({ "border-bottom": "2px solid #fe4c50" });
            $(this).children().css({ "color": "#fe4c50" });
        }
        else if ($(this).data('id') == 'home' && menuCurrentName[1] == '') {
            $(this).css({ "border-bottom": "2px solid #fe4c50" });
            $(this).children().css({ "color": "#fe4c50" });
        }
        
    });


    searchForm = document.querySelector('.search-form');
    document.querySelector("#search-btn").onclick = () => {
        searchForm.classList.toggle('active');
    }

});