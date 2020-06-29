$('#SignInForm').on('submit', function (e) {
    e.preventDefault();
    if (!$(this).valid()) return;

    $('#SignInForm [type="submit"]').prop('disabled', true);

    var email = $('#SignInForm [name="Email"]').val();
    var password = $('#SignInForm [name="Password"]').val();

    firebase.auth().signInWithEmailAndPassword(email, password)
        .then(function (result) {
            firebase.auth().currentUser.getIdToken()
                .then(function (token) {
                    $.ajax({
                        type: "POST",
                        url: "/SignInUser",
                        data: {
                            'Token': token
                        },
                        success: function (data) {
                            window.location.replace(data.redirect);
                        },
                        error: function (error) {
                            toastr.error(error.message);
                            $('#SignInForm [type="submit"]').prop('disabled', false);
                        }
                    });
                })
                .catch(function (error) {
                    toastr.error(error);
                    $('#SignInForm [type="submit"]').prop('disabled', false);
                });
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
            $('#SignInForm [type="submit"]').prop('disabled', false);
        });
});

$('#GoogleSignIn').click(function () {
    var provider = new firebase.auth.GoogleAuthProvider();
    provider.addscope('profile');
    provider.addscope('email');

    firebase.auth().signInWithPopup(provider).then(function (result) {
        var token = result.credential.accessToken;
        var user = result.user;
    }).catch(function (error) {
        var errorCode = error.code;
        var errorMessage = error.message;
        var email = error.email;
        var credential = error.credential;
        toastr.error(error.message);
    });
});