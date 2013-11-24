Desc("Execute default targets");
Target("default", new[] { "foo3" });

Desc("Foo1");
Target("foo1", () => {
    Console.WriteLine("Executing foo1");
});

Desc("Foo2");
Target("foo2", new[] { "foo1" }, target => {
    Console.WriteLine("Executing {0}", target.Name);
});

Desc("Foo3");
Exec("foo3", new[] { "foo2" }, cmd => {
    cmd.Command = "ping";
});

Exec("foo3", new[] { "foo1" }, cmd => {
    cmd.Parameters = new [] { "localhost" };
});

Target("foo3", target => {
    Console.WriteLine("Executing {0}", target.Name);;
});
