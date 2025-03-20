jQuery(document).ready(function () {
    $('#submenu1').mouseenter(function () {
        //  e.preventDefault();
        //alert($(this).next('ul').attr('display'));
        if ($(this).next('ul').length) {
            $(this).next('ul').show();
        }
    });
});