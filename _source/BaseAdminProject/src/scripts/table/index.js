"use strict";
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
        var e = $("#datatable-basic");
        e.length &&
            e.on("init.dt",
                function () {
                    $("div.dataTables_length select").removeClass("custom-select custom-select-sm");
                }).DataTable({
                    keys: !0,
                    select: {
                        style: "multi"
                    },
                    language: global.datatablesLanguage
                });
    }

}());

//var DatatableBasic = function() {
//    }()
//    , DatatableButtons = function() {
//        var e, a = $("#datatable-buttons");
//        a.length && (e = {
//                lengthChange: !1,
//                dom: "Bfrtip",
//                buttons: ["copy", "print"],
//                language: {
//                    paginate: {
//                        previous: "<i class='fas fa-angle-left'>",
//                        next: "<i class='fas fa-angle-right'>"
//                    }
//                }
//            },
//            a.on("init.dt", function() {
//                $(".dt-buttons .btn").removeClass("btn-secondary").addClass("btn-sm btn-default")
//            }).DataTable(e))
//    }()