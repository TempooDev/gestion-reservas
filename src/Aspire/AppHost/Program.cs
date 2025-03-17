var builder = DistributedApplication.CreateBuilder(args);
var api = builder.AddProject<Projects.Booking_Api>("api");
builder.Build().Run();
