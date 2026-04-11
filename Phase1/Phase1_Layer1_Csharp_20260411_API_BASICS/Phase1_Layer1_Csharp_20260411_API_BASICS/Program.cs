var builder = WebApplication.CreateBuilder(args);
var app = builder.Build();


//ROOT ENDPOINT
app.MapGet("/", () => "GAM PHASE ONE STARTED GLAUM");

//SECOND ENDPOINT
app.MapGet("/gam", () => "you are now using endpoint gam which is displaying this message that ends upon another and yet the one and the same gam ");

//what is an api
//an api stands for application programming interface, it is a set of rules and protocols that allows different software apps to communicate 
//with one another. 
//my api protocol defies how any software app can interact with my app. 
//.net facilitate the creation of apis by providing a framework and tools that allow developers to build and deploy apis quickly and easily.
//the main .net api related jargons are MapGet, MapPost, MapPut, MapDelete.
//MapGet is used to define an endpoint that responds to HTTP GET requests, it is typically used to retrieve data from the server.
//MapPost is used to define an endpoint that responds to HTTP POST requests, it is typically used to submit data to the server for processing or storage.
//MapPut is used to define an endpoint that responds to HTTP PUT requests, it is typically used to update existing data on the server.
//MapDelete is used to define an endpoint that responds to HTTP DELETE requests, it is typically used to delete data from the server.
//syntax for defining an endpoint using MapGet is as follows:
//app.MapGet("/endpoint", () => "response message");
//if the api is opening a new page, the syntax is as follows:
//app.MapGet("/endpoint", () => Results.Redirect("/newpage"));

app.MapGet("/phase/{day}", (int day)  => $"You are on day {day} of gam phase one" );
//in this code above we used MapGet route /phase/{day} and we used a parameter day which is of type int,
//this means that when we access the endpoint /phase/1 it will return "You are on day 1 of gam phase one"
//and when we access the endpoint /phase/2 it will return "You are on day 2 of gam phase one" and so on.

app.Run();
