"use strict";
import "jquery-validation";
import "jquery-validation-unobtrusive";
import "datatables.net";
import "datatables.net-bs4";
import { DatatablesLanguage, Notify, SwalWithBootstrapButtons } from '../common/common';

export default (function () {

    $(function () {
        initPage();
    });

    function initPage() {
        initTable();
    }

    function initTable() {
        $.fn.dataTable.ext.search.push(
            function (settings, data, dataIndex) {
                const showInactive = $("#showInactive").is(":checked");
                const activeAccount = data[5];
                if (activeAccount === "True" || showInactive) {
                    return true;
                }

                return false;
            }
        );

        $("#userDataTable").on("init.dt", function () {
            $("div.dataTables_length select").removeClass("custom-select custom-select-sm");
        }).DataTable({
            language: DatatablesLanguage(),
            ajax: {
                url: "/api/user/getAll",
                type: "GET",
                error: function (e) {
                    Notify("danger", "Não foi possível carregar as informações! <br> Se o problema persistir contate o administrador!");
                }
            },
            columns: [
                { data: "name", name: "Name" },
                { data: "birthdayDate", name: "BirthdayDate" },
                { data: "userName", name: "UserName" },
                { data: "email", name: "Email" },
                {
                    data: "confirmedAccount",
                    name: "ConfirmedAccount",
                    searchable: false,
                    render: {
                        "_": "plain",
                        "filter": "filter",
                        "display": "display"
                    }
                },
                {
                    data: "active",
                    name: "Active",
                    render: {
                        "_": "plain",
                        "filter": "filter",
                        "display": "display"
                    }
                },
                {
                    data: "blockedAccount",
                    name: "BlockedAccount",
                    searchable: false,
                    render: {
                        "_": "plain",
                        "filter": "filter",
                        "display": "display"
                    }
                },
                { data: "role", name: "Role" },
                { data: "actions", name: "Actions", searchable: false }
            ],
            drawCallback: function (settings) {
                $(".enableUserButton").click(function () {
                    initChangeState($(this).data("url"), true);
                });

                $(".disableUserButton").click(function (e) {
                    initChangeState($(this).data("url"));
                });
            }
        });

        $("#showInactive").change(function() {
            $("#userDataTable").DataTable().ajax.reload(null, false);
        });
    }

    function initChangeState(url, enable = false) {
        SwalWithBootstrapButtons.fire({
            title: 'Você têm certeza?',
            text: enable ? "O usuário receberá acesso ao sistema!" : "O usuário perderá o acesso ao sistema!",
            type: 'warning',
            showCancelButton: true,
            showLoaderOnConfirm: true,
            preConfirm: () => {
                $.post(url)
                    .done(function (data, textStatus) {
                        $("#userDataTable").DataTable().ajax.reload(null, false);
                        Notify("success", data);
                    }).fail(function (error) {
                        Notify("danger", error.responseText);
                    });
            }
        });
    }
}());
