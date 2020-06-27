"use strict";
import "jquery-validation";
import "jquery-validation-unobtrusive";
import { SetupValidator, EyePassword } from '../common/common';

export default (function () {

    SetupValidator($.validator);

    $(function () {
        initPage();
    });

    function initPage() {
        initLoginForm();
    }

    function initLoginForm() {
        EyePassword();

        $("#loginForm").submit(function (e) {
            const form = $(this);
            if (form.valid()) {
                const submitButton = $(this).find("button[type='submit']");
                $(submitButton).prop("disabled", "disabled").addClass("disabled");
                $(submitButton).children("span").removeClass("d-none");
            }
        });
    }

}());
