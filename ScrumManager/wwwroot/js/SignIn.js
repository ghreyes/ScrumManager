$('#SignInForm').on('submit', function (e) {
    e.preventDefault();
    if (!$(this).valid()) return;

    var email = $('#SignInForm [name="Email"]').val();
    var password = $('#SignInForm [name="Password"]').val();

    firebase.auth().signInWithEmailAndPassword(email, password)
        .then(function (result) {
            window.location.href = '/Home';
        }).catch(function (error) {
            switch (error.code) {
                case 'auth/wrong-password':
                    toastr.error('Incorrect password');
                    break;
                case 'auth/user-not-found':
                    toastr.error('User not found');
                    break;
                default:
                    toastr.error(error.message);
                    break;
            }
        });
});