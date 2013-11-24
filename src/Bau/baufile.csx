Desc("Execute default tasks");
Task("default", new[] { "foo3" });

Desc("Foo1");
Task("foo1", () => {
    Console.WriteLine("Executing foo1");
});

Desc("Foo2");
Task("foo2", new[] { "foo1" }, task => {
    Console.WriteLine("Executing {0}", task.Name);
});

Desc("Foo3");
Exec("foo3", new[] { "foo2" }, cmd => {
    cmd.Command = "ping";
    cmd.Parameters = new [] { "localhost" };
});
