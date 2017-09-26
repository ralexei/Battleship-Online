using BusinessLogic.GameLogic;

namespace ClientWeb
{
    public class Room
    {
        public string RoomId { get; }
        public Player Player1 { get; }
        public Player Player2 { get; }
        int maps = 0;

        public Room(string id1, string id2)
        {

            Player1 = new Player();
            Player2 = new Player();
            Player1.Name = id1;
            Player2.Name = id2;
            RoomId = id1 + id2;
        }

        public bool HasPlayer(string name)
        {
            if (Player1.Name == name)
                return true;
            if (Player2.Name == name)
                return true;
            return false;
        }
        public void SetMap(string player, Field field)
        {
            if (Player1.Name == player)
            {
                Player1.Field = field;
                maps++;
            }
            if (Player2.Name == player)
            {
                Player2.Field = field;
                maps++;
            }

        }

        public bool ChangePlayerID(string oldId, string newId)
        {
            if (Player1.Name == oldId)
            {
                Player1.Name = newId;
                return true;
            }
            else if (Player2.Name == oldId)
            {
                Player2.Name = newId;
                return true;
            }
            return false;
        }

        public bool IsReady()
        {
            return maps == 2;
        }
    }
}