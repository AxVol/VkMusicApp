using System.Security.Cryptography;
using m3uParser;
using m3uParser.Model;

namespace VKMusicApp.Services.M3U8ToMP3
{
    public class M3U8ToMP3
    {
        public async Task<List<byte[]>> Convert(string url)
        {
            string correctUrl = url.Replace("?siren=1", null);

            List<byte[]> decodedTS = await GetTS(correctUrl);

            return decodedTS;
        }

        // Собирает куски TS файлов в массив
        private async Task<List<byte[]>> GetTS(string url)
        {
            // Список с пометками какой TS файл зашифрованн, а какой нет
            List<string> sequence = new List<string>();

            byte[] outputTS = new byte[1];

            Extm3u m3u8 = await M3U.ParseFromUrlAsync(url);

            foreach (string file in m3u8.Warnings)
            {
                if (file.Contains("KEY:METHOD"))
                {
                    sequence.Add(file);
                }
            }

            List<Media> cypherTs = m3u8.Medias.ToList();
            string keyUrl = GetKey(sequence); //url на ключ шифрования

            List<byte[]> decryptTS = await DecryptTS(keyUrl, cypherTs, sequence);

            return decryptTS;
        }

        // Комбинирует массивы байтов в один
        private byte[] Combine(byte[] first, byte[] second)
        {
            byte[] bytes = new byte[first.Length + second.Length];

            Buffer.BlockCopy(first, 0, bytes, 0, first.Length);
            Buffer.BlockCopy(second, 0, bytes, first.Length, second.Length);

            return bytes;
        }

        /// <summary>
        /// Рассшифровывает TS файлы если они оказались таковыми, если нет просто записывает кусок в общий массив
        /// </summary>
        /// <param name="keyUrl">URL к ключу шифрования</param>
        /// <param name="tsUrls"> Массив с TS файлами из которых достается ссылка на них и их порядковый номер для IV</param>
        /// <param name="sequence"> Последовательность по которой определяется зашифрован ли TS файл</param>
        /// <returns> List<byte[]> список, каждый элемент которого это TS файл в битовом виде</returns>
        private async Task<List<byte[]>> DecryptTS(string keyUrl, List<Media> tsUrls, List<string> sequence)
        {
            List<byte[]> outputTS = new List<byte[]>();

            byte[] key;
            int segmentIndex = 0;
            string hostUrl = GetHostUrl(keyUrl);

            using (HttpClient client = new HttpClient())
            {
                key = await client.GetByteArrayAsync(keyUrl);

                foreach (string action in sequence)
                {
                    byte[] ts = await client.GetByteArrayAsync($"{hostUrl}{tsUrls[segmentIndex].MediaFile}");

                    if (action.Contains("KEY:METHOD=AES-128"))
                    {
                        /// <summary>
                        /// IV генерируется по принципу порядкового номера TS файла в последовательности
                        /// плейлиста, в тегах заголовка указывается с какого начинать #EXT-X-MEDIA-SEQUENCE:1 
                        /// или же служит приставкой в ссылке на TS файл "seg-1-a1.ts" в нем "1" и есть
                        /// его порядковый номер
                        /// </summary>
                        int sequenceIndex = int.Parse(tsUrls[segmentIndex].MediaFile.Split('-')[1]);
                        byte[] iv = sequenceIndex.ToBigEndianBytes();
                        // Так как AES-128 требует IV размером 16 байт, а int это только 8,
                        // То приходиться забивать его пустыми байтами
                        iv = new byte[8].Concat(iv).ToArray();

                        Aes aes = Aes.Create();
                        aes.Key = key;
                        aes.IV = iv;
                        aes.Mode = CipherMode.CBC;
                        aes.Padding = PaddingMode.PKCS7;

                        ICryptoTransform cryptoTransform = aes.CreateDecryptor();

                        using (MemoryStream memoryStream = new MemoryStream())
                        {
                            using (CryptoStream cryptoStream = new CryptoStream(memoryStream, cryptoTransform, CryptoStreamMode.Write))
                            {
                                cryptoStream.Write(ts, 0, ts.Length);
                            }

                            byte[] decrytpTS = memoryStream.ToArray();
                            outputTS.Add(decrytpTS);

                            segmentIndex++;
                        }
                    }
                    else
                    {
                        // Во всех файлах ВКонтаке взято за правило что один TS файл шифруется
                        // Два последующих не шифруются из-за чего их можно пропустить записав 
                        // В том виде в котором они отдаются
                        ts = await client.GetByteArrayAsync($"{hostUrl}{tsUrls[segmentIndex].MediaFile}");
                        outputTS.Add(ts);

                        try
                        {
                            // Если не зашифрованный сегмент идет последний он бывает один
                            ts = await client.GetByteArrayAsync($"{hostUrl}{tsUrls[segmentIndex + 1].MediaFile}");
                            outputTS.Add(ts);
                        }
                        catch
                        {

                        }
                        segmentIndex += 2;
                    }
                }
            }
            return outputTS;
        }

        // Получает полную ссылку на ключ для дешифрования
        private string GetKey(List<string> sequence)
        {
            foreach (string url in sequence)
            {
                if (url.Contains("KEY:METHOD=AES-128"))
                {
                    string[] strings = url.Split('"');
                    string keyUrl = strings[^2];

                    return keyUrl;
                }
            }

            throw new Exception("Key not found or is not AES-128");
        }

        private string GetHostUrl(string keyUrl)
        {
            return keyUrl.Replace("key.pub", null);
        }
    }

    public static class IntExtension
    {
        public static byte[] ToBigEndianBytes(this int segmentIndex)
        {
            byte[] bytes = BitConverter.GetBytes(Convert.ToUInt64(segmentIndex));

            if (BitConverter.IsLittleEndian)
                Array.Reverse(bytes);

            return bytes;
        }
    }

}
