"use strict";
import "jquery-validation";
import "jquery-validation-unobtrusive";
import { SetupValidator } from "../common/common";

export default (function () {
    SetupValidator($.validator);

    $(function () {
        initPage();
    });

    function initPage () {
        initResendEmailForm();
    }

    function initResendEmailForm () {
        $("#resendEmailForm").submit(function () {
            const form = $(this);
            if (form.valid()) {
                const submitButton = $(this).find("button[type='submit']");
                $(submitButton).prop("disabled", "disabled").addClass("disabled");
                $(submitButton).children("span").removeClass("d-none");
            }
        });
    }
}());
