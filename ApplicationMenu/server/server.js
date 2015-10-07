var net = require('net');
var http = require('http');

const HTTP_PORT = '3000'; 

/*
http.listen(3000, function() {});
server.listen(8124, function() {

});

app.get('/', function(req, res){
  res.send('Bonjour');
});

var server = net.createServer(function(c) {
  console.log('client connected');
    
    app.get('/test', function(req, res){
        c.write('Requete GET test \r\n');
        c.pipe(c);
    });
});
*/

function handleRequest(request, response){
    
    response.end('It Works!! Path Hit: ' + request.url);
    // Envoi de la page HTML
}

//Create a server
var server = http.createServer(handleRequest);

//Lets start our server
server.listen(HTTP_PORT, function(){
    //Callback triggered when server is successfully listening. Hurray!
    console.log("Server listening on: http://localhost:%s", HTTP_PORT);
});