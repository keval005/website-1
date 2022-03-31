$(document).ready(function () {
    ManageBookServiceTab();
});

//Global Variables
var _serviceHourlyRate = 18.0;
var _basicServiceHour = 3;
var _extraServiceCount = 0;
var _logInUserId = 0;
var _oldServiceHour = 0;


//Manage Tab - Fill, Active, Disabled
function ManageBookServiceTab() {
    var bookServiceTabs = document.querySelectorAll('#book-service-tab button');
    var activeTab = false;
    for (const tab of bookServiceTabs) {
        if ($("#" + tab.id).hasClass("active")) {
            activeTab = true;
        }
        else if (activeTab == false) {
            $("#" + tab.id).addClass("fill");
            $("#" + tab.id).removeClass("disabled");
        }
        else if (activeTab == true) {
            $("#" + tab.id).addClass("disabled");
            $("#" + tab.id).removeClass("fill");
        }
    }
}

function ScrollToBookServiceTab() {
    $('html,body').animate({ scrollTop: $("#book-service-tab").offset().top - 215 }, 300);
}

//Reset Tabs Start

function SetupServiceTabClicked() {
    ManageBookServiceTab();
    ResetScheduleAndPlanTab();
    ResetYourDetailsTab();
    ResetMakePaymentTab();
}

function ScheduleAndPlanTabClicked() {
    ManageBookServiceTab();
    ResetYourDetailsTab();
    ResetMakePaymentTab();
}

function YourDetailsTabClicked() {
    ManageBookServiceTab();
    ResetMakePaymentTab();
}

//Reset Tab - 2 : Schedule & plan - start
function ResetScheduleAndPlanTab() {
    _basicServiceHour = 3;

    var date = new Date();
    date.setDate(date.getDate() + 1);
    var nextDate = date.getFullYear().toString() + "-" + AppendZero((date.getMonth() + 1).toString()) + "-" + AppendZero(date.getDate().toString());

    //set value to input type date for ServiceDate  
    $('#ServiceDate').val(nextDate);

    $("#ServiceTime").val($("#ServiceTime option:first").val());
    $("#ServiceHours").val($("#ServiceHours option:first").val());
    $("#txtComment").val("");
    $("#chkHasPet").prop("checked", false);

    ResetExtraServicesScheduleAndPlanTab();

    FillServiceDateTimePaymentSummary();
    $('#lblBasicServiceHours').html(_basicServiceHour + " Hrs");
    TotalPayment();
}

function ResetExtraServicesScheduleAndPlanTab() {
    _extraServiceCount = 0;
    $('input[name="ExtraServices"]').each(function () {
        this.checked = false;
    });
    $('#lblExtraServices').html("");
    $('#lblExtraServices').addClass('d-none');
}
//Reset Tab - 2 : Schedule & plan - end

//Reset Tab - 2 : Schedule & plan - start
function ResetYourDetailsTab() {
    $('input:radio[name=UserAddressListYourDetailTab]:checked').prop('checked', false);
    ResetAddressBoxYourDetailsTab();
}

function ResetAddressBoxYourDetailsTab() {
    $("#UserStreetName").val("");
    $("#UserHouseNumber").val("");
    $("#UserPhoneNumber").val("");
    $("#UserCity").val($("#UserCity option:first").val());
}
//Reset Tab - 3 : Schedule & plan - end

//Reset Tab - 4 : Make Payment - start
function ResetMakePaymentTab() {
    $("#PromoCode").val("");
    $("#CardNumber").val("");
    $("#CardExpiryDate").val("");
    $("#CardCVC").val("");
    $("#TermsAndCondition").prop("checked", false);
}
//Reset Tab - 4 : Make Payment - end

//Reset Tabs End

//First Tab - Setup Service -- start
function CheckPostalCode() {

    var PostalCode = $("#PostalCode").val();
    var RegExpressPostalCode = new RegExp("^[1-9][0-9]{5}$");

    if (PostalCode == "") {
        $('#PostalCodeErrorMessage').html("Please Enter Postal Code.");
        return;
    }
    else if (!RegExpressPostalCode.test(PostalCode)) {
        $('#PostalCodeErrorMessage').html("Please Enter Valid Postal Code.");
        return;
    }
    else {
        $.ajax({
            url: '/Home/CheckPostalCode',
            type: 'post',
            dataType: 'json',
            data: { "PostalCode": PostalCode },
            success: function (resp) {
                if (resp) {
                    $('#Schedule-and-Plan-tab').tab('show');
                    ManageBookServiceTab();
                    ScrollToBookServiceTab();

                    var date = new Date();
                    date.setDate(date.getDate() + 1);
                    var nextDate = date.getFullYear().toString() + "-" + AppendZero((date.getMonth() + 1).toString()) + "-" + AppendZero(date.getDate().toString());

                    //set value to input type date for ServiceDate  
                    $('#ServiceDate').val(nextDate);
                    $('#ServiceDate').attr("min", nextDate);

                    FillServiceDateTimePaymentSummary();

                    $('#lblBasicServiceHours').html(_basicServiceHour + " Hrs");

                    TotalPayment();

                    $('#PostalCodeErrorMessage').html("");
                }
                else {
                    $('#PostalCodeErrorMessage').html("We are not providing service in this area. We’ll notify you if any helper would start working near your area.");
                }
            },
            error: function (err) {
                console.log(err);
            }
        });
    }
}

