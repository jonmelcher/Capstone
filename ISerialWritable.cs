namespace Capstone
{
    public interface ISerialWritable
    {
        void Write(byte b);
        void Write(byte[] arr);
    }
}