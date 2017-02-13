var express = require('express');
var app = express();
var fs = require("fs");
var bodyParser = require("body-parser");
var mailer = require("nodemailer");
var TMClient = require("textmagic-rest-client");



app.use(express.static('public'));
app.use(function(request, response, next) {
  response.header("Access-Control-Allow-Origin", "*");
  response.header("Access-Control-Allow-Headers", "Origin, X-Requested-With, Content-Type, Accept");
  next();
});

var http = require('http').Server(app);

var httpReq = require('http');

var io = require('socket.io')(http);


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

var ipWCOMP="";

var listDevicePlug = []; //TODO envoyer la liste des devices connectés lors de la connexion d'un client...

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

app.get('/sendSms', function(req,res) {

	console.log('sendSms : ', req.query);
	var content = fs.readFileSync("config.json");
	var jsonContent = JSON.parse(content);
	var id = req.query.id;
	if(jsonContent[id]) {
		var number = jsonContent[id]["phone"];
		var send = jsonContent[id]["usePhone"];
		if(number == "None" || send == "No") {
			res.send("Sms not send");
		 	res.end();
		 	console.log("Sms not send");
		 	return;
		}
	}

	var retour = "Fin Envois SMS";
	var c = new TMClient('edgarpersenda', 'wxtSKsbxz4iTkSiXiZCXwggTGoWo2n ');
	c.Messages.send({text: 'Va brancher ton portable !', phones: number}, function(err, res){
	    console.log('Messages.send()', err, res);
	    if(err){
	    	console.log("ERROR SMS");
	    	//retour = "Une erreur est survenue lors de l'envois du SMS...";
	    }
	});
	res.send(retour);
});

