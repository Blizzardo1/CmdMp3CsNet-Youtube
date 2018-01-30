# CmdMp3CsNet-Youtube
A Soundboard program for the Elgato Stream Deck

Take advantage of HTTP Requests right from your Stream Deck turning it into a workable Soundboard!

## Configuration
```
{
  "file not found sound": "NOPE.mp3", - Something to play if your file does not exist or is mistyped
  "audio-device": "",                 - Your Preferred audio output device
  "play-param": "play",               - Alias: used for playback skipping
  "loop-param": "loop"                - Alias: used for looping
 }
 ```
 
## Creating and parsing your button
* Using your Elgato Stream Deck, Add a new `Website` button onto your deck
* Add a URL encoded string in the `URL:` Field *e.g.* `http://localhost:4002/C:/Path/To/Your%20Sound%20File.mp3` wav can also be used
* Check off the `Access in background` check box to prevent any new tabs from being opened.

### Available Parameters per file passed
You can pass multiple files through your URL using **+**

*e.g.* `http://localhost:4002/C:/Path/To/Your%20Sound%20File.mp3?play=15+C:/Path/To/Your%20Sound%20File.mp3?play=250&loop=1`
