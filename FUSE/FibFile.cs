using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LABO.FUSE
{
    public class FibFile
    {
        public string Path;
        public uint Hash;
        public uint Offset;
        public uint Flags;
        public uint Size; // NOTE: Can be decompressed size if compressed.
        public CompressionFormat Compression;

        public FibFile(uint hash, uint offset, uint flags, uint size, CompressionFormat compression)
        {
            Hash = hash;
            Path = ""; // TODO: Add files path table and search the right one into it based on the hash.

            Offset = offset;
            Flags = flags;
            Size = size;
            Compression = compression;
        }
    }
}
