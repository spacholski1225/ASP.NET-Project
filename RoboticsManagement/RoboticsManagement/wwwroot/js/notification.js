"use strict";

var connection = new signalR.HubConnectionBuilder().withUrl("/notificationHub").build(); //tutaj sie laczy z startup i tworzy polaczenie

//Disable send button until connection is established
document.getElementById("getNoti").disabled = true;

connection.on("ReceiveTask", function (employeeId, notification) {
    var heading = document.createElement("h3");
    heading.textContent = "New Task"
    var encodedMsg = employeeId + " : " +notification.NotiBody;
    var li = document.createElement("li");
    li.textContent = encodedMsg;
    document.getElementById("notificationsList").appendChild(li);
});

connection.start().then(function () {
    document.getElementById("getNoti").disabled = false;
}).catch(function (err) {
    return console.error(err.toString());
});

document.getElementById("getNoti").addEventListener("click", function (event) {
    //tutaj polaczenie z DB i pobranie danych

    var employeeId = "empid";
    var notification = "Notification Message";
    connection.invoke("AddNewNotificationForEmployee", employeeId, notification).catch(function (err) {
        return console.error(err.toString());
    });
    event.preventDefault();
});