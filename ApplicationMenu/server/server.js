var net = require('net');
var http = require('http');
var express = require('express')();

const HTTP_PORT = '3000'; 
const UNITY_PORT = '8000'; 


express.get('/', function(req, res){
  res.sendFile('index.html', { root : __dirname});
});

express.listen(HTTP_PORT);

var server = net.createServer(function (socket) {    

    express.get('/baton:id', function(req, res){
        var id = req.params.id;
        var ret = socket.write('baton' + req.params.id);
        console.log(ret);
        res.send(id);
    });
    
    socket.on('end', function() {
        server.close();
        express
    });
    
});

server.listen(UNITY_PORT);