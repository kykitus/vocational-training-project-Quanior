#include <Arduino.h>
#include <TinyMPU6050.h>
#include "BluetoothSerial.h"
#include "Wire.h"

BluetoothSerial SerialBT;

MPU6050 mpu(Wire);


void setup()
{
  //EEPROM.begin(1);
  delay(1000);

  Serial.begin(115200);

  // Initialization
  mpu.Initialize();
  SerialBT.begin("QuackPLank");

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
