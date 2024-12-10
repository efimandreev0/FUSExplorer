# Formats
## .fib:
### Header ###
Magic: FUSE1.00
Count: UInt32
Field0C: UInt32
TocOffset: UInt32
### Entry ###
Hash: UInt32
Offset: UInt32
DecSize\Flag: UInt32 >> 5 ; UInt32 & 3 (1 - RefPack, 3 - Inflate)
## .loc:
### Header ###
Magic: LOCA (UInt32: 0x41434F4C)
Version: UInt32 (Every file - 2)
Count: UInt32
TocSize: UInt32
TextBlockSize: UInt32
### Entry ###
Hash: UInt32
RelPointer: UInt32 (TocSize + 0x14 + RelPointer);

# About LABO
Labo is experemintal tool to edit lego-games. Now it can Unpack\Pack .loc and .fib-files. Fun Fact: at .fib archive toc under all bytes, but nobody can you prohibit write toc after the header at 0x14, and at the tocOffset write 0x14.
