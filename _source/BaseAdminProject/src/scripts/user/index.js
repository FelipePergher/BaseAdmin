"use strict";
import "jquery-validation";
import "jquery-validation-unobtrusive";
import "datatables.net";
import "datatables.net-bs4";

export default (function () {

    $(function () {
        initPage();
    });

    function initPage() {
        let userTable = $("#userTable").DataTable({
            language: global.datatablesLanguage
        });

        $("#searchForm").submit(function (e) {
            e.preventDefault();
            userTable.search("").draw("");
        });
    }
}());