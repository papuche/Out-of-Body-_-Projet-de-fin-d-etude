var net = require('net');
var http = require('http');
var express = require('express');
var path = require('path');

const HTTP_PORT = '3000';
const UNITY_PORT = '8000';

var app = express();
var clients_unity = [];

var door_finish = false;

app.get('/', function(req, res){
    if(req=="portes")
        res.sendFile(path.join(__dirname,'../client/templates','/portes.html'));
    else
        res.sendFile(path.join(__dirname,'../client/templates','/menuWeb.html'));
});

app.use(express.static(__dirname+"/.."));
app.listen(HTTP_PORT);

var server = net.createServer(function (socket) {
    clients_unity.push(socket);

    socket.on('close', function(e) {
        clients_unity.splice(clients_unity.indexOf(socket), 1);
    });

    socket.on('data', function(data) {
      if (data.toString() === "door_finish") {
        door_finish = true;
      }
    });

    process.on('uncaughtException', function (err) {});

    getAndSendWithoutParams(socket, 'avatar');
    getAndSendWithoutParams(socket, 'exit');
    getAndSendWithoutParams(socket, 'baton');
    getAndSendWithoutParams(socket, 'morphing');
    getAndSendWithoutParams(socket, 'baton_morphing');
	  getAndSendWithoutParams(socket, 'ghost');
    getAndSendWithoutParams(socket, 'stop');
    getAndSendWithoutParams(socket, 'nothing');
    getAndSendWithoutParams(socket, 'M_avatar');
    getAndSendWithoutParams(socket, 'F_avatar');

    requestDoorFinish(socket);

    getAndSendWithParams(socket, 'e');
    getAndSendWithParams(socket, 'db');
    getAndSendWithParams(socket, 'dh');
    getAndSendWithParams(socket, 'validerAvatar');
});
server.listen(UNITY_PORT);

function requestDoorFinish(socket) {
    app.get('/porte', function(req, res) {
        if (door_finish == true) {
            door_finish = false
            res.send("Fin de porte.");
            res.end();
        }
    });
};

function getAndSendWithoutParams(socket, url) {
    app.get('/' + url, function(req, res) {
        clients_unity[0].write(url);
        res.end();
    });
}

function getAndSendWithParams(socket, url) {
    url += "/:values";
    app.get('/' + url, function(req, res) {
        var send = req.originalUrl.replace('/', '');
        clients_unity[0].write(send);
        res.end();
    });
}
