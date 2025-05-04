using Terraria;
using Terraria.ModLoader;
using Terraria.IO;
using Terraria.ModLoader.IO;
using System.IO;

namespace CSkies.Utilities
{
    public class CSystem : ModSystem
    {
        public static bool _Observer = false;
        public static bool _FurySoul = false;
        public static bool _Heartcore = false;
        public static bool _ObserverVoid = false; 
        public static bool _Starcore = false;
        public static bool _Void = false;

        public override void ClearWorld()
        {
            _Observer = false;
            _FurySoul = false;
            _Heartcore = false;
            _ObserverVoid = false;
            _Starcore = false;
            _Void = false;
        }

        public override void SaveWorldData(TagCompound tag)
        {
            if (_Observer)
            {
                tag["_Observer"] = true;
            }
            if (_FurySoul)
            {
                tag["_FurySoul"] = true;
            }
            if (_Heartcore)
            {
                tag["_Heartcore"] = true;
            }
            if (_ObserverVoid)
            {
                tag["_ObserverVoid"] = true;
            }
            if (_Starcore)
            {
                tag["_Starcore"] = true;
            }
            if (_Void)
            {
                tag["_Void"] = true;
            }
        }
        public override void NetSend(BinaryWriter writer) // не трогать
        {
            writer.WriteFlags(_Observer, _FurySoul, _Heartcore, _ObserverVoid, _Starcore, _Void);
        }
        public override void NetReceive(BinaryReader reader) // не трогать
        {
            reader.ReadFlags(out _Observer, out _FurySoul, out _Heartcore, out _ObserverVoid, out _Starcore, out _Void);
        }
        public override void LoadWorldData(TagCompound tag)
        {
            _Observer = tag.ContainsKey("_Observer");
            _FurySoul = tag.ContainsKey("_FurySoul");
            _Heartcore = tag.ContainsKey("_Heartcore");
            _ObserverVoid = tag.ContainsKey("_ObserverVoid");
            _Starcore = tag.ContainsKey("_Starcore");
            _Void = tag.ContainsKey("_Void");
        }
    }
}