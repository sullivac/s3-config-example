using Amazon.S3;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

// Set up default AWS options that will be used with any other Amazon client
var awsOptions = builder.Configuration.GetAWSOptions();
builder.Services.AddDefaultAWSOptions(awsOptions);

// Set up S3 client with specific AmazonS3Config loaded from appsettings.json.
// Does not use the DefaultAWSOptions.
//
// The AmazonS3Config must have all of the ClientConfig properties set in the
// appsettings regardless if they are configured in the DefaultAWSOptions.
var s3Options = builder.Configuration.GetAWSOptions<AmazonS3Config>("AWS:S3");
builder.Services.AddAWSService<IAmazonS3>(s3Options);

var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

// Get the S3 client and log the configuration
var s3Client = app.Services.GetRequiredService<IAmazonS3>();

var s3Config = s3Client.Config as AmazonS3Config;

app.Logger.LogInformation(
    "s3Config: {s3Config}",
    new
    {
        s3Config?.ServiceURL,
        s3Config?.RegionEndpoint,
        s3Config?.ForcePathStyle
    });

app.Run();
