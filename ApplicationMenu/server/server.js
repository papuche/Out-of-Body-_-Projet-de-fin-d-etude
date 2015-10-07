var net = require('net');
var http = require('http');
var express = require('express')();

const HTTP_PORT = '3000'; 
const UNITY_PORT = '8000'; 


express.get('/', function(req, res){
  res.sendFile('index.html', { root : __dirname});
});

express.listen(HTTP_PORT);

net.createServer(function (socket) {    

    express.get('/baton:id', function(req, res){
        console.log(req.params.id);
    });
}).listen(UNITY_PORT);