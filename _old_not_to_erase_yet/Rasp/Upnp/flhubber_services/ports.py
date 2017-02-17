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

import os
import glob
import time
import usb1
import getAllUSB

def readPorts():
	return getAllUSB.getJSONUsb()

class PortsService(Service):
    version = (1, 0)
    serviceType = "urn:schemas-upnp-org:service:PortsService:1"
    serviceId = "urn:upnp-org:serviceId:PortsService"

    portsUsed = "[{'portNumber': 1, 'status': 'used'}, {'portNumber': 2, 'status': 'used'}, {'portNumber': 3, 'status': 'unused'}, {'portNumber': 4, 'status': 'used'}]"

    actions = {
        'GetPorts': [
            ServiceActionArgument('Ports','out','Ports')
        ],
        'StartListeningPorts': [
            ServiceActionArgument('ListeningPorts','out','ListeningPorts')
        ]
    }

    stateVariables = [
        ServiceStateVariable('Ports','string',sendEvents=True),
        ServiceStateVariable('ListeningPorts','boolean',sendEvents=True)
    ]

    state=EventProperty('ListeningPorts')
    ports=EventProperty('Ports')


    @register_action('GetPorts')
    def getState(self):
        print("DANS GETPORTS")
        return {
            'Ports' : str(readPorts())
        }

    def setPorts(self,s):
        print("SET ....")
        self.ports = s

    i=True
    def PortsThread(self,s):
        self.loopCheckPorts()

    @register_action('StartListeningPorts')
    def startListening(self):
        self.state=True
        
        self.thread = threading.Thread(target=PortsService.PortsThread, args = (self,0))
        self.thread.daemon = True
        self.thread.start();
        return {
            'ListeningPorts':True
        }


    def hotplug_callback(self, context, device, event):
        print "calling the usb script"
        if event == usb1.HOTPLUG_EVENT_DEVICE_ARRIVED:
            print("Plug : ")
            print readPorts()
            self.setPorts(readPorts())
        else:
            print("UnPlug")
            print readPorts()
            self.setPorts(readPorts())


        print "Device %s: %s" % (
            {
                usb1.HOTPLUG_EVENT_DEVICE_ARRIVED: 'arrived',
                usb1.HOTPLUG_EVENT_DEVICE_LEFT: 'left',
            }[event],
            device,
        )

    def loopCheckPorts(self):
        with usb1.USBContext() as context:
            if not context.hasCapability(usb1.CAP_HAS_HOTPLUG):
                print 'Hotplug support is missing. Please update your libusb version.'
                return
            print 'Registering hotplug callback...'
            opaque = context.hotplugRegisterCallback(self.hotplug_callback)
            print 'Callback registered. Monitoring events, ^C to exit'
            try:
                while True:
                    context.handleEvents()
            except (KeyboardInterrupt, SystemExit):
                print 'Exiting'
