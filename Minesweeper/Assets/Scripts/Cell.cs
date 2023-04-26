using System.Collections;
using UnityEngine;

namespace Assets.Scripts
{
    public class Cell
    {
        public enum Type
        {
            Invalid,
            Number,
            Mine,
        }

        public enum Status
        {
            Unreveal,
            Revealed,
            Flagged,
        }

        public int x, y;
        public Type type;
        public Status status;
        public int number;
        public bool exploded = false;

        public Cell(int x, int y)
        {
            this.x = x;
            this.y = y;
            type = Type.Number;
            number = 0;
        }
    }
}