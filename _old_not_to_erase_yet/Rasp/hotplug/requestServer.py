import requests

def requestServer(json):
	url = 'http://flhubber-aoechat.rhcloud.com/s?ports='+json
	print url
	# GET
	r = requests.get(url)

	if r.status_code == 200:
		print "good"
	else:
		print "an error has occured"
