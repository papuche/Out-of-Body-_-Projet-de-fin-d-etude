<<<<<<< HEAD
var net = require('net'); // Librairie de socket c#.
var express = require('express')(); // Librairie pour la gestion des requètes http.
=======
var net = require('net');
var http = require('http');
var express = require('express');
var path = require('path');
>>>>>>> 01b2bb52c3e4eb387e1a86ef8a86bb5a272bcc6f

const HTTP_PORT = '3000';  // port http pour le navigateur client.
const UNITY_PORT = '8000';  // port socket unity.

<<<<<<< HEAD
// tableau de sockets C# connectées.
var clients_unity = [];

// On écoute les requète http sur le port HTTP_PORT
express.listen(HTTP_PORT);

// page par défaut de l'application (à revoir avec Sarah et Florian).
express.get('/', function(req, res){
  res.sendFile('index.html', { root : __dirname});
});

// Evenement lors de la connexion d'une socket c#.
net.createServer(function (socket) {    
    
    // on ajoute la socket au tableau. 
    clients_unity.push(socket);    
=======
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
>>>>>>> 01b2bb52c3e4eb387e1a86ef8a86bb5a272bcc6f

    // Url à revoir avec Sarah et Florian.
    express.get('/baton:id', function(req, res){
        var id = req.params.id;
        // Ecriture sur le client[0] => socket couante.
        clients_unity[0].write('baton' + req.params.id);    
    });

    // Evenement lors de la déconnexion de la socket c#.
    socket.on('close', function () {
        // Suppression de la socket du tableau de clients.
        clients_unity.splice(clients_unity.indexOf(socket), 1);
    });
    
<<<<<<< HEAD
    socket.on('error', function(e){});
    
}).listen(UNITY_PORT); // le serveur écoute sur le port UNITY_PORT pour les sockets c#.
=======
});


server.listen(UNITY_PORT);
>>>>>>> 01b2bb52c3e4eb387e1a86ef8a86bb5a272bcc6f
