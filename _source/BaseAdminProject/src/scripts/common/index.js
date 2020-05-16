"use strict";
import './scss';
import "bootstrap";
import Cookies from 'js-cookie';
import 'jquery.scrollbar';
import 'jquery-scroll-lock';
import "@creative-tim-official/argon-dashboard-free/assets/js/components/init/navbar.min";
import "@creative-tim-official/argon-dashboard-free/assets/js/components/vendor/scrollbar.min";

export default (function () {

    $(function () {
        initPage();
    });

    function initPage() {
        var $sidenavState = Cookies.get('sidenav-state') ? Cookies.get('sidenav-state') : 'pinned';

        if ($(window).width() > 1200) {
            if ($sidenavState == 'pinned') {
                pinSidenav();
            }

            if (Cookies.get('sidenav-state') == 'unpinned') {
                unpinSidenav();
            }

            $(window).resize(function () {
                if ($('body').hasClass('g-sidenav-show') && !$('body').hasClass('g-sidenav-pinned')) {
                    $('body').removeClass('g-sidenav-show').addClass('g-sidenav-hidden');
                }
            });
        }

        if ($(window).width() < 1200) {
            $('body').removeClass('g-sidenav-hide').addClass('g-sidenav-hidden');
            $('body').removeClass('g-sidenav-show');
            $(window).resize(function () {
                if ($('body').hasClass('g-sidenav-show') && !$('body').hasClass('g-sidenav-pinned')) {
                    $('body').removeClass('g-sidenav-show').addClass('g-sidenav-hidden');
                }
            });
        }

        $("body").on("click", "[data-action]", function (e) {

            console.log(e);
            e.preventDefault();

            var $this = $(this);
            var action = $this.data('action');
            var target = $this.data('target');

            // Manage actions
            switch (action) {
                case 'sidenav-pin':
                    pinSidenav();
                    break;

                case 'sidenav-unpin':
                    unpinSidenav();
                    break;

                case 'search-show':
                    target = $this.data('target');
                    $('body').removeClass('g-navbar-search-show').addClass('g-navbar-search-showing');

                    setTimeout(function () {
                        $('body').removeClass('g-navbar-search-showing').addClass('g-navbar-search-show');
                    },
                        150);

                    setTimeout(function () {
                        $('body').addClass('g-navbar-search-shown');
                    },
                        300)
                    break;

                case 'search-close':
                    target = $this.data('target');
                    $('body').removeClass('g-navbar-search-shown');

                    setTimeout(function () {
                        $('body').removeClass('g-navbar-search-show').addClass('g-navbar-search-hiding');
                    },
                        150);

                    setTimeout(function () {
                        $('body').removeClass('g-navbar-search-hiding').addClass('g-navbar-search-hidden');
                    },
                        300);

                    setTimeout(function () {
                        $('body').removeClass('g-navbar-search-hidden');
                    },
                        500);
                    break;
            }
        });

        // Add sidenav modifier classes on mouse events
        $('.sidenav').on('mouseenter', function () {
            if (!$('body').hasClass('g-sidenav-pinned')) {
                $('body').removeClass('g-sidenav-hide').removeClass('g-sidenav-hidden').addClass('g-sidenav-show');
            }
        });

        $('.sidenav').on('mouseleave', function () {
            if (!$('body').hasClass('g-sidenav-pinned')) {
                $('body').removeClass('g-sidenav-show').addClass('g-sidenav-hide');

                setTimeout(function () {
                    $('body').removeClass('g-sidenav-hide').addClass('g-sidenav-hidden');
                }, 300);
            }
        });

        // Make the body full screen size if it has not enough content inside
        $(window).on('load resize', function () {
            if ($('body').height() < 800) {
                $('body').css('min-height', '100vh');
                $('#footer-main').addClass('footer-auto-bottom');
            }
        });
    }

    function pinSidenav() {
        $('.sidenav-toggler').addClass('active');
        $('.sidenav-toggler').data('action', 'sidenav-unpin');
        $('body').removeClass('g-sidenav-hidden').addClass('g-sidenav-show g-sidenav-pinned');
        $('body').append('<div class="backdrop d-xl-none" data-action="sidenav-unpin" data-target=' + $('#sidenav-main').data('target') + ' />');

        // Store the sidenav state in a cookie session
        Cookies.set('sidenav-state', 'pinned');
    }

    function unpinSidenav() {
        $('.sidenav-toggler').removeClass('active');
        $('.sidenav-toggler').data('action', 'sidenav-pin');
        $('body').removeClass('g-sidenav-pinned').addClass('g-sidenav-hidden');
        $('body').find('.backdrop').remove();

        // Store the sidenav state in a cookie session
        Cookies.set('sidenav-state', 'unpinned');
    }

    // Global Methods
    global.datatablesLanguage = {
        "sEmptyTable": "Nenhum registro encontrado",
        "sInfo": "Mostrando de _START_ até _END_ de _TOTAL_ registros",
        "sInfoEmpty": "Mostrando 0 até 0 de 0 registros",
        "sInfoFiltered": "(Filtrados de _MAX_ registros)",
        "sInfoPostFix": "",
        "sInfoThousands": ".",
        "sLengthMenu": "_MENU_ resultados por página",
        "sLoadingRecords": "Carregando...",
        "sProcessing": "Processando...",
        "sZeroRecords": "Nenhum registro encontrado",
        "sSearch": "Pesquisar",
        "oPaginate": {
            "sNext": "Próximo",
            "sPrevious": "Anterior",
            "sFirst": "Primeiro",
            "sLast": "Último"
        },
        "oAria": {
            "sSortAscending": ": Ordenar colunas de forma ascendente",
            "sSortDescending": ": Ordenar colunas de forma descendente"
        }
    };

    global.select2Language = {
        errorLoading: function () {
            return 'Os resultados não puderam ser carregados.';
        },
        inputTooLong: function (args) {
            var overChars = args.input.length - args.maximum;

            var message = 'Apague ' + overChars + ' caracter';

            if (overChars != 1) {
                message += 'es';
            }

            return message;
        },
        inputTooShort: function (args) {
            var remainingChars = args.minimum - args.input.length;

            var message = 'Digite ' + remainingChars + ' ou mais caracteres';

            return message;
        },
        loadingMore: function () {
            return 'Carregando mais resultados…';
        },
        maximumSelected: function (args) {
            var message = 'Você só pode selecionar ' + args.maximum + ' ite';

            if (args.maximum == 1) {
                message += 'm';
            } else {
                message += 'ns';
            }

            return message;
        },
        noResults: function () {
            return 'Nenhum resultado encontrado';
        },
        searching: function () {
            return 'Buscando…';
        },
        removeAllItems: function () {
            return 'Remover todos os itens';
        }
    };

    //Todo use if necessary
    //global.setupValidator = function (validator) {
    //    validator.setDefaults({
    //        highlight: function highlight(element) {
    //            $(element).addClass('is-invalid').removeClass('is-valid');
    //        },
    //        // eslint-disable-next-line object-shorthand
    //        unhighlight: function unhighlight(element) {
    //            $(element).addClass('is-valid').removeClass('is-invalid');
    //        },
    //        errorElement: 'span',
    //        errorPlacement: function errorPlacement(error, element) {
    //            error.addClass('invalid-feedback');
    //            element.prop('type') === 'checkbox' ? error.insertAfter(element.parent('label')) : error.insertAfter(element);
    //        }
    //    });
    //};

    global.eyePassword = function () {
        $(".eyePassword").click(function () {
            let icon = $(this);
            let inputPassword = icon.siblings("input");
            if (icon.hasClass("fa-eye")) {
                icon.removeClass("fa-eye").addClass("fa-eye-slash");
                inputPassword.attr("type", "password");
            } else {
                icon.addClass("fa-eye").removeClass("fa-eye-slash");
                inputPassword.attr("type", "text");
            }
        });
    };

    global.masks = {
        Cpf: '000.000.000-00',
        Price: '000.000.000.000.000,00',
        date: '99/99/9999'
    };

    global.setupPhoneMaskOnField = function (selector) {
        var inputElement = $(selector);

        setCorrectPhoneMask(inputElement);
        inputElement.on('input, keyup', function () {
            setCorrectPhoneMask(inputElement);
        });
    }

    global.SPMaskBehavior = function (val) {
        return val.replace(/\D/g, '').length === 11 ? '(00) 00000-0000' : '(00) 0000-00009';
    };

    global.spOptions = {
        onKeyPress: function (val, e, field, options) {
            field.mask(SPMaskBehavior.apply({}, arguments), options);
        }
    };

}());
