$('#RegisterForm').on('submit', function (e) {
    e.preventDefault();
    if (!$(this).valid()) return;

    var email = $('#RegisterForm [name="Email"]').val();
    var password = $('#RegisterForm [name="Password"]').val();

    firebase.auth().createUserWithEmailAndPassword(email, password)
    .then(function (result) {

    })
    .catch(function (error) {
        toastr.error(error.message);
    });

    //$.ajax({
    //    type: "POST",
    //    dataType: "json",
    //    url: e.currentTarget.action,
    //    data: $(this).serialize(),
    //    success: function (log) {
    //        toastr.success('Success');
    //        $('#yesterdayTA').data('revert', log.yesterday);
    //        $('#todayTA').data('revert', log.today);
    //        $('#blockersTA').data('revert', log.blockers);

    //        $('#variableFormDetails').empty();
    //        $('#variableFormDetails').append('<input hidden name="DocId" value="' + log.docId + '" />')
    //        //$('#variableFormDetails').append('<input hidden name="Date" value="' + log.date_datetime + '" />')
    //        $('#variableFormDetails').append('<input hidden name="UserName" value="' + log.userName + '" />')
    //        $('#variableFormDetails').append('<input hidden name="UserID" value="' + log.userID + '" />')

    //    },
    //    error: function () {
    //        toastr.warning("Something went wrong. Try again.");
    //    }
    //});
});