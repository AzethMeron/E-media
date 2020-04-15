using System;
using System.Collections.Generic;
using System.IO;
using System.Text;
using System.IO.Compression;
using ICSharpCode.SharpZipLib.Zip.Compression.Streams;

namespace PNG_WindowAPP
{
    struct PNG
    {
        public byte[] headline;
        public List<Chunk> zawartosc;
    }
    public class Color
    {
        public byte r { get; set; }
        public byte g { get; set; }
        public byte b { get; set; }
        public byte alpha { get; set; }
        public Color() { }
        
    }
    struct Chunk
    {
        public byte[] length;
        public byte[] type;
        public byte[] data;
        public byte[] crc;
        public int Przesunięcie() // Zwraca liczbe bajtów zajmowanych przez Typ, Dane oraz CRC
        {
            return type.Length + data.Length + crc.Length;
        }
        public void Wczytaj(int index, byte[] plik)
        {
            length = new byte[4];
            type = new byte[4];

            // Wczytywanie Długości
            Array.Copy(plik, index - 4, length, 0, 4);

            // Wczytywanie Typu
            Array.Copy(plik, index, type, 0, 4);

            // Konwersja długośći
            if (BitConverter.IsLittleEndian) Array.Reverse(length);
            int dist = BitConverter.ToInt32(length, 0);
            if (BitConverter.IsLittleEndian) Array.Reverse(length);
            data = new byte[dist];

            // Wczytywanie Danych
            Array.Copy(plik, index + 4, data, 0, dist);

            // Wczytywanie CRC
            crc = new byte[4];
            Array.Copy(plik, index + 4 + dist, crc, 0, 4);
        }
        public void Wyświetl()
        {
            Console.WriteLine(BitConverter.ToString(length));
            Console.WriteLine(BitConverter.ToString(type));
            Console.WriteLine(BitConverter.ToString(data));
            Console.WriteLine(BitConverter.ToString(crc));
        }
    }

    public class Ihdr
    {
        public byte[] Width { get; set; }
        public byte[] Height { get; set; }
        public byte Bit_depth { get; set; }
        public byte Color_type { get; set; }
        public byte Compression_method { get; set; }
        public byte Filter_method { get; set; }
        public byte Interlace_method { get; set; }
        public Ihdr() { }
    }
    public class Phys
    {
        public byte[] Xaxis { get; set; }
        public byte[] Yaxis { get; set; }
        public string unitSpecifier { get; set; }
        public Phys() { }
    }

    public class Sbit
    {
        public byte sRed { get; set; }
        public byte sGreen { get; set; }
        public byte sBlue { get; set; }
        public byte sAlpha { get; set; }
        public byte sGreyscale { get; set; }
        public Sbit() { }
    }

    public class Chrm
    {
        // punkt biały (środkowy punkt odniesienia - neutralny)
        public byte[] WhitePointX { get; set; } 
        public byte[] WhitePointY { get; set; }

        // informacja o odcieniu i nasyceniu barwy czerwonej
        public byte[] RedX { get; set; } // maksymalny punkt koloru czerwonego na osi X
        public byte[] RedY { get; set; } // maksymalny punkt koloru czerwonego na osi Y 

        // informacja o odcieniu i nasyceniu barwy zielonej
        public byte[] GreenX { get; set; } // maksymalny punkt koloru zielonego na osi X
        public byte[] GreenY { get; set; } // maksymalny punkt koloru zielonego na osi X

        // informacja o odcieniu i nasyceniu barwy niebieskiej
        public byte[] BlueX { get; set; } // maksymalny punkt koloru niebieskiego na osi X
        public byte[] BlueY { get; set; } // maksymalny punkt koloru niebieskiego na osi X
        public Chrm() { }
    }

    public class Bkgd
    {
        public byte PaletteIndex { get; set; }
        public byte[] Gray { get; set; }
        public byte[] Red { get; set; }
        public byte[] Green { get; set; }
        public byte[] Blue { get; set; }
        public Bkgd() { }
    }

    public class Iccp
    {
        public string ProfileName { get; set; }
        public byte CompressionMethod { get; set; }
        public byte[] CompressedProfile { get; set; }
        public Iccp() { }
    }

    public class Text
    {
        public string Keyword { get; set; }
        public byte CompressionFlag { get; set; }
        public byte CompressionMethod { get; set;}
        public string LanguageTag { get; set; }
        public string TranslatedKeyword { get; set; }
        public string _Text { get; set; }
        public Text() { }
    }

    public class Trns
    {
        public byte[] Alpha { get; set; }
        public byte[] Grey { get; set; }
        public byte[] Red { get; set; }
        public byte[] Green { get; set; }
        public byte[] Blue { get; set; }
        public Trns() { }
    }

    public class Time
    {
        public byte[] Year { get; set; }
        public byte Month { get; set; }
        public byte Day { get; set; }
        public byte Hour { get; set; }
        public byte Minute { get; set; }
        public byte Second { get; set; }
        public Time() { }
    }

    public class Metadane
    {
        public Ihdr IHDR { get; set; }
        public byte[] IDAT { get; set; }
        public List<Color> PLTE { get; set; }
        public Phys pHYs { get; set; }
        public Sbit sBIT { get; set; }
        public Chrm cHRM { get; set; }
        public Bkgd bKGD { get; set; }
        public double gAMA { get; set; } // jezeli bedziemy wyswietlac to "1/gAMA"
        public Iccp iCCP { get; set; }
        public Text iTXt { get; set; }
        public string sRGB { get; set; }
        public Trns tRNS { get; set; }
        public Text tEXt { get; set; }
        public List<Text> zTXt { get; set; }
        public Time tIME { get; set; }
        public Metadane() { }
    }

    struct Constans
    {
        public const string Nagłówek = "89-50-4E-47-0D-0A-1A-0A";
        public const string IHDR = "49-48-44-52";
        public const string PLTE = "50-4C-54-45";
        public const string IDAT = "49-44-41-54";
        public const string IEND = "49-45-4E-44";
        public const string eXIf = "65-58-49-66";
        public const string pHYs = "70-48-59-73";
        public const string sBIT = "73-42-49-54";
        public const string tEXt = "74-45-58-74";
        public const string cHRM = "63-48-52-4D";
        public const string bKGD = "62-4B-47-44";
        public const string gAMA = "67-41-4D-41";
        public const string iCCP = "69-43-43-50";
        public const string iTXt = "69-54-58-74";
        public const string sRGB = "73-52-47-42";
        public const string tRNS = "74-52-4E-53";
        public const string zTXt = "7A-54-58-74";
        public const string tIME = "74-49-4D-45";
    }
   

    class Program
    {
        static void Main(string[] args)
        {
            Menu();
        }
        /*public static byte[] Decompress(byte[] data)
        {
            MemoryStream input = new MemoryStream(data);
            MemoryStream output = new MemoryStream();
            using (DeflateStream dstream = new DeflateStream(input, CompressionMode.Decompress))
            {
                dstream.CopyTo(output);
            }
            return output.ToArray();
        }*/
        
