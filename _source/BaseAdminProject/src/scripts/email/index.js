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
        initEmailForm();
    }

    function initEmailForm() {
        $("#emailForm").submit(function (e) {
            let form = $(this);
            if (form.valid()) {
                let submitButton = $(this).find("button[type='submit']");
                let submitButtonClicked = $(this).find("button[type='submit']:focus");
                $(submitButton).prop("disabled", "disabled").addClass("disabled");
                $(submitButtonClicked).children("span").removeClass("d-none");
            }
        });
    }

}());