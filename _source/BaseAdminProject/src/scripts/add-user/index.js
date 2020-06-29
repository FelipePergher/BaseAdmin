"use strict";
import "jquery-validation";
import "jquery-validation-unobtrusive";
import "jquery-mask-plugin";
import { SetupValidator, EyePassword, InitCalendar } from "../common/common";

export default (function () {
    SetupValidator($.validator);

    $(function () {
        initPage();
    });

    function initPage () {
        initLoginForm();
    }

    function initLoginForm () {
        EyePassword();

        InitCalendar("#birthdayDate");

        $("#addUserForm").submit(function () {
            const form = $(this);
            if (form.valid()) {
                const submitButton = $(this).find("button[type='submit']");
                $(submitButton).prop("disabled", "disabled").addClass("disabled");
                $(submitButton).children("span").removeClass("d-none");
            }
        });
    }
}());
