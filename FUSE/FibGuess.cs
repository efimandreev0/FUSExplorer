using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Text;
using System.Threading.Tasks;

namespace LABO.FUSE
{
    internal class FibGuess
    {
        public static byte[] UnRef(byte[] srcBuffer)
        {
            List<byte> dstBuffer = [];
            int position = 0;
            int readCount = 0;

            while (position < srcBuffer.Length)
            {
                byte b = srcBuffer[position];
                position += 1;

                if (b >= 0xFC)
                {
                    readCount = b - 0xFC;

                    for (int i = position; i < position + readCount; i++)
                    {
                        dstBuffer.Add(srcBuffer[i]);
                    }

                    position += readCount;

                    break;
                }

                // Raw data.
                // 111xxxxx
                // Count -> (x + 1) * 4
                if (b >= 0xE0)
                {
                    readCount = ((b & 0b11111) + 1) * 4;

                    for (int i = position; i < position + readCount; i++)
                    {
                        dstBuffer.Add(srcBuffer[i]);
                    }

                    position += readCount;
                }
                else // Raw data, then copy from the output buffer.
                {
                    int rawCount = 0;
                    int offset = 0;

                    // 110xxyyz zzzzzzzz zzzzzzzz yyyyyyyy
                    // Raw count  -> x
                    // Read count -> y + 5
                    // Offset     -> z + 1
                    if (b >= 0xC0)
                    {
                        byte b2 = srcBuffer[position];
                        byte b3 = srcBuffer[position + 1];
                        byte b4 = srcBuffer[position + 2];

                        position += 3;

                        rawCount = (b & 0b00011000) >> 3;
                        readCount = ((b & 0b00000110) << 7) + b4 + 5;
                        offset = ((b & 1) << 0b00000001) + (b2 << 8) + b3 + 1;
                    }
                    // 10yyyyyy xxzzzzzz zzzzzzzz
                    // Raw count  -> x
                    // Read count -> y + 4
                    // Offset     -> z + 1
                    else if (b >= 0x80)
                    {
                        byte b2 = srcBuffer[position];
                        byte b3 = srcBuffer[position + 1];

                        position += 2;

                        rawCount = b2 >> 6;
                        readCount = (b & 0b00111111) + 4;
                        offset = ((b2 & 0b00111111) << 8) + b3 + 1;
                    }
                    // 0yyyxxzz zzzzzzzz
                    // Raw count  -> x
                    // Read count -> y + 3
                    // Offset     -> z + 1
                    else
                    {
                        byte b2 = srcBuffer[position];

                        position += 1;

                        rawCount = (b & 0b00001100) >> 2;
                        readCount = ((b & 0b01110000) >> 4) + 3;
                        offset = ((b & 0b00000011) << 8) + b2 + 1;
                    }

                    for (int i = position; i < position + rawCount; i++)
                        dstBuffer.Add(srcBuffer[i]);

                    if (dstBuffer.Count + 1 >= 32) return [.. dstBuffer];

                    position += rawCount;

                    for (int i = 0; i < readCount; i++)
                        dstBuffer.Add(dstBuffer.ToArray()[dstBuffer.Count - offset]);

                    if (dstBuffer.Count + 1 >= 32) return [.. dstBuffer];
                }
            }

            return [.. dstBuffer];
        }

        private struct MagicExtension
        {
            public byte[] Magic { get; }
            public int Offset { get; }
            public string Extension { get; }

            public MagicExtension(byte[] magic, int offset, string extension)
            {
                Magic = magic;
                Offset = offset;
                Extension = extension;
            }
        }

