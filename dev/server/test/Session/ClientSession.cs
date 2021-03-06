using System;
using System.Collections.Generic;
using System.Net;
using System.Text;
using System.Threading;
using core;

namespace test
{
    class ClientSession : PacketSession
    {
        public int SessionId { get; set; }
        public GameRoom Room { get; set; }

        // 원래는 분리
        public float PosX { get; set; }
        public float PosY { get; set; }
        public float PosZ { get; set; }

        public override void OnConnected(EndPoint endPoint)
        {
            Console.WriteLine($"Onconnected: {endPoint}");
            Program.Room.Push(() => Program.Room.Enter(this));
        }
        public override void OnRecvPacket(ArraySegment<byte> buffer)
        {
            PacketManager.Instance.OnRecvPacket(this, buffer);
        }
        public override void OnDisconnected(EndPoint endPoint)
        {
            SessionManager.Instance.Remove(this);
            if (Room != null)
            {
                GameRoom room = Room;
                room.Push(() => room.Leave(this));
                Room = null;
            }

            Console.WriteLine($"OnDisconnected: {endPoint}");
        }
        public override void OnSend(int numOfBytes)
        {
            // Console.WriteLine($"Transferred Byte: {numOfBytes}");
        }
    }
}