import usb.core
import usb.util
import sys

def getJSONUsb():
	dev = usb.core.find(find_all=True, bDeviceClass=0x00)

	json = "["
	for d in dev:
		'''
		print d.idVendor
		print d.idProduct
		print d.bcdDevice
		print "PORT : "+str(d.port_number)
		print "PORTS : "+str(d.port_numbers)
		'''
		json += "{\"portNumber\":"+str(d.port_number)+",\"idVendor\":\""+str(d.idVendor)+"\",\"idProduct\":\""+str(d.idProduct)+"\"},"

	if json != "[":
		json = json[:-1]
	json += "]"

	return json
