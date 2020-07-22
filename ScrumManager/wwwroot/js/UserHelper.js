$('.GroupUser_AddBtn').on('click', function (e) {
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

    // Input not email format
    if (!ValidateEmail(email)) {
        toastr.warning("Please enter a valid email");
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
                newRow.find('.displayName').val(data.displayName).attr('name', 'Users[' + uid + '][DisplayName]');
                newRow.find('.emailIcon').remove();
                removeBtn.data('id', uid);
            }
            else {
                newRow.data('id', email);
                newRow.find('.nameText').text(email);
                newRow.find('.roles').val(role).attr('name', 'Invites[' + email + '][Roles]');
                newRow.find('.displayName').val(email).attr('name', 'Invites[' + email + '][Email]');
                newRow.find('.emailIcon').tooltip();
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

    var itemToRemove = $(datacontainer).children().filter(function () { return $(this).data('id') == id });
    if (itemToRemove.data('existing') != null) {
        if (confirm('Are you sure you want to remove this user?')) itemToRemove.remove();
    }
    else {
        itemToRemove.remove();
    }
});

$('#CreateGroupForm,#EditUsersForm').on('keydown', 'input.no-enter', function (e) {
    if (e.key == 'Enter') {
        e.preventDefault();
        var btn = $(this).parent().find('.GroupUser_AddBtn');
        btn.trigger('click');
    }
});