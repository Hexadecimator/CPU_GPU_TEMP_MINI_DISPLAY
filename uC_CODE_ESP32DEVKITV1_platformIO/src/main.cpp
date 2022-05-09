#include <Arduino.h>
#include <SPI.h>
#include <Wire.h>
#include <Adafruit_GFX.h>
#include <Adafruit_SSD1306.h> 
#include "delaySafe.h"

#define SCREEN_WIDTH 128 // OLED display width, in pixels
#define SCREEN_HEIGHT 64 // OLED display height, in pixels

// Declaration for an SSD1306 display connected to I2C (SDA, SCL pins)
#define OLED_RESET     -1 // Reset pin # (or -1 if sharing Arduino reset pin)
#define SCREEN_ADDRESS 0x3C /// both are 0x3C
Adafruit_SSD1306 LED(SCREEN_WIDTH, SCREEN_HEIGHT, &Wire, OLED_RESET);

int info_refresh_rate = 1; // Hz

  // ------------------------------------
 // START SERIAL COMM TO CSHARP DEFINES |
// --------------------------------------
#define SOP '<' // denotes start of serial data packet
#define EOP '>' // denotes end of serial data packet
char ctrlChar;
char serialData[20];     // serial receive buffer

char CPU_TEMP[3];        // 2 digits, if temps are 3 digits eg: 110 then "!!" will be displayed.
char GPU_TEMP[3];        // C# will control this and just send char data for the uC to easily handle

byte serial_idx;         // serialData indexer
boolean started = false; // serial data flow control
boolean ended   = false; // serial data flow control

void clearScreenAndDrawBorder() 
{
  LED.clearDisplay();
  LED.setTextColor(SSD1306_WHITE);
  LED.setTextSize(2);
  LED.setCursor(10, 0);
  LED.print("CPU");
  LED.setCursor(75, 0);
  LED.print("GPU");
}

// initDisplayOrErrorCondition() is meant to be called upon startup before
// having obtained info from the C# program, or in the case that the 
// microcontroller detects it is not communicating correctly with the C# app 
void initDisplayOrErrorCondition()
{
  clearScreenAndDrawBorder();  
  LED.setTextSize(4.5);
  LED.setCursor(5, 20);
  LED.print("??"); // CPU temp
  LED.setCursor(70, 20);
  LED.print("??"); // GPU temp
  LED.display();
}

void updateCPUandGPUTempToScreen(char * CPU_TEMP_FROM_CSHARP, char * GPU_TEMP_FROM_CSHARP)
{
  clearScreenAndDrawBorder();  
  LED.setTextSize(4.5);
  LED.setCursor(5, 20);
  LED.print(CPU_TEMP_FROM_CSHARP); // CPU temp should be 2 characters
  LED.setCursor(70, 20);
  LED.print(GPU_TEMP_FROM_CSHARP); // GPU temp should be 2 characters
  LED.display();
}

/* Function Name: void checkSerial()
 * 
 * Returns: None
 *
 * Description:
 *  - inspects incoming serial data for "packetized" data. A proper 
 *    serial data packet starts with an '<' and ends with an '>'
 * 
 *  Proper format for our application is this:
 *  <C##G##>
 *  - 8 bytes total
 *  (where # will be a char representation of the degrees in celsius)
 */
void checkSerial()
{
  // if serial is available, receive the
  // serial data packet (it better be formatted
  // correctly!!)
  while (Serial.available() > 0)
  {
    char inChar = Serial.read();
    // check if we receive the start character  
    // (SOP) of a serial packet
    if (inChar == SOP)
    {
      serial_idx = 0;
      serialData[serial_idx] = '\0'; // null character
      started = true;
      ended = false;
    }
    // check if we receive the end character
    // (EOP) of a serial packet
    else if (inChar == EOP)
    {
      ended = true;
      break;
    }
    else
    {
      if (serial_idx < 9) // 8 char total... 9 for feel-goods
      {
        serialData[serial_idx] = inChar;
        ++serial_idx;
        serialData[serial_idx] = '\0';
      }
    }
  }
  if (started && ended)
  {
    // we now have a packet of this format:
    //          C##G##
    //          012345
    // cpu temp = serialData[2] and serialData[3]
    // gpu temp = serialData[5] and serialData[6]
    CPU_TEMP[0] = serialData[1]; CPU_TEMP[1] = serialData[2];
    GPU_TEMP[0] = serialData[4]; GPU_TEMP[1] = serialData[5];

    updateCPUandGPUTempToScreen(CPU_TEMP, GPU_TEMP);
    
    // packet processing completed, reset packet
    // parameters to get ready for next packet
    started = false;
    ended = false;
    serial_idx = 0;
    serialData[serial_idx] = '\0';
  }
}


void setup() 
{
  Serial.begin(115200);
  while(!Serial); // wait
  Serial.println("Allocating SSD1306...");

  if(!LED.begin(SSD1306_SWITCHCAPVCC, SCREEN_ADDRESS)) 
  {
    Serial.println(F("SSD1306 allocation failed"));   
    for(;;); // Don't proceed, loop forever
  } 

  delaySafeMillis(100); // for feel-goods

  initDisplayOrErrorCondition();
}

void runTestData()
{
  // *** for testing ***
  CPU_TEMP[0] = '6'; CPU_TEMP[1] = '9';
  GPU_TEMP[0] = '4'; GPU_TEMP[1] = '2';
  updateCPUandGPUTempToScreen(CPU_TEMP, GPU_TEMP);
  delaySafeMillis(3000);
  CPU_TEMP[0] = '1'; CPU_TEMP[1] = '2';
  GPU_TEMP[0] = '3'; GPU_TEMP[1] = '4';
  updateCPUandGPUTempToScreen(CPU_TEMP, GPU_TEMP);
  delaySafeMillis(3000);
  initDisplayOrErrorCondition();
}

void loop() 
{
  checkSerial();
  //runTestData();
  delaySafeMillis(250);
}