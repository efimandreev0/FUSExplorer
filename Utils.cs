using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.IO;
using System.Drawing;
using System.IO.Compression;
using System.Text;
using System.Text.Json.Serialization;

namespace LABO
{
    public static class Utils
    {
        public static string AutoFitLine(string text, int maxLength)
        {
            string[] splitted = text.Split();
            string result = "";
            int symbols = 0;
            int line_count = 0;
            foreach (var word in splitted)
            {
                if (word.Length + symbols > maxLength)
                {
                    result += "\n";
                    symbols = word.Length;
                    result += word;
                    line_count++;
                }
                else
                {
                    if (symbols > 0)
                    {
                        result += " ";
                        symbols++;
                    }
                    result += word;
                    symbols += word.Length;
                }
            }
            return result;
        }
        public static byte[] ReadByteArray(BinaryReader reader, int offset, int size)
        {
            byte[] result = new byte[size];
            var savepos = reader.BaseStream.Position;
            reader.BaseStream.Position = offset;
            result = reader.ReadBytes(size);
            reader.BaseStream.Position = savepos;
            return result;
        }
        public static byte[] DecompressZlib(byte[] data)
        {
            using (var compressedStream = new MemoryStream(data))
            using (var zipStream = new GZipStream(compressedStream, CompressionMode.Decompress))
            using (var resultStream = new MemoryStream())
            {
                zipStream.CopyTo(resultStream);
                return resultStream.ToArray();
            }
        }
        public static string Reverse(string s)
        {
            char[] charArray = s.ToCharArray();
            Array.Reverse(charArray);
            return new string(charArray);
        }
        public static string[] GetFiles(string directoryPath)
        {
            List<string> sorted = [];
            // Получаем директории _FBIN и _TBB1
            var fbinDirectories = Directory.GetDirectories(directoryPath, "*_FBIN", SearchOption.TopDirectoryOnly).ToList();
            var tbb1Directories = Directory.GetDirectories(directoryPath, "*_TBB1", SearchOption.AllDirectories).ToList();
            var text = Directory.GetFiles(directoryPath, "*.txt", SearchOption.AllDirectories).ToList();

            for (int i = 0; i < fbinDirectories.Count; i++)
            {
                for (int a = 0; a < tbb1Directories.Count; a++)
                {
                    if (tbb1Directories[a].Contains(fbinDirectories[i]))
                        tbb1Directories.Remove(tbb1Directories[a]);
                }
            }
            sorted.AddRange(fbinDirectories);
            sorted.AddRange(tbb1Directories);
            for (int i = 0; i < fbinDirectories.Count; i++)
            {
                for (int a = 0; a < tbb1Directories.Count; a++)
                {
                    for (int b = 0; b < text.Count; b++)
                    {
                        if (text[b].Contains(fbinDirectories[i]))
                            text.Remove(text[b]);
                        else if (text[b].Contains(tbb1Directories[a]))
                            text.Remove(text[b]);
                    }
                }
            }
            sorted.AddRange(text);
            return sorted.ToArray();
        }


