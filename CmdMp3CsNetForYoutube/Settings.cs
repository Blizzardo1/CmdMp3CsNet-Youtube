/** Program.cs
A Soundboard program for the Elgato Stream Deck
  Copyright (C) 2018  Adonis S. Deliannis
  This program is free software: you can redistribute it and/or modify
  it under the terms of the GNU General Public License as published by
  the Free Software Foundation, either version 3 of the License, or
  (at your option) any later version.
  This program is distributed in the hope that it will be useful,
  but WITHOUT ANY WARRANTY; without even the implied warranty of
  MERCHANTABILITY or FITNESS FOR A PARTICULAR PURPOSE.  See the
  GNU General Public License for more details.
  You should have received a copy of the GNU General Public License
  along with this program.  If not, see <http://www.gnu.org/licenses/>.
*/

using System;
using System.IO;
using System.Linq;
using NAudio.Wave;
using Newtonsoft.Json;

namespace CmdMp3CsNetForYoutube
{
    public class Settings {
        private const string Default = "settings.json";

        [JsonProperty("Port")]
        public int Port { get; set; }

        [JsonProperty("file not found sound")]
        public string FileNotFoundSound { get; set; }

        [JsonProperty("Device Info")]
        public string DeviceInfo { get; set; }

        [JsonProperty("play-param")]
        public string PlaybackAlias { get; set; }

        [JsonProperty("loop-param")]
        public string LoopAlias { get; set; }

        public static Settings Load(string config = Default)
        {
            if(!File.Exists(config)) {
                new Settings {
                    Port = 4004,
                    DeviceInfo = DirectSoundOut.Devices.First( ).Description,
                    FileNotFoundSound = "",
                    PlaybackAlias = "play",
                    LoopAlias = "loop"
                }.Save( config );
            }

            try {
                string json = File.ReadAllText( config );
                var settings = JsonConvert.DeserializeObject< Settings >( json );
                return settings;
            } catch (Exception ex) {
                Console.WriteLine( ex.Message );
                return null;
            }
        }

        public void Save(string config = Default) {
            string json = JsonConvert.SerializeObject( this, Formatting.Indented );
            File.WriteAllText( config, json );
        }
    }
}
