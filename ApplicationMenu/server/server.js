var net = require('net');
var http = require('http');
var express = require('express');
var path = require('path');

const HTTP_PORT = '3000'; 
const UNITY_PORT = '8000'; 

var app = express();

app.get('/', function(req, res){
    if(req=="portes")
        res.sendFile(path.join(__dirname,'../client/templates','/portes.html'));
    else
        res.sendFile(path.join(__dirname,'../client/templates','/menuWeb.html'));
});

app.use(express.static(__dirname+"/.."));
app.listen(HTTP_PORT);

var server = net.createServer(function (socket) {    

    getAndSendWithoutParams(socket, 'baton');
    getAndSendWithoutParams(socket, 'morphing');
    getAndSendWithoutParams(socket, 'baton_morphing');
    getAndSendWithoutParams(socket, 'stop');
    getAndSendWithoutParams(socket, 'nothing');
    
    getAndSendWithParams(socket, 'e');
    getAndSendWithParams(socket, 'db');
    getAndSendWithParams(socket, 'dh');

    socket.on('end', function() {
        server.close();
    });
    
    socket.on('error', function() {});
    
});

server.listen(UNITY_PORT);


function getAndSendWithoutParams(socket, url) {
    app.get('/' + url, function(req, res) {
          socket.write(url);
    });
}

function getAndSendWithParams(socket, url) {
    url += ":values";
    app.get('/' + url, function(req, res) {
        var send = req.originalUrl.replace('/', '');
        socket.write(url);
    });
}
