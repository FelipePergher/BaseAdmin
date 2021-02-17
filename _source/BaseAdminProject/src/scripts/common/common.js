"use strict";
import Swal from "sweetalert2";
import "bootstrap-datepicker";
import "bootstrap-datepicker/dist/locales/bootstrap-datepicker.pt-br.min";
import "bootstrap4-notify";

export function DatatablesLanguage () {
    return {
        sEmptyTable: "Nenhum registro encontrado",
        sInfo: "Mostrando de _START_ até _END_ de _TOTAL_ registros",
        sInfoEmpty: "Mostrando 0 até 0 de 0 registros",
        sInfoFiltered: "(Filtrados de _MAX_ registros)",
        sInfoPostFix: "",
        sInfoThousands: ".",
        sLengthMenu: "_MENU_ resultados por página",
        sLoadingRecords: "Carregando...",
        sProcessing: "Processando...",
        sZeroRecords: "Nenhum registro encontrado",
        sSearch: "Pesquisar",
        paginate: {
            previous: "<i class='fas fa-angle-left'>",
            next: "<i class='fas fa-angle-right'>"
        },
        oAria: {
            sSortAscending: ": Ordenar colunas de forma ascendente",
            sSortDescending: ": Ordenar colunas de forma descendente"
        }
    };
}

export function SetupValidator (validator) {
    validator.setDefaults({
        highlight: function highlight (element) {
            if ($(element).prop("type") !== "checkbox") {
                $(element).addClass("is-invalid").removeClass("is-valid").closest(".form-group").addClass("form-group-invalid").removeClass("form-group-valid");
            }
        },
        // eslint-disable-next-line object-shorthand
        unhighlight: function unhighlight (element) {
            if ($(element).prop("type") !== "checkbox") {
                $(element).addClass("is-valid").removeClass("is-invalid").closest(".form-group").addClass("form-group-valid").removeClass("form-group-invalid");
            }
        },
        errorElement: "span",
        errorPlacement: function errorPlacement (error, element) {
            error.addClass("invalid-feedback");
            element.prop("type") === "checkbox" ? error.insertAfter(element.parent("label")) : error.insertAfter(element);
        }
    });
}

export function Select2Language () {
    return {
        errorLoading: function () {
            return "Os resultados não puderam ser carregados.";
        },
        inputTooLong: function (args) {
            var overChars = args.input.length - args.maximum;

            var message = `Apague ${overChars} caracter`;

            if (overChars !== 1) {
                message += "es";
            }

            return message;
        },
        inputTooShort: function (args) {
            var remainingChars = args.minimum - args.input.length;

            return `Digite ${remainingChars} ou mais caracteres`;
        },
        loadingMore: function () {
            return "Carregando mais resultados…";
        },
        maximumSelected: function (args) {
            var message = `Você só pode selecionar ${args.maximum} ite`;

            message += args.maximum === 1 ? "m" : "ns";

            return message;
        },
        noResults: function () {
            return "Nenhum resultado encontrado";
        },
        searching: function () {
            return "Buscando…";
        },
        removeAllItems: function () {
            return "Remover todos os itens";
        }
    };
}

export function EyePassword () {
    $(".eyePassword").click(function () {
        const icon = $(this).find("i");
        const inputPassword = $(this).siblings("input");
        if (icon.hasClass("fa-eye")) {
            icon.removeClass("fa-eye").addClass("fa-eye-slash");
            inputPassword.attr("type", "password");
        } else {
            icon.addClass("fa-eye").removeClass("fa-eye-slash");
            inputPassword.attr("type", "text");
        }
    });
}

export function Notify (type, message) {
    $.notify({
        message: message
    }, {
        type: type,
        placement: {
            placement: "top",
            align: "right"
        }
    });
}

export function InitCalendar (elementId) {
    $(elementId).datepicker({
        clearBtn: true,
        format: "dd/mm/yyyy",
        language: "pt-BR",
        templates: {
            leftArrow: "<i class=\"fas fa-chevron-left\"></i>",
            rightArrow: "<i class=\"fas fa-chevron-right\"></i>"
        }
    });
}

export const SwalWithBootstrapButtons = Swal.mixin({
    customClass: {
        confirmButton: "btn btn-success",
        cancelButton: "btn btn-danger"
    },
    buttonsStyling: false,
    reverseButtons: true,
    confirmButtonText: "Sim",
    cancelButtonText: "Não"
});