    static Metadane Parser(PNG png)
        {
            Metadane info = new Metadane();
            for (int i = 0; i < png.zawartosc.Count; i++)
            {
                string analizowany_typ = (BitConverter.ToString(png.zawartosc[i].type));
                byte[] dane_z_obrazu = png.zawartosc[i].data;

                if (analizowany_typ == Constans.IHDR)
                {

                    // Zczytujemy dane do zmiennych lokalnych, celem obracania przy odczycie
                    byte[] width = new byte[4];
                    Array.Copy(dane_z_obrazu, 0, width, 0, 4);
                    byte[] height = new byte[4];
                    Array.Copy(dane_z_obrazu, 4, height, 0, 4);

                    // Jednobajtowych nie trzeba obracać
                    byte bit_depth = dane_z_obrazu[8];
                    byte color_type = dane_z_obrazu[9];
                    byte compression_method = dane_z_obrazu[10];
                    byte filter_method = dane_z_obrazu[11];
                    byte interlace_method = dane_z_obrazu[12];

                    // Obracanie, jeśli little endian
                    if (BitConverter.IsLittleEndian) Array.Reverse(width);
                    if (BitConverter.IsLittleEndian) Array.Reverse(height);


                    Ihdr ihdr = new Ihdr { Width = width, Height = height, Bit_depth = bit_depth, Color_type = color_type, Compression_method = compression_method, Filter_method = filter_method, Interlace_method = interlace_method };
                    info.IHDR = ihdr;

                    //Console.WriteLine((Convert.ToInt16(głębia_kolorów)).ToString());
                }
                else if (analizowany_typ == Constans.PLTE)
                {
                    if ((info.IHDR.Color_type == 3) || (info.IHDR.Color_type == 2))
                    {
                        List<Color> Palette = new List<Color>();
                        for (int j = 0; j < dane_z_obrazu.Length; j = j + 3)
                        {
                            byte red = dane_z_obrazu[j];
                            byte green = dane_z_obrazu[j + 1];
                            byte blue = dane_z_obrazu[j + 2];
                            Color color = new Color { r = red, g = green, b = blue, alpha = 255 };
                            Palette.Add(color);
                        }
                        info.PLTE = Palette;
                    }
                    if (info.IHDR.Color_type == 6)
                    {
                        List<Color> Palette = new List<Color>();
                        for (int j = 0; j < dane_z_obrazu.Length; j = j + 4)
                        {
                            byte red = dane_z_obrazu[j];
                            byte green = dane_z_obrazu[j + 1];
                            byte blue = dane_z_obrazu[j + 2];
                            byte alfa = dane_z_obrazu[j + 3];
                            Color color = new Color { r = red, g = green, b = blue, alpha = alfa };
                            Palette.Add(color);
                        }
                        info.PLTE = Palette;
                    }
                }
                else if (analizowany_typ == Constans.IDAT)
                {
                    // * * * C B D * * *
                    // * * * A X * * * *
                    
                    //Filter method: 0 "none"
                    if (info.IHDR.Filter_method == 0)
                    {
                        info.IDAT = dane_z_obrazu;
                    }

                    //Filter method: 1 "sub" (Byte A "to the left")
                    if (info.IHDR.Filter_method == 1)
                    {
                        byte[] tmp = new byte[dane_z_obrazu.Length];
                        for (int x = 0; x < dane_z_obrazu.Length; x++)
                        {
                            if (x % 16 != 0) tmp[x] = (byte)(dane_z_obrazu[x] - dane_z_obrazu[x - 1]);
                            else tmp[x] = dane_z_obrazu[x];
                        }

                        info.IDAT = tmp;
                    }

                    //Filter method: 2 "up" (Byte B "above")
                    if (info.IHDR.Filter_method == 2)
                    {
                        byte[] tmp = new byte[dane_z_obrazu.Length];
                        for (int x = 0; x < 16; x++)
                        {
                            tmp[x] = dane_z_obrazu[x];
                        }
                        for (int y = 16; y < dane_z_obrazu.Length; y++)
                        {
                            tmp[y] = (byte)(dane_z_obrazu[y] - dane_z_obrazu[y - 16]);
                        }
                        info.IDAT = tmp;
                    }

                    //Filter method: 3 "average" (Mean of bytes A and B, rounded down)
                    if (info.IHDR.Filter_method == 3)
                    {
                        byte[] tmp = new byte[dane_z_obrazu.Length];
                        for (int x = 0; x < 16; x++)
                        {
                            tmp[x] = dane_z_obrazu[x];
                        }
                        for (int y = 16; y < dane_z_obrazu.Length; y++)
                        {
                            if (y % 16 != 0) tmp[y] = (byte)(dane_z_obrazu[y] - ((dane_z_obrazu[y - 1] + dane_z_obrazu[y - 16]) / 2));
                            else tmp[y] = dane_z_obrazu[y];
                        }
                        info.IDAT = tmp;
                    }

                    //Filter method: 4 "Paeth" (A, B, or C, whichever is closest to p = A + B − C)
                    if (info.IHDR.Filter_method == 4)
                    {
                        byte[] tmp = new byte[dane_z_obrazu.Length];
                        for (int x = 0; x < 16; x++)
                        {
                            tmp[x] = dane_z_obrazu[x];
                        }
                        for (int y = 16; y < dane_z_obrazu.Length; y++)
                        {
                            if (y % 16 != 0)
                            {
                                byte paeth = Paeth(dane_z_obrazu[y - 1], dane_z_obrazu[y - 16], dane_z_obrazu[y - 17]);
                                tmp[y] = (byte)(dane_z_obrazu[y] - paeth);
                            }
                            else tmp[y] = dane_z_obrazu[y];
                        }
                        info.IDAT = tmp;

                    }
                }
                else if (analizowany_typ == Constans.IEND)
                {
                    return info;
                }
                else if (analizowany_typ == Constans.eXIf)
                {

                }
                else if (analizowany_typ == Constans.pHYs)
                {
                    byte[] xaxis = new byte[4]; // pixele na jednostkę, oś X 
                    Array.Copy(dane_z_obrazu, 0, xaxis, 0, 4);
                    byte[] yaxis = new byte[4]; // pixele na jednostkę, oś Y
                    Array.Copy(dane_z_obrazu, 4, yaxis, 0, 4);
                    byte unitSpec = dane_z_obrazu[8]; // jeżeli jest 0 to jednostka jest nieznana, jeżeli jest 1 to jednostką jest metr
                    string unitSpec_string = "null";
                    if (BitConverter.IsLittleEndian) Array.Reverse(xaxis);
                    if (BitConverter.IsLittleEndian) Array.Reverse(yaxis);
                    if(unitSpec == 0)
                    {
                        unitSpec_string = "0: unit is unknown";
                    }
                    else if(unitSpec == 1)
                    {
                        unitSpec_string = "1: unit is the meter";
                    }
                    Phys phys = new Phys { Xaxis = xaxis, Yaxis = yaxis, unitSpecifier = unitSpec_string };
                    info.pHYs = phys;
                }
                else if (analizowany_typ == Constans.sBIT)
                {
                    // indicating the number of bits that were significant in the source data
                    if (info.IHDR.Color_type == 0)
                    {
                        byte sgrey = dane_z_obrazu[0];
                        Sbit sbit = new Sbit { sGreyscale = sgrey };
                        info.sBIT = sbit;
                    }

                    // indicating the number of bits that were significant in the source data for the red, green, and blue channels
                    if ((info.IHDR.Color_type == 2) || (info.IHDR.Color_type == 3))
                    {
                        byte sr = dane_z_obrazu[0];
                        byte sg = dane_z_obrazu[1];
                        byte sb = dane_z_obrazu[2];
                        Sbit sbit = new Sbit { sRed = sr, sGreen = sg, sBlue = sb };
                        info.sBIT = sbit;
                    }

                    // indicating the number of bits that were significant in the source grayscale data and the source alpha data
                    if (info.IHDR.Color_type == 4)
                    {
                        byte sgrey = dane_z_obrazu[0];
                        byte sa = dane_z_obrazu[1];
                        Sbit sbit = new Sbit { sGreyscale = sgrey, sAlpha = sa };
                        info.sBIT = sbit;
                    }

                    // indicating the number of bits that were significant in the source data for the red, green, blue and alpha channels
                    if (info.IHDR.Color_type == 6)
                    {
                        byte sr = dane_z_obrazu[0];
                        byte sg = dane_z_obrazu[1];
                        byte sb = dane_z_obrazu[2];
                        byte sa = dane_z_obrazu[3];
                        Sbit sbit = new Sbit { sRed = sr, sGreen = sg, sBlue = sb, sAlpha = sa };
                        info.sBIT = sbit;
                    }
                }

                else if (analizowany_typ == Constans.tEXt)
                {
                    byte[] key_tab = new byte[78];
                    string keyword;
                    int pos = 0;
                    while (dane_z_obrazu[pos] != 0)
                    {
                        key_tab[pos] = dane_z_obrazu[pos];
                        pos++;
                    }
                    if (pos  < 78) Array.Resize(ref key_tab, pos );
                    keyword = Encoding.Default.GetString(key_tab);
                    pos++;
                    byte[] text_tab = new byte[dane_z_obrazu.Length];
                    int pos_text = 0;
                    while (pos != dane_z_obrazu.Length)
                    {
                        text_tab[pos_text] = dane_z_obrazu[pos];
                        pos++; pos_text++;
                    }
                    if (pos_text  < dane_z_obrazu.Length) Array.Resize(ref text_tab, pos_text);
                    string _text = Encoding.Default.GetString(text_tab);
                    Text text = new Text { Keyword = keyword, _Text = _text };
                    info.tEXt = text;
                }

                else if (analizowany_typ == Constans.cHRM)
                {
                    /***********************************************************************/
                    /* PAMIĘTAĆ, ŻE KAŻDA Z TYCH WARTOŚCI MUSI BYĆ PODZIELONA PRZEZ 100000 */
                    /***********************************************************************/
                    byte[] whiteX = new byte[4];
                    byte[] whiteY = new byte[4];
                    byte[] redX = new byte[4];
                    byte[] redY = new byte[4];
                    byte[] greenX = new byte[4];
                    byte[] greenY = new byte[4];
                    byte[] blueX = new byte[4];
                    byte[] blueY = new byte[4];
                    Array.Copy(dane_z_obrazu, 0, whiteX, 0, 4);
                    Array.Copy(dane_z_obrazu, 4, whiteY, 0, 4);
                    Array.Copy(dane_z_obrazu, 8, redX, 0, 4);
                    Array.Copy(dane_z_obrazu, 12, redY, 0, 4);
                    Array.Copy(dane_z_obrazu, 16, greenX, 0, 4);
                    Array.Copy(dane_z_obrazu, 20, greenY, 0, 4);
                    Array.Copy(dane_z_obrazu, 24, blueX, 0, 4);
                    Array.Copy(dane_z_obrazu, 28, blueY, 0, 4);
                    if (BitConverter.IsLittleEndian) Array.Reverse(whiteX);
                    if (BitConverter.IsLittleEndian) Array.Reverse(whiteY);
                    if (BitConverter.IsLittleEndian) Array.Reverse(redX);
                    if (BitConverter.IsLittleEndian) Array.Reverse(redY);
                    if (BitConverter.IsLittleEndian) Array.Reverse(greenX);
                    if (BitConverter.IsLittleEndian) Array.Reverse(greenY);
                    if (BitConverter.IsLittleEndian) Array.Reverse(blueX);
                    if (BitConverter.IsLittleEndian) Array.Reverse(blueY);
                    Chrm chrm = new Chrm { WhitePointX = whiteX, WhitePointY = whiteY, RedX = redX, RedY = redY, GreenX = greenX, GreenY = greenY, BlueX = blueX, BlueY = blueY };
                    info.cHRM = chrm;
                }

                else if (analizowany_typ == Constans.bKGD)
                {
                    if (info.IHDR.Color_type == 2 || info.IHDR.Color_type == 6)
                    {
                        byte[] r = new byte[2]; // range 0 to (2^bit_depth)-1
                        byte[] g = new byte[2]; // range 0 to (2^bit_depth)-1
                        byte[] b = new byte[2]; // range 0 to (2^bit_depth)-1
                        Array.Copy(dane_z_obrazu, 0, r, 0, 2);
                        Array.Copy(dane_z_obrazu, 2, g, 0, 2);
                        Array.Copy(dane_z_obrazu, 4, b, 0, 2);
                        if (BitConverter.IsLittleEndian) Array.Reverse(r);
                        if (BitConverter.IsLittleEndian) Array.Reverse(g);
                        if (BitConverter.IsLittleEndian) Array.Reverse(b);
                        Bkgd bkgd = new Bkgd { Red = r, Green = g, Blue = b };
                        info.bKGD = bkgd;
                    }
                    if (info.IHDR.Color_type == 3)
                    {
                        byte paletteIndex = dane_z_obrazu[0];
                        Bkgd bkgd = new Bkgd { PaletteIndex = paletteIndex };
                        info.bKGD = bkgd;
                    }
                    if(info.IHDR.Color_type==0 || info.IHDR.Color_type == 4)
                    {
                        byte[] gray = new byte[2];
                        Array.Copy(dane_z_obrazu, 0, gray, 0, 2);
                        if (BitConverter.IsLittleEndian) Array.Reverse(gray);
                        Bkgd bkgd = new Bkgd { Gray = gray };
                        info.bKGD = bkgd;
                    }
                }

                else if (analizowany_typ == Constans.gAMA)
                {
                    byte[] gamma_stored = new byte[4]; // gamma stored in 4 bytes
                    Array.Copy(dane_z_obrazu, 0, gamma_stored, 0, 4);
                    if (BitConverter.IsLittleEndian) Array.Reverse(gamma_stored);
                    double gamma_real = Math.Round((double)1 / BitConverter.ToInt32(gamma_stored, 0) * 100000, 1); // gamma real: (1/gamma_real)
                    info.gAMA = gamma_real;
                }

                else if (analizowany_typ == Constans.iCCP)
                {
                    byte[] profileName = new byte[78];
                    int pos = 0;
                    while (dane_z_obrazu[pos] != 0)
                    {
                        profileName[pos] = dane_z_obrazu[pos];
                        pos++;
                    }
                    if (pos < 78) Array.Resize(ref profileName, pos); // zmiana rozmiaru tablicy, jeżeli nie są zapisane wszystkie jej indeksy
                    pos++;
                    byte compressionMethod = dane_z_obrazu[pos];
                    byte[] compressedProfile = new byte[dane_z_obrazu.Length - profileName.Length - 2];
                    Array.Copy(dane_z_obrazu, profileName.Length + 2, compressedProfile, 0, compressedProfile.Length);
                    if (BitConverter.IsLittleEndian) Array.Reverse(compressedProfile);
                    string profileNameString = System.Text.Encoding.Default.GetString(profileName);
                    Iccp iccp = new Iccp { ProfileName = profileNameString, CompressionMethod = compressionMethod, CompressedProfile = compressedProfile };
                    info.iCCP = iccp;
                }

                else if (analizowany_typ == Constans.iTXt)
                {
                    byte[] keyword = new byte[78];
                    string key;
                    int pos = 0;
                    while (dane_z_obrazu[pos] != 0)
                    {
                        keyword[pos] = dane_z_obrazu[pos];
                        pos++;
                    }
                    if (pos < 78) Array.Resize(ref keyword, pos); // zmiana rozmiaru tablicy, jeżeli nie są zapisane wszystkie jej indeksy
                    key = System.Text.Encoding.Default.GetString(keyword);
                    pos++;
                    byte compressionFlag = dane_z_obrazu[pos]; pos++;
                    byte compressionMethod = dane_z_obrazu[pos]; pos++;
                    string lang;
                    string transKeyword;
                    string text;
                    if (dane_z_obrazu[pos] != 0)
                    {
                        byte[] language_tab = new byte[10];
                        int pos_lang = 0;
                        while (dane_z_obrazu[pos] != 0)
                        {
                            language_tab[pos_lang] = dane_z_obrazu[pos];
                            pos++; pos_lang++;
                        }
                        if (pos_lang < 10) Array.Resize(ref language_tab, pos_lang);
                        lang = System.Text.Encoding.Default.GetString(language_tab);
                        pos++;
                        if (dane_z_obrazu[pos] != 0)
                        {
                            byte[] transKey_tab = new byte[dane_z_obrazu.Length];
                            int pos_trans = 0;
                            while (dane_z_obrazu[pos] != 0)
                            {
                                transKey_tab[pos_trans] = dane_z_obrazu[pos];
                                pos++; pos_trans++;
                            }
                            if (pos_trans < dane_z_obrazu.Length) Array.Resize(ref transKey_tab, pos_trans);
                            transKeyword = System.Text.Encoding.Default.GetString(transKey_tab);
                            pos++;
                            if (dane_z_obrazu[pos] != 0)
                            {
                                byte[] text_tab = new byte[dane_z_obrazu.Length];
                                int pos_text = 0;
                                while (pos != dane_z_obrazu.Length)
                                {
                                    text_tab[pos_text] = dane_z_obrazu[pos];
                                    pos++; pos_text++;
                                }
                                if (pos_text < dane_z_obrazu.Length) Array.Resize(ref text_tab, pos_text);
                                text = System.Text.Encoding.Default.GetString(text_tab);
                            }
                            else text = "no text";
                        }
                        else
                        {
                            transKeyword = "no Translated Keyword";
                            pos++;
                            if (dane_z_obrazu[pos] != 0)
                            {
                                byte[] text_tab = new byte[dane_z_obrazu.Length];
                                int pos_text = 0;
                                while (pos != dane_z_obrazu.Length)
                                {
                                    text_tab[pos_text] = dane_z_obrazu[pos];
                                    pos++; pos_text++;
                                }
                                if (pos_text < dane_z_obrazu.Length) Array.Resize(ref text_tab, pos_text);
                                text = System.Text.Encoding.Default.GetString(text_tab);
                            }
                            else text = "no text";
                        }
                    }
                    else
                    {
                        lang = "unspecified";
                        pos++;
                        if (dane_z_obrazu[pos] != 0)
                        {
                            byte[] transKey_tab = new byte[dane_z_obrazu.Length];
                            int pos_trans = 0;
                            while (dane_z_obrazu[pos] != 0)
                            {
                                transKey_tab[pos_trans] = dane_z_obrazu[pos];
                                pos++; pos_trans++;
                            }
                            if (pos_trans < dane_z_obrazu.Length) Array.Resize(ref transKey_tab, pos_trans);
                            transKeyword = System.Text.Encoding.Default.GetString(transKey_tab);
                            pos++;
                            if (dane_z_obrazu[pos] != 0)
                            {
                                byte[] text_tab = new byte[dane_z_obrazu.Length];
                                int pos_text = 0;
                                while (pos != dane_z_obrazu.Length)
                                {
                                    text_tab[pos_text] = dane_z_obrazu[pos];
                                    pos++; pos_text++;
                                }
                                if (pos_text < dane_z_obrazu.Length) Array.Resize(ref text_tab, pos_text);
                                text = System.Text.Encoding.Default.GetString(text_tab);
                            }
                            else text = "no text";
                        }
                        else
                        {
                            transKeyword = "no Translated Keyword";
                            pos++;
                            if (dane_z_obrazu[pos] != 0)
                            {
                                byte[] text_tab = new byte[dane_z_obrazu.Length];
                                int pos_text = 0;
                                while (pos != dane_z_obrazu.Length)
                                {
                                    text_tab[pos_text] = dane_z_obrazu[pos];
                                    pos++; pos_text++;
                                }
                                if (pos_text < dane_z_obrazu.Length) Array.Resize(ref text_tab, pos_text);
                                text = System.Text.Encoding.Default.GetString(text_tab);
                            }
                            else text = "no text";
                        }
                    }

                    Text itxt = new Text { Keyword = key, CompressionFlag = compressionFlag, CompressionMethod = compressionMethod, LanguageTag = lang, TranslatedKeyword = transKeyword, _Text = text };
                    info.iTXt = itxt;
                }

                else if (analizowany_typ == Constans.sRGB)
                {
                    byte renderingIntent = dane_z_obrazu[0];
                    string render;
                    if (renderingIntent == 0)
                    {
                        render = "0: Perceptual (like photographs)";
                        info.sRGB = render;
                    }
                    if (renderingIntent == 1)
                    {
                        render = "1: Relative colorimetric (like logos)";
                        info.sRGB = render;
                    }
                    if (renderingIntent == 2)
                    {
                        render = "2: Saturation (like charts and graphs)";
                        info.sRGB = render;
                    }
                    if (renderingIntent == 3)
                    {
                        render = "3: Absolute colometric (like proofs - previews of images destined for a different output device) ";
                        info.sRGB = render;
                    }
                }

                else if (analizowany_typ == Constans.tRNS)
                {
                    if(info.IHDR.Color_type == 3)
                    {
                        byte[] alpha = new byte[dane_z_obrazu.Length];
                        Array.Copy(dane_z_obrazu, 0, alpha, 0, dane_z_obrazu.Length);
                        if (BitConverter.IsLittleEndian) Array.Reverse(alpha);
                        Trns trns = new Trns { Alpha = alpha };
                        info.tRNS = trns;
                    }
                    if(info.IHDR.Color_type == 0)
                    {
                        byte[] grey = new byte[2];
                        Array.Copy(dane_z_obrazu, 0, grey, 0, 2);
                        if (BitConverter.IsLittleEndian) Array.Reverse(grey);
                        Trns trns = new Trns { Grey = grey };
                        info.tRNS = trns;
                    }
                    if(info.IHDR.Color_type == 2)
                    {
                        byte[] r = new byte[2];
                        byte[] g = new byte[2];
                        byte[] b = new byte[2];
                        Array.Copy(dane_z_obrazu, 0, r, 0, 2);
                        Array.Copy(dane_z_obrazu, 2, g, 0, 2);
                        Array.Copy(dane_z_obrazu, 4, b, 0, 2);
                        if (BitConverter.IsLittleEndian) Array.Reverse(r);
                        if (BitConverter.IsLittleEndian) Array.Reverse(g);
                        if (BitConverter.IsLittleEndian) Array.Reverse(b);
                        Trns trns = new Trns { Red = r, Green = g, Blue = b };
                        info.tRNS = trns;
                    }
                }

                if (analizowany_typ == Constans.zTXt)
                {
                    List<Text> te = new List<Text>();
                    List<int> index = new List<int>();
                    string[] original = new string[] { "Title", "Author", "Description", "Copyright", "Creation Time", "Software", "Disclaimer", "Warning", "Source", "Comment" };
                    foreach (string s in original)
                    {
                        if (Encoding.Default.GetString(dane_z_obrazu).Contains(s))
                        {
                            index.Add(Encoding.Default.GetString(dane_z_obrazu).IndexOf(s));
                        }
                    }
                    index.Sort();
                    int m = 0;
                    if (index.Count == 1)
                    {
                     
                            byte[] key_tab = new byte[78];
                            int pos = 0;
                            while (dane_z_obrazu[pos] != 0)
                            {
                                key_tab[pos] = dane_z_obrazu[pos];
                                pos++;
                            }
                            if (pos < 78) Array.Resize(ref key_tab, pos);
                            string keyword = Encoding.Default.GetString(key_tab);
                            pos++;
                            byte compressionMethod = dane_z_obrazu[pos];
                            pos++;
                            byte[] text_tab = new byte[dane_z_obrazu.Length];
                            int pos_text = 0;
                            while (pos != dane_z_obrazu.Length)
                            {
                                text_tab[pos_text] = dane_z_obrazu[pos];
                                pos++; pos_text++;
                            }
                            Console.WriteLine(pos);
                            if (pos_text < dane_z_obrazu.Length) Array.Resize(ref text_tab, pos_text);
                            //if (BitConverter.IsLittleEndian) Array.Reverse(text_tab);
                            var uncompressed = Ionic.Zlib.ZlibStream.UncompressBuffer(text_tab);
                            string text = Encoding.Default.GetString(uncompressed);
                            Text ztxt = new Text { Keyword = keyword, CompressionMethod = compressionMethod, _Text = text };
                            te.Add(ztxt);
                            info.zTXt = te;
                        }
                    else if (index.Count > 1)
                    {
                        Console.WriteLine("leng: "+dane_z_obrazu.Length);
                        m = 1;int pos = 0;
                        while (pos !=dane_z_obrazu.Length)
                        {
                            byte[] key_tab = new byte[78];
                            
                            while (dane_z_obrazu[pos] != 0)
                            {
                                Console.WriteLine("pos key: " + pos);
                                //key_tab[pos] = dane_z_obrazu[pos];
                                pos++;
                            }
                            if (pos < 78) Array.Resize(ref key_tab, pos);
                            string keyword = Encoding.Default.GetString(key_tab);
                            pos++;
                            byte compressionMethod = dane_z_obrazu[pos];
                            pos++;
                            byte[] text_tab = new byte[dane_z_obrazu.Length];
                            int pos_text = 0;
                            if (m < index.Count)
                            {
                            while (pos != index[m])
                            {
                                Console.WriteLine("pos text: " + pos);
                                text_tab[pos_text] = dane_z_obrazu[pos];
                                pos++; pos_text++;
                            }
                            }else if (m == index.Count)
                            {
                                while (pos != dane_z_obrazu.Length)
                                {
                                    Console.WriteLine("pos text: " + pos);
                                    text_tab[pos_text] = dane_z_obrazu[pos];
                                    pos++; pos_text++;
                                }
                            }
                            
                           
                            //pos = index[m];
                            pos = pos + key_tab.Length+2;
                            if (pos_text < dane_z_obrazu.Length) Array.Resize(ref text_tab, pos_text);
                            //if (BitConverter.IsLittleEndian) Array.Reverse(text_tab);
                            var uncompressed = Ionic.Zlib.ZlibStream.UncompressBuffer(text_tab);
                            string text = Encoding.Default.GetString(uncompressed);
                            Text ztxt = new Text { Keyword = keyword, CompressionMethod = compressionMethod, _Text = text };
                            te.Add(ztxt);
                            m++;
                        }info.zTXt = te;
                    }
                    
                    
                }

                if(analizowany_typ == Constans.tIME)
                {
                    byte[] year = new byte[2];
                    Array.Copy(dane_z_obrazu, 0, year, 0, 2);
                    if (BitConverter.IsLittleEndian) Array.Reverse(year);
                    byte month = dane_z_obrazu[2];
                    byte day = dane_z_obrazu[3];
                    byte hour = dane_z_obrazu[4];
                    byte minute = dane_z_obrazu[5];
                    byte second = dane_z_obrazu[6];
                    Time time = new Time { Year = year, Month = month, Day = day, Hour = hour, Minute = minute, Second = second };
                    info.tIME = time;
                }
            }
            return info;
        }