        public static string[] GetFolders(string folderPath)
        {
            // Получаем все папки в указанной папке, кроме скрытых
            return Directory.GetDirectories(folderPath, "*_*", SearchOption.TopDirectoryOnly)
                            .Where(d => (File.GetAttributes(d) & FileAttributes.Hidden) != FileAttributes.Hidden)
                            .ToArray();
        }
        public static string GetSMTMagic(string file)
        {
            BinaryReader reader;
            try
            {
                reader = new BinaryReader(File.OpenRead(file));
            }
            catch
            {
                return "nonmagic";
            }
            string Magic = "";
            try
            {
                try
                {
                    if (reader.ReadInt32() != 0)
                    {
                        reader.BaseStream.Position -= 4;
                        Magic = Encoding.UTF8.GetString(reader.ReadBytes(4));
                    }
                    else
                    {
                        Magic = Encoding.UTF8.GetString(reader.ReadBytes(4));
                        reader.BaseStream.Position -= 4;
                    }
                }
                catch
                {
                    reader.Close();
                    return Magic;
                }
                reader.BaseStream.Position -= 4;
                reader.Close();
            }
            catch
            {
                reader.Close();
                return Magic;
            }
            reader.Close();
            return Magic;
        }
        public static void CopyTextFiles(string sourcePath, string targetPath)
        {
            string[] txt = Directory.GetFiles(sourcePath, "*.txt", SearchOption.AllDirectories);
            for (int i = 0; i < txt.Length; i++)
            {
                string[] Paths = txt[i].Split(Path.DirectorySeparatorChar);
                string copyed = targetPath;
                for (int a = 0; a < Paths.Length; a++)
                {
                    if (Paths[a] == "")
                        continue;
                    else if (Paths[a].Contains(".txt"))
                    {
                        if (Paths[a].Contains("_msg2"))
                        {
                            try
                            {
                                File.Copy(txt[i].Replace("_msg2.txt", ".mtbl"), copyed + "/" + Paths[a].Replace("_msg2.txt", ".mtbl"), true);
                            }
                            catch
                            {
                                File.Copy(txt[i].Replace("_msg2.txt", ".mbm"), copyed + "/" + Paths[a].Replace("_msg2.txt", ".mbm"), true);
                            }
                        }
                        File.Copy(txt[i], copyed + "/" + Paths[a], true);
                        continue;
                    }

                    Directory.CreateDirectory(copyed + "/" + Paths[a]);
                    copyed += "/" + Paths[a];
                }
            }
            Console.WriteLine($"All .txt files have been copied to '{targetPath}'.");
        }
        public static void SearchWordInFolder(string folderPath, string searchWord)
        {
            // Список найденных вхождений
            List<FileResult> results = new List<FileResult>();

            // Рекурсивно ищем файлы во всех подпапках
            SearchFilesRecursive(folderPath, searchWord, results);

            // Выводим результаты
            if (results.Count > 0)
            {
                Console.WriteLine($"Found '{searchWord}' in {results.Count} files:");
                foreach (var result in results)
                {
                    Console.WriteLine($"File: {result.FileName}, Line: {result.LineNumber}, Position: {result.Position}");
                }
            }
            else
            {
                Console.WriteLine($"'{searchWord}' not found in any files.");
            }
        }
        static void SearchFilesRecursive(string folderPath, string searchWord, List<FileResult> results)
        {
            // Получаем все файлы в папке
            string[] files = Directory.GetFiles(folderPath);

            // Обрабатываем каждый файл
            foreach (string file in files)
            {
                SearchInFile(file, searchWord, results);
            }

            // Рекурсивно обрабатываем подпапки
            string[] directories = Directory.GetDirectories(folderPath);
            foreach (string directory in directories)
            {
                SearchFilesRecursive(directory, searchWord, results);
            }
        }
        static void SearchInFile(string filePath, string searchWord, List<FileResult> results)
        {
            int lineNumber = 0;
            foreach (string line in File.ReadLines(filePath))
            {
                lineNumber++;
                int position = line.IndexOf(searchWord, StringComparison.OrdinalIgnoreCase);
                if (position >= 0)
                {
                    results.Add(new FileResult
                    {
                        FileName = Path.GetFileName(filePath),
                        LineNumber = lineNumber,
                        Position = position
                    });
                }
            }
        }
        class FileResult
        {
            public string? FileName { get; set; }
            public int LineNumber { get; set; }
            public int Position { get; set; }
        }
        public static string GetSMTName(byte b1, byte b2)
        {
            string[] Names;
            if (!Path.Exists("Names.txt")) Names = new string[] { "\t", "\t", "\t" };
            else Names = File.ReadAllLines("Names.txt");
            int size = b1 | (b2 << 8);
            if (size < 0 || size >= Names.Length)
            {
                // Проверяем, что индекс не выходит за границы
                return b1.ToString("X2") + " " + b2.ToString("X2");
            }

            return Names[size];
        }
        public static (bool, int) IsRLEEncoded(string filePath)
        {
            // Прочитаем первый байт файла
            int len = 0;
            byte[] firstByte = new byte[1];
            using (FileStream fs = new FileStream(filePath, FileMode.Open, FileAccess.Read))
            {
                fs.Read(firstByte, 0, 1);
                len = (int)fs.Length;
            }

            // Проверим, является ли первый байт магическим числом RLE
            return (firstByte[0] == 0x30, len);
        }
        public static void AlignPosition(BinaryReader reader)
        {
            long pos = reader.BaseStream.Position;
            if (pos % 0x10 != 0)
                reader.BaseStream.Position = (0x10 - pos % 0x10) + pos;
        }

