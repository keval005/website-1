$(document).ready(function () {

    // this will get the full URL at the address bar
    var url = window.location.href;
    // hightlight active link
    $("#navLinks li a").each(function () {
        // checks if its the same on the address bar
        if (url == (this.href)) {
            $(this).addClass("active");
        }
    });

    $(window).scroll(function () {
        if (this.scrollY > 20) {
            $('.transparentNavbar.navbar').addClass("navbar-bg-color");
        }
        else {
            $('.transparentNavbar.navbar').removeClass("navbar-bg-color");
        }
        if (this.scrollY > 500) {
            $('.scroll-up-btn').addClass("show");
        } else {
            $('.scroll-up-btn').removeClass("show");
        }
    })

    //collapse(hide navbar) when scrolling
    $(window).scroll(function () {

        $(".navbar .navbar-collapse").removeClass("show");
        $('.backblack').removeClass("open");

    });

    //on model open   close sidebar(navbar) for smaller devices
    $(".modal").on('show.bs.modal', function () {
        $(".navbar .navbar-collapse").removeClass("show");
        $('.backblack').removeClass("open");
    });

    //Login and ForgotPassword Model
    $('#loginModal').modal({
        backdrop: 'static',
        keyboard: false
    });
    $('#forgotPasswordModal').modal({
        backdrop: 'static',
        keyboard: false
    });

    //open login model if return queryString is present in url
    var url = window.location.href.slice(window.location.href.indexOf('?') + 1);
    var urlParam = url.split('=');
    //checking returnUrl queryString is present or not
    if (urlParam[0] == "returnUrl") {
        $('#loginModal').modal('show');
    }

})

//black backgroung on open side navbar
$('.navbar-toggler').click(function () {
    $('.backblack').addClass("open");
});
$('.backblack').click(function () {
    $(".navbar .navbar-collapse").removeClass("show");
    $('.backblack').removeClass("open");
});

// top Arrow
$('.top-arrow').click(function () {
    $('html').animate({ scrollTop: 0 })
});

//Attachment name in input countect us
$('#inputAttachment').change(function (e) {
    var fileName = e.target.files[0].name;
    document.getElementById("lblAttachmentName").innerHTML = fileName;
});
//on click of input type file --> label is set to null/empty...
$('#inputAttachment').click(function () {
    document.getElementById("lblAttachmentName").innerHTML = "";
});

var RegExpressEmail = new RegExp("^[a-zA-Z0-9_.+-]+@[a-zA-Z0-9-]+\.[a-zA-Z0-9-.]+$");

function Login() {
    if ($("#loginEmail").val() == "") {
        $('#loginEmailErrorMessage').html("Please Enter Email");
    }
    else if (!RegExpressEmail.test($("#loginEmail").val())) {
        $('#loginEmailErrorMessage').html("Please Enter Valid Email");
    }
    else {
        $('#loginEmailErrorMessage').html("");
    }

    if ($("#loginPassword").val() == "") {
        $('#loginPasswordErrorMessage').html("Please Enter Password");
    }
    else {
        $('#loginPasswordErrorMessage').html("");
    }

    if ($('#loginEmailErrorMessage').text() != "" || $("#loginPasswordErrorMessage").text() != "") {
        return;
    }

    var loginDetail = {};

    loginDetail.email = $("#loginEmail").val();
    loginDetail.password = $("#loginPassword").val();
    loginDetail.rememberMe = $('#LoginRememberMe').prop('checked');

    var url = window.location.href.slice(window.location.href.indexOf('?') + 1);
    var urlParam = url.split('=');

    var returnUrl = "";
    //checking returnUrl queryString is present or not
    if (urlParam[0] == "returnUrl") {
        returnUrl = decodeURIComponent(urlParam[1]);
    }

    $('#preloader').removeClass("d-none");

    $.ajax({
        url: '/Account/Login',
        type: 'post',
        dataType: 'json',
        contentType: 'application/json',
        data: JSON.stringify(loginDetail),
        success: function (resp) {

            $('#preloader').addClass("d-none");

            if (resp.status == "ok") {
                if (returnUrl == "") {
                    window.location.href = "http://" + window.location.host + "/" + resp.url;
                }
                else {
                    window.location.href = window.location.href.split("?")[0] + returnUrl;
                }
            }
            else if (resp.status == "Error") {
                BootstrapAlert("divLoginErrorMessage", resp.errorMessage, "danger");
            }

        },
        error: function (err) {
            console.log(err);
        }
    });
}

function ForgotPassword() {
    if ($("#ForgotPasswordEmail").val() == "") {
        $('#ForgotPasswordEmailErrorMessage').html("Please Enter Email");
        return;
    }
    else if (!RegExpressEmail.test($("#ForgotPasswordEmail").val())) {
        $('#ForgotPasswordEmailErrorMessage').html("Please Enter Valid Email");
        return;
    }
    else {
        $('#ForgotPasswordEmailErrorMessage').html("");
    }

    forgotPasswordDetail = {}
    forgotPasswordDetail.email = $("#ForgotPasswordEmail").val();

    $('#preloader').removeClass("d-none");

    $.ajax({
        url: '/Account/ForgotPassword',
        type: 'post',
        dataType: 'json',
        contentType: 'application/json',
        data: JSON.stringify(forgotPasswordDetail),
        success: function (resp) {

            $('#preloader').addClass("d-none");
            
            if (resp.status == "ok") {
                $('#forgotPasswordModal').modal('hide');

                $("#divForgotPasswordErrorMessage").html("");
                $("#ForgotPasswordEmail").val("");
                
                Swal.fire({
                    icon: 'success',
                    title: 'Forgot Password',
                    text: 'You will receive an email with further instructions on how to reset your password.',
                    allowOutsideClick: false,
                    allowEscapeKey: false
                })
            }
            else if (resp.status == "Error") {
                BootstrapAlert("divForgotPasswordErrorMessage", resp.errorMessage, "danger");
            }

        },
        error: function (err) {
            console.log(err);
        }
    });
}

//Bootstrap alert
function BootstrapAlert(id, message, type) {
    var wrapper = document.createElement('div')
    wrapper.innerHTML = '<div class="alert alert-' + type + ' alert-dismissible" role="alert">' + message + '<button type="button" class="btn-close" data-bs-dismiss="alert" aria-label="Close"></button></div>'
    $('#' + id).html(wrapper);
}