        public static string GuessExt(byte[] data, string platform = null)
        {
            var extensions = new List<MagicExtension> {
                new MagicExtension(new byte[] { 0x01, 0x00, 0x00, 0x00, 0x2C, 0x00, 0x00, 0x00, 0x2C, 0x00, 0x00, 0x00 }, 0, ".bwav"),
                new MagicExtension(new byte[] { 0xF2, 0xFF, 0xFF, 0xFF, 0x2C, 0x00, 0x00, 0x00 }, 8, ".bwav"),
                new MagicExtension(new byte[] { 0x01, 0x00, 0x00, 0x00, 0x20, 0x00, 0x00, 0x00, 0x20, 0x00, 0x00, 0x00 }, 0, platform == "PLATFORM_PSP" ? ".bwav" : ".btga"),
                new MagicExtension(new byte[] { 0x00, 0x04, 0x00, 0x00, 0x10, 0x00, 0x00, 0x00, 0xF2, 0xFF, 0xFF, 0xFF, 0x20, 0x00, 0x00, 0x00 }, 0, ".btga"),
                new MagicExtension(new byte[] { 0x00, 0x04, 0x00, 0x00, 0x10, 0x00, 0x00, 0x00, 0xF2, 0xFF, 0xFF, 0xFF, 0x1C, 0x00, 0x00, 0x00 }, 0, ".btga"),
                new MagicExtension(new byte[] { 0x50, 0x00, 0x00, 0x00 }, 8, ".btga"),
                new MagicExtension(new byte[] { 0x04, 0x00, 0x00, 0x00 }, 8, ".bxaml"),
                new MagicExtension(new byte[] { 0x08, 0x00, 0x00, 0x00 }, 8, ".bxls"),
                new MagicExtension(new byte[] { 0x10, 0x00, 0x00, 0x00 }, 8, ".fnskl"),
                new MagicExtension(new byte[] { 0x1C, 0x00, 0x00, 0x00 }, 8, ".fnanm"),
                new MagicExtension(new byte[] { 0x24, 0x00, 0x00, 0x00 }, 8, ".fnmdl"),
                new MagicExtension(new byte[] { 0x28, 0x00, 0x00, 0x00 }, 8, ".fnmdl"),
                new MagicExtension(new byte[] { 0x3C, 0x00, 0x00, 0x00 }, 8, ".lvl"),
                new MagicExtension(new byte[] { 0x44, 0x00, 0x00, 0x00 }, 8, ".lvl"),
                new MagicExtension(System.Text.Encoding.ASCII.GetBytes("CameraFollow"), 0, ".cam"),
                new MagicExtension(System.Text.Encoding.ASCII.GetBytes("leCameraFollow"), 0, ".cam"),
                new MagicExtension(System.Text.Encoding.ASCII.GetBytes("NCSC"), 0, ".ncsc"),
                new MagicExtension(new byte[] { 0xF0, 0xFF, 0xFF, 0xFF }, 8, ".fnanm"),
                new MagicExtension(new byte[] { 0xF1, 0xFF, 0xFF, 0xFF, 0x38, 0x00, 0x00, 0x00 }, 8, ".lvl"),
                new MagicExtension(new byte[] { 0xF1, 0xFF, 0xFF, 0xFF, 0x3C, 0x00, 0x00, 0x00 }, 8, ".lvl"),
                new MagicExtension(new byte[] { 0xF2, 0xFF, 0xFF, 0xFF, 0x04, 0x00, 0x00, 0x00 }, 8, ".bxaml"),
                new MagicExtension(new byte[] { 0xF2, 0xFF, 0xFF, 0xFF, 0x08, 0x00, 0x00, 0x00 }, 8, ".bxls"),
                new MagicExtension(new byte[] { 0xF2, 0xFF, 0xFF, 0xFF, 0x10, 0x00, 0x00, 0x00 }, 8, ".fnskl"),
                new MagicExtension(new byte[] { 0xF2, 0xFF, 0xFF, 0xFF, 0x24, 0x00, 0x00, 0x00 }, 8, ".fnmdl"),
                new MagicExtension(new byte[] { 0xF2, 0xFF, 0xFF, 0xFF, 0x28, 0x00, 0x00, 0x00 }, 8, ".fnmdl"),
                new MagicExtension(System.Text.Encoding.ASCII.GetBytes("LOCA"), 0, ".loc"),
                new MagicExtension(new byte[] { 0x31, 0x0D, 0x0A, 0x30, 0x30, 0x3A }, 0, ".srt"),
                new MagicExtension(new byte[] { 0x47, 0x58, 0x50, 0x00, 0x01, 0x05, 0x50, 0x02 }, 0, ".gxp"),
                new MagicExtension(System.Text.Encoding.ASCII.GetBytes("TRUEVISION-XFILE."), -18, ".tga"),
                new MagicExtension(new byte[] { 0x89, 0x50, 0x4E, 0x47 }, 0, ".png"),
                new MagicExtension(new byte[] { 0xFF, 0xD8, 0xFF, 0xDB }, 0, ".jpg"),
                new MagicExtension(new byte[] { 0xFF, 0xD8, 0xFF, 0xE0 }, 0, ".jpg"),
                new MagicExtension(new byte[] { 0xFF, 0xD8, 0xFF, 0xE1 }, 0, ".jpg"),
                new MagicExtension(new byte[] { 0x42, 0x4D }, 0, ".bmp"),
            
                new MagicExtension(System.Text.Encoding.ASCII.GetBytes("WAVE"), 8, ".wav"),
                new MagicExtension(new byte[] { 0x4F, 0x67, 0x67, 0x53 }, 0, ".ogg")
            };

            foreach (var ext in extensions) {
                if (data.Length >= ext.Offset + ext.Magic.Length && ByteArrayCompare(data, ext.Offset, ext.Magic)) {
                    return ext.Extension;
                }
            }

            return "Unknown";
        }
        private static bool ByteArrayCompare(byte[] data, int offset, byte[] magic)
        {
            // Убедимся, что offset не выходит за границы массива data
            if (offset < 0 || offset + magic.Length > data.Length)
                return false;

            for (int i = 0; i < magic.Length; i++) {
                if (data[offset + i] != magic[i])
                    return false;
            }
            return true;
        }
    }
}
