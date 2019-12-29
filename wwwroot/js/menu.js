$(document).ready(function () {
    $('.menu1 li:has(ul)').click(function (e) {
        e.preventDefault();

        if ($(this).hasClass('activado')) {
            $(this).removeClass('activado');
            $(this).children('ul').slideUp();
        } else {
            $('.menu1 li ul').slideUp();
            $('.menu1 li').removeClass('activado');
            $(this).addClass('activado');
            $(this).children('ul').slideDown();
        }
    });

    $('.btn-menu').click(function () {
        $('.Contenedor-menu .menu1').slideToggle();
    });

    $(window).resize(function () {
        if ($(document).width() > 450) {
            $('.Contenedor-menu .menu1').css({ 'display': 'block' });
        }
        if ($(document).width() < 450) {
            $('.Contenedor-menu .menu1').css({ 'display': 'none' });
            $('.menu1 li ul').slideUp();
            $('.menu1 li').removeClass('activado');
        }

    });

});