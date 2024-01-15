# BarBotV2 Control Server 

This is the control software for the BarBotV2. 

This is a .NET 6 Blazor Server application setup to run on an Raspberry Pi using I2C and MariaDB/MySQL. 

## Setup 

Hardware

The BarBot uses daisy-chained SparkFun I2C Qwuiic modules to communicate with each unit. 
Ensure that I2C SDA is connected to the host Raspberry Pi's GPIO 2 (pin 3) and SCL to GPIO 3 (pin 5) as well as one of the grounding pins, this is all the required hardware setup, WiFi/Ethernet optional. 

