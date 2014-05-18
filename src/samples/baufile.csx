// usage:
//   scriptcs -install Bau -pre
//   scriptcs baufile.csx
//
// inspired by on http://jasonseifer.com/2010/04/06/rake-tutorial
Require<Bau>()
    .DependsOn("groom_myself", "walk_dog")
.Task("turn_off_alarm")
    .Do(() => Console.WriteLine("Turned off alarm. Would have liked 5 more minutes, though."))
.Task("make_coffee")
    .DependsOn("turn_off_alarm")
    .Do(() => Console.WriteLine("Made coffee."))
.Task("groom_myself")
    .DependsOn("make_coffee")
    .Do(() =>
    {
        Console.WriteLine("Brushed teeth.");
        Console.WriteLine("Showered.");
        Console.WriteLine("Shaved.");
    })
.Task("walk_dog")
    .DependsOn("make_coffee")
    .Do(() => Console.WriteLine("Walked the dog."))
.Run();
