var net = require('net');
var http = require('http');
var express = require('express');
var path = require('path');

const HTTP_PORT = '3000'; 
const UNITY_PORT = '8000'; 

var app = express();
var clients_unity = [];

app.get('/', function(req, res){
    if(req=="portes")
        res.sendFile(path.join(__dirname,'../client/templates','/portes.html'));
    else
        res.sendFile(path.join(__dirname,'../client/templates','/menuWeb.html'));
});

app.use(express.static(__dirname+"/.."));
app.listen(HTTP_PORT);


function getAndSendWithoutParams(socket, url) {
    app.get('/' + url, function(req, res) {
        console.log(url);
        clients_unity[0].write(url);
        res.end();
    });
}

function getAndSendWithParams(socket, url) {
    url += "/:values";
    app.get('/' + url, function(req, res) {
        var send = req.originalUrl.replace('/', '');
        console.log(send);
        clients_unity[0].write(send);
        res.end();
    });
}

var server = net.createServer(function (socket) {    
    
    clients_unity.push(socket);
      
    socket.on('close', function(e) { 
        clients_unity.splice(clients_unity.indexOf(socket), 1);
    });

    
    process.on('uncaughtException', function (err) {
        console.error(err.stack);
    });
    
    getAndSendWithoutParams(socket, 'avatar');
    getAndSendWithoutParams(socket, 'baton');
    getAndSendWithoutParams(socket, 'morphing');
    getAndSendWithoutParams(socket, 'baton_morphing');
	getAndSendWithoutParams(socket, 'ghost');
    getAndSendWithoutParams(socket, 'stop');
    getAndSendWithoutParams(socket, 'nothing');
    getAndSendWithoutParams(socket, 'M_avatar');
    getAndSendWithoutParams(socket, 'F_avatar');
    
    getAndSendWithParams(socket, 'e');
    getAndSendWithParams(socket, 'db');
    getAndSendWithParams(socket, 'dh');
    getAndSendWithParams(socket, 'validerAvatar');
});

server.timeout = 0;
server.listen(UNITY_PORT);
