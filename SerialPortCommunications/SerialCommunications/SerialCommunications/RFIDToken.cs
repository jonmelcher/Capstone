// ********************************************************************************
//  filename    :   RFIDToken.cs
//  purpose     :   container for valid ID scans from the Parallax28140 RFID Reader
//
//  written by Jonathan Melcher and Brennan MacGregor
//  last updated 2016-03-30
// ********************************************************************************


using System;
using System.Linq;


namespace SerialPortCommunications
{
    public class RFIDToken
    {
        private const int ID_LENGTH = 10;
        private readonly byte[] _id;

        public static int Length => ID_LENGTH;
        public byte this[int i] { get { return _id[i]; } }

        public RFIDToken(byte[] id)
        {
            if (id == null)
                throw new ArgumentNullException("ID has not been initialized!");
            if (id.Length != Length)
                throw new ArgumentException("ID is not the correct length");
            if (id.Any(b => !char.IsLetterOrDigit((char)b)))
                throw new ArgumentException("ID is not alphanumeric!");

            _id = id;
        }

        public override string ToString()
        {
            if (_id == null)
                return "";

            return new string(_id.Select(b => (char)b).ToArray());
        }
    }
}
