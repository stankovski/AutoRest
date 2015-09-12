var express = require('express');
var app = express();

app.get('/api/HelloWorld', function (req, res) {
  res.send('"Hello via AutoRest. Hi!"');
});

var server = app.listen(9990, function () {
  var host = server.address().address;
  var port = server.address().port;

  console.log('Example app listening at http://%s:%s', host, port);
});