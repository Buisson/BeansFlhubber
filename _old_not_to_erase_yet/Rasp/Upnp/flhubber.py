# PyUPnP - Simple Python UPnP device library built in Twisted
# Copyright (C) 2013  Dean Gardiner <gardiner91@gmail.com>

# This program is free software: you can redistribute it and/or modify
# it under the terms of the GNU General Public License as published by
# the Free Software Foundation, either version 3 of the License, or
# (at your option) any later version.

# This program is distributed in the hope that it will be useful,
# but WITHOUT ANY WARRANTY; without even the implied warranty of
# MERCHANTABILITY or FITNESS FOR A PARTICULAR PURPOSE.  See the
# GNU General Public License for more details.

# You should have received a copy of the GNU General Public License
# along with this program.  If not, see <http://www.gnu.org/licenses/>.

import logging
from threading import Thread
import time
from twisted.internet import reactor
from pyupnp.device import Device, DeviceIcon
from pyupnp.logr import Logr
from pyupnp.services import register_action, Service, ServiceActionArgument, ServiceStateVariable
from pyupnp.services.connection_manager import ConnectionManagerService
from pyupnp.services.content_directory import ContentDirectoryService
from pyupnp.services.microsoft.media_receiver_registrar import MediaReceiverRegistrarService
from pyupnp.ssdp import SSDP
# from pyupnp.upnp import UPnP
from upnp import UPnP
from flhubber_services.ports import PortsService
from flhubber_services.leds import LedsService


class FlhubberDevice(Device):
    deviceType = 'urn:schemas-upnp-org:device:Flhubber:1'

    friendlyName = "PyUPnP-Flhubber"

    def __init__(self):
        Device.__init__(self)

        self.uuid = '2fac1234-31f8-11b4-a222-08002b34c003'

        self.portsservice = PortsService()
        self.ledsservice = LedsService()

        self.services = [
            self.portsservice,
            self.ledsservice
        ]

        self.portsservice.startListening()

        self.icons = [DeviceIcon('image/png', 32, 32, 24,'./index.png')]


if __name__ == '__main__':
    Logr.configure(logging.DEBUG)

    device = FlhubberDevice()

    upnp = UPnP(device)
    ssdp = SSDP(device)

    upnp.listen()
    ssdp.listen()

    reactor.run()
