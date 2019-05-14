import paho.mqtt.client as mqtt #import the client1
import time
import random
import asyncio

def on_message(client, userdata, message):
    print("message received " ,str(message.payload.decode("utf-8")))
    print("message topic=",message.topic)
    print("message qos=",message.qos)
    print("message retain flag=",message.retain)

async def send_message(client):
    var = 1
    while var == 1:
        temp = random.randint(1,101)
        client.publish("/master",str(temp) + ",P1")#publish
        await asyncio.sleep(3)

broker_address="iot.eclipse.org" 
#broker_address="iot.eclipse.org" #use external broker
client = mqtt.Client("Python") #create new instance
client.subscribe("/c/P1")
client.on_message=on_message 
client.connect(broker_address) #connect to broker
client.loop_start()
loop = asyncio.get_event_loop()
#send_message(client)    
tasks = [send_message(client)]
loop.run_until_complete(asyncio.wait(tasks))
loop.close()
client.loop_stop() #stop the loop