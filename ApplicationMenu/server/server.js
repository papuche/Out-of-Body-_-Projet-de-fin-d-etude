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