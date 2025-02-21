#include <MPU6050_tockn.h>
#include <Wire.h>
#include "BluetoothSerial.h"

MPU6050 mpu6050(Wire);

BluetoothSerial SerialBT;

void setup() {
  Serial.begin(115200);
  SerialBT.begin("QuackPLank"); //Bluetooth device name
  Serial.println("The device started, now you can pair it with bluetooth!");
  Wire.begin();
  mpu6050.begin();
  mpu6050.calcGyroOffsets(true);
}


void loop() {
  mpu6050.update();
  SerialBT.print(mpu6050.getAngleX());
  SerialBT.print(" ");
  SerialBT.print(mpu6050.getAngleY());
  SerialBT.print(" ");
  SerialBT.print(mpu6050.getAngleZ());
  SerialBT.println("");
  delay(20);
}