# S3ConfigExample

Demonstrates using `IConfiguration.GetAWSOptions<TConfig>(string sectionName)` to create an `AmazonS3Config` instance to pass to `IServiceCollection.AddAWSService<TService>(AWSOptions options)`.

Using the `GetAWSOptions` with the generic type parameter binds `appsettings.json`-sourced configuration to a configuration options class provided by the AWS SDK. Each client specific options class like `AmazonS3Config` inherits `ClientConfig`. Specifics for what `ClientConfig` needs can be found at https://docs.aws.amazon.com/sdk-for-net/v3/developer-guide/net-dg-config-netcore.html#net-core-appsettings-values. Despite what the documentation says, the property names in the JSON configured do **not** need to match the casing. The `GetAWSOptions` implementation uses `StringComparison.OrdinalIgnoreCase` when comparing property names of the options classes to the JSON property names.

This is the reason why there will be duplication of the client config in the `appsettings.json`:

```json
{
    "AWS": {
        "Region": "us-east-1",
        "S3": {
            "Region": "us-east-1",
            "ForcePathStyle": false
        }
    }
}
```

Notes:

1. The `AWS` section is what the no-argument, non-generic `GetAWSOptions` uses as its configuration section.
2. The `AWS:S3` section is only for convention. The `S3` section does not have to be a child property of `AWS`. This is done for stylistic and organizational reasons.
3. Either the `Region` or `ServiceURL` property is required for any `ClientConfig`-based AWS option instances (which is all of them.)
4. The root `AWS` section as well as the `S3` must have all of the `ClientConfig` properties needed to configure a client. When using specific options classes with a client registration, it does not assume the values from the default AWS options.
