#include <Arduino.h>

#include <WebSocketsClient.h>
#include <M5Unified.h>

WebSocketsClient webSocket;

const char *ssid = SHOWCASE_WIFI_SSID;
const char *password = SHOWCASE_WIFI_PASSWORD;
const char *plantId = SHOWCASE_PLANT_ID;
const char *plantPassword = "1$oZttTMVuhh0qm/XvP1viIe";

unsigned long lastInterval = 0;
int pumpDuration = 0;
bool sendState = false;
int defaultScreen = RED;

void webSocketEvent(WStype_t type, uint8_t *payload, size_t length)
{
  switch (type)
  {
  case WStype_DISCONNECTED:
    defaultScreen = RED;
    pumpDuration = -1;
    Serial.println("[WSc] Disconnected");
    break;
  case WStype_CONNECTED:
  {
    sendState = true;
    Serial.println("[WSc] Connected");
    defaultScreen = GREEN;
  }
  break;
  case WStype_TEXT:
    String text = String((char *)payload);
    Serial.printf("[WSc] Text: %s\n", payload);
    pumpDuration = text.substring(2).toInt() * 1000;
    sendState = true;
    break;
  }
}

void setup()
{
  auto cfg = M5.config();

  cfg.serial_baudrate = 9600;

  M5.begin(cfg);

  Serial.begin(9600);

  Serial.printf("Connecting to SSID %s\n", ssid);
  WiFi.begin(ssid, password);

  while (WiFi.status() != WL_CONNECTED)
  {
    Serial.printf("WiFi status: %d\n", WiFi.status());
    delay(100);
  }

  webSocket.begin("192.168.0.201", 19, "/hydro");
  webSocket.setAuthorization(plantId, plantPassword);
  webSocket.onEvent(webSocketEvent);
  webSocket.setReconnectInterval(5000);
}

void loop()
{
  webSocket.loop();
  if (pumpDuration > 0)
  {
    M5.Lcd.fillScreen(BLUE);
  }
  else
  {
    M5.Lcd.fillScreen(defaultScreen);
  }

  if (sendState)
  {
    if (pumpDuration > 0) {
      webSocket.sendTXT("p:1");
    } else {
      webSocket.sendTXT("p:0");
    }
    sendState = false;
  }

  int tempDuration = pumpDuration;
  if (pumpDuration > 0)
  {
    pumpDuration -= millis() - lastInterval;
    if (pumpDuration <= 0) {
      sendState = true;
    }
  }
  else
  {
    pumpDuration = 0;
  }

  lastInterval = millis();
}