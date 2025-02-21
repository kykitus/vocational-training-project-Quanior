#include <Arduino.h>
#include <TinyMPU6050.h>
#include "BluetoothSerial.h"

BluetoothSerial SerialBT;

MPU6050 mpu (Wire);

void setup()
{
  // Initialization
  mpu.Initialize();
  Serial.begin(115200);
  SerialBT.begin("QuackPLank"); //Bluetooth device name

  // Calibration
  
  Serial.println("=====================================");
  Serial.println("Starting calibration...");
  mpu.Calibrate();
  Serial.println("Calibration complete!");
}

void loop()
{
  mpu.Execute();
  SerialBT.print(mpu.GetAngX());
  SerialBT.print(" ");
  SerialBT.print(mpu.GetAngY());
  SerialBT.print(" ");
  SerialBT.print(mpu.GetAngZ());
  SerialBT.println("");
  delay(20);
}
