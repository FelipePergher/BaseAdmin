"use strict";
import "jquery-validation";
import "jquery-validation-unobtrusive";
import "jquery-mask-plugin";
import { SetupValidator, Notify } from '../common/common';

export default (function () {

    SetupValidator($.validator);

    $(function () {
        initPage();
    });

    function initPage() {
        phoneForm();
        usernameForm();
        sendVerificationEmail();
        emailForm();
    }

    function phoneForm() {
        $("#phoneForm").submit(function (e) {
            e.preventDefault();
            const form = $(this);
            if (form.valid()) {
                const submitButton = form.find("button[type='submit']");
                $(submitButton).prop("disabled", "disabled").addClass("disabled");
                $(submitButton).children("span").removeClass("d-none").show();

                $.post($(this).attr("action"), form.serialize())
                    .done(function (data) {
                        if (!!data.message && !!data.type) {
                            Notify(data.type, data.message);
                        }
                        else {
                            $("#phoneFormWrapper").html(data);
                            $.validator.unobtrusive.parse("#phoneForm");
                            phoneForm();
                        }
                    })
                    .fail(function (data) {
                        Notify("danger", data.responseText);
                    })
                    .always(function () {
                        $(submitButton).removeAttr("disabled").removeClass("disabled");
                        $(submitButton).children("span").fadeOut();
                    });
            }
        });
    }

    function usernameForm() {
        $("#userNameInput").blur().focus();

        $("#usernameForm").submit(function (e) {
            e.preventDefault();
            const form = $(this);
            if (form.valid()) {
                const submitButton = form.find("button[type='submit']");
                $(submitButton).prop("disabled", "disabled").addClass("disabled");
                $(submitButton).children("span").removeClass("d-none").show();

                $.post($(this).attr("action"), form.serialize())
                    .done(function (data) {
                        if (data.message) {
                            Notify(data.type, data.message);
                        }
                        else {
                            $("#usernameFormWrapper").html(data);
                            $.validator.unobtrusive.parse("#usernameForm");
                            usernameForm();
                        }
                    })
                    .fail(function (data) {
                        Notify("danger", data.responseText);
                    })
                    .always(function () {
                        $(submitButton).removeAttr("disabled").removeClass("disabled");
                        $(submitButton).children("span").fadeOut();
                    });
            }
        });
    }

    function sendVerificationEmail() {
        $("#sendVerificationEmail").click(function () {
            const button = $(this);
            $(button).prop("disabled", "disabled").addClass("disabled");
            $(button).children("span").removeClass("d-none").show();

            $.post($(this).attr("formaction"))
                .done(function (data) {
                    Notify(data.type, data.message);
                })
                .fail(function (data) {
                    Notify("danger", data.responseText);
                })
                .always(function () {
                    $(button).removeAttr("disabled").removeClass("disabled");
                    $(button).children("span").fadeOut();
                });
        });
    }

    function emailForm() {
        $("#emailInput").blur().focus();

        $("#emailForm").submit(function (e) {
            e.preventDefault();
            const form = $(this);
            if (form.valid()) {
                const submitButton = form.find("button[type='submit']");
                $(submitButton).prop("disabled", "disabled").addClass("disabled");
                $(submitButton).children("span").removeClass("d-none").show();

                $.post($(this).attr("action"), form.serialize())
                    .done(function (data) {
                        if (!!data.message && !!data.type) {
                            Notify(data.type, data.message);
                        }
                        else {
                            $("#emailFormWrapper").html(data);
                            $.validator.unobtrusive.parse("#emailForm");
                            emailForm();
                        }
                    })
                    .fail(function (data) {
                        Notify("danger", data.responseText);
                    })
                    .always(function () {
                        $(submitButton).removeAttr("disabled").removeClass("disabled");
                        $(submitButton).children("span").fadeOut();
                    });
            }
        });
    }

}());
