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
		  console.log(data.toString());
        door_finish = true;
      }
    });

    process.on('uncaughtException', function (err) {});

    getAndSendWithoutParams('avatar');
    getAndSendWithoutParams('exit');
    getAndSendWithoutParams('stop');
    getAndSendWithoutParams('M_avatar');
    getAndSendWithoutParams('F_avatar');

	requestDoorsFinish();

    getAndSendWithParams('e');
    getAndSendWithParams('db');
    getAndSendWithParams('dh');
	getAndSendWithParams('oob');
    getAndSendWithParams('validerAvatar');
}).listen(UNITY_PORT);

function requestDoorsFinish() {
  app.get('/porte', function(req, res) {
    if (door_finish == true) {
      door_finish = false;
	  res.end();
    } else {
		res.sendStatus(403);
	}
  });
}

function getAndSendWithoutParams(url) {
    app.get('/' + url, function(req, res) {
        clients_unity[0].write(url);
        res.end();
    });
}

function getAndSendWithParams(url) {
    url += "/:values";
    app.get('/' + url, function(req, res) {
        var send = req.originalUrl.replace('/', '');
        clients_unity[0].write(send);
        res.end();
    });
}
