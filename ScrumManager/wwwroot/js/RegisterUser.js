$('#RegisterForm').on('submit', function (e) {
    e.preventDefault();
    if (!$(this).valid()) return;

    var email = $('#RegisterForm [name="Email"]').val();
    var password = $('#RegisterForm [name="Password"]').val();
    var fName = $('#RegisterForm [name="FirstName"]').val();
    var lName = $('#RegisterForm [name="LastName"]').val();

    firebase.auth().createUserWithEmailAndPassword(email, password)
        .then(function (result) {
            var user = firebase.auth().currentUser;

            user.updateProfile({
                displayName: fName + ' ' + lName
            });
        }).then(function () {
            $.ajax({
                type: "POST",
                url: "/RegisterUser",
                data: {
                    'DocId': firebase.auth().currentUser.uid,
                    'DisplayName': fName + ' ' + lName,
                    'Email': email
                },
                success: function (user) {

                },
                error: function (error) {
                    toastr.error(error.message);
                }
            });

            window.location.href = "/Home";
        }).catch(function (error) {
            toastr.error(error.message);
        });
});