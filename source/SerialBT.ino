#include "Wire.h"
#include <MPU6050_light_better.h>
#include "BluetoothSerial.h"

MPU6050 mpu(Wire);

BluetoothSerial SerialBT;

String result = "";

void setup() {
  Serial.begin(115200);

   while (!Serial) 
  {
    delay(10);
  }


  SerialBT.begin("QuackPLank"); //Bluetooth device name
  Serial.println("The device started, now you can pair it with bluetooth!");


  Serial.println("Adafruit MPU6050 test!");

  byte status = mpu.begin();
  Serial.print(F("MPU6050 status: "));
  Serial.println(status);
  while(status!=0){ } // stop everything if could not connect to MPU6050
  
  Serial.println(F("Calculating offsets, do not move MPU6050"));
  delay(1000);
  mpu.calcOffsets(true,true); // gyro and accelero
  Serial.println("Done!\n");
}

void loop() {

  mpu.update();

  result.concat(mpu.getAngleX());
  result.concat(" ");
  result.concat(mpu.getAngleY());
  result.concat(" ");
  result.concat(mpu.getAngleZ());

  SerialBT.println(result);
  result = "";
  
  delay(20);
}