        static bool TextKeyword(string keyword)
        {
            string[] original = new string[] { "Title", "Author", "Description", "Copyright", "Creation Time", "Software", "Disclaimer", "Warning", "Source", "Comment" };
            foreach(string s in original)
            {
                if (s == keyword) return true;
            }
            return false;
        }
        
        static PNG Joint(PNG wczytany)
        {
            // Zmienne do których zbieramy zawartości typów, które łączymy
            Chunk IDAT = new Chunk();
            IDAT.length = new byte[0];
            IDAT.type = new byte[0];
            IDAT.crc = new byte[0];
            IDAT.data = new byte[0];
            Chunk tEXt = new Chunk();
            tEXt.length = new byte[0];
            tEXt.type = new byte[0];
            tEXt.crc = new byte[0];
            tEXt.data = new byte[0];
            Chunk zTXt = new Chunk();
            zTXt.length = new byte[0];
            zTXt.type = new byte[0];
            zTXt.crc = new byte[0];
            zTXt.data = new byte[0];
            int indIDAT = -1;
            int indTEXT = -1;
            int indZTXT = -1;
            int textExist = 0;
            int ztextExist = 0;

            // Pętla główna
            for (int i = 0; i < wczytany.zawartosc.Count; i++)
            {
                string sygn = (BitConverter.ToString(wczytany.zawartosc[i].type));
                if (sygn == Constans.IDAT)
                {
                    if (indIDAT == -1)
                    {
                        indIDAT = i;
                        
                    }
                        // Kopiowanie typu, bo tak łatwiej to zrobić
                    IDAT.type = wczytany.zawartosc[i].type;
                    // Kopiowanie zawartości
                    int rozmiarPierwotny = IDAT.data.Length;
                    int rozmiarZnalezionego = wczytany.zawartosc[i].data.Length;
                    Array.Resize(ref IDAT.data, rozmiarPierwotny + rozmiarZnalezionego);
                    Array.Copy(wczytany.zawartosc[i].data, 0, IDAT.data, rozmiarPierwotny, rozmiarZnalezionego);
                    wczytany.zawartosc.Remove(wczytany.zawartosc[i]);
                    i--;

                }

                if (sygn == Constans.tEXt)
                {
                    if (indTEXT == -1) indTEXT = i;
                    // Kopiowanie typu, bo tak łatwiej to zrobić
                    tEXt.type = wczytany.zawartosc[i].type;
                    // Kopiowanie zawartości
                    int rozmiarPierwotny = tEXt.data.Length;
                    int rozmiarZnalezionego = wczytany.zawartosc[i].data.Length;
                    Array.Resize(ref tEXt.data, rozmiarPierwotny + rozmiarZnalezionego);
                    Array.Copy(wczytany.zawartosc[i].data, 0, tEXt.data, rozmiarPierwotny, rozmiarZnalezionego);
                    wczytany.zawartosc.Remove(wczytany.zawartosc[i]);
                    i--;
                    textExist = 1;
                }
                
                if (sygn == Constans.zTXt)
                {
                    if (indZTXT == -1) indZTXT = i;
                    // Kopiowanie typu, bo tak łatwiej to zrobić
                    zTXt.type = wczytany.zawartosc[i].type;
                    // Kopiowanie zawartości
                    int rozmiarPierwotny = zTXt.data.Length;
                    int rozmiarZnalezionego = wczytany.zawartosc[i].data.Length;
                    Array.Resize(ref zTXt.data, rozmiarPierwotny + rozmiarZnalezionego);
                    Array.Copy(wczytany.zawartosc[i].data, 0, zTXt.data, rozmiarPierwotny, rozmiarZnalezionego);
                    wczytany.zawartosc.Remove(wczytany.zawartosc[i]);
                    i--;
                    ztextExist = 1;
                }
            }

            // Tutaj uzupełniamy dane chunków
            IDAT.length = new byte[4];
            IDAT.crc = new byte[4];
            byte[] temp = BitConverter.GetBytes(IDAT.data.Length);            
            if (BitConverter.IsLittleEndian) Array.Reverse(temp);
            Array.Resize(ref temp, 4);
            Array.Copy(temp, 0, IDAT.length, 0, 4);
            byte[] crc_temp = new byte[IDAT.data.Length + IDAT.type.Length];
            Array.Copy(IDAT.type, 0, crc_temp, 0, 4);
            Array.Copy(IDAT.data, 0, crc_temp, 4, IDAT.data.Length);
            Crc32 crc = new Crc32();
            byte[] Crc = crc.ComputeChecksumBytes(crc_temp);
            Array.Copy(Crc, 0, IDAT.crc, 0, 4);

            

            wczytany.zawartosc.Insert(indIDAT, IDAT);
            
            if (textExist==1)
            {
                tEXt.crc = new byte[4];
                tEXt.length = new byte[4];
                byte[] temp1 = BitConverter.GetBytes(tEXt.data.Length);
                if (BitConverter.IsLittleEndian) Array.Reverse(temp1);
                Array.Resize(ref temp1, 4);
                Array.Copy(temp1, 0, tEXt.length, 0, 4);
                byte[] crc_temp1 = new byte[tEXt.data.Length + tEXt.type.Length];
                Array.Copy(tEXt.type, 0, crc_temp1, 0, 4);
                Array.Copy(tEXt.data, 0, crc_temp1, 4, tEXt.data.Length);
                Crc32 crc1 = new Crc32();
                byte[] Crc1 = crc1.ComputeChecksumBytes(crc_temp1);
                Array.Copy(Crc1, 0, tEXt.crc, 0, 4);
                wczytany.zawartosc.Insert(indTEXT, tEXt);
            }
            
            if (ztextExist == 1)
            {
                zTXt.crc = new byte[4];
                zTXt.length = new byte[4];
                byte[] temp1 = BitConverter.GetBytes(zTXt.data.Length);
                if (BitConverter.IsLittleEndian) Array.Reverse(temp1);
                Array.Resize(ref temp1, 4);
                Array.Copy(temp1, 0, zTXt.length, 0, 4);
                byte[] crc_temp1 = new byte[zTXt.data.Length + zTXt.type.Length];
                Array.Copy(zTXt.type, 0, crc_temp1, 0, 4);
                Array.Copy(zTXt.data, 0, crc_temp1, 4, zTXt.data.Length);
                Crc32 crc1 = new Crc32();
                byte[] Crc1 = crc1.ComputeChecksumBytes(crc_temp1);
                Array.Copy(Crc1, 0, zTXt.crc, 0, 4);
                wczytany.zawartosc.Insert(indZTXT, zTXt);
            }


            return wczytany;
        }


