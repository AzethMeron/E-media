using System;
using System.Collections.Generic;
using System.IO;
using System.Text;
using System.Linq;
using System.Numerics;
using System.Drawing;
using Accord.Math;
using UltimateOrb;
using System.Security.Cryptography;
using System.ComponentModel.DataAnnotations.Schema;
using Accord.Math.Optimization;
using Accord;
using Ionic.Zlib;



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
        public int hIST { get; set; }
        public int sPLT { get; set; }
        public int eXIf { get; set; }
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
        public const string sPLT = "73-50-4C-54";
        public const string hIST = "68-49-53-54";
        public const string xDxD = "78-44-78-44";
    }
   

    class Program
    {
        
        static void Main(string[] args)
        {
            Menu();
          
        }

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
                  
                    info.IDAT = dane_z_obrazu;
                }
                else if (analizowany_typ == Constans.IEND)
                {
                    return info;
                }
                else if (analizowany_typ == Constans.eXIf)
                {
                    info.eXIf = dane_z_obrazu.Length;
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
                    //string s1 = Encoding.Default.GetString(dane_z_obrazu);
                    char[] characters = dane_z_obrazu.Select(b => (char)b).ToArray();
                    string s1 = new string(characters);
                    foreach (string s in original)
                    {
                        if (s1.Contains(s))
                        {
                            index.Add(s1.IndexOf(s));
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
                        if (pos_text < dane_z_obrazu.Length) Array.Resize(ref text_tab, pos_text);
                        var uncompressed = Ionic.Zlib.ZlibStream.UncompressBuffer(text_tab);
                        string text = Encoding.Default.GetString(uncompressed);
                        Text ztxt = new Text { Keyword = keyword, CompressionMethod = compressionMethod, _Text = text };
                        te.Add(ztxt);
                        info.zTXt = te;
                    }
                    else if (index.Count > 1)
                    {
                        m = 1; int pos = 0;
                        while (pos != dane_z_obrazu.Length)
                        {
                            byte[] key_tab = new byte[78];
                            int pos_key=0;
                            while (dane_z_obrazu[pos] != 0)
                            {
                                key_tab[pos_key] = dane_z_obrazu[pos];
                                pos++;pos_key++;
                            }
                            if (pos_key < 78) Array.Resize(ref key_tab, pos_key);
                            string keyword = Encoding.Default.GetString(key_tab);
                            pos++;
                            byte compressionMethod = dane_z_obrazu[pos];
                            pos++;
                            byte[] text_tab = new byte[dane_z_obrazu.Length];
                            int pos_text = 0;
                            if (m < index.Count)
                            {
                                while (pos <= index[m]-1)
                                {
                                    text_tab[pos_text] = dane_z_obrazu[pos];
                                    pos++; pos_text++;
                                }
                                m++;
                            }
                            else if (m == index.Count)
                            {
                               while (pos != dane_z_obrazu.Length)
                               {
                                    text_tab[pos_text] = dane_z_obrazu[pos];
                                    pos++; pos_text++;
                               }
                            }
                            if (pos_text < dane_z_obrazu.Length) Array.Resize(ref text_tab, pos_text);
                            var uncompressed = Ionic.Zlib.ZlibStream.UncompressBuffer(text_tab);
                            string text = Encoding.Default.GetString(uncompressed);
                            Text ztxt = new Text { Keyword = keyword, CompressionMethod = compressionMethod, _Text = text };
                            te.Add(ztxt);
                            
                            }
                        info.zTXt = te;
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

                if (analizowany_typ == Constans.sPLT)
                {
                    info.sPLT = dane_z_obrazu.Length;
                }

                if (analizowany_typ == Constans.hIST)
                {
                    info.hIST = dane_z_obrazu.Length;
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


        /*********************************************************************************************************************************************/
        /*                                                      FUNKCJA ŁĄCZĄCA CHUNKI                                                               */
        /********************************************************************************************************************************************/        
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
            Chunk iTXt = new Chunk();
            iTXt.length = new byte[0];
            iTXt.type = new byte[0];
            iTXt.crc = new byte[0];
            iTXt.data = new byte[0];
            int indIDAT = -1;
            int indTEXT = -1;
            int indZTXT = -1;
            int indITXT = -1;
            int textExist = 0;
            int ztextExist = 0;
            int itextExist = 0;

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

                if (sygn == Constans.iTXt)
                {
                    if (indITXT == -1) indITXT = i;
                    // Kopiowanie typu, bo tak łatwiej to zrobić
                    iTXt.type = wczytany.zawartosc[i].type;
                    // Kopiowanie zawartości
                    int rozmiarPierwotny = iTXt.data.Length;
                    int rozmiarZnalezionego = wczytany.zawartosc[i].data.Length;
                    Array.Resize(ref iTXt.data, rozmiarPierwotny + rozmiarZnalezionego);
                    Array.Copy(wczytany.zawartosc[i].data, 0, iTXt.data, rozmiarPierwotny, rozmiarZnalezionego);
                    wczytany.zawartosc.Remove(wczytany.zawartosc[i]);
                    i--;
                    itextExist = 1;
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

            if (itextExist == 1)
            {
                iTXt.crc = new byte[4];
                iTXt.length = new byte[4];
                byte[] temp1 = BitConverter.GetBytes(iTXt.data.Length);
                if (BitConverter.IsLittleEndian) Array.Reverse(temp1);
                Array.Resize(ref temp1, 4);
                Array.Copy(temp1, 0, iTXt.length, 0, 4);
                byte[] crc_temp1 = new byte[iTXt.data.Length + iTXt.type.Length];
                Array.Copy(iTXt.type, 0, crc_temp1, 0, 4);
                Array.Copy(iTXt.data, 0, crc_temp1, 4, iTXt.data.Length);
                Crc32 crc1 = new Crc32();
                byte[] Crc1 = crc1.ComputeChecksumBytes(crc_temp1);
                Array.Copy(Crc1, 0, iTXt.crc, 0, 4);
                wczytany.zawartosc.Insert(indITXT, iTXt);
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
                baza_typów.Add(Constans.sPLT); // sPLT
                baza_typów.Add(Constans.xDxD); 
                baza_typów.Add(Constans.hIST); // hIST

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

        public class FFT
        {
            public Bitmap AbsToBitmap(Complex[,] comp, double corrected_width, double corrected_height)
            {
                Bitmap bitmap = new Bitmap((int)corrected_width,(int)corrected_height);
                Console.WriteLine("cor h: " + corrected_height);
                Console.WriteLine("cor w: " + corrected_width);
                for(int i=0; i < corrected_width; i++)
                {
                    for(int k = 0; k < corrected_height; k++)
                    {
                        double temp = Complex.Abs(comp[i,k]);
                        bitmap.SetPixel(k,i , System.Drawing.Color.FromArgb(255, (int)temp, (int)temp, (int)temp));                        
                    }
                }
                return bitmap;
            }
            public Complex[,] Truncate(Bitmap bitmap,double corrected_width, double corrected_height)
            {
                
                Complex[,] tab = new Complex[(int)(corrected_width),(int)(corrected_height)];
                for(int i=0; i< corrected_width; i++)
                { 
                    for(int k=0; k < corrected_height; k++)
                    {
                        tab[i,k] = 0;
                        if (i < bitmap.Width)
                        {
                            if (k < bitmap.Height)
                            {
                                var pixel = bitmap.GetPixel(k,i);
                                tab[i,k] = pixel.R;
                            }
                        }                       
                    }                                  
                }
                return tab;
                
            }
            public Complex[] BitmapToComplex(Bitmap bitmap)
            {
                Complex[] complex = new Complex[bitmap.Height * bitmap.Width];
                int index = 0;
                for(int i = 0; i < bitmap.Height; i++)
                {
                    for(int k = 0; k < bitmap.Width; k++)
                    {
                        var pixel=bitmap.GetPixel(k, i);
                        complex[index] = pixel.R;
                        ++index;
                    }
                }
                return complex;
            }
            public Bitmap BitmapToGrayscale(Bitmap bitmap/*,string filename*/)
            {
                for (int y = 0; y < bitmap.Height; y++)
                {
                    for (int x = 0; x < bitmap.Width; x++)
                    {
                        var pixel = bitmap.GetPixel(x, y);
                        int a = pixel.A;
                        if (pixel.A == 0)
                        {
                            a = 255;
                        }
                        
                        int avg = (pixel.R + pixel.G + pixel.B)/3;
                        bitmap.SetPixel(x, y, System.Drawing.Color.FromArgb(a, avg, avg, avg));
                    }
                }
                //bitmap.Save(filename);
                return bitmap;
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
            Console.WriteLine("6 - FFT");
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
        static void InfoAboutChunk(Metadane info, string chunk, PNG wczytany)
        {
            // Chunk bKGD
            if(chunk == "bKGD" && IsThatChunk(wczytany,"bKGD"))
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
            else if (chunk == "bKGD")
            {
                Console.WriteLine("In your .png file isn't " + chunk + " chunk!");
            }

            // Chunk cHRM
            if(chunk == "cHRM" && IsThatChunk(wczytany,chunk))
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
            else if (chunk == "cHRM")
            {
                Console.WriteLine("In your .png file isn't " + chunk + " chunk!");
            }

            // Chunk gAMA
            if (chunk == "gAMA" && IsThatChunk(wczytany, chunk))
            {
                Console.WriteLine("gAMA:");
                Console.WriteLine("     Gamma: " + info.gAMA + " (1/" + info.gAMA + ")");
            }
            else if (chunk == "gAMA")
            {
                Console.WriteLine("In your .png file isn't " + chunk + " chunk!");
            }

            //Chunk iCCP
            if (chunk == "iCCP" && IsThatChunk(wczytany, chunk))
            {
                Console.WriteLine("iCCP: ");
                Console.WriteLine("     Profile name: " + info.iCCP.ProfileName);
                Console.WriteLine("     Compression method: " + info.iCCP.CompressionMethod);
            }
            else if (chunk == "iCCP")
            {
                Console.WriteLine("In your .png file isn't " + chunk + " chunk!");
            }

            // Chunk iTXT
            if (chunk == "iTXt" && IsThatChunk(wczytany, chunk))
            {
                Console.WriteLine("iTXt:");
                Console.WriteLine("     Keyword: " + info.iTXt.Keyword);
                Console.WriteLine("     Compression flag: " + info.iTXt.CompressionFlag);
                Console.WriteLine("     Compression method: " + info.iTXt.CompressionMethod);
                Console.WriteLine("     Language tag: " + info.iTXt.LanguageTag);
                Console.WriteLine("     Translated keyword: " + info.iTXt.TranslatedKeyword);
                Console.WriteLine("     Text: " + info.iTXt._Text);
            }
            else if (chunk == "iTXt")
            {
                Console.WriteLine("In your .png file isn't " + chunk + " chunk!");
            }

            // Chunk pHYs
            if (chunk == "pHYs" && IsThatChunk(wczytany, chunk))
            {
                Console.WriteLine("pHYs: ");
                Console.WriteLine("     Pixels per unit, X axis: " + BitConverter.ToInt32(info.pHYs.Xaxis,0));
                Console.WriteLine("     Pixels per unit, Y axis: " + BitConverter.ToInt32(info.pHYs.Yaxis, 0));
                Console.WriteLine("     Unit specifier: " + info.pHYs.unitSpecifier);
            }
            else if (chunk == "pHYs")
            {
                Console.WriteLine("In your .png file isn't " + chunk + " chunk!");
            }

            // Chunk sBIT
            if (chunk == "sBIT" && IsThatChunk(wczytany, chunk))
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
            else if (chunk == "sBIT")
            {
                Console.WriteLine("In your .png file isn't " + chunk + " chunk!");
            }

            // Chunk sRGB
            if (chunk == "sRGB" && IsThatChunk(wczytany, chunk))
            {
                Console.WriteLine("sRGB:");
                Console.WriteLine("     Rendering intent: " + info.sRGB);
            }
            else if (chunk == "sRGB")
            {
                Console.WriteLine("In your .png file isn't " + chunk + " chunk!");
            }

            if (chunk == "tRNS" && IsThatChunk(wczytany, chunk))
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
            else if (chunk == "tRNS")
            {
                Console.WriteLine("In your .png file isn't " + chunk + " chunk!");
            }

            // Chunk tEXt
            if (chunk == "tEXt" && IsThatChunk(wczytany, chunk))
            {
                Console.WriteLine("tEXt");
                Console.WriteLine("     Keyword: " + info.tEXt.Keyword);
                Console.WriteLine("     Text: " + info.tEXt._Text);
            }
            else if (chunk == "tEXt")
            {
                Console.WriteLine("In your .png file isn't " + chunk + " chunk!");
            }

            // Chunk tIME
            if (chunk == "tIME" && IsThatChunk(wczytany, chunk))
            {
                Console.WriteLine("tIME:");
                Console.WriteLine("     Year: " + BitConverter.ToInt16(info.tIME.Year,0));
                Console.WriteLine("     Month: " + info.tIME.Month);
                Console.WriteLine("     Day: " + info.tIME.Day);
                Console.WriteLine("     Hour: " + info.tIME.Hour);
                Console.WriteLine("     Minute: " + info.tIME.Minute);
                Console.WriteLine("     Second: " + info.tIME.Second);
            }
            else if (chunk == "tIME")
            {
                Console.WriteLine("In your .png file isn't " + chunk + " chunk!");
            }

            // Chunk zTXt
            if (chunk == "zTXt" && IsThatChunk(wczytany, chunk))
            {
                Console.WriteLine("zTXt:");

                for(int i=0; i < info.zTXt.Count; i++)
                {
                    Console.WriteLine("     " + info.zTXt[i].Keyword+": " +info.zTXt[i]._Text);
                    Console.WriteLine("     Compression method: " + info.zTXt[i].CompressionMethod);
                }
            }
            else if (chunk == "zTXt")
            {
                Console.WriteLine("In your .png file isn't " + chunk + " chunk!");
            }

            // Chunk sPLT
            if (chunk == "sPLT" && IsThatChunk(wczytany, chunk))
            {
                Console.WriteLine("sPLT:");
                Console.WriteLine("     Chunk length: " + info.sPLT);
            }
            else if (chunk == "sPLT")
            {
                Console.WriteLine("In your .png file isn't " + chunk + " chunk!");
            }

            // Chunk hIST
            if (chunk == "hIST" && IsThatChunk(wczytany, chunk))
            {
                Console.WriteLine("hIST:");
                Console.WriteLine("     Chunk length: " + info.hIST);
            }
            else if (chunk == "hIST")
            {
                Console.WriteLine("In your .png file isn't " + chunk + " chunk!");
            }

            // Chunk eXIf
            if (chunk == "eXIf" && IsThatChunk(wczytany, chunk))
            {
                Console.WriteLine("eXIf:");
                Console.WriteLine("     Chunk length: " + info.eXIf);
            }
            else if (chunk == "eXIf")
            {
                Console.WriteLine("In your .png file isn't " + chunk + " chunk!");
            }
        }


        ////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////
        //                                                                                                                                                        //
        //                                                              SZYFROWANIE RSA                                                                           //
        //                                                                                                                                                        //
        ////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////

        public class Key
        {
            public BigInteger left { get; set; }
            public BigInteger right { get; set;  }
            public Key() { }
        }
        // Losowanie nieparzystej liczby 64 bitowej
        static BigInteger RandomBigNumber()
        {
            Random rnd = new Random();
            RNGCryptoServiceProvider rngCsp = new RNGCryptoServiceProvider();
            BigInteger byt = 0;
            do
            {
                BigInteger uint64 = BigInteger.Multiply(UInt64.MaxValue, rnd.Next());
                BigInteger number = BigInteger.Pow(uint64, 3);
                byte[] by = number.ToByteArray();
                rngCsp.GetBytes(by);
                byt = new BigInteger(by);
                if (byt < 0)
                {
                    byt = byt * (-1);
                }
            } while (byt % 2 == 0);
            return byt;
        }
        static BigInteger ExtendedAlgorithmEuklides(BigInteger a, BigInteger b)
        {
            // Współczynniki równań
            BigInteger u = 1;
            BigInteger w = a;
            BigInteger x = 0;
            BigInteger z = b;

            BigInteger q = 0; // całkowity iloraz 
            BigInteger swap = 0; // zmienna pomocnicza do zamiany wartosci zmiennych

            while (w != 0)
            {
                if (w < z)
                {
                    // u <-> x
                    swap = u;
                    u = x;
                    x = swap;

                    // w <-> z
                    swap = w;
                    w = z;
                    z = swap;
                }
                q = w / z;
                u -= q * x;
                w -= q * z;
            }
            if (z == 1)
            {
                if (x < 0)
                {
                    x = x + b;
                }
                return x;
            }
            return 0;
        }
        // Generowanie kluczy RSA
        static List<Key> RSA_key()
        {
            BigInteger p = RandomBigNumber();
            BigInteger q = RandomBigNumber();
            List<Key> keys = new List<Key>();

            do
            {
                while (!IsPrime(p, 10))
                {
                    p = RandomBigNumber();
                }

                while (!IsPrime(q, 10))
                {
                    q = RandomBigNumber();
                }
            } while (p == q);

            BigInteger phi = (p - 1) * (q - 1); // phi - funkcja eulera
            BigInteger n = p * q; // n - moduł

            // Szukanie liczby e, która jest względnie pierwsza
            BigInteger e;
            for (e = 3; BigInteger.GreatestCommonDivisor(e, phi) != 1; e += 2) ;
            BigInteger d;
            d = ExtendedAlgorithmEuklides(e, phi);
            if (d == 0) Console.WriteLine("Insert modulo doesn't exist");

            keys.Add(new Key { left = e, right = n });
            keys.Add(new Key { left = d, right = n });
            return keys;

        }

     
        static byte[] Padding(BigInteger number, int len)
        {
            byte[] temp = number.ToByteArray();
            
            if(temp.Length == len)
            {
                return temp;
            }
            else
            {
                byte[] output = new byte[len];
               // if (BitConverter.IsLittleEndian) Array.Reverse(temp);
                for(int i =0; i< output.Length; i++)
                {
                    output[i] = 0;
                }
                Array.Copy(temp, 0, output, 0, temp.Length);
                return output;
            }
            
        }

        static Chunk newChunk(string name, int len, byte[] data)
        {
            Chunk chunk = new Chunk();
            byte[] _len = BitConverter.GetBytes(len);
            if (BitConverter.IsLittleEndian) Array.Reverse(_len);
            byte[] _name = Encoding.ASCII.GetBytes(name);
            Crc32 Crc = new Crc32();
            byte[] crc = Crc.ComputeChecksumBytes(data);
            chunk.length = _len;
            chunk.type = _name;
            chunk.data = data;
            chunk.crc = crc;
            return chunk;
        }

        // Szyfrowanie danych
        static PNG Encrypt(PNG png, BigInteger e, BigInteger n, BigInteger d)
        {
            Metadane info = new Metadane();
            for (int i = 0; i < png.zawartosc.Count; i++)
            {
                string type = (BitConverter.ToString(png.zawartosc[i].type));
                byte[] data = png.zawartosc[i].data;
                BigInteger temp;
                BigInteger max=0;
                byte[] maxByte;
                int maxLength = 0;
                byte tmp;//= new byte[data.Length];
            
                BigInteger[] bigTab = new BigInteger[data.Length];
                string dekompres;
                if (type == Constans.IDAT)
                {
                    Chunk ob = png.zawartosc[i];
                    Array.Resize(ref ob.data, data.Length);
                    Array.Copy(data, ob.data, data.Length);
                    png.zawartosc[i] = ob;
                    Array.Resize(ref bigTab, data.Length);
                    for (int k = 2; k < png.zawartosc[i].data.Length; k++)
                    {
                        temp = BigInteger.ModPow(png.zawartosc[i].data[k], e, n);
                        bigTab[k] = temp;                        
                        if(max < bigTab[k]) 
                        {
                            max = bigTab[k];
                        }
                    }
                    BigInteger[] b_temp = new BigInteger[bigTab.Length-2];
                    Array.Copy(bigTab, 2, b_temp, 0, b_temp.Length);
                    maxLength = max.ToByteArray().Length;
                    byte[] encrypt = new byte[maxLength * b_temp.Length+4];
                    for(int l = 0; l<b_temp.Length; l++)
                    {
                        Array.Copy(Padding(b_temp[l], maxLength),0, encrypt, 4+l*maxLength, maxLength);
                    }
                    byte[] tmp_maxLen = BitConverter.GetBytes(maxLength);
                    if (BitConverter.IsLittleEndian) Array.Reverse(tmp_maxLen);
                    Array.Copy(tmp_maxLen, 0, encrypt, 0, 4);
                    Chunk ch = newChunk("xDxD", encrypt.Length, encrypt);
                    for(int c = 0; c < png.zawartosc.Count; c++)
                    {
                        string _type = (BitConverter.ToString(png.zawartosc[c].type));
                        if (_type == Constans.IEND)
                        {
                            png.zawartosc.Insert(c, ch);
                            break;
                        }
                    }

                
                    Array.Copy(encrypt, 0, png.zawartosc[i].data, 2, png.zawartosc[i].data.Length - 2);
                   
                }

            }
                
            
            return png;
        }

        // Odszyfrowywanie
        static PNG Decryption(PNG png, BigInteger d, BigInteger n)
        {
            int index_idat = 0;
            int index_xdxd = 0;

            for(int i = 0; i < png.zawartosc.Count; i++)
            {
                string type = (BitConverter.ToString(png.zawartosc[i].type));
                if(type == Constans.IDAT)
                {
                    index_idat = i;
                }
                else if(type == Constans.xDxD)
                {
                    index_xdxd = i;
                }
            }

            Chunk chunk = png.zawartosc[index_xdxd];

            byte[] encrypt = png.zawartosc[index_xdxd].data;
            byte[] len = new byte[4];
            Array.Copy(encrypt, 0, len, 0, 4);
            if (BitConverter.IsLittleEndian) Array.Reverse(len);
            int _len = BitConverter.ToInt32(len, 0);

            BigInteger[] bigTab = new BigInteger[(encrypt.Length-4)/_len];
            for(int i=0; i < bigTab.Length; i++)
            {
                byte[] tmp = new byte[_len];
                Array.Copy(encrypt, 4 + i * _len, tmp, 0, _len);
                BigInteger big = new BigInteger(tmp);
                bigTab[i] = big;
            }

            byte[] _bigTab = new byte[bigTab.Length + 2];
            _bigTab[0] = png.zawartosc[index_idat].data[0];
            _bigTab[1] = png.zawartosc[index_idat].data[1];

            for(int i=0; i < bigTab.Length; i++)
            {
                _bigTab[i+2] = (byte)BigInteger.ModPow(bigTab[i], d, n);
            }

            Array.Copy(_bigTab, 0, png.zawartosc[index_idat].data, 0, _bigTab.Length);
            png.zawartosc.Remove(chunk);
            return png;
        }

        public static byte[] RSAEncryptPart(byte[] DataToEncrypt, RSAParameters RSAKeyInfo, bool DoOAEPPadding)
        {
            try
            {
                byte[] encryptedData;
                //Create a new instance of RSACryptoServiceProvider.
                using (RSACryptoServiceProvider RSA = new RSACryptoServiceProvider())
                {

                    //Import the RSA Key information. This only needs
                    //toinclude the public key information.
                    RSA.ImportParameters(RSAKeyInfo);

                    //Encrypt the passed byte array and specify OAEP padding.  
                    //OAEP padding is only available on Microsoft Windows XP or
                    //later.  
                    encryptedData = RSA.Encrypt(DataToEncrypt, DoOAEPPadding);
                }
                return encryptedData;
            }
            //Catch and display a CryptographicException  
            //to the console.
            catch (CryptographicException e)
            {
                Console.WriteLine(e.Message);

                return null;
            }
        }

        static PNG RSAEncryptAll(PNG png, RSAParameters RSAKeyInfo)
        {
            for (int i = 0; i < png.zawartosc.Count; i++)
            {
                string type = (BitConverter.ToString(png.zawartosc[i].type));
                if (type == Constans.IDAT)
                {
                    byte[] data = png.zawartosc[i].data;
                    byte[] encrypted = new byte[data.Length];
                    int before = 0;
                    int sum = 0;
                    for (int j = 2; j < data.Length / 117; j++)
                    {
                        byte[] data_pom = new byte[117];
                        byte[] encrypted_pom;
                        for (int k = 0; k < 117; k++)
                        {
                            data_pom[k] = data[j + k];
                        }
                        encrypted_pom = RSAEncryptPart(data_pom, RSAKeyInfo, false);
                        sum = before + encrypted_pom.Length;
                        Console.WriteLine("enctypted pom: " + encrypted_pom.Length);
                        Array.Resize(ref encrypted, sum);
                        Array.Copy(encrypted_pom, 0, encrypted, before, encrypted_pom.Length);
                        before = sum;

                    }
                    Console.WriteLine(encrypted.Length);
                    Console.WriteLine(data.Length);
                    Array.Copy(encrypted, 0, png.zawartosc[i].data, 2, png.zawartosc[i].data.Length - 2);
                }
            }
            return png;
        }

        public static byte[] RSADecryptPart(byte[] DataToDecrypt, RSAParameters RSAKeyInfo, bool DoOAEPPadding)
        {
            try
            {
                byte[] decryptedData;
                //Create a new instance of RSACryptoServiceProvider.
                using (RSACryptoServiceProvider RSA = new RSACryptoServiceProvider())
                {
                    //Import the RSA Key information. This needs
                    //to include the private key information.
                    RSA.ImportParameters(RSAKeyInfo);

                    //Decrypt the passed byte array and specify OAEP padding.  
                    //OAEP padding is only available on Microsoft Windows XP or
                    //later.  
                    decryptedData = RSA.Decrypt(DataToDecrypt, DoOAEPPadding);
                }
                return decryptedData;
            }
            //Catch and display a CryptographicException  
            //to the console.
            catch (CryptographicException e)
            {
                Console.WriteLine(e.ToString());

                return null;
            }
        }

        static PNG RSADecryptAll(PNG png, RSAParameters RSAKeyInfo)
        {
            for (int i = 0; i < png.zawartosc.Count; i++)
            {
                string type = (BitConverter.ToString(png.zawartosc[i].type));
                if (type == Constans.IDAT)
                {
                    byte[] data = png.zawartosc[i].data;
                    byte[] decrypted = new byte[data.Length];
                    int before = 0;
                    int sum = 0;
                    for (int j = 2; j < data.Length / 117-2; j++)
                    {
                        byte[] data_pom = new byte[117];
                        
                        for (int k = 0; k < 117; k++)
                        {
                            data_pom[k] = data[j + k];
                        }
                        byte[]  decrypted_pom = RSADecryptPart(data_pom, RSAKeyInfo, false);
                        sum = before + decrypted_pom.Length;
                        Console.WriteLine("enctypted pom: " + decrypted_pom.Length);
                        Array.Resize(ref decrypted, sum);
                        Array.Copy(decrypted_pom, 0, decrypted, before, decrypted_pom.Length);
                        before = sum;

                    }
                    Console.WriteLine(decrypted.Length);
                    Console.WriteLine(data.Length);
                    Array.Copy(decrypted, 0, png.zawartosc[i].data, 2, png.zawartosc[i].data.Length - 2);
                }
            }
            return png;
        }

        static bool IsPrime(BigInteger prime, int n) //prime - liczba, ktora sprawdzamy, czy jest pierwsza; n - ilosc powtorzen testu millera-rabina
        {
            if (prime == 2 || prime == 3)
                return true;
            if (prime < 2 || prime % 2 == 0)
                return false;

            BigInteger d = prime - 1; // mnoznik potegi 2 w dzielniku prime-1
            int s = 0; // wykladnik potegi 2 w dzielniku prime-1

            while (d % 2 == 0)
            {
                s++;
                d /= 2;
            }

            // There is no built-in method for generating random BigInteger values.
            // Instead, random BigIntegers are constructed from randomly generated
            // byte arrays of the same length as the source.
            RandomNumberGenerator rng = RandomNumberGenerator.Create();
            byte[] bytes = new byte[prime.ToByteArray().LongLength];
            BigInteger a;

            for (int i = 0; i < n; i++)
            {
                do
                {                  
                    rng.GetBytes(bytes);
                    a = new BigInteger(bytes);
                }while (a < 2 || a >= prime - 2);

                BigInteger x = BigInteger.ModPow(a, d, prime);
                if (x == 1 || x == prime - 1) continue;

                for (int j = 1; j < s; j++)
                {
                    x = BigInteger.ModPow(x, 2, prime);
                    if (x == 1)
                        return false;
                    if (x == prime - 1)
                        break;
                }

                if (x != prime - 1)
                    return false;
            }

            return true;
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
            Bitmap bitmap = new Bitmap(filenameIN);
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
            List<Key> keys = new List<Key>();
            keys = RSA_key();
            BigInteger e = keys[0].left;
            BigInteger d = keys[1].left;
            BigInteger n = keys[1].right;

            RSACryptoServiceProvider RSA = new RSACryptoServiceProvider();
            RSAParameters RSAKey = new RSAParameters();
            RSAKey.Exponent = e.ToByteArray();
            RSAKey.D = d.ToByteArray();
            RSAKey.Modulus = n.ToByteArray();

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
                        InfoAboutChunk(przeanalizowany, chunk,wczytany);
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
                                string[] chunki = chunk_tab.Split(new char[] { ' ' });
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
                                for (int l = 0; l < wczytany.zawartosc.Count; l++)
                                {
                                    if (!AllChunksInLine(chunki, Encoding.Default.GetString(wczytany.zawartosc[l].type)))
                                    {
                                        wczytany.zawartosc.Remove(wczytany.zawartosc[l]);
                                        l--;
                                    }
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
                    case "6":
                        FFT fft = new FFT();
                        Bitmap b;
                        Complex[,] comp;
                        b=fft.BitmapToGrayscale(bitmap);
                        
                        double corrected_width = Math.Pow(2, Math.Ceiling(Math.Log(bitmap.Width) / Math.Log(2)));
                        double corrected_height = Math.Pow(2, Math.Ceiling(Math.Log(bitmap.Height) / Math.Log(2)));
                        comp = fft.Truncate(b,corrected_width,corrected_height);
                        //MathNet.Numerics.IntegralTransforms.Fourier.Forward2D(comp, (int)corrected_height, (int)corrected_width, MathNet.Numerics.IntegralTransforms.FourierOptions.NoScaling);
                        Accord.Math.FourierTransform.FFT2(comp, Accord.Math.FourierTransform.Direction.Forward);
                        
                      
                        b =fft.AbsToBitmap(comp, corrected_width, corrected_height);
                        b.Save(filenameOUT);
                        break;
                    case "7":
                        //List<Key> key = new List<Key>();
                        //key = RSA_key();
                        Console.WriteLine("Public key:  (" + keys[0].left + ", " + keys[0].right + ")");
                        Console.WriteLine("Private key: (" + keys[1].left + ", " + keys[1].right + ")");


                        //PNG png = Encrypt(wczytany, e, n,d);
                        //PNG png = RSAEncrypt(wczytany, RSA.ExportParameters(false), true);
                        int ind = 0;
                        for(int i = 0; i < wczytany.zawartosc.Count; i++)
                        {
                            string type = (BitConverter.ToString(wczytany.zawartosc[i].type));
                            if(type == Constans.IDAT)
                            {
                                ind = i;
                            }
                        }
                       
                        PNG png = RSAEncryptAll(wczytany, RSA.ExportParameters(false));
                        //PNG png = wczytany;
                        int _len = png.headline.Length;
                        int _pos_length = 8;
                        byte[] _tmp = new byte[_len];
                        
                        Array.Copy(png.headline, 0, _tmp, 0, 8);
                        for (int k = 0; k < png.zawartosc.Count; k++)
                        {
                                _len = _len + png.zawartosc[k].length.Length + png.zawartosc[k].type.Length + png.zawartosc[k].data.Length + png.zawartosc[k].crc.Length;
                                Array.Resize(ref _tmp, _len);
                                Array.Copy(png.zawartosc[k].length, 0, _tmp, 0 + _pos_length, png.zawartosc[k].length.Length);
                                Array.Copy(png.zawartosc[k].type, 0, _tmp, 4 + _pos_length, png.zawartosc[k].type.Length);
                                Array.Copy(png.zawartosc[k].data, 0, _tmp, 8 + _pos_length, png.zawartosc[k].data.Length);
                                Array.Copy(png.zawartosc[k].crc, 0, _tmp, 8 + _pos_length + png.zawartosc[k].data.Length, png.zawartosc[k].crc.Length);
                                _pos_length = _pos_length + 12 + png.zawartosc[k].data.Length;
                                File.WriteAllBytes(filenameOUT, _tmp);
                            

                        }
                        
                       
                        
                        break;
                    case "8":
                        byte[] _plik = File.ReadAllBytes(filenameOUT);
                        PNG _wczytany = Loader(_plik);
                        _wczytany = Joint(_wczytany);
                        //PNG encrypt = Decryption(_wczytany, d, n);

                        PNG encrypt = RSADecryptAll(_wczytany, RSA.ExportParameters(false));
                        int _len_ = encrypt.headline.Length;
                        int _pos_length_ = 8;
                        byte[] _tmp_ = new byte[_len_];

                        Array.Copy(encrypt.headline, 0, _tmp_, 0, 8);
                        for (int k = 0; k < encrypt.zawartosc.Count; k++)
                        {

                            _len_ = _len_ + encrypt.zawartosc[k].length.Length + encrypt.zawartosc[k].type.Length + encrypt.zawartosc[k].data.Length + encrypt.zawartosc[k].crc.Length;
                            Array.Resize(ref _tmp_, _len_);
                            Array.Copy(encrypt.zawartosc[k].length, 0, _tmp_, 0 + _pos_length_, encrypt.zawartosc[k].length.Length);
                            Array.Copy(encrypt.zawartosc[k].type, 0, _tmp_, 4 + _pos_length_, encrypt.zawartosc[k].type.Length);
                            Array.Copy(encrypt.zawartosc[k].data, 0, _tmp_, 8 + _pos_length_, encrypt.zawartosc[k].data.Length);
                            Array.Copy(encrypt.zawartosc[k].crc, 0, _tmp_, 8 + _pos_length_ + encrypt.zawartosc[k].data.Length, encrypt.zawartosc[k].crc.Length);
                            _pos_length_ = _pos_length_ + 12 + encrypt.zawartosc[k].data.Length;
                            File.WriteAllBytes("o.png", _tmp_);


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

        static bool IsThatChunk(PNG wczytany, string chunk)
        {
            for(int i = 0; i < wczytany.zawartosc.Count; i++)
            {
                if (Encoding.Default.GetString(wczytany.zawartosc[i].type) == chunk) return true;
            }
            return false;            
        }

    }
}
