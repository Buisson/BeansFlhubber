var express = require('express');
var app = express();
var fs = require("fs");
var bodyParser = require("body-parser");
var mailer = require("nodemailer");

app.use(express.static('public'));


var smtpTransport = mailer.createTransport("SMTP",{
					service: "Gmail",
					auth: {
						user: "flhubberocs@gmail.com",
						pass: "polytech"
					}
				});


var test = {
	data: "test ok"
};

app.get('/test', function (req, res) {
    console.log( "test" );
    res.json( test );
    res.end();
});

app.get('/json', function (req, res){
	 var content = fs.readFileSync("config.json");
	 var jsonContent = JSON.parse(content);
	 res.send(jsonContent);
	 res.end();
});

app.get('/sendMail', function (req, res){
	 console.log('sendMail : ', req.query);
	 var content = fs.readFileSync("config.json");
	 var jsonContent = JSON.parse(content);
	 var id = req.query.id;
	 if(jsonContent[id]) {
		 var adress = jsonContent[id]["email"];

		 if(adress == "None") {
		 	res.send("Email not set");
		 	res.end();
		 	console.log("Email not set");
		 	return;
		 }

		var mail = {
			from: "flhubberocs@gmail.com",
			to: adress,
			subject: "Branche ton portable branleur",
			html: "Qu'est ce que tu fous encore là, va brancher ton portable"
		}

		smtpTransport.sendMail(mail, function(error, response){
						if(error){
							console.log("Erreur lors de l'envoie du mail!");
							console.log(error);
						}else{
							console.log("Mail envoyé avec succès!")
						}
						smtpTransport.close();
					});
	}
	console.log("ID DONT EXIST");
	res.send(req.query);
	res.end();
});


app.get('/getDevices', function (req,res)
{
	var content = fs.readFileSync("config.json");
	var jsonContent = JSON.parse(content);
	var devices_id = Object.keys(jsonContent);
	var reponse = [];
	for (var i = 0, len = devices_id.length; i < len; i++) {
		var device = {
			id : devices_id[i],
			name: jsonContent[devices_id[i]].deviceName
		}
		reponse.push(device);
		//users[i] = jsonContent[users[i]]["deviceName"] +"-" + users[i];
    }


	console.log(reponse);

	res.send(reponse);
	res.end();
});

app.get('/user', function (req, res){
	 var content = fs.readFileSync("config.json");
	 var jsonContent = JSON.parse(content);
	 var id = req.query.id;
	 var user = jsonContent[id];
     console.log("user", user);
	 res.send(user);
	 res.end();
});

app.use(bodyParser.urlencoded({
    extended: true
}));

app.use(bodyParser.json());

app.post('/form', function (req, res) {
    console.log( "form", req.body );
    var content = fs.readFileSync("config.json");
	var jsonContent = JSON.parse(content);

	jsonContent[req.body.id].email = req.body.email;
	jsonContent[req.body.id].deviceName = req.body.surname;
	jsonContent[req.body.id].timeBeforeAlert = req.body.timeBeforeAlert * 3600;
	
	fs.writeFile('config.json', JSON.stringify(jsonContent), function (err) {
	  if (err) return console.log(err);
	  console.log('Json updated');
	});	//todo save the json

});


app.get('/create', function (req, res) {

	console.log('create : ', req.query);
    var content = fs.readFileSync("config.json");
	var jsonContent = JSON.parse(content);
	
	var usersReceive = [];
	if(req.query.id === undefined){

	}
	else{
		usersReceive = req.query.ids.split(",");
	}

    var users = Object.keys(jsonContent);

    for (var i = 0, len = users.length; i < len; i++) {
    	for (var j = 0, len = usersReceive.length; j < len; j++) {
	    	if(users[i] != usersReceive[j]) {
    		    jsonContent[usersReceive[j]] = {"timeBeforeAlert":0,"email":"None","deviceName":usersReceive[j]};
	    	}
		}
	}

    fs.writeFile('config.json', JSON.stringify(jsonContent), function (err) {
		if (err){
			return console.log(err);
		}
		console.log('Json updated');
	});

    res.send(req.query);
});



var server = app.listen(8081, function () {

  var host = server.address().address;
  var port = server.address().port;

  console.log("Example app listening on port : %s", port);

});