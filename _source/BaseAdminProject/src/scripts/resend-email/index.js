"use strict";
import "jquery-validation";
import "jquery-validation-unobtrusive";

export default (function () {

    $(function () {
        initPage();
    });

    function initPage() {
        initResendEmailForm();
    }

    function initResendEmailForm() {
        global.eyePassword();

        $("#resendEmailForm").submit(function (e) {
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