        static PNG Loader(byte[] plik)
        {

            PNG wynik = new PNG();
            wynik.zawartosc = new List<Chunk>();

            // Skopiuj naglowek
            wynik.headline = new byte[8];
            Array.Copy(plik, 0, wynik.headline, 0, 8);

            // Sprawdzanie nagłówka
            if (BitConverter.ToString(wynik.headline) != "89-50-4E-47-0D-0A-1A-0A")
            {
                Console.WriteLine("To nie jest plik .png głupcze!");
                Console.WriteLine("Myślałeś że możesz mnie oszukać???");
            }
            else
            {

                /* UWAGA! Wielkość liter w stringach ma znaczenie!! 
                Muszą być duże. I tak, pomiędzy znakami MUSI znajdować się -
                https://www.rapidtables.com/convert/number/ascii-to-hex.html */

                // Lista typów metadanych
                List<string> baza_typów = new List<string>();
                baza_typów.Add(Constans.IHDR); // IHDR
                baza_typów.Add(Constans.PLTE); // PLTE
                baza_typów.Add(Constans.IDAT); // IDAT
                baza_typów.Add(Constans.IEND); // IEND
                baza_typów.Add(Constans.eXIf); // eXIf
                baza_typów.Add(Constans.pHYs); // pHYs
                baza_typów.Add(Constans.sBIT); // sBit
                baza_typów.Add(Constans.tEXt); // tEXt
                baza_typów.Add(Constans.cHRM); // cHRM
                baza_typów.Add(Constans.bKGD); // bkGD
                baza_typów.Add(Constans.gAMA); // gAMA
                baza_typów.Add(Constans.iCCP); // iCCP
                baza_typów.Add(Constans.iTXt); // iTXt
                baza_typów.Add(Constans.sRGB); // sRGB
                baza_typów.Add(Constans.tRNS); // tRNS
                baza_typów.Add(Constans.tIME); // tIME
                baza_typów.Add(Constans.zTXt); // zTXt

                // Parser metadanych
                for (int i = 8; i < plik.Length - 3; i++)
                {
                    byte[] temp = new byte[4];
                    Array.Copy(plik, i, temp, 0, 4);
                    string sygn = (BitConverter.ToString(temp));

                    bool kontrolka = false;
                    for (int j = 0; j < baza_typów.Count; j++)
                    {
                        if (sygn == baza_typów[j])
                        {
                            kontrolka = true;
                            //Console.WriteLine(baza_typów[j]);
                        }
                    }
                    if (kontrolka == true)
                    {
                        Chunk nowy = new Chunk();
                        nowy.Wczytaj(i, plik);
                        wynik.zawartosc.Add(nowy);
                        i += (nowy.Przesunięcie() - 1); // Dla optymalizacji
                                                        //Console.WriteLine(nowy.Przesunięcie());
                    }
                }

            }
            Parser(wynik);
            return wynik;

        }

