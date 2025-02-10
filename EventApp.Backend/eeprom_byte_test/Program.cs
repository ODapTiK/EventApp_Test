namespace eeprom_byte_test
{
    internal class Program
    {
        byte[] bytes = new byte[1006];
        static void Main(string[] args)
        {
            AddTiks(1000);
        }

        void AddTiks(short tiks)
        {
            if (bytes[0] == bytes[2] && bytes[0] == bytes[4] && bytes[2] == bytes[4])
            {
                if (bytes[1] == bytes[3] && bytes[1] == bytes[5] && bytes[3] == bytes[5])
                {
                    short firstTwoBytes = (short)((bytes[1] << 8) | bytes[0]);
                    short counter = (short)(firstTwoBytes % 1000);
                    if (counter % 2 == 0)
                    {
                        short currenntTwoBytesTiks = (short)((bytes[counter*2 + 1] << 8) | bytes[counter*2]);
                        if(currenntTwoBytesTiks > short.MaxValue - tiks)
                        {
                            short newTiks = (short)Math.Abs(currenntTwoBytesTiks - tiks);
                            bytes[counter*2+2] = (byte)(newTiks & 0xFF);         // Младший байт
                            bytes[counter*2+3] = (byte)((newTiks >> 8) & 0xFF);  // Старший байт
                            counter++;
                            for(int i = 0; i < 6; i+=2)
                            {
                                bytes[i] = (byte)(counter & 0xFF);
                                bytes[i+1] = (byte)((counter >> 8) & 0xFF);
                            }
                        }
                        else
                        {
                            currenntTwoBytesTiks += tiks;
                            bytes[counter * 2] = (byte)(currenntTwoBytesTiks & 0xFF);         // Младший байт
                            bytes[counter * 2 + 1] = (byte)((currenntTwoBytesTiks >> 8) & 0xFF);  // Старший байт
                        }
                    }
                    else
                    {
                        //TO DO big endian byte order
                    }
                }
                else
                {
                    if (bytes[1] != bytes[3] && bytes[1] != bytes[5])
                    {
                        bytes[1] = bytes[3];
                    }
                    else if (bytes[3] != bytes[5] && bytes[3] != bytes[1])
                    {
                        bytes[3] = bytes[5];
                    }
                    else bytes[5] = bytes[1];
                }
            }
            else
            {
                if (bytes[0] != bytes[2] && bytes[0] != bytes[4])
                {
                    bytes[0] = bytes[2];
                }
                else if (bytes[2] != bytes[4] && bytes[2] != bytes[0])
                {
                    bytes[2] = bytes[4];
                }
                else bytes[4] = bytes[0];
            }
        }
    }
}
