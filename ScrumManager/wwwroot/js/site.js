// Please see documentation at https://docs.microsoft.com/aspnet/core/client-side/bundling-and-minification
// for details on configuring this project to bundle and minify static web assets.

// Write your JavaScript code.

// Enable all tooltips
$(function () {
  $('[data-toggle="tooltip"]').tooltip()
})

// Toastr setup
toastr.options = {
    "closeButton": true,
    "debug": false,
    "newestOnTop": false,
    "progressBar": true,
    "positionClass": "toast-top-center",
    "preventDuplicates": false,
    "onclick": null,
    "showDuration": "3000",
    "hideDuration": "3000",
    "timeOut": "3000",
    "extendedTimeOut": "3000",
    "showEasing": "swing",
    "hideEasing": "linear",
    "showMethod": "fadeIn",
    "hideMethod": "fadeOut"
}

$('.post-ajax').on('submit', function (e) {
    e.preventDefault();
    if (!$(this).valid()) return;
    $.ajax({
        type: "POST",
        url: e.currentTarget.action,
        data: $(this).serialize(),
        success: function () {
            toastr.success('Success');
            $('.modal').modal('hide');
        },
        error: function () {
            toastr.warning("Something went wrong. Try again.");
        }
    });
});

$('.post-ajax-and-reload').on('submit', function (e) {
    e.preventDefault();
    if (!$(this).valid()) return;
    $.ajax({
        type: "POST",
        url: e.currentTarget.action,
        data: $(this).serialize(),
        success: function () {
            toastr.success('Success');
            $('.modal').modal('hide');
            location.reload();
        },
        error: function () {
            toastr.warning("Something went wrong. Try again.");
        }
    });
});

$('#UpdateLogForm').on('submit', function (e) {
    e.preventDefault();
    if (!$(this).valid()) return;
    $.ajax({
        type: "POST",
        dataType: "json",
        url: e.currentTarget.action,
        data: $(this).serialize(),
        success: function (log) {
            toastr.success('Success');
            $('#yesterdayTA').data('revert', log.yesterday);
            $('#todayTA').data('revert', log.today);
            $('#blockersTA').data('revert', log.blockers);

            $('#variableFormDetails').empty();
            $('#variableFormDetails').append('<input hidden name="DocId" value="' + log.docId + '" />')
            //$('#variableFormDetails').append('<input hidden name="Date" value="' + log.date_datetime + '" />')
            $('#variableFormDetails').append('<input hidden name="UserName" value="' + log.userName + '" />')
            $('#variableFormDetails').append('<input hidden name="UserID" value="' + log.userID + '" />')

        },
        error: function () {
            toastr.warning("Something went wrong. Try again.");
        }
    });
});

const daysOfTheWeek = ['Sunday', 'Monday', 'Tuesday', 'Wednesday', 'Thursday', 'Friday', 'Saturday'];

$('.dateChange').on('click', function (e) {
    if(!VerifyInputDiffs()) return;
    const change = this.dataset.datechange;
    if (!change) return;

    var currDate = new Date($('#dateText').text());
    change === '+' ?
        currDate.setDate(currDate.getDate() + 1) :
        currDate.setDate(currDate.getDate() - 1)

    $('#dateText').text(currDate.toLocaleDateString());
    $('#dayText').text(daysOfTheWeek[currDate.getDay()]);

    ChangeListener(currDate);

    //if ($('input[name="FormDate"]').length == 0)
    //    $('#variableFormDetails').append('<input hidden name="FormDate" />')
    //var inputs = $('input[name="FormDate"]');
    //inputs.val(currDate.toLocaleDateString());
});

function VerifyInputDiffs() {
    // check if textareas are different than their data-revert values
    // if diff, send confirmation 
    // if same, return ok
    if (!$('.check-value').data() || !$('.check-value').data('revert')) return true;

    var ret = true;
    $('.check-value').each(function (x) {
        if ($(this).data('revert') != $(this).val() && $(this).data('revert') != '') {
            if (confirm('Are you sure you want to discard your changes?'))
                return;
            else {
                ret = false;
                return;
            }
        }
    });
    return ret;
}

$('#SignOut').on('click', function (e) {
    firebase.auth().signOut();
});

function ValidateEmail(email) {
    return /^\w+([\.-]?\w+)*@\w+([\.-]?\w+)*(\.\w{2,3})+$/.test(email);
}

$('#DailyExportForm').on('submit', function () {
    $('.modal').modal('hide');
});