        static byte Paeth(byte A, byte B, byte C)
        {
            byte p = (byte)(A + B - C);
            byte pa = (byte)(Math.Abs(p - A));
            byte pb = (byte)(Math.Abs(p - B));
            byte pc = (byte)(Math.Abs(p - C));
            if ((pa <= pb) || (pa <= pc)) return A;
            else if (pb <= pc) return B;
            else return C;
        }

        public class Crc32
        {
            UInt32[] table;


            public UInt32 ComputeChecksum(byte[] bytes)
            {
                UInt32 crc = 0xffffffff;
                for (int i = 0; i < bytes.Length; i++)
                {
                    byte index = (byte)((crc ^ bytes[i]) & 0xff);
                    crc = (uint)(table[index]^(crc >> 8));
                }
                return crc;
            }

            public byte[] ComputeChecksumBytes(byte[] bytes)
            {
                byte[] tmp = BitConverter.GetBytes(ComputeChecksum(bytes) ^ 0xffffffff);
                Array.Reverse(tmp);
                return tmp;
            }

            public Crc32()
            {
                UInt32 poly = 0xedb88320;
                table = new UInt32[256];
                UInt32 temp = 0;
                for (uint i = 0; i < table.Length; i++)
                {
                    temp = i;
                    for (int j = 0; j <8; j++)
                    {
                        if ((temp & 1) == 1)
                        {
                            temp = (uint)(poly ^(temp >> 1) );
                        }
                        else
                        {
                            temp = temp>> 1;
                        }
                    }
                    table[i] = temp;
                }
            }
        }

