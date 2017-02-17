import logging
from threading import Thread
import time
import threading
from twisted.internet import reactor
from pyupnp.event import EventProperty
from pyupnp.device import Device, DeviceIcon
from pyupnp.logr import Logr
from pyupnp.services import register_action, Service, ServiceActionArgument, ServiceStateVariable
from pyupnp.ssdp import SSDP
from pyupnp.upnp import UPnP

import RPi.GPIO as GPIO
import time

GPIO.setwarnings(False)
GPIO.setmode(GPIO.BCM)

blueLeds = 14
orangeLeds = 15

GPIO.setup(blueLeds,GPIO.OUT)
GPIO.setup(orangeLeds,GPIO.OUT)

class LedsService(Service):
	version = (1, 0)
	serviceType = "urn:schemas-upnp-org:service:LedsService:1"
	serviceId = "urn:upnp-org:serviceId:LedsService"

	actions = {
	    'SetLedColor': [
	        ServiceActionArgument('color','in','ARG_LED_COLOR')
	    ]
	}

	stateVariables = [
		# Arguments
		ServiceStateVariable('ARG_LED_COLOR','string')
	]

	@register_action('SetLedColor')
	def changeColor(self,color):
		print color
		if color == "BLUE":
			GPIO.output(blueLeds, GPIO.HIGH)
			GPIO.output(orangeLeds, GPIO.LOW)
		elif color == "ORANGE":
			GPIO.output(blueLeds, GPIO.LOW)
			GPIO.output(orangeLeds, GPIO.HIGH)
		elif color == "ALL":
			GPIO.output(blueLeds, GPIO.HIGH)
			GPIO.output(orangeLeds, GPIO.HIGH)
		elif color == "NONE":
			GPIO.output(blueLeds, GPIO.LOW)
			GPIO.output(orangeLeds, GPIO.LOW)
