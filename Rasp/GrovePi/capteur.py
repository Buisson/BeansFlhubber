#!/usr/bin/env python

import smbus 
import time

bus_pi = smbus.SMBus(1)

addr = 0x30

while True:
	x = bus_pi.read_word_data(addr,0)
	msb=x>>8
	lsb=x&0x00FF
	wtemp=((lsb<<8)|msb)>>4
	print 'TMP102 I2C: 0x{0:02x} Lecture 0x{1:04}'.format(addr,wtemp)
	time.sleep(0.5)
