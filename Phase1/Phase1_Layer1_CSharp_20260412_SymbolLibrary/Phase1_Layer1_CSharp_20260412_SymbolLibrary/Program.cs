var builder = WebApplication.CreateBuilder(args);
var app = builder.Build();

//app.MapGet("/", () => "Hello World!");

//symbol library data 
var symbols = new[]
{
    new
    {
        Name = "Crow" ,
        Meaning1 ="A call to notice the small details in life",
        Meaning2 ="An incoming change that may be unsettling but ultimately leads to growth",
        Meaning3 ="A reminder to stay grounded and connected to the present moment"
    },
    new
    {
        Name = "Owl" ,
        Meaning1 ="A symbol of wisdom and knowledge",
        Meaning2 ="A call to trust your intuition and inner guidance",
        Meaning3 ="A reminder to stay alert and aware of your surroundings"
    },
    new
    {
        Name = "Snake" ,
        Meaning1 ="A symbol of transformation and rebirth",
        Meaning2 ="A call to shed old habits and beliefs that no longer serve you",
        Meaning3 ="A reminder to embrace change and personal growth"
    },
    new
    {
        Name = "Butterfly" ,
        Meaning1 ="A symbol of transformation and beauty",
        Meaning2 ="A call to embrace change and let go of the past",
        Meaning3 ="A reminder to find beauty in the present moment and in the process of growth"
    },
    new
    {
        Name = "Tree" ,
        Meaning1 ="A symbol of strength and stability",
        Meaning2 ="A call to stay grounded and connected to your roots",
        Meaning3 ="A reminder to nurture your growth and development over time"
    },
    new
    {
        Name = "Moon" ,
        Meaning1 ="A symbol of femininity and intuition",
        Meaning2 ="A call to connect with your inner self and emotions",
        Meaning3 ="A reminder to embrace the cycles of life and find beauty in the darkness"
    },
    new
    {
        Name = "Sun" ,
        Meaning1 ="A symbol of vitality and energy",
        Meaning2 ="A call to embrace your inner light and radiate positivity",
        Meaning3 ="A reminder to find joy in the present moment and in the simple pleasures of life"
    },
    new
    {
        Name = "Star" ,
        Meaning1 ="A symbol of hope and guidance",
        Meaning2 ="A call to follow your dreams and aspirations",
        Meaning3 ="A reminder to stay optimistic and keep reaching for the stars"
    },
    new
    {
        Name = "Water" ,
        Meaning1 ="A symbol of emotions and intuition",
        Meaning2 ="A call to connect with your feelings and inner wisdom",
        Meaning3 ="A reminder to go with the flow and adapt to the changes in life"
    },
    new
    {
        Name = "Fire" ,
        Meaning1 ="A symbol of passion and transformation",
        Meaning2 ="A call to ignite your inner fire and pursue your passions",
        Meaning3 ="A reminder to embrace change and let go of what no longer serves you"
    },
   new
    {
        Name = "Mountain" ,
        Meaning1 ="A symbol of strength and resilience",
        Meaning2 ="A call to overcome obstacles and challenges in life",
        Meaning3 ="A reminder to stay grounded and connected to your inner strength"
    },
    new
    {
        Name = "River" ,
        Meaning1 ="A symbol of flow and adaptability",
        Meaning2 ="A call to go with the flow and adapt to the changes in life",
        Meaning3 ="A reminder to find beauty in the present moment and in the journey of life"
    },
    new
    {
        Name = "Wind" ,
        Meaning1 ="A symbol of freedom and change",
        Meaning2 ="A call to embrace change and let go of what no longer serves you",
        Meaning3 ="A reminder to stay open to new possibilities and opportunities in life"
    },
    new
    {
        Name = "Flower" ,
        Meaning1 ="A symbol of beauty and growth",
        Meaning2 ="A call to nurture your growth and development over time",
        Meaning3 ="A reminder to find beauty in the present moment and in the process of growth"
    },
    new
    {
        Name = "Heart" ,
        Meaning1 ="A symbol of love and compassion",
        Meaning2 ="A call to connect with your emotions and express love to others",
        Meaning3 ="A reminder to find joy in the present moment and in the connections you have with others"
    }

};

// End point


app.MapGet("/symbols", () => symbols);

app.MapGet("/symbols/{name}", (string name) =>
{
    //but ineed only one random meaning for each symbol displayed when the user search for a symbol by name
    //the each json object has 3 meanings but i want to display only one random meaning for each symbol when the user search for a symbol by name
    //for  that case i will use the random class to generate a random number between 1 and 3 and then use that number to select one of the meanings for each symbol
    //create a random number generator
    var random = new Random();

    var result = symbols.FirstOrDefault(s => s.Name.ToLower() == name.ToLower());
    if (result is not null)
    {
        int randomMeaning = random.Next(1, 4); // Generate a random number between 1 and 3
        string selectedMeaning = randomMeaning switch
        {
            1 => result.Meaning1,
            2 => result.Meaning2,
            3 => result.Meaning3,
            _ => result.Meaning1
        };
        return Results.Ok(new { result.Name, Meaning = selectedMeaning });
    }
    return Results.NotFound();
    

}
);

app.Run();
