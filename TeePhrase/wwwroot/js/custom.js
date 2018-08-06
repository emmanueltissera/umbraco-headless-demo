jQuery(document).ready(function ($) {

    if (jQuery().quicksand) {

        // Clone applications to get a second collection
        var $data = $(".portfolio-area").clone();

        //NOTE: Only filter on the main portfolio page, not on the subcategory pages
        $('.filter button').click(function (e) {
            
            $(".filter button").removeClass("active");
            // Use the last category class as the category to filter by. This means that multiple categories are not supported (yet)
            var filterClass = $(this).attr('data-filter');
            
            if (filterClass == 'all') {
                var $filteredData = $data.find('.portfolio-item');
            } else {
                var $filteredData = $data.find('.portfolio-item[data-type=' + filterClass + ']');
            }

            $(".portfolio-area").quicksand($filteredData, {
                duration: 400,
                easing: 'easeInOutQuad',
                adjustHeight: 'auto'
            });
            
            $(this).addClass("active");
            return false;
        });

    }//if quicksand

});


$(function () {

    $("#paymentForm select#quantity").change(function () {
        var shippingRate = $("#paymentForm input#shipping");
        if ($(this).val() == "") {
            shippingRate.val("");
        }
        else if ($(this).val() == "1") {
            shippingRate.val(shippingRate.data("first-item"));
        }
        else {
            shippingRate.val(shippingRate.data("additional-items"));
        }
    });

});