        public static void AlignPosition(BinaryReader reader, int align)
        {
            long pos = reader.BaseStream.Position;
            if (pos % align != 0)
                reader.BaseStream.Position = (align - pos % align) + pos;
        }

        public static long GetAlignLength(BinaryWriter writer, long align)
        {
            long length = 0;
            long pos = writer.BaseStream.Position;
            if (pos % align != 0)
                length = ((align - pos % align) + pos) - pos;
            return length;
        }

        public static long GetAlignLength(BinaryWriter writer)
        {
            long length = 0;
            long pos = writer.BaseStream.Position;
            if (pos % 0x10 != 0)
                length = ((0x10 - pos % 0x10) + pos) - pos;
            return length;
        }

        public static void AlignPosition(BinaryWriter writer, int align)
        {
            long pos = writer.BaseStream.Position;
            if (pos % align != 0)
                writer.BaseStream.Position = (align - pos % align) + pos;
        }

        public static long GetAlignLength(BinaryReader reader)
        {
            long length = 0;
            long pos = reader.BaseStream.Position;
            if (pos % 0x10 != 0)
                length = ((0x10 - pos % 0x10) + pos) - pos;
            return length;
        }

        public static string ReadString(byte[] namebuf, Encoding encoding)
        {
            BinaryReader binaryReader = new BinaryReader(new MemoryStream(namebuf));
            if (encoding == null) throw new ArgumentNullException("encoding");

            List<byte> data = new List<byte>();

            while (binaryReader.BaseStream.Position < binaryReader.BaseStream.Length)
            {
                data.Add(binaryReader.ReadByte());

                string partialString = encoding.GetString(data.ToArray(), 0, data.Count);

                if (partialString.Length > 0 && partialString.Last() == '\0')
                    return encoding.GetString(data.SkipLast(encoding.GetByteCount("\0")).ToArray()).TrimEnd('\0');
            }
            throw new InvalidDataException("Hit end of stream while reading null-terminated string.");
        }
        public static string ConvertToFull(string input)
        {
            StringBuilder sb = new StringBuilder();

            foreach (char c in input)
            {
                switch (c)
                {
                    // Полноширинные буквы латинского алфавита
                    case '~': sb.Append('～'); break;
                    case '}': sb.Append('｝'); break;
                    case '|': sb.Append('｜'); break;
                    case '{': sb.Append('｛'); break;
                    case '`': sb.Append('｀'); break;
                    case '_': sb.Append('＿'); break;
                    case '^': sb.Append('＾'); break;
                    case ']': sb.Append('］'); break;
                    case '\\': sb.Append('＼'); break;
                    case '[': sb.Append('［'); break;
                    case '@': sb.Append('＠'); break;
                    case '?': sb.Append('？'); break;
                    case '>': sb.Append('＞'); break;
                    case '=': sb.Append('＝'); break;
                    case '<': sb.Append('＜'); break;
                    case ';': sb.Append('；'); break;
                    case ':': sb.Append('：'); break;
                    case '/': sb.Append('／'); break;
                    case '.': sb.Append('．'); break;
                    case '-': sb.Append('－'); break;
                    case ',': sb.Append('，'); break;
                    case '+': sb.Append('＋'); break;
                    case '*': sb.Append('＊'); break;
                    case ')': sb.Append('）'); break;
                    case '(': sb.Append('（'); break;
                    case '\'': sb.Append('＇'); break;
                    case '&': sb.Append('＆'); break;
                    case '%': sb.Append('％'); break;
                    case '$': sb.Append('＄'); break;
                    case '#': sb.Append('＃'); break;
                    case '"': sb.Append('＂'); break;
                    case '!': sb.Append('！'); break;
                    case '9': sb.Append('９'); break;
                    case '8': sb.Append('８'); break;
                    case '7': sb.Append('７'); break;
                    case '6': sb.Append('６'); break;
                    case '5': sb.Append('５'); break;
                    case '4': sb.Append('４'); break;
                    case '3': sb.Append('３'); break;
                    case '2': sb.Append('２'); break;
                    case '1': sb.Append('１'); break;
                    case '0': sb.Append('０'); break;
                    case 'z': sb.Append('ｚ'); break;
                    case 'y': sb.Append('ｙ'); break;
                    case 'x': sb.Append('ｘ'); break;
                    case 'w': sb.Append('ｗ'); break;
                    case 'v': sb.Append('ｖ'); break;
                    case 'u': sb.Append('ｕ'); break;
                    case 't': sb.Append('ｔ'); break;
                    case 's': sb.Append('ｓ'); break;
                    case 'r': sb.Append('ｒ'); break;
                    case 'q': sb.Append('ｑ'); break;
                    case 'p': sb.Append('ｐ'); break;
                    case 'o': sb.Append('ｏ'); break;
                    case 'n': sb.Append('ｎ'); break;
                    case 'm': sb.Append('ｍ'); break;
                    case 'l': sb.Append('ｌ'); break;
                    case 'k': sb.Append('ｋ'); break;
                    case 'j': sb.Append('ｊ'); break;
                    case 'i': sb.Append('ｉ'); break;
                    case 'h': sb.Append('ｈ'); break;
                    case 'g': sb.Append('ｇ'); break;
                    case 'f': sb.Append('ｆ'); break;
                    case 'e': sb.Append('ｅ'); break;
                    case 'd': sb.Append('ｄ'); break;
                    case 'c': sb.Append('ｃ'); break;
                    case 'b': sb.Append('ｂ'); break;
                    case 'a': sb.Append('ａ'); break;
                    case 'Z': sb.Append('Ｚ'); break;
                    case 'Y': sb.Append('Ｙ'); break;
                    case 'X': sb.Append('Ｘ'); break;
                    case 'W': sb.Append('Ｗ'); break;
                    case 'V': sb.Append('Ｖ'); break;
                    case 'U': sb.Append('Ｕ'); break;
                    case 'T': sb.Append('Ｔ'); break;
                    case 'S': sb.Append('Ｓ'); break;
                    case 'R': sb.Append('Ｒ'); break;
                    case 'Q': sb.Append('Ｑ'); break;
                    case 'P': sb.Append('Ｐ'); break;
                    case 'O': sb.Append('Ｏ'); break;
                    case 'N': sb.Append('Ｎ'); break;
                    case 'M': sb.Append('Ｍ'); break;
                    case 'L': sb.Append('Ｌ'); break;
                    case 'K': sb.Append('Ｋ'); break;
                    case 'J': sb.Append('Ｊ'); break;
                    case 'I': sb.Append('Ｉ'); break;
                    case 'H': sb.Append('Ｈ'); break;
                    case 'G': sb.Append('Ｇ'); break;
                    case 'F': sb.Append('Ｆ'); break;
                    case 'E': sb.Append('Ｅ'); break;
                    case 'D': sb.Append('Ｄ'); break;
                    case 'C': sb.Append('Ｃ'); break;
                    case 'B': sb.Append('Ｂ'); break;
                    case 'A': sb.Append('Ａ'); break;
                    case ' ': sb.Append('　'); break;


                    // Если символ не найден в списке, просто добавляем его
                    default: sb.Append(c); break;
                }
            }

            return sb.ToString();
        }
        public static string ConvertToMonospace(string input)
        {
            StringBuilder sb = new StringBuilder();

            foreach (char c in input)
            {
                switch (c)
                {
                    // Полноширинные буквы латинского алфавита
                    case '　': sb.Append(' '); break;
                    case 'Ａ': sb.Append('A'); break;
                    case 'Ｂ': sb.Append('B'); break;
                    case 'Ｃ': sb.Append('C'); break;
                    case 'Ｄ': sb.Append('D'); break;
                    case 'Ｅ': sb.Append('E'); break;
                    case 'Ｆ': sb.Append('F'); break;
                    case 'Ｇ': sb.Append('G'); break;
                    case 'Ｈ': sb.Append('H'); break;
                    case 'Ｉ': sb.Append('I'); break;
                    case 'Ｊ': sb.Append('J'); break;
                    case 'Ｋ': sb.Append('K'); break;
                    case 'Ｌ': sb.Append('L'); break;
                    case 'Ｍ': sb.Append('M'); break;
                    case 'Ｎ': sb.Append('N'); break;
                    case 'Ｏ': sb.Append('O'); break;
                    case 'Ｐ': sb.Append('P'); break;
                    case 'Ｑ': sb.Append('Q'); break;
                    case 'Ｒ': sb.Append('R'); break;
                    case 'Ｓ': sb.Append('S'); break;
                    case 'Ｔ': sb.Append('T'); break;
                    case 'Ｕ': sb.Append('U'); break;
                    case 'Ｖ': sb.Append('V'); break;
                    case 'Ｗ': sb.Append('W'); break;
                    case 'Ｘ': sb.Append('X'); break;
                    case 'Ｙ': sb.Append('Y'); break;
                    case 'Ｚ': sb.Append('Z'); break;
                    case 'ａ': sb.Append('a'); break;
                    case 'ｂ': sb.Append('b'); break;
                    case 'ｃ': sb.Append('c'); break;
                    case 'ｄ': sb.Append('d'); break;
                    case 'ｅ': sb.Append('e'); break;
                    case 'ｆ': sb.Append('f'); break;
                    case 'ｇ': sb.Append('g'); break;
                    case 'ｈ': sb.Append('h'); break;
                    case 'ｉ': sb.Append('i'); break;
                    case 'ｊ': sb.Append('j'); break;
                    case 'ｋ': sb.Append('k'); break;
                    case 'ｌ': sb.Append('l'); break;
                    case 'ｍ': sb.Append('m'); break;
                    case 'ｎ': sb.Append('n'); break;
                    case 'ｏ': sb.Append('o'); break;
                    case 'ｐ': sb.Append('p'); break;
                    case 'ｑ': sb.Append('q'); break;
                    case 'ｒ': sb.Append('r'); break;
                    case 'ｓ': sb.Append('s'); break;
                    case 'ｔ': sb.Append('t'); break;
                    case 'ｕ': sb.Append('u'); break;
                    case 'ｖ': sb.Append('v'); break;
                    case 'ｗ': sb.Append('w'); break;
                    case 'ｘ': sb.Append('x'); break;
                    case 'ｙ': sb.Append('y'); break;
                    case 'ｚ': sb.Append('z'); break;

                    // Полноширинные цифры
                    case '０': sb.Append('0'); break;
                    case '１': sb.Append('1'); break;
                    case '２': sb.Append('2'); break;
                    case '３': sb.Append('3'); break;
                    case '４': sb.Append('4'); break;
                    case '５': sb.Append('5'); break;
                    case '６': sb.Append('6'); break;
                    case '７': sb.Append('7'); break;
                    case '８': sb.Append('8'); break;
                    case '９': sb.Append('9'); break;

                    // Другие полноширинные символы
                    case '！': sb.Append('!'); break;
                    case '＂': sb.Append('"'); break;
                    case '＃': sb.Append('#'); break;
                    case '＄': sb.Append('$'); break;
                    case '％': sb.Append('%'); break;
                    case '＆': sb.Append('&'); break;
                    case '＇': sb.Append('\''); break;
                    case '（': sb.Append('('); break;
                    case '）': sb.Append(')'); break;
                    case '＊': sb.Append('*'); break;
                    case '＋': sb.Append('+'); break;
                    case '，': sb.Append(','); break;
                    case '－': sb.Append('-'); break;
                    case '．': sb.Append('.'); break;
                    case '／': sb.Append('/'); break;
                    case '：': sb.Append(':'); break;
                    case '；': sb.Append(';'); break;
                    case '＜': sb.Append('<'); break;
                    case '＝': sb.Append('='); break;
                    case '＞': sb.Append('>'); break;
                    case '？': sb.Append('?'); break;
                    case '＠': sb.Append('@'); break;
                    case '［': sb.Append('['); break;
                    case '＼': sb.Append('\\'); break;
                    case '］': sb.Append(']'); break;
                    case '＾': sb.Append('^'); break;
                    case '＿': sb.Append('_'); break;
                    case '｀': sb.Append('`'); break;
                    case '｛': sb.Append('{'); break;
                    case '｜': sb.Append('|'); break;
                    case '｝': sb.Append('}'); break;
                    case '～': sb.Append('~'); break;
                    case '’': sb.Append('\''); break;

                    // Если символ не найден в списке, просто добавляем его
                    default: sb.Append(c); break;
                }
            }

            return sb.ToString();
        }
        public static string ReadString(this BinaryReader binaryReader, Encoding encoding)
        {
            if (binaryReader == null) throw new ArgumentNullException("binaryReader");
            if (encoding == null) throw new ArgumentNullException("encoding");

            List<byte> data = new List<byte>();

            while (binaryReader.BaseStream.Position < binaryReader.BaseStream.Length)
            {
                data.Add(binaryReader.ReadByte());

                string partialString = encoding.GetString(data.ToArray(), 0, data.Count);

                if (partialString.Length > 0 && partialString.Last() == '\0')
                    return encoding.GetString(data.SkipLast(encoding.GetByteCount("\0")).ToArray()).TrimEnd('\0');
            }
            throw new InvalidDataException("Hit end of stream while reading null-terminated string.");
        }
        public static Dictionary<char, char> FullWidthCharMap = new Dictionary<char, char>
        {
            {'A', 'Ａ'}, {'B', 'Ｂ'}, {'C', 'Ｃ'}, {'D', 'Ｄ'}, {'E', 'Ｅ'},
            {'F', 'Ｆ'}, {'G', 'Ｇ'}, {'H', 'Ｈ'}, {'I', 'Ｉ'}, {'J', 'Ｊ'},
            {'K', 'Ｋ'}, {'L', 'Ｌ'}, {'M', 'Ｍ'}, {'N', 'Ｎ'}, {'O', 'Ｏ'},
            {'P', 'Ｐ'}, {'Q', 'Ｑ'}, {'R', 'Ｒ'}, {'S', 'Ｓ'}, {'T', 'Ｔ'},
            {'U', 'Ｕ'}, {'V', 'Ｖ'}, {'W', 'Ｗ'}, {'X', 'Ｘ'}, {'Y', 'Ｙ'},
            {'Z', 'Ｚ'},
            {'a', 'ａ'}, {'b', 'ｂ'}, {'c', 'ｃ'}, {'d', 'ｄ'}, {'e', 'ｅ'},
            {'f', 'ｆ'}, {'g', 'ｇ'}, {'h', 'ｈ'}, {'i', 'ｉ'}, {'j', 'ｊ'},
            {'k', 'ｋ'}, {'l', 'ｌ'}, {'m', 'ｍ'}, {'n', 'ｎ'}, {'o', 'ｏ'},
            {'p', 'ｐ'}, {'q', 'ｑ'}, {'r', 'ｒ'}, {'s', 'ｓ'}, {'t', 'ｔ'},
            {'u', 'ｕ'}, {'v', 'ｖ'}, {'w', 'ｗ'}, {'x', 'ｘ'}, {'y', 'ｙ'},
            {'z', 'ｚ'},
            {'0', '０'}, {'1', '１'}, {'2', '２'}, {'3', '３'}, {'4', '４'},
            {'5', '５'}, {'6', '６'}, {'7', '７'}, {'8', '８'}, {'9', '９'},
            {' ', '　'}, {'\'', '’'}
        };

        public static string ConvertToFullWidth(string input)
        {
            StringBuilder result = new StringBuilder();
            foreach (char c in input)
            {
                if (FullWidthCharMap.TryGetValue(c, out char fullWidthChar))
                {
                    result.Append(fullWidthChar);
                }
                else
                {
                    result.Append(c);
                }
            }
            return result.ToString();
        }
        private static IEnumerable<TSource> SkipLast<TSource>(this IEnumerable<TSource> source, int count)
        {
            if (source == null) throw new ArgumentNullException("source");

            Queue<TSource> queue = new Queue<TSource>();

            foreach (TSource item in source)
            {
                queue.Enqueue(item);

                if (queue.Count > count) yield return queue.Dequeue();
            }
        }
    }
}
