# S5 &mdash; Stupid Simple Somewhat Secure Storage

## Example

```csharp
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
```

## Generating keys
```csharp
var key = Store.Key.Generate(); // Generate random key.

var someBytes = new byte[] { 0x00, 0x01, 0x02, ... }
var key = Store.Key.DeriveFrom(someBytes); // Derive key from bytes.

var somePassword = "somewhatSecretPassword";
var key = Store.Key.DeriveFrom(somePassword); // Derive key from string.
```
