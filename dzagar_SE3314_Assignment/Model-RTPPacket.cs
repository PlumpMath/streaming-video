﻿namespace dzagar_SE3314_Assignment
{
    class RTPPacket
    {
        int interval = 100; //Use to create timestamp
        byte[] completePacket; //Entire packet
        byte[] header; //Just header bytes of packet

        public RTPPacket(int seqNo, byte[] contents)
        {
            long timestamp = seqNo * interval;
            EncapsulateRTPPacket(seqNo, timestamp, contents);
        }

        public void EncapsulateRTPPacket(int seqNo, long timestamp, byte[] contents)    //Creates RTP Packet, stores in header/completePacket
        {
            //Initialize header values
            int V = 2, P = 0, X = 0, CC = 0, M = 0, PT = 26;
            long SSRC = 17;
            if (contents != null)
            {
                int payload = contents.Length;
                //Set header size (12 bytes)
                header = new byte[12];
                //Create packet with payload plus 12 bytes of header
                completePacket = new byte[payload + 12];
                //Shift bits to proper spot in header by AND operator as well as left and right shift!
                completePacket[0] = (byte)((V & 0x3) << 6 | (P & 0x1) << 5 | (X & 0x1) << 4 | (CC & 0xf));
                completePacket[1] = (byte)((M & 0x1) << 7 | (PT & 0x7f));
                //Sequence # is 2 bytes. Most significant byte goes first in BIG ENDIAN
                completePacket[2] = (byte)((seqNo & 0xff00) >> 8);
                completePacket[3] = (byte)((seqNo & 0x00ff));
                //Timestamp is 32bits = 4 bytes, BIG ENDIAN
                completePacket[4] = (byte)((timestamp & 0xff000000) >> 24);
                completePacket[5] = (byte)((timestamp & 0x00ff0000) >> 16);
                completePacket[6] = (byte)((timestamp & 0x0000ff00) >> 8);
                completePacket[7] = (byte)((timestamp & 0x000000ff));
                //SSRC is 32 bits = 4 bytes, BIG ENDIAN
                completePacket[8] = (byte)((SSRC & 0xff000000) >> 24);
                completePacket[9] = (byte)((SSRC & 0x00ff0000) >> 16);
                completePacket[10] = (byte)((SSRC & 0x0000ff00) >> 8);
                completePacket[11] = (byte)((SSRC & 0x000000ff));
                //Copy into header var
                for (int i = 0; i < 12; i++)
                {
                    header[i] = completePacket[i];
                }
                //Populate rest of the packet with data
                for (int i = 0; i < contents.Length; i++)
                {
                    completePacket[12 + i] = contents[i];
                }
            }
        }

        //GET FUNCTIONS

        public byte[] GetPacketBytes()      //Get packet as byte array
        {
            return completePacket;
        }

        public byte[] GetPacketHeader()     //Get packet header as byte array
        {
            return header;
        }
    }
}
