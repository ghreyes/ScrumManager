﻿"use strict";

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
    newRow.appendTo('#logs-container').removeAttr('hidden');
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

    $('#logs-container').empty();
    $.ajax({
        type: "GET",
        url: "/Group/ChangeListener/" + currGroup,
        success: function (data) {
            console.log(data);
            data.forEach(log => AddNewLog(log));
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