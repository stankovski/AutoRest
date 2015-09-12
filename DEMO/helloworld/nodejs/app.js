var MyClient = require('./generated/myClient')

var client = new MyClient();

client.getGreeting(function(err, res){
	console.log(res.body);	
});