        /************************************************************************************************************************************************************/
        /*                                                      CZĘŚĆ DOTYCZĄCA MENU I JEGO OPCJI                                                                  */
        /************************************************************************************************************************************************************/

        /************************************************************************************************************************************************************/
        static void MenuMainOptions()
        {
            Console.WriteLine("1 - Display all chunks");
            Console.WriteLine("2 - Display main info about .png file");
            Console.WriteLine("3 - Choose chunk to display info about that");
            Console.WriteLine("4 - Choose chunk you want delete (only ancillary chunk)");
            Console.WriteLine("5 - Change .png filename");
            Console.WriteLine("0 - Exit");
        }

        /************************************************************************************************************************************************************/
        static void MainInfoChunk(Metadane info)
        {
            Console.WriteLine("IHDR: ");
            Console.WriteLine("     Width: " + BitConverter.ToInt32(info.IHDR.Width,0));
            Console.WriteLine("     Height: " + BitConverter.ToInt32(info.IHDR.Height,0));
            Console.WriteLine("     Bit depth: " + info.IHDR.Bit_depth);
            Console.WriteLine("     Color type: " + info.IHDR.Color_type);
            Console.WriteLine("     Compression method: " + info.IHDR.Compression_method);
            Console.WriteLine("     Filter method : " + info.IHDR.Filter_method);
            Console.WriteLine("     Interlace method: " + info.IHDR.Interlace_method);
        }

