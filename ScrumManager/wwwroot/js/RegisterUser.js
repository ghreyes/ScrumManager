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
            toastr.success('user created');
            // Firestore user created using Firebase cloud function
            // PUT REDIRECT HERE

            //var userUID = result.user.uid;

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
        }).catch(function (error) {
            toastr.error(error.message);
        });
});