//appent 0 to single digit number for month and date
function AppendZero(input) {
    if (input.length == 1) {
        return '0' + input;
    }
    return input;
}
//First Tab - Setup Service -- end

//Second Tab - Schedule & plan -- start
function CheckServiceTimeLimit() {
    var totalHour = 0;
    startHour = parseFloat($('#ServiceTime').val());
    totalHour = _basicServiceHour + (_extraServiceCount * 0.5);
    if ((startHour + totalHour) > 21) {
        $('#lblServiceTimeErrorMessage').html('Could not completed the service request, because service booking request is must be completed within 21:00 time');
        $('#lblServiceTimeErrorMessage').removeClass('d-none');
    }
    //else if (_basicServiceHour < 3 || (_basicServiceHour + (_extraServiceCount * 0.5) > 12)) {
    else if (_basicServiceHour < 3) {
        $('#ConfirmServiceTimeModal').modal({
            backdrop: 'static',
            keyboard: false
        });
        $('#ConfirmServiceTimeModal').modal('show');
    }
    else {
        $('#lblServiceTimeErrorMessage').html('');
        $('#lblServiceTimeErrorMessage').addClass('d-none');
    }
}

function ScheduleAndPlanCompeleted(userId) {
    if ($('#lblServiceTimeErrorMessage').html() == "") {
        $('#Your-Details-tab').tab('show');
        ManageBookServiceTab();
        ScrollToBookServiceTab();

        _logInUserId = userId;

        //Load log in user Address List And City DropDown in your Detail tab 
        FillCustomerAddressList();
        FillCityDropDown();

        //clear div error message for your Detail Tab
        $('#Your-Details-tabContent-ErrorMessage').html("");
    }
}

function ConfirmServiceTime() {

    if (_basicServiceHour < 3 && $('#ServiceHours').val() == 3) {

        _basicServiceHour = 3;
        ResetExtraServicesScheduleAndPlanTab();
    }
    else {
        $('#ServiceHours').val(_oldServiceHour);
        _basicServiceHour = parseFloat(_oldServiceHour) - (_extraServiceCount * 0.5);
        TotalPayment();
    }
    $('#ConfirmServiceTimeModal').modal('hide');
    $('#lblBasicServiceHours').html(_basicServiceHour + " Hrs");
}

function CancelConfirmServiceTime() {
    $('#ServiceHours').val(_oldServiceHour);
    _basicServiceHour = _oldServiceHour - (_extraServiceCount * 0.5);
    $('#lblBasicServiceHours').html(_basicServiceHour + " Hrs");
    TotalPayment();
}
//Second Tab - Schedule & plan -- end

//3rd Tab - Your Detail -- start
function FillCustomerAddressList() {
    $("#customerAddressList").html("Loading Addresses...").load('/Home/GetCustomerAddressList?userId=' + _logInUserId + '&postalCode=' + $('#PostalCode').val());
    $('#UserPostalCode').val($('#PostalCode').val());
}

function FillCityDropDown() {
    var postalCode = $('#PostalCode').val();

    $('#preloader').removeClass("d-none");

    $.ajax({
        url: '/Home/GetCitiesByPostalCode',
        type: 'post',
        dataType: 'json',
        data: { "postalCode": postalCode },
        success: function (resp) {

            $('#preloader').addClass("d-none");

            $('#UserCity').empty();
            resp.forEach((city) => {
                $('#UserCity').append($("<option></option>").val(city.cityName).html(city.cityName));
            });
        },
        error: function (err) {
            console.log(err);
        }
    });
}

function AddressBox(show) {
    if (show == true) {
        $('#btn-Add-address').hide();
        $('#address-form').show();
    }
    else {
        $('#address-form').hide();
        $('#btn-Add-address').show();
    }
}