        /************************************************************************************************************************************************************/
        static void InfoAboutChunk(Metadane info, string chunk)
        {
            // Chunk bKGD
            if(chunk == "bKGD")
            {
                Console.WriteLine("bKGD: ");
                if(info.IHDR.Color_type == 3)
                {
                    Console.WriteLine("     Palette index: " + info.bKGD.PaletteIndex);
                }
                if(info.IHDR.Color_type==0 || info.IHDR.Color_type == 4)
                {
                    Console.WriteLine("     Gray: " + info.bKGD.Gray);
                }
                if(info.IHDR.Color_type==2 || info.IHDR.Color_type == 6)
                {
                    Console.WriteLine("     Red: " + info.bKGD.Red);
                    Console.WriteLine("     Green: " + info.bKGD.Green);
                    Console.WriteLine("     Blue: " + info.bKGD.Blue);
                }
            }

            // Chunk cHRM
            if(chunk == "cHRM")
            {
                Console.WriteLine("cHRM: ");
                Console.WriteLine("     White Point x: " + info.cHRM.WhitePointX);
                Console.WriteLine("     White Point y: " + info.cHRM.WhitePointY);
                Console.WriteLine("     Red x: " + info.cHRM.RedX);
                Console.WriteLine("     Red y: " + info.cHRM.RedY);
                Console.WriteLine("     Green x: " + info.cHRM.GreenX);
                Console.WriteLine("     Green y: " + info.cHRM.GreenY);
                Console.WriteLine("     Blue x: " + info.cHRM.BlueX);
                Console.WriteLine("     Blue y: " + info.cHRM.BlueY);
            }

            // Chunk gAMA
            if (chunk == "gAMA")
            {
                Console.WriteLine("gAMA:");
                Console.WriteLine("     Gamma: " + info.gAMA + " (1/" + info.gAMA + ")");
            }

            //Chunk iCCP
            if (chunk == "iCCP")
            {
                Console.WriteLine("iCCP: ");
                Console.WriteLine("     Profile name: " + info.iCCP.ProfileName);
                Console.WriteLine("     Compression method: " + info.iCCP.CompressionMethod);
            }

            // Chunk iTXT
            if(chunk == "iTXt")
            {
                Console.WriteLine("iTXt:");
                Console.WriteLine("     Keyword: " + info.iTXt.Keyword);
                Console.WriteLine("     Compression flag: " + info.iTXt.CompressionFlag);
                Console.WriteLine("     Compression method: " + info.iTXt.CompressionMethod);
                Console.WriteLine("     Language tag: " + info.iTXt.LanguageTag);
                Console.WriteLine("     Translated keyword: " + info.iTXt.TranslatedKeyword);
                Console.WriteLine("     Text: " + info.iTXt._Text);
            }

            // Chunk pHYs
            if(chunk == "pHYs")
            {
                Console.WriteLine("pHYs: ");
                Console.WriteLine("     Pixels per unit, X axis: " + BitConverter.ToInt32(info.pHYs.Xaxis,0));
                Console.WriteLine("     Pixels per unit, Y axis: " + BitConverter.ToInt32(info.pHYs.Yaxis, 0));
                Console.WriteLine("     Unit specifier: " + info.pHYs.unitSpecifier);
            }

            // Chunk sBIT
            if(chunk == "sBIT")
            {
                Console.WriteLine("sBIT: ");
                if(info.IHDR.Color_type == 0)
                {
                    Console.WriteLine("     Number of bits that were significant in the source data: " + info.sBIT.sGreyscale);
                }
                if(info.IHDR.Color_type == 2)
                {
                    Console.WriteLine("     Number of bits that were significant in the source data for the red, green, and blue channels, respectively: ");
                    Console.WriteLine("         Red: " + info.sBIT.sRed);
                    Console.WriteLine("         Green: " + info.sBIT.sGreen);
                    Console.WriteLine("         Blue: " + info.sBIT.sBlue);
                }
                if(info.IHDR.Color_type == 3)
                {
                    Console.WriteLine("     Number of bits that were significant in the source data for the red, green, and blue components of the palette entries, respectively: ");
                    Console.WriteLine("         Red: " + info.sBIT.sRed);
                    Console.WriteLine("         Green: " + info.sBIT.sGreen);
                    Console.WriteLine("         Blue: " + info.sBIT.sBlue);
                }
                if(info.IHDR.Color_type == 4)
                {
                    Console.WriteLine("     Number of bits that were significant in the source grayscale data and the source alpha data, respectively:");
                    Console.WriteLine("         Grayscale: " + info.sBIT.sGreyscale);
                    Console.WriteLine("         Alpha: " + info.sBIT.sAlpha);
                }
                if(info.IHDR.Color_type == 6)
                {
                    Console.WriteLine("     Number of bits that were significant in the source data for the red, green, blue, and alpha channels, respectively:");
                    Console.WriteLine("         Red: " + info.sBIT.sRed);
                    Console.WriteLine("         Green: " + info.sBIT.sGreen);
                    Console.WriteLine("         Blue: " + info.sBIT.sBlue);
                    Console.WriteLine("         Alpha: " + info.sBIT.sAlpha);
                }
            }
            
            // Chunk sRGB
            if(chunk == "sRGB")
            {
                Console.WriteLine("sRGB:");
                Console.WriteLine("     Rendering intent: " + info.sRGB);
            }

            if(chunk == "tRNS")
            {
                Console.WriteLine("tRNS:");
                if(info.IHDR.Color_type == 3)
                {
                    for(int i=0; i < info.tRNS.Alpha.Length; i++)
                    {
                        Console.WriteLine("     Alpha for palette index " + i + ": " + info.tRNS.Alpha[i]);
                    }
                }
                if(info.IHDR.Color_type == 0)
                {
                    int range = (int)Math.Pow(2, info.IHDR.Bit_depth) - 1;
                    Console.WriteLine("     Gray: " + BitConverter.ToInt32(info.tRNS.Grey, 0) + ", range 0 .. " +range);
                }
                if (info.IHDR.Color_type == 2)
                {
                    int range = (int)Math.Pow(2, info.IHDR.Bit_depth) - 1;
                    Console.WriteLine("     Red: " + BitConverter.ToInt32(info.tRNS.Red, 0) + ", range 0 .. " + range);
                    Console.WriteLine("     Green: " + BitConverter.ToInt32(info.tRNS.Green, 0) + ", range 0 .. " + range);
                    Console.WriteLine("     Blue: " + BitConverter.ToInt32(info.tRNS.Blue, 0) + ", range 0 .. " + range);
                }
            }

            // Chunk tEXt
            if(chunk == "tEXt")
            {
                Console.WriteLine("tEXt");
                Console.WriteLine("     Keyword: " + info.tEXt.Keyword);
                Console.WriteLine("     Text: " + info.tEXt._Text);
            }

            // Chunk tIME
            if(chunk == "tIME")
            {
                Console.WriteLine("tIME:");
                Console.WriteLine("     Year: " + BitConverter.ToInt16(info.tIME.Year,0));
                Console.WriteLine("     Month: " + info.tIME.Month);
                Console.WriteLine("     Day: " + info.tIME.Day);
                Console.WriteLine("     Hour: " + info.tIME.Hour);
                Console.WriteLine("     Minute: " + info.tIME.Minute);
                Console.WriteLine("     Second: " + info.tIME.Second);
            }

            // Chunk zTXt
            if(chunk == "zTXt")
            {
                Console.WriteLine("zTXt:");

                for(int i=0; i<info.zTXt.Count;i++)
                    Console.WriteLine("     Keyword: " + info.zTXt[i].Keyword);
                //Console.WriteLine("     Compression method: " + info.zTXt.CompressionMethod);
                //Console.WriteLine("     Text: " + info.zTXt._Text);

            }
        }

