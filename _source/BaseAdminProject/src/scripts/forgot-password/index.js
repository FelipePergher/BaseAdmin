"use strict";
import "jquery-validation";
import "jquery-validation-unobtrusive";

export default (function () {

    $(function () {
        initPage();
    });

    function initPage() {
        initForgotForm();
    }

    function initForgotForm() {
        $("#forgotPasswordForm").submit(function (e) {
            let form = $(this);
            if (form.valid()) {
                //Todo spinner
                let submitButton = $(this).find("button[type='submit']");
                $(submitButton).prop("disabled", "disabled").addClass("disabled");
                $("#submitSpinner").show();
            }
        });
    }

}());