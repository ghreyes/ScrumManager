"use strict";

var connection = new signalR.HubConnectionBuilder().withUrl("/logHub").configureLogging(signalR.LogLevel.Information).build();

connection.on("LogAdded", function (log) {
    // Already exists. Don't add.
    if ($('#logs-container').children('#' + log.userID).length)
        return;

    AddNewLog(log);
});

connection.on("LogModified", function (log) {
    // Doesn't exist so add new
    if (!$('#logs-container').children('#' + log.userID).length)
        AddNewLog(log);

    // Find and edit existing log
    var logRow = $('#logs-container').children('#' + log.userID)
    logRow.find('.log-username').text(log.userName);
    logRow.find('.log-yesterday').text(log.yesterday);
    logRow.find('.log-today').text(log.today);
    logRow.find('.log-blockers').text(log.blockers);
});

connection.on("LogRemoved", function (log) {
    // Doesn't exist. Skip
    if (!$('#logs-container').children('#' + log.userID).length)
        return;

    // Remove existing log
    $('#logs-container').children('#' + log.userID).remove();
});

function AddNewLog(log) {
    // Copy template row
    // Add new data
    var newRow = $('#log-copy-row').clone().prop('id', log.userID);
    newRow.find('.log-username').text(log.userName);
    newRow.find('.log-yesterday').text(log.yesterday);
    newRow.find('.log-today').text(log.today);
    newRow.find('.log-blockers').text(log.blockers);

    if ($('.log-container[data-UserID="' + log.userID + '"]').length == 0) {
        // New User Log
        newRow.removeAttr('hidden');
        newRow.html('<div class="log-container" data-UserID="' + log.userID + '">' + newRow.html() + '</div>');
        newRow.find('.log-copy-content').removeClass('log-copy-content').addClass('log-content');
        newRow.appendTo('#logs-container');
    }
    else {
        // Existing User Log
        //newRow.appendTo('.log-container[data-UserID="' + log.userID + '"]').removeAttr('hidden');
        var container = $('.log-container[data-UserID="' + log.userID + '"]');
        var currContent = container.find('.log-content');
        var newContent = newRow.find('.log-copy-content').removeClass('log-copy-content').addClass('log-content');
        console.log(currContent.html());
        console.log(newContent.html());
        currContent.html(newContent.html());
    }
}

function FormatDate(date) {
    return date.getFullYear().toString()
    + ('0' + (date.getMonth() + 1)).slice(-2)
    + ('0' + date.getDate()).slice(-2);
}

var currGroup = "";

function ChangeListener(date) {
    if (currGroup != "") {
        connection.invoke("RemoveFromGroup", currGroup).catch(function (err) { return console.error(err.toString()); });
        console.log("Removed from group: " + currGroup);
    }
    var groupID = window.location.href.substring(window.location.href.lastIndexOf('/') + 1);
    currGroup = groupID + '_' + FormatDate(date);

    var userID = $('#userID').val();

    //$('#logs-container').empty();
    $('.log-content').empty();
    $('#variableFormDetails').empty();
     if ($('input[name="FormDate"]').length == 0)
        $('#variableFormDetails').append('<input hidden name="FormDate" />')
    var inputs = $('input[name="FormDate"]');
    inputs.val(date.toLocaleDateString());

    $.ajax({
        type: "GET",
        dataType: "json",
        contentType: "application/json; charset=utf-8",
        url: "/Group/ChangeListener/" + currGroup,
        cache: false,
        success: function (data) {
            $('#yesterdayTA').val('').data('revert', '');
            $('#todayTA').val('').data('revert', '');
            $('#blockersTA').val('').data('revert', '');

            data.forEach((log)=> {
                if (log.userID != userID)
                    AddNewLog(log);
                else {
                    $('#yesterdayTA').val(log.yesterday);
                    $('#todayTA').val(log.today);
                    $('#blockersTA').val(log.blockers);
                    $('#yesterdayTA').data('revert', log.yesterday);
                    $('#todayTA').data('revert', log.today);
                    $('#blockersTA').data('revert', log.blockers);

                    $('#variableFormDetails').append('<input hidden name="DocId" value="' + log.docId + '" />')
                    //$('#variableFormDetails').append('<input hidden name="Date" value="' + log.date_datetime + '" />')
                    $('#variableFormDetails').append('<input hidden name="UserName" value="' + log.userName + '" />')
                    $('#variableFormDetails').append('<input hidden name="UserID" value="' + log.userID + '" />')
                }
            });
        },
        error: function () {
            console.log("error changing listener");
        },
        complete: function () {
            var emptyRow = $('#log-copy-row-empty');
            //console.log($('.log-container:empty'));
            $('.log-content:empty').append(emptyRow.clone().removeAttr('hidden'));
        }
    });

    connection.invoke("AddToGroup", currGroup).catch(function (err) { return console.error(err.toString()); });
    console.log("Added to group: " + currGroup);
}

async function start() {
    try {
        await connection.start();
        var today = new Date();
        ChangeListener(today);
        console.log("connected");
    } catch (err) {
        console.log(err);
        setTimeout(() => start(), 5000);
    }
};

connection.onclose(async () => {
    await start();
});

// Start the connection.
start();