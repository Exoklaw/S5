using System.Text;

namespace S5.Sandbox;

internal static class Program
{
    private static void Main(string[] args)
    {
        const string fileName = "somewhatSecretFile";
        const string secretData = "Some small secret";
        const string password = "hardcodedPasswordForIllustrativePurposes";

        var secretDataBytes = Encoding.Unicode.GetBytes(secretData);
        var key = Store.Key.DeriveFrom(password);
        Store.Save(fileName, secretDataBytes, key);

        // [...]

        var key2 = Store.Key.DeriveFrom(password);
        if (!Store.TryLoad(fileName, key2, out var secretDataBytes2))
        {
            throw new OwOException("Somefwing bwoke!");
        }
        
        var secretData2 = Encoding.Unicode.GetString(secretDataBytes2);
        Console.WriteLine(secretData2); // "Some small secret"
        
        
        /*const string fileName = "somewhatSecretFile";
        const string keyFileName = "somewhatSecretFileKey";
        const string password = "someOtherSomewhatSecretPassword";
        const string message = "Somewhat Secret Message";

        var messageBytes = Encoding.Unicode.GetBytes(message);

        /*Console.WriteLine("Deriving key!");
        var key = Store.Key.DeriveFrom(password);*/
        // Store.Save(fileName, messageBytes, key);

        /*Store.Key key;
        if (File.Exists(keyFileName))
        {
            Console.WriteLine("Found key!");
            var keyBytes = File.ReadAllBytes(keyFileName);
            key = new Store.Key(keyBytes);
        }
        else
        {
            Console.WriteLine("Generating new key and saving file!");
            key = Store.Key.Generate();
            File.WriteAllBytes(keyFileName, key.Value);
            Store.Save(fileName, messageBytes, key);
        }

        if (!Store.TryLoad(fileName, key, out var decryptedMessageBytes))
        {
            Console.Error.WriteLine("Oh no! Something went wrong!");
            return;
        }

        var decryptedMessage = Encoding.Unicode.GetString(decryptedMessageBytes);
        Console.Out.WriteLine(decryptedMessage);*/
    }

    public class OwOException(string message) : Exception(message);
}