app.get('/sendMail', function (req, res){
	 console.log('sendMail : ', req.query);
	 var content = fs.readFileSync("config.json");
	 var jsonContent = JSON.parse(content);
	 var id = req.query.id;
	 if(jsonContent[id]) {
		 var adress = jsonContent[id]["email"];
		 var send = jsonContent[id]["useMail"];
		 if(adress == "None" || send == "No") {
		 	res.send("Email not set");
		 	res.end();
		 	console.log("Email not set");
		 	return;
		 }

		var mail = {
			from: "flhubberocs@gmail.com",
			to: adress,
			subject: "[ALERTE FLHUBBER] Branchement de votre périphérique",
			html: "Une alerte a été envoyé depuis votre hub USB FlHubber votre périphérique : "+jsonContent[id]["deviceName"]+" n'a pas été branché depuis "+jsonContent[id]["timeBeforeAlert"]+" secondes. Vous devriez brancher votre périphérique."
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
	console.log("DANS /USERRRRRRRRRRRRRRRRRRRRRRR")
	var content = fs.readFileSync("config.json");
	var jsonContent = JSON.parse(content);
	var id = req.query.id;
	var user = jsonContent[id];
	console.log("user", user);
	var ret = {"id":id , "timeBeforeAlert":user.timeBeforeAlert};
	res.send(ret);
 	res.end();
});

app.get('/userInfoFront', function(req, res){
	var content = fs.readFileSync("config.json",{encoding: 'utf-8'});
	var jsonContent = JSON.parse(content);

	var id = req.query.id;
	var user = jsonContent[id];
	//console.log(content);
	res.send(user);
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
	jsonContent[req.body.id].phone = req.body.phone;
	jsonContent[req.body.id].useMail = req.body.useMail;
	jsonContent[req.body.id].usePhone = req.body.usePhone;
	jsonContent[req.body.id].deviceName = req.body.surname;
	jsonContent[req.body.id].timeBeforeAlert = parseInt(req.body.timeBeforeAlert * 3600);

	var ret = {"id":req.body.id , "timeBeforeAlert":jsonContent[req.body.id].timeBeforeAlert};
	
	fs.writeFile('config.json', JSON.stringify(jsonContent), function (err) {
	  if (err){
	  	res.send("KO");
	  	return console.log(err);
	  }
	  console.log('Json updated');
	});
	if(ipWCOMP!=""){
		var pathWcomp = '/FlHubber/ConfReturn/'+JSON.stringify(ret);
		console.log("Appel de l'url WCOMP : "+"http://"+ipWCOMP+":8081"+pathWcomp);
		//console.log("ICIIIIII : "+pathWcomp);
		httpReq.get("http://"+ipWCOMP+":8081"+pathWcomp,function(response){

		});
	}
	else{
		console.log("IPWcomp pas set ...");
	}

	res.send("OK");
});


app.get('/create', function (req, res) {

	console.log('create : ', req.query);
    var content = fs.readFileSync("config.json");
	var jsonContent = JSON.parse(content);
	
	console.log("ICIIIIIIII : "+req.query.ids);
	usersReceive = JSON.parse(req.query.ids);

    var users = Object.keys(jsonContent);
    var ports = [];

    for(var i = 0, len = users.length; i<len; i++) {
    	jsonContent[users[i]]["portNumber"] = -1;
    }

    for (var i = 0; i < usersReceive.length; i++) {
    	var isequal = false;
    	var atWhatisEqual;
    	for (var j = 0; j < users.length; j++) {
	    	if(users[j] == usersReceive[i].idVendor + usersReceive[i].idProduct + usersReceive[i].iSerialNumber) {
	    		isequal = true;
	    		atWhatisEqual = users[j];
	    	}
		}
		if(isequal) {
			jsonContent[atWhatisEqual]["infos"] = usersReceive[i];
			jsonContent[atWhatisEqual]["portNumber"] = usersReceive[i]["portNumber"];
			isequal = false;
		}	
		else {
			 jsonContent[usersReceive[i]["idVendor"] + usersReceive[i]["idProduct"] + usersReceive[i]["iSerialNumber"]] = {"timeBeforeAlert":0,"email":"None","phone":"None","useMail":"No","usePhone":"No","deviceName":usersReceive[i]["idVendor"] + usersReceive[i]["idProduct"] + usersReceive[i]["iSerialNumber"] ,"port" : usersReceive[i]["portNumber"] ,"info" : usersReceive[i]};
		}
		ports[i] = {'id':(usersReceive[i].idVendor + usersReceive[i].idProduct + usersReceive[i].iSerialNumber ), 'portNum' : usersReceive[i]["portNumber"] , 'name':jsonContent[usersReceive[i].idVendor + usersReceive[i].idProduct + usersReceive[i].iSerialNumber]["deviceName"] };
	}
	listDevicePlug = ports;
	io.emit("updatePorts",ports);



    fs.writeFile('config.json', JSON.stringify(jsonContent), function (err) {
		if (err){
			return console.log(err);
		}
		console.log('Json updated');
	});

    res.send(req.query);
});


app.get('/getDevicesPlugged', function (req, res) {
	res.send(listDevicePlug);
});


app.get('/getConfigMeteo', function(req, res){
	console.log("Dans getConfigMeteo");
	var contentMeteo = fs.readFileSync("meteo.json",{encoding: 'utf-8'});
	console.log(contentMeteo);
	res.send(contentMeteo);
});

app.get("/updateConfigMeteo", function(req, res){
	console.log("dans /updateConfigMeteo");
	fs.writeFile('meteo.json', req.query.newConf, function (err) {
	  if (err){
	  	res.send("KO");
	  	return console.log(err);
	  }
	  console.log('Meteo updated');
	});

	if(ipWCOMP!=""){
		var pathWcomp = '/FlHubber/Meteo/'+req.query.newConf;
		//console.log("ICIIIIII : "+pathWcomp);
		console.log("Appel de ..."+"http://"+ipWCOMP+":8081"+pathWcomp);
		httpReq.get("http://"+ipWCOMP+":8081"+pathWcomp,function(response){

		});
	}
	else{
		console.log("IPWcomp pas set ...");
	}

	res.send("OK");
});

app.get("/setIpWcomp",function(req, res){
	console.log("dans setIpWcomp");
	ipWCOMP = req.query.ip;
	console.log(req.query.ip);
	res.send("OK");
});

app.get("/ipWcomp", function(req, res){
	res.send(ipWCOMP);
});

/*
var server = app.listen(8081, function () {

  var host = server.address().address;
  var port = server.address().port;

  console.log("Example app listening on port : %s", port);

});
*/
http.listen(8081,"0.0.0.0");