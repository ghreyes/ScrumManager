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




$('.CreateGroupForm_AddButton').on('click', function (e) {
    var textfield = $(this).data('textfield');
    var datacontainer = $(this).data('datacontainer');
    var message = $(this).data('message');
    var role = $(this).data('role');
    var datacontainer = $(this).data('datacontainer');
    var message = $(this).data('message');

    var email = $('#' + textfield).val();

    // Invite already added (Email)
    if ($('#' + datacontainer).children().filter(function () { return $(this).data('id') == email }).length > 0) {
        toastr.info(message);
        return;
    }

    // Need ajax call to check if email is tied to user
    // If so, Users[UID]
    // If not, Invites[Email]
    $.ajax({
        type: "POST",
        url: "/User/GetUserByEmail",
        data: {
            'Email': email
        },
        success: function (data) {
            var uid = data.docId;

            // User already added (UID)
            if ($('#' + datacontainer).children().filter(function () { return $(this).data('id') == uid }).length > 0) {
                toastr.info(message);
                return;
            }

            var newRow = $('#userTemplate').clone().removeAttr('id');
            var removeBtn = newRow.find('.remove');
            removeBtn.data('datacontainer', datacontainer);

            if (uid != null && uid.length > 0) {
                newRow.data('id', uid);
                newRow.find('.nameText').text(data.displayName);
                newRow.find('.roles').val(role).attr('name', 'Users[' + uid + '][Roles]');
                newRow.find('.displayName').val(uid).attr('name', 'Users[' + data.displayName + '][DisplayName]');
                removeBtn.data('id', uid);
            }
            else {
                newRow.data('id', email);
                newRow.find('.nameText').text(email);
                newRow.find('.roles').val(role).attr('name', 'Invites[' + email + '][Roles]');
                newRow.find('.displayName').val(email).attr('name', 'Invites[' + email + '][DisplayName]');
                removeBtn.data('id', email);
            }

            newRow.appendTo('#' + datacontainer).removeAttr('hidden');
            $('#' + textfield).val('');
        },
        error: function () {
            toastr.warning("Something went wrong. Try again.");
        }
    });
});

$('body').on('click', '.remove', function (e) {
    var datacontainer = '#' + $(this).data('datacontainer');
    var id = $(this).data('id');

    $(datacontainer).children().filter(function () { return $(this).data('id') == id }).remove();
});