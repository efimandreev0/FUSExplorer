using System;
using System.IO;
using System.Collections.Generic;
using System.IO.Compression;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using LABO.FUSE;
using System.Reflection;

namespace LABO {
    public class FibArchive {
        public const string MagicFuse = "FUSE1.00";
        public uint filesCount;
        public uint filesTocOffset;
        public List<FibFile> Files = [];

        public string ArchiveFilePath;
        public List<string> Hashes = File.ReadAllLines("hash.txt").ToList();
        public string[] Names = File.ReadAllLines("Names.txt");

        public FibArchive(string archiveFilePath) {

            ArchiveFilePath = archiveFilePath;

            using FileStream stream = new(ArchiveFilePath, FileMode.Open, FileAccess.Read);
            using BinaryReader reader = new(stream);

            if (Encoding.UTF8.GetString(reader.ReadBytes(8)) != MagicFuse) {
                MessageBox.Show("It's not a FUSE-file/It's not supported FUSE-file version.");
                return;
            }

            filesCount = reader.ReadUInt32();
            uint foldersCount = reader.ReadUInt32();
            filesTocOffset = reader.ReadUInt32(); // Include the header.

            stream.Seek(filesTocOffset, SeekOrigin.Begin);

            for (int i = 0; i < filesCount; i++) {
                uint hash = reader.ReadUInt32();
                uint offset = reader.ReadUInt32();
                uint flags = reader.ReadUInt32();

                // TODO: This needs improvement to support all FIB archives versions.
                Files.Add(new FibFile(hash, offset, flags, flags >> 5, (CompressionFormat)(flags & 3)));

                int index = Hashes.IndexOf("0x" + hash.ToString("x8"));
                if (index != -1)
                    Files[i].Path = Names[index];
                else {
                    var pos = reader.BaseStream.Position;
                    reader.BaseStream.Position = Files[i].Offset;
                    int size = reader.ReadInt32();
                    byte[] uncm = reader.ReadBytes(512);
                    string tmp = "";
                    if (Files[i].Compression != CompressionFormat.None) {
                        switch (Files[i].Compression) {
                            case (CompressionFormat.Refpack):
                                try {
                                    tmp = FibGuess.GuessExt(FibGuess.UnRef(uncm));
                                }
                                catch {
                                    tmp = FibGuess.GuessExt((uncm));
                                }
                                break;
                            case (CompressionFormat.Inflate):
                                reader.BaseStream.Position -= 512;
                                tmp = FibGuess.GuessExt(Inflate.Decompress(reader.ReadBytes(size)));
                                break;
                        }
                    }
                    else tmp = FibGuess.GuessExt(uncm);
                    string path = "!NoName";
                    if (tmp != "Unknown")
                        path += $"\\{tmp.Substring(1, tmp.Length - 1)}\\0x{hash.ToString("x8")}{tmp}";
                    else
                        path += $"\\Unknown\\0x{hash.ToString("x8")}.bin";
                    Files[i].Path = path;
                    reader.BaseStream.Position = pos;
                }
                
            }
        }
        public void ReplaceFile(FibFile file, string rfile) {
            int index = Files.IndexOf(file);
            byte[] b = File.ReadAllBytes(rfile);
            Files[index].Size = (uint)b.Length;

            using (BinaryWriter writer = new BinaryWriter(File.OpenWrite(ArchiveFilePath))) {
                Files[index].Offset = (uint)writer.BaseStream.Length;
                writer.BaseStream.Position = filesTocOffset + (12 * index) + 4;
                writer.Write((uint)writer.BaseStream.Length);
                writer.Write((uint)(b.Length << 5));
                writer.BaseStream.Position = writer.BaseStream.Length;
                writer.Write(b);
            }
            /*FileStream stream = new(ArchiveFilePath, FileMode.Open, FileAccess.Read);
            BinaryReader reader = new(stream);

            //byte[] bef = reader.ReadBytes((int)file.Offset);
            stream.Seek(file.Offset, SeekOrigin.Begin);
            int size = (int)file.Size;
            int index = Files.IndexOf(file);

            if (file.Compression != CompressionFormat.None) 
            {
                size = reader.ReadInt32();
                file.Size = (uint)size;
            }
            stream.Seek(size, SeekOrigin.Current);
            byte[] aft = reader.ReadBytes((int)(reader.BaseStream.Length - reader.BaseStream.Position));
            reader.Close();
            stream.Close();

            byte[] toWrite = File.ReadAllBytes(rfile);
            int bCount = toWrite.Length - size;
            using (BinaryWriter writer = new BinaryWriter(File.OpenWrite(ArchiveFilePath)))
            {
                writer.BaseStream.Position = file.Offset;
                file.Size = (uint)toWrite.Length;
                file.Flags = (uint)(toWrite.Length << 5);
                writer.Write(toWrite);
                writer.Write(aft);
                filesTocOffset = (uint)((int)(filesTocOffset) + bCount);
                writer.BaseStream.Position = filesTocOffset;
                for (int i = 0; i < filesCount; i++)
                {
                    if (i == index) Files[i] = file;
                    Files[i].Offset = (uint)((int)Files[i].Offset + bCount);
                    writer.Write(Files[i].Hash);
                    writer.Write(Files[i].Offset);
                    writer.Write(Files[i].Flags);
                }
                writer.BaseStream.Position = 0x10;
                writer.Write(filesTocOffset);
            }*/

        }
        public byte[] ExtractFile(FibFile file, bool plainData = false) {
            Directory.CreateDirectory(ArchiveFilePath.Replace(Path.GetExtension(ArchiveFilePath), "") + "\\" + Path.GetDirectoryName(file.Path));
            using FileStream stream = new(ArchiveFilePath, FileMode.Open, FileAccess.Read);
            using BinaryReader reader = new(stream);

            stream.Seek(file.Offset, SeekOrigin.Begin);

            uint size = file.Size;
            List<byte> endData = [];
            while (file.Size > endData.Count) { 
                if (file.Compression != CompressionFormat.None)
                    size = reader.ReadUInt32();
                byte[] fileData = new byte[size];
                stream.Read(fileData, 0, (int)size);
                if (plainData) {
                    switch (file.Compression) {
                        case CompressionFormat.Inflate: {
                                endData.AddRange(Inflate.Decompress(fileData));
                                break;
                            }
                        case CompressionFormat.Refpack: {
                                endData.AddRange(UnRefpack.Decompress(fileData));
                                break;
                            }
                        case CompressionFormat.None: {
                                endData.AddRange(fileData);
                                break;
                            }
                    }
                }
            }
            return endData.ToArray();
        }
    }
}
