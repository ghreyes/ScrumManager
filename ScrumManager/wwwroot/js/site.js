// Please see documentation at https://docs.microsoft.com/aspnet/core/client-side/bundling-and-minification
// for details on configuring this project to bundle and minify static web assets.

// Write your JavaScript code.

// Enable all tooltips
$(function () {
  $('[data-toggle="tooltip"]').tooltip()
})

$('.post-ajax').on('submit', function (e) {
    e.preventDefault();
    $.ajax({
        type: "POST",
        url: e.currentTarget.action,
        data: $(this).serialize(),
        success: function () {
            alert('success');
        }
    });
});