function AddNewCustomerAddress() {
    var regularExpressionPhoneNumber = new RegExp("^[0-9]{10}$");

    if ($('#UserStreetName').val() == '') {
        $('#ErrorMessageUserStreetName').html("Please Enter Street Name");
    }
    else {
        $('#ErrorMessageUserStreetName').html("");
    }

    if ($('#UserPhoneNumber').val() != "") {
        if (!regularExpressionPhoneNumber.test($('#UserPhoneNumber').val())) {
            $('#ErrorMessageUserPhoneNumber').html("Please Enter Valid Phone number.");
        }
        else {
            $('#ErrorMessageUserPhoneNumber').html("");
        }
    }
    else {
        $('#ErrorMessageUserPhoneNumber').html("");
    }

    if ($('#ErrorMessageUserStreetName').text() != "" || $('#ErrorMessageUserPhoneNumber').text() != "") {
        return;
    }
    var userAddress = {};

    userAddress.streetName = $("#UserStreetName").val();
    userAddress.houseNumber = $("#UserHouseNumber").val();
    userAddress.postalCode = $("#UserPostalCode").val();
    userAddress.city = $("#UserCity").val();
    userAddress.phoneNumber = $("#UserPhoneNumber").val();

    $('#preloader').removeClass("d-none");

    $.ajax({
        url: '/Home/AddCustomerAddress',
        type: 'post',
        dataType: 'json',
        contentType: 'application/json',
        data: JSON.stringify(userAddress),
        success: function (resp) {

            $('#preloader').addClass("d-none");

            ResetAddressBoxYourDetailsTab();
            AddressBox(false);
            FillCustomerAddressList();
        },
        error: function (err) {
            console.log(err);
        }
    });
}

function SelectCustomerAddress(isAddrssListAvailable) {
    if ($('#isUserAddressListAvailable').val() == 'false') {
        $('#Your-Details-tabContent-ErrorMessage').removeClass("d-none");
        BootstrapAlert('Your-Details-tabContent-ErrorMessage', "Please add an address", "danger");
    }
    else {
        if ($('input[name=UserAddressListYourDetailTab]:checked').length > 0) {
            $('#Make-Payment-tab').tab('show');
            ManageBookServiceTab();
            ScrollToBookServiceTab();

            $('#Your-Details-tabContent-ErrorMessage').addClass("d-none");
        }
        else {
            $('#Your-Details-tabContent-ErrorMessage').removeClass("d-none");
            BootstrapAlert('Your-Details-tabContent-ErrorMessage', "Please Select Address.", "danger");
            ScrollToBookServiceTab();
        }
    }
}
//3rd Tab - Your Detail -- end

//4th Tab - Make Payment -- Start

function CompleteBooking() {

    var cardNumberRegularExpressionPattern = new RegExp("^[0-9]{16}$");
    var cardCVCRegularExpressionPattern = new RegExp("^[0-9]{3}$");

    if ($("#CardNumber").val() == "") {
        $('#ErrorMessageCardNumber').html("Please Enter Card Number.<br/>");
    }
    else if (!cardNumberRegularExpressionPattern.test($("#CardNumber").val())) {
        $('#ErrorMessageCardNumber').html("Please Enter Valid 16 digit Card Number.<br/>");
    }
    else {
        $('#ErrorMessageCardNumber').html("");
    }

    if ($("#CardExpiryDate").val() == "") {
        $('#ErrorMessageCardExpiryDate').html("Please Enter Card Expiry Date.<br/>");
    }
    else {
        $('#ErrorMessageCardExpiryDate').html("");
    }

    if ($("#CardCVC").val() == "") {
        $('#ErrorMessageCardCVC').html("Please Enter Card CVC.<br/>");
    }
    else if (!cardCVCRegularExpressionPattern.test($("#CardCVC").val())) {
        $('#ErrorMessageCardCVC').html("Please Enter Valid 3 digit Card CVC.<br/>");
    }
    else {
        $('#ErrorMessageCardCVC').html("");
    }

    if ($('#TermsAndCondition').prop('checked') == false) {
        $('#ErrorMessageTermsAndCondition').html("(* Required field)");
    }
    else {
        $('#ErrorMessageTermsAndCondition').html("");
    }

    if ($('#ErrorMessageCardNumber').text() != "" || $("#ErrorMessageCardExpiryDate").text() != "" || $("#ErrorMessageCardCVC").text() != "" || $('#ErrorMessageTermsAndCondition').text() != "") {
        return;
    }
    
    var ServiceRequest = {};

    ServiceRequest.userId = _logInUserId;

    //Tab - 1
    ServiceRequest.postalCode = $("#PostalCode").val();

    //Tab - 2
    ServiceRequest.serviceStartDate = $("#ServiceDate").val();
    ServiceRequest.serviceStartTime = $("#ServiceTime option:selected").text(); //text value
    ServiceRequest.serviceHourlyRate = _serviceHourlyRate;
    ServiceRequest.serviceHours = _basicServiceHour + (_extraServiceCount * 0.5);
    ServiceRequest.extraHours = _extraServiceCount * 0.5;

    ServiceRequest.extraServicesName = [];

    $('input[name="ExtraServices"]').each(function () {
        if (this.checked) {
            ServiceRequest.extraServicesName.push(this.value);
        }
    });

    totalAmount = (_basicServiceHour + (_extraServiceCount * 0.5)) * _serviceHourlyRate;

    ServiceRequest.subTotal = totalAmount;
    ServiceRequest.totalCost = totalAmount;
    ServiceRequest.comments = $("#txtComment").val();
    ServiceRequest.hasPets = $('#chkHasPet').prop('checked');

    //Tab - 3

    ServiceRequest.userAddressId = $('input[name=UserAddressListYourDetailTab]:checked').attr("id");

    //Tab - 4

    ServiceRequest.paymentDone = true;

    $('#preloader').removeClass("d-none");

    $.ajax({
        url: '/Home/BookCustomerServiceRequest',
        type: 'post',
        dataType: 'json',
        contentType: 'application/json',
        data: JSON.stringify(ServiceRequest),
        success: function (resp) {

            $('#preloader').addClass("d-none");

            $("#lblServiceRequestId").html(resp.serviceRequestId);
            $('#BookServiceMessageModal').modal({
                backdrop: 'static',
                keyboard: false
            });
            $('#BookServiceMessageModal').modal('show');

        },
        error: function (err) {
            console.log(err);
        }
    });
}
//4th Tab - Make Payment -- end

