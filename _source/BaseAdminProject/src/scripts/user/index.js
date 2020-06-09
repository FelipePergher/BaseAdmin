"use strict";
import "jquery-validation";
import "jquery-validation-unobtrusive";
import "datatables.net";
import "datatables.net-bs4";
import 'bootstrap4-notify';
import { DatatablesLanguage, Notify } from '../common/common';

export default (function () {

    $(function () {
        initPage();
    });

    function initPage() {
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
                { data: "email", name: "Email" },
                { data: "confirmedAccount", name: "ConfirmedAccount" },
                { data: "blockedAccount", name: "BlockedAccount" },
                { data: "role", title: "Regra", name: "Role" }
            ],
        });
    }
}());