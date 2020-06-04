"use strict";
import "jquery-validation";
import "jquery-validation-unobtrusive";
import "jquery-mask-plugin";
import { SetupValidator } from '../common/common';

export default (function () {

    SetupValidator($.validator);

    $(function () {
        initPage();
    });

    function initPage() {
        initProfileForm();
    }

    function initProfileForm() {
        $("#usernameForm, #phoneForm").submit(function (e) {
            let form = $(this);
            if (form.valid()) {
                let submitButton = $(this).find("button[type='submit']");
                $(submitButton).prop("disabled", "disabled").addClass("disabled");
                $(submitButton).children("span").removeClass("d-none");
            }
        });
    }

}());