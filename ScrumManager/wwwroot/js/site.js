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