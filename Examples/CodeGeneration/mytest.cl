
class Main inherits IO {

  t:Object <- new Object;

  f(x:Object) : IO {
    out_string(x.type_name().concat("\n"))
  };

  main(): IO {{
    --f(t);
    f(t);
    t <- 2;
    f(t);
    t <- "Hi";
    f(t);
    let x:Int in f(x);
    let t:Int in f(t);
    f(t);


    --f(2);
    --f(2);
    --f(5);
    --f(true);
    --f(7);
    --f("hi");
    --f(false);
    --f(new Main);
    --f(false);
    --f(7);
    --f("by");
  }};
};