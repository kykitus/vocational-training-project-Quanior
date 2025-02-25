#include <Arduino.h>
#include <TinyMPU6050.h>
#include "BluetoothSerial.h"
#include "Wire.h"
#include "esp_system.h"

//RTC_NOINIT_ATTR bool bootflag = true; Linie zakomentowane są jedną z prób użycia restartu ESP do poprawienia działania MPU. Restart działa, ale MPU zachowuje się tak samo.

BluetoothSerial SerialBT;

MPU6050 mpu(Wire);

void setup()
{
  //delay(1000);

  Serial.begin(115200);
  
  /*
  if (bootflag != false)
  {
    Serial.println(bootflag);
    Serial.println("Resetting tour");
    bootflag = false;
    //esp_restart();  // More reliable than ESP.restart()
  }*/

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
