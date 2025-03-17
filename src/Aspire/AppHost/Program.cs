var builder = DistributedApplication.CreateBuilder(args);
var api = builder.AddProject<Projects.API>("api");
builder.Build().Run();
