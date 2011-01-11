﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using OpenMaple.IO;
using OpenMaple.Networking;
using OpenMaple.Server.Registry;
using OpenMaple.Tools;

namespace OpenMaple.Server
{
	partial class Player
	{
        public void HandleBuddyOperation(PacketReader reader, NetworkSession session)
        {
            var operation = (BuddyOperation) reader.ReadByte();

            switch (operation)
            {
                case BuddyOperation.AddBuddy:
                    HandleAddBuddy(reader, session);
                    return;
                case BuddyOperation.AcceptRequest:
                    return;
                case BuddyOperation.RemoveBuddy:
                    return;
            }

            // If the operation is undefined...
            session.Close();
        }

        private void HandleAddBuddy(PacketReader reader, NetworkSession session)
        {
	        string name;
            if (!reader.TryReadLengthString(out name) || name.Length > 12)
            {
                session.Close();
                return;
            }

            string groupName;
            if (!reader.TryReadLengthString(out groupName) || groupName.Length > 15)
            {
                session.Close();
                return;
            }

            BuddyListEntry entry = this.buddyList.FirstOrDefault(
                buddy => buddy.Name.Equals(name, StringComparison.OrdinalIgnoreCase));

            if (entry != null)
            {
                SendBuddyListOperationResult(session, BuddyOperationResult.AlreadyOnList);
                return;
            }

            if (buddyList.IsFull)
            {
                SendBuddyListOperationResult(session, BuddyOperationResult.BuddyListFull);
                return;
            }

            // TODO: Check the target buddy list stuff.
        }

	    private static void SendBuddyListOperationResult(NetworkSession session, BuddyOperationResult buddyOperationResult)
	    {
            using (var builder = new PacketBuilder(3))
            {
                builder.WriteOpCode("BuddyListOperationResponse");
                builder.WriteByte((byte) buddyOperationResult);
                session.Write(builder.ToByteArray());
            }
	    }
	}
}