        /************************************************************************************************************************************************************/
        static void Menu()
        {
            Console.Write("Hello! Please enter .png input filename (or path): ");
            string filenameIN = Console.ReadLine();
            // Sprawdzamy, czy podajemy plik .png
            if (!filenameIN.Contains(".png"))
            {
                while (!filenameIN.Contains(".png"))
                {
                    Console.Write("That's not .png file!!!! Please enter .png input filename (or path): ");
                    filenameIN = Console.ReadLine();
                }
            }
            // Sprawdzamy, czy taki plik wgl istnieje
            if (!File.Exists(filenameIN))
            {
                while (!File.Exists(filenameIN))
                {
                    Console.Write(filenameIN + " is not exist! Please enter .png input filename (or path): ");
                    filenameIN = Console.ReadLine();
                    if (!filenameIN.Contains(".png"))
                    {
                        while (!filenameIN.Contains(".png"))
                        {
                            Console.Write("That's not .png file!!!! Please enter .png input filename (or path): ");
                            filenameIN = Console.ReadLine();
                        }
                    }
                }               
            }
            // Jeżeli tak to ładujemy dane o nim
            byte[] plik = File.ReadAllBytes(filenameIN);
            PNG wczytany = Loader(plik);
            wczytany = Joint(wczytany);
            Metadane przeanalizowany = Parser(wczytany);

            Console.Write("Please enter .png output filename (or path): ");
            string filenameOUT = Console.ReadLine();
            if (!filenameOUT.Contains(".png"))
            {
                while (!filenameOUT.Contains(".png"))
                {
                    Console.Write("That's not .png file!!!! Please enter .png input filename (or path): ");
                    filenameOUT = Console.ReadLine();
                }
            }
            // Sprawdzamy, czy taki plik już nie istnieje
            if (File.Exists(filenameOUT))
            {
                while (File.Exists(filenameOUT))
                {
                    Console.WriteLine(filenameOUT + " already exist!");
                    Console.Write("Do you want to overwrite this file? (yes/no)");
                    string dec = Console.ReadLine();
                    if (dec == "yes")
                    {
                        File.Copy(filenameIN, filenameOUT, true);
                        break;
                    }
                    else if (dec == "no")
                    {
                        Console.Write("Please enter .png output filename (or path): ");
                        filenameOUT = Console.ReadLine();
                        if (!filenameOUT.Contains(".png"))
                        {
                            while (!filenameOUT.Contains(".png"))
                            {
                                Console.Write("That's not .png file!!!! Please enter .png input filename (or path): ");
                                filenameOUT = Console.ReadLine();
                            }
                        }

                    }
                }
            }
            else
            {
                File.Copy(filenameIN, filenameOUT,true);
            }


            Console.WriteLine("\n==================MENU==================");
            MenuMainOptions();
            string mainOPT;
            do
            {
                Console.Write("Please enter your selection:");
                mainOPT = Console.ReadLine();
                switch (mainOPT)
                {
                    /******************************************************************************************************************************************/
                    /*                                                       WYŚWIETLANIE CHUNKÓW                                                             */
                    /******************************************************************************************************************************************/
                    case "1":
                        Console.WriteLine("Chunks of this file:");
                        for (int i = 0; i < wczytany.zawartosc.Count; i++)
                        {
                            Console.WriteLine(Encoding.Default.GetString(wczytany.zawartosc[i].type));
                        }
                        break;
                    /******************************************************************************************************************************************/
                    /*                                           WYŚWIETLANIE GŁÓWNYCH INFORMACJI O OBRAZKU                                                   */
                    /******************************************************************************************************************************************/
                    case "2":
                        MainInfoChunk(przeanalizowany);
                        break;
                    /******************************************************************************************************************************************/
                    /*                                           WYŚWIETLANIE INFORMACJI O CHUNKACH                                                           */
                    /******************************************************************************************************************************************/
                    case "3":
                        Console.Write("Please enter chunk name: ");
                        string chunk = Console.ReadLine();
                        InfoAboutChunk(przeanalizowany, chunk);
                        break;
                    /******************************************************************************************************************************************/
                    /*                                                       USUWANIE CHUNKÓW                                                                 */
                    /******************************************************************************************************************************************/
                    case "4":
                        Console.WriteLine("You have chunks:");
                        for (int i = 0; i < wczytany.zawartosc.Count; i++)
                        {
                            Console.WriteLine(Encoding.Default.GetString(wczytany.zawartosc[i].type));
                        }
                        Console.Write("Do you wanna delete one chunk, few chunks or all chunks? Please type (1 - one or few chunks, 2 - all chunks, 0 - back): ");
                        string opt = Console.ReadLine();
                        switch (opt)
                        {
                            /************************************************************************************************************************************/
                            /*                                                       USUWANIE JEDNEGO CHUNKA                                                    */
                            /************************************************************************************************************************************/
                            case "1":
                                Console.Write("Please enter chunk name: ");
                                string type = Console.ReadLine();
                                int len = wczytany.headline.Length;
                                int pos_length = 8;
                                byte[] tmp = new byte[len];
                                string[] chunks = type.Split(new char[] { ' ' });
                                Array.Copy(wczytany.headline, 0, tmp, 0, 8);
                                for (int k = 0; k < wczytany.zawartosc.Count; k++)
                                {
                                    if (!AllChunksInLine(chunks, Encoding.Default.GetString(wczytany.zawartosc[k].type)))
                                    {
                                        len = len + wczytany.zawartosc[k].length.Length + wczytany.zawartosc[k].type.Length + wczytany.zawartosc[k].data.Length + wczytany.zawartosc[k].crc.Length;
                                        Array.Resize(ref tmp, len);
                                        Array.Copy(wczytany.zawartosc[k].length, 0, tmp, 0 + pos_length, wczytany.zawartosc[k].length.Length);
                                        Array.Copy(wczytany.zawartosc[k].type, 0, tmp, 4 + pos_length, wczytany.zawartosc[k].type.Length);
                                        Array.Copy(wczytany.zawartosc[k].data, 0, tmp, 8 + pos_length, wczytany.zawartosc[k].data.Length);
                                        Array.Copy(wczytany.zawartosc[k].crc, 0, tmp, 8 + pos_length + wczytany.zawartosc[k].data.Length, wczytany.zawartosc[k].crc.Length);
                                        pos_length = pos_length + 12 + wczytany.zawartosc[k].data.Length;
                                        File.WriteAllBytes(filenameOUT, tmp);
                                    }                                 
                                    
                                }
                                for(int l = 0; l < wczytany.zawartosc.Count; l++)
                                {
                                    if (AllChunksInLine(chunks, Encoding.Default.GetString(wczytany.zawartosc[l].type)))
                                    {
                                        wczytany.zawartosc.Remove(wczytany.zawartosc[l]);
                                        l--;
                                    }
                                }
                                break;
                            /******************************************************************************************************************************************/
                            /*                                                    USUWANIE WSZYSTKICH CHUNKÓW                                                         */
                            /******************************************************************************************************************************************/
                            case "2":
                                int leng = wczytany.headline.Length;
                                int pos_leng = 8;
                                byte[] temp = new byte[leng];
                                Array.Copy(wczytany.headline, 0, temp, 0, 8);
                                string chunk_tab = "IHDR IDAT PLTE IEND";
                                for (int k = 0; k < wczytany.zawartosc.Count; k++)
                                {
                                    if (chunk_tab.Contains(Encoding.Default.GetString(wczytany.zawartosc[k].type)))
                                    {
                                        leng = leng + wczytany.zawartosc[k].length.Length + wczytany.zawartosc[k].type.Length + wczytany.zawartosc[k].data.Length + wczytany.zawartosc[k].crc.Length;
                                        Array.Resize(ref temp, leng);
                                        Array.Copy(wczytany.zawartosc[k].length, 0, temp, 0 + pos_leng, wczytany.zawartosc[k].length.Length);
                                        Array.Copy(wczytany.zawartosc[k].type, 0, temp, 4 + pos_leng, wczytany.zawartosc[k].type.Length);
                                        Array.Copy(wczytany.zawartosc[k].data, 0, temp, 8 + pos_leng, wczytany.zawartosc[k].data.Length);
                                        Array.Copy(wczytany.zawartosc[k].crc, 0, temp, 8 + pos_leng + wczytany.zawartosc[k].data.Length, wczytany.zawartosc[k].crc.Length);
                                        pos_leng = pos_leng + 12 + wczytany.zawartosc[k].data.Length;

                                    }
                                    File.WriteAllBytes(filenameOUT, temp);
                                }
                                break;
                            case "0":
                                break;
                            default:
                                Console.WriteLine("Invalid selection. Please select 1 or 2");
                                break;
                        }
                        break;

                    /******************************************************************************************************************************************/
                    /*                                            ZMIENIANIE PLIKU NA KTÓRYM CHCEMY DZIAŁAĆ                                                   */
                    /******************************************************************************************************************************************/
                    case "5":
                        Console.Write("Please enter .png input filename (or path): ");
                        filenameIN = Console.ReadLine();
                        //Sprawdzamy, czy podajemy plik.png
                        if (!filenameIN.Contains(".png"))
                        {
                            while (!filenameIN.Contains(".png"))
                            {
                                Console.Write("That's not .png file!!!! Please enter .png input filename (or path): ");
                                filenameIN = Console.ReadLine();
                            }
                        }
                        //Sprawdzamy, czy taki plik wgl istnieje
                        if (!File.Exists(filenameIN))
                        {
                            while (!File.Exists(filenameIN))
                            {
                                Console.Write(filenameIN + " is not exist! Please enter .png input filename (or path): ");
                                filenameIN = Console.ReadLine();
                                if (!filenameIN.Contains(".png"))
                                {
                                    while (!filenameIN.Contains(".png"))
                                    {
                                        Console.Write("That's not .png file!!!! Please enter .png input filename (or path): ");
                                        filenameIN = Console.ReadLine();
                                    }
                                }
                            }
                        }
                        //Jeżeli tak to ładujemy dane o nim
                        plik = File.ReadAllBytes(filenameIN);
                        wczytany = Loader(plik);
                        wczytany = Joint(wczytany);
                        przeanalizowany = Parser(wczytany);

                        Console.Write("Please enter .png output filename (or path): ");
                        filenameOUT = Console.ReadLine();
                        if (!filenameOUT.Contains(".png"))
                        {
                            while (!filenameOUT.Contains(".png"))
                            {
                                Console.Write("That's not .png file!!!! Please enter .png input filename (or path): ");
                                filenameOUT = Console.ReadLine();
                            }
                        }
                        //Sprawdzamy, czy taki plik już nie istnieje
                        if (File.Exists(filenameOUT))
                        {
                            while (File.Exists(filenameOUT))
                            {
                                Console.WriteLine(filenameOUT + " already exist!");
                                Console.Write("Do you wanna overwrite this file? (yes/no)");
                                string dec = Console.ReadLine();
                                if (dec == "yes")
                                {
                                    File.Copy(filenameIN, filenameOUT, true);
                                    break;
                                }
                                else if (dec == "no")
                                {
                                    Console.Write("Please enter .png output filename (or path): ");
                                    filenameOUT = Console.ReadLine();
                                    if (!filenameOUT.Contains(".png"))
                                    {
                                        while (!filenameOUT.Contains(".png"))
                                        {
                                            Console.Write("That's not .png file!!!! Please enter .png input filename (or path): ");
                                            filenameOUT = Console.ReadLine();
                                        }
                                    }

                                }
                            }
                        }
                        else
                        {
                            File.Copy(filenameIN, filenameOUT, true);
                        }
                        break;
                    /******************************************************************************************************************************************/
                    /*                                                      WYJŚCIE Z PROGRAMU                                                                */
                    /******************************************************************************************************************************************/
                    case "0":                       
                        break;
                    default:
                        Console.WriteLine("Invalid selection. Please select 1, 2, 3, 4 or 5");
                        break;
                }
            } while (mainOPT != "0");

            
        }

        static bool AllChunksInLine(string[] line, string type)
        {
            //bool dec = false;
            foreach(string chunk in line)
            {
                if (type == chunk) return true;
            }
            return false;
        }

    }
}
