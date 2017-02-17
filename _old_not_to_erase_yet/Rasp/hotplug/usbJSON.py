import os


# Fonction pour recuperer le nombre de ligne d'un fichier
def file_len(fname):
	with open(fname) as f:
		for i, l in enumerate(f):
			pass
	return i+1

def generateJSON():
	os.system("lsusb -t > dataRASP") # a decommenter pour le mettre sur la rasp (actualiser)
	doc = open('dataRASP', 'r')

	f1 = [x for x in doc.readlines()]

	nbLine = file_len("dataRASP")
	i=4
	portUsed = []
	while i<nbLine:
		currentPort = f1[i].strip()[9]
		print currentPort
		if currentPort not in portUsed:
			portUsed.append(currentPort)
		i += 1
	jsonToSend = "{'ports':" 
	jsonToSend += "["
	for tmp in range(1,5):

		if str(tmp) in portUsed:
			if tmp == 4:
				jsonToSend += "{'portNumber':" + str(tmp) +", 'status':'used'}"
			else:
				jsonToSend += "{'portNumber':" + str(tmp) +", 'status':'used'},"

		else:
			if tmp == 4:
				jsonToSend += "{'portNumber':" + str(tmp) +", 'status':'unused'}"
			else:
				jsonToSend += "{'portNumber':" + str(tmp) +", 'status':'unused'},"

	jsonToSend += "]}"

	doc.close()

	return jsonToSend + '\n'
