using System;
using System.Linq;

namespace Capstone
{
    public struct RFIDToken
    {
        private const int ID_LENGTH = 10;  // const; all tags are 10 digits in length, or they're invalid
        private readonly byte[] _id;  // the RFID key

        // checks for a valid id
        public RFIDToken(byte[] id)
        {
            if (id == null)
                throw new ArgumentNullException("ID has not been initialized!");
            if (id.Length != Length)
                throw new ArgumentException("ID is not the correct length");
            if (id.Any(b => !char.IsLetterOrDigit(b)))
                throw new ArgumentException("ID is not alphanumeric!");

            _id = id;
        }

        public byte this[int i] { get { return _id[i]; } }
        public int Length => ID_LENGTH;

        public override string ToString()
        {
            if (_id == null)
                return null;

            return string.Join("", _id);
        }
    }
}
