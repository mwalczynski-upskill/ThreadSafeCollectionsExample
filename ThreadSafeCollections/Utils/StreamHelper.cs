namespace ThreadSafeCollections.Utils
{
    using System.IO;
    using System.Linq;

    public class StreamHelper
    {
        public string[] GetDataFromFile(string path)
        {
            using (var fileStream = new FileStream(path, FileMode.Open))
            {
                using (var streamReader = new StreamReader(fileStream))
                {
                    var inputData = streamReader.ReadToEnd();
                    var separatedData = inputData.Split('\n');
                    var data = separatedData.Select(sd => sd.Trim()).ToArray();
                    return data;
                }
            }
        }

    }

}
