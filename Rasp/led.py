import RPi.GPIO as GPIO
import time

GPIO.setwarnings(False)
GPIO.setmode(GPIO.BCM)

blueLeds = 14
orangeLeds = 15

GPIO.setup(blueLeds,GPIO.OUT)
GPIO.setup(orangeLeds,GPIO.OUT)

def changeColor(color):
	
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

# while True :
# 	changeColor("BLUE")
# 	time.sleep(1)
# 	changeColor("ORANGE")
# 	time.sleep(1)

changeColor("ALL")
