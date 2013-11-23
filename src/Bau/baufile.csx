Desc("Execute default tasks");
Task("default", new[] { "bar" });

Desc("Foo");
Task("foo", () => {
    Console.WriteLine("Executing foo");
});

Desc("Bar");
Task("bar", new[] { "foo" }, () => {
    Console.WriteLine("Executing bar");
});
