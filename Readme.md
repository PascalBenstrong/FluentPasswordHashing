# FluentPasswordHashing

A fluent password hashing library using argon2

## Target FrameWork

.NET Standard 2.0

## How To Use

### Generating a password hash

```C#
var generator = FluentHashing.Create()
            .WithSaltLength(16)
            .WithDegreeOfParallelism(1)
            .WithIterations(4)
            .WithMemorySize(16)
            .WithHashLength(16)
            .WithAlgorithm(PasswordHashAlgorithm.ARGON2ID)
            .Generator();

var password = "password";

// example hash for password
// $argon2id$m=16,t=4,p=1$aM8qmYw1xfzvHEYTIg0w1g==$5Hg1fRhHv1uOnOMusawtqg==
var hash = await generator.Generate(password);

```

### verifying a password with the hash

```C#
var generator = FluentHashing.Create().Generator();

var isMatch = await generator.VerifyPassword(hash, password);
```

### Author

[Pascal Nsunba](https://github.com/PascalBenstrong/)

### License

[MIT LICENSE](https://github.com/PascalBenstrong/FluentPasswordHashing/blob/main/LICENSE)
