using Terraria.ModLoader;
using Terraria;
using Terraria.ID;

namespace CSkies.Utilities.Base.BaseMod
{
    public class MNet
    {
        public static void SendBaseNetMessage(int msg, params object[] param)
        {
            if (Main.netMode == NetmodeID.SinglePlayer)
            {
                return; 
            }

            // Create a new ModPacket
            ModPacket packet = ModContent.GetInstance<CSkies>().GetPacket();
            packet.Write((byte)msg); 

            foreach (object obj in param)
            {
                if (obj is int) packet.Write((int)obj);
                else if (obj is float) packet.Write((float)obj);
                else if (obj is bool) packet.Write((bool)obj);
                else if (obj is string) packet.Write((string)obj);
                
            }

            packet.Send(); 
        }
    }
}

		//OLD (moved to CSkies)
		/*public override void NetReceive(BinBuffer bb, int msg, MessageBuffer buffer)
		{
			if (msg == 0) //projectile hostility and ownership
			{
				int owner = bb.ReadInt();
				int projID = bb.ReadInt();
				bool friendly = bb.ReadBool();
				bool hostile = bb.ReadBool();
				if (Main.projectile[projID] != null)
				{
					Main.projectile[projID].owner = owner;
					Main.projectile[projID].friendly = friendly;
					Main.projectile[projID].hostile = hostile;
				}
				if (Main.netMode == 2) SendBaseNetMessage(0, owner, projID, friendly, hostile);
			}else
			if (msg == 1) //sync AI array
			{
				int classID = (int)bb.ReadByte();
				int id = (int)bb.ReadShort();
				int aitype = (int)bb.ReadByte();
				int arrayLength = (int)bb.ReadByte();
				float[] newAI = new float[arrayLength];
				for (int m = 0; m < arrayLength; m++)
				{
					newAI[m] = bb.ReadFloat();
				}
				if (classID == 0 && Main.npc[id] != null && Main.npc[id].active && Main.npc[id].subClass != null && Main.npc[id].subClass is ParentNPC)
				{
					((ParentNPC)Main.npc[id].subClass).SetAI(newAI, aitype);
				}else
				if (classID == 1 && Main.projectile[id] != null && Main.projectile[id].active && Main.projectile[id].subClass != null && Main.projectile[id].subClass is ParentProjectile)
				{
					((ParentProjectile)Main.projectile[id].subClass).SetAI(newAI, aitype);
				}
				if (Main.netMode == 2) BaseNet.SyncAI(classID, id, newAI, aitype);
			}
		}*/