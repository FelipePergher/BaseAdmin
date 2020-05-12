"use strict";
import "jquery-validation";
import "jquery-validation-unobtrusive";

export default (function () {

    $(function () {
        initPage();
    });

    function initPage() {
        initLoginForm();
    }

    function initLoginForm() {
        global.eyePassword();

        $("#loginForm").submit(function (e) {
            //Todo spinner
            let form = $(this);
            if (form.valid()) {
                let submitButton = $(this).find("button[type='submit']");
                $(submitButton).prop("disabled", "disabled").addClass("disabled");
                $("#submitSpinner").show();
            }
        });
    }

}());