//Fill Payment Summary
$('#ServiceDate').change(function () {
    FillServiceDateTimePaymentSummary();
});
$('#ServiceTime').change(function () {
    FillServiceDateTimePaymentSummary();
    CheckServiceTimeLimit();
});

function FillServiceDateTimePaymentSummary() {
    var ServiceDate = new Date($('#ServiceDate').val());
    $('#lblServiceDate').html(AppendZero(ServiceDate.getDate().toString()) + "/" + AppendZero((ServiceDate.getMonth() + 1).toString()) + "/" + ServiceDate.getFullYear() + " " + $('#ServiceTime option:selected').text());
}


$('#ServiceHours').focus(function () {
    _oldServiceHour = this.value;
});

$('#ServiceHours').change(function () {
    var serviceHour = parseFloat($('#ServiceHours').val());

    if (_extraServiceCount > 0) {
        serviceHour = serviceHour - (_extraServiceCount * 0.5);
    }

    _basicServiceHour = serviceHour;

    CheckServiceTimeLimit();
    TotalPayment();
    $('#lblBasicServiceHours').html(_basicServiceHour + " Hrs");
});

$('input[name="ExtraServices"]').change(function () {

    var htmlContent = '';
    var newExtraServiceCount = 0;

    $.each($("input[name='ExtraServices']:checked"), function () {
        htmlContent = htmlContent + '<p class="mb-1"><label>' + $(this).next('label').find("span.heading").text() + '</label ><label> 30 Mins</label > </p >';
        newExtraServiceCount = newExtraServiceCount + 1;
    });

    var totalHour = _basicServiceHour + (_extraServiceCount * 0.5);

    if (newExtraServiceCount > _extraServiceCount) { //on extra Service Added, Add it to total Hour
        totalHour = totalHour + ((newExtraServiceCount - _extraServiceCount) * 0.5);
    }
    else { //on extra Service remove, Substract it to total Hour
        totalHour = totalHour - ((_extraServiceCount - newExtraServiceCount) * 0.5);
    }

    _extraServiceCount = newExtraServiceCount;

    $("#ServiceHours").val(totalHour);

    if (htmlContent == "") {
        $('#lblExtraServices').addClass('d-none');
    }
    else {
        $('#lblExtraServices').removeClass('d-none');
    }
    $('#lblExtraServices').html('<p class="mb-1">Extras</p>' + htmlContent);

    TotalPayment();
    CheckServiceTimeLimit();
});

function TotalPayment() {
    var tatalHour = 0;
    var totalPayment = 0;

    tatalHour = _basicServiceHour + (_extraServiceCount * 0.5);
    totalPayment = tatalHour * _serviceHourlyRate;

    $('#lblTotalServiceTime').html(tatalHour + " Hrs");
    $('#lblPerCleaning').html("$ " + totalPayment);
    $('#lblTotalPayment').html("$ " + totalPayment);
}
