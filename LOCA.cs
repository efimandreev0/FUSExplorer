using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LABO
{
    internal class LOCA
    {
        private struct Header
        {
            public string Magic;
            public int Ver;
            public int Count;
            public int TocSize;
            public int TextSize;
        }
        private struct Entry
        {
            public uint Hash;
            public int Offset;
        }
        public static string[] Read(string file)
        {
            using (BinaryReader reader = new(File.OpenRead(file)))
            {
                Encoding.RegisterProvider(CodePagesEncodingProvider.Instance);
                List<string> strings = [];
                List<Entry> en = [];
                Header h = new()
                {
                    Magic = Encoding.UTF8.GetString(reader.ReadBytes(4)),
                    Ver = reader.ReadInt32(),
                    Count = reader.ReadInt32(),
                    TocSize = reader.ReadInt32(),
                    TextSize = reader.ReadInt32()
                };

                for (int i = 0; i < h.Count; i++)
                {
                    en.Add(new()
                    {
                        Hash = reader.ReadUInt32(),
                        Offset = reader.ReadInt32() + h.TocSize + 0x14
                    });
                }
                for (int i = 0; i < h.Count; i++)
                {
                    reader.BaseStream.Position = en[i].Offset;
                    strings.Add(Utils.ReadString(reader, Encoding.GetEncoding("ISO-8859-15")).Replace("\n", "<lf>").Replace("\r", "<br>"));
                }
                return strings.ToArray();
            }
        }
        public static void Write(string file, string[] strings)
        {
            Encoding.RegisterProvider(CodePagesEncodingProvider.Instance);
            int[] pointers = new int[strings.Length];
            using (BinaryWriter writer = new(File.OpenWrite(file)))
            {
                writer.BaseStream.Position += 20 + strings.Length * 8;
                for (int i = 0; i < pointers.Length; i++)
                {

                    pointers[i] = (int)writer.BaseStream.Position - (0x14 + (strings.Length * 8));
                    Console.WriteLine(pointers[i]);
                    writer.Write(Encoding.GetEncoding("ISO-8859-15").GetBytes(strings[i].Replace("<lf>", "\n").Replace("<br>", "\r")));
                    writer.Write((byte)0x0);
                }
                writer.BaseStream.Position = 0x10;
                writer.Write(((int)writer.BaseStream.Length - (0x14 + strings.Length * 8)));
                for (int i = 0; i < pointers.Length; i++)
                {
                    writer.BaseStream.Position += 4;
                    writer.Write((int)pointers[i]);
                }
            }
        }
    }
}
